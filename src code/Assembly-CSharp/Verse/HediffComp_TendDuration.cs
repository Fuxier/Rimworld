using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200032A RID: 810
	[StaticConstructorOnStartup]
	public class HediffComp_TendDuration : HediffComp_SeverityModifierBase
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x00080F73 File Offset: 0x0007F173
		public HediffCompProperties_TendDuration TProps
		{
			get
			{
				return (HediffCompProperties_TendDuration)this.props;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x00080F80 File Offset: 0x0007F180
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.TProps.disappearsAtTotalTendQuality >= 0 && this.totalTendQuality >= (float)this.TProps.disappearsAtTotalTendQuality);
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x00080FB3 File Offset: 0x0007F1B3
		public bool IsTended
		{
			get
			{
				return Current.ProgramState == ProgramState.Playing && this.tendTicksLeft > 0;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x00080FC8 File Offset: 0x0007F1C8
		public bool AllowTend
		{
			get
			{
				if (this.TProps.TendIsPermanent)
				{
					return !this.IsTended;
				}
				return this.TProps.TendTicksOverlap > this.tendTicksLeft;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x00080FF4 File Offset: 0x0007F1F4
		public override string CompTipStringExtra
		{
			get
			{
				if (this.parent.IsPermanent())
				{
					return null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (!this.IsTended)
				{
					if (!base.Pawn.Dead && this.parent.TendableNow(false))
					{
						stringBuilder.AppendLine("NeedsTendingNow".Translate());
					}
				}
				else
				{
					if (this.TProps.showTendQuality)
					{
						string text;
						if (this.parent.Part != null && this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
						{
							text = this.TProps.labelSolidTendedWell;
						}
						else if (this.parent.Part != null && this.parent.Part.depth == BodyPartDepth.Inside)
						{
							text = this.TProps.labelTendedWellInner;
						}
						else
						{
							text = this.TProps.labelTendedWell;
						}
						if (text != null)
						{
							stringBuilder.AppendLine(text.CapitalizeFirst() + " (" + "quality".Translate() + " " + this.tendQuality.ToStringPercent("F0") + ")");
						}
						else
						{
							stringBuilder.AppendLine(string.Format("{0}: {1}", "TendQuality".Translate(), this.tendQuality.ToStringPercent()));
						}
						if (this.TProps.disappearsAtTotalTendQuality >= 0)
						{
							stringBuilder.AppendLine("DisappearsAtTotalTendQuality".Translate() + ": " + this.totalTendQuality.ToStringPercent() + " / " + ((float)this.TProps.disappearsAtTotalTendQuality).ToStringPercent());
						}
					}
					if (!base.Pawn.Dead && !this.TProps.TendIsPermanent && this.parent.TendableNow(true))
					{
						int num = this.tendTicksLeft - this.TProps.TendTicksOverlap;
						if (num < 0)
						{
							stringBuilder.AppendLine("CanTendNow".Translate());
						}
						else if ("NextTendIn".CanTranslate())
						{
							stringBuilder.AppendLine("NextTendIn".Translate(num.ToStringTicksToPeriod(true, false, true, true, false)));
						}
						else
						{
							stringBuilder.AppendLine("NextTreatmentIn".Translate(num.ToStringTicksToPeriod(true, false, true, true, false)));
						}
						stringBuilder.AppendLine("TreatmentExpiresIn".Translate(this.tendTicksLeft.ToStringTicksToPeriod(true, false, true, true, false)));
					}
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x000812CC File Offset: 0x0007F4CC
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.parent is Hediff_Injury)
				{
					if (this.IsTended && !this.parent.IsPermanent())
					{
						Color color = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_Injury, color);
					}
				}
				else if (!(this.parent is Hediff_MissingPart) && !this.parent.FullyImmune())
				{
					if (this.IsTended)
					{
						Color color2 = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_General, color2);
					}
					return HediffComp_TendDuration.TendedIcon_Need_General;
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0008137E File Offset: 0x0007F57E
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.tendTicksLeft, "tendTicksLeft", -1, false);
			Scribe_Values.Look<float>(ref this.tendQuality, "tendQuality", 0f, false);
			Scribe_Values.Look<float>(ref this.totalTendQuality, "totalTendQuality", 0f, false);
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x000813BE File Offset: 0x0007F5BE
		public override float SeverityChangePerDay()
		{
			if (this.IsTended)
			{
				return this.TProps.severityPerDayTended * this.tendQuality;
			}
			return 0f;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x000813E0 File Offset: 0x0007F5E0
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.tendTicksLeft > 0 && !this.TProps.TendIsPermanent)
			{
				this.tendTicksLeft--;
			}
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00081410 File Offset: 0x0007F610
		public override void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
			this.tendQuality = Mathf.Clamp(quality + Rand.Range(-0.25f, 0.25f), 0f, maxQuality);
			this.totalTendQuality += this.tendQuality;
			if (this.TProps.TendIsPermanent)
			{
				this.tendTicksLeft = 1;
			}
			else
			{
				this.tendTicksLeft = Mathf.Max(0, this.tendTicksLeft) + this.TProps.TendTicksFull;
			}
			if (batchPosition == 0 && base.Pawn.Spawned)
			{
				string text = "TextMote_Tended".Translate(this.parent.Label).CapitalizeFirst() + "\n" + "Quality".Translate() + " " + this.tendQuality.ToStringPercent();
				MoteMaker.ThrowText(base.Pawn.DrawPos, base.Pawn.Map, text, Color.white, 3.65f);
			}
			base.Pawn.health.Notify_HediffChanged(this.parent);
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00081534 File Offset: 0x0007F734
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsTended)
			{
				stringBuilder.AppendLine("tendQuality: " + this.tendQuality.ToStringPercent());
				if (!this.TProps.TendIsPermanent)
				{
					stringBuilder.AppendLine("tendTicksLeft: " + this.tendTicksLeft);
				}
			}
			else
			{
				stringBuilder.AppendLine("untended");
			}
			stringBuilder.AppendLine("severity/day: " + this.SeverityChangePerDay().ToString());
			if (this.TProps.disappearsAtTotalTendQuality >= 0)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"totalTendQuality: ",
					this.totalTendQuality.ToString("F2"),
					" / ",
					this.TProps.disappearsAtTotalTendQuality
				}));
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x0400115B RID: 4443
		public int tendTicksLeft = -1;

		// Token: 0x0400115C RID: 4444
		public float tendQuality;

		// Token: 0x0400115D RID: 4445
		private float totalTendQuality;

		// Token: 0x0400115E RID: 4446
		public const float TendQualityRandomVariance = 0.25f;

		// Token: 0x0400115F RID: 4447
		private static readonly Color UntendedColor = new ColorInt(116, 101, 72).ToColor;

		// Token: 0x04001160 RID: 4448
		private static readonly Texture2D TendedIcon_Need_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedNeed", true);

		// Token: 0x04001161 RID: 4449
		private static readonly Texture2D TendedIcon_Well_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedWell", true);

		// Token: 0x04001162 RID: 4450
		private static readonly Texture2D TendedIcon_Well_Injury = ContentFinder<Texture2D>.Get("UI/Icons/Medical/BandageWell", true);
	}
}

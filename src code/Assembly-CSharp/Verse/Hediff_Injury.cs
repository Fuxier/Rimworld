using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000345 RID: 837
	public class Hediff_Injury : HediffWithComps
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001651 RID: 5713 RVA: 0x00083760 File Offset: 0x00081960
		public override int UIGroupKey
		{
			get
			{
				int num = base.UIGroupKey;
				if (this.IsTended())
				{
					num = Gen.HashCombineInt(num, 152235495);
				}
				return num;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x0008378C File Offset: 0x0008198C
		public override string LabelBase
		{
			get
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					if (base.Part.def.delicate && !hediffComp_GetsPermanent.Props.instantlyPermanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.instantlyPermanentLabel;
					}
					if (!hediffComp_GetsPermanent.Props.permanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.permanentLabel;
					}
				}
				return base.LabelBase;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001653 RID: 5715 RVA: 0x00083800 File Offset: 0x00081A00
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.sourceHediffDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.sourceHediffDef.label);
				}
				else if (this.source != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.source.label);
					if (this.sourceBodyPartGroup != null)
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(this.sourceBodyPartGroup.LabelShort);
					}
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent && hediffComp_GetsPermanent.PainCategory != PainCategory.Painless)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(("PainCategory_" + hediffComp_GetsPermanent.PainCategory.ToString()).Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001654 RID: 5716 RVA: 0x00083907 File Offset: 0x00081B07
		public override Color LabelColor
		{
			get
			{
				if (this.IsPermanent())
				{
					return Hediff_Injury.PermanentInjuryColor;
				}
				return Color.white;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001655 RID: 5717 RVA: 0x0008391C File Offset: 0x00081B1C
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToString("F1");
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001656 RID: 5718 RVA: 0x0008394B File Offset: 0x00081B4B
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (this.IsPermanent() || !this.Visible)
				{
					return 0f;
				}
				return this.Severity / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001657 RID: 5719 RVA: 0x0008397C File Offset: 0x00081B7C
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part) || this.causesNoPain)
				{
					return 0f;
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				float num;
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					num = this.Severity * this.def.injuryProps.averagePainPerSeverityPermanent * hediffComp_GetsPermanent.PainFactor;
				}
				else
				{
					num = this.Severity * this.def.injuryProps.painPerSeverity;
				}
				return num / this.pawn.HealthScale;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x00083A1C File Offset: 0x00081C1C
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.BleedingStoppedDueToAge)
				{
					return 0f;
				}
				if (base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) || this.IsTended() || this.IsPermanent())
				{
					return 0f;
				}
				if (this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part))
				{
					return 0f;
				}
				float num = this.Severity * this.def.injuryProps.bleedRate;
				if (base.Part != null)
				{
					num *= base.Part.def.bleedRate;
				}
				return num;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001659 RID: 5721 RVA: 0x00083AE4 File Offset: 0x00081CE4
		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600165A RID: 5722 RVA: 0x00083B31 File Offset: 0x00081D31
		private bool BleedingStoppedDueToAge
		{
			get
			{
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00083B44 File Offset: 0x00081D44
		public override void Tick()
		{
			bool bleedingStoppedDueToAge = this.BleedingStoppedDueToAge;
			base.Tick();
			bool bleedingStoppedDueToAge2 = this.BleedingStoppedDueToAge;
			if (bleedingStoppedDueToAge != bleedingStoppedDueToAge2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00083B78 File Offset: 0x00081D78
		public override void Heal(float amount)
		{
			this.Severity -= amount;
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostInjuryHeal(amount);
				}
			}
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00083BD4 File Offset: 0x00081DD4
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00083C44 File Offset: 0x00081E44
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part != null && base.Part.coverageAbs <= 0f && (dinfo == null || dinfo.Value.Def != DamageDefOf.SurgicalCut))
			{
				Log.Error(string.Concat(new object[]
				{
					"Added injury to ",
					base.Part.def,
					" but it should be impossible to hit it. pawn=",
					this.pawn.ToStringSafe<Pawn>(),
					" dinfo=",
					dinfo.ToStringSafe<DamageInfo?>()
				}));
			}
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00083CDE File Offset: 0x00081EDE
		public override void PostRemoved()
		{
			base.PostRemoved();
			this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00083D17 File Offset: 0x00081F17
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Injury has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x0400118E RID: 4494
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);
	}
}

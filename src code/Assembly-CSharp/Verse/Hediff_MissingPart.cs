using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200034A RID: 842
	public class Hediff_MissingPart : HediffWithComps
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x0600168D RID: 5773 RVA: 0x0008463C File Offset: 0x0008283C
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (!this.IsFreshNonSolidExtremity)
				{
					return 0f;
				}
				if (base.Part.def.tags.NullOrEmpty<BodyPartTagDef>() && base.Part.parts.NullOrEmpty<BodyPartRecord>() && !base.Bleeding)
				{
					return 0f;
				}
				return (float)base.Part.def.hitPoints / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x0000249D File Offset: 0x0000069D
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x0600168F RID: 5775 RVA: 0x000846B4 File Offset: 0x000828B4
		public override string LabelBase
		{
			get
			{
				if (this.lastInjury != null && this.lastInjury.injuryProps.useRemovedLabel)
				{
					return "RemovedBodyPart".Translate();
				}
				if (this.lastInjury == null || base.Part.depth == BodyPartDepth.Inside)
				{
					bool solid = base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs);
					return HealthUtility.GetGeneralDestroyedPartLabel(base.Part, this.IsFreshNonSolidExtremity, solid);
				}
				if (base.Part.def.socketed && !this.lastInjury.injuryProps.destroyedOutLabel.NullOrEmpty())
				{
					return this.lastInjury.injuryProps.destroyedOutLabel;
				}
				return this.lastInjury.injuryProps.destroyedLabel;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x0008478C File Offset: 0x0008298C
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.IsFreshNonSolidExtremity)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append("FreshMissingBodyPart".Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001691 RID: 5777 RVA: 0x000847E4 File Offset: 0x000829E4
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.bleedRate * base.Part.def.bleedRate;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001692 RID: 5778 RVA: 0x0008484C File Offset: 0x00082A4C
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.causesNoPain || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.painPerSeverity / this.pawn.HealthScale;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001693 RID: 5779 RVA: 0x000848B8 File Offset: 0x00082AB8
		private bool ParentIsMissing
		{
			get
			{
				for (int i = 0; i < this.pawn.health.hediffSet.hediffs.Count; i++)
				{
					Hediff_MissingPart hediff_MissingPart = this.pawn.health.hediffSet.hediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part == base.Part.parent)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001694 RID: 5780 RVA: 0x00084924 File Offset: 0x00082B24
		// (set) Token: 0x06001695 RID: 5781 RVA: 0x0008493C File Offset: 0x00082B3C
		public bool IsFresh
		{
			get
			{
				return this.isFreshInt && !this.TicksAfterNoLongerFreshPassed;
			}
			set
			{
				if (this.isFreshInt == value)
				{
					return;
				}
				this.isFreshInt = value;
				this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
				PortraitsCache.SetDirty(this.pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001696 RID: 5782 RVA: 0x0008498C File Offset: 0x00082B8C
		public bool IsFreshNonSolidExtremity
		{
			get
			{
				return Current.ProgramState != ProgramState.Entry && this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) && !this.ParentIsMissing;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x000849F0 File Offset: 0x00082BF0
		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return this.ageTicks >= 90000;
			}
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x00084A02 File Offset: 0x00082C02
		public override bool TendableNow(bool ignoreTimer = false)
		{
			return this.IsFreshNonSolidExtremity;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00084A0C File Offset: 0x00082C0C
		public override void Tick()
		{
			bool ticksAfterNoLongerFreshPassed = this.TicksAfterNoLongerFreshPassed;
			base.Tick();
			bool ticksAfterNoLongerFreshPassed2 = this.TicksAfterNoLongerFreshPassed;
			if (ticksAfterNoLongerFreshPassed != ticksAfterNoLongerFreshPassed2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00084A40 File Offset: 0x00082C40
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.Tended(quality, maxQuality, batchPosition);
			this.IsFresh = false;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00084A64 File Offset: 0x00082C64
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (Current.ProgramState != ProgramState.Playing || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				this.IsFresh = false;
			}
			this.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(this.def, this.pawn, null);
				hediff_MissingPart.IsFresh = false;
				hediff_MissingPart.lastInjury = this.lastInjury;
				hediff_MissingPart.Part = base.Part.parts[i];
				this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
			}
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00084B2C File Offset: 0x00082D2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.lastInjury, "lastInjury");
			Scribe_Values.Look<bool>(ref this.isFreshInt, "isFresh", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_MissingPart has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x04001197 RID: 4503
		public HediffDef lastInjury;

		// Token: 0x04001198 RID: 4504
		private bool isFreshInt;
	}
}

using System;

namespace Verse
{
	// Token: 0x020002E6 RID: 742
	public class HediffComp_GetsPermanent : HediffComp
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x0007E38B File Offset: 0x0007C58B
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060014D9 RID: 5337 RVA: 0x0007E398 File Offset: 0x0007C598
		// (set) Token: 0x060014DA RID: 5338 RVA: 0x0007E3A0 File Offset: 0x0007C5A0
		public bool IsPermanent
		{
			get
			{
				return this.isPermanentInt;
			}
			set
			{
				if (value == this.isPermanentInt)
				{
					return;
				}
				this.isPermanentInt = value;
				if (this.isPermanentInt)
				{
					this.permanentDamageThreshold = 9999f;
					this.SetPainCategory(HealthTuning.InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted cat) => cat.weight).category);
				}
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x0007E405 File Offset: 0x0007C605
		public PainCategory PainCategory
		{
			get
			{
				return this.painCategory;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x0007E40D File Offset: 0x0007C60D
		public float PainFactor
		{
			get
			{
				return (float)this.painCategory;
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0007E416 File Offset: 0x0007C616
		public void SetPainCategory(PainCategory category)
		{
			this.painCategory = category;
			if (base.Pawn != null)
			{
				base.Pawn.health.Notify_HediffChanged(this.parent);
			}
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0007E440 File Offset: 0x0007C640
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.isPermanentInt, "isPermanent", false, false);
			Scribe_Values.Look<float>(ref this.permanentDamageThreshold, "permanentDamageThreshold", 9999f, false);
			Scribe_Values.Look<PainCategory>(ref this.painCategory, "painCategory", PainCategory.Painless, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0007E490 File Offset: 0x0007C690
		public void PreFinalizeInjury()
		{
			if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
			{
				return;
			}
			if (ModsConfig.BiotechActive && this.parent.pawn.genes != null)
			{
				if (this.parent.pawn.genes.GenesListForReading.Any((Gene x) => x.def.preventPermanentWounds))
				{
					return;
				}
			}
			float num = 0.02f * this.parent.Part.def.permanentInjuryChanceFactor * this.Props.becomePermanentChanceFactor;
			if (!this.parent.Part.def.delicate)
			{
				num *= HealthTuning.BecomePermanentChanceFactorBySeverityCurve.Evaluate(this.parent.Severity);
			}
			if (Rand.Chance(num))
			{
				if (this.parent.Part.def.delicate)
				{
					this.IsPermanent = true;
					return;
				}
				this.permanentDamageThreshold = Rand.Range(1f, this.parent.Severity / 2f);
			}
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0007E5B4 File Offset: 0x0007C7B4
		public override void CompPostInjuryHeal(float amount)
		{
			if (this.permanentDamageThreshold >= 9999f || this.IsPermanent)
			{
				return;
			}
			if (this.parent.Severity <= this.permanentDamageThreshold && this.parent.Severity >= this.permanentDamageThreshold - amount)
			{
				this.parent.Severity = this.permanentDamageThreshold;
				this.IsPermanent = true;
				base.Pawn.health.Notify_HediffChanged(this.parent);
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0007E630 File Offset: 0x0007C830
		public override string CompDebugString()
		{
			return string.Concat(new object[]
			{
				"isPermanent: ",
				this.isPermanentInt.ToString(),
				"\npermanentDamageThreshold: ",
				this.permanentDamageThreshold,
				"\npainCategory: ",
				this.painCategory
			});
		}

		// Token: 0x040010E3 RID: 4323
		public float permanentDamageThreshold = 9999f;

		// Token: 0x040010E4 RID: 4324
		public bool isPermanentInt;

		// Token: 0x040010E5 RID: 4325
		private PainCategory painCategory;

		// Token: 0x040010E6 RID: 4326
		private const float NonActivePermanentDamageThresholdValue = 9999f;
	}
}

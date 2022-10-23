using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020000B7 RID: 183
	public class DamageDef : Def
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x000202C8 File Offset: 0x0001E4C8
		public DamageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (DamageWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000202FC File Offset: 0x0001E4FC
		public bool ExternalViolenceFor(Thing thing)
		{
			if (this.externalViolence)
			{
				return true;
			}
			if (this.externalViolenceForMechanoids)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (thing is Building_Turret)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000326 RID: 806
		public Type workerClass = typeof(DamageWorker);

		// Token: 0x04000327 RID: 807
		private bool externalViolence;

		// Token: 0x04000328 RID: 808
		private bool externalViolenceForMechanoids;

		// Token: 0x04000329 RID: 809
		public bool hasForcefulImpact = true;

		// Token: 0x0400032A RID: 810
		public bool harmsHealth = true;

		// Token: 0x0400032B RID: 811
		public bool makesBlood = true;

		// Token: 0x0400032C RID: 812
		public bool canInterruptJobs = true;

		// Token: 0x0400032D RID: 813
		public bool isRanged;

		// Token: 0x0400032E RID: 814
		public bool makesAnimalsFlee;

		// Token: 0x0400032F RID: 815
		public bool execution;

		// Token: 0x04000330 RID: 816
		public RulePackDef combatLogRules;

		// Token: 0x04000331 RID: 817
		public float buildingDamageFactor = 1f;

		// Token: 0x04000332 RID: 818
		public float buildingDamageFactorPassable = 1f;

		// Token: 0x04000333 RID: 819
		public float buildingDamageFactorImpassable = 1f;

		// Token: 0x04000334 RID: 820
		public float plantDamageFactor = 1f;

		// Token: 0x04000335 RID: 821
		public float corpseDamageFactor = 1f;

		// Token: 0x04000336 RID: 822
		public bool canUseDeflectMetalEffect = true;

		// Token: 0x04000337 RID: 823
		public ImpactSoundTypeDef impactSoundType;

		// Token: 0x04000338 RID: 824
		[MustTranslate]
		public string deathMessage = "{0} has been killed.";

		// Token: 0x04000339 RID: 825
		public EffecterDef damageEffecter;

		// Token: 0x0400033A RID: 826
		public int defaultDamage = -1;

		// Token: 0x0400033B RID: 827
		public float defaultArmorPenetration = -1f;

		// Token: 0x0400033C RID: 828
		public float defaultStoppingPower;

		// Token: 0x0400033D RID: 829
		public List<DamageDefAdditionalHediff> additionalHediffs;

		// Token: 0x0400033E RID: 830
		public bool applyAdditionalHediffsIfHuntingForFood = true;

		// Token: 0x0400033F RID: 831
		public DamageArmorCategoryDef armorCategory;

		// Token: 0x04000340 RID: 832
		public int minDamageToFragment = 99999;

		// Token: 0x04000341 RID: 833
		public FloatRange overkillPctToDestroyPart = new FloatRange(0f, 0.7f);

		// Token: 0x04000342 RID: 834
		public bool consideredHelpful;

		// Token: 0x04000343 RID: 835
		public bool harmAllLayersUntilOutside;

		// Token: 0x04000344 RID: 836
		public HediffDef hediff;

		// Token: 0x04000345 RID: 837
		public HediffDef hediffSkin;

		// Token: 0x04000346 RID: 838
		public HediffDef hediffSolid;

		// Token: 0x04000347 RID: 839
		public bool isExplosive;

		// Token: 0x04000348 RID: 840
		public float explosionSnowMeltAmount = 1f;

		// Token: 0x04000349 RID: 841
		public bool explosionAffectOutsidePartsOnly = true;

		// Token: 0x0400034A RID: 842
		public ThingDef explosionCellMote;

		// Token: 0x0400034B RID: 843
		public FleckDef explosionCellFleck;

		// Token: 0x0400034C RID: 844
		public Color explosionColorCenter = Color.white;

		// Token: 0x0400034D RID: 845
		public Color explosionColorEdge = Color.white;

		// Token: 0x0400034E RID: 846
		public EffecterDef explosionInteriorEffecter;

		// Token: 0x0400034F RID: 847
		public ThingDef explosionInteriorMote;

		// Token: 0x04000350 RID: 848
		public FleckDef explosionInteriorFleck;

		// Token: 0x04000351 RID: 849
		public ThingDef explosionCenterMote;

		// Token: 0x04000352 RID: 850
		public FleckDef explosionCenterFleck;

		// Token: 0x04000353 RID: 851
		public EffecterDef explosionCenterEffecter;

		// Token: 0x04000354 RID: 852
		public EffecterDef explosionCellEffecter;

		// Token: 0x04000355 RID: 853
		public float explosionCellEffecterChance;

		// Token: 0x04000356 RID: 854
		public float explosionCellEffecterMaxRadius;

		// Token: 0x04000357 RID: 855
		public float explosionHeatEnergyPerCell;

		// Token: 0x04000358 RID: 856
		public float expolosionPropagationSpeed = 1f;

		// Token: 0x04000359 RID: 857
		public SoundDef soundExplosion;

		// Token: 0x0400035A RID: 858
		public float explosionInteriorCellCountMultiplier = 1f;

		// Token: 0x0400035B RID: 859
		public float explosionInteriorCellDistanceMultiplier = 0.7f;

		// Token: 0x0400035C RID: 860
		public float stabChanceOfForcedInternal;

		// Token: 0x0400035D RID: 861
		public float stabPierceBonus;

		// Token: 0x0400035E RID: 862
		public SimpleCurve cutExtraTargetsCurve;

		// Token: 0x0400035F RID: 863
		public float cutCleaveBonus;

		// Token: 0x04000360 RID: 864
		public float bluntInnerHitChance;

		// Token: 0x04000361 RID: 865
		public FloatRange bluntInnerHitDamageFractionToConvert;

		// Token: 0x04000362 RID: 866
		public FloatRange bluntInnerHitDamageFractionToAdd;

		// Token: 0x04000363 RID: 867
		public float bluntStunDuration = 1f;

		// Token: 0x04000364 RID: 868
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToHeadCurve;

		// Token: 0x04000365 RID: 869
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToBodyCurve;

		// Token: 0x04000366 RID: 870
		public float scratchSplitPercentage = 0.5f;

		// Token: 0x04000367 RID: 871
		public float biteDamageMultiplier = 1f;

		// Token: 0x04000368 RID: 872
		[Unsaved(false)]
		private DamageWorker workerInt;
	}
}

using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000FE RID: 254
	public class HediffStage
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x000253E9 File Offset: 0x000235E9
		public bool AffectsMemory
		{
			get
			{
				return this.forgetMemoryThoughtMtbDays > 0f || this.pctConditionalThoughtsNullified > 0f;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x00025407 File Offset: 0x00023607
		public bool AffectsSocialInteractions
		{
			get
			{
				return this.opinionOfOthersFactor != 1f;
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00025419 File Offset: 0x00023619
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00025427 File Offset: 0x00023627
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return HediffStatsUtility.SpecialDisplayStats(this, null);
		}

		// Token: 0x0400060C RID: 1548
		public float minSeverity;

		// Token: 0x0400060D RID: 1549
		[MustTranslate]
		public string label;

		// Token: 0x0400060E RID: 1550
		[MustTranslate]
		public string overrideLabel;

		// Token: 0x0400060F RID: 1551
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04000610 RID: 1552
		public bool becomeVisible = true;

		// Token: 0x04000611 RID: 1553
		public bool lifeThreatening;

		// Token: 0x04000612 RID: 1554
		public TaleDef tale;

		// Token: 0x04000613 RID: 1555
		public float vomitMtbDays = -1f;

		// Token: 0x04000614 RID: 1556
		public float deathMtbDays = -1f;

		// Token: 0x04000615 RID: 1557
		public bool mtbDeathDestroysBrain;

		// Token: 0x04000616 RID: 1558
		public float painFactor = 1f;

		// Token: 0x04000617 RID: 1559
		public float painOffset;

		// Token: 0x04000618 RID: 1560
		public float totalBleedFactor = 1f;

		// Token: 0x04000619 RID: 1561
		public float naturalHealingFactor = -1f;

		// Token: 0x0400061A RID: 1562
		public float forgetMemoryThoughtMtbDays = -1f;

		// Token: 0x0400061B RID: 1563
		public float pctConditionalThoughtsNullified;

		// Token: 0x0400061C RID: 1564
		public float opinionOfOthersFactor = 1f;

		// Token: 0x0400061D RID: 1565
		public float hungerRateFactor = 1f;

		// Token: 0x0400061E RID: 1566
		public float hungerRateFactorOffset;

		// Token: 0x0400061F RID: 1567
		public float restFallFactor = 1f;

		// Token: 0x04000620 RID: 1568
		public float restFallFactorOffset;

		// Token: 0x04000621 RID: 1569
		public float socialFightChanceFactor = 1f;

		// Token: 0x04000622 RID: 1570
		public float foodPoisoningChanceFactor = 1f;

		// Token: 0x04000623 RID: 1571
		public float mentalBreakMtbDays = -1f;

		// Token: 0x04000624 RID: 1572
		public string mentalBreakExplanation;

		// Token: 0x04000625 RID: 1573
		public List<MentalBreakIntensity> allowedMentalBreakIntensities;

		// Token: 0x04000626 RID: 1574
		public List<HediffDef> makeImmuneTo;

		// Token: 0x04000627 RID: 1575
		public List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();

		// Token: 0x04000628 RID: 1576
		public List<HediffGiver> hediffGivers;

		// Token: 0x04000629 RID: 1577
		public List<MentalStateGiver> mentalStateGivers;

		// Token: 0x0400062A RID: 1578
		public List<StatModifier> statOffsets;

		// Token: 0x0400062B RID: 1579
		public List<StatModifier> statFactors;

		// Token: 0x0400062C RID: 1580
		public bool multiplyStatChangesBySeverity;

		// Token: 0x0400062D RID: 1581
		public StatDef statOffsetEffectMultiplier;

		// Token: 0x0400062E RID: 1582
		public StatDef statFactorEffectMultiplier;

		// Token: 0x0400062F RID: 1583
		public StatDef capacityFactorEffectMultiplier;

		// Token: 0x04000630 RID: 1584
		public WorkTags disabledWorkTags;

		// Token: 0x04000631 RID: 1585
		[MustTranslate]
		public string overrideTooltip;

		// Token: 0x04000632 RID: 1586
		[MustTranslate]
		public string extraTooltip;

		// Token: 0x04000633 RID: 1587
		public float partEfficiencyOffset;

		// Token: 0x04000634 RID: 1588
		public bool partIgnoreMissingHP;

		// Token: 0x04000635 RID: 1589
		public bool destroyPart;
	}
}

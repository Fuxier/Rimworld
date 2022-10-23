using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000119 RID: 281
	public class PawnKindDef : Def
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x000260EC File Offset: 0x000242EC
		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000260FC File Offset: 0x000242FC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00026136 File Offset: 0x00024336
		public string GetLabelPlural(int count = -1)
		{
			if (!this.labelPlural.NullOrEmpty())
			{
				return this.labelPlural;
			}
			return Find.ActiveLanguageWorker.Pluralize(this.label, count);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002615D File Offset: 0x0002435D
		public RulePackDef GetNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.nameMakerFemale != null)
			{
				return this.nameMakerFemale;
			}
			if (this.nameMaker != null)
			{
				return this.nameMaker;
			}
			return null;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00026184 File Offset: 0x00024384
		public override void PostLoad()
		{
			if (this.backstoryCategories != null && this.backstoryCategories.Count > 0)
			{
				if (this.backstoryFilters == null)
				{
					this.backstoryFilters = new List<BackstoryCategoryFilter>();
				}
				this.backstoryFilters.Add(new BackstoryCategoryFilter
				{
					categories = this.backstoryCategories
				});
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000261D8 File Offset: 0x000243D8
		public float GetAnimalPointsToHuntOrSlaughter()
		{
			return this.combatPower * 5f * (1f + this.RaceProps.manhunterOnDamageChance * 0.5f) * (1f + this.RaceProps.manhunterOnTameFailChance * 0.2f) * (1f + this.RaceProps.wildness) + this.race.BaseMarketValue;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0002623F File Offset: 0x0002443F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.backstoryFilters != null && this.backstoryFiltersOverride != null)
			{
				yield return "both backstoryCategories and backstoryCategoriesOverride are defined";
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			if (this.combatPower < 0f)
			{
				yield return this.defName + " has no combatPower.";
			}
			if (this.weaponMoney != FloatRange.Zero)
			{
				float num = 999999f;
				int num2;
				int i;
				for (i = 0; i < this.weaponTags.Count; i = num2 + 1)
				{
					IEnumerable<ThingDef> source = from d in DefDatabase<ThingDef>.AllDefs
					where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
					select d;
					if (source.Any<ThingDef>())
					{
						num = Mathf.Min(num, source.Min((ThingDef d) => PawnWeaponGenerator.CheapestNonDerpPriceFor(d)));
					}
					num2 = i;
				}
				if (num < 999999f && num > this.weaponMoney.min)
				{
					yield return string.Concat(new object[]
					{
						"Cheapest weapon with one of my weaponTags costs ",
						num,
						" but weaponMoney min is ",
						this.weaponMoney.min,
						", so could end up weaponless."
					});
				}
			}
			if (!this.RaceProps.Humanlike && this.lifeStages.Count != this.RaceProps.lifeStageAges.Count)
			{
				yield return string.Concat(new object[]
				{
					"PawnKindDef defines ",
					this.lifeStages.Count,
					" lifeStages while race def defines ",
					this.RaceProps.lifeStageAges.Count
				});
			}
			if (this.apparelRequired != null)
			{
				int num2;
				for (int k = 0; k < this.apparelRequired.Count; k = num2 + 1)
				{
					for (int j = k + 1; j < this.apparelRequired.Count; j = num2 + 1)
					{
						if (!ApparelUtility.CanWearTogether(this.apparelRequired[k], this.apparelRequired[j], this.race.race.body))
						{
							yield return string.Concat(new object[]
							{
								"required apparel can't be worn together (",
								this.apparelRequired[k],
								", ",
								this.apparelRequired[j],
								")"
							});
						}
						num2 = j;
					}
					num2 = k;
				}
			}
			if (this.alternateGraphics != null)
			{
				using (List<AlternateGraphic>.Enumerator enumerator2 = this.alternateGraphics.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Weight < 0f)
						{
							yield return "alternate graphic has negative weight.";
						}
					}
				}
				List<AlternateGraphic>.Enumerator enumerator2 = default(List<AlternateGraphic>.Enumerator);
			}
			if (this.RaceProps.Humanlike && this.initialResistanceRange == null)
			{
				yield return "initial resistance range is undefined for humanlike pawn kind.";
			}
			if (this.RaceProps.Humanlike && this.initialWillRange == null)
			{
				yield return "initial will range is undefined for humanlike pawn kind.";
			}
			if (this.xenotypeSet != null)
			{
				foreach (string text2 in this.xenotypeSet.ConfigErrors())
				{
					yield return text2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0002624F File Offset: 0x0002444F
		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}

		// Token: 0x040006D8 RID: 1752
		public ThingDef race;

		// Token: 0x040006D9 RID: 1753
		public FactionDef defaultFactionType;

		// Token: 0x040006DA RID: 1754
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x040006DB RID: 1755
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFiltersOverride;

		// Token: 0x040006DC RID: 1756
		[NoTranslate]
		public List<string> backstoryCategories;

		// Token: 0x040006DD RID: 1757
		[MustTranslate]
		public string labelPlural;

		// Token: 0x040006DE RID: 1758
		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		// Token: 0x040006DF RID: 1759
		public List<AlternateGraphic> alternateGraphics;

		// Token: 0x040006E0 RID: 1760
		public List<TraitRequirement> forcedTraits;

		// Token: 0x040006E1 RID: 1761
		public List<TraitDef> disallowedTraits;

		// Token: 0x040006E2 RID: 1762
		public float alternateGraphicChance;

		// Token: 0x040006E3 RID: 1763
		public XenotypeSet xenotypeSet;

		// Token: 0x040006E4 RID: 1764
		public bool useFactionXenotypes = true;

		// Token: 0x040006E5 RID: 1765
		[LoadAlias("hairTags")]
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x040006E6 RID: 1766
		public HairDef forcedHair;

		// Token: 0x040006E7 RID: 1767
		public Color? forcedHairColor;

		// Token: 0x040006E8 RID: 1768
		public List<MissingPart> missingParts;

		// Token: 0x040006E9 RID: 1769
		public RulePackDef nameMaker;

		// Token: 0x040006EA RID: 1770
		public RulePackDef nameMakerFemale;

		// Token: 0x040006EB RID: 1771
		public List<AbilityDef> abilities;

		// Token: 0x040006EC RID: 1772
		public float backstoryCryptosleepCommonality;

		// Token: 0x040006ED RID: 1773
		public FloatRange? chronologicalAgeRange;

		// Token: 0x040006EE RID: 1774
		public int minGenerationAge;

		// Token: 0x040006EF RID: 1775
		public int maxGenerationAge = 999999;

		// Token: 0x040006F0 RID: 1776
		public bool factionLeader;

		// Token: 0x040006F1 RID: 1777
		public Gender? fixedGender;

		// Token: 0x040006F2 RID: 1778
		public bool allowOldAgeInjuries = true;

		// Token: 0x040006F3 RID: 1779
		public bool generateInitialNonFamilyRelations = true;

		// Token: 0x040006F4 RID: 1780
		public DevelopmentalStage? pawnGroupDevelopmentStage;

		// Token: 0x040006F5 RID: 1781
		public bool destroyGearOnDrop;

		// Token: 0x040006F6 RID: 1782
		public bool canStrip = true;

		// Token: 0x040006F7 RID: 1783
		public float defendPointRadius = -1f;

		// Token: 0x040006F8 RID: 1784
		public bool factionHostileOnKill;

		// Token: 0x040006F9 RID: 1785
		public bool factionHostileOnDeath;

		// Token: 0x040006FA RID: 1786
		public FloatRange? initialResistanceRange;

		// Token: 0x040006FB RID: 1787
		public FloatRange? initialWillRange;

		// Token: 0x040006FC RID: 1788
		public bool forceNoDeathNotification;

		// Token: 0x040006FD RID: 1789
		public bool skipResistant;

		// Token: 0x040006FE RID: 1790
		public float controlGroupPortraitZoom = 1f;

		// Token: 0x040006FF RID: 1791
		public float royalTitleChance;

		// Token: 0x04000700 RID: 1792
		public RoyalTitleDef titleRequired;

		// Token: 0x04000701 RID: 1793
		public RoyalTitleDef minTitleRequired;

		// Token: 0x04000702 RID: 1794
		public List<RoyalTitleDef> titleSelectOne;

		// Token: 0x04000703 RID: 1795
		public bool allowRoyalRoomRequirements = true;

		// Token: 0x04000704 RID: 1796
		public bool allowRoyalApparelRequirements = true;

		// Token: 0x04000705 RID: 1797
		public bool isFighter = true;

		// Token: 0x04000706 RID: 1798
		public float combatPower = -1f;

		// Token: 0x04000707 RID: 1799
		public bool canArriveManhunter = true;

		// Token: 0x04000708 RID: 1800
		public bool canBeSapper;

		// Token: 0x04000709 RID: 1801
		public bool isGoodBreacher;

		// Token: 0x0400070A RID: 1802
		public bool allowInMechClusters = true;

		// Token: 0x0400070B RID: 1803
		public int maxPerGroup = int.MaxValue;

		// Token: 0x0400070C RID: 1804
		public bool aiAvoidCover;

		// Token: 0x0400070D RID: 1805
		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		// Token: 0x0400070E RID: 1806
		public float acceptArrestChanceFactor = 1f;

		// Token: 0x0400070F RID: 1807
		public QualityCategory itemQuality = QualityCategory.Normal;

		// Token: 0x04000710 RID: 1808
		public QualityCategory? forceWeaponQuality;

		// Token: 0x04000711 RID: 1809
		public bool forceNormalGearQuality;

		// Token: 0x04000712 RID: 1810
		public FloatRange gearHealthRange = FloatRange.One;

		// Token: 0x04000713 RID: 1811
		public FloatRange weaponMoney = FloatRange.Zero;

		// Token: 0x04000714 RID: 1812
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x04000715 RID: 1813
		public ThingDef weaponStuffOverride;

		// Token: 0x04000716 RID: 1814
		public ThingStyleDef weaponStyleDef;

		// Token: 0x04000717 RID: 1815
		public FloatRange apparelMoney = FloatRange.Zero;

		// Token: 0x04000718 RID: 1816
		public List<ThingDef> apparelRequired;

		// Token: 0x04000719 RID: 1817
		[NoTranslate]
		public List<string> apparelTags;

		// Token: 0x0400071A RID: 1818
		[NoTranslate]
		public List<string> apparelDisallowTags;

		// Token: 0x0400071B RID: 1819
		public float apparelAllowHeadgearChance = 1f;

		// Token: 0x0400071C RID: 1820
		public bool apparelIgnoreSeasons;

		// Token: 0x0400071D RID: 1821
		public bool apparelIgnorePollution;

		// Token: 0x0400071E RID: 1822
		public bool ignoreFactionApparelStuffRequirements;

		// Token: 0x0400071F RID: 1823
		public Color apparelColor = Color.white;

		// Token: 0x04000720 RID: 1824
		public Color? skinColorOverride;

		// Token: 0x04000721 RID: 1825
		public Color? favoriteColor;

		// Token: 0x04000722 RID: 1826
		public bool ignoreIdeoApparelColors;

		// Token: 0x04000723 RID: 1827
		public List<SpecificApparelRequirement> specificApparelRequirements;

		// Token: 0x04000724 RID: 1828
		public List<ThingDef> techHediffsRequired;

		// Token: 0x04000725 RID: 1829
		public FloatRange techHediffsMoney = FloatRange.Zero;

		// Token: 0x04000726 RID: 1830
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x04000727 RID: 1831
		[NoTranslate]
		public List<string> techHediffsDisallowTags;

		// Token: 0x04000728 RID: 1832
		public float techHediffsChance;

		// Token: 0x04000729 RID: 1833
		public int techHediffsMaxAmount = 1;

		// Token: 0x0400072A RID: 1834
		public float biocodeWeaponChance;

		// Token: 0x0400072B RID: 1835
		public float humanPregnancyChance = 0.03f;

		// Token: 0x0400072C RID: 1836
		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		// Token: 0x0400072D RID: 1837
		public PawnInventoryOption inventoryOptions;

		// Token: 0x0400072E RID: 1838
		public float invNutrition;

		// Token: 0x0400072F RID: 1839
		public ThingDef invFoodDef;

		// Token: 0x04000730 RID: 1840
		public float chemicalAddictionChance;

		// Token: 0x04000731 RID: 1841
		public float combatEnhancingDrugsChance;

		// Token: 0x04000732 RID: 1842
		public IntRange combatEnhancingDrugsCount = IntRange.zero;

		// Token: 0x04000733 RID: 1843
		public List<ChemicalDef> forcedAddictions = new List<ChemicalDef>();

		// Token: 0x04000734 RID: 1844
		public bool trader;

		// Token: 0x04000735 RID: 1845
		public List<SkillRange> skills;

		// Token: 0x04000736 RID: 1846
		public WorkTags requiredWorkTags;

		// Token: 0x04000737 RID: 1847
		public int extraSkillLevels;

		// Token: 0x04000738 RID: 1848
		public int minTotalSkillLevels;

		// Token: 0x04000739 RID: 1849
		public int minBestSkillLevel;

		// Token: 0x0400073A RID: 1850
		[MustTranslate]
		public string labelMale;

		// Token: 0x0400073B RID: 1851
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x0400073C RID: 1852
		[MustTranslate]
		public string labelFemale;

		// Token: 0x0400073D RID: 1853
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x0400073E RID: 1854
		public IntRange wildGroupSize = IntRange.one;

		// Token: 0x0400073F RID: 1855
		public float ecoSystemWeight = 1f;

		// Token: 0x04000740 RID: 1856
		private const int MaxWeaponMoney = 999999;
	}
}

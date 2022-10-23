using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000125 RID: 293
	public class RecipeDef : Def
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x0002684B File Offset: 0x00024A4B
		public RecipeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecipeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.recipe = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0002687D File Offset: 0x00024A7D
		public RecipeWorkerCounter WorkerCounter
		{
			get
			{
				if (this.workerCounterInt == null)
				{
					this.workerCounterInt = (RecipeWorkerCounter)Activator.CreateInstance(this.workerCounterClass);
					this.workerCounterInt.recipe = this;
				}
				return this.workerCounterInt;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x000268AF File Offset: 0x00024AAF
		public IngredientValueGetter IngredientValueGetter
		{
			get
			{
				if (this.ingredientValueGetterInt == null)
				{
					this.ingredientValueGetterInt = (IngredientValueGetter)Activator.CreateInstance(this.ingredientValueGetterClass);
				}
				return this.ingredientValueGetterInt;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x000268D8 File Offset: 0x00024AD8
		public bool AvailableNow
		{
			get
			{
				if (this.researchPrerequisite != null && !this.researchPrerequisite.IsFinished)
				{
					return false;
				}
				if (this.memePrerequisitesAny != null)
				{
					bool flag = false;
					foreach (MemeDef meme in this.memePrerequisitesAny)
					{
						if (Faction.OfPlayer.ideos.HasAnyIdeoWithMeme(meme))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (this.researchPrerequisites != null)
				{
					if (this.researchPrerequisites.Any((ResearchProjectDef r) => !r.IsFinished))
					{
						return false;
					}
				}
				if (this.factionPrerequisiteTags != null)
				{
					if (this.factionPrerequisiteTags.Any((string tag) => Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag)))
					{
						RecipeDef.<>c__DisplayClass79_0 CS$<>8__locals1;
						CS$<>8__locals1.unlockedByIdeo = false;
						this.<get_AvailableNow>g__Check|79_2(ref CS$<>8__locals1);
						if (!CS$<>8__locals1.unlockedByIdeo)
						{
							return false;
						}
					}
				}
				return !this.fromIdeoBuildingPreceptOnly || (ModsConfig.IdeologyActive && IdeoUtility.PlayerHasPreceptForBuilding(this.ProducedThingDef));
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x00026A04 File Offset: 0x00024C04
		public string MinSkillString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				if (this.skillRequirements != null)
				{
					for (int i = 0; i < this.skillRequirements.Count; i++)
					{
						SkillRequirement skillRequirement = this.skillRequirements[i];
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"   ",
							skillRequirement.skill.skillLabel.CapitalizeFirst(),
							": ",
							skillRequirement.minLevel
						}));
						flag = true;
					}
				}
				if (!flag)
				{
					stringBuilder.AppendLine("   (" + "NoneLower".Translate() + ")");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x00026ABC File Offset: 0x00024CBC
		public IEnumerable<ThingDef> AllRecipeUsers
		{
			get
			{
				int num;
				if (this.recipeUsers != null)
				{
					for (int i = 0; i < this.recipeUsers.Count; i = num + 1)
					{
						yield return this.recipeUsers[i];
						num = i;
					}
				}
				List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < thingDefs.Count; i = num + 1)
				{
					if (thingDefs[i].recipes != null && thingDefs[i].recipes.Contains(this))
					{
						yield return thingDefs[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x00026ACC File Offset: 0x00024CCC
		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00026AD8 File Offset: 0x00024CD8
		public bool IsSurgery
		{
			get
			{
				if (this.isSurgeryCached == null)
				{
					this.isSurgeryCached = new bool?(false);
					using (IEnumerator<ThingDef> enumerator = this.AllRecipeUsers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.category == ThingCategory.Pawn)
							{
								this.isSurgeryCached = new bool?(true);
								break;
							}
						}
					}
				}
				return this.isSurgeryCached.Value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x00026B58 File Offset: 0x00024D58
		public ThingDef ProducedThingDef
		{
			get
			{
				if (this.specialProducts != null)
				{
					return null;
				}
				if (this.products == null || this.products.Count != 1)
				{
					return null;
				}
				return this.products[0].thingDef;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00026B8D File Offset: 0x00024D8D
		public ThingDef UIIconThing
		{
			get
			{
				return this.uiIconThing ?? this.ProducedThingDef;
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00026B9F File Offset: 0x00024D9F
		public bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return this.Worker.AvailableOnNow(thing, part);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00026BAE File Offset: 0x00024DAE
		public float WorkAmountTotal(ThingDef stuffDef)
		{
			if (this.workAmount >= 0f)
			{
				return this.workAmount;
			}
			return this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00026BE0 File Offset: 0x00024DE0
		public IEnumerable<ThingDef> PotentiallyMissingIngredients(Pawn billDoer, Map map)
		{
			int num;
			for (int i = 0; i < this.ingredients.Count; i = num + 1)
			{
				IngredientCount ingredientCount = this.ingredients[i];
				bool flag = false;
				List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
				for (int j = 0; j < list.Count; j++)
				{
					Thing thing = list[j];
					if ((billDoer == null || !thing.IsForbidden(billDoer)) && !thing.Position.Fogged(map) && (ingredientCount.IsFixedIngredient || this.fixedIngredientFilter.Allows(thing)) && ingredientCount.filter.Allows(thing))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if (ingredientCount.IsFixedIngredient)
					{
						yield return ingredientCount.filter.AllowedThingDefs.First<ThingDef>();
					}
					else
					{
						ThingDef thingDef = (from x in ingredientCount.filter.AllowedThingDefs
						orderby x.BaseMarketValue
						select x).FirstOrDefault((ThingDef x) => this.fixedIngredientFilter.Allows(x));
						if (thingDef != null)
						{
							yield return thingDef;
						}
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00026C00 File Offset: 0x00024E00
		public bool IsIngredient(ThingDef th)
		{
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				if (this.ingredients[i].filter.Allows(th) && (this.ingredients[i].IsFixedIngredient || this.fixedIngredientFilter.Allows(th)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00026C60 File Offset: 0x00024E60
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.workerClass == null)
			{
				yield return "workerClass is null.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00026C70 File Offset: 0x00024E70
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			DeepProfiler.Start("Stat refs");
			try
			{
				if (this.workTableSpeedStat == null)
				{
					this.workTableSpeedStat = StatDefOf.WorkTableWorkSpeedFactor;
				}
				if (this.workTableEfficiencyStat == null)
				{
					this.workTableEfficiencyStat = StatDefOf.WorkTableEfficiencyFactor;
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ingredients reference resolve");
			try
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					this.ingredients[i].ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("fixedIngredientFilter.ResolveReferences()");
			try
			{
				if (this.fixedIngredientFilter != null)
				{
					this.fixedIngredientFilter.ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter setup");
			try
			{
				if (this.defaultIngredientFilter == null)
				{
					this.defaultIngredientFilter = new ThingFilter();
					if (this.fixedIngredientFilter != null)
					{
						this.defaultIngredientFilter.CopyAllowancesFrom(this.fixedIngredientFilter);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter.ResolveReferences()");
			try
			{
				this.defaultIngredientFilter.ResolveReferences();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00026DB4 File Offset: 0x00024FB4
		public bool CompatibleWithHediff(HediffDef hediffDef)
		{
			if (this.incompatibleWithHediffTags.NullOrEmpty<string>() || hediffDef.tags.NullOrEmpty<string>())
			{
				return true;
			}
			for (int i = 0; i < this.incompatibleWithHediffTags.Count; i++)
			{
				for (int j = 0; j < hediffDef.tags.Count; j++)
				{
					if (this.incompatibleWithHediffTags[i].Equals(hediffDef.tags[j], StringComparison.InvariantCultureIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00026E2C File Offset: 0x0002502C
		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00026E38 File Offset: 0x00025038
		public SkillRequirement FirstSkillRequirementPawnDoesntSatisfy(Pawn pawn)
		{
			if (this.skillRequirements == null)
			{
				return null;
			}
			for (int i = 0; i < this.skillRequirements.Count; i++)
			{
				if (!this.skillRequirements[i].PawnSatisfies(pawn))
				{
					return this.skillRequirements[i];
				}
			}
			return null;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00026E88 File Offset: 0x00025088
		public List<ThingDef> GetPremultipliedSmallIngredients()
		{
			if (this.premultipliedSmallIngredients != null)
			{
				return this.premultipliedSmallIngredients;
			}
			this.premultipliedSmallIngredients = (from td in this.ingredients.SelectMany((IngredientCount ingredient) => ingredient.filter.AllowedThingDefs)
			where td.smallVolume
			select td).Distinct<ThingDef>().ToList<ThingDef>();
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					if (this.ingredients[i].filter.AllowedThingDefs.Any((ThingDef td) => !this.premultipliedSmallIngredients.Contains(td)))
					{
						foreach (ThingDef item in this.ingredients[i].filter.AllowedThingDefs)
						{
							flag |= this.premultipliedSmallIngredients.Remove(item);
						}
					}
				}
			}
			return this.premultipliedSmallIngredients;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00026FB4 File Offset: 0x000251B4
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetIngredientsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.ingredients
			where i.IsFixedIngredient
			select i.FixedIngredient into i
			where i != null
			select i);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00027038 File Offset: 0x00025238
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetProductsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.products
			select i.thingDef);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00027069 File Offset: 0x00025269
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.workSkill != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, "Stat_Recipe_Skill_Desc".Translate(), 4404, null, null, false);
			}
			if (this.ingredients != null && this.ingredients.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), (from ic in this.ingredients
				select ic.SummaryFor(this)).ToCommaList(false, false), "Stat_Recipe_Ingredients_Desc".Translate(), 4405, null, this.GetIngredientsHyperlinks(), false);
			}
			if (this.skillRequirements != null && this.skillRequirements.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), (from sr in this.skillRequirements
				select sr.Summary).ToCommaList(false, false), "Stat_Recipe_SkillRequirements_Desc".Translate(), 4403, null, null, false);
			}
			if (this.products != null && this.products.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), (from pr in this.products
				select pr.Summary).ToCommaList(false, false), "Stat_Recipe_Products_Desc".Translate(), 4405, null, this.GetProductsHyperlinks(), false);
				float num = this.WorkAmountTotal(null);
				if (num > 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, StatDefOf.WorkToMake.LabelCap, num.ToStringWorkAmount(), StatDefOf.WorkToMake.description, StatDefOf.WorkToMake.displayPriorityInCategory, null, null, false);
				}
			}
			if (this.workSpeedStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), this.workSpeedStat.LabelCap, "Stat_Recipe_WorkSpeedStat_Desc".Translate(), 4402, null, null, false);
			}
			if (this.efficiencyStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), this.efficiencyStat.LabelCap, "Stat_Recipe_EfficiencyStat_Desc".Translate(), 4401, null, null, false);
			}
			if (this.IsSurgery)
			{
				if (this.surgerySuccessChanceFactor >= 99999f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail_Desc".Translate(), 4102, null, null, false);
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), this.surgerySuccessChanceFactor.ToStringPercent(), "Stat_Thing_Surgery_SuccessChanceFactor_Desc".Translate(), 4102, null, null, false);
					if (this.deathOnFailedSurgeryChance >= 99999f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), "100%", "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101, null, null, false);
					}
					else
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), this.deathOnFailedSurgeryChance.ToStringPercent(), "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101, null, null, false);
					}
				}
			}
			if (this.addsHediff != null)
			{
				foreach (StatDrawEntry statDrawEntry in MedicalRecipesUtility.GetMedicalStatsFromRecipeDefs(Gen.YieldSingle<RecipeDef>(this)))
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002716C File Offset: 0x0002536C
		[CompilerGenerated]
		private void <get_AvailableNow>g__Check|79_2(ref RecipeDef.<>c__DisplayClass79_0 A_1)
		{
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept_Role precept_Role in ideo.RolesListForReading)
				{
					if (precept_Role.apparelRequirements != null)
					{
						foreach (PreceptApparelRequirement preceptApparelRequirement in precept_Role.apparelRequirements)
						{
							ThingDef thingDef = preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None).FirstOrDefault<ThingDef>();
							if (thingDef == null)
							{
								Log.Error("Apparel requirement for role " + precept_Role.Label + " is null");
							}
							using (List<ThingDefCountClass>.Enumerator enumerator4 = this.products.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									if (enumerator4.Current.thingDef == thingDef)
									{
										A_1.unlockedByIdeo = true;
										return;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000779 RID: 1913
		public Type workerClass = typeof(RecipeWorker);

		// Token: 0x0400077A RID: 1914
		public Type workerCounterClass = typeof(RecipeWorkerCounter);

		// Token: 0x0400077B RID: 1915
		[MustTranslate]
		public string jobString = "Doing an unknown recipe.";

		// Token: 0x0400077C RID: 1916
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x0400077D RID: 1917
		public float workAmount = -1f;

		// Token: 0x0400077E RID: 1918
		public StatDef workSpeedStat;

		// Token: 0x0400077F RID: 1919
		public StatDef efficiencyStat;

		// Token: 0x04000780 RID: 1920
		public StatDef workTableEfficiencyStat;

		// Token: 0x04000781 RID: 1921
		public StatDef workTableSpeedStat;

		// Token: 0x04000782 RID: 1922
		public List<IngredientCount> ingredients = new List<IngredientCount>();

		// Token: 0x04000783 RID: 1923
		public ThingFilter fixedIngredientFilter = new ThingFilter();

		// Token: 0x04000784 RID: 1924
		public ThingFilter defaultIngredientFilter;

		// Token: 0x04000785 RID: 1925
		public bool allowMixingIngredients;

		// Token: 0x04000786 RID: 1926
		public bool ignoreIngredientCountTakeEntireStacks;

		// Token: 0x04000787 RID: 1927
		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		// Token: 0x04000788 RID: 1928
		public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

		// Token: 0x04000789 RID: 1929
		public bool autoStripCorpses = true;

		// Token: 0x0400078A RID: 1930
		public bool interruptIfIngredientIsRotting;

		// Token: 0x0400078B RID: 1931
		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

		// Token: 0x0400078C RID: 1932
		public List<SpecialProductType> specialProducts;

		// Token: 0x0400078D RID: 1933
		public bool productHasIngredientStuff;

		// Token: 0x0400078E RID: 1934
		public bool useIngredientsForColor = true;

		// Token: 0x0400078F RID: 1935
		public int targetCountAdjustment = 1;

		// Token: 0x04000790 RID: 1936
		public ThingDef unfinishedThingDef;

		// Token: 0x04000791 RID: 1937
		public List<SkillRequirement> skillRequirements;

		// Token: 0x04000792 RID: 1938
		public SkillDef workSkill;

		// Token: 0x04000793 RID: 1939
		public float workSkillLearnFactor = 1f;

		// Token: 0x04000794 RID: 1940
		public EffecterDef effectWorking;

		// Token: 0x04000795 RID: 1941
		public SoundDef soundWorking;

		// Token: 0x04000796 RID: 1942
		private ThingDef uiIconThing;

		// Token: 0x04000797 RID: 1943
		public List<ThingDef> recipeUsers;

		// Token: 0x04000798 RID: 1944
		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		// Token: 0x04000799 RID: 1945
		public List<BodyPartGroupDef> appliedOnFixedBodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x0400079A RID: 1946
		public HediffDef addsHediff;

		// Token: 0x0400079B RID: 1947
		public HediffDef removesHediff;

		// Token: 0x0400079C RID: 1948
		public HediffDef addsHediffOnFailure;

		// Token: 0x0400079D RID: 1949
		public HediffDef changesHediffLevel;

		// Token: 0x0400079E RID: 1950
		public List<string> incompatibleWithHediffTags;

		// Token: 0x0400079F RID: 1951
		public int hediffLevelOffset;

		// Token: 0x040007A0 RID: 1952
		public bool hideBodyPartNames;

		// Token: 0x040007A1 RID: 1953
		public bool isViolation;

		// Token: 0x040007A2 RID: 1954
		[MustTranslate]
		public string successfullyRemovedHediffMessage;

		// Token: 0x040007A3 RID: 1955
		public float surgerySuccessChanceFactor = 1f;

		// Token: 0x040007A4 RID: 1956
		public float deathOnFailedSurgeryChance;

		// Token: 0x040007A5 RID: 1957
		public bool targetsBodyPart = true;

		// Token: 0x040007A6 RID: 1958
		public bool anesthetize = true;

		// Token: 0x040007A7 RID: 1959
		public int minPartCount = -1;

		// Token: 0x040007A8 RID: 1960
		public bool surgeryIgnoreEnvironment;

		// Token: 0x040007A9 RID: 1961
		public SurgeryOutcomeEffectDef surgeryOutcomeEffect;

		// Token: 0x040007AA RID: 1962
		public Gender? genderPrerequisite;

		// Token: 0x040007AB RID: 1963
		public bool mustBeFertile;

		// Token: 0x040007AC RID: 1964
		public bool allowedForQuestLodgers = true;

		// Token: 0x040007AD RID: 1965
		public int minAllowedAge = -1;

		// Token: 0x040007AE RID: 1966
		public DevelopmentalStage? developmentalStageFilter;

		// Token: 0x040007AF RID: 1967
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x040007B0 RID: 1968
		public List<MemeDef> memePrerequisitesAny;

		// Token: 0x040007B1 RID: 1969
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040007B2 RID: 1970
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x040007B3 RID: 1971
		public bool fromIdeoBuildingPreceptOnly;

		// Token: 0x040007B4 RID: 1972
		public ConceptDef conceptLearned;

		// Token: 0x040007B5 RID: 1973
		public bool dontShowIfAnyIngredientMissing;

		// Token: 0x040007B6 RID: 1974
		public int displayPriority = 99999;

		// Token: 0x040007B7 RID: 1975
		public bool mechanitorOnlyRecipe;

		// Token: 0x040007B8 RID: 1976
		public int gestationCycles;

		// Token: 0x040007B9 RID: 1977
		public int formingTicks;

		// Token: 0x040007BA RID: 1978
		public bool mechResurrection;

		// Token: 0x040007BB RID: 1979
		[Unsaved(false)]
		private RecipeWorker workerInt;

		// Token: 0x040007BC RID: 1980
		[Unsaved(false)]
		private RecipeWorkerCounter workerCounterInt;

		// Token: 0x040007BD RID: 1981
		[Unsaved(false)]
		private IngredientValueGetter ingredientValueGetterInt;

		// Token: 0x040007BE RID: 1982
		[Unsaved(false)]
		private List<ThingDef> premultipliedSmallIngredients;

		// Token: 0x040007BF RID: 1983
		[Unsaved(false)]
		public bool regenerateOnDifficultyChange;

		// Token: 0x040007C0 RID: 1984
		[Unsaved(false)]
		public int adjustedCount = 1;

		// Token: 0x040007C1 RID: 1985
		private bool? isSurgeryCached;
	}
}

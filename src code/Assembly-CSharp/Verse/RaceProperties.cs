using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000DB RID: 219
	public class RaceProperties
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00022203 File Offset: 0x00020403
		public bool Humanlike
		{
			get
			{
				return this.intelligence >= Intelligence.Humanlike;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00022211 File Offset: 0x00020411
		public bool ToolUser
		{
			get
			{
				return this.intelligence >= Intelligence.ToolUser;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0002221F File Offset: 0x0002041F
		public bool Animal
		{
			get
			{
				return !this.ToolUser && this.IsFlesh;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00022231 File Offset: 0x00020431
		public bool Insect
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Insectoid;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00022240 File Offset: 0x00020440
		public bool Dryad
		{
			get
			{
				return this.animalType == AnimalType.Dryad;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0002224B File Offset: 0x0002044B
		public bool EatsFood
		{
			get
			{
				return this.foodType > FoodTypeFlags.None;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00022258 File Offset: 0x00020458
		public float FoodLevelPercentageWantEat
		{
			get
			{
				switch (this.ResolvedDietCategory)
				{
				case DietCategory.NeverEats:
					return 0.3f;
				case DietCategory.Herbivorous:
					return 0.45f;
				case DietCategory.Dendrovorous:
					return 0.45f;
				case DietCategory.Ovivorous:
					return 0.4f;
				case DietCategory.Omnivorous:
					return 0.3f;
				case DietCategory.Carnivorous:
					return 0.3f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x000222B8 File Offset: 0x000204B8
		public DietCategory ResolvedDietCategory
		{
			get
			{
				if (!this.EatsFood)
				{
					return DietCategory.NeverEats;
				}
				if (this.Eats(FoodTypeFlags.Tree))
				{
					return DietCategory.Dendrovorous;
				}
				if (this.Eats(FoodTypeFlags.Meat))
				{
					if (this.Eats(FoodTypeFlags.VegetableOrFruit) || this.Eats(FoodTypeFlags.Plant))
					{
						return DietCategory.Omnivorous;
					}
					return DietCategory.Carnivorous;
				}
				else
				{
					if (this.Eats(FoodTypeFlags.AnimalProduct))
					{
						return DietCategory.Ovivorous;
					}
					return DietCategory.Herbivorous;
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x0002230C File Offset: 0x0002050C
		public DeathActionWorker DeathActionWorker
		{
			get
			{
				if (this.deathActionWorkerInt == null)
				{
					if (this.deathActionWorkerClass != null)
					{
						this.deathActionWorkerInt = (DeathActionWorker)Activator.CreateInstance(this.deathActionWorkerClass);
					}
					else
					{
						this.deathActionWorkerInt = new DeathActionWorker_Simple();
					}
				}
				return this.deathActionWorkerInt;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00022358 File Offset: 0x00020558
		public FleshTypeDef FleshType
		{
			get
			{
				if (this.fleshType != null)
				{
					return this.fleshType;
				}
				return FleshTypeDefOf.Normal;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x0002236E File Offset: 0x0002056E
		public bool IsMechanoid
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0002237D File Offset: 0x0002057D
		public bool IsFlesh
		{
			get
			{
				return this.FleshType != FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x0002238F File Offset: 0x0002058F
		public ThingDef BloodDef
		{
			get
			{
				if (this.bloodDef != null)
				{
					return this.bloodDef;
				}
				if (this.IsFlesh)
				{
					return ThingDefOf.Filth_Blood;
				}
				return null;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x000223AF File Offset: 0x000205AF
		public bool CanDoHerdMigration
		{
			get
			{
				return this.Animal && this.herdMigrationAllowed;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x000223C1 File Offset: 0x000205C1
		public bool CanPassFences
		{
			get
			{
				return !this.FenceBlocked;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x000223CC File Offset: 0x000205CC
		public bool FenceBlocked
		{
			get
			{
				return this.Roamer;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x000223D4 File Offset: 0x000205D4
		public bool Roamer
		{
			get
			{
				return this.roamMtbDays != null;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x000223E1 File Offset: 0x000205E1
		public bool IsWorkMech
		{
			get
			{
				return !this.mechEnabledWorkTypes.NullOrEmpty<WorkTypeDef>();
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x000223F4 File Offset: 0x000205F4
		public PawnKindDef AnyPawnKind
		{
			get
			{
				if (this.cachedAnyPawnKind == null)
				{
					List<PawnKindDef> allDefsListForReading = DefDatabase<PawnKindDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].race.race == this)
						{
							this.cachedAnyPawnKind = allDefsListForReading[i];
							break;
						}
					}
				}
				return this.cachedAnyPawnKind;
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00022449 File Offset: 0x00020649
		public RulePackDef GetNameGenerator(Gender gender)
		{
			if (gender == Gender.Female && this.nameGeneratorFemale != null)
			{
				return this.nameGeneratorFemale;
			}
			return this.nameGenerator;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00022464 File Offset: 0x00020664
		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00022474 File Offset: 0x00020674
		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != FoodPreferability.Undefined && (this.willNeverEat == null || !this.willNeverEat.Contains(t)) && this.Eats(t.ingestible.foodType);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x000224CD File Offset: 0x000206CD
		public bool Eats(FoodTypeFlags food)
		{
			return this.EatsFood && (this.foodType & food) > FoodTypeFlags.None;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x000224E4 File Offset: 0x000206E4
		public void ResolveReferencesSpecial()
		{
			if (this.specificMeatDef != null)
			{
				this.meatDef = this.specificMeatDef;
			}
			else if (this.useMeatFrom != null)
			{
				this.meatDef = this.useMeatFrom.race.meatDef;
			}
			if (this.useLeatherFrom != null)
			{
				this.leatherDef = this.useLeatherFrom.race.leatherDef;
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00022543 File Offset: 0x00020743
		public IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			if (this.soundMeleeHitPawn == null)
			{
				yield return "soundMeleeHitPawn is null";
			}
			if (this.soundMeleeHitBuilding == null)
			{
				yield return "soundMeleeHitBuilding is null";
			}
			if (this.soundMeleeMiss == null)
			{
				yield return "soundMeleeMiss is null";
			}
			if (this.predator && !this.Eats(FoodTypeFlags.Meat))
			{
				yield return "predator but doesn't eat meat";
			}
			int num;
			for (int i = 0; i < this.lifeStageAges.Count; i = num + 1)
			{
				for (int j = 0; j < i; j = num + 1)
				{
					if (this.lifeStageAges[j].minAge > this.lifeStageAges[i].minAge)
					{
						yield return "lifeStages minAges are not in ascending order";
					}
					num = j;
				}
				num = i;
			}
			if (thingDef.IsCaravanRideable())
			{
				if (!this.lifeStageAges.Any((LifeStageAge s) => s.def.caravanRideable))
				{
					yield return "must contain at least one lifeStage with caravanRideable when CaravanRidingSpeedFactor is defined";
				}
			}
			if (this.litterSizeCurve != null)
			{
				foreach (string text in this.litterSizeCurve.ConfigErrors("litterSizeCurve"))
				{
					yield return text;
				}
				IEnumerator<string> enumerator = null;
			}
			if (this.nameOnTameChance > 0f && this.nameGenerator == null)
			{
				yield return "can be named, but has no nameGenerator";
			}
			if (this.Animal && this.wildness < 0f)
			{
				yield return "is animal but wildness is not defined";
			}
			if (this.specificMeatDef != null && this.useMeatFrom != null)
			{
				yield return "specificMeatDef and useMeatFrom are both non-null. specificMeatDef will be chosen.";
			}
			if (this.useMeatFrom != null && this.useMeatFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use meat from non-pawn " + this.useMeatFrom;
			}
			if (this.useMeatFrom != null && this.useMeatFrom.race.useMeatFrom != null)
			{
				yield return string.Concat(new object[]
				{
					"tries to use meat from ",
					this.useMeatFrom,
					" which uses meat from ",
					this.useMeatFrom.race.useMeatFrom
				});
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.category != ThingCategory.Pawn)
			{
				yield return "tries to use leather from non-pawn " + this.useLeatherFrom;
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.race.useLeatherFrom != null)
			{
				yield return string.Concat(new object[]
				{
					"tries to use leather from ",
					this.useLeatherFrom,
					" which uses leather from ",
					this.useLeatherFrom.race.useLeatherFrom
				});
			}
			if (this.Animal && this.trainability == null)
			{
				yield return "animal has trainability = null";
			}
			if (this.fleshType == FleshTypeDefOf.Normal && this.gestationPeriodDays < 0f)
			{
				yield return "normal flesh but gestationPeriodDays not configured.";
			}
			if (this.IsMechanoid && thingDef.butcherProducts.NullOrEmpty<ThingDefCountClass>())
			{
				yield return thingDef.label + " mech has no butcher products set";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0002255A File Offset: 0x0002075A
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
		{
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			if (!ModsConfig.BiotechActive || !this.Humanlike || ((pawn != null) ? pawn.genes : null) == null || pawn.genes.Xenotype == XenotypeDefOf.Baseliner)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Race".Translate(), parentDef.LabelCap, parentDef.description, 2100, null, null, false);
			}
			if (!parentDef.race.IsMechanoid)
			{
				string text = this.foodType.ToHumanString().CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Diet".Translate(), text, "Stat_Race_Diet_Desc".Translate(text), 1500, null, null, false);
			}
			Pawn pawn2;
			if ((pawn2 = (req.Thing as Pawn)) != null)
			{
				Pawn_NeedsTracker needs = pawn2.needs;
				if (((needs != null) ? needs.food : null) != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "FoodConsumption".Translate(), RaceProperties.NutritionEatenPerDay(pawn2), RaceProperties.NutritionEatenPerDayExplanation(pawn2, false, false, true), 1600, null, null, false);
				}
			}
			if (parentDef.race.leatherDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "LeatherType".Translate(), parentDef.race.leatherDef.LabelCap, "Stat_Race_LeatherType_Desc".Translate(), 3550, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(parentDef.race.leatherDef, -1)
				}, false);
			}
			if (parentDef.race.Animal || this.wildness > 0f)
			{
				StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Wildness".Translate(), this.wildness.ToStringPercent(), TrainableUtility.GetWildnessExplanation(parentDef), 2050, null, null, false);
				yield return statDrawEntry;
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "HarmedRevengeChance".Translate(), PawnUtility.GetManhunterOnDamageChance(parentDef.race).ToStringPercent(), "HarmedRevengeChanceExplanation".Translate(), 510, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "TameFailedRevengeChance".Translate(), parentDef.race.manhunterOnTameFailChance.ToStringPercent(), "Stat_Race_Animal_TameFailedRevengeChance_Desc".Translate(), 511, null, null, false);
			}
			if (this.intelligence < Intelligence.Humanlike && this.trainability != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Trainability".Translate(), this.trainability.LabelCap, "Stat_Race_Trainability_Desc".Translate(), 2500, null, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "StatsReport_LifeExpectancy".Translate(), this.lifeExpectancy.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Absolute), "Stat_Race_LifeExpectancy_Desc".Translate(), 2000, null, null, false);
			if (parentDef.race.Animal || this.FenceBlocked)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "StatsReport_BlockedByFences".Translate(), this.FenceBlocked ? "Yes".Translate() : "No".Translate(), "Stat_Race_BlockedByFences_Desc".Translate(), 2040, null, null, false);
			}
			if (parentDef.race.Animal)
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "PackAnimal".Translate(), this.packAnimal ? "Yes".Translate() : "No".Translate(), "PackAnimalExplanation".Translate(), 2202, null, null, false);
				yield return statDrawEntry2;
				Pawn pawn3;
				if ((pawn3 = (req.Thing as Pawn)) != null && pawn3.gender != Gender.None)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Sex".Translate(), pawn3.gender.GetLabel(true).CapitalizeFirst(), pawn3.gender.GetLabel(true).CapitalizeFirst(), 2208, null, null, false);
				}
				if (parentDef.race.nuzzleMtbHours > 0f)
				{
					StatDrawEntry statDrawEntry3 = new StatDrawEntry(StatCategoryDefOf.PawnSocial, "NuzzleInterval".Translate(), Mathf.RoundToInt(parentDef.race.nuzzleMtbHours * 2500f).ToStringTicksToPeriod(true, false, true, true, false), "NuzzleIntervalExplanation".Translate(), 500, null, null, false);
					yield return statDrawEntry3;
				}
				if (parentDef.race.roamMtbDays != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "StatsReport_RoamInterval".Translate(), Mathf.RoundToInt(parentDef.race.roamMtbDays.Value * 60000f).ToStringTicksToPeriod(true, false, true, true, false), "Stat_Race_RoamInterval_Desc".Translate(), 2030, null, null, false);
				}
				foreach (StatDrawEntry statDrawEntry4 in AnimalProductionUtility.AnimalProductionStats(parentDef))
				{
					yield return statDrawEntry4;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			if (ModsConfig.BiotechActive && this.IsMechanoid)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Mechanoid, "MechWeightClass".Translate(), this.mechWeightClass.ToStringHuman().CapitalizeFirst(), "MechWeightClassExplanation".Translate(), 500, null, null, false);
				ThingDef thingDef = MechanitorUtility.RechargerForMech(parentDef);
				if (thingDef != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Mechanoid, "StatsReport_RechargerNeeded".Translate(), thingDef.LabelCap, "StatsReport_RechargerNeeded_Desc".Translate(), 503, null, new Dialog_InfoCard.Hyperlink[]
					{
						new Dialog_InfoCard.Hyperlink(thingDef, -1)
					}, false);
				}
				foreach (StatDrawEntry statDrawEntry5 in MechWorkUtility.SpecialDisplayStats(parentDef, req))
				{
					yield return statDrawEntry5;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00022578 File Offset: 0x00020778
		public static string NutritionEatenPerDay(Pawn p)
		{
			float num = p.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false) * 60000f;
			Hediff firstHediffOfDef;
			HediffComp_Lactating hediffComp_Lactating;
			if (ModsConfig.BiotechActive && (firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false)) != null && (hediffComp_Lactating = firstHediffOfDef.TryGetComp<HediffComp_Lactating>()) != null)
			{
				num += hediffComp_Lactating.AddedNutritionPerDay();
			}
			if (ModsConfig.BiotechActive && p.genes != null)
			{
				int num2 = 0;
				foreach (Gene gene in p.genes.GenesListForReading)
				{
					if (!gene.Overridden)
					{
						num2 += gene.def.biostatMet;
					}
				}
				num *= GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)num2);
			}
			return num.ToString("0.##");
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0002265C File Offset: 0x0002085C
		public static string NutritionEatenPerDayExplanation(Pawn p, bool showDiet = false, bool showLegend = false, bool showCalculations = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("NutritionEatenPerDayTip".Translate(ThingDefOf.MealSimple.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##")));
			stringBuilder.AppendLine();
			if (showDiet)
			{
				stringBuilder.AppendLine("CanEat".Translate() + ": " + p.RaceProps.foodType.ToHumanString());
				stringBuilder.AppendLine();
			}
			if (showLegend)
			{
				stringBuilder.AppendLine("Legend".Translate() + ":");
				stringBuilder.AppendLine("NoDietCategoryLetter".Translate() + " - " + DietCategory.Omnivorous.ToStringHuman());
				DietCategory[] array = (DietCategory[])Enum.GetValues(typeof(DietCategory));
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != DietCategory.NeverEats && array[i] != DietCategory.Omnivorous)
					{
						stringBuilder.AppendLine(array[i].ToStringHumanShort() + " - " + array[i].ToStringHuman());
					}
				}
				stringBuilder.AppendLine();
			}
			if (showCalculations)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + (p.ageTracker.CurLifeStage.hungerRateFactor * p.RaceProps.baseHungerRate * 2.6666667E-05f * 60000f).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute));
				if (p.health.hediffSet.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate() + ": " + p.health.hediffSet.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Hediff hediff in p.health.hediffSet.hediffs)
					{
						if (hediff.CurStage != null && hediff.CurStage.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + hediff.LabelCap + ": " + hediff.CurStage.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
					foreach (Hediff hediff2 in p.health.hediffSet.hediffs)
					{
						if (hediff2.CurStage != null && hediff2.CurStage.hungerRateFactorOffset != 0f)
						{
							stringBuilder.AppendLine("    " + hediff2.LabelCap + ": +" + hediff2.CurStage.hungerRateFactorOffset.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Factor));
						}
					}
				}
				if (p.story != null && p.story.traits != null && p.story.traits.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate() + ": " + p.story.traits.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Trait trait in p.story.traits.allTraits)
					{
						if (!trait.Suppressed && trait.CurrentData.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + trait.LabelCap + ": " + trait.CurrentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
				}
				Building_Bed building_Bed = p.CurrentBed() ?? p.CurrentCaravanBed();
				if (building_Bed != null)
				{
					float statValue = building_Bed.GetStatValue(StatDefOf.BedHungerRateFactor, true, -1);
					if (statValue != 1f)
					{
						stringBuilder.AppendLine().AppendLine("StatsReport_InBed".Translate() + ": x" + statValue.ToStringPercent());
					}
				}
				if (p.genes != null)
				{
					int num = 0;
					foreach (Gene gene in p.genes.GenesListForReading)
					{
						if (!gene.Overridden)
						{
							num += gene.def.biostatMet;
						}
					}
					float num2 = GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)num);
					if (num2 != 1f)
					{
						stringBuilder.AppendLine().AppendLine("FactorForMetabolism".Translate() + ": x" + num2.ToStringPercent());
					}
				}
				Hediff firstHediffOfDef;
				HediffComp_Lactating hediffComp_Lactating;
				if (ModsConfig.BiotechActive && (firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false)) != null && (hediffComp_Lactating = firstHediffOfDef.TryGetComp<HediffComp_Lactating>()) != null)
				{
					float f = hediffComp_Lactating.AddedNutritionPerDay();
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(firstHediffOfDef.LabelBaseCap + ": " + f.ToStringWithSign("0.##"));
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + RaceProperties.NutritionEatenPerDay(p));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x04000440 RID: 1088
		public Intelligence intelligence;

		// Token: 0x04000441 RID: 1089
		private FleshTypeDef fleshType;

		// Token: 0x04000442 RID: 1090
		private ThingDef bloodDef;

		// Token: 0x04000443 RID: 1091
		public bool hasGenders = true;

		// Token: 0x04000444 RID: 1092
		public bool needsRest = true;

		// Token: 0x04000445 RID: 1093
		public ThinkTreeDef thinkTreeMain;

		// Token: 0x04000446 RID: 1094
		public ThinkTreeDef thinkTreeConstant;

		// Token: 0x04000447 RID: 1095
		public DutyDef dutyBoss;

		// Token: 0x04000448 RID: 1096
		public PawnNameCategory nameCategory;

		// Token: 0x04000449 RID: 1097
		public FoodTypeFlags foodType;

		// Token: 0x0400044A RID: 1098
		public BodyDef body;

		// Token: 0x0400044B RID: 1099
		public Type deathActionWorkerClass;

		// Token: 0x0400044C RID: 1100
		public List<AnimalBiomeRecord> wildBiomes;

		// Token: 0x0400044D RID: 1101
		public SimpleCurve ageGenerationCurve;

		// Token: 0x0400044E RID: 1102
		public bool makesFootprints;

		// Token: 0x0400044F RID: 1103
		public int executionRange = 2;

		// Token: 0x04000450 RID: 1104
		public float lifeExpectancy = 10f;

		// Token: 0x04000451 RID: 1105
		public List<HediffGiverSetDef> hediffGiverSets;

		// Token: 0x04000452 RID: 1106
		public float? roamMtbDays;

		// Token: 0x04000453 RID: 1107
		public bool allowedOnCaravan = true;

		// Token: 0x04000454 RID: 1108
		public bool canReleaseToWild = true;

		// Token: 0x04000455 RID: 1109
		public bool playerCanChangeMaster = true;

		// Token: 0x04000456 RID: 1110
		public bool showTrainables = true;

		// Token: 0x04000457 RID: 1111
		public bool hideTrainingTab;

		// Token: 0x04000458 RID: 1112
		public bool herdAnimal;

		// Token: 0x04000459 RID: 1113
		public bool packAnimal;

		// Token: 0x0400045A RID: 1114
		public bool predator;

		// Token: 0x0400045B RID: 1115
		public float maxPreyBodySize = 99999f;

		// Token: 0x0400045C RID: 1116
		public float wildness;

		// Token: 0x0400045D RID: 1117
		public float petness;

		// Token: 0x0400045E RID: 1118
		public float nuzzleMtbHours = -1f;

		// Token: 0x0400045F RID: 1119
		public float manhunterOnDamageChance;

		// Token: 0x04000460 RID: 1120
		public float manhunterOnTameFailChance;

		// Token: 0x04000461 RID: 1121
		public bool canBePredatorPrey = true;

		// Token: 0x04000462 RID: 1122
		public bool herdMigrationAllowed = true;

		// Token: 0x04000463 RID: 1123
		public AnimalType animalType;

		// Token: 0x04000464 RID: 1124
		public List<ThingDef> willNeverEat;

		// Token: 0x04000465 RID: 1125
		public bool giveNonToolUserBeatFireVerb;

		// Token: 0x04000466 RID: 1126
		public bool disableAreaControl;

		// Token: 0x04000467 RID: 1127
		public int maxMechEnergy = 100;

		// Token: 0x04000468 RID: 1128
		public List<WorkTypeDef> mechEnabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04000469 RID: 1129
		public int mechFixedSkillLevel = 10;

		// Token: 0x0400046A RID: 1130
		public List<MechWorkTypePriority> mechWorkTypePriorities;

		// Token: 0x0400046B RID: 1131
		public int? bulletStaggerDelayTicks;

		// Token: 0x0400046C RID: 1132
		public float? bulletStaggerSpeedFactor;

		// Token: 0x0400046D RID: 1133
		public EffecterDef bulletStaggerEffecterDef;

		// Token: 0x0400046E RID: 1134
		public bool bulletStaggerIgnoreBodySize;

		// Token: 0x0400046F RID: 1135
		public MechWeightClass mechWeightClass = MechWeightClass.Medium;

		// Token: 0x04000470 RID: 1136
		public float gestationPeriodDays = -1f;

		// Token: 0x04000471 RID: 1137
		public SimpleCurve litterSizeCurve;

		// Token: 0x04000472 RID: 1138
		public float mateMtbHours = 12f;

		// Token: 0x04000473 RID: 1139
		[NoTranslate]
		public List<string> untrainableTags;

		// Token: 0x04000474 RID: 1140
		[NoTranslate]
		public List<string> trainableTags;

		// Token: 0x04000475 RID: 1141
		public TrainabilityDef trainability;

		// Token: 0x04000476 RID: 1142
		private RulePackDef nameGenerator;

		// Token: 0x04000477 RID: 1143
		private RulePackDef nameGeneratorFemale;

		// Token: 0x04000478 RID: 1144
		public float nameOnTameChance;

		// Token: 0x04000479 RID: 1145
		public float baseBodySize = 1f;

		// Token: 0x0400047A RID: 1146
		public float baseHealthScale = 1f;

		// Token: 0x0400047B RID: 1147
		public float baseHungerRate = 1f;

		// Token: 0x0400047C RID: 1148
		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		// Token: 0x0400047D RID: 1149
		public List<LifeStageWorkSettings> lifeStageWorkSettings = new List<LifeStageWorkSettings>();

		// Token: 0x0400047E RID: 1150
		[MustTranslate]
		public string meatLabel;

		// Token: 0x0400047F RID: 1151
		public Color meatColor = Color.white;

		// Token: 0x04000480 RID: 1152
		public float meatMarketValue = 2f;

		// Token: 0x04000481 RID: 1153
		public ThingDef specificMeatDef;

		// Token: 0x04000482 RID: 1154
		public ThingDef useMeatFrom;

		// Token: 0x04000483 RID: 1155
		public ThingDef useLeatherFrom;

		// Token: 0x04000484 RID: 1156
		public ThingDef leatherDef;

		// Token: 0x04000485 RID: 1157
		public ShadowData specialShadowData;

		// Token: 0x04000486 RID: 1158
		public List<Vector3> headPosPerRotation;

		// Token: 0x04000487 RID: 1159
		public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

		// Token: 0x04000488 RID: 1160
		public float soundCallIntervalFriendlyFactor = 1f;

		// Token: 0x04000489 RID: 1161
		public float soundCallIntervalAggressiveFactor = 0.25f;

		// Token: 0x0400048A RID: 1162
		public SoundDef soundMeleeHitPawn;

		// Token: 0x0400048B RID: 1163
		public SoundDef soundMeleeHitBuilding;

		// Token: 0x0400048C RID: 1164
		public SoundDef soundMeleeMiss;

		// Token: 0x0400048D RID: 1165
		public SoundDef soundMeleeDodge;

		// Token: 0x0400048E RID: 1166
		public SoundDef soundAmbience;

		// Token: 0x0400048F RID: 1167
		[Unsaved(false)]
		private DeathActionWorker deathActionWorkerInt;

		// Token: 0x04000490 RID: 1168
		[Unsaved(false)]
		public ThingDef meatDef;

		// Token: 0x04000491 RID: 1169
		[Unsaved(false)]
		public ThingDef corpseDef;

		// Token: 0x04000492 RID: 1170
		[Unsaved(false)]
		private PawnKindDef cachedAnyPawnKind;
	}
}

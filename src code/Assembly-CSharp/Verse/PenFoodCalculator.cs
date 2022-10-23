using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001AE RID: 430
	public class PenFoodCalculator
	{
		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x00043205 File Offset: 0x00041405
		public float NutritionPerDayToday
		{
			get
			{
				return this.sumNutritionPerDayToday;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x0004320D File Offset: 0x0004140D
		public List<PenFoodCalculator.PenAnimalInfo> ActualAnimalInfos
		{
			get
			{
				return this.animals;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x00043215 File Offset: 0x00041415
		public List<PenFoodCalculator.PenFoodItemInfo> AllStockpiledInfos
		{
			get
			{
				return this.stockpiled;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0004321D File Offset: 0x0004141D
		public bool Unenclosed
		{
			get
			{
				return this.numCells == 0;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x00043228 File Offset: 0x00041428
		public float SumNutritionConsumptionPerDay
		{
			get
			{
				float num = 0f;
				foreach (PenFoodCalculator.PenAnimalInfo penAnimalInfo in this.animals)
				{
					num += penAnimalInfo.TotalNutritionConsumptionPerDay;
				}
				return num;
			}
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00043284 File Offset: 0x00041484
		public PenFoodCalculator.PenAnimalInfo GetAnimalInfo(ThingDef animalDef)
		{
			foreach (PenFoodCalculator.PenAnimalInfo penAnimalInfo in this.animals)
			{
				if (penAnimalInfo.animalDef == animalDef)
				{
					return penAnimalInfo;
				}
			}
			PenFoodCalculator.PenAnimalInfo penAnimalInfo2 = new PenFoodCalculator.PenAnimalInfo(animalDef);
			this.animals.Add(penAnimalInfo2);
			return penAnimalInfo2;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000432F4 File Offset: 0x000414F4
		public PenFoodCalculator.PenFoodItemInfo GetStockpiledInfo(ThingDef itemDef)
		{
			foreach (PenFoodCalculator.PenFoodItemInfo penFoodItemInfo in this.stockpiled)
			{
				if (penFoodItemInfo.itemDef == itemDef)
				{
					return penFoodItemInfo;
				}
			}
			PenFoodCalculator.PenFoodItemInfo penFoodItemInfo2 = new PenFoodCalculator.PenFoodItemInfo(itemDef);
			this.stockpiled.Add(penFoodItemInfo2);
			return penFoodItemInfo2;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00043364 File Offset: 0x00041564
		public string NaturalGrowthRateTooltip()
		{
			if (this.cachedNaturalGrowthRateTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_NaturalNutritionGrowthRateDescription".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("PenFoodTab_NaturalNutritionGrowthRateSeasonal".Translate());
				stringBuilder.AppendLine();
				stringBuilder.Append("PenFoodTab_GrowthPerSeason".Translate()).AppendLine(":");
				Vector2 vector = Find.WorldGrid.LongLatOf(this.mapCalc.tile);
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum = (Quadrum)i;
					stringBuilder.Append("- ").Append(quadrum.Label()).Append(" (").Append(quadrum.GetSeason(vector.y).Label()).Append("): ");
					stringBuilder.AppendLine(PenFoodCalculator.NutritionPerDayToString(this.nutritionPerDayPerQuadrum.ForQuadrum(quadrum), false));
				}
				this.cachedNaturalGrowthRateTooltip = stringBuilder.ToString();
			}
			return this.cachedNaturalGrowthRateTooltip;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00043470 File Offset: 0x00041670
		public string TotalConsumedToolTop()
		{
			if (this.cachedTotalConsumedTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_NutritionConsumptionDescription".Translate());
				this.cachedTotalConsumedTooltip = stringBuilder.ToString();
			}
			return this.cachedTotalConsumedTooltip;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000434B4 File Offset: 0x000416B4
		public string StockpileToolTip()
		{
			if (this.cachedStockpileTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_StockpileTotalDescription".Translate());
				stringBuilder.AppendLine();
				foreach (PenFoodCalculator.PenFoodItemInfo penFoodItemInfo in this.stockpiled)
				{
					stringBuilder.Append("- ").Append(penFoodItemInfo.itemDef.LabelCap).Append(" x").Append(penFoodItemInfo.totalCount).Append(": ");
					stringBuilder.AppendLine(PenFoodCalculator.NutritionToString(penFoodItemInfo.totalNutritionAvailable, true));
				}
				this.cachedStockpileTooltip = stringBuilder.ToString();
			}
			return this.cachedStockpileTooltip;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00043594 File Offset: 0x00041794
		private void Reset(Map map)
		{
			this.mapCalc.Reset(map);
			this.animals.Clear();
			this.stockpiled.Clear();
			this.cachedNaturalGrowthRateTooltip = null;
			this.cachedTotalConsumedTooltip = null;
			this.cachedStockpileTooltip = null;
			this.nutritionPerDayPerQuadrum = new MapPastureNutritionCalculator.NutritionPerDayPerQuadrum();
			this.sumNutritionPerDayToday = 0f;
			this.sumStockpiledNutritionAvailableNow = 0f;
			this.numCells = 0;
			this.numCellsSoil = 0;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00043608 File Offset: 0x00041808
		public List<PenFoodCalculator.PenAnimalInfo> ComputeExampleAnimals(List<ThingDef> animalDefs)
		{
			this.tmpAddedExampleAnimals.Clear();
			foreach (ThingDef thingDef in animalDefs)
			{
				LifeStageAge lifeStageAge = thingDef.race.lifeStageAges.Last<LifeStageAge>();
				PenFoodCalculator.PenAnimalInfo penAnimalInfo = new PenFoodCalculator.PenAnimalInfo(thingDef);
				penAnimalInfo.example = true;
				penAnimalInfo.nutritionConsumptionPerDay = SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(thingDef, lifeStageAge.def);
				this.tmpAddedExampleAnimals.Add(penAnimalInfo);
			}
			PenFoodCalculator.SortAnimals(this.tmpAddedExampleAnimals);
			return this.tmpAddedExampleAnimals;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x000436A8 File Offset: 0x000418A8
		public void ResetAndProcessPen(CompAnimalPenMarker marker)
		{
			this.ResetAndProcessPen(marker.parent.Position, marker.parent.Map, false);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x000436C7 File Offset: 0x000418C7
		public void ResetAndProcessPen(IntVec3 position, Map map, bool considerBlueprints)
		{
			this.Reset(map);
			if (map == null)
			{
				return;
			}
			if (considerBlueprints)
			{
				this.ProcessBlueprintPen(position, map);
			}
			else
			{
				this.ProcessRealPen(position, map);
			}
			this.SortResults(map);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000436F0 File Offset: 0x000418F0
		private void SortResults(Map map)
		{
			this.stockpiled.Sort(new Comparison<PenFoodCalculator.PenFoodItemInfo>(PenFoodCalculator.<>c.<>9.<SortResults>g__FoodSorter|36_0));
			PenFoodCalculator.SortAnimals(this.animals);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00043718 File Offset: 0x00041918
		private static void SortAnimals(List<PenFoodCalculator.PenAnimalInfo> infos)
		{
			infos.Sort(new Comparison<PenFoodCalculator.PenAnimalInfo>(PenFoodCalculator.<>c.<>9.<SortAnimals>g__AnimalSorter|37_0));
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00043730 File Offset: 0x00041930
		private void ProcessBlueprintPen(IntVec3 markerPos, Map map)
		{
			this.blueprintEnclosureCalc.VisitPen(markerPos, map);
			if (!this.blueprintEnclosureCalc.isEnclosed)
			{
				return;
			}
			foreach (IntVec3 c in this.blueprintEnclosureCalc.cellsFound)
			{
				this.ProcessCell(c, map);
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x000437A4 File Offset: 0x000419A4
		private void ProcessRealPen(IntVec3 markerPos, Map map)
		{
			foreach (District district in this.connectedDistrictsCalc.CalculateConnectedDistricts(markerPos, map))
			{
				foreach (Region region in district.Regions)
				{
					this.ProcessRegion(region);
				}
			}
			this.connectedDistrictsCalc.Reset();
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00043844 File Offset: 0x00041A44
		private void ProcessRegion(Region region)
		{
			foreach (IntVec3 c in region.Cells)
			{
				this.ProcessCell(c, region.Map);
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00043898 File Offset: 0x00041A98
		private void ProcessCell(IntVec3 c, Map map)
		{
			this.ProcessTerrain(c, map);
			foreach (Thing thing in c.GetThingList(map))
			{
				Pawn animal;
				if ((animal = (thing as Pawn)) != null && thing.def.race.Animal)
				{
					this.ProcessAnimal(animal);
				}
				else if (thing.def.category == ThingCategory.Item && thing.IngestibleNow && MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(thing.def))
				{
					this.ProcessStockpiledFood(thing);
				}
			}
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0004393C File Offset: 0x00041B3C
		private void ProcessTerrain(IntVec3 c, Map map)
		{
			this.numCells++;
			if (c.GetEdifice(map) != null)
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(map);
			if (terrain.IsSoil)
			{
				this.numCellsSoil++;
			}
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other = this.mapCalc.CalculateAverageNutritionPerDay(terrain);
			this.nutritionPerDayPerQuadrum.AddFrom(other);
			this.sumNutritionPerDayToday += this.mapCalc.GetAverageNutritionPerDayToday(terrain);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x000439B4 File Offset: 0x00041BB4
		private void ProcessStockpiledFood(Thing thing)
		{
			PenFoodCalculator.PenFoodItemInfo stockpiledInfo = this.GetStockpiledInfo(thing.def);
			float num = thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thing.stackCount;
			stockpiledInfo.totalCount += thing.stackCount;
			stockpiledInfo.totalNutritionAvailable += num;
			this.sumStockpiledNutritionAvailableNow += num;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00043A14 File Offset: 0x00041C14
		private void ProcessAnimal(Pawn animal)
		{
			if (!MapPlantGrowthRateCalculator.IsPastureAnimal(animal.def) || !animal.Spawned)
			{
				return;
			}
			PenFoodCalculator.PenAnimalInfo animalInfo = this.GetAnimalInfo(animal.def);
			animalInfo.count++;
			animalInfo.nutritionConsumptionPerDay += SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal);
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00043A64 File Offset: 0x00041C64
		public static string NutritionToString(float value, bool withUnits = true)
		{
			string text = value.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
			if (withUnits)
			{
				return text + " " + "PenFoodTab_Nutrition_Unit".Translate();
			}
			return text;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00043AA0 File Offset: 0x00041CA0
		public static string NutritionPerDayToString(float value, bool withUnits = true)
		{
			string text = value.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
			if (withUnits)
			{
				return text + " " + "PenFoodTab_NutritionPerDay_Unit".Translate();
			}
			return text;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00043ADA File Offset: 0x00041CDA
		public float CapacityOf(Quadrum q, ThingDef animal)
		{
			return this.nutritionPerDayPerQuadrum.ForQuadrum(q) / SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00043AF0 File Offset: 0x00041CF0
		public Quadrum GetSummerOrBestQuadrum()
		{
			Vector2 location = Find.WorldGrid.LongLatOf(this.mapCalc.tile);
			Quadrum? quadrum = null;
			float num = 0f;
			foreach (Quadrum quadrum2 in QuadrumUtility.Quadrums)
			{
				if (quadrum2.GetSeason(location) == Season.Summer)
				{
					return quadrum2;
				}
				float num2 = this.nutritionPerDayPerQuadrum.ForQuadrum(quadrum2);
				if (quadrum == null || num2 > num)
				{
					quadrum = new Quadrum?(quadrum2);
					num = num2;
				}
			}
			return quadrum.Value;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00043BA4 File Offset: 0x00041DA4
		public string PenSizeDescription()
		{
			string result;
			if (this.Unenclosed)
			{
				result = "PenSizeDesc_Unenclosed".Translate();
			}
			else if (this.numCellsSoil < 50)
			{
				result = "PenSizeDesc_VerySmall".Translate();
			}
			else if (this.numCellsSoil < 100)
			{
				result = "PenSizeDesc_Small".Translate();
			}
			else if (this.numCellsSoil < 400)
			{
				result = "PenSizeDesc_Medium".Translate();
			}
			else
			{
				result = "PenSizeDesc_Large".Translate();
			}
			return result;
		}

		// Token: 0x04000B19 RID: 2841
		public const ToStringStyle NutritionStringStyle = ToStringStyle.FloatMaxTwo;

		// Token: 0x04000B1A RID: 2842
		private AnimalPenConnectedDistrictsCalculator connectedDistrictsCalc = new AnimalPenConnectedDistrictsCalculator();

		// Token: 0x04000B1B RID: 2843
		private AnimalPenBlueprintEnclosureCalculator blueprintEnclosureCalc = new AnimalPenBlueprintEnclosureCalculator();

		// Token: 0x04000B1C RID: 2844
		private MapPastureNutritionCalculator mapCalc = new MapPastureNutritionCalculator();

		// Token: 0x04000B1D RID: 2845
		private List<PenFoodCalculator.PenAnimalInfo> animals = new List<PenFoodCalculator.PenAnimalInfo>();

		// Token: 0x04000B1E RID: 2846
		private List<PenFoodCalculator.PenFoodItemInfo> stockpiled = new List<PenFoodCalculator.PenFoodItemInfo>();

		// Token: 0x04000B1F RID: 2847
		private string cachedNaturalGrowthRateTooltip;

		// Token: 0x04000B20 RID: 2848
		private string cachedTotalConsumedTooltip;

		// Token: 0x04000B21 RID: 2849
		private string cachedStockpileTooltip;

		// Token: 0x04000B22 RID: 2850
		public MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum = new MapPastureNutritionCalculator.NutritionPerDayPerQuadrum();

		// Token: 0x04000B23 RID: 2851
		public float sumStockpiledNutritionAvailableNow;

		// Token: 0x04000B24 RID: 2852
		public int numCells;

		// Token: 0x04000B25 RID: 2853
		public int numCellsSoil;

		// Token: 0x04000B26 RID: 2854
		private float sumNutritionPerDayToday;

		// Token: 0x04000B27 RID: 2855
		private List<PenFoodCalculator.PenAnimalInfo> tmpAddedExampleAnimals = new List<PenFoodCalculator.PenAnimalInfo>();

		// Token: 0x02001D45 RID: 7493
		public class PenAnimalInfo
		{
			// Token: 0x17001E2C RID: 7724
			// (get) Token: 0x0600B3CE RID: 46030 RVA: 0x0040EFA0 File Offset: 0x0040D1A0
			public int TotalCount
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x17001E2D RID: 7725
			// (get) Token: 0x0600B3CF RID: 46031 RVA: 0x0040EFA8 File Offset: 0x0040D1A8
			public float TotalNutritionConsumptionPerDay
			{
				get
				{
					return this.nutritionConsumptionPerDay;
				}
			}

			// Token: 0x0600B3D0 RID: 46032 RVA: 0x0040EFB0 File Offset: 0x0040D1B0
			public PenAnimalInfo(ThingDef animalDef)
			{
				this.animalDef = animalDef;
			}

			// Token: 0x0600B3D1 RID: 46033 RVA: 0x0040EFC0 File Offset: 0x0040D1C0
			public string ToolTip(PenFoodCalculator calc)
			{
				if (this.cachedToolTip == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					int value = Mathf.FloorToInt(calc.NutritionPerDayToday / SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(this.animalDef));
					stringBuilder.Append("PenFoodTab_AnimalTypeAnimalCapacity".Translate()).Append(": ").Append(value).AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("PenFoodTab_NutritionConsumedPerDay".Translate(this.animalDef.Named("ANIMALDEF"))).AppendLine(":");
					List<LifeStageAge> lifeStageAges = this.animalDef.race.lifeStageAges;
					for (int i = 0; i < lifeStageAges.Count; i++)
					{
						LifeStageDef def = lifeStageAges[i].def;
						float value2 = SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(this.animalDef, def);
						stringBuilder.Append("- ").Append(def.LabelCap).Append(": ").AppendLine(PenFoodCalculator.NutritionPerDayToString(value2, false));
					}
					this.cachedToolTip = stringBuilder.ToString();
				}
				return this.cachedToolTip;
			}

			// Token: 0x0400739C RID: 29596
			public ThingDef animalDef;

			// Token: 0x0400739D RID: 29597
			public bool example;

			// Token: 0x0400739E RID: 29598
			public int count;

			// Token: 0x0400739F RID: 29599
			public float nutritionConsumptionPerDay;

			// Token: 0x040073A0 RID: 29600
			private string cachedToolTip;
		}

		// Token: 0x02001D46 RID: 7494
		public class PenFoodItemInfo
		{
			// Token: 0x0600B3D2 RID: 46034 RVA: 0x0040F0DD File Offset: 0x0040D2DD
			public PenFoodItemInfo(ThingDef itemDef)
			{
				this.itemDef = itemDef;
			}

			// Token: 0x040073A1 RID: 29601
			public ThingDef itemDef;

			// Token: 0x040073A2 RID: 29602
			public int totalCount;

			// Token: 0x040073A3 RID: 29603
			public float totalNutritionAvailable;
		}
	}
}

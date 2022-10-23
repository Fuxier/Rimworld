using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001AD RID: 429
	public static class SimplifiedPastureNutritionSimulator
	{
		// Token: 0x06000BEA RID: 3050 RVA: 0x00043140 File Offset: 0x00041340
		public static float NutritionProducedPerDay(BiomeDef biome, ThingDef plantDef, float averageGrowthRate, float mapRespawnChance)
		{
			if (Mathf.Approximately(averageGrowthRate, 0f))
			{
				return 0f;
			}
			float num = biome.wildPlantRegrowDays / mapRespawnChance;
			float num2 = plantDef.plant.growDays / averageGrowthRate * plantDef.plant.harvestMinGrowth;
			return plantDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * PlantUtility.NutritionFactorFromGrowth(plantDef, plantDef.plant.harvestMinGrowth) / (num + num2) * biome.CommonalityPctOfPlant(plantDef) * 0.85f;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x000431B3 File Offset: 0x000413B3
		public static float NutritionConsumedPerDay(Pawn animal)
		{
			return SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal.def, animal.ageTracker.CurLifeStage);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x000431CC File Offset: 0x000413CC
		public static float NutritionConsumedPerDay(ThingDef animalDef)
		{
			LifeStageAge lifeStageAge = animalDef.race.lifeStageAges.Last<LifeStageAge>();
			return SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animalDef, lifeStageAge.def);
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x000431F6 File Offset: 0x000413F6
		public static float NutritionConsumedPerDay(ThingDef animalDef, LifeStageDef lifeStageDef)
		{
			return Need_Food.BaseHungerRate(lifeStageDef, animalDef) * 60000f;
		}

		// Token: 0x04000B18 RID: 2840
		public const float UnderEstimateNutritionFactor = 0.85f;
	}
}

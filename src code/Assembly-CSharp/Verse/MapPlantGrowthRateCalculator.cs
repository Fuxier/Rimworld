using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001AC RID: 428
	public class MapPlantGrowthRateCalculator
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x00042CA7 File Offset: 0x00040EA7
		public List<TerrainDef> TerrainDefs
		{
			get
			{
				this.ComputeIfDirty();
				return this.terrainDefs;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x00042CB5 File Offset: 0x00040EB5
		public List<ThingDef> WildGrazingPlants
		{
			get
			{
				this.ComputeIfDirty();
				return this.wildGrazingPlants;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x00042CC3 File Offset: 0x00040EC3
		public List<ThingDef> GrazingAnimals
		{
			get
			{
				this.ComputeIfDirty();
				return this.includeAnimalTypes;
			}
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00042CD1 File Offset: 0x00040ED1
		public void BuildFor(int tile)
		{
			this.tile = tile;
			this.longLat = Find.WorldGrid.LongLatOf(tile);
			this.biome = Find.WorldGrid[tile].biome;
			this.ComputeIfDirty();
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00042D08 File Offset: 0x00040F08
		public float GrowthRateForDay(int nowTicks, ThingDef plantDef, TerrainDef terrainDef)
		{
			this.ComputeIfDirty();
			int index = nowTicks / 60000 % 60;
			return MapPlantGrowthRateCalculator.ComputeGrowthRate(plantDef, terrainDef, this.dailyGrowthRates[index]);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00042D39 File Offset: 0x00040F39
		public float QuadrumGrowthRateFor(Quadrum quadrum, ThingDef plantDef, TerrainDef terrainDef)
		{
			this.ComputeIfDirty();
			return MapPlantGrowthRateCalculator.ComputeGrowthRate(plantDef, terrainDef, this.seasonalGrowthRates[quadrum]);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00042D54 File Offset: 0x00040F54
		private static float ComputeGrowthRate(ThingDef plantDef, TerrainDef terrainDef, MapPlantGrowthRateCalculator.PlantGrowthRates rates)
		{
			if (terrainDef.fertility < plantDef.plant.fertilityMin)
			{
				return 0f;
			}
			MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRateAccumulator = rates.For(plantDef);
			return PlantUtility.GrowthRateFactorFor_Fertility(plantDef, terrainDef.fertility) * growthRateAccumulator.GrowthRateForTemperature * growthRateAccumulator.GrowthRateForGlow;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00042D9C File Offset: 0x00040F9C
		private void ComputeIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.includeAnimalTypes.Clear();
			this.seasonalGrowthRates.Clear();
			this.dailyGrowthRates.Clear();
			this.seasonalGrowthRates.Add(Quadrum.Aprimay, new MapPlantGrowthRateCalculator.PlantGrowthRates());
			this.seasonalGrowthRates.Add(Quadrum.Decembary, new MapPlantGrowthRateCalculator.PlantGrowthRates());
			this.seasonalGrowthRates.Add(Quadrum.Jugust, new MapPlantGrowthRateCalculator.PlantGrowthRates());
			this.seasonalGrowthRates.Add(Quadrum.Septober, new MapPlantGrowthRateCalculator.PlantGrowthRates());
			this.AddIncludedAnimals();
			this.terrainDefs = DefDatabase<TerrainDef>.AllDefsListForReading;
			this.wildGrazingPlants = (from plantDef in this.biome.AllWildPlants
			where MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(plantDef)
			select plantDef).ToList<ThingDef>();
			this.CalculateDailyFertility();
			this.CalculateSeasonalFertility();
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00042E78 File Offset: 0x00041078
		private void CalculateDailyFertility()
		{
			for (int i = 0; i < 60; i++)
			{
				int num = i * 60000;
				int nowTicks = num - num % 60000;
				MapPlantGrowthRateCalculator.PlantGrowthRates plantGrowthRates = new MapPlantGrowthRateCalculator.PlantGrowthRates();
				this.dailyGrowthRates.Add(plantGrowthRates);
				foreach (ThingDef plantDef in this.wildGrazingPlants)
				{
					this.SimulateGrowthRateForDay(nowTicks, plantGrowthRates.For(plantDef));
				}
			}
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00042F04 File Offset: 0x00041104
		private void CalculateSeasonalFertility()
		{
			for (int i = 0; i < this.dailyGrowthRates.Count; i++)
			{
				Quadrum key = GenDate.Quadrum((long)(i * 60000), this.longLat.x);
				MapPlantGrowthRateCalculator.PlantGrowthRates plantGrowthRates = this.seasonalGrowthRates[key];
				foreach (KeyValuePair<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator> keyValuePair in this.dailyGrowthRates[i].byPlant)
				{
					plantGrowthRates.For(keyValuePair.Value.plantDef).Accumulate(keyValuePair.Value);
				}
			}
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00042FBC File Offset: 0x000411BC
		private void AddIncludedAnimals()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (MapPlantGrowthRateCalculator.IsPastureAnimal(thingDef))
				{
					this.includeAnimalTypes.Add(thingDef);
				}
			}
			this.includeAnimalTypes.Sort((ThingDef a, ThingDef b) => string.CompareOrdinal(a.label, b.label));
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00043040 File Offset: 0x00041240
		public static bool IsPastureAnimal(ThingDef td)
		{
			return td.race != null && td.race.Animal && td.race.Roamer;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00043064 File Offset: 0x00041264
		public static bool IsEdibleByPastureAnimals(ThingDef foodDef)
		{
			return foodDef.ingestible != null && foodDef.ingestible.preferability != FoodPreferability.Undefined && (FoodTypeFlags.VegetarianRoughAnimal & foodDef.ingestible.foodType) > FoodTypeFlags.None;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00043094 File Offset: 0x00041294
		private void SimulateGrowthRateForDay(int nowTicks, MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRates)
		{
			int num = nowTicks - nowTicks % 60000;
			int num2 = 24;
			int num3 = 60000 / num2;
			for (int i = 0; i < num2; i++)
			{
				int num4 = num + i * num3;
				float cellTemp = Find.World.tileTemperatures.OutdoorTemperatureAt(this.tile, num4);
				float glow = GenCelestial.CelestialSunGlow(this.tile, num4);
				float grTemp = PlantUtility.GrowthRateFactorFor_Temperature(cellTemp);
				float grGlow = PlantUtility.GrowthRateFactorFor_Light(growthRates.plantDef, glow);
				growthRates.Accumulate(grTemp, grGlow);
			}
		}

		// Token: 0x04000B0F RID: 2831
		private Vector2 longLat;

		// Token: 0x04000B10 RID: 2832
		private int tile;

		// Token: 0x04000B11 RID: 2833
		private BiomeDef biome;

		// Token: 0x04000B12 RID: 2834
		private bool dirty = true;

		// Token: 0x04000B13 RID: 2835
		private readonly List<ThingDef> includeAnimalTypes = new List<ThingDef>();

		// Token: 0x04000B14 RID: 2836
		private List<TerrainDef> terrainDefs;

		// Token: 0x04000B15 RID: 2837
		private List<ThingDef> wildGrazingPlants;

		// Token: 0x04000B16 RID: 2838
		private readonly Dictionary<Quadrum, MapPlantGrowthRateCalculator.PlantGrowthRates> seasonalGrowthRates = new Dictionary<Quadrum, MapPlantGrowthRateCalculator.PlantGrowthRates>();

		// Token: 0x04000B17 RID: 2839
		private readonly List<MapPlantGrowthRateCalculator.PlantGrowthRates> dailyGrowthRates = new List<MapPlantGrowthRateCalculator.PlantGrowthRates>();

		// Token: 0x02001D42 RID: 7490
		private class GrowthRateAccumulator
		{
			// Token: 0x0600B3C3 RID: 46019 RVA: 0x0040EE81 File Offset: 0x0040D081
			public GrowthRateAccumulator(ThingDef plantDef)
			{
				this.plantDef = plantDef;
			}

			// Token: 0x17001E2A RID: 7722
			// (get) Token: 0x0600B3C4 RID: 46020 RVA: 0x0040EE90 File Offset: 0x0040D090
			public float GrowthRateForTemperature
			{
				get
				{
					if (this.numSamples != 0)
					{
						return this.sumGrowthRateForTemperature / (float)this.numSamples;
					}
					return 0f;
				}
			}

			// Token: 0x17001E2B RID: 7723
			// (get) Token: 0x0600B3C5 RID: 46021 RVA: 0x0040EEAE File Offset: 0x0040D0AE
			public float GrowthRateForGlow
			{
				get
				{
					if (this.numSamples != 0)
					{
						return this.sumGrowthRateForGlow / (float)this.numSamples;
					}
					return 0f;
				}
			}

			// Token: 0x0600B3C6 RID: 46022 RVA: 0x0040EECC File Offset: 0x0040D0CC
			public void Accumulate(float grTemp, float grGlow)
			{
				this.sumGrowthRateForTemperature += grTemp;
				this.sumGrowthRateForGlow += grGlow;
				this.numSamples++;
			}

			// Token: 0x0600B3C7 RID: 46023 RVA: 0x0040EEF8 File Offset: 0x0040D0F8
			public void Accumulate(MapPlantGrowthRateCalculator.GrowthRateAccumulator other)
			{
				this.sumGrowthRateForTemperature += other.sumGrowthRateForTemperature;
				this.sumGrowthRateForGlow += other.sumGrowthRateForGlow;
				this.numSamples += other.numSamples;
			}

			// Token: 0x04007394 RID: 29588
			public readonly ThingDef plantDef;

			// Token: 0x04007395 RID: 29589
			private float sumGrowthRateForTemperature;

			// Token: 0x04007396 RID: 29590
			private float sumGrowthRateForGlow;

			// Token: 0x04007397 RID: 29591
			private int numSamples;
		}

		// Token: 0x02001D43 RID: 7491
		private class PlantGrowthRates
		{
			// Token: 0x0600B3C8 RID: 46024 RVA: 0x0040EF34 File Offset: 0x0040D134
			public MapPlantGrowthRateCalculator.GrowthRateAccumulator For(ThingDef plantDef)
			{
				MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRateAccumulator;
				if (!this.byPlant.TryGetValue(plantDef, out growthRateAccumulator))
				{
					growthRateAccumulator = new MapPlantGrowthRateCalculator.GrowthRateAccumulator(plantDef);
					this.byPlant.Add(plantDef, growthRateAccumulator);
				}
				return growthRateAccumulator;
			}

			// Token: 0x04007398 RID: 29592
			public Dictionary<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator> byPlant = new Dictionary<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator>();
		}
	}
}

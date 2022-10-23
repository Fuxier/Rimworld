using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001AB RID: 427
	public class MapPastureNutritionCalculator
	{
		// Token: 0x06000BD1 RID: 3025 RVA: 0x00042990 File Offset: 0x00040B90
		public void Reset(Map map)
		{
			this.Reset(map.Tile, map.wildPlantSpawner.CachedChanceFromDensity, map.plantGrowthRateCalculator);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000429B0 File Offset: 0x00040BB0
		public void Reset(int tile, float newMapChanceRegrowth, MapPlantGrowthRateCalculator growthRateCalculator)
		{
			newMapChanceRegrowth = (float)Math.Round((double)newMapChanceRegrowth, 7);
			if (this.tile == tile && Mathf.Approximately(this.mapChanceRegrowth, newMapChanceRegrowth))
			{
				return;
			}
			this.tile = tile;
			this.biome = Find.WorldGrid[tile].biome;
			this.mapChanceRegrowth = newMapChanceRegrowth;
			this.plantGrowthRateCalculator = growthRateCalculator;
			this.cachedSeasonalDetailed.Clear();
			this.cachedSeasonalByTerrain.Clear();
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00042A24 File Offset: 0x00040C24
		public MapPastureNutritionCalculator.NutritionPerDayPerQuadrum CalculateAverageNutritionPerDay(TerrainDef terrain)
		{
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;
			if (!this.cachedSeasonalByTerrain.TryGetValue(terrain, out nutritionPerDayPerQuadrum))
			{
				nutritionPerDayPerQuadrum = new MapPastureNutritionCalculator.NutritionPerDayPerQuadrum();
				foreach (ThingDef plantDef in this.plantGrowthRateCalculator.WildGrazingPlants)
				{
					MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other = this.CalculateAverageNutritionPerDay(plantDef, terrain);
					nutritionPerDayPerQuadrum.AddFrom(other);
				}
				this.cachedSeasonalByTerrain.Add(terrain, nutritionPerDayPerQuadrum);
			}
			return nutritionPerDayPerQuadrum;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00042AAC File Offset: 0x00040CAC
		private MapPastureNutritionCalculator.NutritionPerDayPerQuadrum CalculateAverageNutritionPerDay(ThingDef plantDef, TerrainDef terrain)
		{
			Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum> dictionary;
			if (!this.cachedSeasonalDetailed.TryGetValue(plantDef, out dictionary))
			{
				dictionary = new Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>();
				this.cachedSeasonalDetailed.Add(plantDef, dictionary);
			}
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;
			if (!dictionary.TryGetValue(terrain, out nutritionPerDayPerQuadrum))
			{
				nutritionPerDayPerQuadrum = new MapPastureNutritionCalculator.NutritionPerDayPerQuadrum();
				dictionary.Add(terrain, nutritionPerDayPerQuadrum);
				nutritionPerDayPerQuadrum.quadrum[0] = this.GetAverageNutritionPerDay(Quadrum.Aprimay, plantDef, terrain);
				nutritionPerDayPerQuadrum.quadrum[3] = this.GetAverageNutritionPerDay(Quadrum.Decembary, plantDef, terrain);
				nutritionPerDayPerQuadrum.quadrum[1] = this.GetAverageNutritionPerDay(Quadrum.Jugust, plantDef, terrain);
				nutritionPerDayPerQuadrum.quadrum[2] = this.GetAverageNutritionPerDay(Quadrum.Septober, plantDef, terrain);
			}
			return nutritionPerDayPerQuadrum;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00042B3C File Offset: 0x00040D3C
		public float GetAverageNutritionPerDayToday(TerrainDef terrainDef)
		{
			float num = 0f;
			foreach (ThingDef plantDef in this.plantGrowthRateCalculator.WildGrazingPlants)
			{
				num += this.GetAverageNutritionPerDayToday(plantDef, terrainDef);
			}
			return num;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00042BA0 File Offset: 0x00040DA0
		private float GetAverageNutritionPerDayToday(ThingDef plantDef, TerrainDef terrainDef)
		{
			if (terrainDef.fertility <= 0f)
			{
				return 0f;
			}
			int ticksAbs = Find.TickManager.TicksAbs;
			int nowTicks = ticksAbs - ticksAbs % 60000;
			float growthRate = this.plantGrowthRateCalculator.GrowthRateForDay(nowTicks, plantDef, terrainDef);
			return this.ComputeNutritionProducedPerDay(plantDef, growthRate);
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00042BEC File Offset: 0x00040DEC
		public float GetAverageNutritionPerDay(Quadrum quadrum, TerrainDef terrainDef)
		{
			float num = 0f;
			foreach (ThingDef plantDef in this.plantGrowthRateCalculator.WildGrazingPlants)
			{
				num += this.GetAverageNutritionPerDay(quadrum, plantDef, terrainDef);
			}
			return num;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00042C50 File Offset: 0x00040E50
		public float GetAverageNutritionPerDay(Quadrum quadrum, ThingDef plantDef, TerrainDef terrainDef)
		{
			float growthRate = this.plantGrowthRateCalculator.QuadrumGrowthRateFor(quadrum, plantDef, terrainDef);
			return this.ComputeNutritionProducedPerDay(plantDef, growthRate);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00042C74 File Offset: 0x00040E74
		private float ComputeNutritionProducedPerDay(ThingDef plantDef, float growthRate)
		{
			return SimplifiedPastureNutritionSimulator.NutritionProducedPerDay(this.biome, plantDef, growthRate, this.mapChanceRegrowth);
		}

		// Token: 0x04000B09 RID: 2825
		public MapPlantGrowthRateCalculator plantGrowthRateCalculator;

		// Token: 0x04000B0A RID: 2826
		public BiomeDef biome;

		// Token: 0x04000B0B RID: 2827
		public int tile;

		// Token: 0x04000B0C RID: 2828
		public float mapChanceRegrowth;

		// Token: 0x04000B0D RID: 2829
		private Dictionary<ThingDef, Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>> cachedSeasonalDetailed = new Dictionary<ThingDef, Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>>();

		// Token: 0x04000B0E RID: 2830
		private Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum> cachedSeasonalByTerrain = new Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>();

		// Token: 0x02001D41 RID: 7489
		public class NutritionPerDayPerQuadrum
		{
			// Token: 0x0600B3C0 RID: 46016 RVA: 0x0040EDF4 File Offset: 0x0040CFF4
			public float ForQuadrum(Quadrum q)
			{
				return this.quadrum[(int)q];
			}

			// Token: 0x0600B3C1 RID: 46017 RVA: 0x0040EE00 File Offset: 0x0040D000
			public void AddFrom(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other)
			{
				this.quadrum[0] += other.quadrum[0];
				this.quadrum[3] += other.quadrum[3];
				this.quadrum[1] += other.quadrum[1];
				this.quadrum[2] += other.quadrum[2];
			}

			// Token: 0x04007393 RID: 29587
			public float[] quadrum = new float[4];
		}
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x0200022E RID: 558
	public static class PollutionUtility
	{
		// Token: 0x06000FC2 RID: 4034 RVA: 0x0005B7C8 File Offset: 0x000599C8
		public static void PawnPollutionTick(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return;
			}
			if (pawn.IsHashIntervalTick(3451) && pawn.Position.IsPolluted(pawn.Map))
			{
				GameCondition_ToxicFallout.DoPawnToxicDamage(pawn, false, 1f);
			}
			if (pawn.IsHashIntervalTick(60) && pawn.Position.IsPolluted(pawn.Map) && PollutionUtility.StimulatedByPollution(pawn) && !pawn.health.hediffSet.HasHediff(HediffDefOf.PollutionStimulus, false))
			{
				pawn.health.AddHediff(HediffDefOf.PollutionStimulus, null, null, null);
			}
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0005B864 File Offset: 0x00059A64
		public static void Notify_TunnelHiveSpawnedInsect(Pawn pawn)
		{
			if (!pawn.Spawned || !pawn.RaceProps.Insect)
			{
				return;
			}
			if (pawn.Position.IsPolluted(pawn.Map))
			{
				pawn.health.AddHediff(HediffDefOf.PollutionStimulus, null, null, null);
				HealthUtility.AdjustSeverity(pawn, HediffDefOf.PollutionStimulus, HediffDefOf.PollutionStimulus.maxSeverity);
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0005B8CB File Offset: 0x00059ACB
		private static bool StimulatedByPollution(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (pawn.RaceProps.Insect)
			{
				return true;
			}
			Pawn_GeneTracker genes = pawn.genes;
			return ((genes != null) ? genes.GetFirstGeneOfType<Gene_PollutionRush>() : null) != null;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0005B8FC File Offset: 0x00059AFC
		public static bool SettableEntirelyPolluted(IPlantToGrowSettable s)
		{
			using (IEnumerator<IntVec3> enumerator = s.Cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsPolluted(s.Map))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0005B958 File Offset: 0x00059B58
		public static bool CanPlantAt(ThingDef plantDef, IPlantToGrowSettable settable)
		{
			if (plantDef.plant.RequiresNoPollution)
			{
				using (IEnumerator<IntVec3> enumerator = settable.Cells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsPolluted(settable.Map))
						{
							return true;
						}
					}
				}
				return false;
			}
			if (plantDef.plant.RequiresPollution)
			{
				using (IEnumerator<IntVec3> enumerator = settable.Cells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsPolluted(settable.Map))
						{
							return true;
						}
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0005BA14 File Offset: 0x00059C14
		public static void GrowPollutionAt(IntVec3 root, Map map, int cellCountToPollute = 4, Action<IntVec3> onPolluteAction = null, bool silent = false, Func<IntVec3, bool> adjacentPolluteValidator = null)
		{
			PollutionUtility.<>c__DisplayClass9_0 CS$<>8__locals1 = new PollutionUtility.<>c__DisplayClass9_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.adjacentPolluteValidator = adjacentPolluteValidator;
			if (cellCountToPollute < 0)
			{
				Log.Error("Non-positive max cells value passed in PolluteClosestMapTerrain");
				return;
			}
			if (CS$<>8__locals1.map.pollutionGrid.TotalPollution >= CS$<>8__locals1.map.cellIndices.NumGridCells)
			{
				return;
			}
			if (root.CanPollute(CS$<>8__locals1.map))
			{
				root.Pollute(CS$<>8__locals1.map, false);
				cellCountToPollute--;
				if (onPolluteAction != null)
				{
					onPolluteAction(root);
				}
			}
			if (cellCountToPollute <= 0)
			{
				return;
			}
			PollutionUtility.tmpPollutableCells = new FastPriorityQueue<IntVec3>(new PollutionUtility.PollutionCellComparer(root, CS$<>8__locals1.map, 0.015f));
			CS$<>8__locals1.map.floodFiller.FloodFill(root, (IntVec3 x) => x.IsPolluted(CS$<>8__locals1.map), delegate(IntVec3 x)
			{
				PollutionUtility.tmpPollutedCells.Add(x);
			}, int.MaxValue, false, null);
			if (PollutionUtility.tmpPollutedCells.Count == 0)
			{
				return;
			}
			PollutionUtility.tmpPollutableCells.Clear();
			for (int i = 0; i < PollutionUtility.tmpPollutedCells.Count; i++)
			{
				foreach (IntVec3 item in CS$<>8__locals1.<GrowPollutionAt>g__GetAdjacentPollutableCells|0(PollutionUtility.tmpPollutedCells[i], CS$<>8__locals1.map))
				{
					if (!PollutionUtility.tmpPollutableCells.Contains(item))
					{
						PollutionUtility.tmpPollutableCells.Push(item);
					}
				}
			}
			PollutionUtility.tmpPollutedCells.Clear();
			while (cellCountToPollute > 0 && PollutionUtility.tmpPollutableCells.Count > 0)
			{
				IntVec3 intVec = PollutionUtility.tmpPollutableCells.Pop();
				CS$<>8__locals1.map.pollutionGrid.SetPolluted(intVec, true, silent);
				if (onPolluteAction != null)
				{
					onPolluteAction(intVec);
				}
				foreach (IntVec3 item2 in CS$<>8__locals1.<GrowPollutionAt>g__GetAdjacentPollutableCells|0(intVec, CS$<>8__locals1.map))
				{
					if (!PollutionUtility.tmpPollutableCells.Contains(item2))
					{
						PollutionUtility.tmpPollutableCells.Push(item2);
					}
				}
				cellCountToPollute--;
			}
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0005BC2C File Offset: 0x00059E2C
		public static PollutionLevel PollutionLevel(this Tile tile)
		{
			if (tile.pollution < 0.25f)
			{
				return Verse.PollutionLevel.None;
			}
			if (tile.pollution < 0.5f)
			{
				return Verse.PollutionLevel.Light;
			}
			if (tile.pollution < 0.75f)
			{
				return Verse.PollutionLevel.Moderate;
			}
			return Verse.PollutionLevel.Extreme;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0005BC5C File Offset: 0x00059E5C
		public static void PolluteMapToPercent(Map map, float mapPollutionPercent, int globSize = 1000)
		{
			PollutionUtility.<>c__DisplayClass12_0 CS$<>8__locals1 = new PollutionUtility.<>c__DisplayClass12_0();
			CS$<>8__locals1.map = map;
			mapPollutionPercent = Mathf.Clamp01(mapPollutionPercent);
			float totalPollutionPercent = CS$<>8__locals1.map.pollutionGrid.TotalPollutionPercent;
			if (mapPollutionPercent == totalPollutionPercent)
			{
				return;
			}
			List<IntVec3> allPollutableCells = CS$<>8__locals1.map.pollutionGrid.AllPollutableCells;
			if (mapPollutionPercent < totalPollutionPercent)
			{
				PollutionUtility.tmpMapCells.Clear();
				PollutionUtility.tmpMapCells.AddRange(allPollutableCells.InRandomOrder(null));
				float num = totalPollutionPercent - mapPollutionPercent;
				float num2 = Mathf.Max(0f, (float)PollutionUtility.tmpMapCells.Count * num);
				int num3 = 0;
				while ((float)num3 < num2)
				{
					PollutionUtility.tmpMapCells[num3].Unpollute(CS$<>8__locals1.map);
					num3++;
				}
				PollutionUtility.tmpMapCells.Clear();
				return;
			}
			int num4 = Mathf.FloorToInt((float)allPollutableCells.Count * mapPollutionPercent);
			int num5 = Mathf.CeilToInt((float)(num4 / globSize));
			for (int i = 0; i < num5; i++)
			{
				int num6 = Mathf.Min(globSize, num4);
				PollutionUtility.GrowPollutionAt(allPollutableCells.RandomElementByWeight(new Func<IntVec3, float>(CS$<>8__locals1.<PolluteMapToPercent>g__GlobCellSelectionWeight|0)), CS$<>8__locals1.map, num6, null, false, null);
				num4 -= num6;
			}
		}

		// Token: 0x04000E06 RID: 3590
		private const float PollutionPerlinFrequency = 0.015f;

		// Token: 0x04000E07 RID: 3591
		private static FastPriorityQueue<IntVec3> tmpPollutableCells;

		// Token: 0x04000E08 RID: 3592
		private static List<IntVec3> tmpPollutedCells = new List<IntVec3>();

		// Token: 0x04000E09 RID: 3593
		private static List<IntVec3> tmpMapCells = new List<IntVec3>();

		// Token: 0x02001D89 RID: 7561
		internal class PollutionCellComparer : IComparer<IntVec3>
		{
			// Token: 0x0600B4C8 RID: 46280 RVA: 0x004119C0 File Offset: 0x0040FBC0
			public PollutionCellComparer(IntVec3 root, Map map, float frequency = 0.015f)
			{
				this.root = root;
				this.map = map;
				this.perlin = new Perlin((double)frequency, 2.0, 0.5, 6, map.uniqueID, QualityMode.Medium);
				this.perlin = new ScaleBias(0.5, 0.5, this.perlin);
			}

			// Token: 0x0600B4C9 RID: 46281 RVA: 0x00411A2C File Offset: 0x0040FC2C
			private float PolluteScore(IntVec3 c)
			{
				float num = 1f;
				num *= 1f / c.DistanceTo(this.root);
				num *= 1f + (float)this.perlin.GetValue((double)c.x, (double)c.y, (double)c.z) * 2f;
				if (MapGenerator.mapBeingGenerated == this.map)
				{
					num *= c.DistanceTo(MapGenerator.PlayerStartSpot) / this.map.Size.LengthHorizontal;
				}
				return num * (1f + (float)this.AdjacentPollutedCount(c) / 8f * 0.25f);
			}

			// Token: 0x0600B4CA RID: 46282 RVA: 0x00411AD4 File Offset: 0x0040FCD4
			private int AdjacentPollutedCount(IntVec3 c)
			{
				int num = 0;
				for (int i = 0; i < GenAdj.AdjacentCells.Length; i++)
				{
					IntVec3 b = GenAdj.AdjacentCells[i];
					IntVec3 c2 = c + b;
					if (c2.InBounds(this.map) && c2.IsPolluted(this.map))
					{
						num++;
					}
				}
				return num;
			}

			// Token: 0x0600B4CB RID: 46283 RVA: 0x00411B2C File Offset: 0x0040FD2C
			public int Compare(IntVec3 a, IntVec3 b)
			{
				float num = this.PolluteScore(a);
				float num2 = this.PolluteScore(b);
				if (num < num2)
				{
					return 1;
				}
				if (num > num2)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x04007490 RID: 29840
			private const float NoisyEdgeFactor = 0.25f;

			// Token: 0x04007491 RID: 29841
			private const float PerlinNoiseFactor = 2f;

			// Token: 0x04007492 RID: 29842
			private IntVec3 root;

			// Token: 0x04007493 RID: 29843
			private Map map;

			// Token: 0x04007494 RID: 29844
			private ModuleBase perlin;
		}
	}
}

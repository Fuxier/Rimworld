using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000230 RID: 560
	public static class WorldPollutionUtility
	{
		// Token: 0x06000FCD RID: 4045 RVA: 0x0005BE24 File Offset: 0x0005A024
		public static void PolluteWorldAtTile(int root, float pollutionAmount)
		{
			if (root == -1)
			{
				return;
			}
			WorldPollutionUtility.tmpSeenFactions.Clear();
			int num = WorldPollutionUtility.FindBestTileToPollute(root);
			if (num == -1)
			{
				return;
			}
			Tile tile = Find.WorldGrid[num];
			float pollution = tile.pollution;
			float num2 = tile.pollution + pollutionAmount;
			float num3 = num2 - 1f;
			tile.pollution = Mathf.Clamp01(num2);
			MapParent mapParent = Find.WorldObjects.MapParentAt(num);
			if (mapParent == null || !mapParent.HasMap)
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(num);
				string value = vector.y.ToStringLatitude() + " / " + vector.x.ToStringLongitude();
				Messages.Message("MessageWorldTilePollutionChanged".Translate(pollutionAmount.ToStringPercent(), value), new LookTargets(num), MessageTypeDefOf.NegativeEvent, false);
			}
			Map map = Current.Game.FindMap(num);
			if (map != null)
			{
				PollutionUtility.PolluteMapToPercent(map, tile.pollution, 1000);
			}
			Find.World.renderer.Notify_TilePollutionChanged(num);
			WorldPollutionUtility.tmpSeenFactions.Clear();
			if (num3 > 0f)
			{
				WorldPollutionUtility.PolluteWorldAtTile(num, num3);
			}
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0005BF48 File Offset: 0x0005A148
		public static int FindBestTileToPollute(int root)
		{
			WorldPollutionUtility.<>c__DisplayClass6_0 CS$<>8__locals1 = new WorldPollutionUtility.<>c__DisplayClass6_0();
			CS$<>8__locals1.root = root;
			if (CS$<>8__locals1.root == -1)
			{
				return -1;
			}
			World world = Find.World;
			CS$<>8__locals1.grid = world.grid;
			Tile tile = CS$<>8__locals1.grid[CS$<>8__locals1.root];
			if (CS$<>8__locals1.<FindBestTileToPollute>g__CanPollute|0(CS$<>8__locals1.root))
			{
				return CS$<>8__locals1.root;
			}
			WorldPollutionUtility.tmpPossiblePollutableTiles.Clear();
			CS$<>8__locals1.bestDistance = int.MaxValue;
			Find.WorldFloodFiller.FloodFill(CS$<>8__locals1.root, (int x) => !base.<FindBestTileToPollute>g__CanPollute|0(x), delegate(int t, int d)
			{
				WorldPollutionUtility.tmpTileNeighbors.Clear();
				CS$<>8__locals1.grid.GetTileNeighbors(t, WorldPollutionUtility.tmpTileNeighbors);
				for (int i = 0; i < WorldPollutionUtility.tmpTileNeighbors.Count; i++)
				{
					if (base.<FindBestTileToPollute>g__CanPollute|0(WorldPollutionUtility.tmpTileNeighbors[i]) && !WorldPollutionUtility.tmpPossiblePollutableTiles.Contains(WorldPollutionUtility.tmpTileNeighbors[i]))
					{
						int num = Mathf.RoundToInt(CS$<>8__locals1.grid.ApproxDistanceInTiles(CS$<>8__locals1.root, WorldPollutionUtility.tmpTileNeighbors[i]));
						if (num <= CS$<>8__locals1.bestDistance)
						{
							CS$<>8__locals1.bestDistance = num;
							WorldPollutionUtility.tmpPossiblePollutableTiles.Add(WorldPollutionUtility.tmpTileNeighbors[i]);
							List<int> list = WorldPollutionUtility.tmpPossiblePollutableTiles;
							Predicate<int> match;
							if ((match = CS$<>8__locals1.<>9__6) == null)
							{
								match = (CS$<>8__locals1.<>9__6 = ((int u) => Mathf.RoundToInt(CS$<>8__locals1.grid.ApproxDistanceInTiles(CS$<>8__locals1.root, u)) > CS$<>8__locals1.bestDistance));
							}
							list.RemoveAll(match);
						}
					}
				}
				return false;
			}, int.MaxValue, null);
			CS$<>8__locals1.found = (from t in WorldPollutionUtility.tmpPossiblePollutableTiles
			orderby CS$<>8__locals1.grid[t].PollutionLevel(), CS$<>8__locals1.grid[t].pollution descending
			select t).FirstOrFallback(-1);
			WorldPollutionUtility.tmpPossiblePollutableTiles.RemoveAll((int t) => CS$<>8__locals1.grid[t].PollutionLevel() > CS$<>8__locals1.grid[CS$<>8__locals1.found].PollutionLevel() && CS$<>8__locals1.grid[t].pollution < CS$<>8__locals1.grid[CS$<>8__locals1.found].pollution);
			CS$<>8__locals1.found = WorldPollutionUtility.tmpPossiblePollutableTiles.RandomElement<int>();
			WorldPollutionUtility.tmpPossiblePollutableTiles.Clear();
			WorldPollutionUtility.tmpTileNeighbors.Clear();
			return CS$<>8__locals1.found;
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0005C060 File Offset: 0x0005A260
		public static float CalculateNearbyPollutionScore(int tileId)
		{
			int maxTilesToProcess = Find.WorldGrid.TilesNumWithinTraversalDistance(4);
			float nearbyPollutionScore = 0f;
			Find.WorldFloodFiller.FloodFill(tileId, (int x) => true, delegate(int tile, int dist)
			{
				nearbyPollutionScore += WorldPollutionUtility.NearbyPollutionOverDistanceCurve.Evaluate((float)Mathf.RoundToInt((float)dist)) * Find.WorldGrid[tile].pollution;
				return false;
			}, maxTilesToProcess, null);
			return nearbyPollutionScore;
		}

		// Token: 0x04000E0B RID: 3595
		public const int NearbyPollutionTileRadius = 4;

		// Token: 0x04000E0C RID: 3596
		public static readonly SimpleCurve NearbyPollutionOverDistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1f),
				true
			},
			{
				new CurvePoint(3f, 0.5f),
				true
			},
			{
				new CurvePoint(4f, 0.5f),
				true
			}
		};

		// Token: 0x04000E0D RID: 3597
		private static HashSet<Faction> tmpSeenFactions = new HashSet<Faction>();

		// Token: 0x04000E0E RID: 3598
		private static List<int> tmpTileNeighbors = new List<int>();

		// Token: 0x04000E0F RID: 3599
		private static List<int> tmpPossiblePollutableTiles = new List<int>();
	}
}

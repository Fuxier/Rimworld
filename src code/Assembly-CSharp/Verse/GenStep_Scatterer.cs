using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000223 RID: 547
	public abstract class GenStep_Scatterer : GenStep
	{
		// Token: 0x06000F91 RID: 3985 RVA: 0x0005AA34 File Offset: 0x00058C34
		public override void Generate(Map map, GenStepParams parms)
		{
			if (this.ShouldSkipMap(map))
			{
				return;
			}
			int num = this.CalculateFinalCount(map);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, 1);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0005AA8C File Offset: 0x00058C8C
		protected virtual bool ShouldSkipMap(Map map)
		{
			return (!this.allowInWaterBiome && map.TileInfo.WaterCovered) || (this.onlyOnStartingMap && Find.GameInfo.startingTile != map.Tile) || (ModsConfig.BiotechActive && map.TileInfo.pollution < this.minPollution) || (ModsConfig.BiotechActive && Find.History.mechanoidDatacoreReadOrLost && !this.allowMechanoidDatacoreReadOrLost);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0005AB08 File Offset: 0x00058D08
		protected virtual bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.nearMapCenter)
			{
				if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => this.CanScatterAt(x, map), map, out result))
				{
					return true;
				}
			}
			else
			{
				if (this.nearPlayerStart)
				{
					result = CellFinder.RandomClosewalkCellNear(MapGenerator.PlayerStartSpot, map, 20, (IntVec3 x) => this.CanScatterAt(x, map));
					return true;
				}
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(5, (IntVec3 x) => this.CanScatterAt(x, map), map, out result))
				{
					return true;
				}
			}
			if (this.warnOnFail)
			{
				Log.Warning("Scatterer " + this.ToString() + " could not find cell to generate at.");
			}
			return false;
		}

		// Token: 0x06000F94 RID: 3988
		protected abstract void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1);

		// Token: 0x06000F95 RID: 3989 RVA: 0x0005ABBC File Offset: 0x00058DBC
		protected virtual bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (this.extraNoBuildEdgeDist > 0 && loc.CloseToEdge(map, this.extraNoBuildEdgeDist + 10))
			{
				return false;
			}
			if (this.minEdgeDist > 0 && loc.CloseToEdge(map, this.minEdgeDist))
			{
				return false;
			}
			if (this.minEdgeDistPct > 0f && loc.CloseToEdge(map, (int)(this.minEdgeDistPct * (float)Mathf.Min(map.Size.x, map.Size.z))))
			{
				return false;
			}
			if (this.NearUsedSpot(loc, this.minSpacing))
			{
				return false;
			}
			if (this.minDistToPlayerStart > 0 && (map.Center - loc).LengthHorizontalSquared < this.minDistToPlayerStart * this.minDistToPlayerStart)
			{
				return false;
			}
			if (this.minDistToPlayerStartPct > 0f && (map.Center - loc).LengthHorizontal < this.minDistToPlayerStartPct * (float)Mathf.Min(map.Size.x, map.Size.z))
			{
				return false;
			}
			if (this.spotMustBeStandable && !loc.Standable(map))
			{
				return false;
			}
			if (!this.allowFoggedPositions && loc.Fogged(map))
			{
				return false;
			}
			if (!this.allowRoofed && loc.Roofed(map))
			{
				return false;
			}
			if (this.validators != null)
			{
				for (int i = 0; i < this.validators.Count; i++)
				{
					if (!this.validators[i].Allows(loc, map))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0005AD34 File Offset: 0x00058F34
		protected bool NearUsedSpot(IntVec3 c, float dist)
		{
			for (int i = 0; i < this.usedSpots.Count; i++)
			{
				if ((float)(this.usedSpots[i] - c).LengthHorizontalSquared <= dist * dist)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0005AD7A File Offset: 0x00058F7A
		protected int CalculateFinalCount(Map map)
		{
			if (this.count < 0)
			{
				return GenStep_Scatterer.CountFromPer10kCells(this.countPer10kCellsRange.RandomInRange, map, -1);
			}
			return this.count;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0005ADA0 File Offset: 0x00058FA0
		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				mapSize = map.Size.x;
			}
			int num = Mathf.RoundToInt(10000f / countPer10kCells);
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0005ADD8 File Offset: 0x00058FD8
		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, default(GenStepParams), 1);
		}

		// Token: 0x04000DDA RID: 3546
		public int count = -1;

		// Token: 0x04000DDB RID: 3547
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x04000DDC RID: 3548
		public bool nearPlayerStart;

		// Token: 0x04000DDD RID: 3549
		public bool nearMapCenter;

		// Token: 0x04000DDE RID: 3550
		public float minSpacing = 10f;

		// Token: 0x04000DDF RID: 3551
		public bool spotMustBeStandable;

		// Token: 0x04000DE0 RID: 3552
		public int minDistToPlayerStart;

		// Token: 0x04000DE1 RID: 3553
		public float minDistToPlayerStartPct;

		// Token: 0x04000DE2 RID: 3554
		public int minEdgeDist;

		// Token: 0x04000DE3 RID: 3555
		public float minEdgeDistPct;

		// Token: 0x04000DE4 RID: 3556
		public int extraNoBuildEdgeDist;

		// Token: 0x04000DE5 RID: 3557
		public List<ScattererValidator> validators = new List<ScattererValidator>();

		// Token: 0x04000DE6 RID: 3558
		public bool allowInWaterBiome = true;

		// Token: 0x04000DE7 RID: 3559
		public bool allowFoggedPositions = true;

		// Token: 0x04000DE8 RID: 3560
		public bool allowRoofed = true;

		// Token: 0x04000DE9 RID: 3561
		public bool onlyOnStartingMap;

		// Token: 0x04000DEA RID: 3562
		public float minPollution;

		// Token: 0x04000DEB RID: 3563
		public bool allowMechanoidDatacoreReadOrLost = true;

		// Token: 0x04000DEC RID: 3564
		public bool warnOnFail = true;

		// Token: 0x04000DED RID: 3565
		[Unsaved(false)]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		// Token: 0x04000DEE RID: 3566
		private const int ScatterNearPlayerRadius = 20;
	}
}

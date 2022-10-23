using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001EB RID: 491
	public sealed class SnowGrid : IExposable
	{
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x0004D103 File Offset: 0x0004B303
		internal float[] DepthGridDirect_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x0004D10B File Offset: 0x0004B30B
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0004D114 File Offset: 0x0004B314
		public SnowGrid(Map map)
		{
			this.map = map;
			this.depthGrid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0004D139 File Offset: 0x0004B339
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SnowGrid.SnowFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SnowGrid.SnowShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0004D163 File Offset: 0x0004B363
		private static ushort SnowFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0004D187 File Offset: 0x0004B387
		private static float SnowShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0004D194 File Offset: 0x0004B394
		private bool CanHaveSnow(int ind)
		{
			Building building = this.map.edificeGrid[ind];
			if (building != null && !SnowGrid.CanCoexistWithSnow(building.def))
			{
				return false;
			}
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(ind);
			return terrainDef == null || terrainDef.holdSnow;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0004D1E5 File Offset: 0x0004B3E5
		public static bool CanCoexistWithSnow(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0004D1FC File Offset: 0x0004B3FC
		public void AddDepth(IntVec3 c, float depthToAdd)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			float num2 = this.depthGrid[num];
			if (num2 <= 0f && depthToAdd < 0f)
			{
				return;
			}
			if (num2 >= 0.999f && depthToAdd > 1f)
			{
				return;
			}
			if (!this.CanHaveSnow(num))
			{
				this.depthGrid[num] = 0f;
				return;
			}
			float num3 = num2 + depthToAdd;
			num3 = Mathf.Clamp(num3, 0f, 1f);
			float num4 = num3 - num2;
			this.totalDepth += (double)num4;
			if (Mathf.Abs(num4) > 0.0001f)
			{
				this.depthGrid[num] = num3;
				this.CheckVisualOrPathCostChange(c, num2, num3);
			}
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0004D2A4 File Offset: 0x0004B4A4
		public void SetDepth(IntVec3 c, float newDepth)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (newDepth > 0f && !this.CanHaveSnow(num))
			{
				newDepth = 0f;
			}
			newDepth = Mathf.Clamp(newDepth, 0f, 1f);
			float num2 = this.depthGrid[num];
			this.depthGrid[num] = newDepth;
			float num3 = newDepth - num2;
			this.totalDepth += (double)num3;
			this.CheckVisualOrPathCostChange(c, num2, newDepth);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0004D31C File Offset: 0x0004B51C
		private void CheckVisualOrPathCostChange(IntVec3 c, float oldDepth, float newDepth)
		{
			if (!Mathf.Approximately(oldDepth, newDepth))
			{
				if (Mathf.Abs(oldDepth - newDepth) > 0.15f || Rand.Value < 0.0125f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Snow, true, false);
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Things, true, false);
				}
				else if (newDepth == 0f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Snow, true, false);
				}
				float num = 0.4f;
				if (c.IsPolluted(this.map) && ((oldDepth > num && newDepth < num) || (oldDepth < num && newDepth > num)))
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Terrain, true, false);
				}
				if (SnowUtility.GetSnowCategory(oldDepth) != SnowUtility.GetSnowCategory(newDepth))
				{
					this.map.pathing.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0004D3F0 File Offset: 0x0004B5F0
		public void MakeMeshDirty(IntVec3 c)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Snow, true, false);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0004D407 File Offset: 0x0004B607
		public float GetDepth(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				return 0f;
			}
			return this.depthGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0004D435 File Offset: 0x0004B635
		public SnowCategory GetCategory(IntVec3 c)
		{
			return SnowUtility.GetSnowCategory(this.GetDepth(c));
		}

		// Token: 0x04000C62 RID: 3170
		private Map map;

		// Token: 0x04000C63 RID: 3171
		private float[] depthGrid;

		// Token: 0x04000C64 RID: 3172
		private double totalDepth;

		// Token: 0x04000C65 RID: 3173
		public const float MaxDepth = 1f;
	}
}

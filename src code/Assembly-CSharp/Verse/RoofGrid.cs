using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000255 RID: 597
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x00063D64 File Offset: 0x00061F64
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3620, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x00063DB5 File Offset: 0x00061FB5
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00063DCB File Offset: 0x00061FCB
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00063DF0 File Offset: 0x00061FF0
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, delegate(IntVec3 c)
			{
				if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null)
				{
					return this.roofGrid[this.map.cellIndices.CellToIndex(c)].shortHash;
				}
				return 0;
			}, delegate(IntVec3 c, ushort val)
			{
				this.SetRoof(c, DefDatabase<RoofDef>.GetByShortHash(val));
			}, "roofs");
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00063E1A File Offset: 0x0006201A
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00063E3C File Offset: 0x0006203C
		public Color GetCellExtraColor(int index)
		{
			if (RoofDefOf.RoofRockThick != null && this.roofGrid[index] == RoofDefOf.RoofRockThick)
			{
				return Color.gray;
			}
			return Color.white;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00063E5F File Offset: 0x0006205F
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00063E6C File Offset: 0x0006206C
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00063E8A File Offset: 0x0006208A
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00063EA7 File Offset: 0x000620A7
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00063EB1 File Offset: 0x000620B1
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00063ECB File Offset: 0x000620CB
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00063EE8 File Offset: 0x000620E8
		public void SetRoof(IntVec3 c, RoofDef def)
		{
			if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] == def)
			{
				return;
			}
			this.roofGrid[this.map.cellIndices.CellToIndex(c)] = def;
			this.map.glowGrid.MarkGlowGridDirty(c);
			Region validRegionAt_NoRebuild = this.map.regionGrid.GetValidRegionAt_NoRebuild(c);
			if (validRegionAt_NoRebuild != null)
			{
				validRegionAt_NoRebuild.District.Notify_RoofChanged();
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Roofs);
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00063F81 File Offset: 0x00062181
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x04000EBD RID: 3773
		private Map map;

		// Token: 0x04000EBE RID: 3774
		private RoofDef[] roofGrid;

		// Token: 0x04000EBF RID: 3775
		private CellBoolDrawer drawerInt;
	}
}

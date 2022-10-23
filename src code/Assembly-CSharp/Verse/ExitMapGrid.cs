using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E4 RID: 484
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x0004B3DC File Offset: 0x000495DC
		public bool MapUsesExitGrid
		{
			get
			{
				if (this.map.IsPlayerHome)
				{
					return false;
				}
				CaravansBattlefield caravansBattlefield = this.map.Parent as CaravansBattlefield;
				if (caravansBattlefield != null && caravansBattlefield.def.blockExitGridUntilBattleIsWon && !caravansBattlefield.WonBattle)
				{
					return false;
				}
				FormCaravanComp component = this.map.Parent.GetComponent<FormCaravanComp>();
				return component == null || !component.CanFormOrReformCaravanNow;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0004B444 File Offset: 0x00049644
		public CellBoolDrawer Drawer
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3690, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x0004B4AD File Offset: 0x000496AD
		public BoolGrid Grid
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				return this.exitMapGrid;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0004B4CD File Offset: 0x000496CD
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0004B4E8 File Offset: 0x000496E8
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0004B4FE File Offset: 0x000496FE
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00020495 File Offset: 0x0001E695
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0004B524 File Offset: 0x00049724
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && this.Grid[c];
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0004B53C File Offset: 0x0004973C
		public void ExitMapGridUpdate()
		{
			if (!this.MapUsesExitGrid)
			{
				return;
			}
			this.Drawer.MarkForDraw();
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0004B55D File Offset: 0x0004975D
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0004B55D File Offset: 0x0004975D
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0004B568 File Offset: 0x00049768
		private void Rebuild()
		{
			this.dirty = false;
			if (this.exitMapGrid == null)
			{
				this.exitMapGrid = new BoolGrid(this.map);
			}
			else
			{
				this.exitMapGrid.Clear();
			}
			CellRect cellRect = CellRect.WholeMap(this.map);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (i > 1 && i < cellRect.maxZ - 2 + 1 && j > 1 && j < cellRect.maxX - 2 + 1)
					{
						j = cellRect.maxX - 2 + 1;
					}
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (this.IsGoodExitCell(intVec))
					{
						this.exitMapGrid[intVec] = true;
					}
				}
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0004B63C File Offset: 0x0004983C
		private bool IsGoodExitCell(IntVec3 cell)
		{
			if (!cell.CanBeSeenOver(this.map))
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map) && intVec.OnEdge(this.map) && intVec.CanBeSeenOverFast(this.map) && GenSight.LineOfSight(cell, intVec, this.map, false, null, 0, 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000C32 RID: 3122
		private Map map;

		// Token: 0x04000C33 RID: 3123
		private bool dirty = true;

		// Token: 0x04000C34 RID: 3124
		private BoolGrid exitMapGrid;

		// Token: 0x04000C35 RID: 3125
		private CellBoolDrawer drawerInt;

		// Token: 0x04000C36 RID: 3126
		private const int MaxDistToEdge = 2;
	}
}

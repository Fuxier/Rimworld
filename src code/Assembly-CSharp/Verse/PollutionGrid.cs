using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001EA RID: 490
	public class PollutionGrid : IExposable
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x0004CCCB File Offset: 0x0004AECB
		public int TotalPollution
		{
			get
			{
				return this.grid.TrueCount;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x0004CCD8 File Offset: 0x0004AED8
		public List<IntVec3> AllPollutableCells
		{
			get
			{
				this.allPollutableCells.Clear();
				this.allPollutableCells.AddRange(this.map.AllCells.Where(new Func<IntVec3, bool>(this.EverPollutable)));
				return this.allPollutableCells;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x0004CD12 File Offset: 0x0004AF12
		public float TotalPollutionPercent
		{
			get
			{
				return (float)this.grid.TrueCount / (float)this.AllPollutableCells.Count;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x0004CD30 File Offset: 0x0004AF30
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3650, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0004CDA4 File Offset: 0x0004AFA4
		public PollutionGrid(Map map)
		{
			this.grid = new BoolGrid(map);
			this.map = map;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0004CDD5 File Offset: 0x0004AFD5
		public bool IsPolluted(IntVec3 cell)
		{
			return ModsConfig.BiotechActive && cell.InBounds(this.map) && this.grid[cell];
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0004CDFC File Offset: 0x0004AFFC
		public bool EverPollutable(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(this.map);
			return edifice == null || edifice.def.EverPollutable;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0004CE3C File Offset: 0x0004B03C
		public void SetPolluted(IntVec3 cell, bool isPolluted, bool silent = false)
		{
			if (!ModLister.CheckBiotech("Set pollution"))
			{
				return;
			}
			if (cell.InBounds(this.map) && this.grid[cell] != isPolluted)
			{
				this.grid.Set(cell, isPolluted);
				this.dirty = true;
				this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlag.Terrain, true, false);
				this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlag.Pollution, false, false);
				this.Drawer.SetDirty();
				this.map.fertilityGrid.Drawer.SetDirty();
				if (!silent && isPolluted && MapGenerator.mapBeingGenerated != this.map && !this.pollutedCellsThisTick.Contains(cell))
				{
					this.pollutedCellsThisTick.Add(cell);
				}
			}
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0004CF09 File Offset: 0x0004B109
		public bool CanPollute(IntVec3 c)
		{
			return this.EverPollutable(c) && !this.IsPolluted(c);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0004CF20 File Offset: 0x0004B120
		public bool CanUnpollute(IntVec3 c)
		{
			return this.IsPolluted(c);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0004CF2C File Offset: 0x0004B12C
		public void PollutionTick()
		{
			if (this.dirty)
			{
				Find.WorldGrid[this.map.Tile].pollution = this.TotalPollutionPercent;
				Find.World.renderer.Notify_TilePollutionChanged(this.map.Tile);
				this.dirty = false;
			}
			if (this.map.IsHashIntervalTick(200) && this.TotalPollution > 0)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.PollutedTerrain, OpportunityType.Important);
			}
			if (this.pollutedCellsThisTick.Count > 0)
			{
				EffecterDef effecterDef = (this.pollutedCellsThisTick.Count > 10) ? EffecterDefOf.CellPollution_Performant : EffecterDefOf.CellPollution;
				for (int i = 0; i < this.pollutedCellsThisTick.Count; i++)
				{
					IntVec3 intVec = this.pollutedCellsThisTick[i];
					Effecter eff = effecterDef.Spawn(intVec, this.map, Vector3.zero, 1f);
					this.map.effecterMaintainer.AddEffecterToMaintain(eff, intVec, 45);
				}
			}
			this.pollutedCellsThisTick.Clear();
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0004D02C File Offset: 0x0004B22C
		public void PollutionGridUpdate()
		{
			if (Find.PlaySettings.showPollutionOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0004D050 File Offset: 0x0004B250
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			return intVec.InBounds(this.map) && !intVec.Filled(this.map) && !intVec.Fogged(this.map) && this.EverPollutable(intVec) && this.IsPolluted(intVec);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00020495 File Offset: 0x0001E695
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0004D0B4 File Offset: 0x0004B2B4
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			IntVec3 cell = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			if (!this.IsPolluted(cell))
			{
				return Color.white;
			}
			return Color.red;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0004D0EC File Offset: 0x0004B2EC
		public void ExposeData()
		{
			Scribe_Deep.Look<BoolGrid>(ref this.grid, "grid", Array.Empty<object>());
		}

		// Token: 0x04000C59 RID: 3161
		private const int LessonPollutionTicks = 200;

		// Token: 0x04000C5A RID: 3162
		private const int PerformantPollutionEffectThreshold = 10;

		// Token: 0x04000C5B RID: 3163
		public const int DissolutionEffecterDuration = 45;

		// Token: 0x04000C5C RID: 3164
		private BoolGrid grid;

		// Token: 0x04000C5D RID: 3165
		private Map map;

		// Token: 0x04000C5E RID: 3166
		private bool dirty;

		// Token: 0x04000C5F RID: 3167
		private CellBoolDrawer drawerInt;

		// Token: 0x04000C60 RID: 3168
		private List<IntVec3> allPollutableCells = new List<IntVec3>();

		// Token: 0x04000C61 RID: 3169
		private List<IntVec3> pollutedCellsThisTick = new List<IntVec3>();
	}
}

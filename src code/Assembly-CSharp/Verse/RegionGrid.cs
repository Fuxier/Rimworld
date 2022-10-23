using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000240 RID: 576
	public sealed class RegionGrid
	{
		// Token: 0x17000322 RID: 802
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x0005FE88 File Offset: 0x0005E088
		public Region[] DirectGrid
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get the region grid but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				return this.regionGrid;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x0005FED9 File Offset: 0x0005E0D9
		public IEnumerable<Region> AllRegions_NoRebuild_InvalidAllowed
		{
			get
			{
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int num;
					for (int i = 0; i < count; i = num + 1)
					{
						if (this.regionGrid[i] != null && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
						num = i;
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x0005FEE9 File Offset: 0x0005E0E9
		public IEnumerable<Region> AllRegions
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get all valid regions but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int num;
					for (int i = 0; i < count; i = num + 1)
					{
						if (this.regionGrid[i] != null && this.regionGrid[i].valid && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
						num = i;
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0005FEFC File Offset: 0x0005E0FC
		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0005FF50 File Offset: 0x0005E150
		public Region GetValidRegionAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c);
				return null;
			}
			if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
			{
				Log.Warning("Trying to get valid region at " + c + " but RegionAndRoomUpdater is disabled. The result may be incorrect.");
			}
			this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0005FFF8 File Offset: 0x0005E1F8
		public Region GetValidRegionAt_NoRebuild(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c);
				return null;
			}
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00060051 File Offset: 0x0005E251
		public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
		{
			return this.regionGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0006006B File Offset: 0x0005E26B
		public void SetRegionAt(IntVec3 c, Region reg)
		{
			this.regionGrid[this.map.cellIndices.CellToIndex(c)] = reg;
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x00060088 File Offset: 0x0005E288
		public void UpdateClean()
		{
			for (int i = 0; i < 16; i++)
			{
				if (this.curCleanIndex >= this.regionGrid.Length)
				{
					this.curCleanIndex = 0;
				}
				Region region = this.regionGrid[this.curCleanIndex];
				if (region != null && !region.valid)
				{
					this.regionGrid[this.curCleanIndex] = null;
				}
				this.curCleanIndex++;
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000600F0 File Offset: 0x0005E2F0
		public void DebugDraw()
		{
			if (this.map != Find.CurrentMap)
			{
				return;
			}
			if (DebugViewSettings.drawRegionTraversal)
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				currentViewRect.ClipInsideMap(this.map);
				foreach (IntVec3 c in currentViewRect)
				{
					Region validRegionAt = this.GetValidRegionAt(c);
					if (validRegionAt != null && !this.drawnRegions.Contains(validRegionAt))
					{
						validRegionAt.DebugDraw();
						this.drawnRegions.Add(validRegionAt);
					}
				}
				this.drawnRegions.Clear();
			}
			IntVec3 intVec = UI.MouseCell();
			if (intVec.InBounds(this.map))
			{
				if (DebugViewSettings.drawDistricts)
				{
					District district = intVec.GetDistrict(this.map, RegionType.Set_Passable);
					if (district != null)
					{
						district.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRooms)
				{
					Room room = intVec.GetRoom(this.map);
					if (room != null)
					{
						room.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRegions || DebugViewSettings.drawRegionLinks || DebugViewSettings.drawRegionThings)
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
					if (regionAt_NoRebuild_InvalidAllowed != null)
					{
						regionAt_NoRebuild_InvalidAllowed.DebugDrawMouseover();
					}
				}
			}
		}

		// Token: 0x04000E66 RID: 3686
		private Map map;

		// Token: 0x04000E67 RID: 3687
		private Region[] regionGrid;

		// Token: 0x04000E68 RID: 3688
		private int curCleanIndex;

		// Token: 0x04000E69 RID: 3689
		public List<District> allDistricts = new List<District>();

		// Token: 0x04000E6A RID: 3690
		public List<Room> allRooms = new List<Room>();

		// Token: 0x04000E6B RID: 3691
		public static HashSet<Region> allRegionsYielded = new HashSet<Region>();

		// Token: 0x04000E6C RID: 3692
		private const int CleanSquaresPerFrame = 16;

		// Token: 0x04000E6D RID: 3693
		public HashSet<Region> drawnRegions = new HashSet<Region>();
	}
}

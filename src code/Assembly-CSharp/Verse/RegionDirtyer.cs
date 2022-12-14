using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200023F RID: 575
	public class RegionDirtyer
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x0005F947 File Offset: 0x0005DB47
		public bool AnyDirty
		{
			get
			{
				return this.dirtyCells.Count > 0;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x0005F957 File Offset: 0x0005DB57
		public List<IntVec3> DirtyCells
		{
			get
			{
				return this.dirtyCells;
			}
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0005F95F File Offset: 0x0005DB5F
		public RegionDirtyer(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0005F984 File Offset: 0x0005DB84
		internal void Notify_WalkabilityChanged(IntVec3 c, bool newWalkability)
		{
			this.regionsToDirty.Clear();
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(this.map))
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c2);
					if (regionAt_NoRebuild_InvalidAllowed != null && regionAt_NoRebuild_InvalidAllowed.valid)
					{
						this.map.temperatureCache.TryCacheRegionTempInfo(c, regionAt_NoRebuild_InvalidAllowed);
						this.regionsToDirty.Add(regionAt_NoRebuild_InvalidAllowed);
					}
				}
			}
			for (int j = 0; j < this.regionsToDirty.Count; j++)
			{
				this.SetRegionDirty(this.regionsToDirty[j], true);
			}
			this.regionsToDirty.Clear();
			if (newWalkability && !this.dirtyCells.Contains(c))
			{
				this.dirtyCells.Add(c);
			}
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0005FA58 File Offset: 0x0005DC58
		internal void Notify_ThingAffectingRegionsSpawned(Thing b)
		{
			this.regionsToDirty.Clear();
			foreach (IntVec3 c in b.OccupiedRect().ExpandedBy(1).ClipInsideMap(b.Map))
			{
				Region validRegionAt_NoRebuild = b.Map.regionGrid.GetValidRegionAt_NoRebuild(c);
				if (validRegionAt_NoRebuild != null)
				{
					b.Map.temperatureCache.TryCacheRegionTempInfo(c, validRegionAt_NoRebuild);
					this.regionsToDirty.Add(validRegionAt_NoRebuild);
				}
			}
			for (int i = 0; i < this.regionsToDirty.Count; i++)
			{
				this.SetRegionDirty(this.regionsToDirty[i], true);
			}
			this.regionsToDirty.Clear();
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0005FB3C File Offset: 0x0005DD3C
		internal void Notify_ThingAffectingRegionsDespawned(Thing b)
		{
			this.regionsToDirty.Clear();
			Region validRegionAt_NoRebuild = this.map.regionGrid.GetValidRegionAt_NoRebuild(b.Position);
			if (validRegionAt_NoRebuild != null)
			{
				this.map.temperatureCache.TryCacheRegionTempInfo(b.Position, validRegionAt_NoRebuild);
				this.regionsToDirty.Add(validRegionAt_NoRebuild);
			}
			foreach (IntVec3 c in GenAdj.CellsAdjacent8Way(b))
			{
				if (c.InBounds(this.map))
				{
					Region validRegionAt_NoRebuild2 = this.map.regionGrid.GetValidRegionAt_NoRebuild(c);
					if (validRegionAt_NoRebuild2 != null)
					{
						this.map.temperatureCache.TryCacheRegionTempInfo(c, validRegionAt_NoRebuild2);
						this.regionsToDirty.Add(validRegionAt_NoRebuild2);
					}
				}
			}
			for (int i = 0; i < this.regionsToDirty.Count; i++)
			{
				this.SetRegionDirty(this.regionsToDirty[i], true);
			}
			this.regionsToDirty.Clear();
			if (b.def.size.x == 1 && b.def.size.z == 1)
			{
				this.dirtyCells.Add(b.Position);
				return;
			}
			CellRect cellRect = b.OccupiedRect();
			for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
			{
				for (int k = cellRect.minX; k <= cellRect.maxX; k++)
				{
					IntVec3 item = new IntVec3(k, 0, j);
					this.dirtyCells.Add(item);
				}
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x0005FCD8 File Offset: 0x0005DED8
		internal void SetAllClean()
		{
			for (int i = 0; i < this.dirtyCells.Count; i++)
			{
				this.map.temperatureCache.ResetCachedCellInfo(this.dirtyCells[i]);
			}
			this.dirtyCells.Clear();
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0005FD24 File Offset: 0x0005DF24
		private void SetRegionDirty(Region reg, bool addCellsToDirtyCells = true)
		{
			if (!reg.valid)
			{
				return;
			}
			reg.valid = false;
			reg.District = null;
			for (int i = 0; i < reg.links.Count; i++)
			{
				reg.links[i].Deregister(reg);
			}
			reg.links.Clear();
			if (addCellsToDirtyCells)
			{
				foreach (IntVec3 intVec in reg.Cells)
				{
					this.dirtyCells.Add(intVec);
					if (DebugViewSettings.drawRegionDirties)
					{
						this.map.debugDrawer.FlashCell(intVec, 0f, null, 50);
					}
				}
			}
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0005FDE4 File Offset: 0x0005DFE4
		internal void SetAllDirty()
		{
			this.dirtyCells.Clear();
			foreach (IntVec3 item in this.map)
			{
				this.dirtyCells.Add(item);
			}
			foreach (Region reg in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
			{
				this.SetRegionDirty(reg, false);
			}
		}

		// Token: 0x04000E63 RID: 3683
		private Map map;

		// Token: 0x04000E64 RID: 3684
		private List<IntVec3> dirtyCells = new List<IntVec3>();

		// Token: 0x04000E65 RID: 3685
		private List<Region> regionsToDirty = new List<Region>();
	}
}

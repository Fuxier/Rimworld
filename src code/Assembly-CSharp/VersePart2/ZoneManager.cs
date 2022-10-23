using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000272 RID: 626
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x00067FFC File Offset: 0x000661FC
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00068004 File Offset: 0x00066204
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00068034 File Offset: 0x00066234
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00068060 File Offset: 0x00066260
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00068098 File Offset: 0x00066298
		private void RebuildZoneGrid()
		{
			CellIndices cellIndices = this.map.cellIndices;
			this.zoneGrid = new Zone[cellIndices.NumGridCells];
			foreach (Zone zone in this.allZones)
			{
				foreach (IntVec3 c in zone)
				{
					this.zoneGrid[cellIndices.CellToIndex(c)] = zone;
				}
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00068144 File Offset: 0x00066344
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00068158 File Offset: 0x00066358
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
			if (Find.Selector.SelectedZone == oldZone)
			{
				Find.Selector.ClearSelection();
			}
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00068184 File Offset: 0x00066384
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0006819F File Offset: 0x0006639F
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x000681BA File Offset: 0x000663BA
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x000681D4 File Offset: 0x000663D4
		public string NewZoneName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i;
				if (!this.allZones.Any((Zone z) => z.label == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of zone names.");
			return "Zone X";
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00068240 File Offset: 0x00066440
		internal void Notify_NoZoneOverlapThingSpawned(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					Zone zone = this.ZoneAt(c);
					if (zone != null)
					{
						zone.RemoveCell(c);
						zone.CheckContiguous();
					}
				}
			}
		}

		// Token: 0x04000F27 RID: 3879
		public Map map;

		// Token: 0x04000F28 RID: 3880
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x04000F29 RID: 3881
		private Zone[] zoneGrid;
	}
}

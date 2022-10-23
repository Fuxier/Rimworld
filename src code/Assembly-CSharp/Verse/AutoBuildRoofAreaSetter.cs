using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200024F RID: 591
	public class AutoBuildRoofAreaSetter
	{
		// Token: 0x060010F1 RID: 4337 RVA: 0x00062BDC File Offset: 0x00060DDC
		public AutoBuildRoofAreaSetter(Map map)
		{
			this.map = map;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00062C17 File Offset: 0x00060E17
		public void TryGenerateAreaFor(Room room)
		{
			this.queuedGenerateRooms.Add(room);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00062C25 File Offset: 0x00060E25
		public void AutoBuildRoofAreaSetterTick_First()
		{
			this.ResolveQueuedGenerateRoofs();
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00062C30 File Offset: 0x00060E30
		public void ResolveQueuedGenerateRoofs()
		{
			for (int i = 0; i < this.queuedGenerateRooms.Count; i++)
			{
				this.TryGenerateAreaNow(this.queuedGenerateRooms[i]);
			}
			this.queuedGenerateRooms.Clear();
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00062C70 File Offset: 0x00060E70
		private void TryGenerateAreaNow(Room room)
		{
			if (room.Dereferenced || room.TouchesMapEdge)
			{
				return;
			}
			if (room.RegionCount > 26 || room.CellCount > 320)
			{
				return;
			}
			if (room.IsDoorway)
			{
				return;
			}
			bool flag = false;
			foreach (IntVec3 c in room.BorderCells)
			{
				Thing roofHolderOrImpassable = c.GetRoofHolderOrImpassable(this.map);
				if (roofHolderOrImpassable != null)
				{
					if (roofHolderOrImpassable.Faction != null && roofHolderOrImpassable.Faction != Faction.OfPlayer)
					{
						return;
					}
					if (roofHolderOrImpassable.def.building != null && !roofHolderOrImpassable.def.building.allowAutoroof)
					{
						return;
					}
					if (roofHolderOrImpassable.Faction == Faction.OfPlayer)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			this.innerCells.Clear();
			foreach (IntVec3 intVec in room.Cells)
			{
				if (!this.innerCells.Contains(intVec))
				{
					this.innerCells.Add(intVec);
				}
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = intVec + GenAdj.AdjacentCells[i];
					if (c2.InBounds(this.map))
					{
						Thing roofHolderOrImpassable2 = c2.GetRoofHolderOrImpassable(this.map);
						if (roofHolderOrImpassable2 != null && (roofHolderOrImpassable2.def.size.x > 1 || roofHolderOrImpassable2.def.size.z > 1))
						{
							CellRect cellRect = roofHolderOrImpassable2.OccupiedRect();
							cellRect.ClipInsideMap(this.map);
							for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
							{
								for (int k = cellRect.minX; k <= cellRect.maxX; k++)
								{
									IntVec3 item = new IntVec3(k, 0, j);
									if (!this.innerCells.Contains(item))
									{
										this.innerCells.Add(item);
									}
								}
							}
						}
					}
				}
			}
			this.cellsToRoof.Clear();
			foreach (IntVec3 a in this.innerCells)
			{
				for (int l = 0; l < 9; l++)
				{
					IntVec3 intVec2 = a + GenAdj.AdjacentCellsAndInside[l];
					if (intVec2.InBounds(this.map) && (l == 8 || intVec2.GetRoofHolderOrImpassable(this.map) != null) && !this.cellsToRoof.Contains(intVec2))
					{
						this.cellsToRoof.Add(intVec2);
					}
				}
			}
			this.justRoofedCells.Clear();
			foreach (IntVec3 intVec3 in this.cellsToRoof)
			{
				if (this.map.roofGrid.RoofAt(intVec3) == null && !this.justRoofedCells.Contains(intVec3) && !this.map.areaManager.NoRoof[intVec3] && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec3, this.map, true))
				{
					this.map.areaManager.BuildRoof[intVec3] = true;
					this.justRoofedCells.Add(intVec3);
				}
			}
		}

		// Token: 0x04000EAF RID: 3759
		private Map map;

		// Token: 0x04000EB0 RID: 3760
		private List<Room> queuedGenerateRooms = new List<Room>();

		// Token: 0x04000EB1 RID: 3761
		private HashSet<IntVec3> cellsToRoof = new HashSet<IntVec3>();

		// Token: 0x04000EB2 RID: 3762
		private HashSet<IntVec3> innerCells = new HashSet<IntVec3>();

		// Token: 0x04000EB3 RID: 3763
		private List<IntVec3> justRoofedCells = new List<IntVec3>();
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000252 RID: 594
	public static class RoofCollapseCellsFinder
	{
		// Token: 0x060010FD RID: 4349 RVA: 0x000632AC File Offset: 0x000614AC
		public static void Notify_RoofHolderDespawned(Thing t, Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			RoofCollapseCellsFinder.ProcessRoofHolderDespawned(t.OccupiedRect(), t.Position, map, false, false);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000632CC File Offset: 0x000614CC
		public static void ProcessRoofHolderDespawned(CellRect rect, IntVec3 position, Map map, bool removalMode = false, bool canRemoveThickRoof = false)
		{
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(rect, map, removalMode, canRemoveThickRoof);
			RoofGrid roofGrid = map.roofGrid;
			RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Clear();
			for (int i = 0; i < RoofCollapseUtility.RoofSupportRadialCellsCount; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && roofGrid.Roofed(intVec.x, intVec.z) && !map.roofCollapseBuffer.IsMarkedToCollapse(intVec) && !RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, map, false))
				{
					if (removalMode && (canRemoveThickRoof || intVec.GetRoof(map).VanishOnCollapse))
					{
						map.roofGrid.SetRoof(intVec, null);
					}
					else
					{
						map.roofCollapseBuffer.MarkToCollapse(intVec);
					}
					RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Add(intVec);
				}
			}
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar, map, removalMode, canRemoveThickRoof);
			RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Clear();
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000633A4 File Offset: 0x000615A4
		public static void RemoveBulkCollapsingRoofs(List<IntVec3> nearCells, Map map)
		{
			for (int i = 0; i < nearCells.Count; i++)
			{
				RoofCollapseCellsFinder.ProcessRoofHolderDespawned(new CellRect(nearCells[i].x, nearCells[i].z, 1, 1), nearCells[i], map, true, true);
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x000633F0 File Offset: 0x000615F0
		public static void CheckCollapseFlyingRoofs(List<IntVec3> nearCells, Map map, bool removalMode = false, bool canRemoveThickRoof = false)
		{
			RoofCollapseCellsFinder.visitedCells.Clear();
			for (int i = 0; i < nearCells.Count; i++)
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofAtAndAdjInternal(nearCells[i], map, removalMode, canRemoveThickRoof);
			}
			RoofCollapseCellsFinder.visitedCells.Clear();
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00063434 File Offset: 0x00061634
		public static void CheckCollapseFlyingRoofs(CellRect nearRect, Map map, bool removalMode = false, bool canRemoveThickRoof = false)
		{
			RoofCollapseCellsFinder.visitedCells.Clear();
			foreach (IntVec3 root in nearRect)
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofAtAndAdjInternal(root, map, removalMode, canRemoveThickRoof);
			}
			RoofCollapseCellsFinder.visitedCells.Clear();
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00063498 File Offset: 0x00061698
		private static bool CheckCollapseFlyingRoofAtAndAdjInternal(IntVec3 root, Map map, bool removalMode, bool canRemoveThickRoof)
		{
			RoofCollapseBuffer roofCollapseBuffer = map.roofCollapseBuffer;
			if (removalMode && roofCollapseBuffer.CellsMarkedToCollapse.Count > 0)
			{
				map.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			}
			Predicate<IntVec3> <>9__0;
			Action<IntVec3> <>9__1;
			for (int i = 0; i < 5; i++)
			{
				IntVec3 intVec = root + GenAdj.CardinalDirectionsAndInside[i];
				if (intVec.InBounds(map) && intVec.Roofed(map) && !RoofCollapseCellsFinder.visitedCells.Contains(intVec) && !roofCollapseBuffer.IsMarkedToCollapse(intVec) && !RoofCollapseCellsFinder.ConnectsToRoofHolder(intVec, map, RoofCollapseCellsFinder.visitedCells))
				{
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root2 = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => x.Roofed(map)));
					}
					Action<IntVec3> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(IntVec3 x)
						{
							roofCollapseBuffer.MarkToCollapse(x);
						});
					}
					floodFiller.FloodFill(root2, passCheck, processor, int.MaxValue, false, null);
					if (removalMode)
					{
						List<IntVec3> cellsMarkedToCollapse = roofCollapseBuffer.CellsMarkedToCollapse;
						for (int j = cellsMarkedToCollapse.Count - 1; j >= 0; j--)
						{
							RoofDef roofDef = map.roofGrid.RoofAt(cellsMarkedToCollapse[j]);
							if (roofDef != null && (canRemoveThickRoof || roofDef.VanishOnCollapse))
							{
								map.roofGrid.SetRoof(cellsMarkedToCollapse[j], null);
								cellsMarkedToCollapse.RemoveAt(j);
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00063638 File Offset: 0x00061838
		public static bool ConnectsToRoofHolder(IntVec3 c, Map map, HashSet<IntVec3> visitedCells)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (IntVec3 x) => x.Roofed(map) && !connected, delegate(IntVec3 x)
			{
				if (visitedCells.Contains(x))
				{
					connected = true;
					return;
				}
				visitedCells.Add(x);
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[i];
					if (c2.InBounds(map))
					{
						Building edifice = c2.GetEdifice(map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							connected = true;
							return;
						}
					}
				}
			}, int.MaxValue, false, null);
			return connected;
		}

		// Token: 0x04000EB8 RID: 3768
		private static List<IntVec3> roofsCollapsingBecauseTooFar = new List<IntVec3>();

		// Token: 0x04000EB9 RID: 3769
		private static HashSet<IntVec3> visitedCells = new HashSet<IntVec3>();
	}
}

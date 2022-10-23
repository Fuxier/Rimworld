using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000569 RID: 1385
	public static class GenMorphology
	{
		// Token: 0x06002AB3 RID: 10931 RVA: 0x0011095C File Offset: 0x0010EB5C
		public static void Erode(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			GenMorphology.cellsSet.Clear();
			GenMorphology.cellsSet.AddRange(cells);
			GenMorphology.tmpEdgeCells.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					IntVec3 item = cells[i] + cardinalDirections[j];
					if (!GenMorphology.cellsSet.Contains(item))
					{
						GenMorphology.tmpEdgeCells.Add(cells[i]);
						break;
					}
				}
			}
			if (!GenMorphology.tmpEdgeCells.Any<IntVec3>())
			{
				return;
			}
			GenMorphology.tmpOutput.Clear();
			Predicate<IntVec3> passCheck;
			if (extraPredicate != null)
			{
				passCheck = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				passCheck = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x));
			}
			map.floodFiller.FloodFill(IntVec3.Invalid, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				if (traversalDist >= count)
				{
					GenMorphology.tmpOutput.Add(cell);
				}
				return false;
			}, int.MaxValue, false, GenMorphology.tmpEdgeCells);
			cells.Clear();
			cells.AddRange(GenMorphology.tmpOutput);
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x00110A8C File Offset: 0x0010EC8C
		public static void Dilate(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			FloodFiller floodFiller = map.floodFiller;
			IntVec3 invalid = IntVec3.Invalid;
			Predicate<IntVec3> passCheck = extraPredicate;
			if (extraPredicate == null && (passCheck = GenMorphology.<>c.<>9__4_0) == null)
			{
				passCheck = (GenMorphology.<>c.<>9__4_0 = ((IntVec3 x) => true));
			}
			floodFiller.FloodFill(invalid, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				if (traversalDist > count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					cells.Add(cell);
				}
				return false;
			}, int.MaxValue, false, cells);
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x00110B03 File Offset: 0x0010ED03
		public static void Open(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Erode(cells, count, map, null);
			GenMorphology.Dilate(cells, count, map, null);
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x00110B17 File Offset: 0x0010ED17
		public static void Close(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Dilate(cells, count, map, null);
			GenMorphology.Erode(cells, count, map, null);
		}

		// Token: 0x04001BEE RID: 7150
		private static HashSet<IntVec3> tmpOutput = new HashSet<IntVec3>();

		// Token: 0x04001BEF RID: 7151
		private static HashSet<IntVec3> cellsSet = new HashSet<IntVec3>();

		// Token: 0x04001BF0 RID: 7152
		private static List<IntVec3> tmpEdgeCells = new List<IntVec3>();
	}
}

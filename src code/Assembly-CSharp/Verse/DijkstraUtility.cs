using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000548 RID: 1352
	public static class DijkstraUtility
	{
		// Token: 0x06002973 RID: 10611 RVA: 0x00109224 File Offset: 0x00107424
		public static IEnumerable<IntVec3> AdjacentCellsNeighborsGetter(IntVec3 cell, Map map)
		{
			DijkstraUtility.adjacentCells.Clear();
			IntVec3[] array = GenAdj.AdjacentCells;
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 intVec = cell + array[i];
				if (intVec.InBounds(map))
				{
					DijkstraUtility.adjacentCells.Add(intVec);
				}
			}
			return DijkstraUtility.adjacentCells;
		}

		// Token: 0x04001B66 RID: 7014
		private static List<IntVec3> adjacentCells = new List<IntVec3>();
	}
}

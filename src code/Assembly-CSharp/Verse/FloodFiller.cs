using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000559 RID: 1369
	public class FloodFiller
	{
		// Token: 0x060029F5 RID: 10741 RVA: 0x0010A7F8 File Offset: 0x001089F8
		public FloodFiller(Map map)
		{
			this.map = map;
			this.traversalDistance = new IntGrid(map);
			this.traversalDistance.Clear(-1);
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x0010A838 File Offset: 0x00108A38
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Action<IntVec3> processor, int maxCellsToProcess = 2147483647, bool rememberParents = false, IEnumerable<IntVec3> extraRoots = null)
		{
			this.FloodFill(root, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				processor(cell);
				return false;
			}, maxCellsToProcess, rememberParents, extraRoots);
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x0010A86C File Offset: 0x00108A6C
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Action<IntVec3, int> processor, int maxCellsToProcess = 2147483647, bool rememberParents = false, IEnumerable<IntVec3> extraRoots = null)
		{
			this.FloodFill(root, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				processor(cell, traversalDist);
				return false;
			}, maxCellsToProcess, rememberParents, extraRoots);
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x0010A8A0 File Offset: 0x00108AA0
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Func<IntVec3, bool> processor, int maxCellsToProcess = 2147483647, bool rememberParents = false, IEnumerable<IntVec3> extraRoots = null)
		{
			this.FloodFill(root, passCheck, (IntVec3 cell, int traversalDist) => processor(cell), maxCellsToProcess, rememberParents, extraRoots);
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x0010A8D4 File Offset: 0x00108AD4
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Func<IntVec3, int, bool> processor, int maxCellsToProcess = 2147483647, bool rememberParents = false, IEnumerable<IntVec3> extraRoots = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.");
			}
			this.working = true;
			this.ClearVisited();
			if (rememberParents && this.parentGrid == null)
			{
				this.parentGrid = new CellGrid(this.map);
			}
			if (root.IsValid && extraRoots == null && !passCheck(root))
			{
				if (rememberParents)
				{
					this.parentGrid[root] = IntVec3.Invalid;
				}
				this.working = false;
				return;
			}
			int area = this.map.Area;
			IntVec3[] cardinalDirectionsAround = GenAdj.CardinalDirectionsAround;
			int num = cardinalDirectionsAround.Length;
			CellIndices cellIndices = this.map.cellIndices;
			int num2 = 0;
			this.openSet.Clear();
			if (root.IsValid)
			{
				int num3 = cellIndices.CellToIndex(root);
				this.visited.Add(num3);
				this.traversalDistance[num3] = 0;
				this.openSet.Enqueue(root);
			}
			if (extraRoots != null)
			{
				IList<IntVec3> list = extraRoots as IList<IntVec3>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						int num4 = cellIndices.CellToIndex(list[i]);
						this.visited.Add(num4);
						this.traversalDistance[num4] = 0;
						this.openSet.Enqueue(list[i]);
					}
				}
				else
				{
					foreach (IntVec3 intVec in extraRoots)
					{
						int num5 = cellIndices.CellToIndex(intVec);
						this.visited.Add(num5);
						this.traversalDistance[num5] = 0;
						this.openSet.Enqueue(intVec);
					}
				}
			}
			if (rememberParents)
			{
				for (int j = 0; j < this.visited.Count; j++)
				{
					IntVec3 intVec2 = cellIndices.IndexToCell(this.visited[j]);
					this.parentGrid[this.visited[j]] = (passCheck(intVec2) ? intVec2 : IntVec3.Invalid);
				}
			}
			while (this.openSet.Count > 0)
			{
				IntVec3 intVec3 = this.openSet.Dequeue();
				int num6 = this.traversalDistance[cellIndices.CellToIndex(intVec3)];
				if (processor(intVec3, num6))
				{
					break;
				}
				num2++;
				if (num2 == maxCellsToProcess)
				{
					break;
				}
				for (int k = 0; k < num; k++)
				{
					IntVec3 intVec4 = intVec3 + cardinalDirectionsAround[k];
					int num7 = cellIndices.CellToIndex(intVec4);
					if (intVec4.InBounds(this.map) && this.traversalDistance[num7] == -1 && passCheck(intVec4))
					{
						this.visited.Add(num7);
						this.openSet.Enqueue(intVec4);
						this.traversalDistance[num7] = num6 + 1;
						if (rememberParents)
						{
							this.parentGrid[num7] = intVec3;
						}
					}
				}
				if (this.openSet.Count > area)
				{
					Log.Error("Overflow on flood fill (>" + area + " cells). Make sure we're not flooding over the same area after we check it.");
					this.working = false;
					return;
				}
			}
			this.working = false;
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x0010AC18 File Offset: 0x00108E18
		public void ReconstructLastFloodFillPath(IntVec3 dest, List<IntVec3> outPath)
		{
			outPath.Clear();
			if (this.parentGrid == null || !dest.InBounds(this.map) || !this.parentGrid[dest].IsValid)
			{
				return;
			}
			int num = 0;
			int num2 = this.map.Area + 1;
			IntVec3 intVec = dest;
			for (;;)
			{
				num++;
				if (num > num2)
				{
					break;
				}
				if (!intVec.IsValid)
				{
					goto IL_8C;
				}
				outPath.Add(intVec);
				if (this.parentGrid[intVec] == intVec)
				{
					goto IL_8C;
				}
				intVec = this.parentGrid[intVec];
			}
			Log.Error("Too many iterations.");
			IL_8C:
			outPath.Reverse();
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x0010ACB8 File Offset: 0x00108EB8
		private void ClearVisited()
		{
			int i = 0;
			int count = this.visited.Count;
			while (i < count)
			{
				int index = this.visited[i];
				this.traversalDistance[index] = -1;
				if (this.parentGrid != null)
				{
					this.parentGrid[index] = IntVec3.Invalid;
				}
				i++;
			}
			this.visited.Clear();
			this.openSet.Clear();
		}

		// Token: 0x04001B7D RID: 7037
		private Map map;

		// Token: 0x04001B7E RID: 7038
		private bool working;

		// Token: 0x04001B7F RID: 7039
		private Queue<IntVec3> openSet = new Queue<IntVec3>();

		// Token: 0x04001B80 RID: 7040
		private IntGrid traversalDistance;

		// Token: 0x04001B81 RID: 7041
		private CellGrid parentGrid;

		// Token: 0x04001B82 RID: 7042
		private List<int> visited = new List<int>();
	}
}

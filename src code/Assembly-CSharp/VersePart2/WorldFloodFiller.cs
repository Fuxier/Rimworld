using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000072 RID: 114
	public class WorldFloodFiller
	{
		// Token: 0x06000485 RID: 1157 RVA: 0x00019CFC File Offset: 0x00017EFC
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00019D30 File Offset: 0x00017F30
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile, traversalDistance);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00019D64 File Offset: 0x00017F64
		public void FloodFill(int rootTile, Predicate<int> passCheck, Predicate<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (int tile, int traversalDistance) => processor(tile), maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00019D98 File Offset: 0x00017F98
		public void FloodFill(int rootTile, Predicate<int> passCheck, Func<int, int, bool> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.");
			}
			this.working = true;
			this.ClearVisited();
			if (rootTile != -1 && extraRootTiles == null && !passCheck(rootTile))
			{
				this.working = false;
				return;
			}
			int tilesCount = Find.WorldGrid.TilesCount;
			int num = tilesCount;
			if (this.traversalDistance.Count != tilesCount)
			{
				this.traversalDistance.Clear();
				for (int i = 0; i < tilesCount; i++)
				{
					this.traversalDistance.Add(-1);
				}
			}
			WorldGrid worldGrid = Find.WorldGrid;
			List<int> tileIDToNeighbors_offsets = worldGrid.tileIDToNeighbors_offsets;
			List<int> tileIDToNeighbors_values = worldGrid.tileIDToNeighbors_values;
			int num2 = 0;
			this.openSet.Clear();
			if (rootTile != -1)
			{
				this.visited.Add(rootTile);
				this.traversalDistance[rootTile] = 0;
				this.openSet.Enqueue(rootTile);
			}
			if (extraRootTiles == null)
			{
				goto IL_25F;
			}
			this.visited.AddRange(extraRootTiles);
			IList<int> list = extraRootTiles as IList<int>;
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					int num3 = list[j];
					this.traversalDistance[num3] = 0;
					this.openSet.Enqueue(num3);
				}
				goto IL_25F;
			}
			using (IEnumerator<int> enumerator = extraRootTiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num4 = enumerator.Current;
					this.traversalDistance[num4] = 0;
					this.openSet.Enqueue(num4);
				}
				goto IL_25F;
			}
			IL_16E:
			int num5 = this.openSet.Dequeue();
			int num6 = this.traversalDistance[num5];
			if (processor(num5, num6))
			{
				goto IL_270;
			}
			num2++;
			if (num2 == maxTilesToProcess)
			{
				goto IL_270;
			}
			int num7 = (num5 + 1 < tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_offsets[num5 + 1] : tileIDToNeighbors_values.Count;
			for (int k = tileIDToNeighbors_offsets[num5]; k < num7; k++)
			{
				int num8 = tileIDToNeighbors_values[k];
				if (this.traversalDistance[num8] == -1 && passCheck(num8))
				{
					this.visited.Add(num8);
					this.openSet.Enqueue(num8);
					this.traversalDistance[num8] = num6 + 1;
				}
			}
			if (this.openSet.Count > num)
			{
				Log.Error("Overflow on world flood fill (>" + num + " cells). Make sure we're not flooding over the same area after we check it.");
				this.working = false;
				return;
			}
			IL_25F:
			if (this.openSet.Count > 0)
			{
				goto IL_16E;
			}
			IL_270:
			this.working = false;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001A02C File Offset: 0x0001822C
		private void ClearVisited()
		{
			int i = 0;
			int count = this.visited.Count;
			while (i < count)
			{
				this.traversalDistance[this.visited[i]] = -1;
				i++;
			}
			this.visited.Clear();
			this.openSet.Clear();
		}

		// Token: 0x0400020C RID: 524
		private bool working;

		// Token: 0x0400020D RID: 525
		private Queue<int> openSet = new Queue<int>();

		// Token: 0x0400020E RID: 526
		private List<int> traversalDistance = new List<int>();

		// Token: 0x0400020F RID: 527
		private List<int> visited = new List<int>();
	}
}

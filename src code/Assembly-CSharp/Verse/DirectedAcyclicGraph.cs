using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000549 RID: 1353
	public class DirectedAcyclicGraph
	{
		// Token: 0x06002975 RID: 10613 RVA: 0x00109284 File Offset: 0x00107484
		public DirectedAcyclicGraph(int numVertices)
		{
			this.numVertices = numVertices;
			for (int i = 0; i < numVertices; i++)
			{
				this.adjacencyList.Add(new List<int>());
			}
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x001092C5 File Offset: 0x001074C5
		public void AddEdge(int from, int to)
		{
			this.adjacencyList[from].Add(to);
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x001092DC File Offset: 0x001074DC
		public List<int> TopologicalSort()
		{
			bool[] array = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
			}
			List<int> result = new List<int>();
			for (int j = 0; j < this.numVertices; j++)
			{
				if (!array[j])
				{
					this.TopologicalSortInner(j, array, result);
				}
			}
			return result;
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x00109330 File Offset: 0x00107530
		private void TopologicalSortInner(int v, bool[] visited, List<int> result)
		{
			visited[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num])
				{
					this.TopologicalSortInner(num, visited, result);
				}
			}
			result.Add(v);
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x0010939C File Offset: 0x0010759C
		public bool IsCyclic()
		{
			return this.FindCycle() != -1;
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x001093AC File Offset: 0x001075AC
		public int FindCycle()
		{
			bool[] array = new bool[this.numVertices];
			bool[] array2 = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
				array2[i] = false;
			}
			for (int j = 0; j < this.numVertices; j++)
			{
				if (this.IsCyclicInner(j, array, array2))
				{
					return j;
				}
			}
			return -1;
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x0010940C File Offset: 0x0010760C
		private bool IsCyclicInner(int v, bool[] visited, bool[] history)
		{
			visited[v] = true;
			history[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num] && this.IsCyclicInner(num, visited, history))
				{
					return true;
				}
				if (history[num])
				{
					return true;
				}
			}
			history[v] = false;
			return false;
		}

		// Token: 0x04001B67 RID: 7015
		private int numVertices;

		// Token: 0x04001B68 RID: 7016
		private List<List<int>> adjacencyList = new List<List<int>>();
	}
}

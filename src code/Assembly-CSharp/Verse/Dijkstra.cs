using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000547 RID: 1351
	public static class Dijkstra<T>
	{
		// Token: 0x0600296D RID: 10605 RVA: 0x00108E8D File Offset: 0x0010708D
		public static void Run(T startingNode, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, List<KeyValuePair<T, float>> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.singleNodeList.Clear();
			Dijkstra<T>.singleNodeList.Add(startingNode);
			Dijkstra<T>.Run(Dijkstra<T>.singleNodeList, neighborsGetter, distanceGetter, outDistances, outParents);
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x00108EB4 File Offset: 0x001070B4
		public static void Run(IEnumerable<T> startingNodes, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, List<KeyValuePair<T, float>> outDistances, Dictionary<T, T> outParents = null)
		{
			outDistances.Clear();
			Dijkstra<T>.distances.Clear();
			Dijkstra<T>.queue.Clear();
			if (outParents != null)
			{
				outParents.Clear();
			}
			IList<T> list = startingNodes as IList<!0>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					T key = list[i];
					if (!Dijkstra<T>.distances.ContainsKey(key))
					{
						Dijkstra<T>.distances.Add(key, 0f);
						Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(key, 0f));
					}
				}
				goto IL_183;
			}
			using (IEnumerator<T> enumerator = startingNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T key2 = enumerator.Current;
					if (!Dijkstra<T>.distances.ContainsKey(key2))
					{
						Dijkstra<T>.distances.Add(key2, 0f);
						Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(key2, 0f));
					}
				}
				goto IL_183;
			}
			IL_DC:
			KeyValuePair<T, float> node = Dijkstra<T>.queue.Pop();
			float num = Dijkstra<T>.distances[node.Key];
			if (node.Value == num)
			{
				IEnumerable<T> enumerable = neighborsGetter(node.Key);
				if (enumerable != null)
				{
					IList<T> list2 = enumerable as IList<!0>;
					if (list2 != null)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							Dijkstra<T>.HandleNeighbor(list2[j], num, node, distanceGetter, outParents);
						}
					}
					else
					{
						foreach (!0 n in enumerable)
						{
							Dijkstra<T>.HandleNeighbor(n, num, node, distanceGetter, outParents);
						}
					}
				}
			}
			IL_183:
			if (Dijkstra<T>.queue.Count == 0)
			{
				foreach (KeyValuePair<T, float> item in Dijkstra<T>.distances)
				{
					outDistances.Add(item);
				}
				Dijkstra<T>.distances.Clear();
				return;
			}
			goto IL_DC;
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x001090C0 File Offset: 0x001072C0
		public static void Run(T startingNode, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, Dictionary<T, float> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.singleNodeList.Clear();
			Dijkstra<T>.singleNodeList.Add(startingNode);
			Dijkstra<T>.Run(Dijkstra<T>.singleNodeList, neighborsGetter, distanceGetter, outDistances, outParents);
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x001090E8 File Offset: 0x001072E8
		public static void Run(IEnumerable<T> startingNodes, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, Dictionary<T, float> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.Run(startingNodes, neighborsGetter, distanceGetter, Dijkstra<T>.tmpResult, outParents);
			outDistances.Clear();
			for (int i = 0; i < Dijkstra<T>.tmpResult.Count; i++)
			{
				outDistances.Add(Dijkstra<T>.tmpResult[i].Key, Dijkstra<T>.tmpResult[i].Value);
			}
			Dijkstra<T>.tmpResult.Clear();
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x00109158 File Offset: 0x00107358
		private static void HandleNeighbor(T n, float nodeDist, KeyValuePair<T, float> node, Func<T, T, float> distanceGetter, Dictionary<T, T> outParents)
		{
			float num = nodeDist + Mathf.Max(distanceGetter(node.Key, n), 0f);
			bool flag = false;
			float num2;
			if (Dijkstra<T>.distances.TryGetValue(n, out num2))
			{
				if (num < num2)
				{
					Dijkstra<T>.distances[n] = num;
					flag = true;
				}
			}
			else
			{
				Dijkstra<T>.distances.Add(n, num);
				flag = true;
			}
			if (flag)
			{
				Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(n, num));
				if (outParents != null)
				{
					if (outParents.ContainsKey(n))
					{
						outParents[n] = node.Key;
						return;
					}
					outParents.Add(n, node.Key);
				}
			}
		}

		// Token: 0x04001B62 RID: 7010
		private static Dictionary<T, float> distances = new Dictionary<T, float>();

		// Token: 0x04001B63 RID: 7011
		private static FastPriorityQueue<KeyValuePair<T, float>> queue = new FastPriorityQueue<KeyValuePair<T, float>>(new Dijkstra<T>.DistanceComparer());

		// Token: 0x04001B64 RID: 7012
		private static List<T> singleNodeList = new List<T>();

		// Token: 0x04001B65 RID: 7013
		private static List<KeyValuePair<T, float>> tmpResult = new List<KeyValuePair<T, float>>();

		// Token: 0x02002108 RID: 8456
		private class DistanceComparer : IComparer<KeyValuePair<T, float>>
		{
			// Token: 0x0600C5C6 RID: 50630 RVA: 0x0043CBB0 File Offset: 0x0043ADB0
			public int Compare(KeyValuePair<T, float> a, KeyValuePair<T, float> b)
			{
				return a.Value.CompareTo(b.Value);
			}
		}
	}
}

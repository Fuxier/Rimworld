using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056C RID: 1388
	public static class GenThreading
	{
		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x0011129D File Offset: 0x0010F49D
		public static int ProcessorCount
		{
			get
			{
				return Environment.ProcessorCount;
			}
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x001112A4 File Offset: 0x0010F4A4
		private static void GetMaxDegreeOfParallelism(ref int maxDegreeOfParallelism)
		{
			if (maxDegreeOfParallelism == -1)
			{
				maxDegreeOfParallelism = GenThreading.ProcessorCount;
			}
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x001112B4 File Offset: 0x0010F4B4
		public static void SliceWorkNoAlloc(int fromInclusive, int toExclusive, int maxBatches, List<GenThreading.Slice> batches)
		{
			int num = toExclusive - fromInclusive;
			if (num <= 0)
			{
				return;
			}
			int num2 = num % maxBatches;
			int num3 = Mathf.FloorToInt((float)num / (float)maxBatches);
			if (num3 > 0)
			{
				int num4 = 0;
				for (int i = 0; i < maxBatches; i++)
				{
					int num5 = num3;
					if (num2 > 0)
					{
						num5++;
						num2--;
					}
					batches.Add(new GenThreading.Slice(num4, num4 + num5));
					num4 += num5;
				}
				return;
			}
			for (int j = 0; j < num2; j++)
			{
				batches.Add(new GenThreading.Slice(j, j + 1));
			}
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x00111338 File Offset: 0x0010F538
		public static List<GenThreading.Slice> SliceWork(int fromInclusive, int toExclusive, int maxBatches)
		{
			List<GenThreading.Slice> list = new List<GenThreading.Slice>(maxBatches);
			GenThreading.SliceWorkNoAlloc(fromInclusive, toExclusive, maxBatches, list);
			return list;
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x00111358 File Offset: 0x0010F558
		public static List<List<T>> SliceWork<T>(List<T> list, int maxBatches)
		{
			List<List<T>> list2 = new List<List<T>>(maxBatches);
			foreach (GenThreading.Slice slice in GenThreading.SliceWork(0, list.Count, maxBatches))
			{
				List<T> list3 = new List<T>(slice.toExclusive - slice.fromInclusive);
				for (int i = slice.fromInclusive; i < slice.toExclusive; i++)
				{
					list3.Add(list[i]);
				}
				list2.Add(list3);
			}
			return list2;
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x001113F8 File Offset: 0x0010F5F8
		public static void ParallelForEach<T>(List<T> list, Action<T> callback, int maxDegreeOfParallelism = -1)
		{
			GenThreading.GetMaxDegreeOfParallelism(ref maxDegreeOfParallelism);
			int count = list.Count;
			long tasksDone = 0L;
			AutoResetEvent taskDoneEvent = new AutoResetEvent(false);
			using (List<List<T>>.Enumerator enumerator = GenThreading.SliceWork<T>(list, maxDegreeOfParallelism).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<T> localBatch2 = enumerator.Current;
					List<T> localBatch = localBatch2;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						foreach (T obj in localBatch)
						{
							try
							{
								callback(obj);
							}
							catch (Exception arg)
							{
								Log.Error("Error in ParallelForEach(): " + arg);
							}
						}
						Interlocked.Add(ref tasksDone, (long)localBatch.Count);
						taskDoneEvent.Set();
					});
				}
				goto IL_8F;
			}
			IL_83:
			taskDoneEvent.WaitOne();
			IL_8F:
			if (Interlocked.Read(ref tasksDone) >= (long)count)
			{
				return;
			}
			goto IL_83;
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x001114B4 File Offset: 0x0010F6B4
		public static void ParallelFor(int fromInclusive, int toExclusive, Action<int> callback, int maxDegreeOfParallelism = -1)
		{
			GenThreading.GetMaxDegreeOfParallelism(ref maxDegreeOfParallelism);
			int num = toExclusive - fromInclusive;
			long tasksDone = 0L;
			AutoResetEvent taskDoneEvent = new AutoResetEvent(false);
			using (List<GenThreading.Slice>.Enumerator enumerator = GenThreading.SliceWork(fromInclusive, toExclusive, maxDegreeOfParallelism).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenThreading.Slice localBatch2 = enumerator.Current;
					GenThreading.Slice localBatch = localBatch2;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						for (int i = localBatch.fromInclusive; i < localBatch.toExclusive; i++)
						{
							try
							{
								callback(i);
							}
							catch (Exception arg)
							{
								Log.Error("Error in ParallelFor(): " + arg);
							}
						}
						Interlocked.Add(ref tasksDone, (long)(localBatch.toExclusive - localBatch.fromInclusive));
						taskDoneEvent.Set();
					});
				}
				goto IL_8D;
			}
			IL_81:
			taskDoneEvent.WaitOne();
			IL_8D:
			if (Interlocked.Read(ref tasksDone) >= (long)num)
			{
				return;
			}
			goto IL_81;
		}

		// Token: 0x02002133 RID: 8499
		public struct Slice
		{
			// Token: 0x0600C652 RID: 50770 RVA: 0x0043E463 File Offset: 0x0043C663
			public Slice(int fromInclusive, int toExclusive)
			{
				this.fromInclusive = fromInclusive;
				this.toExclusive = toExclusive;
			}

			// Token: 0x040083C0 RID: 33728
			public readonly int fromInclusive;

			// Token: 0x040083C1 RID: 33729
			public readonly int toExclusive;
		}
	}
}

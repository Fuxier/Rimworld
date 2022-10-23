using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x02000540 RID: 1344
	public static class DeepProfiler
	{
		// Token: 0x06002948 RID: 10568 RVA: 0x00108234 File Offset: 0x00106434
		public static ThreadLocalDeepProfiler Get()
		{
			object deepProfilersLock = DeepProfiler.DeepProfilersLock;
			ThreadLocalDeepProfiler result;
			lock (deepProfilersLock)
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				ThreadLocalDeepProfiler threadLocalDeepProfiler;
				if (!DeepProfiler.deepProfilers.TryGetValue(managedThreadId, out threadLocalDeepProfiler))
				{
					threadLocalDeepProfiler = new ThreadLocalDeepProfiler();
					DeepProfiler.deepProfilers.Add(managedThreadId, threadLocalDeepProfiler);
					result = threadLocalDeepProfiler;
				}
				else
				{
					result = threadLocalDeepProfiler;
				}
			}
			return result;
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x001082A4 File Offset: 0x001064A4
		public static void Start(string label = null)
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().Start(label);
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x001082C2 File Offset: 0x001064C2
		public static void End()
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().End();
		}

		// Token: 0x04001B4E RID: 6990
		public static volatile bool enabled = true;

		// Token: 0x04001B4F RID: 6991
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x04001B50 RID: 6992
		private static readonly object DeepProfilersLock = new object();
	}
}

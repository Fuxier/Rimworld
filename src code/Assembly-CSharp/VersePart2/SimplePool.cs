using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000059 RID: 89
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00017FD5 File Offset: 0x000161D5
		public static int FreeItemsCount
		{
			get
			{
				return SimplePool<T>.freeItems.Count;
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00017FE4 File Offset: 0x000161E4
		public static T Get()
		{
			if (SimplePool<T>.freeItems.Count == 0)
			{
				return Activator.CreateInstance<T>();
			}
			int index = SimplePool<T>.freeItems.Count - 1;
			T result = SimplePool<T>.freeItems[index];
			SimplePool<T>.freeItems.RemoveAt(index);
			return result;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00018026 File Offset: 0x00016226
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}

		// Token: 0x04000168 RID: 360
		private static List<T> freeItems = new List<T>();
	}
}

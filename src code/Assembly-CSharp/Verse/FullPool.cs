using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200005B RID: 91
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x0001803F File Offset: 0x0001623F
		public static T Get()
		{
			if (FullPool<T>.freeItems.Count == 0)
			{
				return Activator.CreateInstance<T>();
			}
			T result = FullPool<T>.freeItems[FullPool<T>.freeItems.Count - 1];
			FullPool<T>.freeItems.RemoveAt(FullPool<T>.freeItems.Count - 1);
			return result;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001807F File Offset: 0x0001627F
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}

		// Token: 0x04000169 RID: 361
		private static List<T> freeItems = new List<T>();
	}
}

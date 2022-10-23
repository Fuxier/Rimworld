using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200005C RID: 92
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x0600044C RID: 1100 RVA: 0x000180A8 File Offset: 0x000162A8
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			List<T> list = this.items;
			int num = this.readIndex;
			this.readIndex = num + 1;
			return list[num];
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000180F4 File Offset: 0x000162F4
		public void Clear()
		{
			this.readIndex = 0;
		}

		// Token: 0x0400016A RID: 362
		private List<T> items = new List<T>();

		// Token: 0x0400016B RID: 363
		private int readIndex;
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000557 RID: 1367
	public class FastPriorityQueue<T>
	{
		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x060029A0 RID: 10656 RVA: 0x0010A1F2 File Offset: 0x001083F2
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x0010A1FF File Offset: 0x001083FF
		public FastPriorityQueue()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x0010A21D File Offset: 0x0010841D
		public FastPriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x0010A238 File Offset: 0x00108438
		public void Push(T item)
		{
			int num = this.innerList.Count;
			this.innerList.Add(item);
			while (num != 0)
			{
				int num2 = (num - 1) / 2;
				if (this.CompareElements(num, num2) >= 0)
				{
					break;
				}
				this.SwapElements(num, num2);
				num = num2;
			}
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x0010A280 File Offset: 0x00108480
		public T Pop()
		{
			T result = this.innerList[0];
			int num = 0;
			int count = this.innerList.Count;
			this.innerList[0] = this.innerList[count - 1];
			this.innerList.RemoveAt(count - 1);
			count = this.innerList.Count;
			for (;;)
			{
				int num2 = num;
				int num3 = 2 * num + 1;
				int num4 = num3 + 1;
				if (num3 < count && this.CompareElements(num, num3) > 0)
				{
					num = num3;
				}
				if (num4 < count && this.CompareElements(num, num4) > 0)
				{
					num = num4;
				}
				if (num == num2)
				{
					break;
				}
				this.SwapElements(num, num2);
			}
			return result;
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x0010A322 File Offset: 0x00108522
		public void Clear()
		{
			this.innerList.Clear();
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x0010A330 File Offset: 0x00108530
		protected void SwapElements(int i, int j)
		{
			T value = this.innerList[i];
			this.innerList[i] = this.innerList[j];
			this.innerList[j] = value;
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x0010A36F File Offset: 0x0010856F
		protected int CompareElements(int i, int j)
		{
			return this.comparer.Compare(this.innerList[i], this.innerList[j]);
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x0010A394 File Offset: 0x00108594
		public bool Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x04001B7B RID: 7035
		protected List<T> innerList = new List<T>();

		// Token: 0x04001B7C RID: 7036
		protected IComparer<T> comparer;
	}
}

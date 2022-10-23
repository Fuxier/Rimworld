using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000527 RID: 1319
	internal class Deque<T>
	{
		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x0010481C File Offset: 0x00102A1C
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x00104827 File Offset: 0x00102A27
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x0010484C File Offset: 0x00102A4C
		public void PushFront(T item)
		{
			this.PushPrep();
			this.first--;
			if (this.first < 0)
			{
				this.first += this.data.Length;
			}
			this.count++;
			this.data[this.first] = item;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x001048AC File Offset: 0x00102AAC
		public void PushBack(T item)
		{
			this.PushPrep();
			T[] array = this.data;
			int num = this.first;
			int num2 = this.count;
			this.count = num2 + 1;
			array[(num + num2) % this.data.Length] = item;
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x001048EC File Offset: 0x00102AEC
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x00104949 File Offset: 0x00102B49
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x0010495C File Offset: 0x00102B5C
		private void PushPrep()
		{
			if (this.count < this.data.Length)
			{
				return;
			}
			T[] destinationArray = new T[this.data.Length * 2];
			Array.Copy(this.data, this.first, destinationArray, 0, Mathf.Min(this.count, this.data.Length - this.first));
			if (this.first + this.count > this.data.Length)
			{
				Array.Copy(this.data, 0, destinationArray, this.data.Length - this.first, this.count - this.data.Length + this.first);
			}
			this.data = destinationArray;
			this.first = 0;
		}

		// Token: 0x04001A82 RID: 6786
		private T[] data;

		// Token: 0x04001A83 RID: 6787
		private int first;

		// Token: 0x04001A84 RID: 6788
		private int count;
	}
}

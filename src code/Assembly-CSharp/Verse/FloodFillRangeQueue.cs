using System;

namespace Verse
{
	// Token: 0x02000234 RID: 564
	public class FloodFillRangeQueue
	{
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0005C911 File Offset: 0x0005AB11
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x0005C919 File Offset: 0x0005AB19
		public FloodFillRange First
		{
			get
			{
				return this.array[this.head];
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0005C92C File Offset: 0x0005AB2C
		public string PerfDebugString
		{
			get
			{
				return string.Concat(new object[]
				{
					"NumTimesExpanded: ",
					this.debugNumTimesExpanded,
					", MaxUsedSize= ",
					this.debugMaxUsedSpace,
					", ClaimedSize=",
					this.array.Length,
					", UnusedSpace=",
					this.array.Length - this.debugMaxUsedSpace
				});
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0005C9A7 File Offset: 0x0005ABA7
		public FloodFillRangeQueue(int initialSize)
		{
			this.array = new FloodFillRange[initialSize];
			this.head = 0;
			this.count = 0;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0005C9CC File Offset: 0x0005ABCC
		public void Enqueue(FloodFillRange r)
		{
			if (this.count + this.head == this.array.Length)
			{
				FloodFillRange[] destinationArray = new FloodFillRange[2 * this.array.Length];
				Array.Copy(this.array, this.head, destinationArray, 0, this.count);
				this.array = destinationArray;
				this.head = 0;
				this.debugNumTimesExpanded++;
			}
			FloodFillRange[] array = this.array;
			int num = this.head;
			int num2 = this.count;
			this.count = num2 + 1;
			array[num + num2] = r;
			this.debugMaxUsedSpace = this.count + this.head;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0005CA6C File Offset: 0x0005AC6C
		public FloodFillRange Dequeue()
		{
			FloodFillRange result = default(FloodFillRange);
			if (this.count > 0)
			{
				result = this.array[this.head];
				this.array[this.head] = default(FloodFillRange);
				this.head++;
				this.count--;
			}
			return result;
		}

		// Token: 0x04000E21 RID: 3617
		private FloodFillRange[] array;

		// Token: 0x04000E22 RID: 3618
		private int count;

		// Token: 0x04000E23 RID: 3619
		private int head;

		// Token: 0x04000E24 RID: 3620
		private int debugNumTimesExpanded;

		// Token: 0x04000E25 RID: 3621
		private int debugMaxUsedSpace;
	}
}

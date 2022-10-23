using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000454 RID: 1108
	public class DebugHistogram
	{
		// Token: 0x0600222C RID: 8748 RVA: 0x000D9F6D File Offset: 0x000D816D
		public DebugHistogram(float[] buckets)
		{
			this.buckets = buckets.Concat(float.PositiveInfinity).ToArray<float>();
			this.counts = new int[this.buckets.Length];
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x000D9FA0 File Offset: 0x000D81A0
		public void Add(float val)
		{
			for (int i = 0; i < this.buckets.Length; i++)
			{
				if (this.buckets[i] > val)
				{
					this.counts[i]++;
					return;
				}
			}
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x000D9FE0 File Offset: 0x000D81E0
		public void Display()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Display(stringBuilder);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x000DA008 File Offset: 0x000D8208
		public void Display(StringBuilder sb)
		{
			int num = Mathf.Max(this.counts.Max(), 1);
			int num2 = this.counts.Aggregate((int a, int b) => a + b);
			for (int i = 0; i < this.buckets.Length; i++)
			{
				sb.AppendLine(string.Format("{0}    {1}: {2} ({3:F2}%)", new object[]
				{
					new string('#', this.counts[i] * 40 / num),
					this.buckets[i],
					this.counts[i],
					(double)this.counts[i] * 100.0 / (double)num2
				}));
			}
		}

		// Token: 0x040015BD RID: 5565
		private float[] buckets;

		// Token: 0x040015BE RID: 5566
		private int[] counts;
	}
}

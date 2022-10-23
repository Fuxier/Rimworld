using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000022 RID: 34
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00008150 File Offset: 0x00006350
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00008159 File Offset: 0x00006359
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00008162 File Offset: 0x00006362
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00008175 File Offset: 0x00006375
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00008188 File Offset: 0x00006388
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000819F File Offset: 0x0000639F
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000081B2 File Offset: 0x000063B2
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000081C2 File Offset: 0x000063C2
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000081E0 File Offset: 0x000063E0
		public static IntRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split(new char[]
			{
				'~'
			});
			if (array.Length == 1)
			{
				int num = Convert.ToInt32(array[0], invariantCulture);
				return new IntRange(num, num);
			}
			int num2 = array[0].NullOrEmpty() ? int.MinValue : Convert.ToInt32(array[0], invariantCulture);
			int num3 = array[1].NullOrEmpty() ? int.MaxValue : Convert.ToInt32(array[1], invariantCulture);
			return new IntRange(num2, num3);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008256 File Offset: 0x00006456
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008278 File Offset: 0x00006478
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000828B File Offset: 0x0000648B
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000082A3 File Offset: 0x000064A3
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000082C3 File Offset: 0x000064C3
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000082CD File Offset: 0x000064CD
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000082D9 File Offset: 0x000064D9
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}

		// Token: 0x0400005A RID: 90
		public int min;

		// Token: 0x0400005B RID: 91
		public int max;
	}
}

using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000021 RID: 33
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00007ED6 File Offset: 0x000060D6
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00007EE7 File Offset: 0x000060E7
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00007EF8 File Offset: 0x000060F8
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00007F09 File Offset: 0x00006109
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00007F1E File Offset: 0x0000611E
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00007F31 File Offset: 0x00006131
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00007F44 File Offset: 0x00006144
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00007F57 File Offset: 0x00006157
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00007F66 File Offset: 0x00006166
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00007F76 File Offset: 0x00006176
		public float ClampToRange(float value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007F8A File Offset: 0x0000618A
		public float RandomInRangeSeeded(int seed)
		{
			return Rand.RangeSeeded(this.min, this.max, seed);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007F9E File Offset: 0x0000619E
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007FB2 File Offset: 0x000061B2
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00007FC6 File Offset: 0x000061C6
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007FDF File Offset: 0x000061DF
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008004 File Offset: 0x00006204
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000801B File Offset: 0x0000621B
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000803B File Offset: 0x0000623B
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000805E File Offset: 0x0000625E
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00008075 File Offset: 0x00006275
		public static FloatRange operator *(float val, FloatRange r)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000808C File Offset: 0x0000628C
		public static FloatRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split(new char[]
			{
				'~'
			});
			if (array.Length == 1)
			{
				float num = Convert.ToSingle(array[0], invariantCulture);
				return new FloatRange(num, num);
			}
			return new FloatRange(Convert.ToSingle(array[0], invariantCulture), Convert.ToSingle(array[1], invariantCulture));
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000080DE File Offset: 0x000062DE
		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00008100 File Offset: 0x00006300
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00008118 File Offset: 0x00006318
		public override bool Equals(object obj)
		{
			return obj is FloatRange && this.Equals((FloatRange)obj);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00008130 File Offset: 0x00006330
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x04000058 RID: 88
		public float min;

		// Token: 0x04000059 RID: 89
		public float max;
	}
}

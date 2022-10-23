using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001E RID: 30
	public struct ColorInt : IEquatable<ColorInt>
	{
		// Token: 0x060000FD RID: 253 RVA: 0x00007853 File Offset: 0x00005A53
		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00007875 File Offset: 0x00005A75
		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00007894 File Offset: 0x00005A94
		public ColorInt(Color32 col)
		{
			this.r = (int)col.r;
			this.g = (int)col.g;
			this.b = (int)col.b;
			this.a = (int)col.a;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000078C8 File Offset: 0x00005AC8
		public ColorInt(Color color)
		{
			this.r = (int)ColorInt.FloatToByte(color.r);
			this.g = (int)ColorInt.FloatToByte(color.g);
			this.b = (int)ColorInt.FloatToByte(color.b);
			this.a = (int)ColorInt.FloatToByte(color.a);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007919 File Offset: 0x00005B19
		private static byte FloatToByte(float value)
		{
			if (value >= 1f)
			{
				return byte.MaxValue;
			}
			if (value <= 0f)
			{
				return 0;
			}
			return (byte)Mathf.Floor(value * 256f);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00007940 File Offset: 0x00005B40
		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000797B File Offset: 0x00005B7B
		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + (int)colB.r, colA.g + (int)colB.g, colA.b + (int)colB.b, colA.a + (int)colB.a);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000079B6 File Offset: 0x00005BB6
		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000079F1 File Offset: 0x00005BF1
		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007A18 File Offset: 0x00005C18
		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00007A47 File Offset: 0x00005C47
		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007A6E File Offset: 0x00005C6E
		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007A9D File Offset: 0x00005C9D
		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00007AD9 File Offset: 0x00005CD9
		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007B18 File Offset: 0x00005D18
		public override bool Equals(object o)
		{
			return o is ColorInt && this.Equals((ColorInt)o);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007B30 File Offset: 0x00005D30
		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007B40 File Offset: 0x00005D40
		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007B8C File Offset: 0x00005D8C
		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}", new object[]
			{
				this.r,
				this.g,
				this.b,
				this.a
			});
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public void ClampToNonNegative()
		{
			if (this.r < 0)
			{
				this.r = 0;
			}
			if (this.g < 0)
			{
				this.g = 0;
			}
			if (this.b < 0)
			{
				this.b = 0;
			}
			if (this.a < 0)
			{
				this.a = 0;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00007C34 File Offset: 0x00005E34
		public Color ToColor
		{
			get
			{
				return new Color
				{
					r = (float)this.r / 255f,
					g = (float)this.g / 255f,
					b = (float)this.b / 255f,
					a = (float)this.a / 255f
				};
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00007C9C File Offset: 0x00005E9C
		public Color32 ProjectToColor32
		{
			get
			{
				Color32 result = default(Color32);
				if (this.a > 255)
				{
					result.a = byte.MaxValue;
				}
				else
				{
					result.a = (byte)this.a;
				}
				int num = Mathf.Max(new int[]
				{
					this.r,
					this.g,
					this.b
				});
				if (num > 255)
				{
					result.r = (byte)(this.r * 255 / num);
					result.g = (byte)(this.g * 255 / num);
					result.b = (byte)(this.b * 255 / num);
				}
				else
				{
					result.r = (byte)this.r;
					result.g = (byte)this.g;
					result.b = (byte)this.b;
				}
				return result;
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007D78 File Offset: 0x00005F78
		public void SetHueSaturation(float hue, float sat)
		{
			float v = (float)Mathf.Max(new int[]
			{
				this.r,
				this.g,
				this.b
			}) / 255f;
			ColorInt colorInt = ColorInt.FromHdrColor(Color.HSVToRGB(hue, sat, v, true), null);
			this.r = colorInt.r;
			this.g = colorInt.g;
			this.b = colorInt.b;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007DF0 File Offset: 0x00005FF0
		public static ColorInt FromHdrColor(Color color, float? alphaOverride = null)
		{
			return new ColorInt
			{
				r = Mathf.RoundToInt(color.r * 255f),
				g = Mathf.RoundToInt(color.g * 255f),
				b = Mathf.RoundToInt(color.b * 255f),
				a = Mathf.RoundToInt((alphaOverride ?? color.a) * 255f)
			};
		}

		// Token: 0x04000053 RID: 83
		public int r;

		// Token: 0x04000054 RID: 84
		public int g;

		// Token: 0x04000055 RID: 85
		public int b;

		// Token: 0x04000056 RID: 86
		public int a;
	}
}

using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000032 RID: 50
	public static class GenColor
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000D784 File Offset: 0x0000B984
		public static Color SaturationChanged(this Color col, float change)
		{
			float num = col.r;
			float num2 = col.g;
			float num3 = col.b;
			float num4 = Mathf.Sqrt(num * num * 0.299f + num2 * num2 * 0.587f + num3 * num3 * 0.114f);
			num = num4 + (num - num4) * change;
			num2 = num4 + (num2 - num4) * change;
			num3 = num4 + (num3 - num4) * change;
			return new Color(num, num2, num3);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000D7EC File Offset: 0x0000B9EC
		public static bool IndistinguishableFromFast(this Color colA, Color colB)
		{
			return Mathf.Abs(colA.r - colB.r) + Mathf.Abs(colA.g - colB.g) + Mathf.Abs(colA.b - colB.b) + Mathf.Abs(colA.a - colB.a) < 0.005f;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000D84C File Offset: 0x0000BA4C
		public static bool IndistinguishableFrom(this Color colA, Color colB)
		{
			if (GenColor.Colors32Equal(colA, colB))
			{
				return true;
			}
			Color color = colA - colB;
			return Mathf.Abs(color.r) + Mathf.Abs(color.g) + Mathf.Abs(color.b) + Mathf.Abs(color.a) < 0.005f;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000D8A4 File Offset: 0x0000BAA4
		public static bool WithinDiffThresholdFrom(this Color colA, Color colB, float threshold)
		{
			Color color = colA - colB;
			return Mathf.Abs(color.r) + Mathf.Abs(color.g) + Mathf.Abs(color.b) + Mathf.Abs(color.a) < threshold;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000D8EC File Offset: 0x0000BAEC
		public static bool Colors32Equal(Color a, Color b)
		{
			Color32 color = a;
			Color32 color2 = b;
			return color.r == color2.r && color.g == color2.g && color.b == color2.b && color.a == color2.a;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000D941 File Offset: 0x0000BB41
		public static Color RandomColorOpaque()
		{
			return new Color(Rand.Value, Rand.Value, Rand.Value, 1f);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000D95C File Offset: 0x0000BB5C
		public static Color FromBytes(int r, int g, int b, int a = 255)
		{
			return new Color
			{
				r = (float)r / 255f,
				g = (float)g / 255f,
				b = (float)b / 255f,
				a = (float)a / 255f
			};
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000D9B0 File Offset: 0x0000BBB0
		public static Color FromHex(string hex)
		{
			if (hex.StartsWith("#"))
			{
				hex = hex.Substring(1);
			}
			if (hex.Length != 6 && hex.Length != 8)
			{
				Log.Error(hex + " is not a valid hex color.");
				return Color.white;
			}
			int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			int a = 255;
			if (hex.Length == 8)
			{
				a = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
			}
			return GenColor.FromBytes(r, g, b, a);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000DA60 File Offset: 0x0000BC60
		public static Color GetDominantColor(this Texture2D texture, int buckets = 25, float minBrightness = 0f)
		{
			if (texture == BaseContent.BadTex)
			{
				return Color.white;
			}
			if (GenColor.tmpBuckets == null || GenColor.tmpBuckets.GetLength(0) != buckets)
			{
				GenColor.tmpBuckets = new float[buckets, buckets, buckets];
			}
			for (int i = 0; i < texture.width; i++)
			{
				for (int j = 0; j < texture.height; j++)
				{
					Color pixel = texture.GetPixel(i, j);
					if ((pixel.r + pixel.g + pixel.b) / 3f >= minBrightness)
					{
						GenColor.tmpBuckets[Mathf.Clamp((int)(pixel.r * (float)buckets), 0, buckets - 1), Mathf.Clamp((int)(pixel.g * (float)buckets), 0, buckets - 1), Mathf.Clamp((int)(pixel.b * (float)buckets), 0, buckets - 1)] += pixel.a;
					}
				}
			}
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int k = 0; k < buckets; k++)
			{
				for (int l = 0; l < buckets; l++)
				{
					for (int m = 0; m < buckets; m++)
					{
						if (GenColor.tmpBuckets[k, l, m] > num)
						{
							num = GenColor.tmpBuckets[k, l, m];
							num2 = k;
							num3 = l;
							num4 = m;
						}
					}
				}
			}
			return new Color(((float)num2 + 0.5f) / (float)buckets, ((float)num3 + 0.5f) / (float)buckets, ((float)num4 + 0.5f) / (float)buckets);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000DBE4 File Offset: 0x0000BDE4
		public static Color ClampToValueRange(this Color color, FloatRange range)
		{
			float h;
			float s;
			float num;
			Color.RGBToHSV(color, out h, out s, out num);
			num = range.ClampToRange(num);
			color = Color.HSVToRGB(h, s, num);
			return color;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000DC14 File Offset: 0x0000BE14
		public static Color FromColorTemperature(float temperatureKelvin)
		{
			float num = temperatureKelvin / 100f;
			float value;
			float value2;
			float value3;
			if (num <= 66f)
			{
				value = 1f;
				value2 = 0.39008158f * Mathf.Log(num) - 0.6318414f;
				if (num <= 19f)
				{
					value3 = 0f;
				}
				else
				{
					value3 = 0.5432068f * Mathf.Log(num - 10f) - 1.1962541f;
				}
			}
			else
			{
				num -= 60f;
				value = 1.2929362f * Mathf.Pow(num, -0.13320476f);
				value2 = 1.1298909f * Mathf.Pow(num, -0.075514846f);
				value3 = 1f;
			}
			return new Color(Mathf.Clamp01(value), Mathf.Clamp01(value2), Mathf.Clamp01(value3));
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000DCC0 File Offset: 0x0000BEC0
		public static float? ColorTemperature(this Color color)
		{
			float num = Mathf.Max(new float[]
			{
				color.r,
				color.g,
				color.b
			});
			if (num == 0f)
			{
				return null;
			}
			float num2 = color.r / num;
			float num3 = color.g / num;
			float num4 = color.b / num;
			if (num2 == 1f && num3 == 1f && num4 == 1f)
			{
				return new float?(6600f);
			}
			float num7;
			if (num4 < 1f)
			{
				if (num2 < 1f)
				{
					return null;
				}
				float num5 = Mathf.Exp((num3 + 0.6318414f) / 0.39008158f);
				float num6;
				if (num4 == 0f)
				{
					num6 = Mathf.Min(19f, num5);
				}
				else
				{
					num6 = Mathf.Exp((num4 + 1.1962541f) / 0.5432068f) + 10f;
				}
				if (Mathf.Abs(num6 - num5) >= 1f)
				{
					return null;
				}
				num7 = 50f * num5 + 50f * num6;
			}
			else
			{
				float num8 = Mathf.Exp(Mathf.Log(num3 / 1.1298909f) / -0.075514846f) + 60f;
				float num9;
				if (num2 == 1f)
				{
					num9 = Mathf.Min(66.98f, Mathf.Max(66f, num8));
				}
				else
				{
					num9 = Mathf.Exp(Mathf.Log(num2 / 1.2929362f) / -0.13320476f) + 60f;
				}
				if (Mathf.Abs(num9 - num8) >= 1f)
				{
					return null;
				}
				num7 = 50f * num8 + 50f * num9;
			}
			if (num7 >= 900f && num7 <= 40100f)
			{
				num7 = Mathf.Clamp(num7, 1000f, 40000f);
				return new float?(num7);
			}
			return null;
		}

		// Token: 0x04000094 RID: 148
		private static float[,,] tmpBuckets;

		// Token: 0x04000095 RID: 149
		private const float redScaleFactor = 1.2929362f;

		// Token: 0x04000096 RID: 150
		private const float redPowerFactor = -0.13320476f;

		// Token: 0x04000097 RID: 151
		private const float blueScaleFactor = 0.5432068f;

		// Token: 0x04000098 RID: 152
		private const float blueOffset = 1.1962541f;

		// Token: 0x04000099 RID: 153
		private const float coolGreenScale = 0.39008158f;

		// Token: 0x0400009A RID: 154
		private const float coolGreenOffset = 0.6318414f;

		// Token: 0x0400009B RID: 155
		private const float warmGreenScale = 1.1298909f;

		// Token: 0x0400009C RID: 156
		private const float warmGreenPower = -0.075514846f;

		// Token: 0x0400009D RID: 157
		public const float minColorTemperature = 1000f;

		// Token: 0x0400009E RID: 158
		public const float maxColorTemperature = 40000f;

		// Token: 0x0400009F RID: 159
		public const float whiteColorTemperature = 6600f;
	}
}

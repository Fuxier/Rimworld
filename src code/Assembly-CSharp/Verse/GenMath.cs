using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000038 RID: 56
	public static class GenMath
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x0000E640 File Offset: 0x0000C840
		public static float RoundedHundredth(float f)
		{
			return Mathf.Round(f * 100f) / 100f;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000E654 File Offset: 0x0000C854
		public static int RoundTo(int value, int roundToNearest)
		{
			return (int)Math.Round((double)((float)value / (float)roundToNearest)) * roundToNearest;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E664 File Offset: 0x0000C864
		public static float RoundTo(float value, float roundToNearest)
		{
			return (float)((int)Math.Round((double)(value / roundToNearest))) * roundToNearest;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000E674 File Offset: 0x0000C874
		public static float ChanceEitherHappens(float chanceA, float chanceB)
		{
			return chanceA + (1f - chanceA) * chanceB;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000E681 File Offset: 0x0000C881
		public static float SmootherStep(float edge0, float edge1, float x)
		{
			x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
			return x * x * x * (x * (x * 6f - 15f) + 10f);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000E6AC File Offset: 0x0000C8AC
		public static int RoundRandom(float f)
		{
			return (int)f + ((Rand.Value < f % 1f) ? 1 : 0);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000E6C3 File Offset: 0x0000C8C3
		public static float WeightedAverage(float A, float weightA, float B, float weightB)
		{
			return (A * weightA + B * weightB) / (weightA + weightB);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000E6D0 File Offset: 0x0000C8D0
		public static float Median<T>(IList<T> list, Func<T, float> orderBy, float noneValue = 0f, float center = 0.5f)
		{
			if (list.NullOrEmpty<T>())
			{
				return noneValue;
			}
			GenMath.tmpElements.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				GenMath.tmpElements.Add(orderBy(list[i]));
			}
			GenMath.tmpElements.Sort();
			return GenMath.tmpElements[Mathf.Min(Mathf.FloorToInt((float)GenMath.tmpElements.Count * center), GenMath.tmpElements.Count - 1)];
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000E750 File Offset: 0x0000C950
		public static float WeightedMedian(IList<Pair<float, float>> list, float noneValue = 0f, float center = 0.5f)
		{
			GenMath.tmpPairs.Clear();
			GenMath.tmpPairs.AddRange(list);
			float num = 0f;
			for (int i = 0; i < GenMath.tmpPairs.Count; i++)
			{
				float second = GenMath.tmpPairs[i].Second;
				if (second < 0f)
				{
					Log.ErrorOnce("Negative weight in WeightedMedian: " + second, GenMath.tmpPairs.GetHashCode());
				}
				else
				{
					num += second;
				}
			}
			if (num <= 0f)
			{
				return noneValue;
			}
			GenMath.tmpPairs.SortBy((Pair<float, float> x) => x.First);
			float num2 = 0f;
			for (int j = 0; j < GenMath.tmpPairs.Count; j++)
			{
				float first = GenMath.tmpPairs[j].First;
				float second2 = GenMath.tmpPairs[j].Second;
				num2 += second2 / num;
				if (num2 >= center)
				{
					return first;
				}
			}
			return GenMath.tmpPairs.Last<Pair<float, float>>().First;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000E870 File Offset: 0x0000CA70
		public static float Sqrt(float f)
		{
			return (float)Math.Sqrt((double)f);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E87C File Offset: 0x0000CA7C
		public static float LerpDouble(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			float num = (x - inFrom) / (inTo - inFrom);
			return outFrom + (outTo - outFrom) * num;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000E899 File Offset: 0x0000CA99
		public static float LerpDoubleClamped(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			return GenMath.LerpDouble(inFrom, inTo, outFrom, outTo, Mathf.Clamp(x, Mathf.Min(inFrom, inTo), Mathf.Max(inFrom, inTo)));
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000E8B9 File Offset: 0x0000CAB9
		public static float Reflection(float value, float mirror)
		{
			return mirror - (value - mirror);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000E8C0 File Offset: 0x0000CAC0
		public static Quaternion ToQuat(this float ang)
		{
			return Quaternion.AngleAxis(ang, Vector3.up);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000E8D0 File Offset: 0x0000CAD0
		public static float GetFactorInInterval(float min, float mid, float max, float power, float x)
		{
			if (min > max)
			{
				return 0f;
			}
			if (x <= min || x >= max)
			{
				return 0f;
			}
			if (x == mid)
			{
				return 1f;
			}
			float f;
			if (x < mid)
			{
				f = 1f - (mid - x) / (mid - min);
			}
			else
			{
				f = 1f - (x - mid) / (max - mid);
			}
			return Mathf.Pow(f, power);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000E934 File Offset: 0x0000CB34
		public static float FlatHill(float min, float lower, float upper, float max, float x)
		{
			if (x < min)
			{
				return 0f;
			}
			if (x < lower)
			{
				return Mathf.InverseLerp(min, lower, x);
			}
			if (x < upper)
			{
				return 1f;
			}
			if (x < max)
			{
				return Mathf.InverseLerp(max, upper, x);
			}
			return 0f;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000E970 File Offset: 0x0000CB70
		public static float FlatHill(float minY, float min, float lower, float upper, float max, float maxY, float x)
		{
			if (x < min)
			{
				return minY;
			}
			if (x < lower)
			{
				return GenMath.LerpDouble(min, lower, minY, 1f, x);
			}
			if (x < upper)
			{
				return 1f;
			}
			if (x < max)
			{
				return GenMath.LerpDouble(upper, max, 1f, maxY, x);
			}
			return maxY;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000E9BE File Offset: 0x0000CBBE
		public static int OctileDistance(int dx, int dz, int cardinal, int diagonal)
		{
			return cardinal * (dx + dz) + (diagonal - 2 * cardinal) * Mathf.Min(dx, dz);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000E9D3 File Offset: 0x0000CBD3
		public static float UnboundedValueToFactor(float val)
		{
			if (val > 0f)
			{
				return 1f + val;
			}
			return 1f / (1f - val);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000E9F4 File Offset: 0x0000CBF4
		[DebugOutput("System", false)]
		public static void TestMathPerf()
		{
			IntVec3 intVec = new IntVec3(72, 0, 65);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Math perf tests (" + 10000000f + " tests each)");
			float num = 0f;
			Stopwatch stopwatch = Stopwatch.StartNew();
			int num2 = 0;
			while ((float)num2 < 10000000f)
			{
				num += (float)Math.Sqrt(101.20999908447266);
				num2++;
			}
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"(float)System.Math.Sqrt(",
				101.21f,
				"): ",
				stopwatch.ElapsedTicks
			}));
			Stopwatch stopwatch2 = Stopwatch.StartNew();
			int num3 = 0;
			while ((float)num3 < 10000000f)
			{
				num += Mathf.Sqrt(101.21f);
				num3++;
			}
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"UnityEngine.Mathf.Sqrt(",
				101.21f,
				"): ",
				stopwatch2.ElapsedTicks
			}));
			Stopwatch stopwatch3 = Stopwatch.StartNew();
			int num4 = 0;
			while ((float)num4 < 10000000f)
			{
				num += GenMath.Sqrt(101.21f);
				num4++;
			}
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Verse.GenMath.Sqrt(",
				101.21f,
				"): ",
				stopwatch3.ElapsedTicks
			}));
			Stopwatch stopwatch4 = Stopwatch.StartNew();
			int num5 = 0;
			while ((float)num5 < 10000000f)
			{
				num += (float)intVec.LengthManhattan;
				num5++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthManhattan: " + stopwatch4.ElapsedTicks);
			Stopwatch stopwatch5 = Stopwatch.StartNew();
			int num6 = 0;
			while ((float)num6 < 10000000f)
			{
				num += intVec.LengthHorizontal;
				num6++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontal: " + stopwatch5.ElapsedTicks);
			Stopwatch stopwatch6 = Stopwatch.StartNew();
			int num7 = 0;
			while ((float)num7 < 10000000f)
			{
				num += (float)intVec.LengthHorizontalSquared;
				num7++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontalSquared: " + stopwatch6.ElapsedTicks);
			stringBuilder.AppendLine("total: " + num);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000EC5E File Offset: 0x0000CE5E
		public static float Min(float a, float b, float c)
		{
			if (a < b)
			{
				if (a < c)
				{
					return a;
				}
				return c;
			}
			else
			{
				if (b < c)
				{
					return b;
				}
				return c;
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000EC73 File Offset: 0x0000CE73
		public static int Max(int a, int b, int c)
		{
			if (a > b)
			{
				if (a > c)
				{
					return a;
				}
				return c;
			}
			else
			{
				if (b > c)
				{
					return b;
				}
				return c;
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000EC88 File Offset: 0x0000CE88
		public static float SphericalDistance(Vector3 normalizedA, Vector3 normalizedB)
		{
			if (normalizedA == normalizedB)
			{
				return 0f;
			}
			return Mathf.Acos(Vector3.Dot(normalizedA, normalizedB));
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000ECA8 File Offset: 0x0000CEA8
		public static void DHondtDistribution(List<int> candidates, Func<int, float> scoreGetter, int numToDistribute)
		{
			GenMath.tmpScores.Clear();
			GenMath.tmpCalcList.Clear();
			for (int i = 0; i < candidates.Count; i++)
			{
				float item = scoreGetter(i);
				candidates[i] = 0;
				GenMath.tmpScores.Add(item);
				GenMath.tmpCalcList.Add(item);
			}
			for (int j = 0; j < numToDistribute; j++)
			{
				int num = GenMath.tmpCalcList.IndexOf(GenMath.tmpCalcList.Max());
				int index = num;
				int num2 = candidates[index];
				candidates[index] = num2 + 1;
				GenMath.tmpCalcList[num] = GenMath.tmpScores[num] / ((float)candidates[num] + 1f);
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000ED5F File Offset: 0x0000CF5F
		public static int PositiveMod(int x, int m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000ED5F File Offset: 0x0000CF5F
		public static long PositiveMod(long x, long m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000ED5F File Offset: 0x0000CF5F
		public static float PositiveMod(float x, float m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000ED68 File Offset: 0x0000CF68
		public static int PositiveModRemap(long x, int d, int m)
		{
			if (x < 0L)
			{
				x -= (long)(d - 1);
			}
			return (int)((x / (long)d % (long)m + (long)m) % (long)m);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000ED85 File Offset: 0x0000CF85
		public static Vector3 BezierCubicEvaluate(float t, GenMath.BezierCubicControls bcc)
		{
			return GenMath.BezierCubicEvaluate(t, bcc.w0, bcc.w1, bcc.w2, bcc.w3);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
		public static Vector3 BezierCubicEvaluate(float t, Vector3 w0, Vector3 w1, Vector3 w2, Vector3 w3)
		{
			float d = t * t;
			float num = 1f - t;
			float d2 = num * num;
			return w0 * d2 * num + 3f * w1 * d2 * t + 3f * w2 * num * d + w3 * d * t;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000EE20 File Offset: 0x0000D020
		public static float CirclesOverlapArea(float x1, float y1, float r1, float x2, float y2, float r2)
		{
			float num = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
			float num2 = Mathf.Sqrt(num);
			float num3 = r1 * r1;
			float num4 = r2 * r2;
			float num5 = Mathf.Abs(r1 - r2);
			if (num2 >= r1 + r2)
			{
				return 0f;
			}
			if (num2 <= num5 && r1 >= r2)
			{
				return 3.1415927f * num4;
			}
			if (num2 <= num5 && r2 >= r1)
			{
				return 3.1415927f * num3;
			}
			float num6 = Mathf.Acos((num3 - num4 + num) / (2f * r1 * num2)) * 2f;
			float num7 = Mathf.Acos((num4 - num3 + num) / (2f * r2 * num2)) * 2f;
			float num8 = (num7 * num4 - num4 * Mathf.Sin(num7)) * 0.5f;
			float num9 = (num6 * num3 - num3 * Mathf.Sin(num6)) * 0.5f;
			return num8 + num9;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000EEF2 File Offset: 0x0000D0F2
		public static bool AnyIntegerInRange(float min, float max)
		{
			return Mathf.Ceil(min) <= max;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000EF00 File Offset: 0x0000D100
		public static void NormalizeToSum1(ref float a, ref float b, ref float c)
		{
			float num = a + b + c;
			if (num == 0f)
			{
				a = 1f;
				b = 0f;
				c = 0f;
				return;
			}
			a /= num;
			b /= num;
			c /= num;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000EF46 File Offset: 0x0000D146
		public static float InverseLerp(float a, float b, float value)
		{
			if (a != b)
			{
				return Mathf.InverseLerp(a, b, value);
			}
			if (value >= a)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000EF64 File Offset: 0x0000D164
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			if (by1 >= by2 && by1 >= by3)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3)
			{
				return elem2;
			}
			return elem3;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000EF7E File Offset: 0x0000D17E
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4)
			{
				return elem3;
			}
			return elem4;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5)
			{
				return elem4;
			}
			return elem5;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000F024 File Offset: 0x0000D224
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6)
			{
				return elem5;
			}
			return elem6;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F0C4 File Offset: 0x0000D2C4
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7)
			{
				return elem5;
			}
			if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7)
			{
				return elem6;
			}
			return elem7;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F1A8 File Offset: 0x0000D3A8
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7 && by1 >= by8)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7 && by2 >= by8)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7 && by3 >= by8)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7 && by4 >= by8)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7 && by5 >= by8)
			{
				return elem5;
			}
			if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7 && by6 >= by8)
			{
				return elem6;
			}
			if (by7 >= by1 && by7 >= by2 && by7 >= by3 && by7 >= by4 && by7 >= by5 && by7 >= by6 && by7 >= by8)
			{
				return elem7;
			}
			return elem8;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F2D6 File Offset: 0x0000D4D6
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F2E8 File Offset: 0x0000D4E8
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F300 File Offset: 0x0000D500
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F328 File Offset: 0x0000D528
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F354 File Offset: 0x0000D554
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F388 File Offset: 0x0000D588
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7, elem8, -by8);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F3C0 File Offset: 0x0000D5C0
		public static T MaxByRandomIfEqual<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8, float eps = 0.0001f)
		{
			return GenMath.MaxBy<T>(elem1, by1 + Rand.Range(0f, eps), elem2, by2 + Rand.Range(0f, eps), elem3, by3 + Rand.Range(0f, eps), elem4, by4 + Rand.Range(0f, eps), elem5, by5 + Rand.Range(0f, eps), elem6, by6 + Rand.Range(0f, eps), elem7, by7 + Rand.Range(0f, eps), elem8, by8 + Rand.Range(0f, eps));
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F458 File Offset: 0x0000D658
		public static float Stddev(IEnumerable<float> data)
		{
			int num = 0;
			double num2 = 0.0;
			double num3 = 0.0;
			foreach (float num4 in data)
			{
				num++;
				num2 += (double)num4;
				num3 += (double)(num4 * num4);
			}
			double num5 = num2 / (double)num;
			return Mathf.Sqrt((float)(num3 / (double)num - num5 * num5));
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F4DC File Offset: 0x0000D6DC
		public static float InverseParabola(float x)
		{
			x = Mathf.Clamp01(x);
			return -4f * x * (x - 1f);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F4F8 File Offset: 0x0000D6F8
		public static float ExponentialWarpInterpolation(float min, float max, float fraction, Vector2 setPoint)
		{
			float p = Mathf.Log(Mathf.InverseLerp(min, max, setPoint.y), setPoint.x);
			float t = Mathf.Pow(fraction, p);
			return Mathf.Lerp(min, max, t);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000F530 File Offset: 0x0000D730
		public static float InverseExponentialWarpInterpolation(float min, float max, float value, Vector2 setPoint)
		{
			float num = Mathf.Log(Mathf.InverseLerp(min, max, setPoint.y), setPoint.x);
			return Mathf.Exp(Mathf.Log(Mathf.InverseLerp(min, max, value)) / num);
		}

		// Token: 0x040000A1 RID: 161
		public const float BigEpsilon = 1E-07f;

		// Token: 0x040000A2 RID: 162
		public const float Sqrt2 = 1.4142135f;

		// Token: 0x040000A3 RID: 163
		private static List<float> tmpElements = new List<float>();

		// Token: 0x040000A4 RID: 164
		private static List<Pair<float, float>> tmpPairs = new List<Pair<float, float>>();

		// Token: 0x040000A5 RID: 165
		private static List<float> tmpScores = new List<float>();

		// Token: 0x040000A6 RID: 166
		private static List<float> tmpCalcList = new List<float>();

		// Token: 0x02001C6C RID: 7276
		public struct BezierCubicControls
		{
			// Token: 0x04006FFD RID: 28669
			public Vector3 w0;

			// Token: 0x04006FFE RID: 28670
			public Vector3 w1;

			// Token: 0x04006FFF RID: 28671
			public Vector3 w2;

			// Token: 0x04007000 RID: 28672
			public Vector3 w3;
		}
	}
}

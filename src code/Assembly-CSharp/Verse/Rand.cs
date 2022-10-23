using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000055 RID: 85
	public static class Rand
	{
		// Token: 0x17000091 RID: 145
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00016509 File Offset: 0x00014709
		public static int Seed
		{
			set
			{
				if (Rand.stateStack.Count == 0)
				{
					Log.ErrorOnce("Modifying the initial rand seed. Call PushState() first. The initial rand seed should always be based on the startup time and set only once.", 825343540);
				}
				Rand.seed = (uint)value;
				Rand.iterations = 0U;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00016532 File Offset: 0x00014732
		public static float Value
		{
			get
			{
				return (float)(((double)MurmurHash.GetInt(Rand.seed, Rand.iterations++) - -2147483648.0) / 4294967295.0);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00016561 File Offset: 0x00014761
		public static bool Bool
		{
			get
			{
				return Rand.Value < 0.5f;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001656F File Offset: 0x0001476F
		public static int Sign
		{
			get
			{
				if (!Rand.Bool)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0001657B File Offset: 0x0001477B
		public static int Int
		{
			get
			{
				return MurmurHash.GetInt(Rand.seed, Rand.iterations++);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00016594 File Offset: 0x00014794
		public static Vector3 UnitVector3
		{
			get
			{
				return new Vector3(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x000165DC File Offset: 0x000147DC
		public static Vector2 UnitVector2
		{
			get
			{
				return new Vector2(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00016614 File Offset: 0x00014814
		public static Vector2 InsideUnitCircle
		{
			get
			{
				Vector2 result;
				do
				{
					result = new Vector2(Rand.Value - 0.5f, Rand.Value - 0.5f) * 2f;
				}
				while (result.sqrMagnitude > 1f);
				return result;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00016658 File Offset: 0x00014858
		public static Vector3 InsideUnitCircleVec3
		{
			get
			{
				Vector2 insideUnitCircle = Rand.InsideUnitCircle;
				return new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00016681 File Offset: 0x00014881
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x00016693 File Offset: 0x00014893
		private static ulong StateCompressed
		{
			get
			{
				return (ulong)Rand.seed | (ulong)Rand.iterations << 32;
			}
			set
			{
				Rand.seed = (uint)(value & (ulong)-1);
				Rand.iterations = (uint)(value >> 32 & (ulong)-1);
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000166AC File Offset: 0x000148AC
		static Rand()
		{
			Rand.seed = (uint)DateTime.Now.GetHashCode();
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000166E5 File Offset: 0x000148E5
		public static void EnsureStateStackEmpty()
		{
			if (Rand.stateStack.Count > 0)
			{
				Log.Warning("Random state stack is not empty. There were more calls to PushState than PopState. Fixing.");
				while (Rand.stateStack.Any<ulong>())
				{
					Rand.PopState();
				}
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00016714 File Offset: 0x00014914
		public static Vector2 InsideAnnulus(float innerRadius, float outerRadius)
		{
			float f = 6.2831855f * Rand.Value;
			Vector2 a = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
			innerRadius *= innerRadius;
			outerRadius *= outerRadius;
			return Mathf.Sqrt(Mathf.Lerp(innerRadius, outerRadius, Rand.Value)) * a;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00016764 File Offset: 0x00014964
		public static Vector3 InsideAnnulusVector3(float innerRadius, float outerRadius)
		{
			Vector2 vector = Rand.InsideAnnulus(innerRadius, outerRadius);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00016790 File Offset: 0x00014990
		public static float Gaussian(float centerX = 0f, float widthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			return Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.2831855f * value2) * widthFactor + centerX;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x000167CC File Offset: 0x000149CC
		public static float GaussianAsymmetric(float centerX = 0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			float num = Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.2831855f * value2);
			if (num <= 0f)
			{
				return num * lowerWidthFactor + centerX;
			}
			return num * upperWidthFactor + centerX;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00016817 File Offset: 0x00014A17
		public static int Range(int min, int max)
		{
			if (max <= min)
			{
				return min;
			}
			return min + Mathf.Abs(Rand.Int % (max - min));
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001682F File Offset: 0x00014A2F
		public static int RangeInclusive(int min, int max)
		{
			if (max <= min)
			{
				return min;
			}
			return Rand.Range(min, max + 1);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00016840 File Offset: 0x00014A40
		public static float Range(float min, float max)
		{
			if (max <= min)
			{
				return min;
			}
			return Rand.Value * (max - min) + min;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00016853 File Offset: 0x00014A53
		public static bool Chance(float chance)
		{
			return chance > 0f && (chance >= 1f || Rand.Value < chance);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00016871 File Offset: 0x00014A71
		public static bool ChanceSeeded(float chance, int specialSeed)
		{
			Rand.PushState(specialSeed);
			bool result = Rand.Chance(chance);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00016884 File Offset: 0x00014A84
		public static float ValueSeeded(int specialSeed)
		{
			Rand.PushState(specialSeed);
			float value = Rand.Value;
			Rand.PopState();
			return value;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00016896 File Offset: 0x00014A96
		public static float RangeSeeded(float min, float max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			float result = Rand.Range(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000168AA File Offset: 0x00014AAA
		public static int RangeSeeded(int min, int max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.Range(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000168BE File Offset: 0x00014ABE
		public static int RangeInclusiveSeeded(int min, int max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.RangeInclusive(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000168D2 File Offset: 0x00014AD2
		public static T Element<T>(T a, T b)
		{
			if (!Rand.Bool)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000168E0 File Offset: 0x00014AE0
		public static T Element<T>(T a, T b, T c)
		{
			float value = Rand.Value;
			if (value < 0.33333f)
			{
				return a;
			}
			if (value < 0.66666f)
			{
				return b;
			}
			return c;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00016908 File Offset: 0x00014B08
		public static T Element<T>(T a, T b, T c, T d)
		{
			float value = Rand.Value;
			if (value < 0.25f)
			{
				return a;
			}
			if (value < 0.5f)
			{
				return b;
			}
			if (value < 0.75f)
			{
				return c;
			}
			return d;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001693C File Offset: 0x00014B3C
		public static T Element<T>(T a, T b, T c, T d, T e)
		{
			float value = Rand.Value;
			if (value < 0.2f)
			{
				return a;
			}
			if (value < 0.4f)
			{
				return b;
			}
			if (value < 0.6f)
			{
				return c;
			}
			if (value < 0.8f)
			{
				return d;
			}
			return e;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001697C File Offset: 0x00014B7C
		public static T Element<T>(T a, T b, T c, T d, T e, T f)
		{
			float value = Rand.Value;
			if (value < 0.16666f)
			{
				return a;
			}
			if (value < 0.33333f)
			{
				return b;
			}
			if (value < 0.5f)
			{
				return c;
			}
			if (value < 0.66666f)
			{
				return d;
			}
			if (value < 0.83333f)
			{
				return e;
			}
			return f;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000169C4 File Offset: 0x00014BC4
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB)
		{
			float num = weightA + weightB;
			if (Rand.Value < weightA / num)
			{
				return a;
			}
			return b;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x000169E4 File Offset: 0x00014BE4
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC)
		{
			float num = weightA + weightB + weightC;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			return c;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00016A14 File Offset: 0x00014C14
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD)
		{
			float num = weightA + weightB + weightC + weightD;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			return d;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00016A54 File Offset: 0x00014C54
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD, T e, float weightE)
		{
			float num = weightA + weightB + weightC + weightD + weightE;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			if (value < (weightA + weightB + weightC + weightD) / num)
			{
				return d;
			}
			return e;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00016AA8 File Offset: 0x00014CA8
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD, T e, float weightE, T f, float weightF)
		{
			float num = weightA + weightB + weightC + weightD + weightE + weightF;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			if (value < (weightA + weightB + weightC + weightD) / num)
			{
				return d;
			}
			if (value < (weightA + weightB + weightC + weightD + weightE) / num)
			{
				return e;
			}
			return f;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00016B12 File Offset: 0x00014D12
		public static void PushState()
		{
			Rand.stateStack.Push(Rand.StateCompressed);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00016B23 File Offset: 0x00014D23
		public static void PushState(int replacementSeed)
		{
			Rand.PushState();
			Rand.Seed = replacementSeed;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00016B30 File Offset: 0x00014D30
		public static void PopState()
		{
			Rand.StateCompressed = Rand.stateStack.Pop();
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00016B44 File Offset: 0x00014D44
		public static float ByCurve(SimpleCurve curve)
		{
			if (curve.PointsCount < 3)
			{
				throw new ArgumentException("curve has < 3 points");
			}
			if (curve[0].y != 0f || curve[curve.PointsCount - 1].y != 0f)
			{
				throw new ArgumentException("curve has start/end point with y != 0");
			}
			float num = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				if (curve[i].y < 0f)
				{
					throw new ArgumentException("curve has point with y < 0");
				}
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
			}
			float num2 = Rand.Range(0f, num);
			for (int j = 0; j < curve.PointsCount - 1; j++)
			{
				float num3 = (curve[j + 1].x - curve[j].x) * (curve[j].y + curve[j + 1].y);
				if (num3 >= num2)
				{
					float num4 = curve[j + 1].x - curve[j].x;
					float y = curve[j].y;
					float y2 = curve[j + 1].y;
					float num5 = num2 / (y + y2);
					if (Rand.Range(0f, (y + y2) / 2f) > Mathf.Lerp(y, y2, num5 / num4))
					{
						num5 = num4 - num5;
					}
					return num5 + curve[j].x;
				}
				num2 -= num3;
			}
			throw new Exception("Reached end of Rand.ByCurve without choosing a point.");
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00016D44 File Offset: 0x00014F44
		public static float ByCurveAverage(SimpleCurve curve)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
				num2 += (curve[i + 1].x - curve[i].x) * (curve[i].x * (2f * curve[i].y + curve[i + 1].y) + curve[i + 1].x * (curve[i].y + 2f * curve[i + 1].y));
			}
			return num2 / num / 3f;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00016E5C File Offset: 0x0001505C
		public static bool MTBEventOccurs(float mtb, float mtbUnit, float checkDuration)
		{
			if (mtb == float.PositiveInfinity)
			{
				return false;
			}
			if (mtb == 0f)
			{
				return true;
			}
			if (mtb < 0f)
			{
				Log.Error("MTBEventOccurs with nevative mtb=" + mtb);
				return true;
			}
			if (mtbUnit <= 0f)
			{
				Log.Error("MTBEventOccurs with mtbUnit=" + mtbUnit);
				return false;
			}
			if (checkDuration <= 0f)
			{
				Log.Error("MTBEventOccurs with checkDuration=" + checkDuration);
				return false;
			}
			double num = (double)checkDuration / ((double)mtb * (double)mtbUnit);
			if (num <= 0.0)
			{
				Log.Error(string.Concat(new object[]
				{
					"chancePerCheck is ",
					num,
					". mtb=",
					mtb,
					", mtbUnit=",
					mtbUnit,
					", checkDuration=",
					checkDuration
				}));
				return false;
			}
			double num2 = 1.0;
			if (num < 0.0001)
			{
				while (num < 0.0001)
				{
					num *= 8.0;
					num2 /= 8.0;
				}
				if ((double)Rand.Value > num2)
				{
					return false;
				}
			}
			return (double)Rand.Value < num;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00016F98 File Offset: 0x00015198
		public static float CombineMTBs(List<float> mtbs)
		{
			if (mtbs.Count == 0)
			{
				return 0f;
			}
			if (mtbs.Count == 1)
			{
				return mtbs[0];
			}
			foreach (float num in mtbs)
			{
				if (num < 0f || float.IsNaN(num))
				{
					Log.Error(string.Format("Unexpected MTB value found while combining MTBs: {0}", num));
					return 0f;
				}
			}
			return (float)(1.0 / mtbs.Aggregate(0.0, (double acc, float mtb) => acc + 1.0 / (double)mtb));
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00017068 File Offset: 0x00015268
		public static float CombineMTBs(float mtb1, float mtb2)
		{
			return (float)(1.0 / (1.0 / (double)mtb1 + 1.0 / (double)mtb2));
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00017090 File Offset: 0x00015290
		public static void SplitRandomly(float valueToSplit, int splitIntoCount, List<float> outValues, List<float> zeroIfFractionBelow = null, List<float> minFractions = null)
		{
			outValues.Clear();
			if (splitIntoCount > 0)
			{
				if (minFractions != null)
				{
					float num = 0f;
					for (int i = 0; i < minFractions.Count; i++)
					{
						if (minFractions[i] > 1f)
						{
							throw new ArgumentException("minFractions[i] > 1");
						}
						num += minFractions[i];
					}
					if (num > 1f)
					{
						throw new ArgumentException("minFractions sum > 1");
					}
				}
				float num2 = 0f;
				int num3 = 0;
				for (;;)
				{
					num3++;
					if (num3 > 10000)
					{
						break;
					}
					outValues.Clear();
					for (int j = 0; j < splitIntoCount; j++)
					{
						float num4 = Rand.Range(0f, 1f);
						num2 += num4;
						outValues.Add(num4);
					}
					bool flag = true;
					if (minFractions != null)
					{
						for (int k = 0; k < minFractions.Count; k++)
						{
							if (outValues[k] / num2 < minFractions[k])
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						goto IL_14F;
					}
				}
				Log.Error(string.Concat(new object[]
				{
					"Could not meet SplitRandomly requirements. valueToSplit=",
					valueToSplit,
					", splitIntoCount=",
					splitIntoCount,
					", zeroIfFractionsBelow=",
					zeroIfFractionBelow.ToStringSafeEnumerable(),
					", minFractions=",
					minFractions.ToStringSafeEnumerable()
				}));
				IL_14F:
				if (zeroIfFractionBelow != null)
				{
					for (int l = 0; l < zeroIfFractionBelow.Count; l++)
					{
						if (outValues[l] / num2 < zeroIfFractionBelow[l])
						{
							outValues[l] = 0f;
							num2 = 0f;
							for (int m = 0; m < outValues.Count; m++)
							{
								num2 += outValues[m];
							}
						}
					}
				}
				if (num2 != 0f)
				{
					for (int n = 0; n < outValues.Count; n++)
					{
						outValues[n] = outValues[n] / num2 * valueToSplit;
					}
				}
				return;
			}
			if (valueToSplit == 0f)
			{
				return;
			}
			throw new ArgumentException("splitIntoCount <= 0");
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001727C File Offset: 0x0001547C
		[DebugOutput("System", false)]
		internal static void RandTests()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Random generation tests");
			stringBuilder.Append("To see texture outputs, turn on 'draw recorded noise' and run this again.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Performance test with " + 10000000 + " values");
			Stopwatch stopwatch = new Stopwatch();
			float num = 0f;
			Rand.PushState();
			stopwatch.Start();
			for (int i = 0; i < 10000000; i++)
			{
				num += Rand.Value;
			}
			stopwatch.Stop();
			Rand.PopState();
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Time: ",
				stopwatch.ElapsedMilliseconds.ToString(),
				"ms (for sum ",
				num,
				")"
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Distribution test with " + 100000 + " values");
			DebugHistogram debugHistogram = new DebugHistogram(new float[]
			{
				0f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				1f
			});
			DebugHistogram debugHistogram2 = new DebugHistogram(new float[]
			{
				0f,
				0.01f,
				0.02f,
				0.03f,
				0.04f,
				0.05f,
				0.06f,
				0.07f,
				0.08f,
				0.09f,
				0.1f,
				1f
			});
			Rand.PushState();
			for (int j = 0; j < 100000; j++)
			{
				debugHistogram.Add(Rand.Value);
				debugHistogram2.Add(Rand.Value);
			}
			Rand.PopState();
			stringBuilder.AppendLine("Gross histogram:");
			debugHistogram.Display(stringBuilder);
			stringBuilder.AppendLine("Fine histogram:");
			debugHistogram2.Display(stringBuilder);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Long-term tests");
			for (int k = 0; k < 3; k++)
			{
				int num2 = 0;
				for (int l = 0; l < 5000000; l++)
				{
					if (Rand.MTBEventOccurs(250f, 60000f, 60f))
					{
						num2++;
					}
				}
				string value = string.Concat(new object[]
				{
					"MTB=",
					250,
					" days, MTBUnit=",
					60000,
					", check duration=",
					60,
					" Simulated ",
					5000,
					" days (",
					5000000,
					" tests). Got ",
					num2,
					" events."
				});
				stringBuilder.AppendLine(value);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Short-term tests");
			for (int m = 0; m < 5; m++)
			{
				int num3 = 0;
				for (int n = 0; n < 10000; n++)
				{
					if (Rand.MTBEventOccurs(1f, 24000f, 12000f))
					{
						num3++;
					}
				}
				string value2 = string.Concat(new object[]
				{
					"MTB=",
					1f,
					" days, MTBUnit=",
					24000f,
					", check duration=",
					12000f,
					", ",
					10000,
					" tests got ",
					num3,
					" events."
				});
				stringBuilder.AppendLine(value2);
			}
			for (int num4 = 0; num4 < 5; num4++)
			{
				int num5 = 0;
				for (int num6 = 0; num6 < 10000; num6++)
				{
					if (Rand.MTBEventOccurs(2f, 24000f, 6000f))
					{
						num5++;
					}
				}
				string value3 = string.Concat(new object[]
				{
					"MTB=",
					2f,
					" days, MTBUnit=",
					24000f,
					", check duration=",
					6000f,
					", ",
					10000,
					" tests got ",
					num5,
					" events."
				});
				stringBuilder.AppendLine(value3);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Near seed tests");
			DebugHistogram debugHistogram3 = new DebugHistogram(new float[]
			{
				0f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				1f
			});
			Rand.PushState();
			for (int num7 = 0; num7 < 1000; num7++)
			{
				Rand.Seed = num7;
				debugHistogram3.Add(Rand.Value);
			}
			Rand.PopState();
			debugHistogram3.Display(stringBuilder);
			int @int = Rand.Int;
			stringBuilder.AppendLine("Repeating single ValueSeeded with seed " + @int + ". This should give the same result:");
			for (int num8 = 0; num8 < 4; num8++)
			{
				stringBuilder.AppendLine("   " + Rand.ValueSeeded(@int));
			}
			Log.Message(stringBuilder.ToString());
			if (DebugViewSettings.drawRecordedNoise)
			{
				int[] array = new int[65536];
				Rand.PushState();
				for (int num9 = 0; num9 < 65536; num9++)
				{
					array[num9] = (int)(Rand.Value * 1000f);
				}
				Rand.PopState();
				Rand.DebugStoreBucketsAsTexture("One rand output per pixel", array, 1000f, 256);
				int[] array2 = new int[65536];
				Rand.PushState();
				for (int num10 = 0; num10 < 65536; num10++)
				{
					Rand.Seed = num10;
					array2[num10] = (int)(Rand.Value * 1000f);
				}
				Rand.PopState();
				Rand.DebugStoreBucketsAsTexture("One seed per pixel", array2, 1000f, 256);
				int[] array3 = new int[65536];
				Rand.PushState();
				for (int num11 = 0; num11 < 300000; num11++)
				{
					int num12 = (int)(Rand.Value * 65536f);
					array3[num12]++;
				}
				Rand.PopState();
				float whiteValue = 9.155273f;
				Rand.DebugStoreBucketsAsTexture("Brighten random pixel index", array3, whiteValue, 256);
				int[] array4 = new int[65536];
				Rand.PushState();
				for (int num13 = 0; num13 < 300000; num13++)
				{
					int num14 = (int)(Rand.Value * 256f);
					int num15 = (int)(Rand.Value * 256f);
					int num16 = num14 + 256 * num15;
					array4[num16]++;
				}
				Rand.PopState();
				float whiteValue2 = 9.155273f;
				Rand.DebugStoreBucketsAsTexture("Brighten random pixel coords", array4, whiteValue2, 256);
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000178FC File Offset: 0x00015AFC
		private static void DebugStoreBucketsAsTexture(string name, int[] buckets, float whiteValue, int texSize)
		{
			Texture2D texture2D = new Texture2D(texSize, texSize);
			for (int i = 0; i < texSize; i++)
			{
				for (int j = 0; j < texSize; j++)
				{
					int num = i + j * texSize;
					float num2 = (float)buckets[num] / whiteValue;
					num2 = Mathf.Clamp01(num2);
					texture2D.SetPixel(i, j, new Color(num2, num2, num2));
				}
			}
			texture2D.Apply();
			NoiseDebugUI.StoreTexture(texture2D, name);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00017960 File Offset: 0x00015B60
		public static int RandSeedForHour(this Thing t, int salt)
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(t.HashOffset(), Find.TickManager.TicksAbs / 2500), salt);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00017984 File Offset: 0x00015B84
		public static bool TryRangeInclusiveWhere(int from, int to, Predicate<int> predicate, out int value)
		{
			int num = to - from + 1;
			if (num <= 0)
			{
				value = 0;
				return false;
			}
			int num2 = Mathf.Max(Mathf.RoundToInt(Mathf.Sqrt((float)num)), 5);
			for (int i = 0; i < num2; i++)
			{
				int num3 = Rand.RangeInclusive(from, to);
				if (predicate(num3))
				{
					value = num3;
					return true;
				}
			}
			Rand.tmpRange.Clear();
			for (int j = from; j <= to; j++)
			{
				Rand.tmpRange.Add(j);
			}
			Rand.tmpRange.Shuffle<int>();
			int k = 0;
			int count = Rand.tmpRange.Count;
			while (k < count)
			{
				if (predicate(Rand.tmpRange[k]))
				{
					value = Rand.tmpRange[k];
					return true;
				}
				k++;
			}
			value = 0;
			return false;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00017A4C File Offset: 0x00015C4C
		public static Vector3 PointOnSphereCap(Vector3 center, float angle)
		{
			if (angle <= 0f)
			{
				return center;
			}
			if (angle >= 180f)
			{
				return Rand.UnitVector3;
			}
			float num = Rand.Range(Mathf.Cos(angle * 0.017453292f), 1f);
			float f = Rand.Range(0f, 6.2831855f);
			Vector3 point = new Vector3(Mathf.Sqrt(1f - num * num) * Mathf.Cos(f), Mathf.Sqrt(1f - num * num) * Mathf.Sin(f), num);
			return Quaternion.FromToRotation(Vector3.forward, center) * point;
		}

		// Token: 0x04000134 RID: 308
		private static uint seed;

		// Token: 0x04000135 RID: 309
		private static uint iterations = 0U;

		// Token: 0x04000136 RID: 310
		private static Stack<ulong> stateStack = new Stack<ulong>();

		// Token: 0x04000137 RID: 311
		private static List<int> tmpRange = new List<int>();
	}
}

using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000050 RID: 80
	public static class MathEvaluatorCustomFunctions
	{
		// Token: 0x060003EC RID: 1004 RVA: 0x00015998 File Offset: 0x00013B98
		private static object Min(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Min(Convert.ToDouble(args[0], invariantCulture), Convert.ToDouble(args[1], invariantCulture));
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000159C8 File Offset: 0x00013BC8
		private static object Max(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Max(Convert.ToDouble(args[0], invariantCulture), Convert.ToDouble(args[1], invariantCulture));
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000159F8 File Offset: 0x00013BF8
		private static object Round(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Round(Convert.ToDouble(args[0], invariantCulture));
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00015A20 File Offset: 0x00013C20
		private static object RoundToDigits(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Round(Convert.ToDouble(args[0], invariantCulture), Convert.ToInt32(args[1], invariantCulture));
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00015A50 File Offset: 0x00013C50
		private static object Floor(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Floor(Convert.ToDouble(args[0], invariantCulture));
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00015A78 File Offset: 0x00013C78
		private static object RoundRandom(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)GenMath.RoundRandom(Convert.ToSingle(args[0], invariantCulture));
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00015AA0 File Offset: 0x00013CA0
		private static object RandInt(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)Rand.RangeInclusive(Convert.ToInt32(args[0], invariantCulture), Convert.ToInt32(args[1], invariantCulture));
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00015AD0 File Offset: 0x00013CD0
		private static object RandFloat(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)Rand.Range(Convert.ToSingle(args[0], invariantCulture), Convert.ToSingle(args[1], invariantCulture));
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00015B00 File Offset: 0x00013D00
		private static object RangeAverage(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)FloatRange.FromString(Convert.ToString(args[0], invariantCulture)).Average;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00015B30 File Offset: 0x00013D30
		private static object RoundToTicksRough(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int num = Convert.ToInt32(args[0], invariantCulture);
			if (num <= 250)
			{
				return 250;
			}
			if (num < 5000)
			{
				return GenMath.RoundTo(num, 250);
			}
			if (num < 60000)
			{
				return GenMath.RoundTo(num, 2500);
			}
			if (num < 120000)
			{
				return GenMath.RoundTo(num, 6000);
			}
			return GenMath.RoundTo(num, 60000);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00015BBC File Offset: 0x00013DBC
		public static object Lerp(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Mathf.Lerp(Convert.ToSingle(args[0], invariantCulture), Convert.ToSingle(args[1], invariantCulture), Convert.ToSingle(args[2], invariantCulture));
		}

		// Token: 0x0400011B RID: 283
		public static readonly MathEvaluatorCustomFunctions.FunctionType[] FunctionTypes = new MathEvaluatorCustomFunctions.FunctionType[]
		{
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "min",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Min)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "max",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Max)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "round",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Round)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "roundToDigits",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RoundToDigits)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "floor",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Floor)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "roundRandom",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RoundRandom)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "randInt",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RandInt)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "randFloat",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RandFloat)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "rangeAverage",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RangeAverage)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "roundToTicksRough",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RoundToTicksRough)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "lerp",
				minArgs = 3,
				maxArgs = 3,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Lerp)
			}
		};

		// Token: 0x02001C86 RID: 7302
		public class FunctionType
		{
			// Token: 0x0400706F RID: 28783
			public string name;

			// Token: 0x04007070 RID: 28784
			public int minArgs;

			// Token: 0x04007071 RID: 28785
			public int maxArgs;

			// Token: 0x04007072 RID: 28786
			public Func<object[], object> func;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200002D RID: 45
	public static class Gen
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x0000A4C0 File Offset: 0x000086C0
		public static Vector3 AveragePosition(List<IntVec3> cells)
		{
			return new Vector3((float)cells.Average((IntVec3 c) => c.x) + 0.5f, 0f, (float)cells.Average((IntVec3 c) => c.z) + 0.5f);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000A530 File Offset: 0x00008730
		public static T RandomEnumValue<T>(bool disallowFirstValue)
		{
			int min = disallowFirstValue ? 1 : 0;
			T[] array = (T[])Enum.GetValues(typeof(T));
			int num = Rand.Range(min, array.Length);
			return array[num];
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000A569 File Offset: 0x00008769
		public static Vector3 RandomHorizontalVector(float max)
		{
			return new Vector3(Rand.Range(-max, max), 0f, Rand.Range(-max, max));
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A585 File Offset: 0x00008785
		public static Vector3 Random2DVector(Vector3 max)
		{
			return new Vector3(Rand.Range(-max.x, max.x), 0f, Rand.Range(-max.z, max.z));
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000A5B8 File Offset: 0x000087B8
		public static int GetBitCountOf(long lValue)
		{
			int num = 0;
			while (lValue != 0L)
			{
				lValue &= lValue - 1L;
				num++;
			}
			return num;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000A5D9 File Offset: 0x000087D9
		public static IEnumerable<T> GetAllSelectedItems<T>(this Enum value)
		{
			CultureInfo cult = CultureInfo.InvariantCulture;
			int valueAsInt = Convert.ToInt32(value, cult);
			foreach (object obj in Enum.GetValues(typeof(T)))
			{
				int num = Convert.ToInt32(obj, cult);
				if (num == (valueAsInt & num))
				{
					yield return (T)((object)obj);
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A5E9 File Offset: 0x000087E9
		public static IEnumerable<T> YieldSingle<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A5F9 File Offset: 0x000087F9
		public static IEnumerable YieldSingleNonGeneric<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000A60C File Offset: 0x0000880C
		public static string ToStringSafe<T>(this T obj)
		{
			if (obj == null)
			{
				return "null";
			}
			string result;
			try
			{
				result = obj.ToString();
			}
			catch (Exception arg)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = obj.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					Log.ErrorOnce("Exception in ToString(): " + arg, num ^ 1857461521);
				}
				else
				{
					Log.Error("Exception in ToString(): " + arg);
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000A6A4 File Offset: 0x000088A4
		public static string ToStringSafeEnumerable(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return "null";
			}
			string result;
			try
			{
				string text = "";
				foreach (object obj in enumerable)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += obj.ToStringSafe<object>();
				}
				result = text;
			}
			catch (Exception arg)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = enumerable.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					Log.ErrorOnce("Exception while enumerating: " + arg, num ^ 581736153);
				}
				else
				{
					Log.Error("Exception while enumerating: " + arg);
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A790 File Offset: 0x00008990
		public static void Swap<T>(ref T x, ref T y)
		{
			T t = y;
			y = x;
			x = t;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A7B7 File Offset: 0x000089B7
		public static T MemberwiseClone<T>(T obj)
		{
			if (Gen.s_memberwiseClone == null)
			{
				Gen.s_memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)((object)Gen.s_memberwiseClone.Invoke(obj, null));
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A7F8 File Offset: 0x000089F8
		public static int FixedTimeStepUpdate(ref float timeBuffer, float fps)
		{
			timeBuffer += Mathf.Min(Time.deltaTime, 1f);
			float num = 1f / fps;
			int num2 = Mathf.FloorToInt(timeBuffer / num);
			timeBuffer -= (float)num2 * num;
			return num2;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000A838 File Offset: 0x00008A38
		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj == null) ? 0 : obj.GetHashCode();
			return (int)((long)seed ^ (long)num + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A875 File Offset: 0x00008A75
		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A89A File Offset: 0x00008A9A
		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000A8B4 File Offset: 0x00008AB4
		public static int HashCombineInt(int v1, int v2, int v3, int v4)
		{
			int num = 352654597;
			int num2 = num;
			num = ((num << 5) + num + (num >> 27) ^ v1);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v2);
			num = ((num << 5) + num + (num >> 27) ^ v3);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v4);
			return num + num2 * 1566083941;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000A906 File Offset: 0x00008B06
		public static int HashOffset(this int baseInt)
		{
			return Gen.HashCombineInt(baseInt, 169495093);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A913 File Offset: 0x00008B13
		public static int HashOffset(this Thing t)
		{
			return t.thingIDNumber.HashOffset();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000A920 File Offset: 0x00008B20
		public static int HashOffset(this WorldObject o)
		{
			return o.ID.HashOffset();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000A92D File Offset: 0x00008B2D
		public static bool IsHashIntervalTick(this Map m, int interval)
		{
			return m.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A93A File Offset: 0x00008B3A
		public static int HashOffsetTicks(this Map m)
		{
			return Find.TickManager.TicksGame + m.uniqueID.HashOffset();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A952 File Offset: 0x00008B52
		public static bool IsHashIntervalTick(this Thing t, int interval)
		{
			return t.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A95F File Offset: 0x00008B5F
		public static int HashOffsetTicks(this Thing t)
		{
			return Find.TickManager.TicksGame + t.thingIDNumber.HashOffset();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A977 File Offset: 0x00008B77
		public static bool IsHashIntervalTick(this WorldObject o, int interval)
		{
			return o.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A984 File Offset: 0x00008B84
		public static int HashOffsetTicks(this WorldObject o)
		{
			return Find.TickManager.TicksGame + o.ID.HashOffset();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A99C File Offset: 0x00008B9C
		public static bool IsHashIntervalTick(this Faction f, int interval)
		{
			return f.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A9A9 File Offset: 0x00008BA9
		public static int HashOffsetTicks(this Faction f)
		{
			return Find.TickManager.TicksGame + f.randomKey.HashOffset();
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A9C1 File Offset: 0x00008BC1
		public static int HashOrderless(int v1, int v2)
		{
			return Gen.HashCombineInt(Math.Min(v1, v2), Math.Max(v1, v2));
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A9D8 File Offset: 0x00008BD8
		public static bool IsNestedHashIntervalTick(this Thing t, int outerInterval, int approxInnerInterval)
		{
			int num = Mathf.Max(Mathf.RoundToInt((float)approxInnerInterval / (float)outerInterval), 1);
			return t.HashOffsetTicks() / outerInterval % num == 0;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000AA04 File Offset: 0x00008C04
		public static void ReplaceNullFields<T>(ref T replaceIn, T replaceWith)
		{
			if (replaceIn == null || replaceWith == null)
			{
				return;
			}
			foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (fieldInfo.GetValue(replaceIn) == null)
				{
					object value = fieldInfo.GetValue(replaceWith);
					if (value != null)
					{
						object obj = replaceIn;
						fieldInfo.SetValue(obj, value);
						replaceIn = (T)((object)obj);
					}
				}
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000AA94 File Offset: 0x00008C94
		public static void EnsureAllFieldsNullable(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				Type fieldType = fieldInfo.FieldType;
				if (fieldType.IsValueType && !(Nullable.GetUnderlyingType(fieldType) != null))
				{
					Log.Warning(string.Concat(new string[]
					{
						"Field ",
						type.Name,
						".",
						fieldInfo.Name,
						" is not nullable."
					}));
				}
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000AB14 File Offset: 0x00008D14
		public static string GetNonNullFieldsDebugInfo(object obj)
		{
			if (obj == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object value = fieldInfo.GetValue(obj);
				if (value != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(fieldInfo.Name + "=" + value.ToStringSafe<object>());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000AB96 File Offset: 0x00008D96
		public static bool InBounds<T>(this T[,] array, int i, int j)
		{
			return i >= 0 && j >= 0 && i < array.GetLength(0) && j < array.GetLength(1);
		}

		// Token: 0x04000085 RID: 133
		private static MethodInfo s_memberwiseClone;
	}
}

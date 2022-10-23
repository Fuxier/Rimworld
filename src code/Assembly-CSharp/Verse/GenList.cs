using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000036 RID: 54
	public static class GenList
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x0000E2EF File Offset: 0x0000C4EF
		public static int CountAllowNull<T>(this IList<T> list)
		{
			if (list == null)
			{
				return 0;
			}
			return list.Count;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000E2FC File Offset: 0x0000C4FC
		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000E30C File Offset: 0x0000C50C
		public static bool HasData<T>(this IList<T> list)
		{
			return list != null && list.Count >= 1;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000E320 File Offset: 0x0000C520
		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000E358 File Offset: 0x0000C558
		public static List<T> ListFullCopyOrNull<T>(this List<T> source)
		{
			if (source == null)
			{
				return null;
			}
			return source.ListFullCopy<T>();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000E368 File Offset: 0x0000C568
		public static void RemoveDuplicates<T>(this List<T> list, Func<T, T, bool> comparer = null) where T : class
		{
			if (list.Count <= 1)
			{
				return;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < i; j++)
				{
					if ((comparer == null && list[i] == list[j]) || (comparer != null && comparer(list[i], list[j])))
					{
						list.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public static void TruncateToLength<T>(this List<T> list, int maxLength)
		{
			if (list.Count == 0 || list.Count <= maxLength)
			{
				return;
			}
			list.RemoveRange(maxLength, list.Count - maxLength);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000E400 File Offset: 0x0000C600
		public static void Shuffle<T>(this IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = Rand.RangeInclusive(0, i);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000E444 File Offset: 0x0000C644
		public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
		{
			int count = list.Count;
			for (int i = 1; i < count; i++)
			{
				T t = list[i];
				int num = i - 1;
				while (num >= 0 && comparison(list[num], t) > 0)
				{
					list[num + 1] = list[num];
					num--;
				}
				list[num + 1] = t;
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000E4A5 File Offset: 0x0000C6A5
		public static bool NotNullAndContains<T>(this IList<T> list, T element)
		{
			return !list.NullOrEmpty<T>() && list.Contains(element);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000E4B8 File Offset: 0x0000C6B8
		public static bool RemoveAll_IfNotAll<T, ContextType>(this List<T> list, ContextType context, Predicate<ContextType, T> predicate)
		{
			int num = list.Count - 1;
			while (num >= 0 && !predicate(context, list[num]))
			{
				num--;
			}
			if (num < 0)
			{
				return false;
			}
			T item = list[num];
			list.RemoveRange(num, list.Count - num);
			list.RemoveAll(context, predicate, true);
			list.Add(item);
			return true;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000E518 File Offset: 0x0000C718
		public static int RemoveAll<T, ContextType>(this List<T> list, ContextType context, Predicate<ContextType, T> predicate, bool negatePredicate = false)
		{
			bool flag = !negatePredicate;
			int num = 0;
			while (num < list.Count && predicate(context, list[num]) != flag)
			{
				num++;
			}
			if (num == list.Count)
			{
				return 0;
			}
			for (int i = num + 1; i < list.Count; i++)
			{
				if (predicate(context, list[i]) != flag)
				{
					list[num++] = list[i];
				}
			}
			int num2 = list.Count - num;
			list.RemoveRange(num, num2);
			return num2;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000031 RID: 49
	public static class GenCollection
	{
		// Token: 0x0600023C RID: 572 RVA: 0x0000BECC File Offset: 0x0000A0CC
		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				hashSet.Add(item);
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000BF18 File Offset: 0x0000A118
		public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dest, IDictionary<TKey, TValue> source)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
			{
				dest.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000BF70 File Offset: 0x0000A170
		public static void SetOrAdd<K, V>(this Dictionary<K, V> dict, K key, V value)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = value;
				return;
			}
			dict.Add(key, value);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000BF8C File Offset: 0x0000A18C
		public static void AddDistinct<K, V>(this Dictionary<K, V> dict, K key, V value)
		{
			if (!dict.ContainsKey(key))
			{
				dict.Add(key, value);
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		public static void Increment<K>(this Dictionary<K, int> dict, K key)
		{
			if (dict.ContainsKey(key))
			{
				int num = dict[key];
				dict[key] = num + 1;
				return;
			}
			dict[key] = 1;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
		public static bool SharesElementWith<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			IList<T> list;
			IList<T> list2;
			if ((list = (source as IList<T>)) != null && (list2 = (other as IList<T>)) != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						if (EqualityComparer<T>.Default.Equals(list[i], list2[j]))
						{
							return true;
						}
					}
				}
				return false;
			}
			return source.Any((T item) => other.Contains(item));
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000C05E File Offset: 0x0000A25E
		public static IEnumerable<T> InRandomOrder<T>(this IEnumerable<T> source, IList<T> workingList = null)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (workingList == null)
			{
				workingList = source.ToList<T>();
			}
			else
			{
				workingList.Clear();
				foreach (T item in source)
				{
					workingList.Add(item);
				}
			}
			int countUnChosen = workingList.Count;
			int rand = 0;
			while (countUnChosen > 0)
			{
				rand = Rand.Range(0, countUnChosen);
				yield return workingList[rand];
				T value = workingList[rand];
				workingList[rand] = workingList[countUnChosen - 1];
				workingList[countUnChosen - 1] = value;
				int num = countUnChosen;
				countUnChosen = num - 1;
			}
			yield break;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000C078 File Offset: 0x0000A278
		public static T RandomElement<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list == null)
			{
				list = source.ToList<T>();
			}
			if (list.Count == 0)
			{
				Log.Warning("Getting random element from empty collection.");
				return default(T);
			}
			return list[Rand.Range(0, list.Count)];
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		public static T RandomElementWithFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElement(out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000C0F0 File Offset: 0x0000A2F0
		public static bool TryRandomElement<T>(this IEnumerable<T> source, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				if (list.Count == 0)
				{
					result = default(T);
					return false;
				}
			}
			else
			{
				list = source.ToList<T>();
				if (!list.Any<T>())
				{
					result = default(T);
					return false;
				}
			}
			result = list.RandomElement<T>();
			return true;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000C14C File Offset: 0x0000A34C
		public static T RandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
		{
			float num = 0f;
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num2,
							" from ",
							list[i]
						}));
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					return list[0];
				}
			}
			else
			{
				int num3 = 0;
				foreach (T t in source)
				{
					num3++;
					float num4 = weightSelector(t);
					if (num4 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num4,
							" from ",
							t
						}));
						num4 = 0f;
					}
					num += num4;
				}
				if (num3 == 1 && num > 0f)
				{
					return source.First<T>();
				}
			}
			if (num <= 0f)
			{
				Log.Error("RandomElementByWeight with totalWeight=" + num + " - use TryRandomElementByWeight.");
				return default(T);
			}
			float num5 = Rand.Value * num;
			float num6 = 0f;
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					float num7 = weightSelector(list[j]);
					if (num7 > 0f)
					{
						num6 += num7;
						if (num6 >= num5)
						{
							return list[j];
						}
					}
				}
			}
			else
			{
				foreach (T t2 in source)
				{
					float num8 = weightSelector(t2);
					if (num8 > 0f)
					{
						num6 += num8;
						if (num6 >= num5)
						{
							return t2;
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000C390 File Offset: 0x0000A590
		public static T RandomElementByWeightWithFallback<T>(this IEnumerable<T> source, Func<T, float> weightSelector, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElementByWeight(weightSelector, out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000C3AC File Offset: 0x0000A5AC
		public static bool TryRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
		{
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num2,
							" from ",
							list[i]
						}));
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					result = list[0];
					return true;
				}
				if (num == 0f)
				{
					result = default(T);
					return false;
				}
				num *= Rand.Value;
				for (int j = 0; j < list.Count; j++)
				{
					float num3 = weightSelector(list[j]);
					if (num3 > 0f)
					{
						num -= num3;
						if (num <= 0f)
						{
							result = list[j];
							return true;
						}
					}
				}
			}
			IEnumerator<T> enumerator = source.GetEnumerator();
			result = default(T);
			float num4 = 0f;
			while (num4 == 0f && enumerator.MoveNext())
			{
				result = enumerator.Current;
				num4 = weightSelector(result);
				if (num4 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num4,
						" from ",
						result
					}));
					num4 = 0f;
				}
			}
			if (num4 == 0f)
			{
				result = default(T);
				return false;
			}
			while (enumerator.MoveNext())
			{
				T t = enumerator.Current;
				float num5 = weightSelector(t);
				if (num5 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num5,
						" from ",
						t
					}));
					num5 = 0f;
				}
				if (Rand.Range(0f, num4 + num5) >= num4)
				{
					result = t;
				}
				num4 += num5;
			}
			return true;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		public static T RandomElementByWeightWithDefault<T>(this IEnumerable<T> source, Func<T, float> weightSelector, float defaultValueWeight)
		{
			if (defaultValueWeight < 0f)
			{
				Log.Error("Negative default value weight.");
				defaultValueWeight = 0f;
			}
			float num = 0f;
			foreach (T t in source)
			{
				float num2 = weightSelector(t);
				if (num2 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num2,
						" from ",
						t
					}));
					num2 = 0f;
				}
				num += num2;
			}
			float num3 = defaultValueWeight + num;
			if (num3 <= 0f)
			{
				Log.Error("RandomElementByWeightWithDefault with totalWeight=" + num3);
				return default(T);
			}
			if (Rand.Value < defaultValueWeight / num3 || num == 0f)
			{
				return default(T);
			}
			return source.RandomElementByWeight(weightSelector);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000C6E8 File Offset: 0x0000A8E8
		public static T FirstOrFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return fallback;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000C730 File Offset: 0x0000A930
		public static T FirstOrFallback<T>(this IEnumerable<T> source, Func<T, bool> predicate, T fallback = default(T))
		{
			return source.Where(predicate).FirstOrFallback(fallback);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000C73F File Offset: 0x0000A93F
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000C750 File Offset: 0x0000A950
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) > 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000C7FC File Offset: 0x0000A9FC
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, TSource fallback = default(TSource))
		{
			return source.MaxByWithFallback(selector, Comparer<TKey>.Default, fallback);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C80C File Offset: 0x0000AA0C
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, TSource fallback = default(TSource))
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = fallback;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					result = tsource;
				}
			}
			return result;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000C8B4 File Offset: 0x0000AAB4
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMaxBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000C978 File Offset: 0x0000AB78
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000C988 File Offset: 0x0000AB88
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) < 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000CA34 File Offset: 0x0000AC34
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMinBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000CA44 File Offset: 0x0000AC44
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) < 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		public static void SortBy<T, TSortBy>(this List<T> list, Func<T, TSortBy> selector) where TSortBy : IComparable<TSortBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				return tsortBy.CompareTo(selector(b));
			});
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000CB30 File Offset: 0x0000AD30
		public static void SortBy<T, TSortBy, TThenBy>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				TSortBy other = selector(b);
				if (!tsortBy.Equals(other))
				{
					return tsortBy.CompareTo(other);
				}
				TThenBy tthenBy = thenBySelector(a);
				return tthenBy.CompareTo(thenBySelector(b));
			});
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000CB70 File Offset: 0x0000AD70
		public static void SortBy<T, TSortBy, TThenBy, TThenBy2>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector, Func<T, TThenBy2> thenBy2Selector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>, IEquatable<TThenBy> where TThenBy2 : IComparable<TThenBy2>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				TSortBy other = selector(b);
				if (!tsortBy.Equals(other))
				{
					return tsortBy.CompareTo(other);
				}
				TThenBy tthenBy = thenBySelector(a);
				TThenBy other2 = thenBySelector(b);
				if (!tthenBy.Equals(other2))
				{
					return tthenBy.CompareTo(other2);
				}
				TThenBy2 tthenBy2 = thenBy2Selector(a);
				return tthenBy2.CompareTo(thenBy2Selector(b));
			});
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000CBB4 File Offset: 0x0000ADB4
		public static void SortByColor<T>(this List<T> colorDefs, Func<T, Color> getColor)
		{
			colorDefs.SortBy(delegate(T x)
			{
				float num;
				float a;
				float num2;
				Color.RGBToHSV(getColor(x), out num, out a, out num2);
				if (!Mathf.Approximately(a, 0f))
				{
					return (float)Mathf.RoundToInt(num * 100f);
				}
				return -1f;
			}, delegate(T x)
			{
				float num;
				float num2;
				float num3;
				Color.RGBToHSV(getColor(x), out num, out num2, out num3);
				return Mathf.RoundToInt(num3 * 100f);
			});
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000CBEC File Offset: 0x0000ADEC
		public static void SortByDescending<T, TSortByDescending>(this List<T> list, Func<T, TSortByDescending> selector) where TSortByDescending : IComparable<TSortByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending tsortByDescending = selector(b);
				return tsortByDescending.CompareTo(selector(a));
			});
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000CC24 File Offset: 0x0000AE24
		public static void SortByDescending<T, TSortByDescending, TThenByDescending>(this List<T> list, Func<T, TSortByDescending> selector, Func<T, TThenByDescending> thenByDescendingSelector) where TSortByDescending : IComparable<TSortByDescending>, IEquatable<TSortByDescending> where TThenByDescending : IComparable<TThenByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending other = selector(a);
				TSortByDescending other2 = selector(b);
				if (!other.Equals(other2))
				{
					return other2.CompareTo(other);
				}
				TThenByDescending tthenByDescending = thenByDescendingSelector(b);
				return tthenByDescending.CompareTo(thenByDescendingSelector(a));
			});
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000CC64 File Offset: 0x0000AE64
		public static void SortStable<T>(this IList<T> list, Func<T, T, int> comparator)
		{
			if (list.Count <= 1)
			{
				return;
			}
			List<Pair<T, int>> list2;
			bool flag;
			if (GenCollection.SortStableTempList<T>.working)
			{
				list2 = new List<Pair<T, int>>();
				flag = false;
			}
			else
			{
				list2 = GenCollection.SortStableTempList<T>.list;
				GenCollection.SortStableTempList<T>.working = true;
				flag = true;
			}
			try
			{
				list2.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(new Pair<T, int>(list[i], i));
				}
				list2.Sort(delegate(Pair<T, int> lhs, Pair<T, int> rhs)
				{
					int num = comparator(lhs.First, rhs.First);
					if (num != 0)
					{
						return num;
					}
					return lhs.Second.CompareTo(rhs.Second);
				});
				list.Clear();
				for (int j = 0; j < list2.Count; j++)
				{
					list.Add(list2[j].First);
				}
				list2.Clear();
			}
			finally
			{
				if (flag)
				{
					GenCollection.SortStableTempList<T>.working = false;
				}
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000CD38 File Offset: 0x0000AF38
		public static IComparer<T> ThenBy<T>(this IComparer<T> first, IComparer<T> second)
		{
			return new GenCollection.ComparerChain<T>(first, second);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000CD41 File Offset: 0x0000AF41
		public static IComparer<T> Descending<T>(this IComparer<T> cmp)
		{
			return new GenCollection.DescendingComparer<T>(cmp);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000CD49 File Offset: 0x0000AF49
		public static IComparer<T> CompareBy<T, TComparable>(Func<T, TComparable> selector) where TComparable : IComparable<TComparable>
		{
			return Comparer<T>.Create(delegate(T a, T b)
			{
				TComparable tcomparable = selector(a);
				return tcomparable.CompareTo(selector(b));
			});
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000CD68 File Offset: 0x0000AF68
		public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			List<TKey> list = null;
			int result;
			try
			{
				foreach (KeyValuePair<TKey, TValue> obj in dictionary)
				{
					if (predicate(obj))
					{
						if (list == null)
						{
							list = SimplePool<List<TKey>>.Get();
						}
						list.Add(obj.Key);
					}
				}
				if (list != null)
				{
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						dictionary.Remove(list[i]);
						i++;
					}
					result = list.Count;
				}
				else
				{
					result = 0;
				}
			}
			finally
			{
				if (list != null)
				{
					list.Clear();
					SimplePool<List<TKey>>.Return(list);
				}
			}
			return result;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000CE24 File Offset: 0x0000B024
		public static void RemoveAll<T>(this List<T> list, Func<T, int, bool> predicate)
		{
			int num = 0;
			int count = list.Count;
			while (num < count && !predicate(list[num], num))
			{
				num++;
			}
			if (num >= count)
			{
				return;
			}
			int i = num + 1;
			while (i < count)
			{
				while (i < count && predicate(list[i], i))
				{
					i++;
				}
				if (i < count)
				{
					list[num++] = list[i++];
				}
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000CE95 File Offset: 0x0000B095
		public static void RemoveLast<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000CEA5 File Offset: 0x0000B0A5
		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000CEC3 File Offset: 0x0000B0C3
		public static bool Any<T>(this List<T> list, Predicate<T> predicate)
		{
			return list.FindIndex(predicate) != -1;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000CED2 File Offset: 0x0000B0D2
		public static bool Any<T>(this List<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000CEDD File Offset: 0x0000B0DD
		public static bool Any<T>(this HashSet<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000CEE8 File Offset: 0x0000B0E8
		public static bool Any<T>(this Stack<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000CEF4 File Offset: 0x0000B0F4
		public static void AddRange<T>(this HashSet<T> set, List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				set.Add(list[i]);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000CF20 File Offset: 0x0000B120
		public static void AddRange<T>(this HashSet<T> set, HashSet<T> other)
		{
			foreach (T item in other)
			{
				set.Add(item);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000CF70 File Offset: 0x0000B170
		public static int Count_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return 0;
			}
			ICollection collection = e as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in e)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000CFD8 File Offset: 0x0000B1D8
		public static T FirstOrDefault<T>(this List<T> list, Predicate<T> predicate)
		{
			foreach (T t in list)
			{
				if (predicate(t))
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000D038 File Offset: 0x0000B238
		public static object FirstOrDefault_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return null;
			}
			IList list = e as IList;
			if (list == null)
			{
				using (IEnumerator enumerator = e.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
				return null;
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
		public static float AverageWeighted<T>(this IEnumerable<T> list, Func<T, float> weight, Func<T, float> value)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (T arg in list)
			{
				float num3 = weight(arg);
				num += num3;
				num2 += value(arg) * num3;
			}
			return num2 / num;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000D114 File Offset: 0x0000B314
		public static void ExecuteEnumerable(this IEnumerable enumerable)
		{
			foreach (object obj in enumerable)
			{
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000D15C File Offset: 0x0000B35C
		public static bool EnumerableNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return true;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count == 0;
			}
			return !enumerable.Any<T>();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000D18C File Offset: 0x0000B38C
		public static int EnumerableCount(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in enumerable)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000D1F4 File Offset: 0x0000B3F4
		public static int Count<T>(this List<T> list, Predicate<T> predicate)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (predicate(list[i]))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000D228 File Offset: 0x0000B428
		public static int FirstIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T arg in enumerable)
			{
				if (predicate(arg))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000D280 File Offset: 0x0000B480
		public static V TryGetValue<T, V>(this IDictionary<T, V> dict, T key, V fallback = default(V))
		{
			V result;
			if (!dict.TryGetValue(key, out result))
			{
				result = fallback;
			}
			return result;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000D29B File Offset: 0x0000B49B
		public static IEnumerable<Pair<T, V>> Cross<T, V>(this IEnumerable<T> lhs, IEnumerable<V> rhs)
		{
			T[] lhsv = lhs.ToArray<T>();
			V[] rhsv = rhs.ToArray<V>();
			int num;
			for (int i = 0; i < lhsv.Length; i = num)
			{
				for (int j = 0; j < rhsv.Length; j = num)
				{
					yield return new Pair<T, V>(lhsv[i], rhsv[j]);
					num = j + 1;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000D2B2 File Offset: 0x0000B4B2
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> lhs, T rhs)
		{
			foreach (T t in lhs)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield return rhs;
			yield break;
			yield break;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000D2C9 File Offset: 0x0000B4C9
		public static IEnumerable<TSource> ConcatIfNotNull<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (second == null)
			{
				return first;
			}
			return first.Concat(second);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000D2E8 File Offset: 0x0000B4E8
		public static LocalTargetInfo FirstValid(this List<LocalTargetInfo> source)
		{
			if (source == null)
			{
				return LocalTargetInfo.Invalid;
			}
			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].IsValid)
				{
					return source[i];
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000D32D File Offset: 0x0000B52D
		public static IEnumerable<T> Except<T>(this IEnumerable<T> lhs, T rhs) where T : class
		{
			foreach (T t in lhs)
			{
				if (t != rhs)
				{
					yield return t;
				}
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000D344 File Offset: 0x0000B544
		public static IEnumerable<ValueTuple<T, T>> Pairwise<T>(this IEnumerable<T> enumerable) where T : class
		{
			foreach (T item in enumerable)
			{
				IEnumerator<T> iterator;
				if (!iterator.MoveNext())
				{
					yield return ValueTuple.Create<T, T>(item, default(T));
				}
				else
				{
					yield return ValueTuple.Create<T, T>(item, iterator.Current);
				}
			}
			yield break;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000D354 File Offset: 0x0000B554
		public static bool ListsEqual<T>(List<T> a, List<T> b) where T : class
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < a.Count; i++)
			{
				if (!@default.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000D3CC File Offset: 0x0000B5CC
		public static bool DictsEqual<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> a, IReadOnlyDictionary<TKey, TValue> b)
		{
			int num = (a != null) ? a.Count : 0;
			int num2 = (b != null) ? b.Count : 0;
			if (num != num2)
			{
				return false;
			}
			if (num == 0)
			{
				return true;
			}
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in a)
			{
				TValue y;
				if (!b.TryGetValue(keyValuePair.Key, out y) || !@default.Equals(keyValuePair.Value, y))
				{
					return false;
				}
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair2 in b)
			{
				TValue y2;
				if (!a.TryGetValue(keyValuePair2.Key, out y2) || !@default.Equals(keyValuePair2.Value, y2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000D4BC File Offset: 0x0000B6BC
		public static int DictHashCode<Key, Value>(IReadOnlyDictionary<Key, Value> dict)
		{
			int num = 0;
			foreach (KeyValuePair<Key, Value> keyValuePair in (dict ?? Enumerable.Empty<KeyValuePair<Key, Value>>()))
			{
				num ^= keyValuePair.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000D51C File Offset: 0x0000B71C
		public static void Deconstruct<Key, Value>(this KeyValuePair<Key, Value> tuple, out Key key, out Value value)
		{
			key = tuple.Key;
			value = tuple.Value;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000D538 File Offset: 0x0000B738
		public static bool SetsEqual<T>(this List<T> a, List<T> b)
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!b.Contains(a[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < b.Count; j++)
			{
				if (!a.Contains(b[j]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000D5B4 File Offset: 0x0000B7B4
		public static IEnumerable<T> TakeRandom<T>(this List<T> list, int count)
		{
			if (list.NullOrEmpty<T>())
			{
				yield break;
			}
			int num;
			for (int i = 0; i < count; i = num)
			{
				yield return list[Rand.Range(0, list.Count)];
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000D5CC File Offset: 0x0000B7CC
		public static void AddDistinct<T>(this List<T> list, T element) where T : class
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == element)
				{
					return;
				}
			}
			list.Add(element);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000D608 File Offset: 0x0000B808
		public static int Replace<T>(this IList<T> list, T replace, T with) where T : class
		{
			if (list == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == replace)
				{
					list[i] = with;
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000D64E File Offset: 0x0000B84E
		public static Pair<K, List<E>> ConvertIGroupingToPair<K, E>(IGrouping<K, E> g)
		{
			return new Pair<K, List<E>>(g.Key, g.ToList<E>());
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000D664 File Offset: 0x0000B864
		public static int GetCountGreaterOrEqualInSortedList(List<int> list, int val)
		{
			int num = list.BinarySearch(val);
			if (num >= 0)
			{
				return list.Count - num;
			}
			int num2 = ~num;
			return list.Count - num2;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000D694 File Offset: 0x0000B894
		public static void InsertIntoSortedList<T>(this List<T> list, T val, IComparer<T> cmp)
		{
			if (list.Count == 0)
			{
				list.Add(val);
				return;
			}
			int num = list.BinarySearch(val, cmp);
			if (num >= 0)
			{
				list.Insert(num, val);
				return;
			}
			list.Insert(~num, val);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000D6D0 File Offset: 0x0000B8D0
		public static void RemoveBatchUnordered<T>(this List<T> list, List<int> indices)
		{
			if (indices.Count == 0)
			{
				return;
			}
			int num = list.Count - 1;
			foreach (int num2 in indices)
			{
				if (num == num2)
				{
					num--;
				}
				else
				{
					if (num <= 0)
					{
						break;
					}
					list[num2] = list[num];
					num--;
				}
			}
			for (int i = list.Count - 1; i > num; i--)
			{
				list.RemoveAt(i);
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000D768 File Offset: 0x0000B968
		public static ValueType GetWithFallback<KeyType, ValueType>(this Dictionary<KeyType, ValueType> dictionary, KeyType key, ValueType fallback = default(ValueType))
		{
			ValueType result;
			if (dictionary.TryGetValue(key, out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x02001C59 RID: 7257
		private static class SortStableTempList<T>
		{
			// Token: 0x04006FB3 RID: 28595
			public static List<Pair<T, int>> list = new List<Pair<T, int>>();

			// Token: 0x04006FB4 RID: 28596
			public static bool working;
		}

		// Token: 0x02001C5A RID: 7258
		private class ComparerChain<T> : IComparer<T>
		{
			// Token: 0x0600AEEC RID: 44780 RVA: 0x003FC143 File Offset: 0x003FA343
			public ComparerChain(IComparer<T> first, IComparer<T> second)
			{
				this.first = first;
				this.second = second;
			}

			// Token: 0x0600AEED RID: 44781 RVA: 0x003FC15C File Offset: 0x003FA35C
			public int Compare(T x, T y)
			{
				int num = this.first.Compare(x, y);
				if (num != 0)
				{
					return num;
				}
				return this.second.Compare(x, y);
			}

			// Token: 0x04006FB5 RID: 28597
			private readonly IComparer<T> first;

			// Token: 0x04006FB6 RID: 28598
			private readonly IComparer<T> second;
		}

		// Token: 0x02001C5B RID: 7259
		private class DescendingComparer<T> : IComparer<T>
		{
			// Token: 0x0600AEEE RID: 44782 RVA: 0x003FC189 File Offset: 0x003FA389
			public DescendingComparer(IComparer<T> cmp)
			{
				this.cmp = cmp;
			}

			// Token: 0x0600AEEF RID: 44783 RVA: 0x003FC198 File Offset: 0x003FA398
			public int Compare(T x, T y)
			{
				return -this.cmp.Compare(x, y);
			}

			// Token: 0x04006FB7 RID: 28599
			private readonly IComparer<T> cmp;
		}
	}
}

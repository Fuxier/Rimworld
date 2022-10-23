using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000037 RID: 55
	public static class GenDictionary
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public static string ToStringFullContents<K, V>(this Dictionary<K, V> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<K, V> keyValuePair in dict)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				K key = keyValuePair.Key;
				string str = key.ToString();
				string str2 = ": ";
				V value = keyValuePair.Value;
				stringBuilder2.AppendLine(str + str2 + value.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000E630 File Offset: 0x0000C830
		public static bool NullOrEmpty<K, V>(this Dictionary<K, V> dict)
		{
			return dict == null || dict.Count == 0;
		}
	}
}

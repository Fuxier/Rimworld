using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200003A RID: 58
	public static class GenString
	{
		// Token: 0x060002F6 RID: 758 RVA: 0x0000FCAC File Offset: 0x0000DEAC
		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000FCEE File Offset: 0x0000DEEE
		public static string ToStringCached(this int num)
		{
			if (num < -4999)
			{
				return num.ToString();
			}
			if (num > 4999)
			{
				return num.ToString();
			}
			return GenString.numberStrings[num + 5000];
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000FD1D File Offset: 0x0000DF1D
		public static IEnumerable<string> SplitBy(this string str, int chunkLength)
		{
			if (str.NullOrEmpty())
			{
				yield break;
			}
			if (chunkLength < 1)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < str.Length; i += chunkLength)
			{
				if (chunkLength > str.Length - i)
				{
					chunkLength = str.Length - i;
				}
				yield return str.Substring(i, chunkLength);
			}
			yield break;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000FD34 File Offset: 0x0000DF34
		public static StringBuilder AppendLineIfNotEmpty(this StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			return sb;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000FD47 File Offset: 0x0000DF47
		public static int GetHashCodeSafe(this string val)
		{
			if (val == null)
			{
				return 0;
			}
			return val.GetHashCode();
		}

		// Token: 0x040000AD RID: 173
		private static string[] numberStrings = new string[10000];
	}
}

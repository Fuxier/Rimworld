using System;

namespace Verse
{
	// Token: 0x02000195 RID: 405
	public class LanguageWorker_Romanian : LanguageWorker
	{
		// Token: 0x06000B20 RID: 2848 RVA: 0x0003D05D File Offset: 0x0003B25D
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (!plural)
			{
				return ((gender == Gender.Female) ? "a " : "un ") + str;
			}
			if (gender != Gender.Male)
			{
				return str + "e";
			}
			return str + "i";
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0003D09C File Offset: 0x0003B29C
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (name)
			{
				return str;
			}
			char ch = str[str.Length - 1];
			if (plural)
			{
				if (gender != Gender.Male)
				{
					return str + "e";
				}
				return str + "i";
			}
			else
			{
				if (!this.IsVowel(ch))
				{
					return str + "ul";
				}
				if (gender == Gender.Male)
				{
					return str + "le";
				}
				return str + "a";
			}
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0003D117 File Offset: 0x0003B317
		public bool IsVowel(char ch)
		{
			return "aeiouâîAEIOUÂÎ".IndexOf(ch) >= 0;
		}
	}
}

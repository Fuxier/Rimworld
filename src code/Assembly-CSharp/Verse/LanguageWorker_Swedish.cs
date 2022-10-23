using System;

namespace Verse
{
	// Token: 0x02000198 RID: 408
	public class LanguageWorker_Swedish : LanguageWorker
	{
		// Token: 0x06000B30 RID: 2864 RVA: 0x0003D627 File Offset: 0x0003B827
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "en " + str;
			}
			return "ett " + str;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0003D650 File Offset: 0x0003B850
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
			if (gender == Gender.Male || gender == Gender.Female)
			{
				if (this.IsVowel(ch))
				{
					return str + "n";
				}
				return str + "en";
			}
			else
			{
				if (this.IsVowel(ch))
				{
					return str + "t";
				}
				return str + "et";
			}
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0003D6C5 File Offset: 0x0003B8C5
		public bool IsVowel(char ch)
		{
			return "aeiouyåäöAEIOUYÅÄÖ".IndexOf(ch) >= 0;
		}
	}
}

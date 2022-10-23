using System;

namespace Verse
{
	// Token: 0x02000193 RID: 403
	public class LanguageWorker_Norwegian : LanguageWorker
	{
		// Token: 0x06000B1A RID: 2842 RVA: 0x0003CF57 File Offset: 0x0003B157
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
			return "et " + str;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0003CF80 File Offset: 0x0003B180
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
			char c = str[str.Length - 1];
			if (gender == Gender.Male || gender == Gender.Female)
			{
				if (c == 'e')
				{
					return str + "n";
				}
				return str + "en";
			}
			else
			{
				if (c == 'e')
				{
					return str + "t";
				}
				return str + "et";
			}
		}
	}
}

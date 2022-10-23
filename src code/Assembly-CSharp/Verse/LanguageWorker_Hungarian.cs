using System;

namespace Verse
{
	// Token: 0x02000190 RID: 400
	public class LanguageWorker_Hungarian : LanguageWorker
	{
		// Token: 0x06000B05 RID: 2821 RVA: 0x0003C89C File Offset: 0x0003AA9C
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return "egy " + str;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0003C8B0 File Offset: 0x0003AAB0
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
			char ch = str[0];
			if (this.IsVowel(ch))
			{
				return "az " + str;
			}
			return "a " + str;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0003C8F5 File Offset: 0x0003AAF5
		public bool IsVowel(char ch)
		{
			return "eéöőüűiíaáoóuúEÉÖŐÜŰIÍAÁOÓUÚ".IndexOf(ch) >= 0;
		}
	}
}

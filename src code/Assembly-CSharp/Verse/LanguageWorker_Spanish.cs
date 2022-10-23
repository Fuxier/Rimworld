using System;

namespace Verse
{
	// Token: 0x02000197 RID: 407
	public class LanguageWorker_Spanish : LanguageWorker
	{
		// Token: 0x06000B2A RID: 2858 RVA: 0x0003D495 File Offset: 0x0003B695
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "unas " : "unos ") + str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0003D4CD File Offset: 0x0003B6CD
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "las " : "los ") + str;
			}
			return ((gender == Gender.Female) ? "la " : "el ") + str;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0003D505 File Offset: 0x0003B705
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + ".º";
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0003D518 File Offset: 0x0003B718
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			string result;
			if (this.TryLookupPluralForm(str, gender, out result, count))
			{
				return result;
			}
			if (count != -1 && count < 2)
			{
				return str;
			}
			char c = str[str.Length - 1];
			char c2 = (str.Length >= 2) ? str[str.Length - 2] : '\0';
			if (this.IsVowel(c))
			{
				if (str == "sí")
				{
					return "síes";
				}
				if (c == 'í' || c == 'ú' || c == 'Í' || c == 'Ú')
				{
					return str + "es";
				}
				return str + "s";
			}
			else
			{
				if ((c == 'y' || c == 'Y') && this.IsVowel(c2))
				{
					return str + "es";
				}
				if ("lrndzjsxLRNDZJSX".IndexOf(c) >= 0 || (c == 'h' && c2 == 'c'))
				{
					return str + "es";
				}
				return str + "s";
			}
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0003D614 File Offset: 0x0003B814
		public bool IsVowel(char ch)
		{
			return "aeiouáéíóúAEIOUÁÉÍÓÚ".IndexOf(ch) >= 0;
		}
	}
}

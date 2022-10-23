using System;

namespace Verse
{
	// Token: 0x02000191 RID: 401
	public class LanguageWorker_Italian : LanguageWorker
	{
		// Token: 0x06000B09 RID: 2825 RVA: 0x0003C908 File Offset: 0x0003AB08
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			char c = str[0];
			char c2 = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "un'" + str;
				}
				return "una " + str;
			}
			else
			{
				char c3 = char.ToLower(c);
				char c4 = char.ToLower(c2);
				if ((c == 's' || c == 'S') && !this.IsVowel(c2))
				{
					return "uno " + str;
				}
				if ((c3 == 'p' && c4 == 's') || (c3 == 'p' && c4 == 'n') || c3 == 'z' || c3 == 'x' || c3 == 'y' || (c3 == 'g' && c4 == 'n'))
				{
					return "uno " + str;
				}
				return "un " + str;
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0003C9D0 File Offset: 0x0003ABD0
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
			char c = str[0];
			char ch = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "la " + str;
			}
			else
			{
				if (c == 'z' || c == 'Z')
				{
					return "lo " + str;
				}
				if ((c == 's' || c == 'S') && !this.IsVowel(ch))
				{
					return "lo " + str;
				}
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "il " + str;
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0003CA83 File Offset: 0x0003AC83
		public bool IsVowel(char ch)
		{
			return "aeiouAEIOU".IndexOf(ch) >= 0;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0003CA96 File Offset: 0x0003AC96
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + "°";
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0003CAA8 File Offset: 0x0003ACA8
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
			char ch = str[str.Length - 1];
			if (!this.IsVowel(ch))
			{
				return str;
			}
			if (gender == Gender.Female)
			{
				return str.Substring(0, str.Length - 1) + "e";
			}
			return str.Substring(0, str.Length - 1) + "i";
		}
	}
}

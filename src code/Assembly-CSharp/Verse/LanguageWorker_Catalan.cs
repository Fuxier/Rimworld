using System;

namespace Verse
{
	// Token: 0x02000189 RID: 393
	public class LanguageWorker_Catalan : LanguageWorker
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x0003B66F File Offset: 0x0003986F
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "unes " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0003B6AF File Offset: 0x000398AF
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "les " : "els ") + str;
			}
			return this.WithElLaArticle(str, gender, false);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0003B6E4 File Offset: 0x000398E4
		private string WithElLaArticle(string str, Gender gender, bool name)
		{
			if (str.Length == 0 || (!this.IsVowel(str[0]) && str[0] != 'h' && str[0] != 'H'))
			{
				return ((gender == Gender.Female) ? "la " : "el ") + str;
			}
			if (name)
			{
				return ((gender == Gender.Female) ? "l'" : "n'") + str;
			}
			return "l'" + str;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0003B75C File Offset: 0x0003995C
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			if (gender == Gender.Female)
			{
				return number + "a";
			}
			if (number == 1 || number == 3)
			{
				return number + "r";
			}
			if (number == 2)
			{
				return number + "n";
			}
			if (number == 4)
			{
				return number + "t";
			}
			return number + "è";
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0003B7D1 File Offset: 0x000399D1
		public bool IsVowel(char ch)
		{
			return "ieɛaoɔuəuàêèéòóüúIEƐAOƆUƏUÀÊÈÉÒÓÜÚ".IndexOf(ch) >= 0;
		}
	}
}

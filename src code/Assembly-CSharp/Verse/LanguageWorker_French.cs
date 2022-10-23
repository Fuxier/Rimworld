using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200018E RID: 398
	public class LanguageWorker_French : LanguageWorker
	{
		// Token: 0x06000AF7 RID: 2807 RVA: 0x0003BD0B File Offset: 0x00039F0B
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "des " + str;
			}
			return ((gender == Gender.Female) ? "une " : "un ") + str;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0003BD38 File Offset: 0x00039F38
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
			if (plural)
			{
				return "les " + str;
			}
			char ch = str[0];
			if (this.IsVowel(ch))
			{
				return "l'" + str;
			}
			return ((gender == Gender.Female) ? "la " : "le ") + str;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003BD97 File Offset: 0x00039F97
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			if (number != 1)
			{
				return number + "e";
			}
			return number + "er";
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0003BDC0 File Offset: 0x00039FC0
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
			string item = str.ToLower();
			if (LanguageWorker_French.Exceptions1.Contains(item))
			{
				return str.Substring(0, str.Length - 3) + "aux";
			}
			if (LanguageWorker_French.Exceptions2.Contains(item))
			{
				return str + "s";
			}
			if (LanguageWorker_French.Exceptions3.Contains(item))
			{
				return str + "x";
			}
			if (str[str.Length - 1] == 's' || str[str.Length - 1] == 'x' || str[str.Length - 1] == 'z')
			{
				return str;
			}
			if (str.Length >= 2 && str[str.Length - 2] == 'a' && str[str.Length - 1] == 'l')
			{
				return str.Substring(0, str.Length - 2) + "aux";
			}
			if (str.Length >= 2 && str[str.Length - 2] == 'a' && str[str.Length - 1] == 'u')
			{
				return str.Substring(0, str.Length - 2) + "x";
			}
			if (str.Length >= 2 && str[str.Length - 2] == 'e' && str[str.Length - 1] == 'u')
			{
				return str.Substring(0, str.Length - 2) + "x";
			}
			return str + "s";
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003BF68 File Offset: 0x0003A168
		public override string PostProcessed(string str)
		{
			return this.PostProcessedInt(base.PostProcessed(str));
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003BF77 File Offset: 0x0003A177
		public bool IsVowel(char ch)
		{
			return "hiueøoɛœəɔaãɛ̃œ̃ɔ̃IHUEØOƐŒƏƆAÃƐ̃Œ̃Ɔ̃".IndexOf(ch) >= 0;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003BF8C File Offset: 0x0003A18C
		private string PostProcessedInt(string str)
		{
			str = str.Replace(" de le ", " du ").Replace("De le ", "Du ").Replace(" de les ", " des ").Replace("De les ", "Des ").Replace(" de des ", " des ").Replace("De des ", "Des ").Replace(" à le ", " au ").Replace("À le ", "Au ").Replace(" à les ", " aux ").Replace("À les ", "Aux ").Replace(" si il ", " s'il ").Replace("Si il ", "S'il ").Replace(" si ils ", " s'ils ").Replace("Si ils ", "S'ils ").Replace(" que il ", " qu'il ").Replace("Que il ", "Qu'il ").Replace(" que ils ", " qu'ils ").Replace("Que ils ", "Qu'ils ").Replace(" lorsque il ", " lorsqu'il ").Replace("Lorsque il ", "Lorsqu'il ").Replace(" lorsque ils ", " lorsqu'ils ").Replace("Lorsque ils ", "Lorsqu'ils ").Replace(" que elle ", " qu'elle ").Replace("Que elle ", "Qu'elle ").Replace(" que elles ", " qu'elles ").Replace("Que elles ", "Qu'elles ").Replace(" lorsque elle ", " lorsqu'elle ").Replace("Lorsque elle ", "Lorsqu'elle ").Replace(" lorsque elles ", " lorsqu'elles ").Replace("Lorsque elles ", "Lorsqu'elles ");
			LanguageWorker_French.tmpStr.Clear();
			LanguageWorker_French.tmpStr.Append(str);
			for (int i = 0; i < LanguageWorker_French.tmpStr.Length; i++)
			{
				if (i + 3 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == 'D' && LanguageWorker_French.tmpStr[i + 1] == 'e' && LanguageWorker_French.tmpStr[i + 2] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 3]))
				{
					LanguageWorker_French.tmpStr[i] = '\0';
					LanguageWorker_French.tmpStr[i + 1] = 'D';
					LanguageWorker_French.tmpStr[i + 2] = '\'';
				}
				else if (i + 3 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == 'L' && LanguageWorker_French.tmpStr[i + 1] == 'e' && LanguageWorker_French.tmpStr[i + 2] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 3]))
				{
					LanguageWorker_French.tmpStr[i] = '\0';
					LanguageWorker_French.tmpStr[i + 1] = 'L';
					LanguageWorker_French.tmpStr[i + 2] = '\'';
				}
				else if (i + 3 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == 'L' && LanguageWorker_French.tmpStr[i + 1] == 'a' && LanguageWorker_French.tmpStr[i + 2] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 3]))
				{
					LanguageWorker_French.tmpStr[i] = '\0';
					LanguageWorker_French.tmpStr[i + 1] = 'L';
					LanguageWorker_French.tmpStr[i + 2] = '\'';
				}
				else if (i + 4 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == ' ' && LanguageWorker_French.tmpStr[i + 1] == 'd' && LanguageWorker_French.tmpStr[i + 2] == 'e' && LanguageWorker_French.tmpStr[i + 3] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 4]))
				{
					LanguageWorker_French.tmpStr[i + 1] = '\0';
					LanguageWorker_French.tmpStr[i + 2] = 'd';
					LanguageWorker_French.tmpStr[i + 3] = '\'';
				}
				else if (i + 4 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == ' ' && LanguageWorker_French.tmpStr[i + 1] == 'l' && LanguageWorker_French.tmpStr[i + 2] == 'e' && LanguageWorker_French.tmpStr[i + 3] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 4]))
				{
					LanguageWorker_French.tmpStr[i + 1] = '\0';
					LanguageWorker_French.tmpStr[i + 2] = 'l';
					LanguageWorker_French.tmpStr[i + 3] = '\'';
				}
				else if (i + 4 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == ' ' && LanguageWorker_French.tmpStr[i + 1] == 'l' && LanguageWorker_French.tmpStr[i + 2] == 'a' && LanguageWorker_French.tmpStr[i + 3] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 4]))
				{
					LanguageWorker_French.tmpStr[i + 1] = '\0';
					LanguageWorker_French.tmpStr[i + 2] = 'l';
					LanguageWorker_French.tmpStr[i + 3] = '\'';
				}
			}
			str = LanguageWorker_French.tmpStr.ToString();
			str = str.Replace("\0", "");
			return str;
		}

		// Token: 0x04000A8E RID: 2702
		private static readonly List<string> Exceptions1 = new List<string>
		{
			"bail",
			"corail",
			"émail",
			"gemmail",
			"soupirail",
			"travail",
			"vantail",
			"vitrail"
		};

		// Token: 0x04000A8F RID: 2703
		private static readonly List<string> Exceptions2 = new List<string>
		{
			"bleu",
			"émeu",
			"landau",
			"lieu",
			"pneu",
			"sarrau",
			"bal",
			"banal",
			"fatal",
			"final",
			"festival"
		};

		// Token: 0x04000A90 RID: 2704
		private static readonly List<string> Exceptions3 = new List<string>
		{
			"bijou",
			"caillou",
			"chou",
			"genou",
			"hibou",
			"joujou",
			"pou"
		};

		// Token: 0x04000A91 RID: 2705
		private static StringBuilder tmpStr = new StringBuilder();
	}
}

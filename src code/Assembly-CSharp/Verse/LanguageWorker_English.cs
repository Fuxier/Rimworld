using System;

namespace Verse
{
	// Token: 0x0200018D RID: 397
	public class LanguageWorker_English : LanguageWorker
	{
		// Token: 0x06000AEF RID: 2799 RVA: 0x0003B8DB File Offset: 0x00039ADB
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return str;
			}
			return "a " + str;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0003B901 File Offset: 0x00039B01
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			return "the " + str;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0003B924 File Offset: 0x00039B24
		public override string PostProcessed(string str)
		{
			str = base.PostProcessed(str);
			if (str.StartsWith("a ", StringComparison.OrdinalIgnoreCase) && str.Length >= 3 && (str.Substring(2) == "hour" || str[2] == 'a' || str[2] == 'e' || str[2] == 'i' || str[2] == 'o' || str[2] == 'u'))
			{
				str = str.Insert(1, "n");
			}
			str = str.Replace(" a a", " an a");
			str = str.Replace(" a e", " an e");
			str = str.Replace(" a i", " an i");
			str = str.Replace(" a o", " an o");
			str = str.Replace(" a u", " an u");
			str = str.Replace(" a hour", " an hour");
			str = str.Replace(" an unique", " a unique");
			str = str.Replace(" A a", " An a");
			str = str.Replace(" A e", " An e");
			str = str.Replace(" A i", " An i");
			str = str.Replace(" A o", " An o");
			str = str.Replace(" A u", " An u");
			str = str.Replace(" A hour", " An hour");
			str = str.Replace(" An unique", " A unique");
			str = str.Replace("\na a", "\nan a");
			str = str.Replace("\na e", "\nan e");
			str = str.Replace("\na i", "\nan i");
			str = str.Replace("\na o", "\nan o");
			str = str.Replace("\na u", "\nan u");
			str = str.Replace("\na hour", "\nan hour");
			str = str.Replace("\nan unique", "\na unique");
			str = str.Replace("\nA a", "\nAn a");
			str = str.Replace("\nA e", "\nAn e");
			str = str.Replace("\nA i", "\nAn i");
			str = str.Replace("\nA o", "\nAn o");
			str = str.Replace("\nA u", "\nAn u");
			str = str.Replace("\nA hour", "\nAn hour");
			str = str.Replace("\nAn unique", "\nA unique");
			return str;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0003BBA2 File Offset: 0x00039DA2
		public override string ToTitleCase(string str)
		{
			return GenText.ToTitleCaseSmart(str);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0003BBAC File Offset: 0x00039DAC
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			int num = number % 10;
			if (number / 10 % 10 != 1)
			{
				if (num == 1)
				{
					return number + "st";
				}
				if (num == 2)
				{
					return number + "nd";
				}
				if (num == 3)
				{
					return number + "rd";
				}
			}
			return number + "th";
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0003BC18 File Offset: 0x00039E18
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
			if (str[str.Length - 1] == 's' || (count != -1 && count < 2))
			{
				return str;
			}
			int num = (int)str[str.Length - 1];
			char c = (str.Length == 1) ? '\0' : str[str.Length - 2];
			bool flag = char.IsLetter(c) && "oaieuyOAIEUY".IndexOf(c) >= 0;
			bool flag2 = char.IsLetter(c) && !flag;
			if (num == 121 && flag2)
			{
				return str.Substring(0, str.Length - 1) + "ies";
			}
			return str + "s";
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0003BCE0 File Offset: 0x00039EE0
		public override string PostProcessThingLabelForRelic(string thingLabel)
		{
			int num = thingLabel.LastIndexOf(' ');
			if (num != -1)
			{
				return thingLabel.Substring(num, thingLabel.Length - num);
			}
			return thingLabel;
		}
	}
}

using System;

namespace Verse
{
	// Token: 0x02000196 RID: 406
	public class LanguageWorker_Russian : LanguageWorker
	{
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0003D12A File Offset: 0x0003B32A
		public override int TotalNumCaseCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0003BBA2 File Offset: 0x00039DA2
		public override string ToTitleCase(string str)
		{
			return GenText.ToTitleCaseSmart(str);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0003D130 File Offset: 0x0003B330
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty() || (count != -1 && count < 2))
			{
				return str;
			}
			string text;
			if (!this.TryLookupPluralForm(str, gender, out text, count))
			{
				text = this.PluralizeFallback(str, gender, count);
			}
			if (count == -1)
			{
				return text;
			}
			string formSeveral;
			string formMany;
			if (this.TryLookUp("Case", str, 1, out formSeveral, null) && this.TryLookUp("Case", text, 1, out formMany, null))
			{
				return this.GetFormForNumber(count, str, formSeveral, formMany);
			}
			return text;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0003D19C File Offset: 0x0003B39C
		private string PluralizeFallback(string str, Gender gender, int count = -1)
		{
			char c = str[str.Length - 1];
			char c2 = (str.Length < 2) ? '\0' : str[str.Length - 2];
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					if (gender == Gender.None)
					{
						if (c == 'o')
						{
							return str.Substring(0, str.Length - 1) + "a";
						}
						if (c == 'O')
						{
							return str.Substring(0, str.Length - 1) + "A";
						}
						if (c == 'e' || c == 'E')
						{
							char value = char.ToUpper(c2);
							if ("ГКХЖЧШЩЦ".IndexOf(value) >= 0)
							{
								if (c == 'e')
								{
									return str.Substring(0, str.Length - 1) + "a";
								}
								if (c == 'E')
								{
									return str.Substring(0, str.Length - 1) + "A";
								}
							}
							else
							{
								if (c == 'e')
								{
									return str.Substring(0, str.Length - 1) + "я";
								}
								if (c == 'E')
								{
									return str.Substring(0, str.Length - 1) + "Я";
								}
							}
						}
					}
				}
				else
				{
					if (c == 'я')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'ь')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'Я')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'Ь')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'a' || c == 'A')
					{
						char value2 = char.ToUpper(c2);
						if ("ГКХЖЧШЩ".IndexOf(value2) >= 0)
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "и";
							}
							return str.Substring(0, str.Length - 1) + "И";
						}
						else
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "ы";
							}
							return str.Substring(0, str.Length - 1) + "Ы";
						}
					}
				}
			}
			else
			{
				if (LanguageWorker_Russian.IsConsonant(c))
				{
					return str + "ы";
				}
				if (c == 'й')
				{
					return str.Substring(0, str.Length - 1) + "и";
				}
				if (c == 'ь')
				{
					return str.Substring(0, str.Length - 1) + "и";
				}
				if (c == 'Й')
				{
					return str.Substring(0, str.Length - 1) + "И";
				}
				if (c == 'Ь')
				{
					return str.Substring(0, str.Length - 1) + "И";
				}
			}
			return str;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0003D482 File Offset: 0x0003B682
		private static bool IsConsonant(char ch)
		{
			return "бвгджзклмнпрстфхцчшщБВГДЖЗКЛМНПРСТФХЦЧШЩ".IndexOf(ch) >= 0;
		}
	}
}

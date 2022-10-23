using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000188 RID: 392
	public abstract class LanguageWorker
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual int TotalNumCaseCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0003B0FC File Offset: 0x000392FC
		public virtual string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("IndefiniteForm".CanTranslate())
			{
				return "IndefiniteForm".Translate(str);
			}
			return "IndefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0003B15E File Offset: 0x0003935E
		public string WithIndefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithIndefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null, Gender.Male), plural, name);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0003B176 File Offset: 0x00039376
		public string WithIndefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0003B189 File Offset: 0x00039389
		public string WithIndefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, plural, name));
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0003B19C File Offset: 0x0003939C
		public virtual string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("DefiniteForm".CanTranslate())
			{
				return "DefiniteForm".Translate(str);
			}
			return "DefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0003B1FE File Offset: 0x000393FE
		public string WithDefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithDefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null, Gender.Male), plural, name);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0003B216 File Offset: 0x00039416
		public string WithDefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0003B229 File Offset: 0x00039429
		public string WithDefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, plural, name));
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0003B23A File Offset: 0x0003943A
		public virtual string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString();
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0003B243 File Offset: 0x00039443
		public virtual string PostProcessed(string str)
		{
			return str.MergeMultipleSpaces(true);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0003B24C File Offset: 0x0003944C
		public virtual string ToTitleCase(string str)
		{
			return str.CapitalizeFirst();
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0003B254 File Offset: 0x00039454
		public virtual string Pluralize(string str, Gender gender, int count = -1)
		{
			string result;
			if (this.TryLookupPluralForm(str, gender, out result, count))
			{
				return result;
			}
			return str;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0003B271 File Offset: 0x00039471
		public string Pluralize(string str, int count = -1)
		{
			return this.Pluralize(str, LanguageDatabase.activeLanguage.ResolveGender(str, null, Gender.Male), count);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0003B288 File Offset: 0x00039488
		public virtual bool TryLookupPluralForm(string str, Gender gender, out string plural, int count = -1)
		{
			plural = null;
			if (str.NullOrEmpty() || (count != -1 && count < 2))
			{
				return false;
			}
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable("plural");
			if (lookupTable == null)
			{
				return false;
			}
			string key = str.ToLower();
			if (!lookupTable.ContainsKey(key))
			{
				return false;
			}
			string[] array = lookupTable[key];
			if (array.Length < 2)
			{
				return false;
			}
			plural = array[1];
			if (str.Length != 0 && char.IsUpper(str[0]))
			{
				plural = plural.CapitalizeFirst();
			}
			return true;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0003B310 File Offset: 0x00039510
		public virtual bool TryLookUp(string tableName, string keyName, int index, out string result, string fullStringForReference = null)
		{
			result = null;
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable(tableName);
			if (lookupTable == null)
			{
				return false;
			}
			if (keyName.NullOrEmpty())
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					Log.Warning("Tried to lookup an empty key in table '" + tableName + "'.");
				}
				result = keyName;
				return true;
			}
			string text = keyName.ToLower();
			if (!lookupTable.ContainsKey(text))
			{
				LanguageWorker.ParenthesisRegex.Replace(text, "");
				text = text.Trim();
				if (!lookupTable.ContainsKey(text))
				{
					if (DebugSettings.logTranslationLookupErrors)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Tried a lookup for key '",
							keyName,
							"' in table '",
							tableName,
							"', which doesn't exist."
						}));
					}
					result = keyName;
					return true;
				}
			}
			string[] array = lookupTable[text];
			if (array.Length < index + 1)
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					Log.Warning(string.Format("Tried a lookup an out-of-bounds index '{0}' for key '{1}' in table '{2}'.", index, keyName, tableName));
				}
				result = keyName;
				return true;
			}
			result = array[index];
			return true;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0003B40A File Offset: 0x0003960A
		public virtual string PostProcessThingLabelForRelic(string thingLabel)
		{
			if (thingLabel.IndexOf(' ') != -1)
			{
				return null;
			}
			return thingLabel;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0003B41C File Offset: 0x0003961C
		public virtual string ResolveNumCase(float number, List<string> args)
		{
			string formOne = args[0].Trim(new char[]
			{
				'\''
			});
			string text = args[1].Trim(new char[]
			{
				'\''
			});
			string formMany = args[2].Trim(new char[]
			{
				'\''
			});
			if (number - Mathf.Floor(number) > 1E-45f)
			{
				return number + " " + text;
			}
			return number + " " + this.GetFormForNumber((int)number, formOne, text, formMany);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0003B4B0 File Offset: 0x000396B0
		protected virtual string GetFormForNumber(int num, string formOne, string formSeveral, string formMany)
		{
			int num2 = num % 10;
			if (num / 10 % 10 == 1)
			{
				return formMany;
			}
			if (num2 == 1)
			{
				return formOne;
			}
			if (num2 - 2 > 2)
			{
				return formMany;
			}
			return formSeveral;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0003B4E4 File Offset: 0x000396E4
		public virtual string ResolveReplace(List<string> args)
		{
			if (args.Count == 0)
			{
				return null;
			}
			string text = args[0];
			if (args.Count == 1)
			{
				return text;
			}
			for (int i = 1; i < args.Count; i++)
			{
				string input = args[i];
				Match match = LanguageWorker.replaceArgRegex.Match(input);
				if (!match.Success)
				{
					return null;
				}
				string value = match.Groups["old"].Value;
				string value2 = match.Groups["new"].Value;
				if (text.Contains(value))
				{
					return text.Replace(value, value2);
				}
			}
			return text;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0003B581 File Offset: 0x00039781
		public virtual string ResolveFunction(string functionName, List<string> args, string fullStringForReference)
		{
			if (functionName == "lookup")
			{
				return this.ResolveLookup(args, fullStringForReference);
			}
			if (functionName == "replace")
			{
				return this.ResolveReplace(args);
			}
			return "";
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0003B5B4 File Offset: 0x000397B4
		protected string ResolveLookup(List<string> args, string fullStringForReference)
		{
			if (args.Count != 2 && args.Count != 3)
			{
				Log.Error("Invalid argument number for 'lookup' function, expected 2 or 3. A key to lookup, table name and optional index if there's more than 1 entry per key. Full string: " + fullStringForReference);
				return "";
			}
			string text = args[1];
			int index = 1;
			if (args.Count == 3 && !int.TryParse(args[2], out index))
			{
				Log.Error("Invalid lookup index value: '" + args[2] + "' Full string: " + fullStringForReference);
				return "";
			}
			string result;
			if (this.TryLookUp(text.ToLower(), args[0], index, out result, fullStringForReference))
			{
				return result;
			}
			return "";
		}

		// Token: 0x04000A8C RID: 2700
		private static Regex ParenthesisRegex = new Regex("\\((.*?)\\)");

		// Token: 0x04000A8D RID: 2701
		private static readonly Regex replaceArgRegex = new Regex("(?<old>[^\"]*?)\"-\"(?<new>[^\"]*?)\"", RegexOptions.Compiled);
	}
}

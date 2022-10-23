using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000421 RID: 1057
	public static class ColoredText
	{
		// Token: 0x06001F16 RID: 7958 RVA: 0x000B7DD4 File Offset: 0x000B5FD4
		public static void ResetStaticData()
		{
			ColoredText.DateTimeRegexes = new List<Regex>();
			ColoredText.AddRegexPatternsForDateString("PeriodYears".Translate(), "Period1Year".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodQuadrums".Translate(), "Period1Quadrum".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodDays".Translate(), "Period1Day".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodHours".Translate(), "Period1Hour".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodSeconds".Translate(), "Period1Second".Translate());
			string str = string.Concat(new string[]
			{
				"(",
				FactionDefOf.PlayerColony.pawnsPlural,
				"|",
				FactionDefOf.PlayerColony.pawnSingular,
				")"
			});
			ColoredText.ColonistCountRegex = new Regex("\\d+\\.?\\d* " + str);
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000B7EF0 File Offset: 0x000B60F0
		private static void AddRegexPatternsForDateString(string dateMany, string dateOne)
		{
			if (dateMany.Contains("{0}"))
			{
				ColoredText.DateTimeRegexes.Add(new Regex(string.Concat(new string[]
				{
					"(",
					string.Format(dateMany, "\\d+\\.?\\d*"),
					"|",
					dateOne,
					")"
				})));
				return;
			}
			List<string> list = GrammarResolverSimple.TryParseNumCase(dateMany);
			if (!list.NullOrEmpty<string>())
			{
				foreach (string text in list)
				{
					ColoredText.DateTimeRegexes.Add(new Regex(string.Concat(new string[]
					{
						"(\\d+\\.?\\d* ",
						text,
						"|",
						dateOne,
						")"
					})));
				}
			}
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000B7FD4 File Offset: 0x000B61D4
		public static void ClearCache()
		{
			ColoredText.cache.Clear();
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000B7FE0 File Offset: 0x000B61E0
		public static TaggedString ApplyTag(this string s, TagType tagType, string arg = null)
		{
			if (arg == null)
			{
				return string.Format("(*{0}){1}(/{0})", tagType.ToString(), s);
			}
			return string.Format("(*{0}={1}){2}(/{0})", tagType.ToString(), arg, s);
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x000B802C File Offset: 0x000B622C
		public static TaggedString ApplyTag(this string s, Faction faction)
		{
			if (faction == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Faction, faction.GetUniqueLoadID());
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000B8045 File Offset: 0x000B6245
		public static TaggedString ApplyTag(this string s, Ideo ideo)
		{
			if (ideo == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Ideo, ideo.GetUniqueLoadID());
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000B8060 File Offset: 0x000B6260
		public static string StripTags(this string s)
		{
			if (s.NullOrEmpty() || (s.IndexOf("(*") < 0 && s.IndexOf('<') < 0))
			{
				return s;
			}
			s = ColoredText.XMLRegex.Replace(s, string.Empty);
			return ColoredText.TagRegex.Replace(s, string.Empty);
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000B80B2 File Offset: 0x000B62B2
		public static string ResolveTags(this string str)
		{
			return ColoredText.Resolve(str);
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x000B80C0 File Offset: 0x000B62C0
		public static string Resolve(TaggedString taggedStr)
		{
			if (taggedStr == null)
			{
				return null;
			}
			string rawText = taggedStr.RawText;
			if (rawText.NullOrEmpty())
			{
				return rawText;
			}
			string result;
			if (ColoredText.cache.TryGetValue(rawText, out result))
			{
				return result;
			}
			ColoredText.resultBuffer.Length = 0;
			if (rawText.IndexOf("(*") < 0)
			{
				ColoredText.resultBuffer.Append(rawText);
			}
			else
			{
				for (int i = 0; i < rawText.Length; i++)
				{
					char c = rawText[i];
					if (c == '(' && i < rawText.Length - 1 && rawText[i + 1] == '*' && rawText.IndexOf(')', i) > i + 1)
					{
						bool flag = false;
						int num = i;
						ColoredText.tagBuffer.Length = 0;
						ColoredText.argBuffer.Length = 0;
						ColoredText.capStage = ColoredText.CaptureStage.Tag;
						for (i += 2; i < rawText.Length; i++)
						{
							char c2 = rawText[i];
							if (c2 == ')')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Result;
								if (flag)
								{
									string value = rawText.Substring(num, i - num + 1).SwapTagWithColor(ColoredText.tagBuffer.ToString(), ColoredText.argBuffer.ToString());
									ColoredText.resultBuffer.Append(value);
									break;
								}
							}
							else if (c2 == '/')
							{
								flag = true;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Arg)
							{
								ColoredText.argBuffer.Append(c2);
							}
							if (!flag && c2 == '=')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Arg;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Tag)
							{
								ColoredText.tagBuffer.Append(c2);
							}
						}
						if (!flag)
						{
							ColoredText.resultBuffer.Append(c);
							i = num + 1;
						}
					}
					else
					{
						ColoredText.resultBuffer.Append(c);
					}
				}
			}
			string text = ColoredText.resultBuffer.ToString();
			for (int j = 0; j < ColoredText.DateTimeRegexes.Count; j++)
			{
				text = ColoredText.DateTimeRegexes[j].Replace(text, "$&".Colorize(ColoredText.DateTimeColor));
			}
			text = ColoredText.CurrencyRegex.Replace(text, "$&".Colorize(ColoredText.CurrencyColor));
			text = ColoredText.ColonistCountRegex.Replace(text, "$&".Colorize(ColoredText.ColonistCountColor));
			ColoredText.cache.Add(rawText, text);
			return text;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000B82F8 File Offset: 0x000B64F8
		public static string Colorize(this TaggedString ts, Color color)
		{
			return ts.Resolve().Colorize(color);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x000B8307 File Offset: 0x000B6507
		public static string Colorize(this string s, Color color)
		{
			return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), s);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x000B831C File Offset: 0x000B651C
		private static string SwapTagWithColor(this string str, string tag, string arg)
		{
			TagType tagType = ColoredText.ParseEnum<TagType>(tag.CapitalizeFirst(), true);
			string text = str.StripTags();
			switch (tagType)
			{
			case TagType.Undefined:
				return str;
			case TagType.Name:
				return text.Colorize(ColoredText.NameColor);
			case TagType.Faction:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction = Find.FactionManager.AllFactions.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction == null)
				{
					return text.Colorize(ColoredText.SubtleGrayColor);
				}
				return text.Colorize(ColoredText.GetFactionRelationColor(faction));
			}
			case TagType.Settlement:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction2 = Find.FactionManager.AllFactionsVisible.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction2 == null)
				{
					return text.Colorize(ColoredText.SubtleGrayColor);
				}
				if (faction2 == null)
				{
					return text;
				}
				return text.Colorize(faction2.Color);
			}
			case TagType.DateTime:
				return text.Colorize(ColoredText.DateTimeColor);
			case TagType.ColonistCount:
				return text.Colorize(ColoredText.ColonistCountColor);
			case TagType.Threat:
				return text.Colorize(ColoredText.ThreatColor);
			case TagType.Ideo:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Ideo ideo = Find.IdeoManager.IdeosListForReading.Find((Ideo x) => x.GetUniqueLoadID() == arg);
				if (ideo == null)
				{
					return text;
				}
				return text.Colorize(ideo.TextColor);
			}
			case TagType.SectionTitle:
				return text.Colorize(ColoredText.TipSectionTitleColor);
			case TagType.Red:
				return text.Colorize(ColorLibrary.RedReadable);
			case TagType.Reward:
				return text.Colorize(ColoredText.CurrencyColor);
			case TagType.Gray:
				return text.Colorize(ColoredText.SubtleGrayColor);
			default:
				Log.ErrorOnce("Invalid tag '" + tag + "'", tag.GetHashCode());
				return text;
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x000B84E4 File Offset: 0x000B66E4
		private static Color GetFactionRelationColor(Faction faction)
		{
			if (faction == null)
			{
				return Color.white;
			}
			if (faction.IsPlayer)
			{
				return faction.Color;
			}
			switch (faction.RelationKindWith(Faction.OfPlayer))
			{
			case FactionRelationKind.Hostile:
				return ColoredText.FactionColor_Hostile;
			case FactionRelationKind.Neutral:
				return ColoredText.FactionColor_Neutral;
			case FactionRelationKind.Ally:
				return ColoredText.FactionColor_Ally;
			default:
				return faction.Color;
			}
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x000B8544 File Offset: 0x000B6744
		private static T ParseEnum<T>(string value, bool ignoreCase = true)
		{
			if (Enum.IsDefined(typeof(T), value))
			{
				return (T)((object)Enum.Parse(typeof(T), value, ignoreCase));
			}
			return default(T);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x000B8583 File Offset: 0x000B6783
		public static StringBuilder AppendTagged(this StringBuilder sb, TaggedString taggedString)
		{
			return sb.Append(taggedString.Resolve());
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x000B8592 File Offset: 0x000B6792
		public static StringBuilder AppendLineTagged(this StringBuilder sb, TaggedString taggedString)
		{
			return sb.AppendLine(taggedString.Resolve());
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000B85A1 File Offset: 0x000B67A1
		public static TaggedString ToTaggedString(this StringBuilder sb)
		{
			return new TaggedString(sb.ToString());
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x000B85AE File Offset: 0x000B67AE
		public static string AsTipTitle(this TaggedString ts)
		{
			return ts.Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x000B85BB File Offset: 0x000B67BB
		public static string AsTipTitle(this string s)
		{
			return s.Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x04001515 RID: 5397
		private static StringBuilder resultBuffer = new StringBuilder();

		// Token: 0x04001516 RID: 5398
		private static StringBuilder tagBuffer = new StringBuilder();

		// Token: 0x04001517 RID: 5399
		private static StringBuilder argBuffer = new StringBuilder();

		// Token: 0x04001518 RID: 5400
		private static Dictionary<string, string> cache = new Dictionary<string, string>();

		// Token: 0x04001519 RID: 5401
		private static ColoredText.CaptureStage capStage = ColoredText.CaptureStage.Result;

		// Token: 0x0400151A RID: 5402
		private static Regex ColonistCountRegex;

		// Token: 0x0400151B RID: 5403
		private static List<Regex> DateTimeRegexes;

		// Token: 0x0400151C RID: 5404
		public static readonly Color NameColor = GenColor.FromHex("d09b61");

		// Token: 0x0400151D RID: 5405
		public static readonly Color CurrencyColor = GenColor.FromHex("dbb40c");

		// Token: 0x0400151E RID: 5406
		public static readonly Color TipSectionTitleColor = new Color(0.9f, 0.9f, 0.3f);

		// Token: 0x0400151F RID: 5407
		public static readonly Color DateTimeColor = GenColor.FromHex("87f6f6");

		// Token: 0x04001520 RID: 5408
		public static readonly Color FactionColor_Ally = GenColor.FromHex("00ff00");

		// Token: 0x04001521 RID: 5409
		public static readonly Color FactionColor_Hostile = ColorLibrary.RedReadable;

		// Token: 0x04001522 RID: 5410
		public static readonly Color ThreatColor = GenColor.FromHex("d46f68");

		// Token: 0x04001523 RID: 5411
		public static readonly Color FactionColor_Neutral = GenColor.FromHex("00bfff");

		// Token: 0x04001524 RID: 5412
		public static readonly Color WarningColor = GenColor.FromHex("ff0000");

		// Token: 0x04001525 RID: 5413
		public static readonly Color ColonistCountColor = GenColor.FromHex("dcffaf");

		// Token: 0x04001526 RID: 5414
		public static readonly Color SubtleGrayColor = GenColor.FromHex("999999");

		// Token: 0x04001527 RID: 5415
		public static readonly Color ExpectationsColor = new Color(0.57f, 0.9f, 0.69f);

		// Token: 0x04001528 RID: 5416
		public static readonly Color ImpactColor = GenColor.FromHex("c79fef");

		// Token: 0x04001529 RID: 5417
		public static readonly Color GeneColor = ColorLibrary.LightBlue;

		// Token: 0x0400152A RID: 5418
		private static readonly Regex CurrencyRegex = new Regex("\\$\\d+\\.?\\d*");

		// Token: 0x0400152B RID: 5419
		private static readonly Regex TagRegex = new Regex("\\([\\*\\/][^\\)]*\\)");

		// Token: 0x0400152C RID: 5420
		private static readonly Regex XMLRegex = new Regex("<[^>]*>");

		// Token: 0x0400152D RID: 5421
		private const string Digits = "\\d+\\.?\\d*";

		// Token: 0x0400152E RID: 5422
		private const string Replacement = "$&";

		// Token: 0x0400152F RID: 5423
		private const string TagStartString = "(*";

		// Token: 0x04001530 RID: 5424
		private const char TagStartChar = '(';

		// Token: 0x04001531 RID: 5425
		private const char TagEndChar = ')';

		// Token: 0x02001EB8 RID: 7864
		private enum CaptureStage
		{
			// Token: 0x04007904 RID: 30980
			Tag,
			// Token: 0x04007905 RID: 30981
			Arg,
			// Token: 0x04007906 RID: 30982
			Result
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x02000192 RID: 402
	public class LanguageWorker_Korean : LanguageWorker
	{
		// Token: 0x06000B0F RID: 2831 RVA: 0x0003CB29 File Offset: 0x0003AD29
		public override string PostProcessed(string str)
		{
			return this.ReplaceJosa(base.PostProcessed(str));
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0003CB38 File Offset: 0x0003AD38
		public string ReplaceJosa(string src)
		{
			LanguageWorker_Korean.tmpStringBuilder.Length = 0;
			string text = this.StripTags(src);
			int num = 0;
			MatchCollection matchCollection = LanguageWorker_Korean.JosaPattern.Matches(src);
			MatchCollection matchCollection2 = LanguageWorker_Korean.JosaPattern.Matches(text);
			if (matchCollection2.Count < matchCollection.Count)
			{
				return src;
			}
			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];
				Match match2 = matchCollection2[i];
				LanguageWorker_Korean.tmpStringBuilder.Append(src, num, match.Index - num);
				char? c = this.FindLastChar(text, match2.Index);
				if (c != null)
				{
					LanguageWorker_Korean.tmpStringBuilder.Append(this.ResolveJosa(match.Value, c.Value));
				}
				else
				{
					LanguageWorker_Korean.tmpStringBuilder.Append(match.Value);
				}
				num = match.Index + match.Length;
			}
			LanguageWorker_Korean.tmpStringBuilder.Append(src, num, src.Length - num);
			return LanguageWorker_Korean.tmpStringBuilder.ToString();
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0003CC42 File Offset: 0x0003AE42
		private string StripTags(string inString)
		{
			if (LanguageWorker_Korean.TagOrNodeOpeningPattern.Match(inString).Success)
			{
				return LanguageWorker_Korean.TagOrNodeClosingPattern.Replace(inString, "");
			}
			return inString;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0003CC68 File Offset: 0x0003AE68
		private string ResolveJosa(string josaToken, char lastChar)
		{
			if (!char.IsLetterOrDigit(lastChar))
			{
				return josaToken;
			}
			if (!((josaToken == "(으)로") ? this.HasJongExceptRieul(lastChar) : this.HasJong(lastChar)))
			{
				return LanguageWorker_Korean.JosaPatternPaired[josaToken].Item2;
			}
			return LanguageWorker_Korean.JosaPatternPaired[josaToken].Item1;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0003CCC0 File Offset: 0x0003AEC0
		private char? FindLastChar(string stripped, int strippedMatchIndex)
		{
			if (strippedMatchIndex == 0)
			{
				return null;
			}
			char c = stripped[strippedMatchIndex - 1];
			if (c == '\'' || c == '"')
			{
				if (strippedMatchIndex == 1)
				{
					return null;
				}
				return new char?(stripped[strippedMatchIndex - 2]);
			}
			else
			{
				if (c != ')')
				{
					return new char?(c);
				}
				int num = stripped.LastIndexOf('(', strippedMatchIndex - 1, strippedMatchIndex - 1);
				if (num == -1)
				{
					return null;
				}
				for (int i = num; i >= 0; i--)
				{
					if (stripped[i] != ' ')
					{
						return new char?(stripped[i]);
					}
				}
				return null;
			}
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0003CD61 File Offset: 0x0003AF61
		private bool HasJong(char inChar)
		{
			if (!this.IsKorean(inChar))
			{
				return LanguageWorker_Korean.AlphabetEndPattern.Contains(inChar);
			}
			return this.ExtractJongCode(inChar) > 0;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0003CD84 File Offset: 0x0003AF84
		private bool HasJongExceptRieul(char inChar)
		{
			if (!this.IsKorean(inChar))
			{
				return inChar != 'l' && LanguageWorker_Korean.AlphabetEndPattern.Contains(inChar);
			}
			int num = this.ExtractJongCode(inChar);
			return num != 8 && num != 0;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0003CDBF File Offset: 0x0003AFBF
		private int ExtractJongCode(char inChar)
		{
			return (int)((inChar - '가') % '\u001c');
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0003CDCB File Offset: 0x0003AFCB
		private bool IsKorean(char inChar)
		{
			return inChar >= '가' && inChar <= '힣';
		}

		// Token: 0x04000A92 RID: 2706
		private static StringBuilder tmpStringBuilder = new StringBuilder();

		// Token: 0x04000A93 RID: 2707
		private static readonly Regex JosaPattern = new Regex("\\(이\\)가|\\(와\\)과|\\(을\\)를|\\(은\\)는|\\(아\\)야|\\(이\\)어|\\(으\\)로|\\(이\\)", RegexOptions.Compiled);

		// Token: 0x04000A94 RID: 2708
		private static readonly Dictionary<string, ValueTuple<string, string>> JosaPatternPaired = new Dictionary<string, ValueTuple<string, string>>
		{
			{
				"(이)가",
				new ValueTuple<string, string>("이", "가")
			},
			{
				"(와)과",
				new ValueTuple<string, string>("과", "와")
			},
			{
				"(을)를",
				new ValueTuple<string, string>("을", "를")
			},
			{
				"(은)는",
				new ValueTuple<string, string>("은", "는")
			},
			{
				"(아)야",
				new ValueTuple<string, string>("아", "야")
			},
			{
				"(이)어",
				new ValueTuple<string, string>("이어", "여")
			},
			{
				"(으)로",
				new ValueTuple<string, string>("으로", "로")
			},
			{
				"(이)",
				new ValueTuple<string, string>("이", "")
			}
		};

		// Token: 0x04000A95 RID: 2709
		private static readonly Regex TagOrNodeOpeningPattern = new Regex("\\(\\*|<", RegexOptions.Compiled);

		// Token: 0x04000A96 RID: 2710
		private static readonly Regex TagOrNodeClosingPattern = new Regex("(\\(|<)\\/\\w+(\\)|>)", RegexOptions.Compiled);

		// Token: 0x04000A97 RID: 2711
		private static readonly List<char> AlphabetEndPattern = new List<char>
		{
			'b',
			'c',
			'k',
			'l',
			'm',
			'n',
			'p',
			'q',
			't'
		};
	}
}

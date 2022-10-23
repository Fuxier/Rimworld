using System;

namespace Verse
{
	// Token: 0x020004C6 RID: 1222
	public struct TaggedString
	{
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x000EAB71 File Offset: 0x000E8D71
		public string RawText
		{
			get
			{
				return this.rawText;
			}
		}

		// Token: 0x17000721 RID: 1825
		public char this[int i]
		{
			get
			{
				return this.RawText[i];
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x000EAB87 File Offset: 0x000E8D87
		public int Length
		{
			get
			{
				return this.RawText.Length;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x000EAB94 File Offset: 0x000E8D94
		public int StrippedLength
		{
			get
			{
				return this.RawText.StripTags().Length;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x000EABA6 File Offset: 0x000E8DA6
		public static TaggedString Empty
		{
			get
			{
				if (TaggedString.empty == null)
				{
					TaggedString.empty = new TaggedString("");
				}
				return TaggedString.empty;
			}
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000EABC8 File Offset: 0x000E8DC8
		public TaggedString(string dat)
		{
			this.rawText = dat;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000EABD1 File Offset: 0x000E8DD1
		public string Resolve()
		{
			return ColoredText.Resolve(this);
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000EABE0 File Offset: 0x000E8DE0
		public TaggedString CapitalizeFirst()
		{
			if (this.rawText.NullOrEmpty())
			{
				return this;
			}
			int num = this.FirstLetterBetweenTags();
			if (char.ToUpper(this.rawText[num]) == this.rawText[num])
			{
				return this;
			}
			if (this.rawText.Length == 1)
			{
				return new TaggedString(this.rawText.ToUpper());
			}
			if (num == 0)
			{
				return new TaggedString(char.ToUpper(this.rawText[num]).ToString() + this.rawText.Substring(num + 1));
			}
			return new TaggedString(this.rawText.Substring(0, num) + char.ToUpper(this.rawText[num]).ToString() + this.rawText.Substring(num + 1));
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000EACC0 File Offset: 0x000E8EC0
		private int FirstLetterBetweenTags()
		{
			bool flag = false;
			for (int i = 0; i < this.rawText.Length - 1; i++)
			{
				if (this.rawText[i] == '(' && this.rawText[i + 1] == '*')
				{
					flag = true;
				}
				else
				{
					if (flag && this.rawText[i] == ')' && this.rawText[i + 1] != '(')
					{
						return i + 1;
					}
					if (!flag)
					{
						return i;
					}
				}
			}
			return 0;
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000EAD3C File Offset: 0x000E8F3C
		public bool NullOrEmpty()
		{
			return this.RawText.NullOrEmpty();
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000EAD49 File Offset: 0x000E8F49
		public TaggedString AdjustedFor(Pawn p, string pawnSymbol = "PAWN", bool addRelationInfoSymbol = true)
		{
			return this.RawText.AdjustedFor(p, pawnSymbol, addRelationInfoSymbol);
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000EAD5E File Offset: 0x000E8F5E
		public float GetWidthCached()
		{
			return this.RawText.StripTags().GetWidthCached();
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000EAD70 File Offset: 0x000E8F70
		public TaggedString Trim()
		{
			return new TaggedString(this.RawText.Trim());
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000EAD82 File Offset: 0x000E8F82
		public TaggedString Shorten()
		{
			this.rawText = this.rawText.Shorten();
			return this;
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000EAD9B File Offset: 0x000E8F9B
		public TaggedString ToLower()
		{
			return new TaggedString(this.RawText.ToLower());
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000EADAD File Offset: 0x000E8FAD
		public TaggedString Replace(string oldValue, string newValue)
		{
			return new TaggedString(this.RawText.Replace(oldValue, newValue));
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000EADC1 File Offset: 0x000E8FC1
		public static implicit operator string(TaggedString taggedString)
		{
			return taggedString.RawText.StripTags();
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000EADCF File Offset: 0x000E8FCF
		public static implicit operator TaggedString(string str)
		{
			return new TaggedString(str);
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000EADD7 File Offset: 0x000E8FD7
		public static TaggedString operator +(TaggedString t1, TaggedString t2)
		{
			return new TaggedString(t1.RawText + t2.RawText);
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x000EADF1 File Offset: 0x000E8FF1
		public static TaggedString operator +(string t1, TaggedString t2)
		{
			return new TaggedString(t1 + t2.RawText);
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000EAE05 File Offset: 0x000E9005
		public static TaggedString operator +(TaggedString t1, string t2)
		{
			return new TaggedString(t1.RawText + t2);
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x000EAE19 File Offset: 0x000E9019
		public override string ToString()
		{
			return this.RawText;
		}

		// Token: 0x040017A1 RID: 6049
		private string rawText;

		// Token: 0x040017A2 RID: 6050
		private static TaggedString empty;
	}
}

using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000366 RID: 870
	public class NameTriple : Name
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x000891FB File Offset: 0x000873FB
		public string First
		{
			get
			{
				return this.firstInt;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x00089203 File Offset: 0x00087403
		public string Last
		{
			get
			{
				return this.lastInt;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x0008920C File Offset: 0x0008740C
		public string Nick
		{
			get
			{
				if (!this.nickInt.NullOrEmpty())
				{
					return this.nickInt;
				}
				if (this.Last == "")
				{
					return this.First;
				}
				if (this.First == "")
				{
					return this.Last;
				}
				if ((Gen.HashCombine<int>(this.First.GetHashCode(), this.Last.GetHashCode()) & 1) == 1)
				{
					return this.First;
				}
				return this.Last;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x0008928C File Offset: 0x0008748C
		public override string ToStringFull
		{
			get
			{
				if (this.First == this.Nick || this.Last == this.Nick)
				{
					return this.First + " " + this.Last;
				}
				return string.Concat(new string[]
				{
					this.First,
					" '",
					this.Nick,
					"' ",
					this.Last
				});
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x0008930C File Offset: 0x0008750C
		public override string ToStringShort
		{
			get
			{
				return this.Nick;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x00089314 File Offset: 0x00087514
		public override bool IsValid
		{
			get
			{
				return !this.First.NullOrEmpty() && !this.Last.NullOrEmpty();
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x0000249D File Offset: 0x0000069D
		public override bool Numerical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x00089333 File Offset: 0x00087533
		public bool NickSet
		{
			get
			{
				return !this.nickInt.NullOrEmpty();
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x00089343 File Offset: 0x00087543
		public static NameTriple Invalid
		{
			get
			{
				return NameTriple.invalidInt;
			}
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x0008911D File Offset: 0x0008731D
		public NameTriple()
		{
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x0008934A File Offset: 0x0008754A
		public NameTriple(string first, string nick, string last)
		{
			this.firstInt = first.Trim();
			this.nickInt = ((nick != null) ? nick.Trim() : null);
			this.lastInt = last.Trim();
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0008937C File Offset: 0x0008757C
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.firstInt, "first", null, false);
			Scribe_Values.Look<string>(ref this.nickInt, "nick", null, false);
			Scribe_Values.Look<string>(ref this.lastInt, "last", null, false);
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x000893B4 File Offset: 0x000875B4
		public void PostLoad()
		{
			string text = this.firstInt;
			this.firstInt = ((text != null) ? text.Trim() : null);
			string text2 = this.nickInt;
			this.nickInt = ((text2 != null) ? text2.Trim() : null);
			string text3 = this.lastInt;
			this.lastInt = ((text3 != null) ? text3.Trim() : null);
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0008940C File Offset: 0x0008760C
		public void ResolveMissingPieces(string overrideLastName = null)
		{
			if (this.First.NullOrEmpty() && this.Nick.NullOrEmpty() && this.Last.NullOrEmpty())
			{
				Log.Error("Cannot resolve missing pieces in PawnName: No name data.");
				this.firstInt = (this.nickInt = (this.lastInt = "Empty"));
				return;
			}
			if (this.First == null)
			{
				this.firstInt = "";
			}
			if (this.Last == null)
			{
				this.lastInt = "";
			}
			if (overrideLastName != null)
			{
				this.lastInt = overrideLastName;
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00089498 File Offset: 0x00087698
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameTriple nameTriple = other as NameTriple;
			if (nameTriple != null)
			{
				if (this.Nick != null && this.Nick == nameTriple.Nick)
				{
					return true;
				}
				if (this.First == nameTriple.First && this.Last == nameTriple.Last)
				{
					return true;
				}
			}
			NameSingle nameSingle = other as NameSingle;
			return nameSingle != null && nameSingle.Name == this.Nick;
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00089514 File Offset: 0x00087714
		public static NameTriple FromString(string rawName, bool forceNoNick = false)
		{
			if (rawName.Trim().Length == 0)
			{
				Log.Error("Tried to parse PawnName from empty or whitespace string.");
				return NameTriple.Invalid;
			}
			NameTriple nameTriple = new NameTriple();
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < rawName.Length - 1; i++)
			{
				if (rawName[i] == ' ' && rawName[i + 1] == '\'' && num == -1)
				{
					num = i;
				}
				if (rawName[i] == '\'' && rawName[i + 1] == ' ')
				{
					num2 = i;
				}
			}
			if (num == -1 || num2 == -1)
			{
				if (!rawName.Contains(' '))
				{
					nameTriple.nickInt = rawName.Trim();
				}
				else
				{
					string[] array = rawName.Split(new char[]
					{
						' '
					});
					if (array.Length == 1)
					{
						nameTriple.nickInt = array[0].Trim();
					}
					else if (array.Length == 2)
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = array[1].Trim();
					}
					else
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = "";
						for (int j = 1; j < array.Length; j++)
						{
							NameTriple nameTriple2 = nameTriple;
							nameTriple2.lastInt += array[j];
							if (j < array.Length - 1)
							{
								NameTriple nameTriple3 = nameTriple;
								nameTriple3.lastInt += " ";
							}
						}
					}
				}
			}
			else
			{
				nameTriple.firstInt = rawName.Substring(0, num).Trim();
				if (!forceNoNick)
				{
					nameTriple.nickInt = rawName.Substring(num + 2, num2 - num - 2).Trim();
				}
				nameTriple.lastInt = ((num2 < rawName.Length - 2) ? rawName.Substring(num2 + 2).Trim() : "");
			}
			nameTriple.ResolveMissingPieces(null);
			return nameTriple;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x000896D3 File Offset: 0x000878D3
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.First,
				" '",
				this.Nick,
				"' ",
				this.Last
			});
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x0008970C File Offset: 0x0008790C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameTriple))
			{
				return false;
			}
			NameTriple nameTriple = (NameTriple)obj;
			return this.First == nameTriple.First && this.Last == nameTriple.Last && this.Nick == nameTriple.Nick;
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00089768 File Offset: 0x00087968
		public NameTriple WithoutNick()
		{
			return new NameTriple(this.firstInt, null, this.lastInt);
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0008977C File Offset: 0x0008797C
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.First), this.Last), this.Nick);
		}

		// Token: 0x170004B9 RID: 1209
		public string this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.First;
				case 1:
					return this.Nick;
				case 2:
					return this.Last;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x040011D7 RID: 4567
		[LoadAlias("first")]
		private string firstInt;

		// Token: 0x040011D8 RID: 4568
		[LoadAlias("nick")]
		private string nickInt;

		// Token: 0x040011D9 RID: 4569
		[LoadAlias("last")]
		private string lastInt;

		// Token: 0x040011DA RID: 4570
		private static NameTriple invalidInt = new NameTriple("Invalid", "Invalid", "Invalid");
	}
}

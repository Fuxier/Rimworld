using System;

namespace Verse
{
	// Token: 0x02000365 RID: 869
	public class NameSingle : Name
	{
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x00088FEC File Offset: 0x000871EC
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x00088FEC File Offset: 0x000871EC
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x00088FEC File Offset: 0x000871EC
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001751 RID: 5969 RVA: 0x00088FF4 File Offset: 0x000871F4
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x00089004 File Offset: 0x00087204
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001753 RID: 5971 RVA: 0x0008900C File Offset: 0x0008720C
		private int FirstDigitPosition
		{
			get
			{
				if (!this.numerical)
				{
					return -1;
				}
				if (this.nameInt.NullOrEmpty() || !char.IsDigit(this.nameInt[this.nameInt.Length - 1]))
				{
					return -1;
				}
				for (int i = this.nameInt.Length - 2; i >= 0; i--)
				{
					if (!char.IsDigit(this.nameInt[i]))
					{
						return i + 1;
					}
				}
				return 0;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x00089084 File Offset: 0x00087284
		public string NameWithoutNumber
		{
			get
			{
				if (!this.numerical)
				{
					return this.nameInt;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return this.nameInt;
				}
				int num = firstDigitPosition;
				if (num - 1 >= 0 && this.nameInt[num - 1] == ' ')
				{
					num--;
				}
				if (num <= 0)
				{
					return "";
				}
				return this.nameInt.Substring(0, num);
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x000890E8 File Offset: 0x000872E8
		public int Number
		{
			get
			{
				if (!this.numerical)
				{
					return 0;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return 0;
				}
				return int.Parse(this.nameInt.Substring(firstDigitPosition));
			}
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x0008911D File Offset: 0x0008731D
		public NameSingle()
		{
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00089125 File Offset: 0x00087325
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0008913B File Offset: 0x0008733B
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x00089164 File Offset: 0x00087364
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameSingle nameSingle = other as NameSingle;
			if (nameSingle != null && nameSingle.nameInt == this.nameInt)
			{
				return true;
			}
			NameTriple nameTriple = other as NameTriple;
			return nameTriple != null && nameTriple.Nick == this.nameInt;
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x00088FEC File Offset: 0x000871EC
		public override string ToString()
		{
			return this.nameInt;
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x000891B0 File Offset: 0x000873B0
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameSingle))
			{
				return false;
			}
			NameSingle nameSingle = (NameSingle)obj;
			return this.nameInt == nameSingle.nameInt;
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x000891E4 File Offset: 0x000873E4
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}

		// Token: 0x040011D5 RID: 4565
		private string nameInt;

		// Token: 0x040011D6 RID: 4566
		private bool numerical;
	}
}

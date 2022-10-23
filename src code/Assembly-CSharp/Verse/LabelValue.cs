using System;

namespace Verse
{
	// Token: 0x020004F6 RID: 1270
	public struct LabelValue
	{
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060026B6 RID: 9910 RVA: 0x000F8E75 File Offset: 0x000F7075
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060026B7 RID: 9911 RVA: 0x000F8E7D File Offset: 0x000F707D
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000F8E85 File Offset: 0x000F7085
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000F8E75 File Offset: 0x000F7075
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x04001954 RID: 6484
		private string label;

		// Token: 0x04001955 RID: 6485
		private string value;
	}
}

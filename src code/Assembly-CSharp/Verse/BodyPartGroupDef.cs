using System;

namespace Verse
{
	// Token: 0x020000AE RID: 174
	public class BodyPartGroupDef : Def
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001FD19 File Offset: 0x0001DF19
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001FD35 File Offset: 0x0001DF35
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x040002E0 RID: 736
		[MustTranslate]
		public string labelShort;

		// Token: 0x040002E1 RID: 737
		public int listOrder;

		// Token: 0x040002E2 RID: 738
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}

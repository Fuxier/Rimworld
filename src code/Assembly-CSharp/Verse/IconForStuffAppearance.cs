using System;

namespace Verse
{
	// Token: 0x020000B2 RID: 178
	public class IconForStuffAppearance
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0001FD92 File Offset: 0x0001DF92
		public StuffAppearanceDef Appearance
		{
			get
			{
				return this.appearance;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0001FD9A File Offset: 0x0001DF9A
		public string IconPath
		{
			get
			{
				return this.iconPath;
			}
		}

		// Token: 0x040002EA RID: 746
		private StuffAppearanceDef appearance;

		// Token: 0x040002EB RID: 747
		[NoTranslate]
		private string iconPath;
	}
}

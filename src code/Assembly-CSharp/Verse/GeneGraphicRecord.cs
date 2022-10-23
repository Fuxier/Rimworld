using System;

namespace Verse
{
	// Token: 0x020002A6 RID: 678
	public struct GeneGraphicRecord
	{
		// Token: 0x0600134D RID: 4941 RVA: 0x00073C1A File Offset: 0x00071E1A
		public GeneGraphicRecord(Graphic graphic, Graphic rottingGraphic, Gene sourceGene)
		{
			this.graphic = graphic;
			this.rottingGraphic = rottingGraphic;
			this.sourceGene = sourceGene;
		}

		// Token: 0x04000FF6 RID: 4086
		public Graphic graphic;

		// Token: 0x04000FF7 RID: 4087
		public Graphic rottingGraphic;

		// Token: 0x04000FF8 RID: 4088
		public Gene sourceGene;
	}
}

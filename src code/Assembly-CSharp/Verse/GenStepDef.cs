using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000F4 RID: 244
	public class GenStepDef : Def
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x00024E27 File Offset: 0x00023027
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		// Token: 0x040005AC RID: 1452
		public SitePartDef linkWithSite;

		// Token: 0x040005AD RID: 1453
		public float order;

		// Token: 0x040005AE RID: 1454
		public GenStep genStep;
	}
}

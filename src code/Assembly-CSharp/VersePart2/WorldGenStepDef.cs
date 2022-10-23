using System;

namespace Verse
{
	// Token: 0x02000154 RID: 340
	public class WorldGenStepDef : Def
	{
		// Token: 0x060008E1 RID: 2273 RVA: 0x0002B843 File Offset: 0x00029A43
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}

		// Token: 0x0400097F RID: 2431
		public float order;

		// Token: 0x04000980 RID: 2432
		public WorldGenStep worldGenStep;
	}
}

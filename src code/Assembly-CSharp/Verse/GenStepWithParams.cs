using System;

namespace Verse
{
	// Token: 0x020000F6 RID: 246
	public struct GenStepWithParams
	{
		// Token: 0x060006E7 RID: 1767 RVA: 0x00024E3B File Offset: 0x0002303B
		public GenStepWithParams(GenStepDef def, GenStepParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x040005B0 RID: 1456
		public GenStepDef def;

		// Token: 0x040005B1 RID: 1457
		public GenStepParams parms;
	}
}

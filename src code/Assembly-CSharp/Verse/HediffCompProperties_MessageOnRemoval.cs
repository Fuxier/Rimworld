using System;

namespace Verse
{
	// Token: 0x0200030A RID: 778
	public class HediffCompProperties_MessageOnRemoval : HediffCompProperties_MessageBase
	{
		// Token: 0x0600154E RID: 5454 RVA: 0x000800E5 File Offset: 0x0007E2E5
		public HediffCompProperties_MessageOnRemoval()
		{
			this.compClass = typeof(HediffComp_MessageOnRemoval);
		}

		// Token: 0x04001127 RID: 4391
		public bool messageOnZeroSeverity = true;

		// Token: 0x04001128 RID: 4392
		public bool messageOnNonZeroSeverity = true;
	}
}

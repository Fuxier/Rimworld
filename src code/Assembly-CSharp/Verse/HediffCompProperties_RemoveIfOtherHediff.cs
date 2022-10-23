using System;

namespace Verse
{
	// Token: 0x0200031A RID: 794
	public class HediffCompProperties_RemoveIfOtherHediff : HediffCompProperties_MessageBase
	{
		// Token: 0x0600157A RID: 5498 RVA: 0x000808CC File Offset: 0x0007EACC
		public HediffCompProperties_RemoveIfOtherHediff()
		{
			this.compClass = typeof(HediffComp_RemoveIfOtherHediff);
		}

		// Token: 0x0400113F RID: 4415
		public HediffDef otherHediff;

		// Token: 0x04001140 RID: 4416
		public IntRange? stages;

		// Token: 0x04001141 RID: 4417
		public int mtbHours;
	}
}

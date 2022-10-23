using System;

namespace Verse
{
	// Token: 0x0200030C RID: 780
	public class HediffCompProperties_Pollution : HediffCompProperties
	{
		// Token: 0x06001552 RID: 5458 RVA: 0x00080177 File Offset: 0x0007E377
		public HediffCompProperties_Pollution()
		{
			this.compClass = typeof(HediffComp_Pollution);
		}

		// Token: 0x04001129 RID: 4393
		public float pollutedSeverity;

		// Token: 0x0400112A RID: 4394
		public float unpollutedSeverity;

		// Token: 0x0400112B RID: 4395
		public int interval = 1;
	}
}

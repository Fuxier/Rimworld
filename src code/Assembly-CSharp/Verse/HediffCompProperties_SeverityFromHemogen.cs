using System;

namespace Verse
{
	// Token: 0x02000325 RID: 805
	public class HediffCompProperties_SeverityFromHemogen : HediffCompProperties
	{
		// Token: 0x06001598 RID: 5528 RVA: 0x00080D58 File Offset: 0x0007EF58
		public HediffCompProperties_SeverityFromHemogen()
		{
			this.compClass = typeof(HediffComp_SeverityFromHemogen);
		}

		// Token: 0x0400114E RID: 4430
		public float severityPerHourEmpty;

		// Token: 0x0400114F RID: 4431
		public float severityPerHourHemogen;
	}
}

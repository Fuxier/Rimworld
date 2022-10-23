using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000330 RID: 816
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x060015CF RID: 5583 RVA: 0x0008195D File Offset: 0x0007FB5D
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		// Token: 0x04001169 RID: 4457
		public float immunityPerDayNotSick;

		// Token: 0x0400116A RID: 4458
		public float immunityPerDaySick;

		// Token: 0x0400116B RID: 4459
		public float severityPerDayNotImmune;

		// Token: 0x0400116C RID: 4460
		public float severityPerDayImmune;

		// Token: 0x0400116D RID: 4461
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x0400116E RID: 4462
		public List<HediffDefFactor> severityFactorsFromHediffs = new List<HediffDefFactor>();

		// Token: 0x0400116F RID: 4463
		public bool hidden;
	}
}

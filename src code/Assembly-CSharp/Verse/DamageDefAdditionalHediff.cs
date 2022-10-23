using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B8 RID: 184
	public class DamageDefAdditionalHediff
	{
		// Token: 0x04000369 RID: 873
		public HediffDef hediff;

		// Token: 0x0400036A RID: 874
		public float severityPerDamageDealt = 0.1f;

		// Token: 0x0400036B RID: 875
		public float severityFixed;

		// Token: 0x0400036C RID: 876
		public StatDef victimSeverityScaling;

		// Token: 0x0400036D RID: 877
		public bool inverseStatScaling;

		// Token: 0x0400036E RID: 878
		public bool victimSeverityScalingByInvBodySize;
	}
}

using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000320 RID: 800
	public class HediffCompProperties_SeverityFromGas : HediffCompProperties
	{
		// Token: 0x06001589 RID: 5513 RVA: 0x00080ABD File Offset: 0x0007ECBD
		public HediffCompProperties_SeverityFromGas()
		{
			this.compClass = typeof(HediffComp_SeverityFromGas);
		}

		// Token: 0x04001146 RID: 4422
		public GasType gasType;

		// Token: 0x04001147 RID: 4423
		public float severityGasDensityFactor;

		// Token: 0x04001148 RID: 4424
		public int intervalTicks = 60;

		// Token: 0x04001149 RID: 4425
		public float severityNotExposed;

		// Token: 0x0400114A RID: 4426
		public StatDef exposureStatFactor;
	}
}

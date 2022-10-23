using System;

namespace Verse
{
	// Token: 0x020002EB RID: 747
	public class HediffCompProperties_GiveHediffLungRot : HediffCompProperties_GiveHediff
	{
		// Token: 0x060014EC RID: 5356 RVA: 0x0007E877 File Offset: 0x0007CA77
		public HediffCompProperties_GiveHediffLungRot()
		{
			this.compClass = typeof(HediffComp_GiveHediffLungRot);
		}

		// Token: 0x040010EB RID: 4331
		public SimpleCurve mtbOverRotGasExposureCurve;

		// Token: 0x040010EC RID: 4332
		public float minSeverity = 0.5f;

		// Token: 0x040010ED RID: 4333
		public int mtbCheckDuration = 60;
	}
}

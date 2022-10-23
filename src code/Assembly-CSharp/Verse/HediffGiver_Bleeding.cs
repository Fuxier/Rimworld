using System;

namespace Verse
{
	// Token: 0x02000355 RID: 853
	public class HediffGiver_Bleeding : HediffGiver
	{
		// Token: 0x060016DB RID: 5851 RVA: 0x00086158 File Offset: 0x00084358
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			if (hediffSet.BleedRateTotal >= 0.1f)
			{
				HealthUtility.AdjustSeverity(pawn, this.hediff, hediffSet.BleedRateTotal * 0.001f);
				return;
			}
			HealthUtility.AdjustSeverity(pawn, this.hediff, -0.00033333333f);
		}
	}
}

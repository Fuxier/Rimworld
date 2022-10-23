using System;

namespace Verse
{
	// Token: 0x0200035D RID: 861
	public class HediffGiver_Refresh : HediffGiver
	{
		// Token: 0x060016EC RID: 5868 RVA: 0x0008686C File Offset: 0x00084A6C
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false);
			if (firstHediffOfDef != null)
			{
				firstHediffOfDef.ageTicks = 0;
				return;
			}
			if (base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}
	}
}

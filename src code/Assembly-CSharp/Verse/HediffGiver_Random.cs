using System;

namespace Verse
{
	// Token: 0x0200035A RID: 858
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x060016E6 RID: 5862 RVA: 0x000866F8 File Offset: 0x000848F8
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float num = this.mtbDays;
			float num2 = this.ChanceFactor(pawn);
			if (num2 == 0f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(num / num2, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x040011B7 RID: 4535
		public float mtbDays;
	}
}

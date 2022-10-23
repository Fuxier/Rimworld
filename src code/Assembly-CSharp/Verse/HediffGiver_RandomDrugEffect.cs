using System;

namespace Verse
{
	// Token: 0x0200035B RID: 859
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x060016E8 RID: 5864 RVA: 0x00086744 File Offset: 0x00084944
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if ((this.severityToMtbDaysCurve == null && cause.Severity <= this.minSeverity) || (this.severityToMtbDaysCurve != null && cause.Severity <= this.severityToMtbDaysCurve.Points[0].y))
			{
				return;
			}
			float num = (this.severityToMtbDaysCurve != null) ? this.severityToMtbDaysCurve.Evaluate(cause.Severity) : this.baseMtbDays;
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

		// Token: 0x040011B8 RID: 4536
		public SimpleCurve severityToMtbDaysCurve;

		// Token: 0x040011B9 RID: 4537
		public float baseMtbDays;

		// Token: 0x040011BA RID: 4538
		public float minSeverity;
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000353 RID: 851
	public class HediffGiver_AddSeverity : HediffGiver
	{
		// Token: 0x060016D0 RID: 5840 RVA: 0x00085EB0 File Offset: 0x000840B0
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (pawn.IsNestedHashIntervalTick(60, HediffGiver_AddSeverity.mtbCheckInterval) && Rand.MTBEventOccurs(this.mtbHours, 2500f, (float)HediffGiver_AddSeverity.mtbCheckInterval))
			{
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false).Severity += this.severityAmount;
			}
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00085F1F File Offset: 0x0008411F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (float.IsNaN(this.severityAmount))
			{
				yield return "severityAmount is not defined";
			}
			if (this.mtbHours < 0f)
			{
				yield return "mtbHours is not defined";
			}
			yield break;
			yield break;
		}

		// Token: 0x040011A9 RID: 4521
		public float severityAmount = float.NaN;

		// Token: 0x040011AA RID: 4522
		public float mtbHours = -1f;

		// Token: 0x040011AB RID: 4523
		private static int mtbCheckInterval = 1200;
	}
}

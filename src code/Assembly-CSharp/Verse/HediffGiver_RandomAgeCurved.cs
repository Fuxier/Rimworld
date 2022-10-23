using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200035C RID: 860
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x060016EA RID: 5866 RVA: 0x000867EC File Offset: 0x000849EC
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float x = (float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy;
			if (Rand.MTBEventOccurs(this.ageFractionMtbDaysCurve.Evaluate(x), 60000f, 60f))
			{
				if (this.minPlayerPopulation > 0 && pawn.Faction == Faction.OfPlayer && PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoSuspended.Count<Pawn>() < this.minPlayerPopulation)
				{
					return;
				}
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
			}
		}

		// Token: 0x040011BB RID: 4539
		public SimpleCurve ageFractionMtbDaysCurve;

		// Token: 0x040011BC RID: 4540
		public int minPlayerPopulation;
	}
}

using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000356 RID: 854
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x060016DD RID: 5853 RVA: 0x000861B0 File Offset: 0x000843B0
		public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			if (!(hediff is Hediff_Injury))
			{
				return false;
			}
			if (hediff.Part != pawn.health.hediffSet.GetBrain())
			{
				return false;
			}
			float num = hediff.Severity / hediff.Part.def.GetMaxHealth(pawn);
			if (Rand.Value < num * this.chancePerDamagePct && base.TryApply(pawn, null))
			{
				if ((pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony) && !this.letter.NullOrEmpty())
				{
					Find.LetterStack.ReceiveLetter(this.letterLabel, this.letter.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040011AF RID: 4527
		public float chancePerDamagePct;

		// Token: 0x040011B0 RID: 4528
		public string letterLabel;

		// Token: 0x040011B1 RID: 4529
		public string letter;
	}
}

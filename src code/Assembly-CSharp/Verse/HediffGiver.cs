using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000351 RID: 849
	public abstract class HediffGiver
	{
		// Token: 0x060016C8 RID: 5832 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00085A7E File Offset: 0x00083C7E
		public virtual float ChanceFactor(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && this.hediff == HediffDefOf.Carcinoma)
			{
				return pawn.GetStatValue(StatDefOf.CancerRate, true, -1);
			}
			return 1f;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00085AA8 File Offset: 0x00083CA8
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return (this.allowOnLodgers || !pawn.IsQuestLodger()) && (this.allowOnQuestRewardPawns || !pawn.IsWorldPawn() || !pawn.IsQuestReward()) && (this.allowOnQuestReservedPawns || !pawn.IsWorldPawn() || Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.ReservedByQuest) && (this.allowOnBeggars || pawn.kindDef != PawnKindDefOf.Beggar) && (pawn.ageTracker.CurLifeStage != LifeStageDefOf.HumanlikeBaby || !Find.Storyteller.difficulty.babiesAreHealthy) && (pawn.genes == null || pawn.genes.HediffGiversCanGive(this.hediff)) && HediffGiverUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00085B7C File Offset: 0x00083D7C
		protected void SendLetter(Pawn pawn, Hediff cause)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				if (cause == null)
				{
					Find.LetterStack.ReceiveLetter("LetterHediffFromRandomHediffGiverLabel".Translate(pawn.LabelShortCap, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHediffFromRandomHediffGiver".Translate(pawn.LabelShortCap, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterHealthComplicationsLabel".Translate(pawn.LabelShort, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHealthComplications".Translate(pawn.LabelShortCap, this.hediff.LabelCap, cause.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00085CB8 File Offset: 0x00083EB8
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.hediff == null)
			{
				yield return "hediff is null";
			}
			yield break;
		}

		// Token: 0x040011A1 RID: 4513
		[TranslationHandle]
		public HediffDef hediff;

		// Token: 0x040011A2 RID: 4514
		public List<BodyPartDef> partsToAffect;

		// Token: 0x040011A3 RID: 4515
		public bool canAffectAnyLivePart;

		// Token: 0x040011A4 RID: 4516
		public bool allowOnLodgers = true;

		// Token: 0x040011A5 RID: 4517
		public bool allowOnQuestRewardPawns = true;

		// Token: 0x040011A6 RID: 4518
		public bool allowOnQuestReservedPawns = true;

		// Token: 0x040011A7 RID: 4519
		public bool allowOnBeggars = true;

		// Token: 0x040011A8 RID: 4520
		public int countToAffect = 1;
	}
}

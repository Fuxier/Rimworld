using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020000DD RID: 221
	public class RecipeWorker
	{
		// Token: 0x06000665 RID: 1637 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return true;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00022DA8 File Offset: 0x00020FA8
		public virtual AcceptanceReport AvailableReport(Thing thing, BodyPartRecord part = null)
		{
			return this.AvailableOnNow(thing, part);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00022DB7 File Offset: 0x00020FB7
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return Enumerable.Empty<BodyPartRecord>();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00022DBE File Offset: 0x00020FBE
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return (pawn.Faction != billDoerFaction || pawn.IsQuestLodger()) && this.recipe.isViolation;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00022DDE File Offset: 0x00020FDE
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00022DEB File Offset: 0x00020FEB
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CheckForWarnings(Pawn billDoer)
		{
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00022DF4 File Offset: 0x00020FF4
		public virtual float GetIngredientCount(IngredientCount ing, Bill bill)
		{
			return ing.GetBaseCount();
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00022DFC File Offset: 0x00020FFC
		public virtual TaggedString GetConfirmation(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00022E04 File Offset: 0x00021004
		protected void ReportViolation(Pawn pawn, Pawn billDoer, Faction factionToInform, int goodwillImpact, HistoryEventDef overrideEventDef = null)
		{
			if (factionToInform != null && billDoer != null && billDoer.Faction == Faction.OfPlayer)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(factionToInform, goodwillImpact, true, !factionToInform.temporary, overrideEventDef ?? HistoryEventDefOf.PerformedHarmfulSurgery, null);
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "SurgeryViolation", pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual string LabelFromUniqueIngredients(Bill bill)
		{
			return null;
		}

		// Token: 0x040004AA RID: 1194
		public RecipeDef recipe;
	}
}

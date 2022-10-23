using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200036C RID: 876
	public static class StrippableUtility
	{
		// Token: 0x060018D7 RID: 6359 RVA: 0x000955F0 File Offset: 0x000937F0
		public static bool CanBeStrippedByColony(Thing th)
		{
			IStrippable strippable = th as IStrippable;
			if (strippable == null)
			{
				return false;
			}
			if (!strippable.AnythingToStrip())
			{
				return false;
			}
			Pawn pawn = th as Pawn;
			return pawn == null || (pawn.kindDef.canStrip && !pawn.IsQuestLodger() && (pawn.Downed || (pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure)));
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0009565C File Offset: 0x0009385C
		public static void CheckSendStrippingImpactsGoodwillMessage(Thing th)
		{
			Pawn pawn;
			if ((pawn = (th as Pawn)) != null && !pawn.Dead && pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.Faction.Hidden)
			{
				Messages.Message("MessageStrippingWillAngerFaction".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}

using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002B0 RID: 688
	public class Gene_Bloodfeeder : Gene
	{
		// Token: 0x060013B0 RID: 5040 RVA: 0x00077C20 File Offset: 0x00075E20
		public override void PostAdd()
		{
			base.PostAdd();
			if (this.pawn.IsPrisonerOfColony)
			{
				Pawn_GuestTracker guest = this.pawn.guest;
				if (((guest != null) ? guest.interactionMode : null) != null && this.pawn.guest.interactionMode.hideIfNoBloodfeeders)
				{
					this.pawn.guest.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
				}
			}
		}
	}
}

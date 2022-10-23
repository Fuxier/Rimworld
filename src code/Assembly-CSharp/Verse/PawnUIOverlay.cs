using System;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020002A0 RID: 672
	public class PawnUIOverlay
	{
		// Token: 0x06001332 RID: 4914 RVA: 0x0007340C File Offset: 0x0007160C
		public PawnUIOverlay(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0007341C File Offset: 0x0007161C
		public void DrawPawnGUIOverlay()
		{
			if (!this.pawn.Spawned || this.pawn.Map.fogGrid.IsFogged(this.pawn.Position))
			{
				return;
			}
			if (!this.pawn.RaceProps.Humanlike)
			{
				if (this.pawn.RaceProps.Animal)
				{
					if (!Prefs.AnimalNameMode.ShouldDisplayAnimalName(this.pawn))
					{
						return;
					}
				}
				else
				{
					if (!this.pawn.IsColonyMech)
					{
						return;
					}
					if (this.pawn.IsSelfShutdown())
					{
						this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.SelfShutdown);
					}
					if (!Prefs.MechNameMode.ShouldDisplayMechName(this.pawn))
					{
						return;
					}
				}
			}
			Vector2 pos = GenMapUI.LabelDrawPosFor(this.pawn, -0.6f);
			GenMapUI.DrawPawnLabel(this.pawn, pos, 1f, 9999f, null, GameFont.Tiny, true, true);
			if (this.pawn.CanTradeNow)
			{
				this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.QuestionMark);
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null && lord.CurLordToil != null)
			{
				lord.CurLordToil.DrawPawnGUIOverlay(this.pawn);
			}
		}

		// Token: 0x04000FD5 RID: 4053
		private Pawn pawn;

		// Token: 0x04000FD6 RID: 4054
		private const float PawnLabelOffsetY = -0.6f;

		// Token: 0x04000FD7 RID: 4055
		private const int PawnStatBarWidth = 32;

		// Token: 0x04000FD8 RID: 4056
		private const float ActivityIconSize = 13f;

		// Token: 0x04000FD9 RID: 4057
		private const float ActivityIconOffsetY = 12f;
	}
}

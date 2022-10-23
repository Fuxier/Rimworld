using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000165 RID: 357
	public class GameComponent_OnetimeNotification : GameComponent
	{
		// Token: 0x06000993 RID: 2451 RVA: 0x0002F130 File Offset: 0x0002D330
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002F140 File Offset: 0x0002D340
		public override void GameComponentTick()
		{
			if (Find.TickManager.TicksGame % 2000 != 0 || !Rand.Chance(0.05f))
			{
				return;
			}
			if (this.sendAICoreRequestReminder)
			{
				if (ResearchProjectTagDefOf.ShipRelated.CompletedProjects() < 2)
				{
					return;
				}
				if (PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.AIPersonaCore, 1) || PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.Ship_ComputerCore, 1))
				{
					return;
				}
				Faction faction = Find.FactionManager.RandomNonHostileFaction(false, false, true, TechLevel.Undefined);
				if (faction == null || faction.leader == null)
				{
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterLabelAICoreOffer".Translate(), "LetterAICoreOffer".Translate(faction.leader.LabelDefinite(), faction.NameColored, faction.leader.Named("PAWN")).Resolve().CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, faction, null, null, null);
				this.sendAICoreRequestReminder = false;
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0002F230 File Offset: 0x0002D430
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}

		// Token: 0x040009E2 RID: 2530
		public bool sendAICoreRequestReminder = true;
	}
}

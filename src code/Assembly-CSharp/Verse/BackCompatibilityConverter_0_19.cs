using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200050D RID: 1293
	public class BackCompatibilityConverter_0_19 : BackCompatibilityConverter
	{
		// Token: 0x0600278C RID: 10124 RVA: 0x001004A8 File Offset: 0x000FE6A8
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 && minorVer <= 19;
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000029B0 File Offset: 0x00000BB0
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			return null;
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000029B0 File Offset: 0x00000BB0
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x001004B8 File Offset: 0x000FE6B8
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Game game = obj as Game;
				if (game != null && game.foodRestrictionDatabase == null)
				{
					game.foodRestrictionDatabase = new FoodRestrictionDatabase();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null && pawn.foodRestriction == null && pawn.RaceProps.Humanlike && ((pawn.Faction != null && pawn.Faction.IsPlayer) || (pawn.HostFaction != null && pawn.HostFaction.IsPlayer)))
				{
					pawn.foodRestriction = new Pawn_FoodRestrictionTracker(pawn);
				}
			}
		}
	}
}

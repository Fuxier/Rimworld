using System;
using System.Xml;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200050F RID: 1295
	public class BackCompatibilityConverter_1_2 : BackCompatibilityConverter
	{
		// Token: 0x0600279C RID: 10140 RVA: 0x0010116A File Offset: 0x000FF36A
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 || (majorVer == 1 && minorVer <= 2);
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x00101180 File Offset: 0x000FF380
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(RoyalTitleDef) && defName == "Esquire")
			{
				return "Acolyte";
			}
			if (defType == typeof(PawnKindDef) && defName == "Empire_Royal_Esquire")
			{
				return "Empire_Royal_Acolyte";
			}
			if (defType == typeof(HairDef) && defName == "ShavedFemale")
			{
				return "Shaved";
			}
			return null;
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x001011FD File Offset: 0x000FF3FD
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (providedClassName == "Hediff_ImplantWithLevel")
			{
				return typeof(Hediff_Level);
			}
			return null;
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x00101218 File Offset: 0x000FF418
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Building_Bed building_Bed;
				Pawn_GuestTracker pawn_GuestTracker;
				QuestPart_RequirementsToAcceptNoDanger questPart_RequirementsToAcceptNoDanger;
				if ((building_Bed = (obj as Building_Bed)) != null && building_Bed.CompAssignableToPawn != null)
				{
					bool flag = false;
					Scribe_Values.Look<bool>(ref flag, "forPrisoners", false, false);
					if (flag)
					{
						building_Bed.ForOwnerType = BedOwnerType.Prisoner;
						return;
					}
				}
				else if ((pawn_GuestTracker = (obj as Pawn_GuestTracker)) != null)
				{
					bool flag2 = false;
					Scribe_Values.Look<bool>(ref flag2, "prisoner", false, false);
					if (flag2)
					{
						pawn_GuestTracker.guestStatusInt = GuestStatus.Prisoner;
						return;
					}
				}
				else if ((questPart_RequirementsToAcceptNoDanger = (obj as QuestPart_RequirementsToAcceptNoDanger)) != null)
				{
					Map map = null;
					Scribe_References.Look<Map>(ref map, "map", false);
					questPart_RequirementsToAcceptNoDanger.mapParent = map.Parent;
					return;
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				World world;
				Pawn pawn;
				Faction faction;
				if ((world = (obj as World)) != null)
				{
					if (world.ideoManager == null)
					{
						world.ideoManager = new IdeoManager();
						return;
					}
				}
				else if ((pawn = (obj as Pawn)) != null)
				{
					if (Find.World.ideoManager == null)
					{
						Find.World.ideoManager = new IdeoManager();
					}
					if (pawn.ownership == null)
					{
						pawn.ownership = new Pawn_Ownership(pawn);
					}
					if (pawn.RaceProps.Humanlike)
					{
						if (pawn.ideo == null)
						{
							pawn.ideo = new Pawn_IdeoTracker(pawn);
							if (pawn.Faction != null && pawn.Faction.def.humanlikeFaction && pawn.Faction.ideos == null)
							{
								pawn.Faction.ideos = new FactionIdeosTracker(pawn.Faction);
								pawn.Faction.ideos.ChooseOrGenerateIdeo(FactionIdeosTracker.IdeoGenerationParmsForFaction_BackCompatibility(pawn.Faction.def, !ModsConfig.IdeologyActive));
							}
							if (pawn.Faction != null && pawn.Faction.ideos != null && pawn.Faction.ideos.PrimaryIdeo != null)
							{
								pawn.ideo.SetIdeo(pawn.Faction.ideos.PrimaryIdeo);
							}
							else if (Find.IdeoManager.IdeosListForReading.Any<Ideo>())
							{
								pawn.ideo.SetIdeo(Find.IdeoManager.IdeosListForReading.RandomElement<Ideo>());
							}
						}
						if (pawn.style == null)
						{
							pawn.style = new Pawn_StyleTracker(pawn);
							pawn.style.beardDef = BeardDefOf.NoBeard;
							return;
						}
					}
				}
				else if ((faction = (obj as Faction)) != null)
				{
					if (Find.World.ideoManager == null)
					{
						Find.World.ideoManager = new IdeoManager();
					}
					if (faction.def.humanlikeFaction && faction.ideos == null)
					{
						faction.ideos = new FactionIdeosTracker(faction);
						faction.ideos.ChooseOrGenerateIdeo(FactionIdeosTracker.IdeoGenerationParmsForFaction_BackCompatibility(faction.def, !ModsConfig.IdeologyActive));
					}
				}
			}
		}
	}
}

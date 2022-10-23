using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200043F RID: 1087
	public static class DebugToolsPawns
	{
		// Token: 0x0600206A RID: 8298 RVA: 0x000C2C16 File Offset: 0x000C0E16
		[DebugAction("Pawns", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = 1000)]
		private static void AddSlave()
		{
			DebugToolsPawns.AddGuest(GuestStatus.Slave);
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x000C2C1E File Offset: 0x000C0E1E
		[DebugAction("Pawns", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void AddPrisoner()
		{
			DebugToolsPawns.AddGuest(GuestStatus.Prisoner);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x000C2C26 File Offset: 0x000C0E26
		[DebugAction("Pawns", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void AddGuest()
		{
			DebugToolsPawns.AddGuest(GuestStatus.Guest);
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x000C2C30 File Offset: 0x000C0E30
		private static void AddGuest(GuestStatus guestStatus)
		{
			foreach (Building_Bed building_Bed in Find.CurrentMap.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>())
			{
				if ((!building_Bed.OwnersForReading.Any<Pawn>() || building_Bed.AnyUnownedSleepingSlot) && (guestStatus != GuestStatus.Prisoner || building_Bed.ForPrisoners) && (guestStatus != GuestStatus.Slave || building_Bed.ForSlaves))
				{
					PawnKindDef pawnKindDef;
					if (guestStatus == GuestStatus.Guest)
					{
						pawnKindDef = PawnKindDefOf.SpaceRefugee;
					}
					else
					{
						pawnKindDef = (from pk in DefDatabase<PawnKindDef>.AllDefs
						where pk.defaultFactionType != null && !pk.defaultFactionType.isPlayer && pk.RaceProps.Humanlike
						select pk).RandomElement<PawnKindDef>();
					}
					Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, faction);
					GenSpawn.Spawn(pawn, building_Bed.Position, Find.CurrentMap, WipeMode.Vanish);
					foreach (ThingWithComps eq in pawn.equipment.AllEquipmentListForReading.ToList<ThingWithComps>())
					{
						ThingWithComps thingWithComps;
						if (pawn.equipment.TryDropEquipment(eq, out thingWithComps, pawn.Position, true))
						{
							thingWithComps.Destroy(DestroyMode.Vanish);
						}
					}
					pawn.inventory.innerContainer.Clear();
					pawn.ownership.ClaimBedIfNonMedical(building_Bed);
					pawn.guest.SetGuestStatus(Faction.OfPlayer, guestStatus);
					break;
				}
			}
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000C2DD4 File Offset: 0x000C0FD4
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void DamageUntilDown(Pawn p)
		{
			HealthUtility.DamageUntilDowned(p, true);
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x000C2DDD File Offset: 0x000C0FDD
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DamageLegs(Pawn p)
		{
			HealthUtility.DamageLegsUntilIncapableOfMoving(p, true);
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x000C2DE6 File Offset: 0x000C0FE6
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DamageUntilIncapableOfManipulation(Pawn p)
		{
			HealthUtility.DamageLimbsUntilIncapableOfManipulation(p, true);
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x000C2DEF File Offset: 0x000C0FEF
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p);
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000C2DF7 File Offset: 0x000C0FF7
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void CarriedDamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p.carryTracker.CarriedThing as Pawn);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000C2E10 File Offset: 0x000C1010
		[DebugAction("Pawns", "10 damage until dead", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void Do10DamageUntilDead()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				for (int i = 0; i < 1000; i++)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
					dinfo.SetIgnoreInstantKillProtection(true);
					thing.TakeDamage(dinfo);
					if (thing.Destroyed)
					{
						string str = "Took " + (i + 1) + " hits";
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
							{
								str = str + " (reached lethal damage threshold of " + pawn.health.LethalDamageThreshold.ToString("0.#") + ")";
							}
							else if (PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, pawn.RaceProps.body.corePart, false, null) <= 0.0001f)
							{
								str += " (core part hp reached 0)";
							}
							else
							{
								PawnCapacityDef pawnCapacityDef = pawn.health.ShouldBeDeadFromRequiredCapacity();
								if (pawnCapacityDef != null)
								{
									str = str + " (incapable of " + pawnCapacityDef.defName + ")";
								}
							}
						}
						Log.Message(str + ".");
						break;
					}
				}
			}
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000C2FB4 File Offset: 0x000C11B4
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DamageHeldPawnToDeath()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing is Pawn)
				{
					HealthUtility.DamageUntilDead((Pawn)pawn.carryTracker.CarriedThing);
				}
			}
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x000C3050 File Offset: 0x000C1250
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RestoreBodyPart(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p)));
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000C3068 File Offset: 0x000C1268
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetHeadType(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (List<HeadTypeDef>.Enumerator enumerator = DefDatabase<HeadTypeDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HeadTypeDef headTypeDef = enumerator.Current;
					list.Add(new FloatMenuOption(headTypeDef.defName, delegate()
					{
						p.story.headType = headTypeDef;
						p.Drawer.renderer.graphics.headGraphic = headTypeDef.GetGraphic(p.story.SkinColor, false, p.story.SkinColorOverriden);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list, p.LabelShort, false));
			}
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000C312C File Offset: 0x000C132C
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetBodyType(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (List<BodyTypeDef>.Enumerator enumerator = DefDatabase<BodyTypeDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BodyTypeDef bodyTypeDef = enumerator.Current;
					list.Add(new FloatMenuOption(bodyTypeDef.defName, delegate()
					{
						p.story.bodyType = bodyTypeDef;
						p.Drawer.renderer.graphics.ResolveAllGraphics();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list, p.LabelShort, false));
			}
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x000C31F0 File Offset: 0x000C13F0
		[DebugAction("Pawns", "Apply damage", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> ApplyDamage()
		{
			return DebugTools_Health.Options_ApplyDamage();
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000C31F8 File Offset: 0x000C13F8
		[DebugAction("Pawns", "Heal random injury (10)", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void HealRandomInjury10(Pawn p)
		{
			List<Hediff_Injury> source = new List<Hediff_Injury>();
			p.health.hediffSet.GetHediffs<Hediff_Injury>(ref source, (Hediff_Injury x) => x.CanHealNaturally() || x.CanHealFromTending());
			Hediff_Injury hediff_Injury;
			if (source.TryRandomElement(out hediff_Injury))
			{
				hediff_Injury.Heal(10f);
			}
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000C3254 File Offset: 0x000C1454
		[DebugAction("Pawns", "Make injuries permanent", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeInjuryPermanent(Pawn p)
		{
			foreach (Hediff hd in p.health.hediffSet.hediffs)
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = hd.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null)
				{
					hediffComp_GetsPermanent.IsPermanent = true;
				}
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000C32BC File Offset: 0x000C14BC
		[DebugAction("Pawns", "Toggle immunity", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleImmunity(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				Hediff hediff2 = hediff;
				ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff2.def);
				if (immunityRecord != null)
				{
					Texture2D itemIcon = (immunityRecord.immunity < 1f) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
					list.Add(new FloatMenuOption(hediff2.LabelCap, delegate()
					{
						if (immunityRecord.immunity < 1f)
						{
							immunityRecord.immunity = 1f;
							return;
						}
						immunityRecord.immunity = 0f;
					}, itemIcon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list, p.LabelShort, false));
			}
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000C33BC File Offset: 0x000C15BC
		[DebugAction("Pawns", "Activate HediffGiver", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ActivateHediffGiver(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (p.RaceProps.hediffGiverSets != null)
			{
				foreach (HediffGiver localHdg2 in p.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef set) => set.hediffGivers))
				{
					HediffGiver localHdg = localHdg2;
					list.Add(new FloatMenuOption(localHdg.hediff.defName, delegate()
					{
						if (localHdg.TryApply(p, null))
						{
							Messages.Message(localHdg.hediff.defName + " applied to " + p.Label, MessageTypeDefOf.NeutralEvent, false);
							return;
						}
						Messages.Message("failed to apply " + localHdg.hediff.defName + " to " + p.Label, MessageTypeDefOf.NegativeEvent, false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list));
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000C34C8 File Offset: 0x000C16C8
		[DebugAction("Pawns", "Activate HediffGiver World Pawn", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ActivateHediffGiverWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawnLocal2 in from p in Find.WorldPawns.AllPawnsAlive
			where p.RaceProps.Humanlike
			select p)
			{
				Pawn pawnLocal = pawnLocal2;
				list.Add(new DebugMenuOption(pawnLocal.Label, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (HediffGiver hediffGiverLocal2 in pawnLocal.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef s) => s.hediffGivers))
					{
						HediffGiver hediffGiverLocal = hediffGiverLocal2;
						list2.Add(new DebugMenuOption(hediffGiverLocal.hediff.defName, DebugMenuOptionMode.Action, delegate()
						{
							if (hediffGiverLocal.TryApply(pawnLocal, null))
							{
								Messages.Message(hediffGiverLocal.hediff.defName + " applied to " + pawnLocal.Label, MessageTypeDefOf.NeutralEvent, false);
								return;
							}
							Messages.Message("failed to apply " + hediffGiverLocal.hediff.defName + " to " + pawnLocal.Label, MessageTypeDefOf.NegativeEvent, false);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000C357C File Offset: 0x000C177C
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DiscoverHediffs(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				if (!hediff.Visible)
				{
					hediff.Severity = Mathf.Max(hediff.Severity, hediff.def.stages.First((HediffStage s) => s.becomeVisible).minSeverity);
				}
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000C3620 File Offset: 0x000C1820
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrantImmunities(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff.def);
				if (immunityRecord != null)
				{
					immunityRecord.immunity = 1f;
				}
			}
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x000C369C File Offset: 0x000C189C
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void GiveBirth(Pawn p)
		{
			Hediff_Pregnant.DoBirthSpawn(p, null);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000C36AC File Offset: 0x000C18AC
		[DebugAction("Pawns", "Resistance -1", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ResistanceMinus1(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 1f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x000C3700 File Offset: 0x000C1900
		[DebugAction("Pawns", "Resistance -10", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ResistanceMinus10(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 10f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000C3754 File Offset: 0x000C1954
		[DebugAction("Pawns", "Add/remove pawn relation", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddRemovePawnRelation(Pawn p)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return;
			}
			Func<Pawn, bool> <>9__5;
			Action<bool> act = delegate(bool add)
			{
				if (add)
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (PawnRelationDef pawnRelationDef in DefDatabase<PawnRelationDef>.AllDefs)
					{
						if (!pawnRelationDef.implied)
						{
							PawnRelationDef defLocal = pawnRelationDef;
							list2.Add(new DebugMenuOption(defLocal.defName, DebugMenuOptionMode.Action, delegate()
							{
								List<DebugMenuOption> list4 = new List<DebugMenuOption>();
								IEnumerable<Pawn> source = from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
								where x.RaceProps.IsFlesh || (x.RaceProps.IsMechanoid && x.Faction == Faction.OfPlayer)
								select x;
								Func<Pawn, bool> keySelector;
								if ((keySelector = <>9__5) == null)
								{
									keySelector = (<>9__5 = ((Pawn x) => x.def == p.def));
								}
								foreach (Pawn pawn in source.OrderByDescending(keySelector).ThenBy((Pawn x) => x.IsWorldPawn()))
								{
									if (p != pawn && (!defLocal.familyByBloodRelation || pawn.def == p.def) && !p.relations.DirectRelationExists(defLocal, pawn))
									{
										Pawn otherLocal = pawn;
										list4.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
										{
											p.relations.AddDirectRelation(defLocal, otherLocal);
											if (defLocal == PawnRelationDefOf.Fiance)
											{
												otherLocal.relations.nextMarriageNameChange = (p.relations.nextMarriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage(p));
											}
										}));
									}
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					return;
				}
				List<DebugMenuOption> list3 = new List<DebugMenuOption>();
				List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					DirectPawnRelation rel = directRelations[i];
					list3.Add(new DebugMenuOption(rel.def.defName + " - " + rel.otherPawn.LabelShort, DebugMenuOptionMode.Action, delegate()
					{
						p.relations.RemoveDirectRelation(rel);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Add", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			if (!p.relations.DirectRelations.NullOrEmpty<DirectPawnRelation>())
			{
				list.Add(new DebugMenuOption("Remove", DebugMenuOptionMode.Action, delegate()
				{
					act(false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000C37FC File Offset: 0x000C19FC
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void AddOpinionTalksAbout(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			Action<bool> act = delegate(bool good)
			{
				Func<ThoughtDef, bool> <>9__4;
				foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Humanlike
				select x)
				{
					if (p != pawn)
					{
						IEnumerable<ThoughtDef> allDefs = DefDatabase<ThoughtDef>.AllDefs;
						Func<ThoughtDef, bool> predicate;
						if ((predicate = <>9__4) == null)
						{
							predicate = (<>9__4 = ((ThoughtDef x) => typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((good && x.stages[0].baseOpinionOffset > 0f) || (!good && x.stages[0].baseOpinionOffset < 0f))));
						}
						IEnumerable<ThoughtDef> source = allDefs.Where(predicate);
						if (source.Any<ThoughtDef>())
						{
							int num = Rand.Range(2, 5);
							for (int i = 0; i < num; i++)
							{
								ThoughtDef def = source.RandomElement<ThoughtDef>();
								pawn.needs.mood.thoughts.memories.TryGainMemory(def, p, null);
							}
						}
					}
				}
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Good", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			list.Add(new DebugMenuOption("Bad", DebugMenuOptionMode.Action, delegate()
			{
				act(false);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x000C388C File Offset: 0x000C1A8C
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SetSkill()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
				for (int i = 0; i <= 20; i++)
				{
					int level = i;
					debugActionNode.AddChild(new DebugActionNode(level.ToString(), DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							if (p.skills != null)
							{
								SkillRecord skill = p.skills.GetSkill(localDef);
								skill.Level = level;
								skill.xpSinceLastLevel = skill.XpRequiredForLevelUp / 2f;
								DebugActionsUtility.DustPuffFrom(p);
							}
						}
					});
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000C3960 File Offset: 0x000C1B60
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MaxSkill()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Pawn_SkillTracker skills = p.skills;
					if (skills == null)
					{
						return;
					}
					skills.Learn(localDef, 100000000f, false);
				}));
			}
			return list;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x000C39E0 File Offset: 0x000C1BE0
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MaxAllSkills(Pawn p)
		{
			if (p.skills != null)
			{
				foreach (SkillDef sDef in DefDatabase<SkillDef>.AllDefs)
				{
					p.skills.Learn(sDef, 100000000f, false);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
			if (p.training != null)
			{
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					Pawn trainer = p.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
					bool flag;
					if (p.training.CanAssignToTrain(td, out flag).Accepted)
					{
						p.training.Train(td, trainer, false);
					}
				}
			}
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000C3AC0 File Offset: 0x000C1CC0
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SetPassion()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
				using (IEnumerator enumerator2 = Enum.GetValues(typeof(Passion)).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object passion = enumerator2.Current;
						debugActionNode.AddChild(new DebugActionNode(passion.ToString(), DebugActionType.ToolMapForPawns, null, null)
						{
							pawnAction = delegate(Pawn p)
							{
								if (p.skills != null)
								{
									p.skills.GetSkill(localDef).passion = (Passion)passion;
									DebugActionsUtility.DustPuffFrom(p);
								}
							}
						});
					}
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000C3BCC File Offset: 0x000C1DCC
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MaxPassion()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					if (p.skills == null)
					{
						return;
					}
					p.skills.GetSkill(localDef).passion = Passion.Major;
				}));
			}
			return list;
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x000C3C4C File Offset: 0x000C1E4C
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MaxAllPassions(Pawn p)
		{
			if (p.skills != null)
			{
				foreach (SkillRecord skillRecord in p.skills.skills)
				{
					skillRecord.passion = Passion.Major;
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x000C3CB0 File Offset: 0x000C1EB0
		[DebugAction("Pawns", "Mental break", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MentalBreak()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("(log possibles)", DebugActionType.ToolMapForPawns, null, null);
			debugActionNode.pawnAction = delegate(Pawn p)
			{
				p.mindState.mentalBreaker.LogPossibleMentalBreaks();
				DebugActionsUtility.DustPuffFrom(p);
			};
			list2.Add(debugActionNode);
			List<DebugActionNode> list3 = list;
			DebugActionNode debugActionNode2 = new DebugActionNode("(natural mood break)", DebugActionType.ToolMapForPawns, null, null);
			debugActionNode2.pawnAction = delegate(Pawn p)
			{
				p.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
				DebugActionsUtility.DustPuffFrom(p);
			};
			list3.Add(debugActionNode2);
			foreach (MentalBreakDef locBrDef2 in from x in DefDatabase<MentalBreakDef>.AllDefs
			orderby x.intensity descending
			select x)
			{
				MentalBreakDef locBrDef = locBrDef2;
				Predicate<Pawn> <>9__5;
				list.Add(new DebugActionNode(locBrDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						locBrDef.Worker.TryStart(p, null, false);
						DebugActionsUtility.DustPuffFrom(p);
					},
					labelGetter = delegate()
					{
						string text = locBrDef.defName;
						List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
						Predicate<Pawn> predicate;
						if ((predicate = <>9__5) == null)
						{
							predicate = (<>9__5 = ((Pawn x) => locBrDef.Worker.BreakCanOccur(x)));
						}
						if (!freeColonists.Any(predicate))
						{
							text += " [NO]";
						}
						return text;
					}
				});
			}
			return list;
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000C3DDC File Offset: 0x000C1FDC
		[DebugAction("Pawns", "Mental state...", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> MentalState()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MentalStateDef locBrDef2 in DefDatabase<MentalStateDef>.AllDefs)
			{
				MentalStateDef locBrDef = locBrDef2;
				Predicate<Pawn> <>9__4;
				list.Add(new DebugActionNode(locBrDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						if (locBrDef != MentalStateDefOf.SocialFighting)
						{
							p.mindState.mentalStateHandler.TryStartMentalState(locBrDef, null, true, false, null, false, false, false);
							DebugActionsUtility.DustPuffFrom(p);
							return;
						}
						DebugTools.curTool = new DebugTool("...with", delegate()
						{
							Pawn pawn = (Pawn)(from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
							where t is Pawn
							select t).FirstOrDefault<Thing>();
							if (pawn != null)
							{
								p.interactions.StartSocialFight(pawn, "MessageSocialFight");
								DebugTools.curTool = null;
							}
						}, null);
					},
					labelGetter = delegate()
					{
						string text = locBrDef.defName;
						if (Find.CurrentMap == null)
						{
							return text;
						}
						List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
						Predicate<Pawn> predicate;
						if ((predicate = <>9__4) == null)
						{
							predicate = (<>9__4 = ((Pawn x) => locBrDef.Worker.StateCanOccur(x)));
						}
						if (!freeColonists.Any(predicate))
						{
							text += " [NO]";
						}
						return text;
					}
				});
			}
			return list;
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000C3E74 File Offset: 0x000C2074
		[DebugAction("Pawns", "Stop mental state", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StopMentalState(Pawn p)
		{
			if (p.InMentalState)
			{
				p.MentalState.RecoverFromState();
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000C3E98 File Offset: 0x000C2098
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> Inspiration()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (InspirationDef localDef2 in DefDatabase<InspirationDef>.AllDefs)
			{
				InspirationDef localDef = localDef2;
				Predicate<Pawn> <>9__2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						InspirationHandler inspirationHandler = p.mindState.inspirationHandler;
						if (inspirationHandler != null)
						{
							inspirationHandler.TryStartInspiration(localDef, "Debug gain", true);
						}
						DebugActionsUtility.DustPuffFrom(p);
					},
					labelGetter = delegate()
					{
						string text = localDef.defName;
						List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
						Predicate<Pawn> predicate;
						if ((predicate = <>9__2) == null)
						{
							predicate = (<>9__2 = ((Pawn x) => localDef.Worker.InspirationCanOccur(x)));
						}
						if (!freeColonists.Any(predicate))
						{
							text += " [NO]";
						}
						return text;
					}
				});
			}
			return list;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000C3F30 File Offset: 0x000C2130
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> GiveTrait()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (TraitDef traitDef in DefDatabase<TraitDef>.AllDefs)
			{
				TraitDef trDef = traitDef;
				for (int j = 0; j < traitDef.degreeDatas.Count; j++)
				{
					int i = j;
					list.Add(new DebugActionNode(string.Concat(new object[]
					{
						trDef.degreeDatas[i].label,
						" (",
						trDef.degreeDatas[j].degree,
						")"
					}), DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							if (p.story != null)
							{
								p.story.traits.GainTrait(new Trait(trDef, trDef.degreeDatas[i].degree, false), true);
								DebugActionsUtility.DustPuffFrom(p);
							}
						}
					});
				}
			}
			return list;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000C4050 File Offset: 0x000C2250
		[DebugAction("Pawns", "Remove all traits", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveAllTraits(Pawn p)
		{
			if (p.story != null)
			{
				for (int i = p.story.traits.allTraits.Count - 1; i >= 0; i--)
				{
					p.story.traits.RemoveTrait(p.story.traits.allTraits[i], false);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000C40B4 File Offset: 0x000C22B4
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static List<DebugActionNode> SetBackstory()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			DebugActionNode debugActionNode = new DebugActionNode("Adulthood", DebugActionType.Action, null, null);
			foreach (DebugActionNode child in DebugToolsPawns.<SetBackstory>g__BackstoryOptionNodes|39_0(BackstorySlot.Adulthood))
			{
				debugActionNode.AddChild(child);
			}
			list.Add(debugActionNode);
			DebugActionNode debugActionNode2 = new DebugActionNode("Childhood", DebugActionType.Action, null, null);
			foreach (DebugActionNode child2 in DebugToolsPawns.<SetBackstory>g__BackstoryOptionNodes|39_0(BackstorySlot.Childhood))
			{
				debugActionNode2.AddChild(child2);
			}
			list.Add(debugActionNode2);
			return list;
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x000C4180 File Offset: 0x000C2380
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> GiveAbility()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*All", DebugActionType.ToolMapForPawns, null, null);
			debugActionNode.pawnAction = delegate(Pawn p)
			{
				if (p.abilities != null)
				{
					foreach (AbilityDef def in DefDatabase<AbilityDef>.AllDefs)
					{
						p.abilities.GainAbility(def);
					}
				}
			};
			list2.Add(debugActionNode);
			foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefs)
			{
				AbilityDef localAb = abilityDef;
				list.Add(new DebugActionNode(abilityDef.label, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Pawn_AbilityTracker abilities = p.abilities;
					if (abilities == null)
					{
						return;
					}
					abilities.GainAbility(localAb);
				}));
			}
			return list;
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000C4230 File Offset: 0x000C2430
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresRoyalty = true)]
		private static List<DebugActionNode> GivePsylink()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 1; i <= (int)HediffDefOf.PsychicAmplifier.maxSeverity; i++)
			{
				int level = i;
				list.Add(new DebugActionNode("Level " + i, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						Hediff_Level hediff_Level = p.GetMainPsylinkSource();
						if (hediff_Level == null)
						{
							hediff_Level = (HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, p, p.health.hediffSet.GetBrain()) as Hediff_Level);
							p.health.AddHediff(hediff_Level, null, null, null);
						}
						hediff_Level.ChangeLevel(level - hediff_Level.level);
					}
				});
			}
			return list;
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000C4297 File Offset: 0x000C2497
		[DebugAction("Pawns", "Give good thought", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void GiveGoodThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null, null);
			}
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000C42C7 File Offset: 0x000C24C7
		[DebugAction("Pawns", "Give bad thought", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void GiveBadThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null, null);
			}
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000C42F8 File Offset: 0x000C24F8
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ClearBoundUnfinishedThings()
		{
			foreach (Building_WorkTable building_WorkTable in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Building_WorkTable
			select t).Cast<Building_WorkTable>())
			{
				foreach (Bill bill in building_WorkTable.BillStack)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null)
					{
						bill_ProductionWithUft.ClearBoundUft();
					}
				}
			}
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000C43B8 File Offset: 0x000C25B8
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceBirthday(Pawn p)
		{
			p.ageTracker.AgeBiologicalTicks = (long)((p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1);
			p.ageTracker.DebugForceBirthdayBiological();
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x000C43E8 File Offset: 0x000C25E8
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void Recruit(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer)
			{
				if (p.RaceProps.Humanlike)
				{
					InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p, true);
					DebugActionsUtility.DustPuffFrom(p);
					return;
				}
				if (p.RaceProps.IsMechanoid)
				{
					p.SetFaction(Faction.OfPlayer, null);
					DebugActionsUtility.DustPuffFrom(p);
				}
			}
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x000C4451 File Offset: 0x000C2651
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = 1000)]
		private static void Enslave(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
			{
				GenGuest.EnslavePrisoner(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x000C4490 File Offset: 0x000C2690
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleRecruitable(Pawn p)
		{
			if (p.guest != null)
			{
				p.guest.Recruitable = !p.guest.Recruitable;
				DebugActionsUtility.DustPuffFrom(p);
				MoteMaker.ThrowText(p.DrawPos, p.MapHeld, "Recruitable:\n" + p.guest.Recruitable.ToStringYesNo(), -1f);
			}
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000C44F4 File Offset: 0x000C26F4
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrowPawnToMaturity()
		{
			Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
			if (firstPawn != null)
			{
				firstPawn.ageTracker.AgeBiologicalTicks += (long)Mathf.FloorToInt(firstPawn.ageTracker.AdultMinAge * 3600000f);
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000C4540 File Offset: 0x000C2740
		[DebugAction("Pawns", "Wear apparel (selected)", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> WearApparel_ToSelected()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*Remove all apparel", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				using (List<object>.Enumerator enumerator2 = Find.Selector.SelectedObjectsListForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Pawn pawn;
						if ((pawn = (enumerator2.Current as Pawn)) != null)
						{
							Pawn_ApparelTracker apparel = pawn.apparel;
							if (apparel != null)
							{
								apparel.DestroyAll(DestroyMode.Vanish);
							}
						}
					}
				}
			};
			list2.Add(debugActionNode);
			foreach (ThingDef localDef2 in from d in DefDatabase<ThingDef>.AllDefs
			where d.IsApparel
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						using (List<object>.Enumerator enumerator2 = Find.Selector.SelectedObjectsListForReading.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Pawn pawn;
								if ((pawn = (enumerator2.Current as Pawn)) != null && pawn.apparel != null)
								{
									ThingDef stuff = GenStuff.RandomStuffFor(localDef);
									Apparel newApparel = (Apparel)ThingMaker.MakeThing(localDef, stuff);
									pawn.apparel.Wear(newApparel, false, false);
								}
							}
						}
					}
				});
			}
			return list;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000C4644 File Offset: 0x000C2844
		[DebugAction("Pawns", "Equip primary (selected)...", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> EquipPrimary_ToSelected()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*Remove primary", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				using (List<object>.Enumerator enumerator2 = Find.Selector.SelectedObjectsListForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Pawn pawn;
						if ((pawn = (enumerator2.Current as Pawn)) != null)
						{
							Pawn_EquipmentTracker equipment = pawn.equipment;
							if (((equipment != null) ? equipment.Primary : null) != null)
							{
								pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
							}
						}
					}
				}
			};
			list2.Add(debugActionNode);
			using (IEnumerator<ThingDef> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.equipmentType == EquipmentType.Primary
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					ThingDef def2 = def;
					list.Add(new DebugActionNode(def2.defName, DebugActionType.Action, null, null)
					{
						action = delegate()
						{
							using (List<object>.Enumerator enumerator2 = Find.Selector.SelectedObjectsListForReading.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									Pawn pawn;
									if ((pawn = (enumerator2.Current as Pawn)) != null && pawn.equipment != null)
									{
										if (pawn.equipment.Primary != null)
										{
											pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
										}
										ThingDef stuff = GenStuff.RandomStuffFor(def);
										ThingWithComps newEq = (ThingWithComps)ThingMaker.MakeThing(def, stuff);
										pawn.equipment.AddEquipment(newEq);
									}
								}
							}
						}
					});
				}
			}
			return list;
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000C4748 File Offset: 0x000C2948
		public static List<FloatMenuOption> PawnGearDevOptions(Pawn pawn)
		{
			return new List<FloatMenuOption>
			{
				new FloatMenuOption("Set primary", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_SetPrimary(pawn)));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				new FloatMenuOption("Wear", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_Wear(pawn)));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				new FloatMenuOption("Add to inventory", delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_GiveToInventory(pawn)));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				new FloatMenuOption("Damage random apparel", delegate()
				{
					pawn.apparel.WornApparel.RandomElement<Apparel>().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			};
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x000C4808 File Offset: 0x000C2A08
		private static List<DebugMenuOption> Options_Wear(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Remove all apparel", DebugMenuOptionMode.Action, delegate()
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}));
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			Func<ThingDef, bool> <>9__1;
			Func<ThingDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((ThingDef def) => def.IsApparel && def.apparel.developmentalStageFilter.Has(pawn.DevelopmentalStage)));
			}
			using (IEnumerator<ThingDef> enumerator = (from d in allDefs.Where(predicate)
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						ThingDef stuff = GenStuff.RandomStuffFor(def);
						Apparel newApparel = (Apparel)ThingMaker.MakeThing(def, stuff);
						pawn.apparel.Wear(newApparel, false, false);
					}));
				}
			}
			return list;
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x000C4900 File Offset: 0x000C2B00
		private static List<DebugMenuOption> Options_SetPrimary(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Remove primary", DebugMenuOptionMode.Action, delegate()
			{
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
				}
			}));
			using (IEnumerator<ThingDef> enumerator = (from def in DefDatabase<ThingDef>.AllDefs
			where def.equipmentType == EquipmentType.Primary
			select def into d
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						if (pawn.equipment.Primary != null)
						{
							pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
						}
						ThingDef stuff = GenStuff.RandomStuffFor(def);
						ThingWithComps newEq = (ThingWithComps)ThingMaker.MakeThing(def, stuff);
						pawn.equipment.AddEquipment(newEq);
					}));
				}
			}
			return list;
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x000C49F4 File Offset: 0x000C2BF4
		private static List<DebugMenuOption> Options_GiveToInventory(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Clear all", DebugMenuOptionMode.Action, delegate()
			{
				pawn.inventory.DestroyAll(DestroyMode.Vanish);
			}));
			using (IEnumerator<ThingDef> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item
			orderby d.defName
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.label, DebugMenuOptionMode.Action, delegate()
					{
						pawn.inventory.TryAddItemNotForSale(ThingMaker.MakeThing(def, null));
					}));
				}
			}
			return list;
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x000C4AE8 File Offset: 0x000C2CE8
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void TameAnimal(Pawn p)
		{
			if (p.AnimalOrWildMan() && p.Faction != Faction.OfPlayer)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault<Pawn>(), p, true);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000C4B24 File Offset: 0x000C2D24
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TrainAnimal(Pawn p)
		{
			if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				DebugActionsUtility.DustPuffFrom(p);
				bool flag = false;
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					if (p.training.GetWanted(td))
					{
						p.training.Train(td, null, true);
						flag = true;
					}
				}
				if (!flag)
				{
					foreach (TrainableDef td2 in DefDatabase<TrainableDef>.AllDefs)
					{
						if (p.training.CanAssignToTrain(td2).Accepted)
						{
							p.training.Train(td2, null, true);
						}
					}
				}
			}
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000C4C18 File Offset: 0x000C2E18
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryDevelopBoundRelation(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			Pawn humanlike;
			if (p.RaceProps.Humanlike)
			{
				Pawn animal;
				if ((from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Animal && x.Faction == p.Faction
				select x).TryRandomElement(out animal))
				{
					RelationsUtility.TryDevelopBondRelation(p, animal, 999999f);
					return;
				}
			}
			else if (p.RaceProps.Animal && (from x in p.Map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike && x.Faction == p.Faction
			select x).TryRandomElement(out humanlike))
			{
				RelationsUtility.TryDevelopBondRelation(humanlike, p, 999999f);
			}
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x000C4CE7 File Offset: 0x000C2EE7
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void QueueTrainingDecay(Pawn p)
		{
			if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				p.training.Debug_MakeDegradeHappenSoon();
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x000C4D1C File Offset: 0x000C2F1C
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DisplayRelationsInfo(Pawn pawn)
		{
			List<TableDataGetter<Pawn>> list = new List<TableDataGetter<Pawn>>();
			list.Add(new TableDataGetter<Pawn>("name", (Pawn p) => p.LabelCap));
			list.Add(new TableDataGetter<Pawn>("kind label", (Pawn p) => p.KindLabel));
			list.Add(new TableDataGetter<Pawn>("gender", (Pawn p) => p.gender.GetLabel(false)));
			list.Add(new TableDataGetter<Pawn>("age", (Pawn p) => p.ageTracker.AgeBiologicalYears));
			list.Add(new TableDataGetter<Pawn>("my compat", (Pawn p) => pawn.relations.CompatibilityWith(p).ToString("F2")));
			list.Add(new TableDataGetter<Pawn>("their compat", (Pawn p) => p.relations.CompatibilityWith(pawn).ToString("F2")));
			list.Add(new TableDataGetter<Pawn>("my 2nd\nrom chance", (Pawn p) => pawn.relations.SecondaryRomanceChanceFactor(p).ToStringPercent("F0")));
			list.Add(new TableDataGetter<Pawn>("their 2nd\nrom chance", (Pawn p) => p.relations.SecondaryRomanceChanceFactor(pawn).ToStringPercent("F0")));
			list.Add(new TableDataGetter<Pawn>("lovin mtb", (Pawn p) => LovePartnerRelationUtility.GetLovinMtbHours(pawn, p).ToString("F1") + " h"));
			List<TableDataGetter<Pawn>> list2 = list;
			DebugTables.MakeTablesDialog<Pawn>(from x in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
			where x != pawn && x.RaceProps.Humanlike
			select x, list2.ToArray());
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000C4EC0 File Offset: 0x000C30C0
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DisplayInteractionsInfo(Pawn pawn)
		{
			DebugToolsPawns.<>c__DisplayClass61_0 CS$<>8__locals1 = new DebugToolsPawns.<>c__DisplayClass61_0();
			CS$<>8__locals1.pawn = pawn;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<Pawn> source = CS$<>8__locals1.pawn.Map.mapPawns.SpawnedPawnsInFaction(CS$<>8__locals1.pawn.Faction);
			Func<Pawn, bool> predicate;
			if ((predicate = CS$<>8__locals1.<>9__0) == null)
			{
				predicate = (CS$<>8__locals1.<>9__0 = ((Pawn x) => x != CS$<>8__locals1.pawn && x.RaceProps.Humanlike));
			}
			using (IEnumerator<Pawn> enumerator = source.Where(predicate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugToolsPawns.<>c__DisplayClass61_1 CS$<>8__locals2 = new DebugToolsPawns.<>c__DisplayClass61_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.p = enumerator.Current;
					float totalWeight = DefDatabase<InteractionDef>.AllDefs.Sum((InteractionDef x) => x.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p));
					Func<InteractionDef, string> <>9__5;
					list.Add(new DebugMenuOption(CS$<>8__locals2.p.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						List<TableDataGetter<InteractionDef>> list2 = new List<TableDataGetter<InteractionDef>>();
						list2.Add(new TableDataGetter<InteractionDef>("defName", (InteractionDef i) => i.defName));
						string label = "sel weight";
						Func<InteractionDef, float> getter;
						if ((getter = CS$<>8__locals2.<>9__4) == null)
						{
							getter = (CS$<>8__locals2.<>9__4 = ((InteractionDef i) => i.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p)));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label, getter));
						string label2 = "sel chance";
						Func<InteractionDef, string> getter2;
						if ((getter2 = <>9__5) == null)
						{
							getter2 = (<>9__5 = ((InteractionDef i) => (i.Worker.RandomSelectionWeight(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p) / totalWeight).ToStringPercent()));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label2, getter2));
						string label3 = "fight\nchance";
						Func<InteractionDef, string> getter3;
						if ((getter3 = CS$<>8__locals2.<>9__6) == null)
						{
							getter3 = (CS$<>8__locals2.<>9__6 = ((InteractionDef i) => CS$<>8__locals2.p.interactions.SocialFightChance(i, CS$<>8__locals2.CS$<>8__locals1.pawn).ToStringPercent()));
						}
						list2.Add(new TableDataGetter<InteractionDef>(label3, getter3));
						string label4 = "success\nchance";
						Func<InteractionDef, string> getter4;
						if ((getter4 = CS$<>8__locals2.<>9__7) == null)
						{
							getter4 = (CS$<>8__locals2.<>9__7 = delegate(InteractionDef i)
							{
								if (i == InteractionDefOf.RomanceAttempt)
								{
									return InteractionWorker_RomanceAttempt.SuccessChance(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p, 0.6f).ToStringPercent();
								}
								if (i == InteractionDefOf.MarriageProposal)
								{
									return ((InteractionWorker_MarriageProposal)i.Worker).AcceptanceChance(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.p).ToStringPercent();
								}
								return "";
							});
						}
						list2.Add(new TableDataGetter<InteractionDef>(label4, getter4));
						List<TableDataGetter<InteractionDef>> list3 = list2;
						DebugTables.MakeTablesDialog<InteractionDef>(DefDatabase<InteractionDef>.AllDefs, list3.ToArray());
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000C4FDC File Offset: 0x000C31DC
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void StartMarriageCeremony(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike
			select x)
			{
				if (p != pawn)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
					{
						if (!p.relations.DirectRelationExists(PawnRelationDefOf.Fiance, otherLocal))
						{
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, otherLocal);
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Spouse, otherLocal);
							p.relations.AddDirectRelation(PawnRelationDefOf.Fiance, otherLocal);
							Messages.Message("DEV: Auto added fiance relation.", p, MessageTypeDefOf.TaskCompletion, false);
						}
						if (!p.Map.lordsStarter.TryStartMarriageCeremony(p, otherLocal))
						{
							Messages.Message("Could not find any valid marriage site.", MessageTypeDefOf.RejectInput, false);
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000C50F4 File Offset: 0x000C32F4
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceInteraction(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction))
			{
				if (pawn != p)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (InteractionDef interactionLocal2 in DefDatabase<InteractionDef>.AllDefsListForReading)
						{
							InteractionDef interactionLocal = interactionLocal2;
							list2.Add(new DebugMenuOption(interactionLocal.label, DebugMenuOptionMode.Action, delegate()
							{
								p.interactions.TryInteractWith(otherLocal, interactionLocal);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000C51F4 File Offset: 0x000C33F4
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> StartGathering()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			List<DebugActionNode> list2 = list;
			DebugActionNode debugActionNode = new DebugActionNode("*Random", DebugActionType.Action, null, null);
			debugActionNode.action = delegate()
			{
				if (!Find.CurrentMap.lordsStarter.TryStartRandomGathering(true))
				{
					Messages.Message("Could not find any valid gathering spot or organizer.", MessageTypeDefOf.RejectInput, false);
				}
			};
			list2.Add(debugActionNode);
			foreach (GatheringDef gatheringDef2 in DefDatabase<GatheringDef>.AllDefsListForReading)
			{
				GatheringDef gatheringDef = gatheringDef2;
				list.Add(new DebugActionNode(gatheringDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						gatheringDef.Worker.TryExecute(Find.CurrentMap, null);
					},
					labelGetter = (() => gatheringDef.LabelCap + " (" + (gatheringDef.Worker.CanExecute(Find.CurrentMap, null) ? "Yes" : "No") + ")")
				});
			}
			return list;
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000C52C8 File Offset: 0x000C34C8
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void StartPrisonBreak(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return;
			}
			PrisonBreakUtility.StartPrisonBreak(p);
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x000C52D9 File Offset: 0x000C34D9
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void PassToWorld(Pawn p)
		{
			p.DeSpawn(DestroyMode.Vanish);
			Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x000C52F0 File Offset: 0x000C34F0
		[DebugAction("Spawning", "Remove world pawn...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void RemoveWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAliveOrDead)
			{
				Pawn pLocal = pawn;
				string text = pawn.LabelShort;
				WorldPawnSituation situation = Find.WorldPawns.GetSituation(pawn);
				if (situation != WorldPawnSituation.Free)
				{
					text = string.Concat(new object[]
					{
						text,
						" [",
						situation,
						"]"
					});
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
				{
					Find.WorldPawns.RemovePawn(pLocal);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x000C53C4 File Offset: 0x000C35C4
		[DebugAction("Pawns", "Make +1 year older", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1YearOlder(Pawn p)
		{
			float num = p.ageTracker.BiologicalTicksPerTick;
			if (num == 0f)
			{
				num = 1f;
			}
			p.ageTracker.AgeTickMothballed(Mathf.RoundToInt(3600000f / num));
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000C5402 File Offset: 0x000C3602
		[DebugAction("Pawns", "Make +1 day older", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1DayOlder(Pawn p)
		{
			p.ageTracker.AgeTickMothballed(60000);
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000C5414 File Offset: 0x000C3614
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJobGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localType2 in typeof(ThinkNode_JobGiver).AllSubclasses())
			{
				Type localType = localType2;
				list.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, delegate()
				{
					ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)Activator.CreateInstance(localType);
					thinkNode_JobGiver.ResolveReferences();
					ThinkResult thinkResult = thinkNode_JobGiver.TryIssueJobPackage(p, default(JobIssueParams));
					if (thinkResult.Job != null)
					{
						p.jobs.StartJob(thinkResult.Job, JobCondition.None, null, false, true, null, null, false, false, null, false, true);
						return;
					}
					Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000C54C8 File Offset: 0x000C36C8
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJoyGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<JoyGiverDef>.Enumerator enumerator = DefDatabase<JoyGiverDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JoyGiverDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.Worker.CanBeGivenTo(p) ? def.defName : (def.defName + " [NO]"), DebugMenuOptionMode.Action, delegate()
					{
						Job job = def.Worker.TryGiveJob(p);
						if (job != null)
						{
							p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true);
							return;
						}
						Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000C55A0 File Offset: 0x000C37A0
		[DebugAction("Pawns", "EndCurrentJob(InterruptForced)", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void EndCurrentJobInterruptForced(Pawn p)
		{
			p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x000C55B6 File Offset: 0x000C37B6
		[DebugAction("Pawns", "CheckForJobOverride", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckForJobOverride(Pawn p)
		{
			p.jobs.CheckForJobOverride();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x000C55CC File Offset: 0x000C37CC
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleJobLogging(Pawn p)
		{
			p.jobs.debugLog = !p.jobs.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
			MoteMaker.ThrowText(p.DrawPosHeld.Value, p.MapHeld, p.LabelShort + "\n" + (p.jobs.debugLog ? "ON" : "OFF"), -1f);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x000C563F File Offset: 0x000C383F
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ToggleStanceLogging(Pawn p)
		{
			p.stances.debugLog = !p.stances.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000C5660 File Offset: 0x000C3860
		[DebugAction("Pawns", "Kidnap colonist", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void Kidnap(Pawn p)
		{
			if (p.IsColonist)
			{
				Faction faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
				if (faction != null)
				{
					faction.kidnapped.Kidnap(p, faction.leader);
				}
			}
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x000C569C File Offset: 0x000C389C
		[DebugAction("Pawns", "Face cell (selected)...", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Selected_SetFacing()
		{
			using (List<object>.Enumerator enumerator = Find.Selector.SelectedObjectsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn;
					if ((pawn = (enumerator.Current as Pawn)) != null)
					{
						pawn.rotationTracker.FaceTarget(UI.MouseCell());
					}
				}
			}
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x000C570C File Offset: 0x000C390C
		[DebugAction("Pawns", "Progress life stage", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ProgressLifeStage(Pawn p)
		{
			int curLifeStageIndex = p.ageTracker.CurLifeStageIndex;
			if (curLifeStageIndex < p.ageTracker.MaxRaceLifeStageIndex)
			{
				float minAge = p.RaceProps.lifeStageAges[curLifeStageIndex + 1].minAge;
				float minAge2 = p.RaceProps.lifeStageAges[p.RaceProps.lifeStageAges.Count - 1].minAge;
				if (p.RaceProps.Humanlike)
				{
					p.ageTracker.DebugSetAge((long)minAge * 3600000L);
				}
				else
				{
					p.ageTracker.DebugSetGrowth(minAge / minAge2);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000C57AB File Offset: 0x000C39AB
		[DebugAction("Pawns", "Make guilty", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void MakeGuilty(Pawn p)
		{
			Pawn_GuiltTracker guilt = p.guilt;
			if (guilt != null)
			{
				guilt.Notify_Guilty(60000);
			}
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000C57C9 File Offset: 0x000C39C9
		[DebugAction("Pawns", "Force age reversal demand now", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void ForceAgeReversalDemandNow(Pawn p)
		{
			p.ageTracker.DebugForceAgeReversalDemandNow();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000C57DC File Offset: 0x000C39DC
		[DebugAction("Pawns", "Reset age reversal demand", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void ResetAgeReversalDemandNow(Pawn p)
		{
			p.ageTracker.DebugResetAgeReversalDemand();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000C57F0 File Offset: 0x000C39F0
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Resurrect()
		{
			foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap).ToList<Thing>())
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					ResurrectionUtility.Resurrect(corpse.InnerPawn);
				}
			}
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000C5860 File Offset: 0x000C3A60
		public static List<DebugMenuOption> Options_AddGene(Action<GeneDef> callback)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GeneDef localDef2 in from x in DefDatabase<GeneDef>.AllDefs
			orderby x.defName
			select x)
			{
				GeneDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					callback(localDef);
				}));
			}
			return list;
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000C591C File Offset: 0x000C3B1C
		public static List<DebugMenuOption> Options_RemoveGene(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			if (pawn.genes != null)
			{
				foreach (Gene localG2 in pawn.genes.GenesListForReading)
				{
					Gene localG = localG2;
					list.Add(new DebugMenuOption(localG.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						pawn.genes.RemoveGene(localG);
					}));
				}
			}
			return list;
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000C59D0 File Offset: 0x000C3BD0
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true, displayPriority = 1000)]
		private static List<DebugActionNode> AddGene()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			DebugActionNode debugActionNode = new DebugActionNode("Xenogene", DebugActionType.Action, null, null);
			foreach (DebugActionNode child in DebugToolsPawns.<AddGene>g__GeneOptionNodes|85_0(true, false))
			{
				debugActionNode.AddChild(child);
			}
			list.Add(debugActionNode);
			DebugActionNode debugActionNode2 = new DebugActionNode("Endogene", DebugActionType.Action, null, null);
			foreach (DebugActionNode child2 in DebugToolsPawns.<AddGene>g__GeneOptionNodes|85_0(false, false))
			{
				debugActionNode2.AddChild(child2);
			}
			list.Add(debugActionNode2);
			DebugActionNode debugActionNode3 = new DebugActionNode("Heritable", DebugActionType.Action, null, null);
			foreach (DebugActionNode child3 in DebugToolsPawns.<AddGene>g__GeneOptionNodes|85_0(false, true))
			{
				debugActionNode3.AddChild(child3);
			}
			list.Add(debugActionNode3);
			return list;
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000C5AFC File Offset: 0x000C3CFC
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true, displayPriority = 1000)]
		private static void RemoveGene(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugToolsPawns.Options_RemoveGene(p)));
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000C5B14 File Offset: 0x000C3D14
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true, displayPriority = 1000)]
		private static List<DebugActionNode> SetXenotype()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (XenotypeDef localDef2 in from x in DefDatabase<XenotypeDef>.AllDefs
			orderby x.defName
			select x)
			{
				XenotypeDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn p)
					{
						Pawn_GeneTracker genes = p.genes;
						if (genes != null)
						{
							genes.SetXenotype(localDef);
						}
						DebugActionsUtility.DustPuffFrom(p);
					}
				});
			}
			return list;
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000C5BBC File Offset: 0x000C3DBC
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static List<DebugActionNode> AddLearningDesire()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (LearningDesireDef defLocal2 in DefDatabase<LearningDesireDef>.AllDefs)
			{
				LearningDesireDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Pawn_LearningTracker learning = p.learning;
					if (learning != null)
					{
						learning.Debug_SetLearningDesire(defLocal);
					}
					DebugActionsUtility.DustPuffFrom(p);
				}));
			}
			return list;
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x000C5C3C File Offset: 0x000C3E3C
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static void TryLearningGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<LearningDesireDef>.Enumerator enumerator = DefDatabase<LearningDesireDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LearningDesireDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.Worker.CanDo(p) ? def.defName : (def.defName + " [NO]"), DebugMenuOptionMode.Action, delegate()
					{
						Job job = def.Worker.TryGiveJob(p);
						if (job != null)
						{
							p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true);
							return;
						}
						Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000C5D14 File Offset: 0x000C3F14
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ListMeleeVerbs(Pawn p)
		{
			List<Verb> allMeleeVerbs = (from x in p.meleeVerbs.GetUpdatedAvailableVerbsList(false)
			select x.verb).ToList<Verb>();
			float highestWeight = 0f;
			foreach (Verb v2 in allMeleeVerbs)
			{
				float num = VerbUtility.InitialVerbWeight(v2, p);
				if (num > highestWeight)
				{
					highestWeight = num;
				}
			}
			float totalSelectionWeight = 0f;
			foreach (Verb verb in allMeleeVerbs)
			{
				totalSelectionWeight += VerbUtility.FinalSelectionWeight(verb, p, allMeleeVerbs, highestWeight);
			}
			allMeleeVerbs.SortBy((Verb x) => -VerbUtility.InitialVerbWeight(x, p));
			List<TableDataGetter<Verb>> list = new List<TableDataGetter<Verb>>();
			list.Add(new TableDataGetter<Verb>("verb", (Verb v) => v.ToString().Split(new char[]
			{
				'/'
			})[1].TrimEnd(new char[]
			{
				')'
			})));
			list.Add(new TableDataGetter<Verb>("source", delegate(Verb v)
			{
				if (v.HediffSource != null)
				{
					return v.HediffSource.Label;
				}
				if (v.tool != null)
				{
					return v.tool.label;
				}
				return "";
			}));
			list.Add(new TableDataGetter<Verb>("damage", (Verb v) => v.verbProps.AdjustedMeleeDamageAmount(v, p)));
			list.Add(new TableDataGetter<Verb>("cooldown", (Verb v) => v.verbProps.AdjustedCooldown(v, p) + "s"));
			list.Add(new TableDataGetter<Verb>("dmg/sec", (Verb v) => VerbUtility.DPS(v, p)));
			list.Add(new TableDataGetter<Verb>("armor pen", (Verb v) => v.verbProps.AdjustedArmorPenetration(v, p)));
			list.Add(new TableDataGetter<Verb>("hediff", delegate(Verb v)
			{
				string text = "";
				if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
				{
					foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
					{
						text = text + damageDefAdditionalHediff.hediff.label + " ";
					}
				}
				return text;
			}));
			list.Add(new TableDataGetter<Verb>("weight", (Verb v) => VerbUtility.InitialVerbWeight(v, p)));
			list.Add(new TableDataGetter<Verb>("category", delegate(Verb v)
			{
				VerbSelectionCategory selectionCategory = v.GetSelectionCategory(p, highestWeight);
				if (selectionCategory == VerbSelectionCategory.Best)
				{
					return "Best".Colorize(Color.green);
				}
				if (selectionCategory != VerbSelectionCategory.Worst)
				{
					return "Mid";
				}
				return "Worst".Colorize(Color.grey);
			}));
			list.Add(new TableDataGetter<Verb>("sel %", (Verb v) => base.<ListMeleeVerbs>g__GetSelectionPercent|1(v).ToStringPercent("F2")));
			List<TableDataGetter<Verb>> list2 = list;
			DebugTables.MakeTablesDialog<Verb>(allMeleeVerbs, list2.ToArray());
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000C5FC0 File Offset: 0x000C41C0
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DoVoiceCall(Pawn p)
		{
			Pawn_CallTracker caller = p.caller;
			if (caller == null)
			{
				return;
			}
			caller.DoCall();
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000C5FD4 File Offset: 0x000C41D4
		[DebugAction("Pawns", "Force vomit", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ForceVomit(Pawn p)
		{
			p.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true);
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x000C6011 File Offset: 0x000C4211
		[DebugAction("Pawns", "Reset pawn render cache", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ResetRenderCache(Pawn p)
		{
			p.Drawer.renderer.graphics.SetApparelGraphicsDirty();
			PortraitsCache.SetDirty(p);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(p);
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000C6038 File Offset: 0x000C4238
		[CompilerGenerated]
		internal static List<DebugActionNode> <SetBackstory>g__BackstoryOptionNodes|39_0(BackstorySlot slot)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			IEnumerable<BackstoryDef> allDefs = DefDatabase<BackstoryDef>.AllDefs;
			Func<BackstoryDef, bool> <>9__1;
			Func<BackstoryDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((BackstoryDef b) => b.slot == slot));
			}
			using (IEnumerator<BackstoryDef> enumerator = allDefs.Where(predicate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BackstoryDef outerBackstory = enumerator.Current;
					list.Add(new DebugActionNode(outerBackstory.defName, DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							if (p.story != null)
							{
								if (slot == BackstorySlot.Adulthood)
								{
									p.story.Adulthood = outerBackstory;
								}
								else
								{
									p.story.Childhood = outerBackstory;
								}
								MeditationFocusTypeAvailabilityCache.ClearFor(p);
								DebugActionsUtility.DustPuffFrom(p);
							}
						}
					});
				}
			}
			return list;
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000C60F8 File Offset: 0x000C42F8
		[CompilerGenerated]
		internal static List<DebugActionNode> <AddGene>g__GeneOptionNodes|85_0(bool xenogene, bool heritableOnly)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (GeneDef localDef2 in from x in DefDatabase<GeneDef>.AllDefs
			orderby x.defName
			select x)
			{
				GeneDef localDef = localDef2;
				if ((xenogene || localDef.biostatArc <= 0) && (!heritableOnly || localDef.endogeneCategory != EndogeneCategory.None || !localDef.forcedTraits.NullOrEmpty<GeneticTraitData>() || DefDatabase<XenotypeDef>.AllDefs.Any((XenotypeDef x) => x.genes.Contains(localDef) && x.inheritable)))
				{
					list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMapForPawns, null, null)
					{
						pawnAction = delegate(Pawn p)
						{
							Pawn_GeneTracker genes = p.genes;
							if (genes != null)
							{
								genes.AddGene(localDef, xenogene);
							}
							DebugActionsUtility.DustPuffFrom(p);
						}
					});
				}
			}
			return list;
		}
	}
}

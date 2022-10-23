using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;
using Verse.Profile;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000436 RID: 1078
	public static class DebugActionsMisc
	{
		// Token: 0x06001FE2 RID: 8162 RVA: 0x000BE0C0 File Offset: 0x000BC2C0
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllPlants()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				if (thing is Plant)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000BE12C File Offset: 0x000BC32C
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllThings()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000BE18C File Offset: 0x000BC38C
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyClutter()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Chunk).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
			foreach (Thing thing2 in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Filth).ToList<Thing>())
			{
				thing2.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000BE240 File Offset: 0x000BC440
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllHats()
		{
			foreach (Pawn pawn in PawnsFinder.AllMaps)
			{
				if (pawn.RaceProps.Humanlike)
				{
					for (int i = pawn.apparel.WornApparel.Count - 1; i >= 0; i--)
					{
						Apparel apparel = pawn.apparel.WornApparel[i];
						if (apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
						{
							apparel.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000BE30C File Offset: 0x000BC50C
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllCorpses()
		{
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000BE34A File Offset: 0x000BC54A
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FinishAllResearch()
		{
			Find.ResearchManager.DebugSetAllProjectsFinished();
			Messages.Message("All research finished.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000BE368 File Offset: 0x000BC568
		[DebugAction("General", "Add techprint to project", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					Find.ResearchManager.AddTechprints(localProject, localProject.TechprintCount - Find.ResearchManager.GetTechprints(localProject));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000BE41C File Offset: 0x000BC61C
		[DebugAction("General", "Apply techprint on project", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ApplyTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				Action <>9__2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DebugMenuOption> list3 = list2;
					string label = "None";
					DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
					Action method;
					if ((method = <>9__2) == null)
					{
						method = (<>9__2 = delegate()
						{
							Find.ResearchManager.ApplyTechprint(localProject, null);
						});
					}
					list3.Add(new DebugMenuOption(label, mode, method));
					foreach (Pawn localColonist2 in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
					{
						Pawn localColonist = localColonist2;
						list2.Add(new DebugMenuOption(localColonist.LabelCap, DebugMenuOptionMode.Action, delegate()
						{
							Find.ResearchManager.ApplyTechprint(localProject, localColonist);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x000BE4D0 File Offset: 0x000BC6D0
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> AddTradeShipOfKind()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<TraderKindDef> enumerator = (from t in DefDatabase<TraderKindDef>.AllDefs
			where t.orbital
			select t).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TraderKindDef traderKind = enumerator.Current;
					list.Add(new DebugActionNode(traderKind.label, DebugActionType.Action, delegate()
					{
						Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.traderKind = traderKind;
						IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x000BE570 File Offset: 0x000BC770
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ReplaceAllTradeShips()
		{
			Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
			for (int i = 0; i < 5; i++)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.CurrentMap;
				IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
			}
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000BE5BC File Offset: 0x000BC7BC
		[DebugAction("General", "Change weather...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> ChangeWeather()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (WeatherDef localWeather2 in DefDatabase<WeatherDef>.AllDefs)
			{
				WeatherDef localWeather = localWeather2;
				list.Add(new DebugActionNode(localWeather.LabelCap, DebugActionType.Action, delegate()
				{
					Find.CurrentMap.weatherManager.TransitionTo(localWeather);
				}, null));
			}
			return list;
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000BE640 File Offset: 0x000BC840
		[DebugAction("General", "Play song...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> PlaySong()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SongDef localSong2 in DefDatabase<SongDef>.AllDefs)
			{
				SongDef localSong = localSong2;
				list.Add(new DebugActionNode(localSong.defName, DebugActionType.Action, delegate()
				{
					Find.MusicManagerPlay.ForceStartSong(localSong, false);
				}, null));
			}
			return list;
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000BE6C0 File Offset: 0x000BC8C0
		[DebugAction("General", "Play sound...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> PlaySound()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SoundDef localSd2 in from s in DefDatabase<SoundDef>.AllDefs
			where !s.sustain
			select s)
			{
				SoundDef localSd = localSd2;
				list.Add(new DebugActionNode(localSd.defName, DebugActionType.Action, delegate()
				{
					if (localSd.subSounds.Any((SubSoundDef sub) => sub.onCamera))
					{
						localSd.PlayOneShotOnCamera(null);
						return;
					}
					localSd.PlayOneShot(SoundInfo.InMap(new TargetInfo(Find.CameraDriver.MapPosition, Find.CurrentMap, false), MaintenanceType.None));
				}, null));
			}
			return list;
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x000BE764 File Offset: 0x000BC964
		[DebugAction("General", "End game condition...", false, false, false, 0, false, allowedGameStates = (AllowedGameStates.Playing | AllowedGameStates.IsCurrentlyOnMap | AllowedGameStates.HasGameCondition))]
		private static void EndGameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameCondition localMc2 in Find.CurrentMap.gameConditionManager.ActiveConditions)
			{
				GameCondition localMc = localMc2;
				list.Add(new DebugMenuOption(localMc.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					localMc.End();
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x000BE800 File Offset: 0x000BCA00
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static List<DebugActionNode> SimulateSanguophageMeeting()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 1; i <= 8; i++)
			{
				int num = i;
				list.Add(new DebugActionNode(num + " sanguophages", DebugActionType.ToolMap, delegate()
				{
					List<FactionRelation> list2 = new List<FactionRelation>();
					foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
					{
						if (!faction.def.permanentEnemy)
						{
							list2.Add(new FactionRelation(faction, FactionRelationKind.Neutral));
						}
					}
					Faction faction2 = FactionGenerator.NewGeneratedFactionWithRelations(FactionDefOf.Sanguophages, list2, true);
					faction2.temporary = true;
					Find.FactionManager.Add(faction2);
					List<Pawn> list3 = new List<Pawn>();
					PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Sanguophage, faction2, PawnGenerationContext.NonPlayer, -1, true, false, false, true, true, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false);
					for (int j = 0; j < num; j++)
					{
						list3.Add(PawnGenerator.GeneratePawn(request));
					}
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					PawnsArrivalModeDef edgeWalkIn = PawnsArrivalModeDefOf.EdgeWalkIn;
					edgeWalkIn.Worker.TryResolveRaidSpawnCenter(incidentParms);
					edgeWalkIn.Worker.Arrive(list3, incidentParms);
					LordMaker.MakeNewLord(faction2, new LordJob_SanguophageMeeting(UI.MouseCell(), new List<Thing>(), 60000, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty), Find.CurrentMap, list3);
				}, null));
			}
			return list;
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x000BE85C File Offset: 0x000BCA5C
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyAssault()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				LordToil_Stage lordToil_Stage = lord.CurLordToil as LordToil_Stage;
				if (lordToil_Stage != null)
				{
					foreach (Transition transition in lord.Graph.transitions)
					{
						if (transition.sources.Contains(lordToil_Stage) && (transition.target is LordToil_AssaultColony || transition.target is LordToil_AssaultColonyBreaching || transition.target is LordToil_AssaultColonyPrisoners || transition.target is LordToil_AssaultColonySappers || transition.target is LordToil_AssaultColonyBossgroup || transition.target is LordToil_MoveInBossgroup))
						{
							Messages.Message("Debug forcing to assault toil: " + lord.faction, MessageTypeDefOf.TaskCompletion, false);
							lord.GotoToil(transition.target);
							return;
						}
					}
				}
			}
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (quest.State == QuestState.Ongoing)
				{
					using (List<QuestPart>.Enumerator enumerator4 = quest.PartsListForReading.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							QuestPart_BossgroupArrives questPart_BossgroupArrives;
							if ((questPart_BossgroupArrives = (enumerator4.Current as QuestPart_BossgroupArrives)) != null && questPart_BossgroupArrives.State == QuestPartState.Enabled)
							{
								questPart_BossgroupArrives.DebugForceComplete();
								Messages.Message("Debug forcing bossgroup assault.", MessageTypeDefOf.TaskCompletion, false);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x000BEA58 File Offset: 0x000BCC58
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyFlee()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				if (lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer) && lord.faction.def.autoFlee)
				{
					LordToil lordToil = lord.Graph.lordToils.FirstOrDefault((LordToil st) => st is LordToil_PanicFlee);
					if (lordToil != null)
					{
						lord.GotoToil(lordToil);
					}
				}
			}
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x000BEB14 File Offset: 0x000BCD14
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AdaptionProgress10Days()
		{
			Find.StoryWatcher.watcherAdaptation.Debug_OffsetAdaptDays(10f);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x000BEB2A File Offset: 0x000BCD2A
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void UnloadUnusedAssets()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000BEB34 File Offset: 0x000BCD34
		[DebugAction("General", "Name settlement...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void NameSettlement()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Faction", DebugMenuOptionMode.Action, delegate()
			{
				Find.WindowStack.Add(new Dialog_NamePlayerFaction());
			}));
			if (Find.CurrentMap != null && Find.CurrentMap.IsPlayerHome && Find.CurrentMap.Parent is Settlement)
			{
				Settlement factionBase = (Settlement)Find.CurrentMap.Parent;
				list.Add(new DebugMenuOption("Faction base", DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_NamePlayerSettlement(factionBase));
				}));
				list.Add(new DebugMenuOption("Faction and faction base", DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(factionBase));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000BEC00 File Offset: 0x000BCE00
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void NextLesson()
		{
			LessonAutoActivator.DebugForceInitiateBestLessonNow();
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000BEC08 File Offset: 0x000BCE08
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 900)]
		private static List<DebugActionNode> ChangeCameraConfig()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			list.Add(new DebugActionNode("Open editor", DebugActionType.Action, delegate()
			{
				Find.WindowStack.Add(new Dialog_CameraConfig());
			}, null));
			List<DebugActionNode> list2 = list;
			foreach (Type localType2 in typeof(CameraMapConfig).AllSubclasses())
			{
				Type localType = localType2;
				string text = localType.Name;
				if (text.StartsWith("CameraMapConfig_"))
				{
					text = text.Substring("CameraMapConfig_".Length);
				}
				list2.Add(new DebugActionNode(text, DebugActionType.Action, delegate()
				{
					Find.CameraDriver.config = (CameraMapConfig)Activator.CreateInstance(localType);
				}, null));
			}
			return list2;
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000BECEC File Offset: 0x000BCEEC
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ForceShipCountdown()
		{
			ShipCountdown.InitiateCountdown(null);
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000BECF4 File Offset: 0x000BCEF4
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ForceStartShip()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return;
			}
			Building_ShipComputerCore building_ShipComputerCore = (Building_ShipComputerCore)currentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Ship_ComputerCore).FirstOrDefault<Building>();
			if (building_ShipComputerCore == null)
			{
				Messages.Message("Could not find any compute core on current map!", MessageTypeDefOf.NeutralEvent, true);
			}
			building_ShipComputerCore.ForceLaunch();
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000BED40 File Offset: 0x000BCF40
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashTradeDropSpot()
		{
			IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.CurrentMap);
			Find.CurrentMap.debugDrawer.FlashCell(intVec, 0f, null, 50);
			Log.Message("trade drop spot: " + intVec);
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x000BED88 File Offset: 0x000BCF88
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeFactionLeader(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactionsVisible.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction faction = enumerator.Current;
					list.Add(new DebugMenuOption(faction.Name, DebugMenuOptionMode.Action, delegate()
					{
						if (faction.leader != p)
						{
							faction.leader = p;
							if (ModsConfig.IdeologyActive)
							{
								using (List<Precept>.Enumerator enumerator2 = faction.ideos.PrimaryIdeo.PreceptsListForReading.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										Precept_Role precept_Role;
										if ((precept_Role = (enumerator2.Current as Precept_Role)) != null && precept_Role.def.leaderRole)
										{
											precept_Role.Assign(p, false);
											break;
										}
									}
								}
							}
							DebugActionsUtility.DustPuffFrom(p);
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x000BEE2C File Offset: 0x000BD02C
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillFactionLeader()
		{
			Pawn leader = (from x in Find.FactionManager.AllFactions
			where x.leader != null
			select x).RandomElement<Faction>().leader;
			int num = 0;
			while (!leader.Dead)
			{
				if (++num > 1000)
				{
					Log.Warning("Could not kill faction leader.");
					return;
				}
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Bullet, 30f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetIgnoreInstantKillProtection(true);
				leader.TakeDamage(dinfo);
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000BEEC8 File Offset: 0x000BD0C8
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillKidnappedPawn()
		{
			IEnumerable<Pawn> pawnsBySituation = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.Kidnapped);
			if (pawnsBySituation.Any<Pawn>())
			{
				Pawn pawn = pawnsBySituation.RandomElement<Pawn>();
				pawn.Kill(null, null);
				Messages.Message("Killed " + pawn.LabelCap, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000BEF1C File Offset: 0x000BD11C
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				Pawn pLocal = pawn;
				list.Add(new DebugMenuOption(pawn.LabelShort + "(" + pawn.kindDef.label + ")", DebugMenuOptionMode.Action, delegate()
				{
					pLocal.Kill(null, null);
					Messages.Message("Killed " + pLocal.LabelCap, MessageTypeDefOf.NeutralEvent, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000BEFC8 File Offset: 0x000BD1C8
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetFactionRelations()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactionsVisibleInViewOrder)
			{
				Faction localFac = localFac2;
				foreach (object obj in Enum.GetValues(typeof(FactionRelationKind)))
				{
					FactionRelationKind localRk2 = (FactionRelationKind)obj;
					FactionRelationKind localRk = localRk2;
					FloatMenuOption item = new FloatMenuOption(localFac + " - " + localRk, delegate()
					{
						if (localRk == FactionRelationKind.Hostile)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(localFac, -100, true, true, HistoryEventDefOf.DebugGoodwill, null);
							return;
						}
						if (localRk == FactionRelationKind.Ally)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(localFac, 100, true, true, HistoryEventDefOf.DebugGoodwill, null);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x000BF0F0 File Offset: 0x000BD2F0
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void VisitorGift()
		{
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.AllPawnsSpawned)
			{
				if (pawn.Faction != null && !pawn.Faction.IsPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					list.Add(pawn);
					break;
				}
			}
			VisitorGiftForPlayerUtility.GiveRandomGift(list, list[0].Faction);
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000BF190 File Offset: 0x000BD390
		[DebugAction("General", "Increment time", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 500)]
		private static List<DebugActionNode> IncrementTime()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < DebugActionsMisc.TimeIncreases.Length; i++)
			{
				int durationLocal = DebugActionsMisc.TimeIncreases[i];
				list.Add(new DebugActionNode(durationLocal.ToStringTicksToPeriod(true, false, true, true, false), DebugActionType.Action, delegate()
				{
					Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + durationLocal);
				}, null));
			}
			return list;
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x000BF1F4 File Offset: 0x000BD3F4
		[DebugAction("General", "Storywatcher tick 1 day", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void StorywatcherTick1Day()
		{
			for (int i = 0; i < 60000; i++)
			{
				Find.StoryWatcher.StoryWatcherTick();
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
			}
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x000BF234 File Offset: 0x000BD434
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void KillRandomLentColonist()
		{
			if (QuestUtility.TotalBorrowedColonistCount() > 0)
			{
				DebugActionsMisc.tmpLentColonists.Clear();
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (questsListForReading[i].State == QuestState.Ongoing)
					{
						List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
						for (int j = 0; j < partsListForReading.Count; j++)
						{
							QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction;
							if ((questPart_LendColonistsToFaction = (partsListForReading[j] as QuestPart_LendColonistsToFaction)) != null)
							{
								List<Thing> lentColonistsListForReading = questPart_LendColonistsToFaction.LentColonistsListForReading;
								for (int k = 0; k < lentColonistsListForReading.Count; k++)
								{
									Pawn pawn;
									if ((pawn = (lentColonistsListForReading[k] as Pawn)) != null && !pawn.Dead)
									{
										DebugActionsMisc.tmpLentColonists.Add(pawn);
									}
								}
							}
						}
					}
				}
				Pawn pawn2 = DebugActionsMisc.tmpLentColonists.RandomElement<Pawn>();
				bool flag = pawn2.health.hediffSet.hediffs.Any((Hediff x) => x.def.isBad);
				pawn2.Kill(null, flag ? pawn2.health.hediffSet.hediffs.RandomElement<Hediff>() : null);
			}
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x000BF370 File Offset: 0x000BD570
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ClearPrisonerInteractionSchedule(Pawn p)
		{
			if (p.IsPrisonerOfColony)
			{
				p.mindState.lastAssignedInteractTime = -1;
				p.mindState.interactionsToday = 0;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x000BF398 File Offset: 0x000BD598
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -100)]
		private static void GlowAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (c.InBounds(currentMap))
				{
					float num = Find.CurrentMap.glowGrid.GameGlowAt(c, false);
					currentMap.debugDrawer.FlashCell(c, 0f, num.ToString("F1"), 100);
				}
			}
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x000BF428 File Offset: 0x000BD628
		[DebugAction("General", "HSV At Position", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -100)]
		private static void HSVAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (c.InBounds(currentMap))
				{
					float num;
					float num2;
					float num3;
					Color.RGBToHSV(Find.CurrentMap.glowGrid.VisualGlowAt(c), out num, out num2, out num3);
					currentMap.debugDrawer.FlashCell(c, 0.5f, string.Concat(new string[]
					{
						"HSV(",
						num.ToString(".0#"),
						",",
						num2.ToString(".0#"),
						",",
						num3.ToString(".0#"),
						")"
					}), 100);
				}
			}
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x000BF51C File Offset: 0x000BD71C
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashBlockedLandingCells()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in currentMap.AllCells)
			{
				if (!c.Fogged(currentMap) && !DropCellFinder.IsGoodDropSpot(c, currentMap, false, false, false))
				{
					currentMap.debugDrawer.FlashCell(c, 0f, "bl", 50);
				}
			}
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x000BF598 File Offset: 0x000BD798
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> PawnKindApparelCheck()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.race == ThingDefOf.Human
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.Action, delegate()
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
					bool flag = false;
					for (int i = 0; i < 100; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, faction);
						if (pawn.royalty != null)
						{
							RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
							if (mostSeniorTitle != null && !mostSeniorTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
							{
								for (int j = 0; j < mostSeniorTitle.def.requiredApparel.Count; j++)
								{
									ApparelRequirement apparelRequirement = mostSeniorTitle.def.requiredApparel[j];
									if (apparelRequirement.IsActive(pawn) && !apparelRequirement.IsMet(pawn))
									{
										Log.Error(string.Concat(new object[]
										{
											localKindDef,
											" (",
											mostSeniorTitle.def.label,
											")  does not have its title requirements met. index=",
											j,
											DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|43_0(pawn)
										}));
										flag = true;
									}
								}
							}
						}
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int k = 0; k < wornApparel.Count; k++)
						{
							string text = DebugActionsMisc.<PawnKindApparelCheck>g__apparelOkayToWear|43_1(pawn, wornApparel[k]);
							if (text != "OK")
							{
								Log.Error(text + " - " + wornApparel[k].Label + DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|43_0(pawn));
								flag = true;
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (!flag)
					{
						Log.Message("No errors for " + localKindDef.defName);
					}
				}, null));
			}
			return list;
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x000BF660 File Offset: 0x000BD860
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> PawnKindAbilityCheck()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.titleRequired != null || !kd.titleSelectOne.NullOrEmpty<RoyalTitleDef>()
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.Action, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
					for (int i = 0; i < 100; i++)
					{
						RoyalTitleDef fixedTitle = null;
						if (localKindDef.titleRequired != null)
						{
							fixedTitle = localKindDef.titleRequired;
						}
						else if (!localKindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() && Rand.Chance(localKindDef.royalTitleChance))
						{
							fixedTitle = localKindDef.titleSelectOne.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
						}
						Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(localKindDef, faction, PawnGenerationContext.NonPlayer, -1, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, fixedTitle, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false));
						RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
						if (mostSeniorTitle != null)
						{
							Hediff_Psylink mainPsylinkSource = pawn.GetMainPsylinkSource();
							if (mainPsylinkSource == null)
							{
								if (mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def) > 0)
								{
									string text = mostSeniorTitle.def.LabelCap + " - No psylink.";
									if (pawn.abilities.abilities.Any((Ability x) => x.def.level > 0))
									{
										text += " Has psycasts without psylink.";
									}
									stringBuilder.AppendLine(text);
								}
							}
							else if (mainPsylinkSource.level < mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								stringBuilder.AppendLine(string.Concat(new object[]
								{
									"Psylink at level ",
									mainPsylinkSource.level,
									", but requires ",
									mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def)
								}));
							}
							else if (mainPsylinkSource.level > mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								stringBuilder.AppendLine(string.Concat(new object[]
								{
									"Psylink at level ",
									mainPsylinkSource.level,
									". Max is ",
									mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def)
								}));
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (stringBuilder.Length == 0)
					{
						Log.Message("No errors for " + localKindDef.defName);
						return;
					}
					Log.Error("Errors:\n" + stringBuilder.ToString());
				}, null));
			}
			return list;
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000BF728 File Offset: 0x000BD928
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		public static void AtlasRebuild()
		{
			GlobalTextureAtlasManager.rebakeAtlas = true;
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x000BF730 File Offset: 0x000BD930
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DumpPawnAtlases()
		{
			string text = Application.dataPath + "\\atlasDump_Pawn";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			GlobalTextureAtlasManager.DumpPawnAtlases(text);
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x000BF764 File Offset: 0x000BD964
		[DebugAction("General", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DumpStaticAtlases()
		{
			string text = Application.dataPath + "\\atlasDump_Static";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			GlobalTextureAtlasManager.DumpStaticAtlases(text);
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x000BF7B8 File Offset: 0x000BD9B8
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__logApparel|43_0(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(string.Format("Apparel of {0}:", p.LabelShort));
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				stringBuilder.AppendLine("  - " + wornApparel[i].Label);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x000BF82C File Offset: 0x000BDA2C
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__apparelOkayToWear|43_1(Pawn pawn, Apparel apparel)
		{
			ApparelProperties app = apparel.def.apparel;
			if (!pawn.kindDef.apparelRequired.NullOrEmpty<ThingDef>() && pawn.kindDef.apparelRequired.Contains(apparel.def))
			{
				return "OK";
			}
			if (!app.PawnCanWear(pawn, false))
			{
				return "Pawn cannot wear.";
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirements[i], apparel.def) && PawnApparelGenerator.ApparelRequirementTagsMatch(specificApparelRequirements[i], apparel.def))
					{
						return "OK";
					}
				}
			}
			if (!pawn.kindDef.apparelTags.NullOrEmpty<string>())
			{
				if (!app.tags.Any((string tag) => pawn.kindDef.apparelTags.Contains(tag)))
				{
					return "Required tag missing.";
				}
				if ((pawn.royalty == null || pawn.royalty.MostSeniorTitle == null) && app.tags.Contains("Royal") && !pawn.kindDef.apparelTags.Any((string tag) => app.tags.Contains(tag)))
				{
					return "Royal apparel on non-royal pawn.";
				}
			}
			if (!pawn.kindDef.apparelDisallowTags.NullOrEmpty<string>() && pawn.kindDef.apparelDisallowTags.Any((string t) => app.tags.Contains(t)))
			{
				return "Has a disallowed tag.";
			}
			if (pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
			{
				RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
				QualityCategory qualityCategory;
				if (apparel.TryGetQuality(out qualityCategory) && qualityCategory < mostSeniorTitle.def.requiredMinimumApparelQuality)
				{
					return "Quality too low.";
				}
			}
			return "OK";
		}

		// Token: 0x040015A4 RID: 5540
		private static readonly int[] TimeIncreases = new int[]
		{
			2500,
			15000,
			30000,
			60000,
			900000
		};

		// Token: 0x040015A5 RID: 5541
		private static List<Pawn> tmpLentColonists = new List<Pawn>();

		// Token: 0x040015A6 RID: 5542
		private const string NoErrorString = "OK";

		// Token: 0x040015A7 RID: 5543
		private const string RoyalApparelTag = "Royal";

		// Token: 0x040015A8 RID: 5544
		private const int PawnsToGenerate = 100;
	}
}

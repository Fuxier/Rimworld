using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000433 RID: 1075
	public static class DebugActionsIdeo
	{
		// Token: 0x06001F9D RID: 8093 RVA: 0x000BBEAC File Offset: 0x000BA0AC
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void SetIdeo()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Ideo> ideosListForReading = Find.IdeoManager.IdeosListForReading;
			for (int i = 0; i < ideosListForReading.Count; i++)
			{
				Ideo ideo = ideosListForReading[i];
				list.Add(new DebugMenuOption(ideo.name, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in UI.MouseCell().GetThingList(Find.CurrentMap).OfType<Pawn>().ToList<Pawn>())
					{
						if (!pawn.RaceProps.Humanlike)
						{
							break;
						}
						pawn.ideo.SetIdeo(ideo);
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x000BBF24 File Offset: 0x000BA124
		[DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, hideInSubMenu = true)]
		private static void ConvertToIdeo()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Ideo> ideosListForReading = Find.IdeoManager.IdeosListForReading;
			for (int i = 0; i < ideosListForReading.Count; i++)
			{
				Ideo ideo = ideosListForReading[i];
				list.Add(new DebugMenuOption(ideo.name, DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in UI.MouseCell().GetThingList(Find.CurrentMap).OfType<Pawn>().ToList<Pawn>())
					{
						if (!pawn.RaceProps.Humanlike || pawn.Ideo == ideo)
						{
							break;
						}
						pawn.ideo.IdeoConversionAttempt(1f, ideo, false);
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x000BBF9C File Offset: 0x000BA19C
		[DebugAction("Ideoligion", "Set ideo role...", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void SetIdeoRole(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			if (p.Ideo != null)
			{
				Precept_Role currentRole = p.Ideo.GetRole(p);
				using (List<Precept_Role>.Enumerator enumerator = p.Ideo.cachedPossibleRoles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept_Role role = enumerator.Current;
						if (role != currentRole)
						{
							list.Add(new DebugMenuOption(role.LabelCap, DebugMenuOptionMode.Action, delegate()
							{
								role.Assign(p, true);
							}));
						}
					}
				}
				if (currentRole != null)
				{
					list.Add(new DebugMenuOption("None", DebugMenuOptionMode.Action, delegate()
					{
						currentRole.Assign(null, true);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x000BC0D4 File Offset: 0x000BA2D4
		[DebugAction("Ideoligion", "Certainty - 20%", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, requiresIdeology = true, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetCertaintyNegative20(Pawn p)
		{
			if (p.ideo != null)
			{
				p.ideo.Debug_ReduceCertainty(0.2f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x000BC0F4 File Offset: 0x000BA2F4
		[DebugAction("Ideoligion", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		public static void SpawnRelic()
		{
			IntVec3 cell = UI.MouseCell();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Precept> preceptsListForReading = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				Precept precept = preceptsListForReading[i];
				Precept_Relic relicPrecept;
				if ((relicPrecept = (precept as Precept_Relic)) != null)
				{
					list.Add(new DebugMenuOption(precept.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						GenSpawn.Spawn(relicPrecept.GenerateRelic(), cell, Find.CurrentMap, WipeMode.Vanish);
					}));
				}
			}
			if (list.Any<DebugMenuOption>())
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			}
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x000BC1A4 File Offset: 0x000BA3A4
		[DebugAction("Ideoligion", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		public static void SetSourcePrecept()
		{
			List<Thing> things = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (things.NullOrEmpty<Thing>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Precept> preceptsListForReading = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				Precept precept = preceptsListForReading[i];
				Precept_ThingStyle stylePrecept;
				if ((stylePrecept = (precept as Precept_ThingStyle)) != null)
				{
					list.Add(new DebugMenuOption(precept.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						foreach (Thing thing in things)
						{
							thing.StyleSourcePrecept = stylePrecept;
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x000BC270 File Offset: 0x000BA470
		[DebugAction("Ideoligion", "Remove ritual obligation", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void RemoveRitualObligation()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual ritual;
					if ((ritual = (precept as Precept_Ritual)) != null && !ritual.activeObligations.NullOrEmpty<RitualObligation>())
					{
						using (List<RitualObligation>.Enumerator enumerator3 = ritual.activeObligations.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								RitualObligation obligation = enumerator3.Current;
								string text = ritual.LabelCap;
								string text2 = ritual.obligationTargetFilter.LabelExtraPart(obligation);
								if (text2.NullOrEmpty())
								{
									text = text + " " + text2;
								}
								list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
								{
									ritual.activeObligations.Remove(obligation);
								}));
							}
						}
					}
				}
			}
			if (list.Any<DebugMenuOption>())
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				return;
			}
			Messages.Message("No obligations to remove.", LookTargets.Invalid, MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x000BC454 File Offset: 0x000BA654
		[DebugAction("Ideoligion", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = 1000)]
		private static List<DebugActionNode> AddPrecept()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PreceptDef localDef2 in DefDatabase<PreceptDef>.AllDefs)
			{
				PreceptDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.issue.LabelCap + ": " + localDef.LabelCap, DebugActionType.Action, delegate()
				{
					Faction.OfPlayer.ideos.PrimaryIdeo.AddPrecept(PreceptMaker.MakePrecept(localDef), true, null, null);
				}, null));
			}
			return list;
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x000BC4F8 File Offset: 0x000BA6F8
		[DebugAction("Ideoligion", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresIdeology = true)]
		private static void RemovePrecept()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<Precept>.Enumerator enumerator = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept precept = enumerator.Current;
					list.Add(new DebugMenuOption(precept.def.issue.LabelCap + ": " + precept.def.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						Faction.OfPlayer.ideos.PrimaryIdeo.RemovePrecept(precept, false);
					}));
				}
			}
			using (List<Ideo>.Enumerator enumerator2 = Faction.OfPlayer.ideos.IdeosMinorListForReading.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Ideo ideo = enumerator2.Current;
					using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Precept precept = enumerator.Current;
							list.Add(new DebugMenuOption(precept.def.issue.LabelCap + ": " + precept.def.LabelCap, DebugMenuOptionMode.Action, delegate()
							{
								ideo.RemovePrecept(precept, false);
							}));
						}
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x000BC6C8 File Offset: 0x000BA8C8
		[DebugAction("Ideoligion", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void TriggerDateRitual()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual ritual;
					if ((ritual = (precept as Precept_Ritual)) != null && ritual.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>() != null)
					{
						string text = ritual.LabelCap;
						if (!ideo.ObligationsActive && !precept.def.allowOptionalRitualObligations)
						{
							text += "[NO]";
						}
						list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
						{
							ritual.AddObligation(new RitualObligation(ritual, true));
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x000BC7F8 File Offset: 0x000BA9F8
		[DebugAction("Ideoligion", "Add 5 days to obligation timer", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void Add5DaysToObligationTimer()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				using (List<Precept>.Enumerator enumerator2 = ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Precept_Ritual precept_Ritual;
						if ((precept_Ritual = (enumerator2.Current as Precept_Ritual)) != null && !precept_Ritual.activeObligations.NullOrEmpty<RitualObligation>())
						{
							using (List<RitualObligation>.Enumerator enumerator3 = precept_Ritual.activeObligations.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									RitualObligation obligation = enumerator3.Current;
									string text = precept_Ritual.LabelCap;
									string text2 = precept_Ritual.obligationTargetFilter.LabelExtraPart(obligation);
									if (text2.NullOrEmpty())
									{
										text = text + " " + text2;
									}
									list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
									{
										obligation.DebugOffsetTriggeredTick(-300000);
									}));
								}
							}
						}
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x000BC958 File Offset: 0x000BAB58
		[DebugAction("Pawns", "Suppression +10%", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = -1000)]
		private static void SuppressionPlus10(Pawn p)
		{
			if (p.guest != null && p.IsSlave)
			{
				Need_Suppression need_Suppression = p.needs.TryGetNeed<Need_Suppression>();
				need_Suppression.CurLevel += need_Suppression.MaxLevel * 0.1f;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x000BC9A0 File Offset: 0x000BABA0
		[DebugAction("Pawns", "Suppression -10%", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = -1000)]
		private static void SuppressionMinus10(Pawn p)
		{
			if (p.guest != null && p.IsSlave)
			{
				Need_Suppression need_Suppression = p.needs.TryGetNeed<Need_Suppression>();
				need_Suppression.CurLevel -= need_Suppression.MaxLevel * 0.1f;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x000BC9E8 File Offset: 0x000BABE8
		[DebugAction("Pawns", "Clear suppression schedule", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, hideInSubMenu = true)]
		private static void ResetSuppresionSchedule(Pawn p)
		{
			if (p.guest != null && p.IsSlave)
			{
				p.mindState.lastSlaveSuppressedTick = -99999;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x000BCA10 File Offset: 0x000BAC10
		[DebugAction("Pawns", "Will +1", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void WillPlus1(Pawn p)
		{
			if (p.guest != null && p.IsPrisoner)
			{
				Pawn_GuestTracker guest = p.guest;
				Pawn_GuestTracker guest2 = p.guest;
				float num = guest2.will + 1f;
				guest2.will = num;
				guest.will = Mathf.Max(num, 0f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x000BCA64 File Offset: 0x000BAC64
		[DebugAction("Pawns", "Will -1", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void WillMinus1(Pawn p)
		{
			if (p.guest != null && p.IsPrisoner)
			{
				Pawn_GuestTracker guest = p.guest;
				Pawn_GuestTracker guest2 = p.guest;
				float num = guest2.will - 1f;
				guest2.will = num;
				guest.will = Mathf.Max(num, 0f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x000BCAB6 File Offset: 0x000BACB6
		[DebugAction("Pawns", "Start slave rebellion (random)", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = -1000)]
		private static void StartSlaveRebellion(Pawn p)
		{
			if (SlaveRebellionUtility.CanParticipateInSlaveRebellion(p) && SlaveRebellionUtility.StartSlaveRebellion(p, false))
			{
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x000BCACF File Offset: 0x000BACCF
		[DebugAction("Pawns", "Start slave rebellion (aggressive)", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true, displayPriority = -1000)]
		private static void StartSlaveRebellionAggressive(Pawn p)
		{
			if (SlaveRebellionUtility.CanParticipateInSlaveRebellion(p) && SlaveRebellionUtility.StartSlaveRebellion(p, true))
			{
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x000BCAE8 File Offset: 0x000BACE8
		[DebugAction("Pawns", "Change style", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeStyle(Pawn p)
		{
			if (p.RaceProps.Humanlike && p.story != null)
			{
				Find.WindowStack.Add(new Dialog_StylingStation(p, null));
			}
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000BCB10 File Offset: 0x000BAD10
		[DebugAction("Pawns", "Request style change", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void RequestStyleChange(Pawn p)
		{
			if (p.style != null && p.style.CanDesireLookChange)
			{
				p.style.RequestLookChange();
			}
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000BCB34 File Offset: 0x000BAD34
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -100)]
		private static void DarklightAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (intVec.InBounds(currentMap))
				{
					currentMap.debugDrawer.FlashCell(intVec, DarklightUtility.IsDarklightAt(intVec, currentMap) ? 0.5f : 0f, null, 100);
				}
			}
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x000BCBB8 File Offset: 0x000BADB8
		[DebugAction("Ideoligion", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void MaxDevelopmentPoints()
		{
			if (Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				Faction.OfPlayer.ideos.FluidIdeo.development.TryAddDevelopmentPoints(Faction.OfPlayer.ideos.FluidIdeo.development.NextReformationDevelopmentPoints);
			}
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x000BCC09 File Offset: 0x000BAE09
		[DebugAction("Ideoligion", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void AddDevelopmentPoint()
		{
			if (Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				Faction.OfPlayer.ideos.FluidIdeo.development.TryAddDevelopmentPoints(1);
			}
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x000BCC37 File Offset: 0x000BAE37
		[DebugAction("Ideoligion", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresIdeology = true)]
		private static void ClearDevelopmentPoints()
		{
			if (Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				Faction.OfPlayer.ideos.FluidIdeo.development.ResetDevelopmentPoints();
			}
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x000BCC64 File Offset: 0x000BAE64
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnComplex()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ComplexDef localDef2 in DefDatabase<ComplexDef>.AllDefs)
			{
				ComplexDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
				for (int i = 0; i < 10000; i += 100)
				{
					int localThreatPoints = i;
					debugActionNode.AddChild(new DebugActionNode(i + " threat points", DebugActionType.Action, delegate()
					{
						DebugTool tool = null;
						IntVec3 firstCorner;
						Action <>9__2;
						tool = new DebugTool("first corner...", delegate()
						{
							firstCorner = UI.MouseCell();
							string label = "second corner...";
							Action clickAction;
							if ((clickAction = <>9__2) == null)
							{
								clickAction = (<>9__2 = delegate()
								{
									IntVec3 second = UI.MouseCell();
									CellRect cellRect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
									ComplexSketch sketch = localDef.Worker.GenerateSketch(new IntVec2(cellRect.Width, cellRect.Height), null);
									localDef.Worker.Spawn(sketch, Find.CurrentMap, cellRect.BottomLeft, new float?((float)localThreatPoints), null);
									DebugTools.curTool = tool;
								});
							}
							DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
						}, null);
						DebugTools.curTool = tool;
					}, null));
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x000BCD3C File Offset: 0x000BAF3C
		[DebugAction("Ideoligion", "Generate 200 ritual names", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true, requiresIdeology = true)]
		private static void Generate200RitualNames()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Ritual ritual;
					if ((ritual = (precept as Precept_Ritual)) != null)
					{
						list.Add(new DebugMenuOption(ritual.def.issue.LabelCap + ": " + ritual.LabelCap, DebugMenuOptionMode.Action, delegate()
						{
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < 200; i++)
							{
								stringBuilder.AppendLine(ritual.GenerateNameRaw());
							}
							Log.Message(stringBuilder.ToString());
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000434 RID: 1076
	public static class DebugActionsIncidents
	{
		// Token: 0x06001FB7 RID: 8119 RVA: 0x000BCE4C File Offset: 0x000BB04C
		[DebugAction("Incidents", "Do trade caravan arrival...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DoTradeCaravanSpecific()
		{
			DebugActionsIncidents.<>c__DisplayClass0_0 CS$<>8__locals1 = new DebugActionsIncidents.<>c__DisplayClass0_0();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			CS$<>8__locals1.incidentDef = IncidentDefOf.TraderCaravanArrival;
			CS$<>8__locals1.target = Find.CurrentMap;
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugActionsIncidents.<>c__DisplayClass0_1 CS$<>8__locals2 = new DebugActionsIncidents.<>c__DisplayClass0_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.faction = enumerator.Current;
					if (CS$<>8__locals2.faction.def.caravanTraderKinds != null && CS$<>8__locals2.faction.def.caravanTraderKinds.Any<TraderKindDef>())
					{
						list.Add(new DebugMenuOption(CS$<>8__locals2.faction.Name, DebugMenuOptionMode.Action, delegate()
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							using (List<TraderKindDef>.Enumerator enumerator2 = CS$<>8__locals2.faction.def.caravanTraderKinds.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									DebugActionsIncidents.<>c__DisplayClass0_2 CS$<>8__locals3 = new DebugActionsIncidents.<>c__DisplayClass0_2();
									CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
									CS$<>8__locals3.traderKind = enumerator2.Current;
									string text = CS$<>8__locals3.traderKind.label;
									IncidentParms parms = StorytellerUtility.DefaultParmsNow(CS$<>8__locals2.CS$<>8__locals1.incidentDef.category, CS$<>8__locals2.CS$<>8__locals1.target);
									parms.faction = CS$<>8__locals2.faction;
									parms.traderKind = CS$<>8__locals3.traderKind;
									if (!CS$<>8__locals2.CS$<>8__locals1.incidentDef.Worker.CanFireNow(parms))
									{
										text += " [NO]";
									}
									list2.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
									{
										IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.incidentDef.category, CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.target);
										if (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.incidentDef.pointsScaleable)
										{
											incidentParms = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain).GenerateParms(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.incidentDef.category, parms.target);
										}
										incidentParms.faction = CS$<>8__locals3.CS$<>8__locals2.faction;
										incidentParms.traderKind = CS$<>8__locals3.traderKind;
										CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.incidentDef.Worker.TryExecute(incidentParms);
									}));
								}
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
						}));
					}
				}
			}
			if (list.Count == 0)
			{
				Messages.Message("No valid factions found for trade caravans", MessageTypeDefOf.RejectInput, false);
				return;
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x000BCF40 File Offset: 0x000BB140
		[DebugAction("Incidents", "Execute raid with points...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> ExecuteRaidWithPoints()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (float localP2 in DebugActionsUtility.PointsOptions(true))
			{
				float localP = localP2;
				list.Add(new DebugActionNode(localP.ToString() + " points", DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.points = localP;
						IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
					}
				});
			}
			return list;
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000BCFD4 File Offset: 0x000BB1D4
		[DebugAction("Incidents", "Execute raid with faction...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithFaction()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Func<RaidStrategyDef, bool> <>9__3;
			Func<PawnsArrivalModeDef, bool> <>9__4;
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate()
				{
					parms.faction = localFac;
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float num in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = num;
						list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
						{
							parms.points = localPoints;
							IEnumerable<RaidStrategyDef> allDefs = DefDatabase<RaidStrategyDef>.AllDefs;
							Func<RaidStrategyDef, bool> predicate;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((RaidStrategyDef s) => s.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat)));
							}
							List<RaidStrategyDef> source = allDefs.Where(predicate).ToList<RaidStrategyDef>();
							parms.raidStrategy = source.RandomElement<RaidStrategyDef>();
							if (parms.raidStrategy != null)
							{
								IEnumerable<PawnsArrivalModeDef> allDefs2 = DefDatabase<PawnsArrivalModeDef>.AllDefs;
								Func<PawnsArrivalModeDef, bool> predicate2;
								if ((predicate2 = <>9__4) == null)
								{
									predicate2 = (<>9__4 = ((PawnsArrivalModeDef a) => a.Worker.CanUseWith(parms) && parms.raidStrategy.arriveModes.Contains(a)));
								}
								List<PawnsArrivalModeDef> source2 = allDefs2.Where(predicate2).ToList<PawnsArrivalModeDef>();
								parms.raidArrivalMode = source2.RandomElement<PawnsArrivalModeDef>();
							}
							DebugActionsIncidents.DoRaid(parms);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000BD0E0 File Offset: 0x000BB2E0
		[DebugAction("Incidents", "Execute raid with specifics...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithSpecifics()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Action <>9__4;
			Action <>9__6;
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate()
				{
					parms.faction = localFac;
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float num in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = num;
						list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
						{
							parms.points = localPoints;
							List<DebugMenuOption> list3 = new List<DebugMenuOption>();
							foreach (RaidStrategyDef localStrat2 in DefDatabase<RaidStrategyDef>.AllDefs)
							{
								RaidStrategyDef localStrat = localStrat2;
								string text = localStrat.defName;
								if (!localStrat.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat))
								{
									text += " [NO]";
								}
								list3.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
								{
									parms.raidStrategy = localStrat;
									List<DebugMenuOption> list4 = new List<DebugMenuOption>();
									List<DebugMenuOption> list5 = list4;
									string label = "-Random-";
									DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
									Action method;
									if ((method = <>9__4) == null)
									{
										method = (<>9__4 = delegate()
										{
											DebugActionsIncidents.DoRaid(parms);
										});
									}
									list5.Add(new DebugMenuOption(label, mode, method));
									foreach (PawnsArrivalModeDef localArrival2 in DefDatabase<PawnsArrivalModeDef>.AllDefs)
									{
										PawnsArrivalModeDef localArrival = localArrival2;
										string text2 = localArrival.defName;
										if (!localArrival.Worker.CanUseWith(parms) || !localStrat.arriveModes.Contains(localArrival))
										{
											text2 += " [NO]";
										}
										list4.Add(new DebugMenuOption(text2, DebugMenuOptionMode.Action, delegate()
										{
											parms.raidArrivalMode = localArrival;
											if (ModsConfig.BiotechActive)
											{
												List<DebugMenuOption> list6 = new List<DebugMenuOption>();
												List<DebugMenuOption> list7 = list6;
												string label2 = "-Random-";
												DebugMenuOptionMode mode2 = DebugMenuOptionMode.Action;
												Action method2;
												if ((method2 = <>9__6) == null)
												{
													method2 = (<>9__6 = delegate()
													{
														DebugActionsIncidents.DoRaid(parms);
													});
												}
												list7.Add(new DebugMenuOption(label2, mode2, method2));
												foreach (RaidAgeRestrictionDef localAgeRestriction2 in DefDatabase<RaidAgeRestrictionDef>.AllDefs)
												{
													RaidAgeRestrictionDef localAgeRestriction = localAgeRestriction2;
													string text3 = localAgeRestriction.defName;
													if (!localAgeRestriction.Worker.CanUseWith(parms))
													{
														text3 += " [NO]";
													}
													list6.Add(new DebugMenuOption(text3, DebugMenuOptionMode.Action, delegate()
													{
														parms.raidAgeRestriction = localAgeRestriction;
														DebugActionsIncidents.DoRaid(parms);
													}));
												}
												Find.WindowStack.Add(new Dialog_DebugOptionListLister(list6));
												return;
											}
											DebugActionsIncidents.DoRaid(parms);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x000BD1EC File Offset: 0x000BB3EC
		[DebugActionYielder]
		private static IEnumerable<DebugActionNode> IncidentsYielder()
		{
			yield return DebugActionsIncidents.GetIncidentDebugAction("Do incident", 1);
			yield return DebugActionsIncidents.GetIncidentDebugAction("Do incident x10", 10);
			yield return DebugActionsIncidents.GetIncidentWithPointsDebugAction();
			yield break;
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x000BD1F8 File Offset: 0x000BB3F8
		private static DebugActionNode GetIncidentDebugAction(string name, int iterations)
		{
			return new DebugActionNode(name, DebugActionType.Action, null, null)
			{
				category = "Incidents",
				labelGetter = (() => name + " (" + DebugActionsIncidents.GetIncidentTargetLabel() + ")..."),
				visibilityGetter = new Func<bool>(DebugActionsIncidents.YielderOptionVisible),
				childGetter = delegate()
				{
					List<DebugActionNode> list = new List<DebugActionNode>();
					foreach (IncidentDef localDef2 in from x in DefDatabase<IncidentDef>.AllDefs
					orderby x.defName
					select x)
					{
						IncidentDef localDef = localDef2;
						IIncidentTarget target = DebugActionsIncidents.GetTarget();
						if (target == null || localDef.TargetAllowed(target))
						{
							DebugActionNode child = new DebugActionNode(localDef.defName, DebugActionType.Action, delegate()
							{
								IIncidentTarget target2 = DebugActionsIncidents.GetTarget();
								if (target2 == null || !localDef.TargetAllowed(target2))
								{
									return;
								}
								IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(localDef.category, target2);
								if (!localDef.Worker.CanFireNow(incidentParms))
								{
									return;
								}
								if (localDef.pointsScaleable)
								{
									incidentParms = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain).GenerateParms(localDef.category, incidentParms.target);
								}
								for (int i = 0; i < iterations; i++)
								{
									localDef.Worker.TryExecute(incidentParms);
								}
							}, null);
							child.labelGetter = delegate()
							{
								string text = child.label;
								IIncidentTarget target2 = DebugActionsIncidents.GetTarget();
								if (target2 != null)
								{
									IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target2);
									if (!localDef.TargetAllowed(target2) || !localDef.Worker.CanFireNow(parms))
									{
										text += " [NO]";
									}
								}
								return text;
							};
							list.Add(child);
						}
					}
					return list;
				}
			};
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x000BD268 File Offset: 0x000BB468
		private static DebugActionNode GetIncidentWithPointsDebugAction()
		{
			DebugActionNode debugActionNode = new DebugActionNode("Do incident w/ points", DebugActionType.Action, null, null);
			debugActionNode.category = "Incidents";
			debugActionNode.labelGetter = (() => "Do incident w/ points (" + DebugActionsIncidents.GetIncidentTargetLabel() + ")...");
			debugActionNode.visibilityGetter = new Func<bool>(DebugActionsIncidents.YielderOptionVisible);
			debugActionNode.childGetter = delegate()
			{
				List<DebugActionNode> list = new List<DebugActionNode>();
				foreach (IncidentDef localDef2 in from x in DefDatabase<IncidentDef>.AllDefs
				where x.pointsScaleable
				select x into y
				orderby y.defName
				select y)
				{
					IncidentDef localDef = localDef2;
					IIncidentTarget target = DebugActionsIncidents.GetTarget();
					if (target == null || localDef.TargetAllowed(target))
					{
						DebugActionNode child = new DebugActionNode(localDef.defName, DebugActionType.Action, null, null);
						foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
						{
							float localPoints = localPoints2;
							DebugActionNode grandchild = new DebugActionNode(localPoints + " points", DebugActionType.Action, delegate()
							{
								IIncidentTarget target2 = DebugActionsIncidents.GetTarget();
								if (target2 == null || !localDef.TargetAllowed(target2))
								{
									return;
								}
								IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(localDef.category, target2);
								incidentParms.points = localPoints;
								localDef.Worker.TryExecute(incidentParms);
							}, null);
							grandchild.labelGetter = delegate()
							{
								string text = grandchild.label;
								IIncidentTarget target2 = DebugActionsIncidents.GetTarget();
								if (target2 != null)
								{
									if (!localDef.TargetAllowed(target2))
									{
										text += "[NO]";
									}
									else
									{
										IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(localDef.category, target2);
										incidentParms.points = localPoints;
										if (!localDef.Worker.CanFireNow(incidentParms))
										{
											text += " [NO]";
										}
									}
								}
								return text;
							};
							child.AddChild(grandchild);
						}
						child.labelGetter = delegate()
						{
							string text = child.label;
							IIncidentTarget target2 = DebugActionsIncidents.GetTarget();
							if (target2 != null)
							{
								if (!localDef.TargetAllowed(target2))
								{
									text += " [NO]";
								}
								else
								{
									IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target2);
									if (!localDef.Worker.CanFireNow(parms))
									{
										text += " [NO]";
									}
								}
							}
							return text;
						};
						list.Add(child);
					}
				}
				return list;
			};
			return debugActionNode;
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x000BD2EC File Offset: 0x000BB4EC
		private static void DoRaid(IncidentParms parms)
		{
			IncidentDef incidentDef;
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				incidentDef = IncidentDefOf.RaidEnemy;
			}
			else
			{
				incidentDef = IncidentDefOf.RaidFriendly;
			}
			incidentDef.Worker.TryExecute(parms);
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x000BD328 File Offset: 0x000BB528
		private static bool YielderOptionVisible()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			IIncidentTarget target = DebugActionsIncidents.GetTarget();
			if (target == null)
			{
				return false;
			}
			bool flag = target is World;
			bool flag2 = target is WorldObject;
			if (WorldRendererUtility.WorldRenderedNow)
			{
				return flag || flag2;
			}
			return !flag && !flag2;
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000BD374 File Offset: 0x000BB574
		private static IIncidentTarget GetTarget()
		{
			IIncidentTarget incidentTarget = WorldRendererUtility.WorldRenderedNow ? (Find.WorldSelector.SingleSelectedObject as IIncidentTarget) : null;
			if (incidentTarget == null && WorldRendererUtility.WorldRenderedNow)
			{
				incidentTarget = Find.World;
			}
			if (incidentTarget == null)
			{
				incidentTarget = Find.CurrentMap;
			}
			return incidentTarget;
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x000BD3B8 File Offset: 0x000BB5B8
		private static string GetIncidentTargetLabel()
		{
			IIncidentTarget target = DebugActionsIncidents.GetTarget();
			if (target == null)
			{
				return "null target";
			}
			if (target is Map)
			{
				return "Map";
			}
			if (target is World)
			{
				return "World";
			}
			if (target is Caravan)
			{
				return ((Caravan)target).LabelCap;
			}
			return target.ToString();
		}
	}
}

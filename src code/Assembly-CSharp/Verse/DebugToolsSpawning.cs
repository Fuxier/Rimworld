using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000440 RID: 1088
	public static class DebugToolsSpawning
	{
		// Token: 0x060020CA RID: 8394 RVA: 0x000C621C File Offset: 0x000C441C
		[DebugAction("Spawning", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> SpawnPawn()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap, null, null)
				{
					category = DebugToolsSpawning.GetCategoryForPawnKind(localKindDef),
					action = delegate()
					{
						Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
						Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, faction);
						GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						DebugToolsSpawning.PostPawnSpawn(pawn);
					}
				});
			}
			return list;
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x000C62D8 File Offset: 0x000C44D8
		[DebugAction("Spawning", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresBiotech = true)]
		private static List<DebugActionNode> SpawnNewborn()
		{
			return DebugToolsSpawning.SpawnAtDevelopmentalStages(DevelopmentalStage.Newborn);
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x000C62E0 File Offset: 0x000C44E0
		[DebugAction("Spawning", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresBiotech = true)]
		private static List<DebugActionNode> SpawnChild()
		{
			return DebugToolsSpawning.SpawnAtDevelopmentalStages(DevelopmentalStage.Child);
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x000C62E8 File Offset: 0x000C44E8
		private static List<DebugActionNode> SpawnAtDevelopmentalStages(DevelopmentalStage stages)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap, null, null)
				{
					category = DebugToolsSpawning.GetCategoryForPawnKind(localKindDef),
					action = delegate()
					{
						Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
						Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(localKindDef, faction, PawnGenerationContext.NonPlayer, -1, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, stages, null, null, null, false));
						GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						DebugToolsSpawning.PostPawnSpawn(pawn);
					}
				});
			}
			return list;
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000C63BC File Offset: 0x000C45BC
		private static string GetCategoryForPawnKind(PawnKindDef kindDef)
		{
			if (kindDef.RaceProps.Humanlike)
			{
				return "Humanlike";
			}
			if (kindDef.RaceProps.Insect)
			{
				return "Insect";
			}
			if (kindDef.RaceProps.IsMechanoid)
			{
				return "Mechanoid";
			}
			if (kindDef.RaceProps.Animal)
			{
				return "Animal";
			}
			return "Other";
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000C641C File Offset: 0x000C461C
		private static void PostPawnSpawn(Pawn pawn)
		{
			if (pawn.Spawned && pawn.Faction != null && pawn.Faction != Faction.OfPlayer)
			{
				Lord lord = null;
				if (pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction).Any((Pawn p) => p != pawn))
				{
					lord = ((Pawn)GenClosest.ClosestThing_Global(pawn.Position, pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction), 99999f, (Thing p) => p != pawn && ((Pawn)p).GetLord() != null, null)).GetLord();
				}
				if (lord == null)
				{
					LordJob_DefendPoint lordJob = new LordJob_DefendPoint(pawn.Position, null, false, true);
					lord = LordMaker.MakeNewLord(pawn.Faction, lordJob, Find.CurrentMap, null);
				}
				lord.AddPawn(pawn);
			}
			pawn.Rotation = Rot4.South;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x000C6540 File Offset: 0x000C4740
		[DebugAction("Spawning", "Spawn thing", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> TryPlaceNearThing()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, false)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x000C6574 File Offset: 0x000C4774
		[DebugAction("Spawning", "Spawn thing with style", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TryPlaceNearThingWithStyle()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from d in DefDatabase<ThingDef>.AllDefs
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				if (!localDef.randomStyle.NullOrEmpty<ThingStyleChance>() || DefDatabase<StyleCategoryDef>.AllDefs.Any((StyleCategoryDef s) => s.GetStyleForThingDef(localDef, null) != null))
				{
					DebugActionNode debugActionNode = new DebugActionNode(localDef.LabelCap, DebugActionType.Action, null, null);
					debugActionNode.AddChild(new DebugActionNode("Standard", DebugActionType.ToolMap, delegate()
					{
						DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, null, true, null);
					}, null));
					IEnumerable<StyleCategoryDef> allDefs = DefDatabase<StyleCategoryDef>.AllDefs;
					Func<StyleCategoryDef, bool> predicate;
					Func<StyleCategoryDef, bool> <>9__4;
					if ((predicate = <>9__4) == null)
					{
						Predicate<ThingDefStyle> <>9__5;
						predicate = (<>9__4 = delegate(StyleCategoryDef x)
						{
							List<ThingDefStyle> thingDefStyles = x.thingDefStyles;
							Predicate<ThingDefStyle> predicate2;
							if ((predicate2 = <>9__5) == null)
							{
								predicate2 = (<>9__5 = ((ThingDefStyle y) => y.ThingDef == localDef));
							}
							return thingDefStyles.Any(predicate2);
						});
					}
					using (IEnumerator<StyleCategoryDef> enumerator2 = allDefs.Where(predicate).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							StyleCategoryDef cat = enumerator2.Current;
							StyleCategoryDef cat2 = cat;
							debugActionNode.AddChild(new DebugActionNode(cat2.defName, DebugActionType.ToolMap, delegate()
							{
								DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, cat.GetStyleForThingDef(localDef, null), true, null);
							}, null));
						}
					}
					if (localDef.randomStyle != null)
					{
						foreach (ThingStyleChance style2 in localDef.randomStyle)
						{
							ThingStyleChance style = style2;
							debugActionNode.AddChild(new DebugActionNode(style.StyleDef.defName, DebugActionType.ToolMap, delegate()
							{
								DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, style.StyleDef, true, null);
							}, null));
						}
					}
					list.Add(debugActionNode);
				}
			}
			if (list.Count == 0)
			{
				list.Add(new DebugActionNode("No styleable things", DebugActionType.Action, delegate()
				{
				}, null));
			}
			return list;
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x000C67F4 File Offset: 0x000C49F4
		[DebugAction("Spawning", "Spawn unminified thing", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TryPlaceMinifiedThing()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsUnminified()
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x000C6824 File Offset: 0x000C4A24
		[DebugAction("Spawning", "Spawn full thing stack", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> TryPlaceNearFullStack()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(-1, false)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x000C6856 File Offset: 0x000C4A56
		[DebugAction("Spawning", "Spawn stack of 25", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceNearStacksOf25()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, false)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000C6889 File Offset: 0x000C4A89
		[DebugAction("Spawning", "Spawn stack of 75", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceNearStacksOf75()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(75, false)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000C68BC File Offset: 0x000C4ABC
		[DebugAction("Spawning", "Try place direct thing", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceDirectThing()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, true)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000C68EE File Offset: 0x000C4AEE
		[DebugAction("Spawning", "Try place direct full stack", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceDirectFullStack()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(-1, true)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000C6920 File Offset: 0x000C4B20
		[DebugAction("Spawning", "Try place direct stack of 25", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> TryPlaceDirectStackOf25()
		{
			return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, true)
			orderby x.label
			select x).ToList<DebugActionNode>();
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000C6954 File Offset: 0x000C4B54
		[DebugAction("Spawning", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnWeapon()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.equipmentType == EquipmentType.Primary
			select def into d
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, null, true, null);
				}, null));
			}
			return list;
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000C6A1C File Offset: 0x000C4C1C
		[DebugAction("Spawning", "Spawn apparel", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnApparel()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.IsApparel
			select def into d
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, null, true, null);
				}, null));
			}
			return list;
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000C6AE4 File Offset: 0x000C4CE4
		[DebugAction("Spawning", "Try spawn stack of market value...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TryPlaceNearMarketValue()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (float num in DebugToolsSpawning.MarketValues)
			{
				DebugActionNode debugActionNode = new DebugActionNode(num.ToStringMoney(null), DebugActionType.Action, null, null);
				foreach (DebugActionNode child in DebugThingPlaceHelper.TryPlaceOptionsForBaseMarketValue(num, false))
				{
					debugActionNode.AddChild(child);
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000C6B70 File Offset: 0x000C4D70
		[DebugAction("Spawning", "Spawn meal with specifics...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CreateMealWithSpecifics()
		{
			DebugToolsSpawning.<>c__DisplayClass19_0 CS$<>8__locals1 = new DebugToolsSpawning.<>c__DisplayClass19_0();
			IEnumerable<ThingDef> enumerable = from x in DefDatabase<ThingDef>.AllDefs
			where x.IsNutritionGivingIngestible && x.ingestible.IsMeal
			select x;
			CS$<>8__locals1.ingredientDefs = from x in DefDatabase<ThingDef>.AllDefs
			where x.IsNutritionGivingIngestible && x.ingestible.HumanEdible && !x.ingestible.IsMeal && !x.IsCorpse
			select x;
			CS$<>8__locals1.mealDef = null;
			CS$<>8__locals1.ingredients = new List<ThingDef>();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<ThingDef> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef d = enumerator.Current;
					list.Add(new DebugMenuOption(d.defName, DebugMenuOptionMode.Action, delegate()
					{
						CS$<>8__locals1.mealDef = d;
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(CS$<>8__locals1.<CreateMealWithSpecifics>g__GetIngredientOptions|2()));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000C6C6C File Offset: 0x000C4E6C
		[DebugAction("Spawning", "Spawn thing with wipe mode", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static DebugActionNode SpawnThingWithWipeMode()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			WipeMode[] array = (WipeMode[])Enum.GetValues(typeof(WipeMode));
			for (int i = 0; i < array.Length; i++)
			{
				WipeMode localWipeMode2 = array[i];
				WipeMode localWipeMode = localWipeMode2;
				debugActionNode.AddChild(new DebugActionNode(localWipeMode2.ToString(), DebugActionType.Action, null, null)
				{
					childGetter = (() => DebugThingPlaceHelper.SpawnOptions(localWipeMode))
				});
			}
			return debugActionNode;
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000C6CE9 File Offset: 0x000C4EE9
		private static IEnumerable<float> PointsMechCluster()
		{
			for (float points = 50f; points <= 10000f; points += 50f)
			{
				yield return points;
			}
			yield break;
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x000C6CF4 File Offset: 0x000C4EF4
		[DebugAction("Spawning", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresRoyalty = true)]
		private static DebugActionNode SpawnMechCluster()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			foreach (float localPoints2 in DebugToolsSpawning.PointsMechCluster())
			{
				float localPoints = localPoints2;
				DebugActionNode debugActionNode2 = new DebugActionNode(localPoints.ToString() + " points", DebugActionType.Action, null, null);
				debugActionNode2.AddChild(new DebugActionNode("In pods, click place", DebugActionType.ToolMap, delegate()
				{
					MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
					MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch, true, false, null);
				}, null));
				debugActionNode2.AddChild(new DebugActionNode("In pods, autoplace", DebugActionType.ToolMap, delegate()
				{
					MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
					MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch, 100, 0f), Find.CurrentMap, sketch, true, false, null);
				}, null));
				debugActionNode2.AddChild(new DebugActionNode("Direct spawn, click place", DebugActionType.ToolMap, delegate()
				{
					MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
					MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch, false, false, null);
				}, null));
				debugActionNode2.AddChild(new DebugActionNode("Direct spawn, autoplace", DebugActionType.Action, delegate()
				{
					MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
					MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch, 100, 0f), Find.CurrentMap, sketch, false, false, null);
				}, null));
				debugActionNode.AddChild(debugActionNode2);
			}
			debugActionNode.visibilityGetter = (() => Faction.OfMechanoids != null);
			return debugActionNode;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000C6E1C File Offset: 0x000C501C
		[DebugAction("Spawning", "Make filth x100", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void MakeFilthx100()
		{
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = UI.MouseCell() + GenRadial.RadialPattern[i];
				if (c.InBounds(Find.CurrentMap) && c.WalkableByAny(Find.CurrentMap))
				{
					FilthMaker.TryMakeFilth(c, Find.CurrentMap, ThingDefOf.Filth_Dirt, 2, FilthSourceFlags.None, true);
					FleckMaker.ThrowMetaPuff(c.ToVector3Shifted(), Find.CurrentMap);
				}
			}
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000C6E8C File Offset: 0x000C508C
		[DebugAction("Spawning", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnFactionLeader()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				if (localFac.leader != null)
				{
					list.Add(new FloatMenuOption(localFac.Name + " - " + localFac.leader.Name.ToStringFull, delegate()
					{
						GenSpawn.Spawn(localFac.leader, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000C6F54 File Offset: 0x000C5154
		[DebugAction("Spawning", "Spawn world pawn...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Action<Pawn> act = delegate(Pawn p)
			{
				List<DebugMenuOption> list2 = new List<DebugMenuOption>();
				IEnumerable<PawnKindDef> allDefs = DefDatabase<PawnKindDef>.AllDefs;
				Func<PawnKindDef, bool> <>9__1;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((PawnKindDef x) => x.race == p.def));
				}
				foreach (PawnKindDef kLocal2 in allDefs.Where(predicate))
				{
					PawnKindDef kLocal = kLocal2;
					list2.Add(new DebugMenuOption(kLocal.defName, DebugMenuOptionMode.Tool, delegate()
					{
						PawnGenerationRequest request = new PawnGenerationRequest(kLocal, p.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false);
						PawnGenerator.RedressPawn(p, request);
						GenSpawn.Spawn(p, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						DebugTools.curTool = null;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
			};
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				Pawn pLocal = pawn;
				list.Add(new DebugMenuOption(pawn.LabelShort, DebugMenuOptionMode.Action, delegate()
				{
					act(pLocal);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x000C701C File Offset: 0x000C521C
		[DebugAction("Spawning", "Spawn thing set", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> SpawnThingSet()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingSetMakerDef localGen2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localGen = localGen2;
				list.Add(new DebugActionNode(localGen.defName, DebugActionType.ToolMap, delegate()
				{
					if (!UI.MouseCell().InBounds(Find.CurrentMap))
					{
						return;
					}
					StringBuilder stringBuilder = new StringBuilder();
					string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localGen.debugParams);
					List<Thing> list2 = localGen.root.Generate(localGen.debugParams);
					stringBuilder.Append(string.Concat(new object[]
					{
						localGen.defName,
						" generated ",
						list2.Count,
						" things"
					}));
					if (!nonNullFieldsDebugInfo.NullOrEmpty())
					{
						stringBuilder.Append(" (used custom debug params: " + nonNullFieldsDebugInfo + ")");
					}
					stringBuilder.AppendLine(":");
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < list2.Count; i++)
					{
						stringBuilder.AppendLine("   - " + list2[i].LabelCap);
						num += list2[i].MarketValue * (float)list2[i].stackCount;
						if (!(list2[i] is Pawn))
						{
							num2 += list2[i].GetStatValue(StatDefOf.Mass, true, -1) * (float)list2[i].stackCount;
						}
						if (!GenPlace.TryPlaceThing(list2[i], UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4)))
						{
							list2[i].Destroy(DestroyMode.Vanish);
						}
					}
					stringBuilder.AppendLine("Total market value: " + num.ToString("0.##"));
					stringBuilder.AppendLine("Total mass: " + num2.ToStringMass());
					Log.Message(stringBuilder.ToString());
				}, null));
			}
			return list;
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000C709C File Offset: 0x000C529C
		[DebugAction("Spawning", "Trigger effecter...", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TriggerEffecter()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (EffecterDef localDef2 in DefDatabase<EffecterDef>.AllDefs)
			{
				EffecterDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					Effecter effecter = localDef.Spawn();
					effecter.Trigger(new TargetInfo(UI.MouseCell(), Find.CurrentMap, false), new TargetInfo(UI.MouseCell(), Find.CurrentMap, false), -1);
					effecter.Cleanup();
				}, null));
			}
			return list;
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000C711C File Offset: 0x000C531C
		[DebugAction("Spawning", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static DebugActionNode SpawnShuttle()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			debugActionNode.AddChild(new DebugActionNode("Incoming", DebugActionType.ToolMap, delegate()
			{
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, ThingMaker.MakeThing(ThingDefOf.Shuttle, null)), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}, null));
			debugActionNode.AddChild(new DebugActionNode("Crashing", DebugActionType.ToolMap, delegate()
			{
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleCrashing, ThingMaker.MakeThing(ThingDefOf.ShuttleCrashed, null)), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}, null));
			debugActionNode.AddChild(new DebugActionNode("Stationary", DebugActionType.ToolMap, delegate()
			{
				GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Shuttle, null), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}, null));
			return debugActionNode;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000C71C4 File Offset: 0x000C53C4
		[DebugAction("Spawning", null, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnRandomCaravan()
		{
			int num = GenWorld.MouseTile(false);
			if (Find.WorldGrid[num].biome.impassable)
			{
				return;
			}
			List<Pawn> list = new List<Pawn>();
			int num2 = Rand.RangeInclusive(1, 10);
			for (int i = 0; i < num2; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				list.Add(pawn);
				if (!pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					ThingDef thingDef = (from def in DefDatabase<ThingDef>.AllDefs
					where def.IsWeapon && !def.weaponTags.NullOrEmpty<string>() && (def.weaponTags.Contains("SimpleGun") || def.weaponTags.Contains("IndustrialGunAdvanced") || def.weaponTags.Contains("SpacerGun") || def.weaponTags.Contains("MedievalMeleeAdvanced") || def.weaponTags.Contains("NeolithicRangedBasic") || def.weaponTags.Contains("NeolithicRangedDecent") || def.weaponTags.Contains("NeolithicRangedHeavy"))
					select def).RandomElementWithFallback(null);
					pawn.equipment.AddEquipment((ThingWithComps)ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef)));
				}
			}
			int num3 = Rand.RangeInclusive(-4, 10);
			for (int j = 0; j < num3; j++)
			{
				Pawn item = PawnGenerator.GeneratePawn((from d in DefDatabase<PawnKindDef>.AllDefs
				where d.RaceProps.Animal && d.RaceProps.wildness < 1f
				select d).RandomElement<PawnKindDef>(), Faction.OfPlayer);
				list.Add(item);
			}
			Caravan caravan = CaravanMaker.MakeCaravan(list, Faction.OfPlayer, num, true);
			List<Thing> list2 = ThingSetMakerDefOf.DebugCaravanInventory.root.Generate();
			for (int k = 0; k < list2.Count; k++)
			{
				Thing thing = list2[k];
				if (thing.GetStatValue(StatDefOf.Mass, true, -1) * (float)thing.stackCount > caravan.MassCapacity - caravan.MassUsage)
				{
					break;
				}
				CaravanInventoryUtility.GiveThing(caravan, thing);
			}
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000C7360 File Offset: 0x000C5560
		[DebugAction("Spawning", null, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnRandomFactionBase()
		{
			Faction faction;
			if ((from x in Find.FactionManager.AllFactions
			where !x.IsPlayer && !x.Hidden
			select x).TryRandomElement(out faction))
			{
				int num = GenWorld.MouseTile(false);
				if (Find.WorldGrid[num].biome.impassable)
				{
					return;
				}
				Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
				settlement.SetFaction(faction);
				settlement.Tile = num;
				settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
				Find.WorldObjects.Add(settlement);
			}
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000C73FC File Offset: 0x000C55FC
		[DebugAction("Spawning", null, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnSite()
		{
			DebugToolsSpawning.<>c__DisplayClass31_0 CS$<>8__locals1 = new DebugToolsSpawning.<>c__DisplayClass31_0();
			CS$<>8__locals1.tile = GenWorld.MouseTile(false);
			if (CS$<>8__locals1.tile < 0 || Find.World.Impassable(CS$<>8__locals1.tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<SitePartDef> parts = new List<SitePartDef>();
			Action addPart = null;
			Action <>9__1;
			addPart = delegate()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<DebugMenuOption> list2 = list;
				string label = "-Done (" + parts.Count + " parts)-";
				DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
				Action method;
				if ((method = <>9__1) == null)
				{
					method = (<>9__1 = delegate()
					{
						Site site = SiteMaker.TryMakeSite(parts, CS$<>8__locals1.tile, true, null, true, null);
						if (site == null)
						{
							Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
							return;
						}
						Find.WorldObjects.Add(site);
					});
				}
				list2.Add(new DebugMenuOption(label, mode, method));
				foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
				{
					SitePartDef localPart = sitePartDef;
					list.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						parts.Add(localPart);
						addPart();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			};
			addPart();
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000C7484 File Offset: 0x000C5684
		[DebugAction("Spawning", null, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void DestroySite()
		{
			int tileID = GenWorld.MouseTile(false);
			foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(tileID).ToList<WorldObject>())
			{
				worldObject.Destroy();
			}
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000C74E8 File Offset: 0x000C56E8
		[DebugAction("Spawning", null, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnSiteWithPoints()
		{
			DebugToolsSpawning.<>c__DisplayClass33_0 CS$<>8__locals1 = new DebugToolsSpawning.<>c__DisplayClass33_0();
			CS$<>8__locals1.tile = GenWorld.MouseTile(false);
			if (CS$<>8__locals1.tile < 0 || Find.World.Impassable(CS$<>8__locals1.tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<SitePartDef> parts = new List<SitePartDef>();
			Action addPart = null;
			Action <>9__1;
			addPart = delegate()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<DebugMenuOption> list2 = list;
				string label = "-Done (" + parts.Count + " parts)-";
				DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
				Action method;
				if ((method = <>9__1) == null)
				{
					method = (<>9__1 = delegate()
					{
						List<DebugMenuOption> list3 = new List<DebugMenuOption>();
						foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
						{
							float localPoints = localPoints2;
							list3.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
							{
								Site site = SiteMaker.TryMakeSite(parts, CS$<>8__locals1.tile, true, null, true, new float?(localPoints));
								if (site == null)
								{
									Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
									return;
								}
								Find.WorldObjects.Add(site);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
					});
				}
				list2.Add(new DebugMenuOption(label, mode, method));
				foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
				{
					SitePartDef localPart = sitePartDef;
					list.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						parts.Add(localPart);
						addPart();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			};
			addPart();
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000C7570 File Offset: 0x000C5770
		[DebugAction("Spawning", null, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static List<DebugActionNode> SpawnWorldObject()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (WorldObjectDef localDef2 in DefDatabase<WorldObjectDef>.AllDefs)
			{
				WorldObjectDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolWorld, delegate()
				{
					int num = GenWorld.MouseTile(false);
					if (num < 0 || Find.World.Impassable(num))
					{
						Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
						return;
					}
					WorldObject worldObject = WorldObjectMaker.MakeWorldObject(localDef);
					worldObject.Tile = num;
					Find.WorldObjects.Add(worldObject);
				}, null));
			}
			return list;
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000C75F0 File Offset: 0x000C57F0
		[DebugAction("General", "Change camera config", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static List<DebugActionNode> ChangeCameraConfigWorld()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Type localType2 in typeof(WorldCameraConfig).AllSubclasses())
			{
				Type localType = localType2;
				string text = localType.Name;
				if (text.StartsWith("WorldCameraConfig_"))
				{
					text = text.Substring("WorldCameraConfig_".Length);
				}
				list.Add(new DebugActionNode(text, DebugActionType.Action, delegate()
				{
					Find.WorldCameraDriver.config = (WorldCameraConfig)Activator.CreateInstance(localType);
				}, null));
			}
			return list;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000C76A4 File Offset: 0x000C58A4
		[DebugAction("Spawning", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static List<DebugActionNode> SpawnBossgroup()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<BossgroupDef> enumerator = DefDatabase<BossgroupDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugToolsSpawning.<>c__DisplayClass36_0 CS$<>8__locals1 = new DebugToolsSpawning.<>c__DisplayClass36_0();
					CS$<>8__locals1.def = enumerator.Current;
					BossgroupDef localDef = CS$<>8__locals1.def;
					string text = localDef.defName;
					Pawn caller;
					if (!PawnsFinder.AllMaps_FreeColonists.TryRandomElement(out caller) || !localDef.Worker.CanResolve(caller))
					{
						text += " [NO]";
					}
					list.Add(new DebugActionNode(text, DebugActionType.Action, null, null)
					{
						childGetter = delegate()
						{
							List<DebugActionNode> list2 = new List<DebugActionNode>();
							int currentWave = 0;
							GameComponent_Bossgroup component = Current.Game.GetComponent<GameComponent_Bossgroup>();
							if (component != null)
							{
								currentWave = component.NumTimesCalledBossgroup(CS$<>8__locals1.def);
							}
							list2.Add(new DebugActionNode("*Current (times called: " + currentWave + ")", DebugActionType.Action, delegate()
							{
								if (caller == null)
								{
									Messages.Message("No colonist found to call bossgroup.", MessageTypeDefOf.RejectInput, false);
									return;
								}
								localDef.Worker.Resolve(caller.Map, currentWave);
							}, null));
							for (int i = 0; i < CS$<>8__locals1.def.waves.Count; i++)
							{
								int index = i;
								list2.Add(new DebugActionNode("Wave " + index, DebugActionType.Action, delegate()
								{
									if (caller == null)
									{
										Messages.Message("No colonist found to call bossgroup.", MessageTypeDefOf.RejectInput, false);
										return;
									}
									localDef.Worker.Resolve(caller.Map, index);
								}, null));
							}
							return list2;
						}
					});
				}
			}
			return list;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000C7790 File Offset: 0x000C5990
		[DebugAction("Spawning", null, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		public static List<DebugActionNode> AbandonThing()
		{
			return (from t in DebugThingPlaceHelper.TryAbandonOptionsForStackCount()
			orderby t.label
			select t).ToList<DebugActionNode>();
		}

		// Token: 0x040015B0 RID: 5552
		private static readonly float[] MarketValues = new float[]
		{
			1000f,
			10000f,
			100000f
		};
	}
}

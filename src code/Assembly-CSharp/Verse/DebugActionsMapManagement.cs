using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using RimWorld.SketchGen;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000435 RID: 1077
	public static class DebugActionsMapManagement
	{
		// Token: 0x06001FC2 RID: 8130 RVA: 0x000BD40A File Offset: 0x000BB60A
		[DebugAction("Map", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> UseScatterer()
		{
			return DebugTools_MapGen.Options_Scatterers();
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x000BD414 File Offset: 0x000BB614
		[DebugAction("Map", "BaseGen", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> BaseGen()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (string localSymbol2 in (from x in DefDatabase<RuleDef>.AllDefs
			select x.symbol).Distinct<string>())
			{
				string localSymbol = localSymbol2;
				list.Add(new DebugActionNode(localSymbol, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						DebugTool tool = null;
						IntVec3 firstCorner;
						Action <>9__3;
						tool = new DebugTool("first corner...", delegate()
						{
							firstCorner = UI.MouseCell();
							string label = "second corner...";
							Action clickAction;
							if ((clickAction = <>9__3) == null)
							{
								clickAction = (<>9__3 = delegate()
								{
									IntVec3 second = UI.MouseCell();
									CellRect rect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
									RimWorld.BaseGen.BaseGen.globalSettings.map = Find.CurrentMap;
									RimWorld.BaseGen.BaseGen.symbolStack.Push(localSymbol, rect, null);
									RimWorld.BaseGen.BaseGen.Generate();
									DebugTools.curTool = tool;
								});
							}
							DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
						}, null);
						DebugTools.curTool = tool;
					}
				});
			}
			return list;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000BD4BC File Offset: 0x000BB6BC
		[DebugAction("Map", "SketchGen", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> SketchGen()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SketchResolverDef localResolver2 in from x in DefDatabase<SketchResolverDef>.AllDefs
			where x.isRoot
			select x)
			{
				SketchResolverDef localResolver = localResolver2;
				DebugActionNode debugActionNode = new DebugActionNode(localResolver.defName, DebugActionType.Action, null, null);
				if (localResolver == SketchResolverDefOf.Monument || localResolver == SketchResolverDefOf.MonumentRuin)
				{
					new List<DebugMenuOption>();
					for (int i = 1; i <= 60; i++)
					{
						int localIndex = i;
						debugActionNode.AddChild(new DebugActionNode(localIndex.ToString(), DebugActionType.ToolMap, null, null)
						{
							action = delegate()
							{
								RimWorld.SketchGen.ResolveParams parms = default(RimWorld.SketchGen.ResolveParams);
								parms.sketch = new Sketch();
								parms.monumentSize = new IntVec2?(new IntVec2(localIndex, localIndex));
								RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, true, null, null);
							}
						});
					}
				}
				else
				{
					debugActionNode.actionType = DebugActionType.ToolMap;
					debugActionNode.action = delegate()
					{
						RimWorld.SketchGen.ResolveParams parms = default(RimWorld.SketchGen.ResolveParams);
						parms.sketch = new Sketch();
						RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, true, null, null);
					};
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000BD5F0 File Offset: 0x000BB7F0
		[DebugAction("Map", "Set terrain (rect)", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static List<DebugActionNode> SetTerrainRect()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (TerrainDef localDef2 in DefDatabase<TerrainDef>.AllDefs)
			{
				TerrainDef localDef = localDef2;
				Action<CellRect> <>9__1;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						string defName = localDef.defName;
						Action<CellRect> rectAction;
						if ((rectAction = <>9__1) == null)
						{
							rectAction = (<>9__1 = delegate(CellRect rect)
							{
								foreach (IntVec3 c in rect)
								{
									Find.CurrentMap.terrainGrid.SetTerrain(c, localDef);
								}
							});
						}
						DebugToolsGeneral.GenericRectTool(defName, rectAction);
					}
				});
			}
			return list;
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000BD674 File Offset: 0x000BB874
		[DebugAction("Map", "Pollute (rect)", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100, requiresBiotech = true)]
		private static void PolluteRect()
		{
			DebugToolsGeneral.GenericRectTool("Pollute", delegate(CellRect rect)
			{
				foreach (IntVec3 cell in rect)
				{
					Find.CurrentMap.pollutionGrid.SetPolluted(cell, true, false);
				}
			});
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000BD69F File Offset: 0x000BB89F
		[DebugAction("Map", "Unpollute (rect)", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100, requiresBiotech = true)]
		private static void UnpolluteRect()
		{
			DebugToolsGeneral.GenericRectTool("Unpollute", delegate(CellRect rect)
			{
				foreach (IntVec3 cell in rect)
				{
					Find.CurrentMap.pollutionGrid.SetPolluted(cell, false, false);
				}
			});
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000BD6CA File Offset: 0x000BB8CA
		[DebugAction("Map", "Make rock (rect)", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void MakeRock()
		{
			DebugToolsGeneral.GenericRectTool("Make rock", delegate(CellRect rect)
			{
				foreach (IntVec3 loc in rect)
				{
					GenSpawn.Spawn(ThingDefOf.Granite, loc, Find.CurrentMap, WipeMode.Vanish);
				}
			});
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x000BD6F5 File Offset: 0x000BB8F5
		[DebugAction("Map", "Grow pollution (x10 cell)", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200, requiresBiotech = true)]
		private static void PolluteCellTen()
		{
			PollutionUtility.GrowPollutionAt(UI.MouseCell(), Find.CurrentMap, 10, null, false, null);
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x000BD70B File Offset: 0x000BB90B
		[DebugAction("Map", "Grow pollution (x100 cell)", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200, requiresBiotech = true)]
		private static void PolluteCellHundred()
		{
			PollutionUtility.GrowPollutionAt(UI.MouseCell(), Find.CurrentMap, 100, null, false, null);
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x000BD721 File Offset: 0x000BB921
		[DebugAction("Map", "Grow pollution (x1000 cell)", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200, requiresBiotech = true)]
		private static void PolluteCellThousand()
		{
			PollutionUtility.GrowPollutionAt(UI.MouseCell(), Find.CurrentMap, 1000, null, false, null);
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x000BD73C File Offset: 0x000BB93C
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> AddGameCondition()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (GameConditionDef localDef2 in DefDatabase<GameConditionDef>.AllDefs)
			{
				GameConditionDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode(localDef.LabelCap, DebugActionType.Action, null, null);
				debugActionNode.AddChild(new DebugActionNode("Permanent", DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						GameCondition gameCondition = GameConditionMaker.MakeCondition(localDef, -1);
						gameCondition.Permanent = true;
						Find.CurrentMap.GameConditionManager.RegisterCondition(gameCondition);
					}
				});
				for (int i = 2500; i <= 60000; i += 2500)
				{
					int localTicks = i;
					debugActionNode.AddChild(new DebugActionNode(localTicks.ToStringTicksToPeriod(true, false, true, true, false) ?? "", DebugActionType.Action, null, null)
					{
						action = delegate()
						{
							GameCondition gameCondition = GameConditionMaker.MakeCondition(localDef, -1);
							gameCondition.Duration = localTicks;
							Find.CurrentMap.GameConditionManager.RegisterCondition(gameCondition);
						}
					});
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000BD854 File Offset: 0x000BBA54
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> RemoveGameCondition()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (GameConditionDef localDef2 in DefDatabase<GameConditionDef>.AllDefs)
			{
				GameConditionDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.LabelCap, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						GameCondition activeCondition = Find.CurrentMap.gameConditionManager.GetActiveCondition(localDef);
						if (activeCondition != null)
						{
							activeCondition.Duration = 0;
						}
					},
					visibilityGetter = (() => Find.CurrentMap != null && Find.CurrentMap.gameConditionManager.ConditionIsActive(localDef))
				});
			}
			return list;
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000BD8F0 File Offset: 0x000BBAF0
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RefogMap()
		{
			FloodFillerFog.DebugRefogMap(Find.CurrentMap);
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x000BD8FC File Offset: 0x000BBAFC
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> UseGenStep()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Type localGenStep2 in typeof(GenStep).AllSubclassesNonAbstract())
			{
				Type localGenStep = localGenStep2;
				list.Add(new DebugActionNode(localGenStep.Name, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						((GenStep)Activator.CreateInstance(localGenStep)).Generate(Find.CurrentMap, default(GenStepParams));
					}
				});
			}
			return list;
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x000BD990 File Offset: 0x000BBB90
		[DebugAction("Map", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenSection()
		{
			Find.CurrentMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x000BD9AB File Offset: 0x000BBBAB
		[DebugAction("Map", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenAllMapMeshSections()
		{
			Find.CurrentMap.mapDrawer.RegenerateEverythingNow();
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x000BD9BC File Offset: 0x000BBBBC
		[DebugAction("Map", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddSnow()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x000BD9D7 File Offset: 0x000BBBD7
		[DebugAction("Map", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveSnow()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x000BD9F4 File Offset: 0x000BBBF4
		[DebugAction("Map", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearAllSnow()
		{
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Find.CurrentMap.snowGrid.SetDepth(c, 0f);
			}
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x000BDA54 File Offset: 0x000BBC54
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, hideInSubMenu = true)]
		private static void GenerateMap()
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			mapParent.Tile = TileFinder.RandomStartingTile();
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x000BDAAC File Offset: 0x000BBCAC
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void DestroyMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					Current.Game.DeinitAndRemoveMap(map);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x000BDB1C File Offset: 0x000BBD1C
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, hideInSubMenu = true)]
		private static void LeakMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					DebugActionsMapManagement.mapLeak = map;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x000BDB8C File Offset: 0x000BBD8C
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, hideInSubMenu = true)]
		private static void PrintLeakedMap()
		{
			Log.Message(string.Format("Leaked map {0}", DebugActionsMapManagement.mapLeak));
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x000BDBA4 File Offset: 0x000BBDA4
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing, actionType = DebugActionType.ToolMap)]
		private static void Transfer()
		{
			List<Thing> toTransfer = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (!toTransfer.Any<Thing>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					Predicate<IntVec3> <>9__1;
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						for (int j = 0; j < toTransfer.Count; j++)
						{
							Map map;
							IntVec3 center = map.Center;
							map = map;
							int squareRadius = Mathf.Max(map.Size.x, map.Size.z);
							Predicate<IntVec3> validator;
							if ((validator = <>9__1) == null)
							{
								validator = (<>9__1 = ((IntVec3 x) => !x.Fogged(map) && x.Standable(map)));
							}
							IntVec3 center2;
							if (CellFinder.TryFindRandomCellNear(center, map, squareRadius, validator, out center2, -1))
							{
								toTransfer[j].DeSpawn(DestroyMode.Vanish);
								GenPlace.TryPlaceThing(toTransfer[j], center2, map, ThingPlaceMode.Near, null, null, default(Rot4));
							}
							else
							{
								Log.Error("Could not find spawn cell.");
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x000BDC64 File Offset: 0x000BBE64
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void ChangeMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						Current.Game.CurrentMap = map;
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x000BDCE4 File Offset: 0x000BBEE4
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void RegenerateCurrentMap()
		{
			RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
			int tile = Find.CurrentMap.Tile;
			MapParent parent = Find.CurrentMap.Parent;
			IntVec3 size = Find.CurrentMap.Size;
			Current.Game.DeinitAndRemoveMap(Find.CurrentMap);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def);
			Current.Game.CurrentMap = orGenerateMap;
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000BDD6C File Offset: 0x000BBF6C
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void GenerateMapWithCaves()
		{
			int tile = TileFinder.RandomSettlementTileFor(Faction.OfPlayer, false, (int x) => Find.World.HasCaves(x));
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.Parent.Destroy();
			}
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			mapParent.Tile = tile;
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, null);
			Current.Game.CurrentMap = orGenerateMap;
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000BDE1C File Offset: 0x000BC01C
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static List<DebugActionNode> RunMapGenerator()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MapGeneratorDef defLocal2 in DefDatabase<MapGeneratorDef>.AllDefsListForReading)
			{
				MapGeneratorDef defLocal = defLocal2;
				list.Add(new DebugActionNode(defLocal.defName, DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
						mapParent.Tile = (from tile in Enumerable.Range(0, Find.WorldGrid.TilesCount)
						where Find.WorldGrid[tile].biome.canBuildBase
						select tile).RandomElement<int>();
						mapParent.SetFaction(Faction.OfPlayer);
						Find.WorldObjects.Add(mapParent);
						Map currentMap = MapGenerator.GenerateMap(Find.World.info.initialMapSize, mapParent, defLocal, null, null);
						Current.Game.CurrentMap = currentMap;
					}
				});
			}
			return list;
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000BDEA8 File Offset: 0x000BC0A8
		[DebugAction("Map", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void ForceReformInCurrentMap()
		{
			if (Find.CurrentMap != null)
			{
				MapParent mapParent = Find.CurrentMap.Parent;
				List<Pawn> list = new List<Pawn>();
				if (Dialog_FormCaravan.AllSendablePawns(mapParent.Map, true).Any((Pawn x) => x.IsColonist))
				{
					Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(mapParent.Tile), MessageTypeDefOf.NeutralEvent, true);
					Current.Game.CurrentMap = mapParent.Map;
					Dialog_FormCaravan window = new Dialog_FormCaravan(mapParent.Map, true, delegate()
					{
						if (mapParent.HasMap)
						{
							mapParent.Destroy();
						}
					}, true, null);
					Find.WindowStack.Add(window);
					return;
				}
				list.Clear();
				list.AddRange(from x in mapParent.Map.mapPawns.AllPawns
				where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
				select x);
				if (list.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
				{
					CaravanExitMapUtility.ExitMapAndCreateCaravan(list, Faction.OfPlayer, mapParent.Tile, mapParent.Tile, -1, true);
				}
				list.Clear();
				mapParent.Destroy();
			}
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000BE02C File Offset: 0x000BC22C
		[DebugAction("Map", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FillMapWithTrees()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 intVec in currentMap.AllCells)
			{
				if (intVec.Standable(currentMap))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Plant_TreeOak, null), intVec, currentMap, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000BE098 File Offset: 0x000BC298
		[DebugAction("Map", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static void LogMapPollution()
		{
			Log.Message("Polluted (of all possible pollutable cells): " + Find.CurrentMap.pollutionGrid.TotalPollutionPercent.ToStringPercent());
		}

		// Token: 0x040015A3 RID: 5539
		private static Map mapLeak;
	}
}

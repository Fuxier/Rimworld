using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200048C RID: 1164
	public static class CellInspectorDrawer
	{
		// Token: 0x06002338 RID: 9016 RVA: 0x000E1161 File Offset: 0x000DF361
		public static void Update()
		{
			if (!KeyBindingDefOf.ShowCellInspector.IsDown)
			{
				CellInspectorDrawer.active = false;
				return;
			}
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TileInspector, KnowledgeAmount.TinyInteraction);
			CellInspectorDrawer.active = true;
			if (CellInspectorDrawer.ShouldShow() && !WorldRendererUtility.WorldRenderedNow)
			{
				GenUI.RenderMouseoverBracket();
			}
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000E119C File Offset: 0x000DF39C
		public static void OnGUI()
		{
			if (!CellInspectorDrawer.ShouldShow() || Mouse.IsInputBlockedNow)
			{
				return;
			}
			Rect rect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 336f, (float)CellInspectorDrawer.numLines * 24f + 24f);
			CellInspectorDrawer.numLines = 0;
			rect.x += 26f;
			rect.y += 26f;
			if (rect.xMax > (float)UI.screenWidth)
			{
				rect.x -= rect.width + 52f;
			}
			if (rect.yMax > (float)UI.screenHeight)
			{
				rect.y -= rect.height + 52f;
			}
			Find.WindowStack.ImmediateWindow(62348, rect, WindowLayer.Super, new Action(CellInspectorDrawer.FillWindow), true, false, 1f, null);
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x000E1299 File Offset: 0x000DF499
		private static void FillWindow()
		{
			if (!CellInspectorDrawer.ShouldShow())
			{
				return;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			if (WorldRendererUtility.WorldRenderedNow)
			{
				CellInspectorDrawer.DrawWorldInspector();
			}
			else
			{
				CellInspectorDrawer.DrawMapInspector();
			}
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x000E12D4 File Offset: 0x000DF4D4
		private static void DrawMapInspector()
		{
			IntVec3 intVec = UI.MouseCell();
			List<Thing> list = (from thing in intVec.GetThingList(Find.CurrentMap)
			where thing.def.category != ThingCategory.Mote && thing.def.category != ThingCategory.Filth
			select thing).ToList<Thing>();
			if (list.Any<Thing>())
			{
				foreach (Thing thing2 in list)
				{
					CellInspectorDrawer.DrawThingRow(thing2);
				}
			}
			IEnumerable<string> enumerable = from thing in intVec.GetThingList(Find.CurrentMap)
			where thing.def.category == ThingCategory.Filth
			select thing into filth
			select filth.def.label;
			if (enumerable.Any<string>())
			{
				CellInspectorDrawer.DrawRow("Filth_Label".Translate(), enumerable.ToCommaList(false, false).CapitalizeFirst().Truncate(170f, null));
			}
			if (list.Any<Thing>() || enumerable.Any<string>())
			{
				CellInspectorDrawer.DrawDivider();
			}
			Room room = intVec.GetRoom(Find.CurrentMap);
			if (room != null && room.Role != RoomRoleDefOf.None)
			{
				CellInspectorDrawer.DrawHeader(room.GetRoomRoleLabel().CapitalizeFirst());
				foreach (RoomStatDef roomStatDef in DefDatabase<RoomStatDef>.AllDefsListForReading)
				{
					if (!roomStatDef.isHidden || DebugViewSettings.showAllRoomStats)
					{
						float stat = room.GetStat(roomStatDef);
						RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
						CellInspectorDrawer.DrawRow(roomStatDef.LabelCap, (scoreStage == null) ? "" : (scoreStage.label.CapitalizeFirst() + " (" + roomStatDef.ScoreToString(stat) + ")"));
					}
				}
				CellInspectorDrawer.DrawDivider();
			}
			TerrainDef terrain = intVec.GetTerrain(Find.CurrentMap);
			bool flag = intVec.IsPolluted(Find.CurrentMap);
			float fertility = intVec.GetFertility(Find.CurrentMap);
			float temperature = intVec.GetTemperature(Find.CurrentMap);
			float value = Find.CurrentMap.glowGrid.GameGlowAt(intVec, false);
			Zone zone = intVec.GetZone(Find.CurrentMap);
			float depth = Find.CurrentMap.snowGrid.GetDepth(intVec);
			SnowCategory snowCategory = SnowUtility.GetSnowCategory(depth);
			RoofDef roof = intVec.GetRoof(Find.CurrentMap);
			byte b = Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.BlindSmoke);
			byte b2 = Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.ToxGas);
			byte b3 = Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.RotStink);
			float num = BeautyUtility.AverageBeautyPerceptible(intVec, Find.CurrentMap);
			CellInspectorDrawer.DrawRow("Beauty_Label".Translate(), num.ToString("F1"));
			if (zone != null)
			{
				CellInspectorDrawer.DrawRow("Zone_Label".Translate(), zone.label);
			}
			if (roof != null)
			{
				CellInspectorDrawer.DrawRow("Roof_Label".Translate(), roof.LabelCap);
			}
			CellInspectorDrawer.DrawRow("Terrain_Label".Translate(), flag ? "PollutedTerrain".Translate(terrain.label).CapitalizeFirst() : terrain.LabelCap);
			if (depth > 0.03f)
			{
				CellInspectorDrawer.DrawRow("Snow_Label".Translate(), SnowUtility.GetDescription(snowCategory).CapitalizeFirst());
			}
			CellInspectorDrawer.DrawRow("WalkSpeed_Label".Translate(), GenPath.SpeedPercentString((float)Mathf.Max(terrain.pathCost, SnowUtility.MovementTicksAddOn(snowCategory))));
			if ((double)fertility > 0.0001)
			{
				CellInspectorDrawer.DrawRow("Fertility_Label".Translate(), fertility.ToStringPercent());
			}
			CellInspectorDrawer.DrawRow("Temperature_Label".Translate(), temperature.ToStringTemperature("F0"));
			CellInspectorDrawer.DrawRow("LightLevel_Label".Translate(), MouseoverUtility.GetGlowLabelByValue(value));
			if (b > 0)
			{
				CellInspectorDrawer.DrawRow(GasType.BlindSmoke.GetLabel().CapitalizeFirst(), ((float)b / 255f).ToStringPercent("F0"));
			}
			if (b2 > 0)
			{
				CellInspectorDrawer.DrawRow(GasType.ToxGas.GetLabel().CapitalizeFirst(), ((float)b2 / 255f).ToStringPercent("F0"));
			}
			if (b3 > 0)
			{
				CellInspectorDrawer.DrawRow(GasType.RotStink.GetLabel().CapitalizeFirst(), ((float)b3 / 255f).ToStringPercent("F0"));
			}
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000E1774 File Offset: 0x000DF974
		private static void DrawWorldInspector()
		{
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			int num = GenWorld.MouseTile(false);
			Tile tile = Find.WorldGrid[num];
			foreach (WorldObject worldObject in list)
			{
				CellInspectorDrawer.DrawHeader(worldObject.LabelCap);
				WorldObject worldObject2 = worldObject;
				if (worldObject2 != null)
				{
					Settlement settlement;
					if ((settlement = (worldObject2 as Settlement)) != null)
					{
						Settlement settlement2 = settlement;
						if (settlement2.Faction != null)
						{
							CellInspectorDrawer.DrawRow("Faction_Label".Translate(), settlement2.Faction.Name);
							if (settlement2.Faction == Faction.OfPlayer)
							{
								goto IL_2AC;
							}
							if (settlement2.Faction.Hidden)
							{
								CellInspectorDrawer.DrawRow("Relationship_Label".Translate(), settlement2.Faction.PlayerRelationKind.GetLabelCap());
								goto IL_2AC;
							}
							CellInspectorDrawer.DrawRow("Relationship_Label".Translate(), settlement2.Faction.PlayerRelationKind.GetLabelCap() + " (" + settlement2.Faction.PlayerGoodwill.ToStringWithSign() + ")");
							goto IL_2AC;
						}
					}
					Caravan caravan;
					if ((caravan = (worldObject2 as Caravan)) != null)
					{
						Caravan caravan2 = caravan;
						CellInspectorDrawer.DrawRow("CaravanColonists_Label".Translate(), caravan2.pawns.Count((Pawn pawn) => pawn.IsColonist).ToString());
						if (caravan2.pather.Moving)
						{
							if (!caravan2.pather.MovingNow)
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), CaravanBedUtility.AppendUsingBedsLabel("CaravanResting".Translate(), caravan2.beds.GetUsedBedCount()));
							}
							else if (caravan2.pather.ArrivalAction != null)
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), caravan2.pather.ArrivalAction.ReportString);
							}
							else
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), "CaravanTraveling".Translate());
							}
							float num2 = (float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(caravan2, true) / 60000f;
							CellInspectorDrawer.DrawRow("CaravanTTD_Label".Translate(), num2.ToString("0.#"));
						}
						else
						{
							Settlement settlement3 = CaravanVisitUtility.SettlementVisitedNow(caravan2);
							if (settlement3 != null)
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), "CaravanVisiting".Translate(settlement3.Label));
							}
							else
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), "CaravanWaiting".Translate());
							}
						}
					}
				}
				IL_2AC:
				CellInspectorDrawer.DrawDivider();
			}
			CellInspectorDrawer.DrawRow("Biome_Label".Translate(), tile.biome.LabelCap);
			if (!tile.biome.impassable)
			{
				CellInspectorDrawer.DrawRow("Hilliness_Label".Translate(), tile.hilliness.GetLabelCap());
			}
			if (tile.Roads != null)
			{
				CellInspectorDrawer.DrawRow("Road_Label".Translate(), (from rl in tile.Roads
				select rl.road).MaxBy((RoadDef road) => road.priority).LabelCap);
			}
			if (tile.Rivers != null)
			{
				CellInspectorDrawer.DrawRow("River_Label".Translate(), tile.Rivers[0].river.LabelCap);
			}
			if (!Find.World.Impassable(num))
			{
				string info = (WorldPathGrid.CalculatedMovementDifficultyAt(num, false, null, null) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(num, -1, null)).ToString("0.#");
				CellInspectorDrawer.DrawRow("MovementDifficulty_Label".Translate(), info);
			}
			if (ModsConfig.BiotechActive && tile.pollution > 0f)
			{
				CellInspectorDrawer.DrawRow("Pollution_Label".Translate(), GenWorld.GetPollutionDescription(tile.pollution) + " (" + tile.pollution.ToStringPercent() + ")");
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000E1C08 File Offset: 0x000DFE08
		private static bool ShouldShow()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (WorldRendererUtility.WorldRenderedNow)
			{
				if (GenWorld.MouseTile(false) == -1)
				{
					return false;
				}
			}
			else if (Find.CurrentMap != null && (!UI.MouseCell().InBounds(Find.CurrentMap) || UI.MouseCell().Fogged(Find.CurrentMap)))
			{
				return false;
			}
			return CellInspectorDrawer.active;
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000E1C64 File Offset: 0x000DFE64
		private static void DrawThingRow(Thing thing)
		{
			float num = (float)CellInspectorDrawer.numLines * 24f;
			List<object> selectedObjects = Find.Selector.SelectedObjects;
			Rect rect = new Rect(12f, num + 12f, 312f, 24f);
			if (selectedObjects.Contains(thing))
			{
				Widgets.DrawHighlight(rect);
			}
			else if (CellInspectorDrawer.numLines % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			rect = new Rect(24f, num + 12f + 1f, 22f, 22f);
			if (thing is Blueprint || thing is Frame)
			{
				Widgets.DefIcon(rect, thing.def, null, 1f, null, false, null, null, null);
			}
			else if (thing is Pawn || thing is Corpse)
			{
				Widgets.ThingIcon(rect.ExpandedBy(5f), thing, 1f, null, false);
			}
			else
			{
				Widgets.ThingIcon(rect, thing, 1f, null, false);
			}
			rect = new Rect(58f, num + 12f, 370f, 24f);
			Widgets.Label(rect, thing.LabelMouseover);
			CellInspectorDrawer.numLines++;
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000E1DA0 File Offset: 0x000DFFA0
		private static void DrawRow(string label, string info)
		{
			float num = (float)CellInspectorDrawer.numLines * 24f;
			Rect rect = new Rect(12f, num + 12f, 312f, 24f);
			if (CellInspectorDrawer.numLines % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			GUI.color = Color.gray;
			rect = new Rect(24f, num + 12f, 130f, 24f);
			Widgets.Label(rect, label);
			GUI.color = Color.white;
			rect = new Rect(154f, num + 12f, 170f, 24f);
			Widgets.Label(rect, info);
			CellInspectorDrawer.numLines++;
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000E1E50 File Offset: 0x000E0050
		private static void DrawHeader(string text)
		{
			float num = (float)CellInspectorDrawer.numLines * 24f;
			Rect rect = new Rect(12f, num + 12f - 8f, 312f, 28f);
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, text);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			CellInspectorDrawer.numLines++;
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000E1EB8 File Offset: 0x000E00B8
		private static void DrawDivider()
		{
			float num = (float)CellInspectorDrawer.numLines * 24f;
			GUI.color = Color.gray;
			Widgets.DrawLineHorizontal(0f, num + 12f + 12f, 336f);
			GUI.color = Color.white;
			CellInspectorDrawer.numLines++;
		}

		// Token: 0x04001693 RID: 5779
		private static int numLines;

		// Token: 0x04001694 RID: 5780
		private const float DistFromMouse = 26f;

		// Token: 0x04001695 RID: 5781
		private const float LabelColumnWidth = 130f;

		// Token: 0x04001696 RID: 5782
		private const float InfoColumnWidth = 170f;

		// Token: 0x04001697 RID: 5783
		private const float WindowPadding = 12f;

		// Token: 0x04001698 RID: 5784
		private const float ColumnPadding = 12f;

		// Token: 0x04001699 RID: 5785
		private const float LineHeight = 24f;

		// Token: 0x0400169A RID: 5786
		private const float ThingIconSize = 22f;

		// Token: 0x0400169B RID: 5787
		private const float WindowWidth = 336f;

		// Token: 0x0400169C RID: 5788
		public static bool active;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000422 RID: 1058
	public class EditWindow_DebugInspector : EditWindow
	{
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001F2A RID: 7978 RVA: 0x000B870C File Offset: 0x000B690C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x00002662 File Offset: 0x00000862
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x000B871D File Offset: 0x000B691D
		public EditWindow_DebugInspector()
		{
			this.optionalTitle = "Debug inspector";
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000B8746 File Offset: 0x000B6946
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (Current.ProgramState == ProgramState.Playing)
			{
				GenUI.RenderMouseoverBracket();
			}
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x000B875C File Offset: 0x000B695C
		public override void DoWindowContents(Rect inRect)
		{
			if (KeyBindingDefOf.Dev_ToggleDebugInspector.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			if (Input.GetMouseButtonDown(1))
			{
				GUIUtility.systemCopyBuffer = this.debugStringBuilder.ToString();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			Text.Font = GameFont.Tiny;
			Widgets.Label(new Rect(inRect.x, inRect.y, 200f, 20f), "Right click to copy to clipboard");
			inRect.yMin += 20f;
			WidgetRow widgetRow = new WidgetRow(inRect.x, inRect.y, UIDirection.RightThenUp, 99999f, 4f);
			widgetRow.ToggleableIcon(ref this.fullMode, TexButton.InspectModeToggle, "Toggle deep inspection mode for things on the map.", null, null);
			widgetRow.ToggleableIcon(ref DebugViewSettings.writeCellContents, TexButton.InspectModeToggle, "Toggle shallow inspection for things on the map.", null, null);
			if (widgetRow.ButtonText("Visibility", "Toggle what information should be reported by the inspector.", true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Settings));
			}
			if (widgetRow.ButtonText("Column Width +", "Make the columns wider.", true, true, true, null))
			{
				this.columnWidth += 20f;
				this.columnWidth = Mathf.Clamp(this.columnWidth, 200f, 1600f);
			}
			if (widgetRow.ButtonText("Column Width -", "Make the columns narrower.", true, true, true, null))
			{
				this.columnWidth -= 20f;
				this.columnWidth = Mathf.Clamp(this.columnWidth, 200f, 1600f);
			}
			inRect.yMin += 30f;
			Listing_Standard listing_Standard = new Listing_Standard(GameFont.Tiny);
			listing_Standard.ColumnWidth = Mathf.Min(this.columnWidth, inRect.width);
			listing_Standard.Begin(inRect);
			foreach (string label in this.debugStringBuilder.ToString().Split(new char[]
			{
				'\n'
			}))
			{
				listing_Standard.Label(label, -1f, null);
				listing_Standard.Gap(-9f);
			}
			listing_Standard.End();
			if (Event.current.type == EventType.Repaint)
			{
				this.debugStringBuilder = new StringBuilder();
				this.debugStringBuilder.Append(this.CurrentDebugString());
			}
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x000B89AF File Offset: 0x000B6BAF
		public void AppendDebugString(string str)
		{
			this.debugStringBuilder.AppendLine(str);
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000B89C0 File Offset: 0x000B6BC0
		private string CurrentDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (DebugViewSettings.writeGame)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine((Current.Game == null) ? "Current.Game = null" : Current.Game.DebugString());
			}
			if (DebugViewSettings.writeMusicManagerPlay)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine(Find.MusicManagerPlay.DebugString());
			}
			if (DebugViewSettings.writePlayingSounds)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine("Sustainers:");
				foreach (Sustainer sustainer in Find.SoundRoot.sustainerManager.AllSustainers)
				{
					stringBuilder.AppendLine(sustainer.DebugString());
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("OneShots:");
				foreach (SampleOneShot sampleOneShot in Find.SoundRoot.oneShotManager.PlayingOneShots)
				{
					stringBuilder.AppendLine(sampleOneShot.ToString());
				}
			}
			if (DebugViewSettings.writeSoundEventsRecord)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine("Recent sound events:\n       ...");
				stringBuilder.AppendLine(DebugSoundEventsLog.EventsListingDebugString);
			}
			if (DebugViewSettings.writeSteamItems)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine(WorkshopItems.DebugOutput());
			}
			if (DebugViewSettings.writeConcepts)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine(LessonAutoActivator.DebugString());
			}
			if (DebugViewSettings.writeReservations && Find.CurrentMap != null)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine(string.Join("\r\n", (from r in Find.CurrentMap.reservationManager.ReservationsReadOnly
				select r.ToString()).ToArray<string>()));
			}
			if (DebugViewSettings.writeMemoryUsage)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine("Total allocated: " + Profiler.GetTotalAllocatedMemoryLong().ToStringBytes("F2"));
				stringBuilder.AppendLine("Total reserved: " + Profiler.GetTotalReservedMemoryLong().ToStringBytes("F2"));
				stringBuilder.AppendLine("Total reserved unused: " + Profiler.GetTotalUnusedReservedMemoryLong().ToStringBytes("F2"));
				stringBuilder.AppendLine("Mono heap size: " + Profiler.GetMonoHeapSizeLong().ToStringBytes("F2"));
				stringBuilder.AppendLine("Mono used size: " + Profiler.GetMonoUsedSizeLong().ToStringBytes("F2"));
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				stringBuilder.AppendLine("Tick " + Find.TickManager.TicksGame);
				if (DebugViewSettings.writeStoryteller)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.Storyteller.DebugString());
				}
			}
			if (Current.ProgramState == ProgramState.Playing && Find.CurrentMap != null)
			{
				if (DebugViewSettings.writeMapGameConditions)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.gameConditionManager.DebugString());
				}
				if (DebugViewSettings.drawPawnDebug)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.reservationManager.DebugString());
				}
				if (DebugViewSettings.writeMoteSaturation)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine("Mote count: " + Find.CurrentMap.moteCounter.MoteCount);
					stringBuilder.AppendLine("Mote saturation: " + Find.CurrentMap.moteCounter.Saturation);
				}
				if (DebugViewSettings.writeEcosystem)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.wildAnimalSpawner.DebugString());
				}
				if (DebugViewSettings.writeTotalSnowDepth)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine("Total snow depth: " + Find.CurrentMap.snowGrid.TotalDepth);
				}
				if (DebugViewSettings.writeWind)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.windManager.DebugString());
				}
				if (DebugViewSettings.writeRecentStrikes)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.mineStrikeManager.DebugStrikeRecords());
				}
				if (DebugViewSettings.writeListRepairableBldgs)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerBuildingsRepairable.DebugString());
				}
				if (DebugViewSettings.writeListFilthInHomeArea)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerFilthInHomeArea.DebugString());
				}
				if (DebugViewSettings.writeListHaulables)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerHaulables.DebugString());
				}
				if (DebugViewSettings.writeListMergeables)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerMergeables.DebugString());
				}
				if (DebugViewSettings.drawLords)
				{
					foreach (Lord lord in Find.CurrentMap.lordManager.lords)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(lord.DebugString());
					}
				}
				IntVec3 intVec = UI.MouseCell();
				if (intVec.InBounds(Find.CurrentMap))
				{
					stringBuilder.AppendLine("Inspecting " + intVec.ToString());
					if (DebugViewSettings.writeTerrain)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.CurrentMap.terrainGrid.DebugStringAt(intVec));
					}
					if (DebugViewSettings.writeAttackTargets)
					{
						foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>())
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine("Potential attack targets for " + pawn.LabelShort + ":");
							List<IAttackTarget> potentialTargetsFor = Find.CurrentMap.attackTargetsCache.GetPotentialTargetsFor(pawn);
							for (int i = 0; i < potentialTargetsFor.Count; i++)
							{
								Thing thing = (Thing)potentialTargetsFor[i];
								stringBuilder.AppendLine(string.Concat(new object[]
								{
									thing.LabelShort,
									", ",
									thing.Faction,
									potentialTargetsFor[i].ThreatDisabled(null) ? " (threat disabled)" : ""
								}));
							}
						}
					}
					if (DebugViewSettings.writeSnowDepth)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Snow depth: " + Find.CurrentMap.snowGrid.GetDepth(intVec));
					}
					if (DebugViewSettings.drawDeepResources)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Deep resource def: " + Find.CurrentMap.deepResourceGrid.ThingDefAt(intVec));
						stringBuilder.AppendLine("Deep resource count: " + Find.CurrentMap.deepResourceGrid.CountAt(intVec));
					}
					if (DebugViewSettings.writeCanReachColony)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("CanReachColony: " + Find.CurrentMap.reachability.CanReachColony(UI.MouseCell()).ToString());
					}
					if (DebugViewSettings.writeMentalStateCalcs)
					{
						stringBuilder.AppendLine("---");
						foreach (Pawn pawn2 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							stringBuilder.AppendLine(pawn2.mindState.mentalBreaker.DebugString());
						}
					}
					if (DebugViewSettings.writeWorkSettings)
					{
						foreach (Pawn pawn3 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							if (pawn3.workSettings != null)
							{
								stringBuilder.AppendLine("---");
								stringBuilder.AppendLine(pawn3.workSettings.DebugString());
							}
						}
					}
					if (DebugViewSettings.writeApparelScore)
					{
						stringBuilder.AppendLine("---");
						if (intVec.InBounds(Find.CurrentMap))
						{
							foreach (Thing thing2 in intVec.GetThingList(Find.CurrentMap))
							{
								Apparel apparel = thing2 as Apparel;
								if (apparel != null)
								{
									stringBuilder.AppendLine(apparel.Label + ": " + JobGiver_OptimizeApparel.ApparelScoreRaw(null, apparel).ToString("F2"));
								}
							}
						}
					}
					if (DebugViewSettings.writeCellContents || this.fullMode)
					{
						stringBuilder.AppendLine("---");
						if (intVec.InBounds(Find.CurrentMap))
						{
							foreach (Designation designation in Find.CurrentMap.designationManager.AllDesignationsAt(intVec))
							{
								stringBuilder.AppendLine(designation.ToString());
							}
							foreach (Thing thing3 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
							{
								if (!this.fullMode)
								{
									stringBuilder.AppendLine(thing3.LabelCap + " - " + thing3.ToString());
								}
								else
								{
									stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(thing3));
									stringBuilder.AppendLine();
								}
							}
						}
					}
					if (DebugViewSettings.debugApparelOptimize)
					{
						stringBuilder.AppendLine("---");
						foreach (Thing thing4 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
						{
							Apparel apparel2 = thing4 as Apparel;
							if (apparel2 != null)
							{
								stringBuilder.AppendLine(apparel2.LabelCap);
								stringBuilder.AppendLine("   raw: " + JobGiver_OptimizeApparel.ApparelScoreRaw(null, apparel2).ToString("F2"));
								Pawn pawn4 = Find.Selector.SingleSelectedThing as Pawn;
								if (pawn4 != null)
								{
									List<float> list = new List<float>();
									for (int j = 0; j < pawn4.apparel.WornApparel.Count; j++)
									{
										list.Add(JobGiver_OptimizeApparel.ApparelScoreRaw(pawn4, pawn4.apparel.WornApparel[j]));
									}
									stringBuilder.AppendLine("  Pawn: " + pawn4);
									stringBuilder.AppendLine("  gain: " + JobGiver_OptimizeApparel.ApparelScoreGain(pawn4, apparel2, list).ToString("F2"));
								}
							}
						}
					}
					if (DebugViewSettings.drawRegions)
					{
						stringBuilder.AppendLine("---");
						Region regionAt_NoRebuild_InvalidAllowed = Find.CurrentMap.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
						stringBuilder.AppendLine("Region:\n" + ((regionAt_NoRebuild_InvalidAllowed != null) ? regionAt_NoRebuild_InvalidAllowed.DebugString : "null"));
					}
					if (DebugViewSettings.drawDistricts)
					{
						stringBuilder.AppendLine("---");
						District district = intVec.GetDistrict(Find.CurrentMap, RegionType.Set_Passable);
						if (district != null)
						{
							stringBuilder.AppendLine(district.DebugString());
						}
						else
						{
							stringBuilder.AppendLine("(no district)");
						}
					}
					if (DebugViewSettings.drawRooms)
					{
						stringBuilder.AppendLine("---");
						Room room = intVec.GetRoom(Find.CurrentMap);
						if (room != null)
						{
							stringBuilder.AppendLine(room.DebugString());
						}
						else
						{
							stringBuilder.AppendLine("(no room)");
						}
					}
					if (DebugViewSettings.drawGlow)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Game glow: " + Find.CurrentMap.glowGrid.GameGlowAt(intVec, false));
						stringBuilder.AppendLine("Psych glow: " + Find.CurrentMap.glowGrid.PsychGlowAt(intVec));
						stringBuilder.AppendLine("Visual Glow: " + Find.CurrentMap.glowGrid.VisualGlowAt(intVec));
						stringBuilder.AppendLine("GlowReport:\n" + ((SectionLayer_LightingOverlay)Find.CurrentMap.mapDrawer.SectionAt(intVec).GetLayer(typeof(SectionLayer_LightingOverlay))).GlowReportAt(intVec));
						stringBuilder.AppendLine("SkyManager.CurSkyGlow: " + Find.CurrentMap.skyManager.CurSkyGlow);
					}
					if (DebugViewSettings.writePathCosts)
					{
						stringBuilder.AppendLine("---");
						int num = Find.CurrentMap.pathing.Normal.pathGrid.PerceivedPathCostAt(intVec);
						int num2 = Find.CurrentMap.pathing.Normal.pathGrid.CalculatedCostAt(intVec, false, IntVec3.Invalid);
						stringBuilder.AppendLine("Perceived path cost: " + num);
						stringBuilder.AppendLine("Real path cost: " + num2);
						int num3 = Find.CurrentMap.pathing.FenceBlocked.pathGrid.PerceivedPathCostAt(intVec);
						int num4 = Find.CurrentMap.pathing.FenceBlocked.pathGrid.CalculatedCostAt(intVec, false, IntVec3.Invalid);
						if (num3 != num || num4 != num2)
						{
							stringBuilder.AppendLine("Perceived path cost (for fenceblocked): " + num3);
							stringBuilder.AppendLine("Real path cost (for fenceblocked): " + num4);
						}
					}
					if (DebugViewSettings.writeFertility)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("\nFertility: " + Find.CurrentMap.fertilityGrid.FertilityAt(intVec).ToString("##0.00"));
					}
					if (DebugViewSettings.writeLinkFlags)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("\nLinkFlags: ");
						foreach (object obj in Enum.GetValues(typeof(LinkFlags)))
						{
							if ((Find.CurrentMap.linkGrid.LinkFlagsAt(intVec) & (LinkFlags)obj) != LinkFlags.None)
							{
								stringBuilder.Append(" " + obj);
							}
						}
					}
					if (DebugViewSettings.writeSkyManager)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.CurrentMap.skyManager.DebugString());
					}
					if (DebugViewSettings.writeCover)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.Append("Cover: ");
						Thing thing5 = Find.CurrentMap.coverGrid[intVec];
						if (thing5 == null)
						{
							stringBuilder.AppendLine("null");
						}
						else
						{
							stringBuilder.AppendLine(thing5.ToString());
						}
					}
					if (DebugViewSettings.drawPower)
					{
						stringBuilder.AppendLine("---");
						foreach (Thing thing6 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
						{
							ThingWithComps thingWithComps = thing6 as ThingWithComps;
							if (thingWithComps != null && thingWithComps.GetComp<CompPowerTrader>() != null)
							{
								stringBuilder.AppendLine(" " + thingWithComps.GetComp<CompPowerTrader>().DebugString);
							}
						}
						PowerNet powerNet = Find.CurrentMap.powerNetGrid.TransmittedPowerNetAt(intVec);
						if (powerNet != null)
						{
							stringBuilder.AppendLine(powerNet.DebugString() ?? "");
						}
						else
						{
							stringBuilder.AppendLine("(no PowerNet here)");
						}
					}
					if (DebugViewSettings.drawPreyInfo)
					{
						Pawn pawn5 = Find.Selector.SingleSelectedThing as Pawn;
						if (pawn5 != null)
						{
							List<Thing> thingList = intVec.GetThingList(Find.CurrentMap);
							int k = 0;
							while (k < thingList.Count)
							{
								Pawn pawn6 = thingList[k] as Pawn;
								if (pawn6 != null)
								{
									stringBuilder.AppendLine("---");
									if (FoodUtility.IsAcceptablePreyFor(pawn5, pawn6))
									{
										stringBuilder.AppendLine("Prey score: " + FoodUtility.GetPreyScoreFor(pawn5, pawn6));
										break;
									}
									stringBuilder.AppendLine("Prey score: None");
									break;
								}
								else
								{
									k++;
								}
							}
						}
					}
					if (DebugViewSettings.writeRopesAndPens)
					{
						Building edifice = intVec.GetEdifice(Find.CurrentMap);
						CompAnimalPenMarker compAnimalPenMarker = (edifice != null) ? edifice.TryGetComp<CompAnimalPenMarker>() : null;
						if (compAnimalPenMarker != null)
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine(string.Format("Pen marker {0} - {1}", compAnimalPenMarker.parent, compAnimalPenMarker.label));
							if (compAnimalPenMarker.PenState.Enclosed)
							{
								District district2 = compAnimalPenMarker.parent.GetDistrict(RegionType.Set_Passable);
								float num5 = new AnimalPenBalanceCalculator(Find.CurrentMap, false).TotalBodySizeIn(district2) / (float)district2.CellCount;
								float num6 = new AnimalPenBalanceCalculator(Find.CurrentMap, true).TotalBodySizeIn(district2) / (float)district2.CellCount;
								stringBuilder.AppendLine(string.Format(" animal density: {0} (upcoming: {1})", num5, num6));
							}
						}
						foreach (Pawn pawn7 in intVec.GetThingList(Find.CurrentMap).OfType<Pawn>())
						{
							if (pawn7.roping != null && pawn7.roping.HasAnyRope)
							{
								stringBuilder.AppendLine("---");
								stringBuilder.Append(pawn7.roping.DebugString());
							}
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001532 RID: 5426
		private const float CopyTextHeight = 20f;

		// Token: 0x04001533 RID: 5427
		private StringBuilder debugStringBuilder = new StringBuilder();

		// Token: 0x04001534 RID: 5428
		public bool fullMode;

		// Token: 0x04001535 RID: 5429
		private float columnWidth = 360f;
	}
}

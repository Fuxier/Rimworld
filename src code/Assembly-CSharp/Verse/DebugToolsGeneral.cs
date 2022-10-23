using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200043D RID: 1085
	public static class DebugToolsGeneral
	{
		// Token: 0x06002038 RID: 8248 RVA: 0x000C146C File Offset: 0x000BF66C
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void Destroy()
		{
			Thing.allowDestroyNonDestroyable = true;
			try
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			finally
			{
				Thing.allowDestroyNonDestroyable = false;
			}
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000C14EC File Offset: 0x000BF6EC
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void Kill()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Kill(null, null);
			}
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x000C155C File Offset: 0x000BF75C
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 999)]
		private static void SetFaction()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<Thing> things = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactionsVisibleInViewOrder)
			{
				Faction localFac = localFac2;
				FloatMenuOption item = new FloatMenuOption(localFac.Name, delegate()
				{
					foreach (Thing thing in things)
					{
						if (thing.def.CanHaveFaction)
						{
							thing.SetFaction(localFac, null);
						}
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x000C162C File Offset: 0x000BF82C
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Discard()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
				Pawn p;
				if ((p = (thing as Pawn)) != null)
				{
					Find.WorldPawns.RemoveAndDiscardPawnViaGC(p);
				}
				else
				{
					thing.Discard(false);
				}
			}
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x000C16B0 File Offset: 0x000BF8B0
		[DebugAction("General", "10 damage", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take10Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000C1734 File Offset: 0x000BF934
		[DebugAction("General", "300 damage", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take300Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 300f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x000C17B8 File Offset: 0x000BF9B8
		[DebugAction("General", "5000 damage", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take5000Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 50000f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x000C183C File Offset: 0x000BFA3C
		[DebugAction("General", "Clear area (rect)", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void ClearArea()
		{
			DebugToolsGeneral.GenericRectTool("Clear", delegate(CellRect rect)
			{
				GenDebug.ClearArea(rect, Find.CurrentMap);
			});
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x000C1867 File Offset: 0x000BFA67
		[DebugAction("General", "Make empty room (rect)", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static void MakeEmptyRoom()
		{
			DebugToolsGeneral.GenericRectTool("Make room", delegate(CellRect rect)
			{
				GenDebug.ClearArea(rect, Find.CurrentMap);
				IEnumerable<IntVec3> edgeCells = rect.EdgeCells;
				IntVec3 invalid = IntVec3.Invalid;
				(from x in edgeCells
				where !rect.IsCorner(x)
				select x).TryRandomElement(out invalid);
				foreach (IntVec3 intVec in edgeCells)
				{
					Thing thing = ThingMaker.MakeThing((intVec == invalid) ? ThingDefOf.Door : ThingDefOf.Wall, ThingDefOf.WoodLog);
					thing.SetFaction(Faction.OfPlayer, null);
					GenPlace.TryPlaceThing(thing, intVec, Find.CurrentMap, ThingPlaceMode.Direct, null, null, default(Rot4));
				}
				foreach (IntVec3 c in rect)
				{
					Find.CurrentMap.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
					Find.CurrentMap.terrainGrid.SetTerrain(c, TerrainDefOf.WoodPlankFloor);
				}
			});
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x000C1894 File Offset: 0x000BFA94
		[DebugAction("General", "Edit roof (rect)", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
		private static List<DebugActionNode> MakeRoof()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			list.Add(new DebugActionNode("Clear", DebugActionType.Action, delegate()
			{
				DebugToolsGeneral.GenericRectTool("Clear roof", delegate(CellRect rect)
				{
					foreach (IntVec3 c in rect)
					{
						Find.CurrentMap.roofGrid.SetRoof(c, null);
					}
				});
			}, null));
			foreach (RoofDef localDef2 in DefDatabase<RoofDef>.AllDefs)
			{
				RoofDef localDef = localDef2;
				Action<CellRect> <>9__3;
				list.Add(new DebugActionNode(localDef.LabelCap, DebugActionType.Action, delegate()
				{
					string label = "Make roof (" + localDef.label + ")";
					Action<CellRect> rectAction;
					if ((rectAction = <>9__3) == null)
					{
						rectAction = (<>9__3 = delegate(CellRect rect)
						{
							foreach (IntVec3 c in rect)
							{
								Find.CurrentMap.roofGrid.SetRoof(c, localDef);
							}
						});
					}
					DebugToolsGeneral.GenericRectTool(label, rectAction);
				}, null));
			}
			return list;
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x000C1948 File Offset: 0x000BFB48
		public static void GenericRectTool(string label, Action<CellRect> rectAction)
		{
			DebugTool tool = null;
			IntVec3 firstCorner = UI.MouseCell();
			Action <>9__1;
			tool = new DebugTool(label + ": First corner...", delegate()
			{
				firstCorner = UI.MouseCell();
				string label2 = label + ": Second corner...";
				Action clickAction;
				if ((clickAction = <>9__1) == null)
				{
					clickAction = (<>9__1 = delegate()
					{
						IntVec3 second = UI.MouseCell();
						CellRect obj = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
						rectAction(obj);
						DebugTools.curTool = tool;
					});
				}
				DebugTools.curTool = new DebugTool(label2, clickAction, firstCorner);
			}, null);
			DebugTools.curTool = tool;
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x000C19B0 File Offset: 0x000BFBB0
		[DebugAction("General", "Explosion...", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> Explosion()
		{
			if (DebugToolsGeneral.explosionDatas.NullOrEmpty<DebugToolsGeneral.ExplosionData>())
			{
				DebugToolsGeneral.explosionDatas = new List<DebugToolsGeneral.ExplosionData>
				{
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Bomb
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Flame
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Stun
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.EMP
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Extinguish,
						thingDef = ThingDefOf.Filth_FireFoam,
						thingChance = 1f,
						thingCount = 3,
						applyToNeighbors = true
					},
					new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Smoke,
						gasType = new GasType?(GasType.BlindSmoke)
					}
				};
				if (ModsConfig.BiotechActive)
				{
					DebugToolsGeneral.explosionDatas.Add(new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.ToxGas,
						gasType = new GasType?(GasType.ToxGas)
					});
					DebugToolsGeneral.explosionDatas.Add(new DebugToolsGeneral.ExplosionData
					{
						damageDef = DamageDefOf.Vaporize
					});
				}
			}
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < DebugToolsGeneral.explosionDatas.Count; i++)
			{
				DebugToolsGeneral.ExplosionData data = DebugToolsGeneral.explosionDatas[i];
				list.Add(new DebugActionNode(data.damageDef.LabelCap, DebugActionType.ToolMap, delegate()
				{
					GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 4.9f, data.damageDef, null, -1, -1f, null, null, null, null, data.thingDef, data.thingChance, data.thingCount, data.gasType, data.applyToNeighbors, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
				}, null));
			}
			return list;
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000C1B60 File Offset: 0x000BFD60
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void LightningStrike()
		{
			Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, UI.MouseCell()));
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x000C1B88 File Offset: 0x000BFD88
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static List<DebugActionNode> LightningStrikeDelayed()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < 41; i++)
			{
				int delay = i * 30;
				list.Add(new DebugActionNode(((float)delay / 60f).ToString("F1") + " seconds", DebugActionType.ToolMap, delegate()
				{
					Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeDelayed(Find.CurrentMap, UI.MouseCell(), delay));
				}, null));
			}
			return list;
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000C1BF8 File Offset: 0x000BFDF8
		[DebugAction("General", "Add gas...", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200)]
		private static List<DebugActionNode> PushGas()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (object obj in Enum.GetValues(typeof(GasType)))
			{
				GasType gasType2 = (GasType)obj;
				GasType gasType = gasType2;
				if (gasType != GasType.Unused && (gasType != GasType.ToxGas || ModsConfig.BiotechActive))
				{
					list.Add(new DebugActionNode(gasType.GetLabel().CapitalizeFirst(), DebugActionType.ToolMap, delegate()
					{
						GasUtility.AddGas(UI.MouseCell(), Find.CurrentMap, gasType, 5f);
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000C1CB0 File Offset: 0x000BFEB0
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200)]
		private static void ClearAllGas()
		{
			Find.CurrentMap.gasGrid.Debug_ClearAll();
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000C1CC1 File Offset: 0x000BFEC1
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void FillAllGas()
		{
			Find.CurrentMap.gasGrid.Debug_FillAll();
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000C1CD4 File Offset: 0x000BFED4
		[DebugAction("General", "Push heat...", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -200)]
		private static List<DebugActionNode> PushHeat()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < DebugToolsGeneral.HeatPushOptions.Length; i++)
			{
				int t = DebugToolsGeneral.HeatPushOptions[i];
				list.Add(new DebugActionNode(t.ToString(), DebugActionType.ToolMap, delegate()
				{
					foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
					{
						GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, (float)t);
					}
				}, null));
			}
			return list;
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000C1D34 File Offset: 0x000BFF34
		[DebugAction("General", "Grow plant 1 day", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Grow1Day()
		{
			IntVec3 intVec = UI.MouseCell();
			Plant plant = intVec.GetPlant(Find.CurrentMap);
			if (plant != null && plant.def.plant != null)
			{
				int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
				if (num >= 60000)
				{
					plant.Age += 60000;
				}
				else if (num > 0)
				{
					plant.Age += num;
				}
				plant.Growth += 1f / plant.def.plant.growDays;
				if ((double)plant.Growth > 1.0)
				{
					plant.Growth = 1f;
				}
				Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
			}
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000C1E0C File Offset: 0x000C000C
		[DebugAction("General", "Grow plant to maturity", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrowPlantToMaturity()
		{
			IntVec3 intVec = UI.MouseCell();
			Plant plant = intVec.GetPlant(Find.CurrentMap);
			if (plant != null && plant.def.plant != null)
			{
				int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
				plant.Age += num;
				plant.Growth = 1f;
				Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
			}
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000C1E88 File Offset: 0x000C0088
		[DebugAction("General", "Rotate", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static DebugActionNode Rotate()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			debugActionNode.AddChild(new DebugActionNode("Clockwise", DebugActionType.ToolMap, delegate()
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Rotation = thing.Rotation.Rotated(RotationDirection.Clockwise);
					thing.DirtyMapMesh(thing.Map);
				}
			}, null));
			debugActionNode.AddChild(new DebugActionNode("Counter clockwise", DebugActionType.ToolMap, delegate()
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Rotation = thing.Rotation.Rotated(RotationDirection.Counterclockwise);
					thing.DirtyMapMesh(thing.Map);
				}
			}, null));
			return debugActionNode;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000C1EFC File Offset: 0x000C00FC
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetColor()
		{
			DebugToolsGeneral.<>c__DisplayClass24_0 CS$<>8__locals1 = new DebugToolsGeneral.<>c__DisplayClass24_0();
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			CS$<>8__locals1.cell = UI.MouseCell();
			list.Add(new FloatMenuOption("Random", delegate()
			{
				base.<SetColor>g__SetColor_All|0(GenColor.RandomColorOpaque());
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			using (List<Ideo>.Enumerator enumerator = Find.IdeoManager.IdeosListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ideo i = enumerator.Current;
					if (!i.hiddenIdeoMode && i.Icon != BaseContent.BadTex)
					{
						list.Add(new FloatMenuOption(i.name, delegate()
						{
							CS$<>8__locals1.<SetColor>g__SetColor_All|0(i.Color);
						}, i.Icon, i.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
					}
				}
			}
			using (IEnumerator<ColorDef> enumerator2 = DefDatabase<ColorDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ColorDef c = enumerator2.Current;
					list.Add(new FloatMenuOption(c.defName, delegate()
					{
						CS$<>8__locals1.<SetColor>g__SetColor_All|0(c.color);
					}, BaseContent.WhiteTex, c.color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000C20B0 File Offset: 0x000C02B0
		[DebugAction("General", "Rot 1 day", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Rot1Day()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompRottable compRottable = thing.TryGetComp<CompRottable>();
				if (compRottable != null)
				{
					compRottable.RotProgress += 60000f;
				}
			}
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000C2120 File Offset: 0x000C0320
		[DebugAction("General", "Force sleep", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceSleep()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
				Pawn pawn;
				if (compCanBeDormant != null)
				{
					compCanBeDormant.ToSleep();
				}
				else if ((pawn = (thing as Pawn)) != null)
				{
					pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position), JobCondition.None, null, false, true, null, null, false, false, null, false, true);
				}
			}
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x000C21D8 File Offset: 0x000C03D8
		[DebugAction("General", "Break down...", false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BreakDown()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompBreakdownable compBreakdownable = thing.TryGetComp<CompBreakdownable>();
				if (compBreakdownable != null && !compBreakdownable.BrokenDown)
				{
					compBreakdownable.DoBreakdown();
				}
			}
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000C2244 File Offset: 0x000C0444
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void TestFloodUnfog()
		{
			FloodFillerFog.DebugFloodUnfog(UI.MouseCell(), Find.CurrentMap);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000C2258 File Offset: 0x000C0458
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashClosewalkCell30()
		{
			IntVec3 c = CellFinder.RandomClosewalkCellNear(UI.MouseCell(), Find.CurrentMap, 30, null);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000C2290 File Offset: 0x000C0490
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashWalkPath()
		{
			WalkPathFinder.DebugFlashWalkPath(UI.MouseCell(), 8);
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000C22A0 File Offset: 0x000C04A0
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashSkygazeCell()
		{
			Pawn pawn = Find.CurrentMap.mapPawns.FreeColonists.First<Pawn>();
			IntVec3 c;
			RCellFinder.TryFindSkygazeCell(UI.MouseCell(), pawn, out c);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
			MoteMaker.ThrowText(c.ToVector3Shifted(), Find.CurrentMap, "for " + pawn.Label, Color.white, -1f);
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000C2314 File Offset: 0x000C0514
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashDirectFleeDest()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "select a pawn", 50);
				return;
			}
			IntVec3 c;
			if (RCellFinder.TryFindDirectFleeDestination(UI.MouseCell(), 9f, pawn, out c))
			{
				Find.CurrentMap.debugDrawer.FlashCell(c, 0.5f, null, 50);
				return;
			}
			Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0.8f, "not found", 50);
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x000C23A4 File Offset: 0x000C05A4
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, hideInSubMenu = true)]
		private static void FlashShuttleDropCellsNear()
		{
			IntVec3 center = UI.MouseCell();
			Map currentMap = Find.CurrentMap;
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c;
				DropCellFinder.TryFindDropSpotNear(center, currentMap, out c, false, false, false, new IntVec2?(ThingDefOf.Shuttle.Size + new IntVec2(2, 2)), false);
				currentMap.debugDrawer.FlashCell(c, 0.2f, null, 50);
			}
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x000C2408 File Offset: 0x000C0608
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashSpectatorsCells()
		{
			Action<bool> act = delegate(bool bestSideOnly)
			{
				DebugTool tool = null;
				IntVec3 firstCorner;
				Action <>9__4;
				tool = new DebugTool("first watch rect corner...", delegate()
				{
					firstCorner = UI.MouseCell();
					string label = "second watch rect corner...";
					Action clickAction;
					if ((clickAction = <>9__4) == null)
					{
						clickAction = (<>9__4 = delegate()
						{
							IntVec3 second = UI.MouseCell();
							CellRect spectateRect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
							SpectateRectSide allowedSides = SpectateRectSide.All;
							if (bestSideOnly)
							{
								allowedSides = SpectatorCellFinder.FindSingleBestSide(spectateRect, Find.CurrentMap, SpectateRectSide.All, 1, null);
							}
							SpectatorCellFinder.DebugFlashPotentialSpectatorCells(spectateRect, Find.CurrentMap, allowedSides, 1);
							DebugTools.curTool = tool;
						});
					}
					DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
				}, null);
				DebugTools.curTool = tool;
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("All sides", DebugMenuOptionMode.Action, delegate()
			{
				act(false);
			}));
			list.Add(new DebugMenuOption("Best side only", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000C2490 File Offset: 0x000C0690
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> CheckReachability()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			TraverseMode[] array = (TraverseMode[])Enum.GetValues(typeof(TraverseMode));
			for (int i = 0; i < array.Length; i++)
			{
				TraverseMode traverseMode2 = array[i];
				TraverseMode traverseMode = traverseMode2;
				list.Add(new DebugActionNode(traverseMode2.ToString(), DebugActionType.Action, delegate()
				{
					DebugTool tool = null;
					IntVec3 from;
					Pawn fromPawn;
					Action <>9__2;
					Action <>9__3;
					tool = new DebugTool("from...", delegate()
					{
						from = UI.MouseCell();
						fromPawn = from.GetFirstPawn(Find.CurrentMap);
						string text = "to...";
						if (fromPawn != null)
						{
							text = text + " (pawn=" + fromPawn.LabelShort + ")";
						}
						string label = text;
						Action clickAction;
						if ((clickAction = <>9__2) == null)
						{
							clickAction = (<>9__2 = delegate()
							{
								DebugTools.curTool = tool;
							});
						}
						Action onGUIAction;
						if ((onGUIAction = <>9__3) == null)
						{
							onGUIAction = (<>9__3 = delegate()
							{
								IntVec3 c = UI.MouseCell();
								bool flag;
								IntVec3 intVec;
								if (fromPawn != null)
								{
									flag = fromPawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, traverseMode);
									intVec = fromPawn.Position;
								}
								else
								{
									flag = Find.CurrentMap.reachability.CanReach(from, c, PathEndMode.OnCell, traverseMode, Danger.Deadly);
									intVec = from;
								}
								Color color = flag ? Color.green : Color.red;
								Widgets.DrawLine(intVec.ToUIPosition(), c.ToUIPosition(), color, 2f);
							});
						}
						DebugTools.curTool = new DebugTool(label, clickAction, onGUIAction);
					}, null);
					DebugTools.curTool = tool;
				}, null));
			}
			return list;
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000C2504 File Offset: 0x000C0704
		[DebugAction("General", "Flash TryFindRandomPawnExitCell", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashTryFindRandomPawnExitCell(Pawn p)
		{
			IntVec3 intVec;
			if (CellFinder.TryFindRandomPawnExitCell(p, out intVec))
			{
				p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				return;
			}
			p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no exit cell", 50);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000C2574 File Offset: 0x000C0774
		[DebugAction("General", "RandomSpotJustOutsideColony", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void RandomSpotJustOutsideColony(Pawn p)
		{
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomSpotJustOutsideColony(p, out intVec))
			{
				p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				return;
			}
			p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no cell", 50);
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x000C25E4 File Offset: 0x000C07E4
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void RandomSpotNearThingAvoidingHostiles()
		{
			List<Thing> list = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (list.Count == 0)
			{
				return;
			}
			Thing thing = (from t in list
			where t is Pawn && t.Faction != null
			select t).FirstOrDefault<Thing>();
			if (thing == null)
			{
				thing = list.First<Thing>();
			}
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomSpotNearAvoidingHostilePawns(thing, thing.Map, (IntVec3 s) => true, out intVec, 100f, 10f, 50f, true))
			{
				thing.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				thing.Map.debugDrawer.FlashLine(thing.Position, intVec, 50, SimpleColor.White);
				return;
			}
			thing.Map.debugDrawer.FlashCell(thing.Position, 0.2f, "no cell", 50);
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x000C26DE File Offset: 0x000C08DE
		[DebugAction("Map", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearAllFog()
		{
			Find.CurrentMap.fogGrid.ClearAllFog();
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000C26F0 File Offset: 0x000C08F0
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeThingStyle()
		{
			Thing thing = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).FirstOrDefault((Thing x) => x.def.CanBeStyled());
			if (thing == null)
			{
				return;
			}
			DebugToolsGeneral.tmpStyleDefs.Clear();
			if (!thing.def.randomStyle.NullOrEmpty<ThingStyleChance>())
			{
				foreach (ThingStyleChance thingStyleChance in thing.def.randomStyle)
				{
					if (thingStyleChance.StyleDef.graphicData != null)
					{
						DebugToolsGeneral.tmpStyleDefs.Add(thingStyleChance.StyleDef);
					}
				}
			}
			IEnumerable<StyleCategoryDef> allDefs = DefDatabase<StyleCategoryDef>.AllDefs;
			Func<StyleCategoryDef, bool> <>9__1;
			Func<StyleCategoryDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				Predicate<ThingDefStyle> <>9__2;
				predicate = (<>9__1 = delegate(StyleCategoryDef x)
				{
					List<ThingDefStyle> thingDefStyles = x.thingDefStyles;
					Predicate<ThingDefStyle> predicate2;
					if ((predicate2 = <>9__2) == null)
					{
						predicate2 = (<>9__2 = ((ThingDefStyle y) => y.ThingDef == thing.def));
					}
					return thingDefStyles.Any(predicate2);
				});
			}
			foreach (StyleCategoryDef styleCategoryDef in allDefs.Where(predicate))
			{
				DebugToolsGeneral.tmpStyleDefs.Add(styleCategoryDef.GetStyleForThingDef(thing.def, null));
			}
			if (DebugToolsGeneral.tmpStyleDefs.Any<ThingStyleDef>())
			{
				DebugToolsGeneral.<>c__DisplayClass41_1 CS$<>8__locals2;
				CS$<>8__locals2.opts = new List<DebugMenuOption>();
				DebugToolsGeneral.<ChangeThingStyle>g__AddOption|41_3(thing, () => null, "Standard", ref CS$<>8__locals2);
				Func<ThingStyleDef, float> <>9__7;
				DebugToolsGeneral.<ChangeThingStyle>g__AddOption|41_3(thing, delegate
				{
					IEnumerable<ThingStyleDef> source = DebugToolsGeneral.tmpStyleDefs;
					Func<ThingStyleDef, float> weightSelector;
					if ((weightSelector = <>9__7) == null)
					{
						weightSelector = (<>9__7 = delegate(ThingStyleDef x)
						{
							if (x != thing.StyleDef)
							{
								return 1f;
							}
							return 0.01f;
						});
					}
					return source.RandomElementByWeight(weightSelector);
				}, "Random", ref CS$<>8__locals2);
				using (List<ThingStyleDef>.Enumerator enumerator3 = DebugToolsGeneral.tmpStyleDefs.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						ThingStyleDef s = enumerator3.Current;
						DebugToolsGeneral.<ChangeThingStyle>g__AddOption|41_3(thing, () => s, s.defName, ref CS$<>8__locals2);
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(CS$<>8__locals2.opts));
			}
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000C295C File Offset: 0x000C0B5C
		[CompilerGenerated]
		internal static void <ChangeThingStyle>g__AddOption|41_3(Thing t, Func<ThingStyleDef> styleSelector, string label, ref DebugToolsGeneral.<>c__DisplayClass41_1 A_3)
		{
			A_3.opts.Add(new DebugMenuOption(label, DebugMenuOptionMode.Action, delegate()
			{
				t.StyleDef = styleSelector();
				t.DirtyMapMesh(t.Map);
			}));
		}

		// Token: 0x040015AD RID: 5549
		private static List<DebugToolsGeneral.ExplosionData> explosionDatas;

		// Token: 0x040015AE RID: 5550
		private static readonly int[] HeatPushOptions = new int[]
		{
			10,
			50,
			100,
			1000,
			-10,
			-50,
			-1000
		};

		// Token: 0x040015AF RID: 5551
		private static List<ThingStyleDef> tmpStyleDefs = new List<ThingStyleDef>();

		// Token: 0x02001F3F RID: 7999
		private struct ExplosionData
		{
			// Token: 0x04007A69 RID: 31337
			public DamageDef damageDef;

			// Token: 0x04007A6A RID: 31338
			public GasType? gasType;

			// Token: 0x04007A6B RID: 31339
			public ThingDef thingDef;

			// Token: 0x04007A6C RID: 31340
			public float thingChance;

			// Token: 0x04007A6D RID: 31341
			public int thingCount;

			// Token: 0x04007A6E RID: 31342
			public bool applyToNeighbors;
		}
	}
}

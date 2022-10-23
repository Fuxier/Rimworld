using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200043E RID: 1086
	public static class DebugToolsMisc
	{
		// Token: 0x06002060 RID: 8288 RVA: 0x000C299C File Offset: 0x000C0B9C
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AttachFire()
		{
			foreach (Thing t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				t.TryAttachFire(1f);
			}
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x000C2A04 File Offset: 0x000C0C04
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static DebugActionNode SetQuality()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			foreach (object obj in Enum.GetValues(typeof(QualityCategory)))
			{
				QualityCategory qualityInner2 = (QualityCategory)obj;
				QualityCategory qualityInner = qualityInner2;
				debugActionNode.AddChild(new DebugActionNode(qualityInner.ToString(), DebugActionType.ToolMap, delegate()
				{
					foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap))
					{
						CompQuality compQuality = thing.TryGetComp<CompQuality>();
						if (compQuality != null)
						{
							compQuality.SetQuality(qualityInner, ArtGenerationContext.Outsider);
						}
					}
				}, null));
			}
			return debugActionNode;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000C2AA0 File Offset: 0x000C0CA0
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		public static void MeasureDrawSize()
		{
			Vector3 first;
			Action <>9__1;
			DebugTools.curMeasureTool = new DrawMeasureTool("first corner...", delegate()
			{
				first = UI.MouseMapPosition();
				string label = "second corner...";
				Action clickAction;
				if ((clickAction = <>9__1) == null)
				{
					clickAction = (<>9__1 = delegate()
					{
						Vector3 vector = UI.MouseMapPosition();
						Rect rect = default(Rect);
						rect.xMin = Mathf.Min(first.x, vector.x);
						rect.yMin = Mathf.Min(first.z, vector.z);
						rect.xMax = Mathf.Max(first.x, vector.x);
						rect.yMax = Mathf.Max(first.z, vector.z);
						string text = string.Format("Center: ({0},{1})", rect.center.x, rect.center.y);
						text += string.Format("\nSize: ({0},{1})", rect.size.x, rect.size.y);
						if (Find.Selector.SingleSelectedObject != null)
						{
							Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
							Vector3 drawPos = singleSelectedThing.DrawPos;
							Vector2 vector2 = rect.center - new Vector2(drawPos.x, drawPos.z);
							text += string.Format("\nOffset: ({0},{1})", vector2.x, vector2.y);
							Vector2 vector3 = vector2.RotatedBy(-singleSelectedThing.Rotation.AsAngle);
							text += string.Format("\nUnrotated offset: ({0},{1})", vector3.x, vector3.y);
						}
						Log.Message(text);
						DebugToolsMisc.MeasureDrawSize();
					});
				}
				DebugTools.curMeasureTool = new DrawMeasureTool(label, clickAction, first);
			}, null);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000C2AD0 File Offset: 0x000C0CD0
		[DebugAction("General", "Pollution +1%", false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresBiotech = true)]
		private static void IncreasePollutionSmall()
		{
			int num = GenWorld.MouseTile(false);
			if (num >= 0)
			{
				WorldPollutionUtility.PolluteWorldAtTile(num, 0.01f);
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000C2AF4 File Offset: 0x000C0CF4
		[DebugAction("General", "Pollution +25%", false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresBiotech = true)]
		private static void IncreasePollutionLarge()
		{
			int num = GenWorld.MouseTile(false);
			if (num >= 0)
			{
				WorldPollutionUtility.PolluteWorldAtTile(num, 0.25f);
			}
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x000C2B18 File Offset: 0x000C0D18
		[DebugAction("General", "Pollution -25%", false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresBiotech = true)]
		private static void DecreasePollutionLarge()
		{
			int num = GenWorld.MouseTile(false);
			if (num >= 0)
			{
				WorldPollutionUtility.PolluteWorldAtTile(num, -0.25f);
			}
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x000C2B3B File Offset: 0x000C0D3B
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresBiotech = true)]
		private static void ResetBossgroupCooldown()
		{
			Find.BossgroupManager.lastBossgroupCalled = Find.TickManager.TicksGame - 120000;
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x000C2B57 File Offset: 0x000C0D57
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresBiotech = true)]
		private static void ResetBossgroupKilledPawns()
		{
			Find.BossgroupManager.DebugResetDefeatedPawns();
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x000C2B64 File Offset: 0x000C0D64
		[DebugAction("Insect", "Spawn cocoon infestation", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, hideInSubMenu = true, requiresBiotech = true)]
		private static List<DebugActionNode> SpawnCocoonInfestationWithPoints()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (float localP2 in DebugActionsUtility.PointsOptions(false))
			{
				float localP = localP2;
				DebugActionNode item = new DebugActionNode(localP + " points", DebugActionType.ToolMap, delegate()
				{
					CocoonInfestationUtility.SpawnCocoonInfestation(UI.MouseCell(), Find.CurrentMap, localP);
				}, null);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x000C2BF0 File Offset: 0x000C0DF0
		[DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.Action)]
		private static void BenchmarkPerformance()
		{
			Messages.Message(string.Format("Running benchmark, results displayed in {0} seconds", 30f), MessageTypeDefOf.NeutralEvent, false);
			PerformanceBenchmarkUtility.StartBenchmark();
		}
	}
}

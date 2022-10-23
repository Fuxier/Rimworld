using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200048D RID: 1165
	public static class EnvironmentStatsDrawer
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x000E1F10 File Offset: 0x000E0110
		private static int DisplayedRoomStatsCount
		{
			get
			{
				int num = 0;
				List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!allDefsListForReading[i].isHidden || DebugViewSettings.showAllRoomStats)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x000E1F50 File Offset: 0x000E0150
		private static bool ShouldShowWindowNow()
		{
			return (EnvironmentStatsDrawer.ShouldShowRoomStats() || EnvironmentStatsDrawer.ShouldShowBeauty()) && !Mouse.IsInputBlockedNow;
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000E1F6C File Offset: 0x000E016C
		private static bool ShouldShowRoomStats()
		{
			if (!Find.PlaySettings.showRoomStats)
			{
				return false;
			}
			if (!UI.MouseCell().InBounds(Find.CurrentMap) || UI.MouseCell().Fogged(Find.CurrentMap))
			{
				return false;
			}
			Room room = UI.MouseCell().GetRoom(Find.CurrentMap);
			return room != null && room.Role != RoomRoleDefOf.None;
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000E1FD0 File Offset: 0x000E01D0
		private static bool ShouldShowBeauty()
		{
			return Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && UI.MouseCell().GetRoom(Find.CurrentMap) != null;
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x000E2021 File Offset: 0x000E0221
		public static void EnvironmentStatsOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !EnvironmentStatsDrawer.ShouldShowWindowNow())
			{
				return;
			}
			EnvironmentStatsDrawer.DrawInfoWindow();
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x000E2040 File Offset: 0x000E0240
		private static void DrawInfoWindow()
		{
			Text.Font = GameFont.Small;
			Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(EnvironmentStatsDrawer.ShouldShowBeauty(), EnvironmentStatsDrawer.ShouldShowRoomStats());
			Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.GameUI, delegate
			{
				EnvironmentStatsDrawer.FillWindow(windowRect);
			}, true, false, 1f, null);
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x000E2098 File Offset: 0x000E0298
		public static Rect GetWindowRect(bool shouldShowBeauty, bool shouldShowRoomStats)
		{
			Rect result = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 414f, 24f);
			int num = 0;
			if (shouldShowBeauty)
			{
				num++;
				result.height += 25f;
			}
			if (shouldShowRoomStats)
			{
				num++;
				result.height += 23f;
				result.height += (float)EnvironmentStatsDrawer.DisplayedRoomStatsCount * 25f + 23f;
			}
			result.height += 13f * (float)(num - 1);
			result.x += 26f;
			result.y += 26f;
			if (result.xMax > (float)UI.screenWidth)
			{
				result.x -= result.width + 52f;
			}
			if (result.yMax > (float)UI.screenHeight)
			{
				result.y -= result.height + 52f;
			}
			return result;
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x000E21C0 File Offset: 0x000E03C0
		private static void FillWindow(Rect windowRect)
		{
			EnvironmentStatsDrawer.<>c__DisplayClass21_0 CS$<>8__locals1;
			CS$<>8__locals1.windowRect = windowRect;
			Text.Font = GameFont.Small;
			CS$<>8__locals1.curY = 12f;
			CS$<>8__locals1.dividingLinesSeen = 0;
			if (EnvironmentStatsDrawer.ShouldShowBeauty())
			{
				EnvironmentStatsDrawer.<FillWindow>g__DrawDividingLineIfNecessary|21_0(ref CS$<>8__locals1);
				float beauty = BeautyUtility.AverageBeautyPerceptible(UI.MouseCell(), Find.CurrentMap);
				Rect rect = new Rect(22f, CS$<>8__locals1.curY, CS$<>8__locals1.windowRect.width - 24f - 10f, 100f);
				GUI.color = BeautyDrawer.BeautyColor(beauty, 40f);
				Widgets.Label(rect, "BeautyHere".Translate() + ": " + beauty.ToString("F1"));
				CS$<>8__locals1.curY += 25f;
			}
			if (EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				EnvironmentStatsDrawer.<FillWindow>g__DrawDividingLineIfNecessary|21_0(ref CS$<>8__locals1);
				EnvironmentStatsDrawer.DoRoomInfo(UI.MouseCell().GetRoom(Find.CurrentMap), ref CS$<>8__locals1.curY, CS$<>8__locals1.windowRect);
			}
			GUI.color = Color.white;
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x000E22C4 File Offset: 0x000E04C4
		public static void DrawRoomOverlays()
		{
			if (Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap))
			{
				GenUI.RenderMouseoverBracket();
			}
			if (EnvironmentStatsDrawer.ShouldShowWindowNow() && EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				Room room = UI.MouseCell().GetRoom(Find.CurrentMap);
				if (room != null && room.Role != RoomRoleDefOf.None)
				{
					room.DrawFieldEdges();
				}
			}
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000E2328 File Offset: 0x000E0528
		public static void DoRoomInfo(Room room, ref float curY, Rect windowRect)
		{
			Rect rect = new Rect(12f, curY, windowRect.width - 24f, 100f);
			GUI.color = Color.white;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x + 10f, curY, rect.width - 10f, rect.height), room.GetRoomRoleLabel().CapitalizeFirst());
			curY += 30f;
			Text.Font = GameFont.Small;
			Text.WordWrap = false;
			int num = 0;
			bool flag = false;
			for (int i = 0; i < DefDatabase<RoomStatDef>.AllDefsListForReading.Count; i++)
			{
				RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.AllDefsListForReading[i];
				if (!roomStatDef.isHidden || DebugViewSettings.showAllRoomStats)
				{
					float stat = room.GetStat(roomStatDef);
					RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
					GUI.color = Color.white;
					Rect rect2 = new Rect(rect.x, curY, rect.width, 23f);
					if (num % 2 == 1)
					{
						Widgets.DrawLightHighlight(rect2);
					}
					Rect rect3 = new Rect(rect.x, curY, 10f, 23f);
					if (room.Role.IsStatRelated(roomStatDef))
					{
						flag = true;
						Widgets.Label(rect3, "*");
						GUI.color = EnvironmentStatsDrawer.RelatedStatColor;
					}
					else
					{
						GUI.color = EnvironmentStatsDrawer.UnrelatedStatColor;
					}
					Rect rect4 = new Rect(rect3.xMax, curY, 100f, 23f);
					Widgets.Label(rect4, roomStatDef.LabelCap);
					Rect rect5 = new Rect(rect4.xMax + 35f, curY, 50f, 23f);
					string label = roomStatDef.ScoreToString(stat);
					Widgets.Label(rect5, label);
					Widgets.Label(new Rect(rect5.xMax + 35f, curY, 160f, 23f), (scoreStage == null) ? "" : scoreStage.label.CapitalizeFirst());
					curY += 25f;
					num++;
				}
			}
			if (flag)
			{
				GUI.color = Color.grey;
				Text.Font = GameFont.Tiny;
				Widgets.Label(new Rect(rect.x, curY, rect.width, 23f), "* " + "StatRelatesToCurrentRoom".Translate());
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
			}
			Text.WordWrap = true;
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000E25AC File Offset: 0x000E07AC
		[CompilerGenerated]
		internal static void <FillWindow>g__DrawDividingLineIfNecessary|21_0(ref EnvironmentStatsDrawer.<>c__DisplayClass21_0 A_0)
		{
			int dividingLinesSeen = A_0.dividingLinesSeen;
			A_0.dividingLinesSeen = dividingLinesSeen + 1;
			if (A_0.dividingLinesSeen <= 1)
			{
				return;
			}
			A_0.curY += 5f;
			GUI.color = new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(12f, A_0.curY, A_0.windowRect.width - 24f);
			GUI.color = Color.white;
			A_0.curY += 8f;
		}

		// Token: 0x0400169D RID: 5789
		private const float StatLabelColumnWidth = 100f;

		// Token: 0x0400169E RID: 5790
		private const float StatGutterColumnWidth = 10f;

		// Token: 0x0400169F RID: 5791
		private const float ScoreColumnWidth = 50f;

		// Token: 0x040016A0 RID: 5792
		private const float ScoreStageLabelColumnWidth = 160f;

		// Token: 0x040016A1 RID: 5793
		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		// Token: 0x040016A2 RID: 5794
		private static readonly Color UnrelatedStatColor = Color.gray;

		// Token: 0x040016A3 RID: 5795
		private const float DistFromMouse = 26f;

		// Token: 0x040016A4 RID: 5796
		public const float WindowPadding = 12f;

		// Token: 0x040016A5 RID: 5797
		private const float LineHeight = 23f;

		// Token: 0x040016A6 RID: 5798
		private const float FootnoteHeight = 23f;

		// Token: 0x040016A7 RID: 5799
		private const float TitleHeight = 30f;

		// Token: 0x040016A8 RID: 5800
		private const float SpaceBetweenLines = 2f;

		// Token: 0x040016A9 RID: 5801
		private const float SpaceBetweenColumns = 35f;
	}
}

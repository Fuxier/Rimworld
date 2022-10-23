using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C4 RID: 964
	public class Gizmo_RoomStats : Gizmo
	{
		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001B65 RID: 7013 RVA: 0x000A8956 File Offset: 0x000A6B56
		private Room Room
		{
			get
			{
				return Gizmo_RoomStats.GetRoomToShowStatsFor(this.building);
			}
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x000A8963 File Offset: 0x000A6B63
		public Gizmo_RoomStats(Building building)
		{
			this.building = building;
			this.Order = -100f;
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x000A897D File Offset: 0x000A6B7D
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(300f, maxWidth);
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x000A898C File Offset: 0x000A6B8C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Room room = this.Room;
			if (room == null)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Widgets.DrawWindowBackground(rect);
			Text.WordWrap = false;
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero().ContractedBy(10f);
			Text.Font = GameFont.Small;
			Rect rect3 = new Rect(rect2.x, rect2.y - 2f, rect2.width, 100f);
			float stat = room.GetStat(RoomStatDefOf.Impressiveness);
			RoomStatScoreStage scoreStage = RoomStatDefOf.Impressiveness.GetScoreStage(stat);
			string str = string.Concat(new string[]
			{
				room.Role.PostProcessedLabelCap,
				", ",
				scoreStage.label,
				" (",
				RoomStatDefOf.Impressiveness.ScoreToString(stat),
				")"
			});
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			float num = rect2.y + Text.LineHeight + Text.SpaceBetweenLines + 7f;
			GUI.color = Gizmo_RoomStats.RoomStatsColor;
			Text.Font = GameFont.Tiny;
			List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
			int num2 = 0;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (!allDefsListForReading[i].isHidden && allDefsListForReading[i] != RoomStatDefOf.Impressiveness)
				{
					float stat2 = room.GetStat(allDefsListForReading[i]);
					RoomStatScoreStage scoreStage2 = allDefsListForReading[i].GetScoreStage(stat2);
					Rect rect4;
					if (num2 % 2 == 0)
					{
						rect4 = new Rect(rect2.x, num, rect2.width / 2f, 100f);
					}
					else
					{
						rect4 = new Rect(rect2.x + rect2.width / 2f, num, rect2.width / 2f, 100f);
					}
					string str2 = scoreStage2.label.CapitalizeFirst() + " (" + allDefsListForReading[i].ScoreToString(stat2) + ")";
					Widgets.Label(rect4, str2.Truncate(rect4.width, null));
					if (num2 % 2 == 1)
					{
						num += Text.LineHeight + Text.SpaceBetweenLines;
					}
					num2++;
				}
			}
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Widgets.EndGroup();
			Text.WordWrap = true;
			GenUI.AbsorbClicksInRect(rect);
			if (Mouse.IsOver(rect))
			{
				Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(false, true);
				Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.Super, delegate
				{
					float num3 = 12f;
					EnvironmentStatsDrawer.DoRoomInfo(room, ref num3, windowRect);
				}, true, false, 1f, null);
				return new GizmoResult(GizmoState.Mouseover);
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x000A8C84 File Offset: 0x000A6E84
		public override void GizmoUpdateOnMouseover()
		{
			base.GizmoUpdateOnMouseover();
			Room room = this.Room;
			if (room != null)
			{
				room.DrawFieldEdges();
			}
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x000A8CA8 File Offset: 0x000A6EA8
		public static Room GetRoomToShowStatsFor(Building building)
		{
			if (!building.Spawned || building.Fogged())
			{
				return null;
			}
			Room room = null;
			if (building.def.passability != Traversability.Impassable)
			{
				room = building.GetRoom(RegionType.Set_All);
			}
			else if (building.def.hasInteractionCell)
			{
				room = building.InteractionCell.GetRoom(building.Map);
			}
			else
			{
				CellRect cellRect = building.OccupiedRect().ExpandedBy(1);
				foreach (IntVec3 intVec in cellRect)
				{
					if (cellRect.IsOnEdge(intVec))
					{
						room = intVec.GetRoom(building.Map);
						if (Gizmo_RoomStats.<GetRoomToShowStatsFor>g__IsValid|8_0(room))
						{
							break;
						}
					}
				}
			}
			if (!Gizmo_RoomStats.<GetRoomToShowStatsFor>g__IsValid|8_0(room))
			{
				return null;
			}
			return room;
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x000A8D9B File Offset: 0x000A6F9B
		[CompilerGenerated]
		internal static bool <GetRoomToShowStatsFor>g__IsValid|8_0(Room r)
		{
			return r != null && !r.Fogged && r.Role != RoomRoleDefOf.None;
		}

		// Token: 0x040013D4 RID: 5076
		private Building building;

		// Token: 0x040013D5 RID: 5077
		private static readonly Color RoomStatsColor = new Color(0.75f, 0.75f, 0.75f);
	}
}

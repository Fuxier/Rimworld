using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004E1 RID: 1249
	public static class ReorderableWidget
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x000EF0D0 File Offset: 0x000ED2D0
		public static bool Dragging
		{
			get
			{
				return ReorderableWidget.dragBegun;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x000EF0D7 File Offset: 0x000ED2D7
		public static int GetDraggedIndex
		{
			get
			{
				return ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable);
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x000EF0E3 File Offset: 0x000ED2E3
		public static int GetDraggedFromGroupID
		{
			get
			{
				return ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
			}
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x000EF0FC File Offset: 0x000ED2FC
		public static void ReorderableWidgetOnGUI_BeforeWindowStack()
		{
			if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0 && ReorderableWidget.draggingReorderable < ReorderableWidget.reorderables.Count)
			{
				int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
				if (groupID >= 0 && groupID < ReorderableWidget.groups.Count && ReorderableWidget.groups[groupID].extraDraggedItemOnGUI != null)
				{
					ReorderableWidget.groups[groupID].extraDraggedItemOnGUI(ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable), ReorderableWidget.dragStartPos);
				}
			}
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000EF188 File Offset: 0x000ED388
		public static void ReorderableWidgetOnGUI_AfterWindowStack()
		{
			if (Event.current.rawType == EventType.MouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (ReorderableWidget.clicked)
				{
					ReorderableWidget.StopDragging();
					for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
					{
						if (ReorderableWidget.reorderables[i].groupID == ReorderableWidget.groupClicked && ReorderableWidget.reorderables[i].rect == ReorderableWidget.clickedInRect)
						{
							ReorderableWidget.draggingReorderable = i;
							ReorderableWidget.dragStartPos = Event.current.mousePosition;
							break;
						}
					}
					ReorderableWidget.clicked = false;
				}
				if (ReorderableWidget.draggingReorderable >= ReorderableWidget.reorderables.Count)
				{
					ReorderableWidget.StopDragging();
				}
				if (ReorderableWidget.reorderables.Count != ReorderableWidget.lastFrameReorderableCount)
				{
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastInsertNear = ReorderableWidget.CurrentInsertNear(out ReorderableWidget.lastInsertNearLeft);
				ReorderableWidget.hoveredGroup = -1;
				for (int j = 0; j < ReorderableWidget.groups.Count; j++)
				{
					if (ReorderableWidget.groups[j].absRect.Contains(Event.current.mousePosition))
					{
						ReorderableWidget.hoveredGroup = j;
						if (ReorderableWidget.lastInsertNear >= 0 && ReorderableWidget.AreInMultiGroup(j, ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].groupID) && ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].groupID != j)
						{
							ReorderableWidget.lastInsertNear = ReorderableWidget.FindLastReorderableIndexWithinGroup(j);
							ReorderableWidget.lastInsertNearLeft = (ReorderableWidget.lastInsertNear < 0);
						}
					}
				}
				if (ReorderableWidget.released)
				{
					ReorderableWidget.released = false;
					if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0)
					{
						int indexWithinGroup = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable);
						int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
						int num;
						if (ReorderableWidget.lastInsertNear == ReorderableWidget.draggingReorderable)
						{
							num = indexWithinGroup;
						}
						else if (ReorderableWidget.lastInsertNearLeft)
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear);
						}
						else
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear) + 1;
						}
						int num2 = -1;
						if (ReorderableWidget.lastInsertNear >= 0)
						{
							num2 = ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].groupID;
						}
						if (ReorderableWidget.AreInMultiGroup(groupID, ReorderableWidget.hoveredGroup) && ReorderableWidget.hoveredGroup >= 0 && ReorderableWidget.hoveredGroup != num2)
						{
							num2 = ReorderableWidget.hoveredGroup;
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.FindLastReorderableIndexWithinGroup(num2)) + 1;
						}
						if (ReorderableWidget.AreInMultiGroup(groupID, num2))
						{
							ReorderableWidget.GetMultiGroupByGroupID(groupID).Value.reorderedAction(indexWithinGroup, groupID, num, num2);
							SoundDefOf.DropElement.PlayOneShotOnCamera(null);
						}
						else if (num >= 0 && num != indexWithinGroup && num != indexWithinGroup + 1)
						{
							SoundDefOf.DropElement.PlayOneShotOnCamera(null);
							try
							{
								ReorderableWidget.groups[ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID].reorderedAction(indexWithinGroup, num);
							}
							catch (Exception ex)
							{
								Log.Error(string.Concat(new object[]
								{
									"Could not reorder elements (from ",
									indexWithinGroup,
									" to ",
									num,
									"): ",
									ex
								}));
							}
						}
					}
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastFrameReorderableCount = ReorderableWidget.reorderables.Count;
				ReorderableWidget.multiGroups.Clear();
				ReorderableWidget.groups.Clear();
				ReorderableWidget.reorderables.Clear();
			}
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x000EF4E4 File Offset: 0x000ED6E4
		public static int NewGroup(Action<int, int> reorderedAction, ReorderableDirection direction, Rect rect, float drawLineExactlyBetween_space = -1f, Action<int, Vector2> extraDraggedItemOnGUI = null, bool playSoundOnStartReorder = true)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return -1;
			}
			int count = ReorderableWidget.groups.Count;
			ReorderableWidget.ReorderableGroup item = default(ReorderableWidget.ReorderableGroup);
			item.reorderedAction = reorderedAction;
			item.direction = direction;
			item.absRect = new Rect(UI.GUIToScreenPoint(Vector2.zero), rect.size);
			item.drawLineExactlyBetween_space = drawLineExactlyBetween_space;
			item.extraDraggedItemOnGUI = extraDraggedItemOnGUI;
			item.playSoundOnStartReorder = playSoundOnStartReorder;
			ReorderableWidget.groups.Add(item);
			if (ReorderableWidget.draggingReorderable >= 0 && ReorderableWidget.hoveredGroup == count && ReorderableWidget.lastInsertNear == -1)
			{
				ReorderableWidget.DrawLine(count, new Rect(0f, 0f, rect.width, rect.height));
			}
			return count;
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x000EF5A4 File Offset: 0x000ED7A4
		public static int NewMultiGroup(List<int> includedGroups, Action<int, int, int, int> reorderedAction)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return -1;
			}
			ReorderableWidget.ReorderableMultiGroup item = default(ReorderableWidget.ReorderableMultiGroup);
			item.includedGroups = includedGroups;
			item.reorderedAction = reorderedAction;
			ReorderableWidget.multiGroups.Add(item);
			return ReorderableWidget.multiGroups.Count - 1;
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x000EF5F0 File Offset: 0x000ED7F0
		public static bool Reorderable(int groupID, Rect rect, bool useRightButton = false, bool highlightDragged = true)
		{
			if (Event.current.type == EventType.Repaint)
			{
				ReorderableWidget.ReorderableInstance item = default(ReorderableWidget.ReorderableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				ReorderableWidget.reorderables.Add(item);
				int num = ReorderableWidget.reorderables.Count - 1;
				if (ReorderableWidget.draggingReorderable != -1 && (ReorderableWidget.dragBegun || (Vector2.Distance(ReorderableWidget.clickedAt, Event.current.mousePosition) > 5f && ReorderableWidget.groupClicked == groupID)))
				{
					if (!ReorderableWidget.dragBegun)
					{
						if (groupID >= 0 && groupID < ReorderableWidget.groups.Count && ReorderableWidget.groups[groupID].playSoundOnStartReorder)
						{
							SoundDefOf.DragElement.PlayOneShotOnCamera(null);
						}
						ReorderableWidget.dragBegun = true;
					}
					if (highlightDragged && ReorderableWidget.draggingReorderable == num)
					{
						GUI.color = ReorderableWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
					if (ReorderableWidget.lastInsertNear == num && groupID >= 0 && groupID < ReorderableWidget.groups.Count)
					{
						Rect rect2 = ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].rect;
						ReorderableWidget.DrawLine(groupID, rect2);
					}
				}
				return ReorderableWidget.draggingReorderable == num && ReorderableWidget.dragBegun;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.MouseDown && ((useRightButton && Event.current.button == 1) || (!useRightButton && Event.current.button == 0)) && Mouse.IsOver(rect))
			{
				ReorderableWidget.clicked = true;
				ReorderableWidget.clickedAt = Event.current.mousePosition;
				ReorderableWidget.groupClicked = groupID;
				ReorderableWidget.clickedInRect = rect;
			}
			return false;
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000EF7A4 File Offset: 0x000ED9A4
		private static int CurrentInsertNear(out bool toTheLeft)
		{
			toTheLeft = false;
			if (ReorderableWidget.draggingReorderable < 0)
			{
				return -1;
			}
			int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
			if (groupID < 0 || groupID >= ReorderableWidget.groups.Count)
			{
				Log.ErrorOnce("Reorderable used invalid group.", 1968375560);
				return -1;
			}
			int num = -1;
			for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
			{
				ReorderableWidget.ReorderableInstance reorderableInstance = ReorderableWidget.reorderables[i];
				if ((reorderableInstance.groupID == groupID || ReorderableWidget.AreInMultiGroup(reorderableInstance.groupID, groupID)) && (num == -1 || Event.current.mousePosition.DistanceToRect(reorderableInstance.absRect) < Event.current.mousePosition.DistanceToRect(ReorderableWidget.reorderables[num].absRect)))
				{
					num = i;
				}
			}
			if (num >= 0)
			{
				ReorderableWidget.ReorderableInstance reorderableInstance2 = ReorderableWidget.reorderables[num];
				if (ReorderableWidget.groups[reorderableInstance2.groupID].direction == ReorderableDirection.Horizontal)
				{
					toTheLeft = (Event.current.mousePosition.x < reorderableInstance2.absRect.center.x);
				}
				else
				{
					toTheLeft = (Event.current.mousePosition.y < reorderableInstance2.absRect.center.y);
				}
			}
			return num;
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000EF8E0 File Offset: 0x000EDAE0
		private static void DrawLine(int groupID, Rect r)
		{
			ReorderableWidget.ReorderableGroup reorderableGroup = ReorderableWidget.groups[groupID];
			if (reorderableGroup.DrawLineExactlyBetween)
			{
				if (reorderableGroup.direction == ReorderableDirection.Horizontal)
				{
					r.xMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
					r.xMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
				}
				else
				{
					r.yMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
					r.yMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
				}
			}
			GUI.color = ReorderableWidget.LineColor;
			if (reorderableGroup.direction == ReorderableDirection.Horizontal)
			{
				if (ReorderableWidget.lastInsertNearLeft)
				{
					Widgets.DrawLine(r.position, new Vector2(r.x, r.yMax), ReorderableWidget.LineColor, 2f);
				}
				else
				{
					Widgets.DrawLine(new Vector2(r.xMax, r.y), new Vector2(r.xMax, r.yMax), ReorderableWidget.LineColor, 2f);
				}
			}
			else if (ReorderableWidget.lastInsertNearLeft)
			{
				Widgets.DrawLine(r.position, new Vector2(r.xMax, r.y), ReorderableWidget.LineColor, 2f);
			}
			else
			{
				Widgets.DrawLine(new Vector2(r.x, r.yMax), new Vector2(r.xMax, r.yMax), ReorderableWidget.LineColor, 2f);
			}
			GUI.color = Color.white;
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000EFA64 File Offset: 0x000EDC64
		private static int GetIndexWithinGroup(int index)
		{
			if (index < 0 || index >= ReorderableWidget.reorderables.Count)
			{
				return -1;
			}
			int num = -1;
			for (int i = 0; i <= index; i++)
			{
				if (ReorderableWidget.reorderables[i].groupID == ReorderableWidget.reorderables[index].groupID)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000EFABC File Offset: 0x000EDCBC
		private static int FindLastReorderableIndexWithinGroup(int groupID)
		{
			if (groupID < 0 || groupID >= ReorderableWidget.groups.Count)
			{
				return -1;
			}
			int result = -1;
			for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
			{
				if (ReorderableWidget.reorderables[i].groupID == groupID)
				{
					result = i;
				}
			}
			return result;
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x000EFB0C File Offset: 0x000EDD0C
		private static ReorderableWidget.ReorderableMultiGroup? GetMultiGroupByGroupID(int groupID)
		{
			foreach (ReorderableWidget.ReorderableMultiGroup reorderableMultiGroup in ReorderableWidget.multiGroups)
			{
				if (reorderableMultiGroup.includedGroups.Contains(groupID))
				{
					return new ReorderableWidget.ReorderableMultiGroup?(reorderableMultiGroup);
				}
			}
			return null;
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000EFB7C File Offset: 0x000EDD7C
		private static bool AreInMultiGroup(int groupA, int groupB)
		{
			ReorderableWidget.ReorderableMultiGroup? multiGroupByGroupID = ReorderableWidget.GetMultiGroupByGroupID(groupA);
			return multiGroupByGroupID != null && groupA != groupB && multiGroupByGroupID.Value.includedGroups.Contains(groupB);
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x000EFBB1 File Offset: 0x000EDDB1
		private static void StopDragging()
		{
			ReorderableWidget.draggingReorderable = -1;
			ReorderableWidget.dragStartPos = default(Vector2);
			ReorderableWidget.lastInsertNear = -1;
			ReorderableWidget.dragBegun = false;
		}

		// Token: 0x0400181A RID: 6170
		private static List<ReorderableWidget.ReorderableGroup> groups = new List<ReorderableWidget.ReorderableGroup>();

		// Token: 0x0400181B RID: 6171
		private static List<ReorderableWidget.ReorderableMultiGroup> multiGroups = new List<ReorderableWidget.ReorderableMultiGroup>();

		// Token: 0x0400181C RID: 6172
		private static List<ReorderableWidget.ReorderableInstance> reorderables = new List<ReorderableWidget.ReorderableInstance>();

		// Token: 0x0400181D RID: 6173
		private static int draggingReorderable = -1;

		// Token: 0x0400181E RID: 6174
		private static Vector2 dragStartPos;

		// Token: 0x0400181F RID: 6175
		private static bool clicked;

		// Token: 0x04001820 RID: 6176
		private static bool released;

		// Token: 0x04001821 RID: 6177
		private static bool dragBegun;

		// Token: 0x04001822 RID: 6178
		private static Vector2 clickedAt;

		// Token: 0x04001823 RID: 6179
		private static int groupClicked;

		// Token: 0x04001824 RID: 6180
		private static Rect clickedInRect;

		// Token: 0x04001825 RID: 6181
		private static int lastInsertNear = -1;

		// Token: 0x04001826 RID: 6182
		private static int hoveredGroup = -1;

		// Token: 0x04001827 RID: 6183
		private static bool lastInsertNearLeft;

		// Token: 0x04001828 RID: 6184
		private static int lastFrameReorderableCount = -1;

		// Token: 0x04001829 RID: 6185
		private const float MinMouseMoveToHighlightReorderable = 5f;

		// Token: 0x0400182A RID: 6186
		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x0400182B RID: 6187
		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x0400182C RID: 6188
		private const float LineWidth = 2f;

		// Token: 0x020020CF RID: 8399
		private struct ReorderableGroup
		{
			// Token: 0x17001F2D RID: 7981
			// (get) Token: 0x0600C534 RID: 50484 RVA: 0x0043B743 File Offset: 0x00439943
			public bool DrawLineExactlyBetween
			{
				get
				{
					return this.drawLineExactlyBetween_space > 0f;
				}
			}

			// Token: 0x0400823F RID: 33343
			public Action<int, int> reorderedAction;

			// Token: 0x04008240 RID: 33344
			public ReorderableDirection direction;

			// Token: 0x04008241 RID: 33345
			public float drawLineExactlyBetween_space;

			// Token: 0x04008242 RID: 33346
			public Action<int, Vector2> extraDraggedItemOnGUI;

			// Token: 0x04008243 RID: 33347
			public Rect absRect;

			// Token: 0x04008244 RID: 33348
			public bool playSoundOnStartReorder;
		}

		// Token: 0x020020D0 RID: 8400
		private struct ReorderableMultiGroup
		{
			// Token: 0x04008245 RID: 33349
			public Action<int, int, int, int> reorderedAction;

			// Token: 0x04008246 RID: 33350
			public List<int> includedGroups;
		}

		// Token: 0x020020D1 RID: 8401
		private struct ReorderableInstance
		{
			// Token: 0x04008247 RID: 33351
			public int groupID;

			// Token: 0x04008248 RID: 33352
			public Rect rect;

			// Token: 0x04008249 RID: 33353
			public Rect absRect;
		}
	}
}

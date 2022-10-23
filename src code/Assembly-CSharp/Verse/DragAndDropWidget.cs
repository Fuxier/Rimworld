using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004D3 RID: 1235
	public static class DragAndDropWidget
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002536 RID: 9526 RVA: 0x000EC5C2 File Offset: 0x000EA7C2
		public static bool Dragging
		{
			get
			{
				return DragAndDropWidget.dragBegun;
			}
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000EC5CC File Offset: 0x000EA7CC
		public static void DragAndDropWidgetOnGUI_BeforeWindowStack()
		{
			if (DragAndDropWidget.dragBegun && DragAndDropWidget.draggingDraggable >= 0 && DragAndDropWidget.draggingDraggable < DragAndDropWidget.draggables.Count)
			{
				int groupID = DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].groupID;
				if (groupID >= 0 && groupID < DragAndDropWidget.groups.Count && DragAndDropWidget.groups[groupID].extraDraggedItemOnGUI != null)
				{
					DragAndDropWidget.groups[groupID].extraDraggedItemOnGUI(DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].context, DragAndDropWidget.dragStartPos);
				}
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000EC660 File Offset: 0x000EA860
		public static void DragAndDropWidgetOnGUI_AfterWindowStack()
		{
			if (Event.current.rawType == EventType.MouseUp)
			{
				DragAndDropWidget.released = true;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (DragAndDropWidget.clicked)
				{
					DragAndDropWidget.StopDragging();
					for (int i = 0; i < DragAndDropWidget.draggables.Count; i++)
					{
						if (DragAndDropWidget.draggables[i].rect == DragAndDropWidget.clickedInRect)
						{
							DragAndDropWidget.draggingDraggable = i;
							Action onStartDragging = DragAndDropWidget.draggables[i].onStartDragging;
							if (onStartDragging != null)
							{
								onStartDragging();
							}
							DragAndDropWidget.dragStartPos = Event.current.mousePosition;
							break;
						}
					}
					DragAndDropWidget.mouseIsDown = true;
					DragAndDropWidget.clicked = false;
				}
				if (DragAndDropWidget.draggingDraggable >= DragAndDropWidget.draggables.Count)
				{
					DragAndDropWidget.StopDragging();
				}
				if (DragAndDropWidget.draggables.Count != DragAndDropWidget.lastFrameDraggableCount)
				{
					DragAndDropWidget.StopDragging();
				}
				if (DragAndDropWidget.released)
				{
					DragAndDropWidget.released = false;
					if (!DragAndDropWidget.dragBegun && DragAndDropWidget.mouseIsDown)
					{
						foreach (DragAndDropWidget.DraggableInstance draggableInstance in DragAndDropWidget.draggables)
						{
							Rect absRect = draggableInstance.absRect;
							if (absRect.Contains(Event.current.mousePosition) && draggableInstance.clickHandler != null)
							{
								draggableInstance.clickHandler();
							}
						}
					}
					DragAndDropWidget.mouseIsDown = false;
					if (DragAndDropWidget.dragBegun && DragAndDropWidget.draggingDraggable >= 0)
					{
						DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable];
						DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
						for (int j = DragAndDropWidget.dropAreas.Count - 1; j >= 0; j--)
						{
							DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[j];
							if (draggableInstance2.groupID == dropAreaInstance2.groupID && dropAreaInstance2.absRect.Contains(Event.current.mousePosition))
							{
								dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
							}
						}
						if (dropAreaInstance != null)
						{
							Action<object> onDrop = dropAreaInstance.Value.onDrop;
							if (onDrop != null)
							{
								onDrop(draggableInstance2.context);
							}
						}
						else
						{
							SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
						}
					}
					DragAndDropWidget.StopDragging();
				}
				DragAndDropWidget.lastFrameDraggableCount = DragAndDropWidget.draggables.Count;
				DragAndDropWidget.groups.Clear();
				DragAndDropWidget.draggables.Clear();
				DragAndDropWidget.dropAreas.Clear();
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000EC8B4 File Offset: 0x000EAAB4
		public static int NewGroup(Action<object, Vector2> extraDraggedItemOnGUI = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return -1;
			}
			DragAndDropWidget.DraggableGroup item = default(DragAndDropWidget.DraggableGroup);
			item.extraDraggedItemOnGUI = extraDraggedItemOnGUI;
			DragAndDropWidget.groups.Add(item);
			return DragAndDropWidget.groups.Count - 1;
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000EC8F8 File Offset: 0x000EAAF8
		public static bool Draggable(int groupID, Rect rect, object context, Action clickHandler = null, Action onStartDragging = null)
		{
			if (Event.current.type == EventType.Repaint)
			{
				DragAndDropWidget.DraggableInstance item = default(DragAndDropWidget.DraggableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.context = context;
				item.clickHandler = clickHandler;
				item.onStartDragging = onStartDragging;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				DragAndDropWidget.draggables.Add(item);
				int num = DragAndDropWidget.draggables.Count - 1;
				if (DragAndDropWidget.draggingDraggable != -1 && (DragAndDropWidget.dragBegun || Vector2.Distance(DragAndDropWidget.clickedAt, Event.current.mousePosition) > 5f))
				{
					if (!DragAndDropWidget.dragBegun)
					{
						SoundDefOf.DragElement.PlayOneShotOnCamera(null);
						DragAndDropWidget.dragBegun = true;
					}
					if (DragAndDropWidget.draggingDraggable == num)
					{
						GUI.color = DragAndDropWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
				}
				return DragAndDropWidget.draggingDraggable == num && DragAndDropWidget.dragBegun;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				DragAndDropWidget.released = true;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragAndDropWidget.clicked = true;
				DragAndDropWidget.clickedAt = Event.current.mousePosition;
				DragAndDropWidget.clickedInRect = rect;
			}
			return false;
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000ECA40 File Offset: 0x000EAC40
		public static void DropArea(int groupID, Rect rect, Action<object> onDrop, object context)
		{
			if (Event.current.type == EventType.Repaint)
			{
				DragAndDropWidget.DropAreaInstance item = default(DragAndDropWidget.DropAreaInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.onDrop = onDrop;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				item.context = context;
				DragAndDropWidget.dropAreas.Add(item);
			}
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000ECAAC File Offset: 0x000EACAC
		public static object CurrentlyDraggedDraggable()
		{
			if (!DragAndDropWidget.dragBegun || DragAndDropWidget.draggingDraggable < 0)
			{
				return null;
			}
			return DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].context;
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000ECAD4 File Offset: 0x000EACD4
		public static object HoveringDropArea(int groupID)
		{
			DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
			for (int i = DragAndDropWidget.dropAreas.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[i];
				if (groupID == dropAreaInstance2.groupID && dropAreaInstance2.rect.Contains(Event.current.mousePosition))
				{
					dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
				}
			}
			if (dropAreaInstance == null)
			{
				return null;
			}
			return dropAreaInstance.Value.context;
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000ECB4C File Offset: 0x000EAD4C
		public static Rect? HoveringDropAreaRect(int groupID, Vector3? mousePos = null)
		{
			Vector3 point = mousePos ?? Event.current.mousePosition;
			DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
			for (int i = DragAndDropWidget.dropAreas.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[i];
				if (groupID == dropAreaInstance2.groupID && dropAreaInstance2.rect.Contains(point))
				{
					dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
				}
			}
			if (dropAreaInstance == null)
			{
				return null;
			}
			return new Rect?(dropAreaInstance.GetValueOrDefault().rect);
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000ECBF0 File Offset: 0x000EADF0
		public static object DraggableAt(int groupID, Vector3 mousePos)
		{
			DragAndDropWidget.DraggableInstance? draggableInstance = null;
			for (int i = DragAndDropWidget.draggables.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[i];
				if (groupID == draggableInstance2.groupID && draggableInstance2.rect.Contains(mousePos))
				{
					draggableInstance = new DragAndDropWidget.DraggableInstance?(draggableInstance2);
				}
			}
			if (draggableInstance == null)
			{
				return null;
			}
			return draggableInstance.Value.context;
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x000ECC60 File Offset: 0x000EAE60
		private static object GetDraggable(int groupID, Vector3 mousePosAbs, int direction)
		{
			float num = float.PositiveInfinity;
			DragAndDropWidget.DraggableInstance? draggableInstance = null;
			for (int i = DragAndDropWidget.draggables.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[i];
				if (groupID == draggableInstance2.groupID)
				{
					Rect absRect = draggableInstance2.absRect;
					if (mousePosAbs.y >= absRect.yMin && mousePosAbs.y <= absRect.yMax)
					{
						float num2 = (mousePosAbs.x - absRect.xMax) * (float)direction;
						if (num2 >= 0f && num2 < num)
						{
							num = num2;
							draggableInstance = new DragAndDropWidget.DraggableInstance?(draggableInstance2);
						}
					}
				}
			}
			if (draggableInstance == null)
			{
				return null;
			}
			return draggableInstance.Value.context;
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x000ECD10 File Offset: 0x000EAF10
		public static object GetDraggableBefore(int groupID, Vector3 mousePosAbs)
		{
			return DragAndDropWidget.GetDraggable(groupID, mousePosAbs, 1);
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000ECD1A File Offset: 0x000EAF1A
		public static object GetDraggableAfter(int groupID, Vector3 mousePosAbs)
		{
			return DragAndDropWidget.GetDraggable(groupID, mousePosAbs, -1);
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x000ECD24 File Offset: 0x000EAF24
		private static void StopDragging()
		{
			DragAndDropWidget.draggingDraggable = -1;
			DragAndDropWidget.dragStartPos = default(Vector2);
			DragAndDropWidget.dragBegun = false;
		}

		// Token: 0x040017D6 RID: 6102
		private static List<DragAndDropWidget.DraggableGroup> groups = new List<DragAndDropWidget.DraggableGroup>();

		// Token: 0x040017D7 RID: 6103
		private static List<DragAndDropWidget.DropAreaInstance> dropAreas = new List<DragAndDropWidget.DropAreaInstance>();

		// Token: 0x040017D8 RID: 6104
		private static List<DragAndDropWidget.DraggableInstance> draggables = new List<DragAndDropWidget.DraggableInstance>();

		// Token: 0x040017D9 RID: 6105
		private static int draggingDraggable = -1;

		// Token: 0x040017DA RID: 6106
		private static Vector2 dragStartPos;

		// Token: 0x040017DB RID: 6107
		private static bool mouseIsDown;

		// Token: 0x040017DC RID: 6108
		private static bool clicked;

		// Token: 0x040017DD RID: 6109
		private static bool released;

		// Token: 0x040017DE RID: 6110
		private static bool dragBegun;

		// Token: 0x040017DF RID: 6111
		private static Vector2 clickedAt;

		// Token: 0x040017E0 RID: 6112
		private static Rect clickedInRect;

		// Token: 0x040017E1 RID: 6113
		private static int lastFrameDraggableCount = -1;

		// Token: 0x040017E2 RID: 6114
		private const float MinMouseMoveToHighlightDraggable = 5f;

		// Token: 0x040017E3 RID: 6115
		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x040017E4 RID: 6116
		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x040017E5 RID: 6117
		private const float LineWidth = 2f;

		// Token: 0x020020C1 RID: 8385
		private struct DraggableGroup
		{
			// Token: 0x04008211 RID: 33297
			public Action<object, Vector2> extraDraggedItemOnGUI;
		}

		// Token: 0x020020C2 RID: 8386
		private struct DropAreaInstance
		{
			// Token: 0x04008212 RID: 33298
			public int groupID;

			// Token: 0x04008213 RID: 33299
			public Rect rect;

			// Token: 0x04008214 RID: 33300
			public Rect absRect;

			// Token: 0x04008215 RID: 33301
			public Action<object> onDrop;

			// Token: 0x04008216 RID: 33302
			public object context;
		}

		// Token: 0x020020C3 RID: 8387
		private struct DraggableInstance
		{
			// Token: 0x04008217 RID: 33303
			public int groupID;

			// Token: 0x04008218 RID: 33304
			public Rect rect;

			// Token: 0x04008219 RID: 33305
			public Rect absRect;

			// Token: 0x0400821A RID: 33306
			public object context;

			// Token: 0x0400821B RID: 33307
			public Action clickHandler;

			// Token: 0x0400821C RID: 33308
			public Action onStartDragging;
		}
	}
}

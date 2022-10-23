using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D5 RID: 1237
	public static class DragSliderManager
	{
		// Token: 0x06002549 RID: 9545 RVA: 0x000ECDB3 File Offset: 0x000EAFB3
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000ECDBB File Offset: 0x000EAFBB
		public static bool DragSlider(Rect rect, float rateFactor, DragSliderCallback newStartMethod, DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragSliderManager.lastRateFactor = rateFactor;
				newStartMethod(0f, rateFactor);
				DragSliderManager.StartDragSliding(newDraggingUpdateMethod, newCompletedMethod);
				return true;
			}
			return false;
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000ECDFA File Offset: 0x000EAFFA
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000ECE1D File Offset: 0x000EB01D
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000ECE30 File Offset: 0x000EB030
		public static void DragSlidersOnGUI()
		{
			if (DragSliderManager.dragging && Event.current.type == EventType.MouseUp && Event.current.button == 0)
			{
				DragSliderManager.dragging = false;
				if (DragSliderManager.completedMethod != null)
				{
					DragSliderManager.completedMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
				}
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x000ECE7E File Offset: 0x000EB07E
		public static void DragSlidersUpdate()
		{
			if (DragSliderManager.dragging && DragSliderManager.draggingUpdateMethod != null)
			{
				DragSliderManager.draggingUpdateMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
			}
		}

		// Token: 0x040017E6 RID: 6118
		private static bool dragging = false;

		// Token: 0x040017E7 RID: 6119
		private static float rootX;

		// Token: 0x040017E8 RID: 6120
		private static float lastRateFactor = 1f;

		// Token: 0x040017E9 RID: 6121
		private static DragSliderCallback draggingUpdateMethod;

		// Token: 0x040017EA RID: 6122
		private static DragSliderCallback completedMethod;
	}
}

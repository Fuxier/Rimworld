using System;

namespace Verse
{
	// Token: 0x020004EC RID: 1260
	internal static class DraggableResultUtility
	{
		// Token: 0x06002689 RID: 9865 RVA: 0x000F7392 File Offset: 0x000F5592
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}

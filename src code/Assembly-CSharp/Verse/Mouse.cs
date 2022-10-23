using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D9 RID: 1241
	public static class Mouse
	{
		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002590 RID: 9616 RVA: 0x000EECC4 File Offset: 0x000ECEC4
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000EED07 File Offset: 0x000ECF07
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}

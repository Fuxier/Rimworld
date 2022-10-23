using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C4 RID: 1220
	public class ScreenshotModeHandler
	{
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060024CF RID: 9423 RVA: 0x000EA99E File Offset: 0x000E8B9E
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060024D0 RID: 9424 RVA: 0x000EA9A8 File Offset: 0x000E8BA8
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000EAA04 File Offset: 0x000E8C04
		public void ScreenshotModesOnGUI()
		{
			if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent)
			{
				this.active = !this.active;
				Event.current.Use();
			}
		}

		// Token: 0x0400179E RID: 6046
		private bool active;
	}
}

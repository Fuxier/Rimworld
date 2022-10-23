using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000492 RID: 1170
	public static class GUIEventFilterForOSX
	{
		// Token: 0x0600235B RID: 9051 RVA: 0x000E28EC File Offset: 0x000E0AEC
		public static void CheckRejectGUIEvent()
		{
			if (UnityData.platform != RuntimePlatform.OSXPlayer)
			{
				return;
			}
			if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.MouseUp)
			{
				return;
			}
			if (Time.frameCount != GUIEventFilterForOSX.lastRecordedFrame)
			{
				GUIEventFilterForOSX.eventsThisFrame.Clear();
				GUIEventFilterForOSX.lastRecordedFrame = Time.frameCount;
			}
			for (int i = 0; i < GUIEventFilterForOSX.eventsThisFrame.Count; i++)
			{
				if (GUIEventFilterForOSX.EventsAreEquivalent(GUIEventFilterForOSX.eventsThisFrame[i], Event.current))
				{
					GUIEventFilterForOSX.RejectEvent();
				}
			}
			GUIEventFilterForOSX.eventsThisFrame.Add(Event.current);
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x000E297C File Offset: 0x000E0B7C
		private static bool EventsAreEquivalent(Event A, Event B)
		{
			return A.button == B.button && A.keyCode == B.keyCode && A.type == B.type;
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x000E29AC File Offset: 0x000E0BAC
		private static void RejectEvent()
		{
			if (DebugViewSettings.logInput)
			{
				Log.Message(string.Concat(new object[]
				{
					"Frame ",
					Time.frameCount,
					": REJECTED ",
					Event.current.ToStringFull()
				}));
			}
			Event.current.Use();
		}

		// Token: 0x040016B3 RID: 5811
		private static List<Event> eventsThisFrame = new List<Event>();

		// Token: 0x040016B4 RID: 5812
		private static int lastRecordedFrame = -1;
	}
}

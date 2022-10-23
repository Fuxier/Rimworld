using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DE RID: 1246
	public static class OriginalEventUtility
	{
		// Token: 0x0600259E RID: 9630 RVA: 0x000EF030 File Offset: 0x000ED230
		public static void RecordOriginalEvent(Event e)
		{
			OriginalEventUtility.originalType = new EventType?(e.type);
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x000EF044 File Offset: 0x000ED244
		public static EventType EventType
		{
			get
			{
				EventType? eventType = OriginalEventUtility.originalType;
				if (eventType == null)
				{
					return Event.current.rawType;
				}
				return eventType.GetValueOrDefault();
			}
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x000EF072 File Offset: 0x000ED272
		public static void Reset()
		{
			OriginalEventUtility.originalType = null;
		}

		// Token: 0x04001816 RID: 6166
		private static EventType? originalType;
	}
}

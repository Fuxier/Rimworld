using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004BE RID: 1214
	public static class MessagesRepeatAvoider
	{
		// Token: 0x060024BC RID: 9404 RVA: 0x000E9F55 File Offset: 0x000E8155
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000E9F64 File Offset: 0x000E8164
		public static bool MessageShowAllowed(string tag, float minSecondsSinceLastShow)
		{
			float num;
			if (!MessagesRepeatAvoider.lastShowTimes.TryGetValue(tag, out num))
			{
				num = -99999f;
			}
			bool flag = RealTime.LastRealTime > num + minSecondsSinceLastShow;
			if (flag)
			{
				MessagesRepeatAvoider.lastShowTimes[tag] = RealTime.LastRealTime;
			}
			return flag;
		}

		// Token: 0x0400178C RID: 6028
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();
	}
}

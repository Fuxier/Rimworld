using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000425 RID: 1061
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x06001F3B RID: 7995 RVA: 0x000BA3F4 File Offset: 0x000B85F4
		public static void DelayedErrorWindowRequestOnGUI()
		{
			try
			{
				for (int i = 0; i < DelayedErrorWindowRequest.requests.Count; i++)
				{
					Find.WindowStack.Add(new Dialog_MessageBox(DelayedErrorWindowRequest.requests[i].text, "OK".Translate(), null, null, null, DelayedErrorWindowRequest.requests[i].title, false, null, null, WindowLayer.Dialog));
				}
			}
			finally
			{
				DelayedErrorWindowRequest.requests.Clear();
			}
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x000BA480 File Offset: 0x000B8680
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x0400153E RID: 5438
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x02001EBE RID: 7870
		private struct Request
		{
			// Token: 0x04007917 RID: 30999
			public string text;

			// Token: 0x04007918 RID: 31000
			public string title;
		}
	}
}

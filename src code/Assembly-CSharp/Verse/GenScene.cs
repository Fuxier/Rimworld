using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000173 RID: 371
	public static class GenScene
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00031214 File Offset: 0x0002F414
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00031238 File Offset: 0x0002F438
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0003125C File Offset: 0x0002F45C
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null, false);
		}

		// Token: 0x04000A2F RID: 2607
		public const string EntrySceneName = "Entry";

		// Token: 0x04000A30 RID: 2608
		public const string PlaySceneName = "Play";
	}
}

using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000174 RID: 372
	public static class QuickStarter
	{
		// Token: 0x06000A1B RID: 2587 RVA: 0x00031294 File Offset: 0x0002F494
		public static bool CheckQuickStart()
		{
			if (GenCommandLine.CommandLineArgPassed("quicktest") && !QuickStarter.quickStarted && GenScene.InEntryScene)
			{
				QuickStarter.quickStarted = true;
				SceneManager.LoadScene("Play");
				return true;
			}
			return false;
		}

		// Token: 0x04000A31 RID: 2609
		private static bool quickStarted;
	}
}

using System;

namespace Verse
{
	// Token: 0x020003B6 RID: 950
	public static class Scribe
	{
		// Token: 0x06001B0C RID: 6924 RVA: 0x000A54C7 File Offset: 0x000A36C7
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000A54E4 File Offset: 0x000A36E4
		public static bool EnterNode(string nodeName)
		{
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				return false;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return Scribe.saver.EnterNode(nodeName);
			}
			return (Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit) || Scribe.loader.EnterNode(nodeName);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000A5533 File Offset: 0x000A3733
		public static void ExitNode()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.saver.ExitNode();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.loader.ExitNode();
			}
		}

		// Token: 0x040013B6 RID: 5046
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x040013B7 RID: 5047
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x040013B8 RID: 5048
		public static LoadSaveMode mode = LoadSaveMode.Inactive;
	}
}

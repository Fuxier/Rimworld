using System;
using System.Linq;

namespace Verse
{
	// Token: 0x0200022A RID: 554
	public static class GameAndMapInitExceptionHandlers
	{
		// Token: 0x06000FB4 RID: 4020 RVA: 0x0005B578 File Offset: 0x00059778
		public static void ErrorWhileLoadingAssets(Exception e)
		{
			string text = "ErrorWhileLoadingAssets".Translate();
			if (!ModsConfig.ActiveModsInLoadOrder.Any((ModMetaData x) => !x.Official))
			{
				if (ModsConfig.ActiveModsInLoadOrder.Any((ModMetaData x) => x.IsCoreMod))
				{
					goto IL_86;
				}
			}
			text += "\n\n" + "ErrorWhileLoadingAssets_ModsInfo".Translate();
			IL_86:
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingAssetsTitle".Translate());
			GenScene.GoToMainMenu();
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0005B625 File Offset: 0x00059825
		public static void ErrorWhileGeneratingMap(Exception e)
		{
			DelayedErrorWindowRequest.Add("ErrorWhileGeneratingMap".Translate(), "ErrorWhileGeneratingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0005B654 File Offset: 0x00059854
		public static void ErrorWhileLoadingGame(Exception e)
		{
			string text = "ErrorWhileLoadingMap".Translate();
			string value;
			string value2;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out value, out value2))
			{
				text += "\n\n" + "ModsMismatchWarningText".Translate(value, value2);
			}
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}
	}
}

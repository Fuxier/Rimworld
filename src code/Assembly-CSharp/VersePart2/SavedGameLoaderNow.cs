using System;
using System.Linq;

namespace Verse
{
	// Token: 0x0200022B RID: 555
	public class SavedGameLoaderNow
	{
		// Token: 0x06000FB7 RID: 4023 RVA: 0x0005B6C8 File Offset: 0x000598C8
		public static void LoadGameFromSaveFileNow(string fileName)
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.PackageIdPlayerFacing + ((!mod.ModMetaData.VersionCompatible) ? " (incompatible version)" : "")).ToLineList("  - ", false);
			Log.Message("Loading game from file " + fileName + " with mods:\n" + str);
			DeepProfiler.Start("Loading game from file " + fileName);
			Current.Game = new Game();
			DeepProfiler.Start("InitLoading (read file)");
			Scribe.loader.InitLoading(GenFilePaths.FilePathForSavedGame(fileName));
			DeepProfiler.End();
			try
			{
				ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Map, true);
				if (!Scribe.EnterNode("game"))
				{
					Log.Error("Could not find game XML node.");
					Scribe.ForceStop();
					return;
				}
				Current.Game = new Game();
				Current.Game.LoadGame();
			}
			catch (Exception)
			{
				Scribe.ForceStop();
				throw;
			}
			PermadeathModeUtility.CheckUpdatePermadeathModeUniqueNameOnGameLoad(fileName);
			DeepProfiler.End();
		}
	}
}

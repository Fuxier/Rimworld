using System;

namespace Verse
{
	// Token: 0x020003A5 RID: 933
	public static class PreLoadUtility
	{
		// Token: 0x06001AB3 RID: 6835 RVA: 0x000A25E8 File Offset: 0x000A07E8
		public static void CheckVersionAndLoad(string path, ScribeMetaHeaderUtility.ScribeHeaderMode mode, Action loadAct, bool skipOnMismatch = false)
		{
			try
			{
				Scribe.loader.InitLoadingMetaHeaderOnly(path);
				ScribeMetaHeaderUtility.LoadGameDataHeader(mode, false);
				Scribe.loader.FinalizeLoading();
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Exception loading ",
					path,
					": ",
					ex
				}));
				Scribe.ForceStop();
			}
			string text;
			string text2;
			if (skipOnMismatch && !ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out text, out text2))
			{
				return;
			}
			if (!ScribeMetaHeaderUtility.TryCreateDialogsForVersionMismatchWarnings(loadAct))
			{
				loadAct();
			}
		}
	}
}

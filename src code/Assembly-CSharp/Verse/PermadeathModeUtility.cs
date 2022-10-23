using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000589 RID: 1417
	public static class PermadeathModeUtility
	{
		// Token: 0x06002B35 RID: 11061 RVA: 0x001143F2 File Offset: 0x001125F2
		public static string GeneratePermadeathSaveName()
		{
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(GenFile.SanitizedFileName(NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null)), null);
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x00114417 File Offset: 0x00112617
		public static string GeneratePermadeathSaveNameBasedOnPlayerInput(string factionName, string acceptedNameEvenIfTaken = null)
		{
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(GenFile.SanitizedFileName(factionName), acceptedNameEvenIfTaken);
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x00114428 File Offset: 0x00112628
		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...");
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x00114478 File Offset: 0x00112678
		private static string NewPermadeathSaveNameWithAppendedNumberIfNecessary(string name, string acceptedNameEvenIfTaken = null)
		{
			int num = 0;
			string text;
			do
			{
				num++;
				text = name;
				if (num != 1)
				{
					text += num;
				}
				text = PermadeathModeUtility.AppendedPermadeathModeSuffix(text);
			}
			while (SaveGameFilesUtility.SavedGameNamedExists(text) && text != acceptedNameEvenIfTaken);
			return text;
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x001144B7 File Offset: 0x001126B7
		private static string AppendedPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}

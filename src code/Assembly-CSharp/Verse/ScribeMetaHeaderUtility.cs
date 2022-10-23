using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B7 RID: 951
	public class ScribeMetaHeaderUtility
	{
		// Token: 0x06001B10 RID: 6928 RVA: 0x000A5588 File Offset: 0x000A3788
		public static void WriteMetaHeader()
		{
			if (Scribe.EnterNode("meta"))
			{
				try
				{
					string currentVersionStringWithRev = VersionControl.CurrentVersionStringWithRev;
					Scribe_Values.Look<string>(ref currentVersionStringWithRev, "gameVersion", null, false);
					List<string> list = (from mod in LoadedModManager.RunningMods
					select mod.PackageId).ToList<string>();
					Scribe_Collections.Look<string>(ref list, "modIds", LookMode.Undefined, Array.Empty<object>());
					List<int> list2 = (from mod in LoadedModManager.RunningMods
					select mod.SteamAppId).ToList<int>();
					Scribe_Collections.Look<int>(ref list2, "modSteamIds", LookMode.Undefined, Array.Empty<object>());
					List<string> list3 = (from mod in LoadedModManager.RunningMods
					select mod.Name).ToList<string>();
					Scribe_Collections.Look<string>(ref list3, "modNames", LookMode.Undefined, Array.Empty<object>());
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x000A5694 File Offset: 0x000A3894
		public static void LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode mode, bool logVersionConflictWarning)
		{
			ScribeMetaHeaderUtility.loadedGameVersion = "Unknown";
			ScribeMetaHeaderUtility.loadedGameVersionMajor = 0;
			ScribeMetaHeaderUtility.loadedGameVersionMinor = 0;
			ScribeMetaHeaderUtility.loadedGameVersionBuild = 0;
			ScribeMetaHeaderUtility.loadedModIdsList = null;
			ScribeMetaHeaderUtility.loadedModNamesList = null;
			ScribeMetaHeaderUtility.lastMode = mode;
			if (Scribe.mode != LoadSaveMode.Inactive && Scribe.EnterNode("meta"))
			{
				try
				{
					Scribe_Values.Look<string>(ref ScribeMetaHeaderUtility.loadedGameVersion, "gameVersion", null, false);
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModIdsList, "modIds", LookMode.Undefined, Array.Empty<object>());
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModNamesList, "modNames", LookMode.Undefined, Array.Empty<object>());
					if (Scribe.mode == LoadSaveMode.LoadingVars && !ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty())
					{
						try
						{
							ScribeMetaHeaderUtility.loadedGameVersionMajor = VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
							ScribeMetaHeaderUtility.loadedGameVersionMinor = VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
							ScribeMetaHeaderUtility.loadedGameVersionBuild = VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
						}
						catch (Exception arg)
						{
							Log.Error("Error parsing loaded version. " + arg);
						}
					}
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (logVersionConflictWarning && (mode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map || !UnityData.isEditor) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				Log.Warning(string.Concat(new object[]
				{
					"Loaded file (",
					mode,
					") is from version ",
					ScribeMetaHeaderUtility.loadedGameVersion,
					", we are running version ",
					VersionControl.CurrentVersionStringWithRev,
					"."
				}));
			}
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x000A57FC File Offset: 0x000A39FC
		private static bool VersionsMatch()
		{
			return ScribeMetaHeaderUtility.loadedGameVersionBuild == VersionControl.BuildFromVersionString(VersionControl.CurrentVersionStringWithRev);
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000A5810 File Offset: 0x000A3A10
		public static bool TryCreateDialogsForVersionMismatchWarnings(Action confirmedAction)
		{
			string text = null;
			string title = null;
			if (!BackCompatibility.IsSaveCompatibleWith(ScribeMetaHeaderUtility.loadedGameVersion) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				title = "VersionMismatch".Translate();
				string value = ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() ? ("(" + "UnknownLower".TranslateSimple() + ")") : ScribeMetaHeaderUtility.loadedGameVersion;
				if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map)
				{
					text = "SaveGameIncompatibleWarningText".Translate(value, VersionControl.CurrentVersionString);
				}
				else if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.World)
				{
					text = "WorldFileVersionMismatch".Translate(value, VersionControl.CurrentVersionString);
				}
				else
				{
					text = "FileIncompatibleWarning".Translate(value, VersionControl.CurrentVersionString);
				}
			}
			string text2;
			string text3;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out text2, out text3))
			{
				Find.WindowStack.Add(new Dialog_ModMismatch(confirmedAction, ScribeMetaHeaderUtility.loadedModIdsList, ScribeMetaHeaderUtility.loadedModNamesList));
				return true;
			}
			if (text != null)
			{
				Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation(text, confirmedAction, false, title, WindowLayer.Dialog);
				dialog_MessageBox.buttonAText = "LoadAnyway".Translate();
				Find.WindowStack.Add(dialog_MessageBox);
				return true;
			}
			return false;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x000A5950 File Offset: 0x000A3B50
		public static bool LoadedModsMatchesActiveMods(out string loadedModsSummary, out string runningModsSummary)
		{
			loadedModsSummary = null;
			runningModsSummary = null;
			List<string> list = (from mod in LoadedModManager.RunningMods
			select mod.PackageId).ToList<string>();
			List<string> b = (from mod in LoadedModManager.RunningMods
			select mod.FolderName).ToList<string>();
			if (ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, list) || ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, b))
			{
				return true;
			}
			if (ScribeMetaHeaderUtility.loadedModNamesList == null)
			{
				loadedModsSummary = "None".Translate();
			}
			else
			{
				loadedModsSummary = ScribeMetaHeaderUtility.loadedModNamesList.ToCommaList(false, false);
			}
			runningModsSummary = (from id in list
			select ModLister.GetModWithIdentifier(id, false).Name).ToCommaList(false, false);
			return false;
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000A5A34 File Offset: 0x000A3C34
		private static bool ModListsMatch(List<string> a, List<string> b)
		{
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (a[i].ToLower() != b[i].ToLower())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000A5A8C File Offset: 0x000A3C8C
		public static string GameVersionOf(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new ArgumentException();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(file.FullName))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader) && xmlTextReader.ReadToDescendant("gameVersion"))
						{
							return VersionControl.VersionStringWithoutRev(xmlTextReader.ReadString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception getting game version of " + file.Name + ": " + ex.ToString());
			}
			return null;
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x000A5B48 File Offset: 0x000A3D48
		public static bool ReadToMetaElement(XmlTextReader textReader)
		{
			return ScribeMetaHeaderUtility.ReadToNextElement(textReader) && ScribeMetaHeaderUtility.ReadToNextElement(textReader) && !(textReader.Name != "meta");
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000A5B73 File Offset: 0x000A3D73
		private static bool ReadToNextElement(XmlTextReader textReader)
		{
			while (textReader.Read())
			{
				if (textReader.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040013B9 RID: 5049
		private static ScribeMetaHeaderUtility.ScribeHeaderMode lastMode;

		// Token: 0x040013BA RID: 5050
		public static string loadedGameVersion;

		// Token: 0x040013BB RID: 5051
		public static int loadedGameVersionMajor;

		// Token: 0x040013BC RID: 5052
		public static int loadedGameVersionMinor;

		// Token: 0x040013BD RID: 5053
		public static int loadedGameVersionBuild;

		// Token: 0x040013BE RID: 5054
		public static List<string> loadedModIdsList;

		// Token: 0x040013BF RID: 5055
		public static List<string> loadedModNamesList;

		// Token: 0x040013C0 RID: 5056
		public static List<int> loadedModSteamIdsList;

		// Token: 0x040013C1 RID: 5057
		public const string MetaNodeName = "meta";

		// Token: 0x040013C2 RID: 5058
		public const string GameVersionNodeName = "gameVersion";

		// Token: 0x040013C3 RID: 5059
		public const string ModIdsNodeName = "modIds";

		// Token: 0x040013C4 RID: 5060
		public const string ModNamesNodeName = "modNames";

		// Token: 0x040013C5 RID: 5061
		public const string ModSteamIdsNodeName = "modSteamIds";

		// Token: 0x02001E88 RID: 7816
		public enum ScribeHeaderMode
		{
			// Token: 0x0400782C RID: 30764
			None,
			// Token: 0x0400782D RID: 30765
			Map,
			// Token: 0x0400782E RID: 30766
			World,
			// Token: 0x0400782F RID: 30767
			Scenario,
			// Token: 0x04007830 RID: 30768
			Ideo,
			// Token: 0x04007831 RID: 30769
			Xenotype,
			// Token: 0x04007832 RID: 30770
			Xenogerm,
			// Token: 0x04007833 RID: 30771
			ModList,
			// Token: 0x04007834 RID: 30772
			CameraConfig
		}
	}
}

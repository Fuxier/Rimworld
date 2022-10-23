using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000566 RID: 1382
	public static class GenFilePaths
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x0010F0B8 File Offset: 0x0010D2B8
		public static string SaveDataFolderPath
		{
			get
			{
				if (GenFilePaths.saveDataPath == null)
				{
					string text;
					if (GenCommandLine.TryGetCommandLineArg("savedatafolder", out text))
					{
						text.TrimEnd(new char[]
						{
							'\\',
							'/'
						});
						if (text == "")
						{
							text = (Path.DirectorySeparatorChar.ToString() ?? "");
						}
						GenFilePaths.saveDataPath = text;
						Log.Message("Save data folder overridden to " + GenFilePaths.saveDataPath);
					}
					else
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
						if (UnityData.isEditor)
						{
							GenFilePaths.saveDataPath = Path.Combine(directoryInfo.Parent.ToString(), "SaveData");
						}
						else if (UnityData.platform == RuntimePlatform.OSXPlayer || UnityData.platform == RuntimePlatform.OSXEditor)
						{
							string path = Path.Combine(Directory.GetParent(UnityData.persistentDataPath).ToString(), "RimWorld");
							if (!Directory.Exists(path))
							{
								Directory.CreateDirectory(path);
							}
							GenFilePaths.saveDataPath = path;
						}
						else
						{
							GenFilePaths.saveDataPath = Application.persistentDataPath;
						}
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(GenFilePaths.saveDataPath);
					if (!directoryInfo2.Exists)
					{
						directoryInfo2.Create();
					}
				}
				return GenFilePaths.saveDataPath;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x0010F1D0 File Offset: 0x0010D3D0
		public static string ScenarioPreviewImagePath
		{
			get
			{
				if (!UnityData.isEditor)
				{
					return Path.Combine(GenFilePaths.ExecutableDir.FullName, "ScenarioPreview.jpg");
				}
				return Path.Combine(Path.Combine(Path.Combine(GenFilePaths.ExecutableDir.FullName, "PlatformSpecific"), "All"), "ScenarioPreview.jpg");
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x0010F221 File Offset: 0x0010D421
		private static DirectoryInfo ExecutableDir
		{
			get
			{
				return new DirectoryInfo(UnityData.dataPath).Parent;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x0010F232 File Offset: 0x0010D432
		public static string ModsFolderPath
		{
			get
			{
				if (GenFilePaths.modsFolderPath == null)
				{
					GenFilePaths.modsFolderPath = GenFilePaths.GetOrCreateModsFolder("Mods");
				}
				return GenFilePaths.modsFolderPath;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002A6B RID: 10859 RVA: 0x0010F24F File Offset: 0x0010D44F
		public static string OfficialModsFolderPath
		{
			get
			{
				if (GenFilePaths.officialModsFolderPath == null)
				{
					GenFilePaths.officialModsFolderPath = GenFilePaths.GetOrCreateModsFolder("Data");
				}
				return GenFilePaths.officialModsFolderPath;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x0010F26C File Offset: 0x0010D46C
		public static string ConfigFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Config");
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002A6D RID: 10861 RVA: 0x0010F278 File Offset: 0x0010D478
		public static string SavedGamesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Saves");
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002A6E RID: 10862 RVA: 0x0010F284 File Offset: 0x0010D484
		private static string ScenariosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Scenarios");
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06002A6F RID: 10863 RVA: 0x0010F290 File Offset: 0x0010D490
		private static string ModListsFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("ModLists");
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x0010F29C File Offset: 0x0010D49C
		private static string IdeosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Ideos");
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x0010F2A8 File Offset: 0x0010D4A8
		private static string XenotypesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Xenotypes");
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002A72 RID: 10866 RVA: 0x0010F2B4 File Offset: 0x0010D4B4
		private static string XenogermsFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Xenogerms");
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002A73 RID: 10867 RVA: 0x0010F2C0 File Offset: 0x0010D4C0
		private static string CameraConfigsFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("CameraConfigs");
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002A74 RID: 10868 RVA: 0x0010F2CC File Offset: 0x0010D4CC
		private static string ExternalHistoryFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("External");
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002A75 RID: 10869 RVA: 0x0010F2D8 File Offset: 0x0010D4D8
		public static string ScreenshotFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Screenshots");
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002A76 RID: 10870 RVA: 0x0010F2E4 File Offset: 0x0010D4E4
		public static string DevOutputFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("DevOutput");
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002A77 RID: 10871 RVA: 0x0010F2F0 File Offset: 0x0010D4F0
		public static string ModsConfigFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "ModsConfig.xml");
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002A78 RID: 10872 RVA: 0x0010F301 File Offset: 0x0010D501
		public static string ConceptKnowledgeFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Knowledge.xml");
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002A79 RID: 10873 RVA: 0x0010F312 File Offset: 0x0010D512
		public static string PrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Prefs.xml");
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x0010F323 File Offset: 0x0010D523
		public static string KeyPrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "KeyPrefs.xml");
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002A7B RID: 10875 RVA: 0x0010F334 File Offset: 0x0010D534
		public static string LastPlayedVersionFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "LastPlayedVersion.txt");
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002A7C RID: 10876 RVA: 0x0010F345 File Offset: 0x0010D545
		public static string DevModePermanentlyDisabledFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "DevModeDisabled");
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002A7D RID: 10877 RVA: 0x0010F356 File Offset: 0x0010D556
		public static string BackstoryOutputFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.DevOutputFolderPath, "Fresh_Backstories.xml");
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002A7E RID: 10878 RVA: 0x0010F367 File Offset: 0x0010D567
		public static string TempFolderPath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x0010F370 File Offset: 0x0010D570
		public static IEnumerable<FileInfo> AllSavedGameFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.SavedGamesFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rws"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x0010F3E4 File Offset: 0x0010D5E4
		public static IEnumerable<FileInfo> AllCustomScenarioFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ScenariosFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rsc"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002A81 RID: 10881 RVA: 0x0010F458 File Offset: 0x0010D658
		public static IEnumerable<FileInfo> AllModListFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ModListsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rml"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x0010F4CC File Offset: 0x0010D6CC
		public static IEnumerable<FileInfo> AllCustomIdeoFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.IdeosFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rid"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002A83 RID: 10883 RVA: 0x0010F540 File Offset: 0x0010D740
		public static IEnumerable<FileInfo> AllCustomXenotypeFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.XenotypesFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".xtp"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x0010F5B4 File Offset: 0x0010D7B4
		public static IEnumerable<FileInfo> AllCustomXenogermFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.XenogermsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".xgm"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002A85 RID: 10885 RVA: 0x0010F628 File Offset: 0x0010D828
		public static IEnumerable<FileInfo> AllCameraConfigFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.CameraConfigsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".ccg"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002A86 RID: 10886 RVA: 0x0010F69C File Offset: 0x0010D89C
		public static IEnumerable<FileInfo> AllExternalHistoryFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ExternalHistoryFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rwh"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x0010F710 File Offset: 0x0010D910
		private static string FolderUnderSaveData(string folderName)
		{
			string text = Path.Combine(GenFilePaths.SaveDataFolderPath, folderName);
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			return text;
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x0010F73D File Offset: 0x0010D93D
		public static string FilePathForSavedGame(string gameName)
		{
			return Path.Combine(GenFilePaths.SavedGamesFolderPath, gameName + ".rws");
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x0010F754 File Offset: 0x0010D954
		public static string AbsPathForScenario(string scenarioName)
		{
			return Path.Combine(GenFilePaths.ScenariosFolderPath, scenarioName + ".rsc");
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x0010F76B File Offset: 0x0010D96B
		public static string AbsPathForIdeo(string ideoName)
		{
			return Path.Combine(GenFilePaths.IdeosFolderPath, ideoName + ".rid");
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x0010F782 File Offset: 0x0010D982
		public static string AbsFilePathForXenotype(string xenotypeName)
		{
			return Path.Combine(GenFilePaths.XenotypesFolderPath, xenotypeName + ".xtp");
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x0010F799 File Offset: 0x0010D999
		public static string AbsFilePathForModList(string modListName)
		{
			return Path.Combine(GenFilePaths.ModListsFolderPath, modListName + ".rml");
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x0010F7B0 File Offset: 0x0010D9B0
		public static string AbsFilePathForXenogerm(string xenogermName)
		{
			return Path.Combine(GenFilePaths.XenogermsFolderPath, xenogermName + ".xgm");
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x0010F7C7 File Offset: 0x0010D9C7
		public static string AbsFilePathForCameraConfig(string configName)
		{
			return Path.Combine(GenFilePaths.CameraConfigsFolderPath, configName + ".ccg");
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x0010F7E0 File Offset: 0x0010D9E0
		public static string ContentPath<T>()
		{
			if (typeof(T) == typeof(AudioClip))
			{
				return "Sounds/";
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return "Textures/";
			}
			if (typeof(T) == typeof(string))
			{
				return "Strings/";
			}
			throw new ArgumentException();
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x0010F858 File Offset: 0x0010DA58
		private static string GetOrCreateModsFolder(string folderName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
			DirectoryInfo directoryInfo2;
			if (UnityData.isEditor)
			{
				directoryInfo2 = directoryInfo;
			}
			else
			{
				directoryInfo2 = directoryInfo.Parent;
			}
			string text = Path.Combine(directoryInfo2.ToString(), folderName);
			DirectoryInfo directoryInfo3 = new DirectoryInfo(text);
			if (!directoryInfo3.Exists)
			{
				directoryInfo3.Create();
			}
			return text;
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x0010F8A8 File Offset: 0x0010DAA8
		public static string SafeURIForUnityWWWFromPath(string rawPath)
		{
			string text = rawPath;
			for (int i = 0; i < GenFilePaths.FilePathRaw.Length; i++)
			{
				text = text.Replace(GenFilePaths.FilePathRaw[i], GenFilePaths.FilePathSafe[i]);
			}
			return "file:///" + text;
		}

		// Token: 0x04001BCF RID: 7119
		private static string saveDataPath = null;

		// Token: 0x04001BD0 RID: 7120
		private static string modsFolderPath = null;

		// Token: 0x04001BD1 RID: 7121
		private static string officialModsFolderPath = null;

		// Token: 0x04001BD2 RID: 7122
		public const string SoundsFolder = "Sounds/";

		// Token: 0x04001BD3 RID: 7123
		public const string SoundsFolderName = "Sounds";

		// Token: 0x04001BD4 RID: 7124
		public const string TexturesFolder = "Textures/";

		// Token: 0x04001BD5 RID: 7125
		public const string TexturesFolderName = "Textures";

		// Token: 0x04001BD6 RID: 7126
		public const string StringsFolder = "Strings/";

		// Token: 0x04001BD7 RID: 7127
		public const string DefsFolder = "Defs/";

		// Token: 0x04001BD8 RID: 7128
		public const string PatchesFolder = "Patches/";

		// Token: 0x04001BD9 RID: 7129
		public const string AssetBundlesFolderName = "AssetBundles";

		// Token: 0x04001BDA RID: 7130
		public const string AssetsFolderName = "Assets";

		// Token: 0x04001BDB RID: 7131
		public const string ResourcesFolderName = "Resources";

		// Token: 0x04001BDC RID: 7132
		public const string ModsFolderName = "Mods";

		// Token: 0x04001BDD RID: 7133
		public const string AssembliesFolder = "Assemblies/";

		// Token: 0x04001BDE RID: 7134
		public const string OfficialModsFolderName = "Data";

		// Token: 0x04001BDF RID: 7135
		public const string CoreFolderName = "Core";

		// Token: 0x04001BE0 RID: 7136
		public const string BackstoriesPath = "Backstories";

		// Token: 0x04001BE1 RID: 7137
		public const string SavedGameExtension = ".rws";

		// Token: 0x04001BE2 RID: 7138
		public const string ScenarioExtension = ".rsc";

		// Token: 0x04001BE3 RID: 7139
		public const string ModListExtension = ".rml";

		// Token: 0x04001BE4 RID: 7140
		public const string IdeoExtension = ".rid";

		// Token: 0x04001BE5 RID: 7141
		public const string XenotypeExtension = ".xtp";

		// Token: 0x04001BE6 RID: 7142
		public const string XenogermExtension = ".xgm";

		// Token: 0x04001BE7 RID: 7143
		public const string CameraConfigExtension = ".ccg";

		// Token: 0x04001BE8 RID: 7144
		public const string ExternalHistoryFileExtension = ".rwh";

		// Token: 0x04001BE9 RID: 7145
		private const string SaveDataFolderCommand = "savedatafolder";

		// Token: 0x04001BEA RID: 7146
		private static readonly string[] FilePathRaw = new string[]
		{
			"Ž",
			"ž",
			"Ÿ",
			"¡",
			"¢",
			"£",
			"¤",
			"¥",
			"¦",
			"§",
			"¨",
			"©",
			"ª",
			"À",
			"Á",
			"Â",
			"Ã",
			"Ä",
			"Å",
			"Æ",
			"Ç",
			"È",
			"É",
			"Ê",
			"Ë",
			"Ì",
			"Í",
			"Î",
			"Ï",
			"Ð",
			"Ñ",
			"Ò",
			"Ó",
			"Ô",
			"Õ",
			"Ö",
			"Ù",
			"Ú",
			"Û",
			"Ü",
			"Ý",
			"Þ",
			"ß",
			"à",
			"á",
			"â",
			"ã",
			"ä",
			"å",
			"æ",
			"ç",
			"è",
			"é",
			"ê",
			"ë",
			"ì",
			"í",
			"î",
			"ï",
			"ð",
			"ñ",
			"ò",
			"ó",
			"ô",
			"õ",
			"ö",
			"ù",
			"ú",
			"û",
			"ü",
			"ý",
			"þ",
			"ÿ"
		};

		// Token: 0x04001BEB RID: 7147
		private static readonly string[] FilePathSafe = new string[]
		{
			"%8E",
			"%9E",
			"%9F",
			"%A1",
			"%A2",
			"%A3",
			"%A4",
			"%A5",
			"%A6",
			"%A7",
			"%A8",
			"%A9",
			"%AA",
			"%C0",
			"%C1",
			"%C2",
			"%C3",
			"%C4",
			"%C5",
			"%C6",
			"%C7",
			"%C8",
			"%C9",
			"%CA",
			"%CB",
			"%CC",
			"%CD",
			"%CE",
			"%CF",
			"%D0",
			"%D1",
			"%D2",
			"%D3",
			"%D4",
			"%D5",
			"%D6",
			"%D9",
			"%DA",
			"%DB",
			"%DC",
			"%DD",
			"%DE",
			"%DF",
			"%E0",
			"%E1",
			"%E2",
			"%E3",
			"%E4",
			"%E5",
			"%E6",
			"%E7",
			"%E8",
			"%E9",
			"%EA",
			"%EB",
			"%EC",
			"%ED",
			"%EE",
			"%EF",
			"%F0",
			"%F1",
			"%F2",
			"%F3",
			"%F4",
			"%F5",
			"%F6",
			"%F9",
			"%FA",
			"%FB",
			"%FC",
			"%FD",
			"%FE",
			"%FF"
		};
	}
}

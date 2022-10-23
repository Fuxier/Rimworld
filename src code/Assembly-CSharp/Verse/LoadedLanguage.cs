using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200019A RID: 410
	public class LoadedLanguage
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0003D6EB File Offset: 0x0003B8EB
		public string DisplayName
		{
			get
			{
				return GenText.SplitCamelCase(this.folderName);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0003D6F8 File Offset: 0x0003B8F8
		public string FriendlyNameNative
		{
			get
			{
				if (this.info == null || this.info.friendlyNameNative.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameNative;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x0003D726 File Offset: 0x0003B926
		public string FriendlyNameEnglish
		{
			get
			{
				if (this.info == null || this.info.friendlyNameEnglish.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameEnglish;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0003D754 File Offset: 0x0003B954
		public IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> AllDirectories
		{
			get
			{
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					foreach (string text in mod.foldersToLoadDescendingOrder)
					{
						string path = Path.Combine(text, "Languages");
						VirtualDirectory directory = AbstractFilesystem.GetDirectory(Path.Combine(path, this.folderName));
						if (directory.Exists)
						{
							yield return new Tuple<VirtualDirectory, ModContentPack, string>(directory, mod, text);
						}
						else
						{
							directory = AbstractFilesystem.GetDirectory(Path.Combine(path, this.legacyFolderName));
							if (directory.Exists)
							{
								yield return new Tuple<VirtualDirectory, ModContentPack, string>(directory, mod, text);
							}
						}
					}
					List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
					mod = null;
				}
				IEnumerator<ModContentPack> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x0003D764 File Offset: 0x0003B964
		public LanguageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LanguageWorker)Activator.CreateInstance(this.info.languageWorkerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0003D78F File Offset: 0x0003B98F
		public LanguageWordInfo WordInfo
		{
			get
			{
				return this.wordInfo;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x0003D797 File Offset: 0x0003B997
		public string LegacyFolderName
		{
			get
			{
				return this.legacyFolderName;
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0003D7A0 File Offset: 0x0003B9A0
		public LoadedLanguage(string folderName)
		{
			this.folderName = folderName;
			this.legacyFolderName = (folderName.Contains("(") ? folderName.Substring(0, folderName.IndexOf("(") - 1) : folderName).Trim();
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0003D838 File Offset: 0x0003BA38
		public void LoadMetadata()
		{
			if (this.info != null && this.infoIsRealMetadata)
			{
				return;
			}
			this.infoIsRealMetadata = true;
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				foreach (string path in modContentPack.foldersToLoadDescendingOrder)
				{
					string text = Path.Combine(path, "Languages");
					if (new DirectoryInfo(text).Exists)
					{
						foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly, false))
						{
							if (virtualDirectory.Name == this.folderName || virtualDirectory.Name == this.legacyFolderName)
							{
								this.info = DirectXmlLoader.ItemFromXmlFile<LanguageInfo>(virtualDirectory, "LanguageInfo.xml", false);
								if (this.info.friendlyNameNative.NullOrEmpty() && virtualDirectory.FileExists("FriendlyName.txt"))
								{
									this.info.friendlyNameNative = virtualDirectory.ReadAllText("FriendlyName.txt");
								}
								if (this.info.friendlyNameNative.NullOrEmpty())
								{
									this.info.friendlyNameNative = this.folderName;
								}
								if (this.info.friendlyNameEnglish.NullOrEmpty())
								{
									this.info.friendlyNameEnglish = this.folderName;
								}
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0003DA20 File Offset: 0x0003BC20
		public void InitMetadata(VirtualDirectory directory)
		{
			this.infoIsRealMetadata = false;
			this.info = new LanguageInfo();
			string text = Regex.Replace(directory.Name, "(\\B[A-Z]+?(?=[A-Z][^A-Z])|\\B[A-Z]+?(?=[^A-Z]))", " $1");
			string friendlyNameEnglish = text;
			string friendlyNameNative = text;
			int num = text.FirstIndexOf((char c) => c == '(');
			int num2 = text.LastIndexOf(")");
			if (num >= 0 && num2 >= 0 && num2 > num)
			{
				friendlyNameEnglish = text.Substring(0, num - 1);
				friendlyNameNative = text.Substring(num + 1, num2 - num - 1);
			}
			this.info.friendlyNameEnglish = friendlyNameEnglish;
			this.info.friendlyNameNative = friendlyNameNative;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003DAD0 File Offset: 0x0003BCD0
		public void LoadData()
		{
			if (this.dataIsLoaded)
			{
				return;
			}
			this.dataIsLoaded = true;
			DeepProfiler.Start("Loading language data: " + this.folderName);
			try
			{
				this.tmpAlreadyLoadedFiles.Clear();
				foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in this.AllDirectories)
				{
					Tuple<VirtualDirectory, ModContentPack, string> localDirectory = tuple;
					if (!this.tmpAlreadyLoadedFiles.ContainsKey(localDirectory.Item2))
					{
						this.tmpAlreadyLoadedFiles[localDirectory.Item2] = new HashSet<string>();
					}
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						if (this.icon == BaseContent.BadTex)
						{
							VirtualFile file = localDirectory.Item1.GetFile("LangIcon.png");
							if (file.Exists)
							{
								this.icon = ModContentLoader<Texture2D>.LoadItem(file).contentItem;
							}
						}
					});
					VirtualDirectory directory = localDirectory.Item1.GetDirectory("CodeLinked");
					if (directory.Exists)
					{
						this.loadErrors.Add("Translations aren't called CodeLinked any more. Please rename to Keyed: " + directory);
					}
					else
					{
						directory = localDirectory.Item1.GetDirectory("Keyed");
					}
					if (directory.Exists)
					{
						List<VirtualFile> list = new List<VirtualFile>();
						foreach (VirtualFile virtualFile in directory.GetFiles("*.xml", SearchOption.AllDirectories))
						{
							if (this.TryRegisterFileIfNew(localDirectory, virtualFile.FullPath))
							{
								list.Add(virtualFile);
							}
						}
						List<string> list2 = (from x in list.AsParallel<VirtualFile>()
						select x.ReadAllText()).ToList<string>();
						for (int i = 0; i < list2.Count; i++)
						{
							this.LoadFromFile_Keyed(list[i], list2[i]);
						}
					}
					VirtualDirectory directory2 = localDirectory.Item1.GetDirectory("DefLinked");
					if (directory2.Exists)
					{
						this.loadErrors.Add("Translations aren't called DefLinked any more. Please rename to DefInjected: " + directory2);
					}
					else
					{
						directory2 = localDirectory.Item1.GetDirectory("DefInjected");
					}
					if (directory2.Exists)
					{
						foreach (VirtualDirectory virtualDirectory in directory2.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							string name = virtualDirectory.Name;
							Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name, null);
							if (typeInAnyAssembly == null && name.Length > 3)
							{
								typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name.Substring(0, name.Length - 1), null);
							}
							if (typeInAnyAssembly == null)
							{
								this.loadErrors.Add(string.Concat(new object[]
								{
									"Error loading language from ",
									tuple,
									": dir ",
									virtualDirectory.Name,
									" doesn't correspond to any def type. Skipping..."
								}));
							}
							else
							{
								List<VirtualFile> list3 = new List<VirtualFile>();
								foreach (VirtualFile virtualFile2 in virtualDirectory.GetFiles("*.xml", SearchOption.AllDirectories))
								{
									if (this.TryRegisterFileIfNew(localDirectory, virtualFile2.FullPath))
									{
										list3.Add(virtualFile2);
									}
								}
								List<string> list4 = (from x in list3.AsParallel<VirtualFile>()
								select x.ReadAllText()).ToList<string>();
								for (int j = 0; j < list4.Count; j++)
								{
									this.LoadFromFile_DefInject(list3[j], typeInAnyAssembly, list4[j]);
								}
							}
						}
					}
					this.EnsureAllDefTypesHaveDefInjectionPackage();
					VirtualDirectory directory3 = localDirectory.Item1.GetDirectory("Strings");
					if (directory3.Exists)
					{
						foreach (VirtualDirectory virtualDirectory2 in directory3.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							foreach (VirtualFile virtualFile3 in virtualDirectory2.GetFiles("*.txt", SearchOption.AllDirectories))
							{
								if (this.TryRegisterFileIfNew(localDirectory, virtualFile3.FullPath))
								{
									this.LoadFromFile_Strings(virtualFile3, directory3);
								}
							}
						}
					}
					this.wordInfo.LoadFrom(localDirectory, this);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception loading language data. Rethrowing. Exception: " + arg);
				throw;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0003E028 File Offset: 0x0003C228
		public bool TryRegisterFileIfNew(Tuple<VirtualDirectory, ModContentPack, string> dir, string filePath)
		{
			if (!filePath.StartsWith(dir.Item3))
			{
				Log.Error("Failed to get a relative path for a file: " + filePath + ", located in " + dir.Item3);
				return false;
			}
			string item = filePath.Substring(dir.Item3.Length);
			if (!this.tmpAlreadyLoadedFiles.ContainsKey(dir.Item2))
			{
				this.tmpAlreadyLoadedFiles[dir.Item2] = new HashSet<string>();
			}
			else if (this.tmpAlreadyLoadedFiles[dir.Item2].Contains(item))
			{
				return false;
			}
			this.tmpAlreadyLoadedFiles[dir.Item2].Add(item);
			return true;
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0003E0D4 File Offset: 0x0003C2D4
		private void LoadFromFile_Strings(VirtualFile file, VirtualDirectory stringsTopDir)
		{
			string text;
			try
			{
				text = file.ReadAllText();
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading from strings file ",
					file,
					": ",
					ex
				}));
				return;
			}
			string text2 = file.FullPath;
			if (stringsTopDir != null)
			{
				text2 = text2.Substring(stringsTopDir.FullPath.Length + 1);
			}
			text2 = text2.Substring(0, text2.Length - Path.GetExtension(text2).Length);
			text2 = text2.Replace('\\', '/');
			List<string> list = new List<string>();
			foreach (string item in GenText.LinesFromString(text))
			{
				list.Add(item);
			}
			List<string> list2;
			if (this.stringFiles.TryGetValue(text2, out list2))
			{
				using (List<string>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string item2 = enumerator2.Current;
						list2.Add(item2);
					}
					return;
				}
			}
			this.stringFiles.Add(text2, list);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0003E21C File Offset: 0x0003C41C
		private void LoadFromFile_Keyed(VirtualFile file, string preloadedFileContents)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			try
			{
				foreach (DirectXmlLoaderSimple.XmlKeyValuePair xmlKeyValuePair in DirectXmlLoaderSimple.ValuesFromXmlFile(preloadedFileContents))
				{
					if (dictionary.ContainsKey(xmlKeyValuePair.key))
					{
						this.loadErrors.Add("Duplicate keyed translation key: " + xmlKeyValuePair.key + " in language " + this.folderName);
					}
					else
					{
						dictionary.Add(xmlKeyValuePair.key, xmlKeyValuePair.value);
						dictionary2.Add(xmlKeyValuePair.key, xmlKeyValuePair.lineNumber);
					}
				}
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading from translation file ",
					file,
					": ",
					ex
				}));
				dictionary.Clear();
				dictionary2.Clear();
				this.anyKeyedReplacementsXmlParseError = true;
				this.lastKeyedReplacementsXmlParseErrorInFile = file.Name;
			}
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				string text = keyValuePair.Value;
				LoadedLanguage.KeyedReplacement keyedReplacement = new LoadedLanguage.KeyedReplacement();
				if (text == "TODO")
				{
					keyedReplacement.isPlaceholder = true;
					text = "";
				}
				keyedReplacement.key = keyValuePair.Key;
				keyedReplacement.value = text;
				keyedReplacement.fileSource = file.Name;
				keyedReplacement.fileSourceLine = dictionary2[keyValuePair.Key];
				keyedReplacement.fileSourceFullPath = file.FullPath;
				this.keyedReplacements.SetOrAdd(keyValuePair.Key, keyedReplacement);
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0003E3EC File Offset: 0x0003C5EC
		public void LoadFromFile_DefInject(VirtualFile file, Type defType, string preloadedFileContents)
		{
			DefInjectionPackage defInjectionPackage = (from di in this.defInjections
			where di.defType == defType
			select di).FirstOrDefault<DefInjectionPackage>();
			if (defInjectionPackage == null)
			{
				defInjectionPackage = new DefInjectionPackage(defType);
				this.defInjections.Add(defInjectionPackage);
			}
			bool flag;
			defInjectionPackage.AddDataFromFile(file, out flag, preloadedFileContents);
			if (flag)
			{
				this.anyDefInjectionsXmlParseError = true;
				this.lastDefInjectionsXmlParseErrorInFile = file.Name;
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0003E460 File Offset: 0x0003C660
		private void EnsureAllDefTypesHaveDefInjectionPackage()
		{
			using (IEnumerator<Type> enumerator = GenDefDatabase.AllDefTypesWithDatabases().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type defType = enumerator.Current;
					if (!this.defInjections.Any((DefInjectionPackage x) => x.defType == defType))
					{
						this.defInjections.Add(new DefInjectionPackage(defType));
					}
				}
			}
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0003E4E0 File Offset: 0x0003C6E0
		public bool HaveTextForKey(string key, bool allowPlaceholders = false)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			return key != null && this.keyedReplacements.TryGetValue(key, out keyedReplacement) && (allowPlaceholders || !keyedReplacement.isPlaceholder);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0003E520 File Offset: 0x0003C720
		public bool TryGetTextFromKey(string key, out TaggedString translated)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (key == null)
			{
				translated = key;
				return false;
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement) || keyedReplacement.isPlaceholder)
			{
				translated = key;
				return false;
			}
			translated = keyedReplacement.value;
			return true;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0003E584 File Offset: 0x0003C784
		public bool TryGetStringsFromFile(string fileName, out List<string> stringsList)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (!this.stringFiles.TryGetValue(fileName, out stringsList))
			{
				stringsList = null;
				return false;
			}
			return true;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0003E5AC File Offset: 0x0003C7AC
		public string GetKeySourceFileAndLine(string key)
		{
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement))
			{
				return "unknown";
			}
			return keyedReplacement.fileSource + ":" + keyedReplacement.fileSourceLine;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0003E5EA File Offset: 0x0003C7EA
		public Gender ResolveGender(string str, string fallback = null, Gender defaultGender = Gender.Male)
		{
			return this.wordInfo.ResolveGender(str, fallback, defaultGender);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0003E5FC File Offset: 0x0003C7FC
		public void InjectIntoData_BeforeImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(false);
				}
				catch (Exception arg)
				{
					Log.Error("Critical error while injecting translations into defs: " + arg);
				}
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0003E680 File Offset: 0x0003C880
		public void InjectIntoData_AfterImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			int num = this.loadErrors.Count;
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(true);
					num += defInjectionPackage.loadErrors.Count;
				}
				catch (Exception arg)
				{
					Log.Error("Critical error while injecting translations into defs: " + arg);
				}
			}
			if (num != 0)
			{
				this.anyError = true;
				Log.Warning(string.Concat(new object[]
				{
					"Translation data for language ",
					LanguageDatabase.activeLanguage.FriendlyNameEnglish,
					" has ",
					num,
					" errors. Generate translation report for more info."
				}));
			}
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0003E764 File Offset: 0x0003C964
		public override string ToString()
		{
			return this.info.friendlyNameEnglish;
		}

		// Token: 0x04000A98 RID: 2712
		public string folderName;

		// Token: 0x04000A99 RID: 2713
		public LanguageInfo info;

		// Token: 0x04000A9A RID: 2714
		private LanguageWorker workerInt;

		// Token: 0x04000A9B RID: 2715
		private LanguageWordInfo wordInfo = new LanguageWordInfo();

		// Token: 0x04000A9C RID: 2716
		private bool dataIsLoaded;

		// Token: 0x04000A9D RID: 2717
		public List<string> loadErrors = new List<string>();

		// Token: 0x04000A9E RID: 2718
		public bool anyKeyedReplacementsXmlParseError;

		// Token: 0x04000A9F RID: 2719
		public string lastKeyedReplacementsXmlParseErrorInFile;

		// Token: 0x04000AA0 RID: 2720
		public bool anyDefInjectionsXmlParseError;

		// Token: 0x04000AA1 RID: 2721
		public string lastDefInjectionsXmlParseErrorInFile;

		// Token: 0x04000AA2 RID: 2722
		public bool anyError;

		// Token: 0x04000AA3 RID: 2723
		private string legacyFolderName;

		// Token: 0x04000AA4 RID: 2724
		private Dictionary<ModContentPack, HashSet<string>> tmpAlreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();

		// Token: 0x04000AA5 RID: 2725
		public Texture2D icon = BaseContent.BadTex;

		// Token: 0x04000AA6 RID: 2726
		public Dictionary<string, LoadedLanguage.KeyedReplacement> keyedReplacements = new Dictionary<string, LoadedLanguage.KeyedReplacement>();

		// Token: 0x04000AA7 RID: 2727
		public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

		// Token: 0x04000AA8 RID: 2728
		public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();

		// Token: 0x04000AA9 RID: 2729
		public const string OldKeyedTranslationsFolderName = "CodeLinked";

		// Token: 0x04000AAA RID: 2730
		public const string KeyedTranslationsFolderName = "Keyed";

		// Token: 0x04000AAB RID: 2731
		public const string OldDefInjectionsFolderName = "DefLinked";

		// Token: 0x04000AAC RID: 2732
		public const string DefInjectionsFolderName = "DefInjected";

		// Token: 0x04000AAD RID: 2733
		public const string LanguagesFolderName = "Languages";

		// Token: 0x04000AAE RID: 2734
		public const string PlaceholderText = "TODO";

		// Token: 0x04000AAF RID: 2735
		private bool infoIsRealMetadata;

		// Token: 0x02001D2D RID: 7469
		public class KeyedReplacement
		{
			// Token: 0x04007349 RID: 29513
			public string key;

			// Token: 0x0400734A RID: 29514
			public string value;

			// Token: 0x0400734B RID: 29515
			public string fileSource;

			// Token: 0x0400734C RID: 29516
			public int fileSourceLine;

			// Token: 0x0400734D RID: 29517
			public string fileSourceFullPath;

			// Token: 0x0400734E RID: 29518
			public bool isPlaceholder;
		}
	}
}

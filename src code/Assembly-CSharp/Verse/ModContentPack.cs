using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using KTrie;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200027C RID: 636
	public class ModContentPack
	{
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x0006A01A File Offset: 0x0006821A
		public string RootDir
		{
			get
			{
				return this.rootDirInt.FullName;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600121D RID: 4637 RVA: 0x0006A027 File Offset: 0x00068227
		public string PackageId
		{
			get
			{
				return this.packageIdInt;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600121E RID: 4638 RVA: 0x0006A02F File Offset: 0x0006822F
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.packageIdPlayerFacingInt;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x0006A037 File Offset: 0x00068237
		public int SteamAppId
		{
			get
			{
				ModMetaData modMetaData = this.ModMetaData;
				if (modMetaData == null)
				{
					return 0;
				}
				return modMetaData.SteamAppId;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001220 RID: 4640 RVA: 0x0006A04A File Offset: 0x0006824A
		public ModMetaData ModMetaData
		{
			get
			{
				return ModLister.GetModWithIdentifier(this.PackageId, false);
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x0006A058 File Offset: 0x00068258
		public string FolderName
		{
			get
			{
				return this.rootDirInt.Name;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001222 RID: 4642 RVA: 0x0006A065 File Offset: 0x00068265
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x0006A06D File Offset: 0x0006826D
		public int OverwritePriority
		{
			get
			{
				if (!this.IsCoreMod)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x0006A07A File Offset: 0x0006827A
		public bool IsCoreMod
		{
			get
			{
				return this.PackageId == "ludeon.rimworld";
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x0006A08C File Offset: 0x0006828C
		public bool IsOfficialMod
		{
			get
			{
				return this.official;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x0006A094 File Offset: 0x00068294
		public IEnumerable<Def> AllDefs
		{
			get
			{
				return this.defs;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x0006A09C File Offset: 0x0006829C
		public IEnumerable<PatchOperation> Patches
		{
			get
			{
				if (this.patches == null)
				{
					this.LoadPatches();
				}
				return this.patches;
			}
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0006A0B4 File Offset: 0x000682B4
		public List<string> AllAssetNamesInBundle(int index)
		{
			if (this.allAssetNamesInBundleCached == null)
			{
				this.allAssetNamesInBundleCached = new List<List<string>>();
				foreach (AssetBundle assetBundle in this.assetBundles.loadedAssetBundles)
				{
					this.allAssetNamesInBundleCached.Add(new List<string>(assetBundle.GetAllAssetNames()));
				}
			}
			return this.allAssetNamesInBundleCached[index];
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0006A13C File Offset: 0x0006833C
		public StringTrieSet AllAssetNamesInBundleTrie(int index)
		{
			if (this.allAssetNamesInBundleCachedTrie == null)
			{
				this.allAssetNamesInBundleCachedTrie = new List<StringTrieSet>();
				foreach (AssetBundle assetBundle in this.assetBundles.loadedAssetBundles)
				{
					StringTrieSet stringTrieSet = new StringTrieSet();
					foreach (string item in assetBundle.GetAllAssetNames())
					{
						stringTrieSet.Add(item);
					}
					this.allAssetNamesInBundleCachedTrie.Add(stringTrieSet);
				}
			}
			return this.allAssetNamesInBundleCachedTrie[index];
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0006A1E0 File Offset: 0x000683E0
		public ModContentPack(DirectoryInfo directory, string packageId, string packageIdPlayerFacing, int loadOrder, string name, bool official)
		{
			this.rootDirInt = directory;
			this.loadOrder = loadOrder;
			this.nameInt = name;
			this.official = official;
			this.packageIdInt = packageId.ToLower();
			this.packageIdPlayerFacingInt = packageIdPlayerFacing;
			this.audioClips = new ModContentHolder<AudioClip>(this);
			this.textures = new ModContentHolder<Texture2D>(this);
			this.strings = new ModContentHolder<string>(this);
			this.assetBundles = new ModAssetBundlesHandler(this);
			this.assemblies = new ModAssemblyHandler(this);
			this.InitLoadFolders();
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0006A272 File Offset: 0x00068472
		public void ClearDestroy()
		{
			this.audioClips.ClearDestroy();
			this.textures.ClearDestroy();
			this.assetBundles.ClearDestroy();
			this.allAssetNamesInBundleCached = null;
			this.allAssetNamesInBundleCachedTrie = null;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0006A2A4 File Offset: 0x000684A4
		public ModContentHolder<T> GetContentHolder<T>() where T : class
		{
			if (typeof(T) == typeof(Texture2D))
			{
				return (ModContentHolder<T>)this.textures;
			}
			if (typeof(T) == typeof(AudioClip))
			{
				return (ModContentHolder<T>)this.audioClips;
			}
			if (typeof(T) == typeof(string))
			{
				return (ModContentHolder<T>)this.strings;
			}
			Log.Error("Mod lacks manager for asset type " + this.strings);
			return null;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0006A33C File Offset: 0x0006853C
		private void ReloadContentInt()
		{
			DeepProfiler.Start("Reload audio clips");
			try
			{
				this.audioClips.ReloadAll();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload textures");
			try
			{
				this.textures.ReloadAll();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload strings");
			try
			{
				this.strings.ReloadAll();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload asset bundles");
			try
			{
				this.assetBundles.ReloadAll();
				this.allAssetNamesInBundleCached = null;
				this.allAssetNamesInBundleCachedTrie = null;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0006A400 File Offset: 0x00068600
		public void ReloadContent()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(this.ReloadContentInt));
			this.assemblies.ReloadAll();
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0006A41E File Offset: 0x0006861E
		public IEnumerable<LoadableXmlAsset> LoadDefs()
		{
			if (this.defs.Count != 0)
			{
				Log.ErrorOnce("LoadDefs called with already existing def packages", 39029405);
			}
			DeepProfiler.Start("Load defs via DirectXmlLoader");
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Defs/", null).ToList<LoadableXmlAsset>();
			DeepProfiler.End();
			DeepProfiler.Start("Parse loaded defs");
			foreach (LoadableXmlAsset loadableXmlAsset in list)
			{
				yield return loadableXmlAsset;
			}
			List<LoadableXmlAsset>.Enumerator enumerator = default(List<LoadableXmlAsset>.Enumerator);
			DeepProfiler.End();
			yield break;
			yield break;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0006A430 File Offset: 0x00068630
		private void InitLoadFolders()
		{
			this.foldersToLoadDescendingOrder = new List<string>();
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.PackageId, false);
			if (((modWithIdentifier != null) ? modWithIdentifier.loadFolders : null) != null && modWithIdentifier.loadFolders.DefinedVersions().Count > 0)
			{
				List<LoadFolder> list = modWithIdentifier.LoadFoldersForVersion(VersionControl.CurrentVersionString);
				if (list != null && list.Count > 0)
				{
					this.<InitLoadFolders>g__AddFolders|56_0(list);
					return;
				}
				string text = (from x in modWithIdentifier.loadFolders.DefinedVersions().Where(delegate(string x)
				{
					if (x == "default" || x.NullOrEmpty() || !x.Contains("."))
					{
						return false;
					}
					bool result;
					try
					{
						result = (VersionControl.VersionFromString(x) <= VersionControl.CurrentVersion);
					}
					catch
					{
						result = false;
					}
					return result;
				})
				orderby x descending
				select x).FirstOrDefault<string>();
				if (text != null)
				{
					List<LoadFolder> list2 = modWithIdentifier.LoadFoldersForVersion(text);
					if (list2 != null)
					{
						this.<InitLoadFolders>g__AddFolders|56_0(list2);
						return;
					}
				}
				List<LoadFolder> list3 = modWithIdentifier.LoadFoldersForVersion("default");
				if (list3 != null)
				{
					this.<InitLoadFolders>g__AddFolders|56_0(list3);
					return;
				}
			}
			if (this.foldersToLoadDescendingOrder.Count == 0)
			{
				string text2 = Path.Combine(this.RootDir, VersionControl.CurrentVersionStringWithoutBuild);
				if (Directory.Exists(text2))
				{
					this.foldersToLoadDescendingOrder.Add(text2);
				}
				else
				{
					Version version = new Version(0, 0);
					List<Version> list4 = new List<Version>();
					DirectoryInfo[] directories = this.rootDirInt.GetDirectories();
					for (int i = 0; i < directories.Length; i++)
					{
						Version item;
						if (VersionControl.TryParseVersionString(directories[i].Name, out item))
						{
							list4.Add(item);
						}
					}
					list4.Sort();
					foreach (Version version2 in list4)
					{
						if ((version2 > version || version > VersionControl.CurrentVersion) && (version2 <= VersionControl.CurrentVersion || version.Major == 0))
						{
							version = version2;
						}
					}
					if (version.Major > 0)
					{
						this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, version.ToString()));
					}
				}
				string text3 = Path.Combine(this.RootDir, ModContentPack.CommonFolderName);
				if (Directory.Exists(text3))
				{
					this.foldersToLoadDescendingOrder.Add(text3);
				}
				this.foldersToLoadDescendingOrder.Add(this.RootDir);
			}
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0006A688 File Offset: 0x00068888
		private void LoadPatches()
		{
			DeepProfiler.Start("Loading all patches");
			this.patches = new List<PatchOperation>();
			this.loadedAnyPatches = false;
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Patches/", null).ToList<LoadableXmlAsset>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlElement documentElement = list[i].xmlDoc.DocumentElement;
				if (documentElement.Name != "Patch")
				{
					Log.Error(string.Format("Unexpected document element in patch XML; got {0}, expected 'Patch'", documentElement.Name));
				}
				else
				{
					foreach (object obj in documentElement.ChildNodes)
					{
						XmlNode xmlNode = (XmlNode)obj;
						if (xmlNode.NodeType == XmlNodeType.Element)
						{
							if (xmlNode.Name != "Operation")
							{
								Log.Error(string.Format("Unexpected element in patch XML; got {0}, expected 'Operation'", xmlNode.Name));
							}
							else
							{
								PatchOperation patchOperation = DirectXmlToObject.ObjectFromXml<PatchOperation>(xmlNode, false);
								patchOperation.sourceFile = list[i].FullFilePath;
								this.patches.Add(patchOperation);
								this.loadedAnyPatches = true;
							}
						}
					}
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0006A7CC File Offset: 0x000689CC
		public static Dictionary<string, FileInfo> GetAllFilesForMod(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
					{
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string key = fileInfo.FullName.Substring(text.Length + 1);
							if (!dictionary.ContainsKey(key))
							{
								dictionary.Add(key, fileInfo);
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0006A888 File Offset: 0x00068A88
		public static List<Tuple<string, FileInfo>> GetAllFilesForModPreserveOrder(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			List<Tuple<string, FileInfo>> list2 = new List<Tuple<string, FileInfo>>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
				if (directoryInfo.Exists)
				{
					FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
					Array.Sort<FileInfo>(files, (FileInfo a, FileInfo b) => a.Name.CompareTo(b.Name));
					foreach (FileInfo fileInfo in files)
					{
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string item = fileInfo.FullName.Substring(text.Length + 1);
							list2.Add(new Tuple<string, FileInfo>(item, fileInfo));
						}
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			for (int k = list2.Count - 1; k >= 0; k--)
			{
				Tuple<string, FileInfo> tuple = list2[k];
				if (!hashSet.Contains(tuple.Item1))
				{
					hashSet.Add(tuple.Item1);
				}
				else
				{
					list2.RemoveAt(k);
				}
			}
			return list2;
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0006A9BA File Offset: 0x00068BBA
		public bool AnyContentLoaded()
		{
			return this.AnyNonTranslationContentLoaded() || this.AnyTranslationsLoaded();
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0006A9D4 File Offset: 0x00068BD4
		public bool AnyNonTranslationContentLoaded()
		{
			return (this.textures.contentList != null && this.textures.contentList.Count != 0) || (this.audioClips.contentList != null && this.audioClips.contentList.Count != 0) || (this.strings.contentList != null && this.strings.contentList.Count != 0) || !this.assemblies.loadedAssemblies.NullOrEmpty<Assembly>() || !this.assetBundles.loadedAssetBundles.NullOrEmpty<AssetBundle>() || this.loadedAnyPatches || this.AllDefs.Any<Def>();
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0006AA88 File Offset: 0x00068C88
		public bool AnyTranslationsLoaded()
		{
			foreach (string path in this.foldersToLoadDescendingOrder)
			{
				string path2 = Path.Combine(path, "Languages");
				if (Directory.Exists(path2) && Directory.EnumerateFiles(path2, "*", SearchOption.AllDirectories).Any<string>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0006AB00 File Offset: 0x00068D00
		public void ClearPatchesCache()
		{
			this.patches = null;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0006AB09 File Offset: 0x00068D09
		public void AddDef(Def def, string source = "Unknown")
		{
			def.modContentPack = this;
			def.fileName = source;
			this.defs.Add(def);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0006AB25 File Offset: 0x00068D25
		public override string ToString()
		{
			return this.PackageIdPlayerFacing;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0006AB64 File Offset: 0x00068D64
		[CompilerGenerated]
		private void <InitLoadFolders>g__AddFolders|56_0(List<LoadFolder> folders)
		{
			for (int i = folders.Count - 1; i >= 0; i--)
			{
				if (folders[i].ShouldLoad)
				{
					this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, folders[i].folderName));
				}
			}
		}

		// Token: 0x04000F40 RID: 3904
		private DirectoryInfo rootDirInt;

		// Token: 0x04000F41 RID: 3905
		public int loadOrder;

		// Token: 0x04000F42 RID: 3906
		private string nameInt;

		// Token: 0x04000F43 RID: 3907
		private string packageIdInt;

		// Token: 0x04000F44 RID: 3908
		private string packageIdPlayerFacingInt;

		// Token: 0x04000F45 RID: 3909
		private bool official;

		// Token: 0x04000F46 RID: 3910
		private ModContentHolder<AudioClip> audioClips;

		// Token: 0x04000F47 RID: 3911
		private ModContentHolder<Texture2D> textures;

		// Token: 0x04000F48 RID: 3912
		private ModContentHolder<string> strings;

		// Token: 0x04000F49 RID: 3913
		public ModAssetBundlesHandler assetBundles;

		// Token: 0x04000F4A RID: 3914
		public ModAssemblyHandler assemblies;

		// Token: 0x04000F4B RID: 3915
		private List<PatchOperation> patches;

		// Token: 0x04000F4C RID: 3916
		private List<Def> defs = new List<Def>();

		// Token: 0x04000F4D RID: 3917
		private List<List<string>> allAssetNamesInBundleCached;

		// Token: 0x04000F4E RID: 3918
		private List<StringTrieSet> allAssetNamesInBundleCachedTrie;

		// Token: 0x04000F4F RID: 3919
		public List<string> foldersToLoadDescendingOrder;

		// Token: 0x04000F50 RID: 3920
		private bool loadedAnyPatches;

		// Token: 0x04000F51 RID: 3921
		public const string LudeonPackageIdAuthor = "ludeon";

		// Token: 0x04000F52 RID: 3922
		public const string CoreModPackageId = "ludeon.rimworld";

		// Token: 0x04000F53 RID: 3923
		public const string RoyaltyModPackageId = "ludeon.rimworld.royalty";

		// Token: 0x04000F54 RID: 3924
		public const string IdeologyModPackageId = "ludeon.rimworld.ideology";

		// Token: 0x04000F55 RID: 3925
		public const string BiotechModPackageId = "ludeon.rimworld.biotech";

		// Token: 0x04000F56 RID: 3926
		public static readonly string[] ProductPackageIDs = new string[]
		{
			"ludeon.rimworld",
			"ludeon.rimworld.royalty",
			"ludeon.rimworld.ideology",
			"ludeon.rimworld.biotech"
		};

		// Token: 0x04000F57 RID: 3927
		public static readonly string CommonFolderName = "Common";
	}
}

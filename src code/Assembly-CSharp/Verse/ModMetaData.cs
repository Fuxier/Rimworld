using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200027E RID: 638
	public class ModMetaData : WorkshopUploadable
	{
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x0006B590 File Offset: 0x00069790
		public Texture2D PreviewImage
		{
			get
			{
				if (this.previewImageWasLoaded)
				{
					return this.previewImage;
				}
				if (File.Exists(this.PreviewImagePath))
				{
					this.previewImage = new Texture2D(0, 0);
					this.previewImage.LoadImage(File.ReadAllBytes(this.PreviewImagePath));
				}
				this.previewImageWasLoaded = true;
				return this.previewImage;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x0006B5EA File Offset: 0x000697EA
		public string FolderName
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x0006B5F7 File Offset: 0x000697F7
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x0600125B RID: 4699 RVA: 0x0006B5FF File Offset: 0x000697FF
		public bool IsCoreMod
		{
			get
			{
				return this.SamePackageId("ludeon.rimworld", false);
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x0006B60D File Offset: 0x0006980D
		// (set) Token: 0x0600125D RID: 4701 RVA: 0x0006B615 File Offset: 0x00069815
		public bool Active
		{
			get
			{
				return ModsConfig.IsActive(this);
			}
			set
			{
				ModsConfig.SetActive(this, value);
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x0006B61E File Offset: 0x0006981E
		public bool VersionCompatible
		{
			get
			{
				if (this.IsCoreMod)
				{
					return true;
				}
				return this.meta.SupportedVersions.Any((System.Version v) => VersionControl.IsCompatible(v));
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x0006B659 File Offset: 0x00069859
		public bool MadeForNewerVersion
		{
			get
			{
				if (this.VersionCompatible)
				{
					return false;
				}
				return this.meta.SupportedVersions.Any((System.Version v) => v.Major > VersionControl.CurrentMajor || (v.Major == VersionControl.CurrentMajor && v.Minor > VersionControl.CurrentMinor));
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x0006B694 File Offset: 0x00069894
		public ExpansionDef Expansion
		{
			get
			{
				return ModLister.GetExpansionWithIdentifier(this.PackageId);
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x0006B6A4 File Offset: 0x000698A4
		public string Name
		{
			get
			{
				ExpansionDef expansion = this.Expansion;
				if (expansion == null)
				{
					return this.meta.name;
				}
				return expansion.label;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x0006B6CD File Offset: 0x000698CD
		public string ShortName
		{
			get
			{
				if (!this.meta.shortName.NullOrEmpty())
				{
					return this.meta.shortName;
				}
				return this.Name;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x0006B6F4 File Offset: 0x000698F4
		public string Description
		{
			get
			{
				if (this.descriptionCached == null)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(this.PackageId);
					this.descriptionCached = ((expansionWithIdentifier != null) ? expansionWithIdentifier.description : this.meta.description);
				}
				return this.descriptionCached;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0006B737 File Offset: 0x00069937
		public string AuthorsString
		{
			get
			{
				return this.Authors.ToCommaList(true, false);
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x0006B746 File Offset: 0x00069946
		public IEnumerable<string> Authors
		{
			get
			{
				if (!this.meta.authors.NullOrEmpty<string>())
				{
					foreach (string text in this.meta.authors)
					{
						yield return text;
					}
					List<string>.Enumerator enumerator = default(List<string>.Enumerator);
					yield break;
				}
				if (!string.IsNullOrWhiteSpace(this.meta.author))
				{
					foreach (string text2 in this.meta.author.Split(ModMetaData.AndToken, StringSplitOptions.RemoveEmptyEntries).SelectMany((string x) => x.Split(new char[]
					{
						','
					})))
					{
						yield return text2;
					}
					IEnumerator<string> enumerator2 = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x0006B756 File Offset: 0x00069956
		public virtual string ModVersion
		{
			get
			{
				return this.meta.modVersion;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x0006B763 File Offset: 0x00069963
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0006B770 File Offset: 0x00069970
		public int SteamAppId
		{
			get
			{
				return this.meta.steamAppId;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x0006B77D File Offset: 0x0006997D
		public List<System.Version> SupportedVersionsReadOnly
		{
			get
			{
				return this.meta.SupportedVersions;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x0006B78A File Offset: 0x0006998A
		IEnumerable<System.Version> WorkshopUploadable.SupportedVersions
		{
			get
			{
				return this.SupportedVersionsReadOnly;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x0600126B RID: 4715 RVA: 0x0006B794 File Offset: 0x00069994
		public string PreviewImagePath
		{
			get
			{
				return string.Concat(new string[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar.ToString(),
					"About",
					Path.DirectorySeparatorChar.ToString(),
					"Preview.png"
				});
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0006B7EA File Offset: 0x000699EA
		public bool Official
		{
			get
			{
				return this.IsCoreMod || this.Source == ContentSource.OfficialModsFolder;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x0006B7FF File Offset: 0x000699FF
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x0006B807 File Offset: 0x00069A07
		public string PackageId
		{
			get
			{
				if (!this.appendPackageIdSteamPostfix)
				{
					return this.packageIdLowerCase;
				}
				return this.packageIdLowerCase + ModMetaData.SteamModPostfix;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x0006B828 File Offset: 0x00069A28
		public string PackageIdNonUnique
		{
			get
			{
				return this.packageIdLowerCase;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x0006B830 File Offset: 0x00069A30
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.meta.packageId;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x0006B83D File Offset: 0x00069A3D
		public List<ModDependency> Dependencies
		{
			get
			{
				return this.meta.modDependencies;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x0006B84A File Offset: 0x00069A4A
		public List<string> LoadBefore
		{
			get
			{
				return this.meta.loadBefore;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x0006B857 File Offset: 0x00069A57
		public List<string> LoadAfter
		{
			get
			{
				return this.meta.loadAfter;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x0006B864 File Offset: 0x00069A64
		public List<string> ForceLoadBefore
		{
			get
			{
				return this.meta.forceLoadBefore;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x0006B871 File Offset: 0x00069A71
		public List<string> ForceLoadAfter
		{
			get
			{
				return this.meta.forceLoadAfter;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x0006B87E File Offset: 0x00069A7E
		public List<string> IncompatibleWith
		{
			get
			{
				return this.meta.incompatibleWith;
			}
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0006B88C File Offset: 0x00069A8C
		public List<string> UnsatisfiedDependencies()
		{
			this.unsatisfiedDepsList.Clear();
			for (int i = 0; i < this.Dependencies.Count; i++)
			{
				ModDependency modDependency = this.Dependencies[i];
				if (!modDependency.IsSatisfied)
				{
					this.unsatisfiedDepsList.Add(modDependency.displayName);
				}
			}
			return this.unsatisfiedDepsList;
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x0006B8E6 File Offset: 0x00069AE6
		// (set) Token: 0x06001279 RID: 4729 RVA: 0x0006B8EE File Offset: 0x00069AEE
		public bool HadIncorrectlyFormattedVersionInMetadata { get; private set; }

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x0006B8F7 File Offset: 0x00069AF7
		// (set) Token: 0x0600127B RID: 4731 RVA: 0x0006B8FF File Offset: 0x00069AFF
		public bool HadIncorrectlyFormattedPackageId { get; private set; }

		// Token: 0x0600127C RID: 4732 RVA: 0x0006B908 File Offset: 0x00069B08
		public ModMetaData(string localAbsPath, bool official = false)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = (official ? ContentSource.OfficialModsFolder : ContentSource.ModsFolder);
			this.Init();
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0006B964 File Offset: 0x00069B64
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0006B9B8 File Offset: 0x00069BB8
		public void UnsetPreviewImage()
		{
			this.previewImage = null;
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0006B9C1 File Offset: 0x00069BC1
		public bool SamePackageId(string otherPackageId, bool ignorePostfix = false)
		{
			if (this.PackageId == null)
			{
				return false;
			}
			if (ignorePostfix)
			{
				return this.packageIdLowerCase.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
			}
			return this.PackageId.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x0006B9EB File Offset: 0x00069BEB
		public List<LoadFolder> LoadFoldersForVersion(string version)
		{
			ModLoadFolders modLoadFolders = this.loadFolders;
			if (modLoadFolders == null)
			{
				return null;
			}
			return modLoadFolders.FoldersForVersion(version);
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0006BA00 File Offset: 0x00069C00
		private void Init()
		{
			DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
			this.meta = DirectXmlLoader.ItemFromXmlFile<ModMetaData.ModMetaDataInternal>(GenFile.ResolveCaseInsensitiveFilePath(this.RootDir.FullName + Path.DirectorySeparatorChar.ToString() + "About", "About.xml"), true);
			this.loadFolders = DirectXmlLoader.ItemFromXmlFile<ModLoadFolders>(GenFile.ResolveCaseInsensitiveFilePath(this.RootDir.FullName, "LoadFolders.xml"), true);
			bool shouldLogIssues = ModLister.ShouldLogIssues;
			this.HadIncorrectlyFormattedVersionInMetadata = !this.meta.TryParseSupportedVersions(!this.OnSteamWorkshop && shouldLogIssues);
			if (this.meta.name.NullOrEmpty())
			{
				if (this.OnSteamWorkshop)
				{
					this.meta.name = "Workshop mod " + this.FolderName;
				}
				else
				{
					this.meta.name = this.FolderName;
				}
			}
			this.HadIncorrectlyFormattedPackageId = !this.meta.TryParsePackageId(this.Official, !this.OnSteamWorkshop && shouldLogIssues);
			this.packageIdLowerCase = this.meta.packageId.ToLower();
			this.meta.InitVersionedData();
			this.meta.ValidateDependencies(shouldLogIssues);
			string publishedFileIdPath = this.PublishedFileIdPath;
			ulong value;
			if (File.Exists(this.PublishedFileIdPath) && ulong.TryParse(File.ReadAllText(publishedFileIdPath), out value))
			{
				this.publishedFileIdInt = new PublishedFileId_t(value);
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0006BB5A File Offset: 0x00069D5A
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x0006BB6D File Offset: 0x00069D6D
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x0006BB78 File Offset: 0x00069D78
		private string PublishedFileIdPath
		{
			get
			{
				return string.Concat(new string[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar.ToString(),
					"About",
					Path.DirectorySeparatorChar.ToString(),
					"PublishedFileId.txt"
				});
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x000034B7 File Offset: 0x000016B7
		public void PrepareForWorkshopUpload()
		{
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0006BBCE File Offset: 0x00069DCE
		public bool CanToUploadToWorkshop()
		{
			return !this.Official && this.Source == ContentSource.ModsFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0006BBF5 File Offset: 0x00069DF5
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0006BBFD File Offset: 0x00069DFD
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (this.publishedFileIdInt == newPfid)
			{
				return;
			}
			this.publishedFileIdInt = newPfid;
			File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0006BC2D File Offset: 0x00069E2D
		public string GetWorkshopName()
		{
			return this.Name;
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0006BC35 File Offset: 0x00069E35
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0006BC3D File Offset: 0x00069E3D
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0006BC45 File Offset: 0x00069E45
		public IList<string> GetWorkshopTags()
		{
			if (!this.translationMod)
			{
				return new List<string>
				{
					"Mod"
				};
			}
			return new List<string>
			{
				"Translation"
			};
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0006BC70 File Offset: 0x00069E70
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.RootDir;
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0006BC78 File Offset: 0x00069E78
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0006BC94 File Offset: 0x00069E94
		public IEnumerable<ModRequirement> GetRequirements()
		{
			int num;
			for (int i = 0; i < this.Dependencies.Count; i = num + 1)
			{
				yield return this.Dependencies[i];
				num = i;
			}
			for (int i = 0; i < this.meta.incompatibleWith.Count; i = num + 1)
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.meta.incompatibleWith[i], false);
				if (modWithIdentifier != null)
				{
					yield return new ModIncompatibility
					{
						packageId = modWithIdentifier.PackageIdPlayerFacing,
						displayName = modWithIdentifier.Name
					};
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x0006BCA4 File Offset: 0x00069EA4
		public override int GetHashCode()
		{
			return this.PackageId.GetHashCode();
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0006BCB1 File Offset: 0x00069EB1
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.PackageIdPlayerFacing,
				"|",
				this.Name,
				"]"
			});
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0006BCE8 File Offset: 0x00069EE8
		public string ToStringLong()
		{
			return this.PackageIdPlayerFacing + "(" + this.RootDir.ToString() + ")";
		}

		// Token: 0x04000F62 RID: 3938
		private DirectoryInfo rootDirInt;

		// Token: 0x04000F63 RID: 3939
		private ContentSource source;

		// Token: 0x04000F64 RID: 3940
		private Texture2D previewImage;

		// Token: 0x04000F65 RID: 3941
		private bool previewImageWasLoaded;

		// Token: 0x04000F66 RID: 3942
		public bool enabled = true;

		// Token: 0x04000F67 RID: 3943
		private ModMetaData.ModMetaDataInternal meta = new ModMetaData.ModMetaDataInternal();

		// Token: 0x04000F68 RID: 3944
		public ModLoadFolders loadFolders;

		// Token: 0x04000F69 RID: 3945
		private WorkshopItemHook workshopHookInt;

		// Token: 0x04000F6A RID: 3946
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x04000F6B RID: 3947
		public bool appendPackageIdSteamPostfix;

		// Token: 0x04000F6C RID: 3948
		public bool translationMod;

		// Token: 0x04000F6D RID: 3949
		public string packageIdLowerCase;

		// Token: 0x04000F6E RID: 3950
		private string descriptionCached;

		// Token: 0x04000F6F RID: 3951
		private const string AboutFolderName = "About";

		// Token: 0x04000F70 RID: 3952
		public static readonly string SteamModPostfix = "_steam";

		// Token: 0x04000F71 RID: 3953
		private static readonly string[] AndToken = new string[]
		{
			" and "
		};

		// Token: 0x04000F72 RID: 3954
		private List<string> unsatisfiedDepsList = new List<string>();

		// Token: 0x02001DCA RID: 7626
		private class ModMetaDataInternal
		{
			// Token: 0x17001E78 RID: 7800
			// (get) Token: 0x0600B618 RID: 46616 RVA: 0x00415491 File Offset: 0x00413691
			// (set) Token: 0x0600B619 RID: 46617 RVA: 0x00415499 File Offset: 0x00413699
			public List<System.Version> SupportedVersions { get; private set; }

			// Token: 0x0600B61A RID: 46618 RVA: 0x004154A4 File Offset: 0x004136A4
			private bool TryParseVersion(string str, bool logIssues = true)
			{
				System.Version version;
				if (!VersionControl.TryParseVersionString(str, out version))
				{
					if (logIssues)
					{
						Log.Error(string.Concat(new string[]
						{
							"Unable to parse version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\""
						}));
					}
					return false;
				}
				this.SupportedVersions.Add(version);
				if (!VersionControl.IsWellFormattedVersionString(str))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Malformed (correct format is Major.Minor) version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\" - parsed as \"",
							version.Major.ToString(),
							".",
							version.Minor.ToString(),
							"\""
						}));
					}
					return false;
				}
				return true;
			}

			// Token: 0x0600B61B RID: 46619 RVA: 0x004155A0 File Offset: 0x004137A0
			public bool TryParseSupportedVersions(bool logIssues = true)
			{
				if (this.targetVersion != null && logIssues)
				{
					Log.Warning("Mod " + this.name + ": targetVersion field is obsolete, use supportedVersions instead.");
				}
				bool flag = false;
				this.SupportedVersions = new List<System.Version>();
				if (this.packageId.ToLower() == "ludeon.rimworld")
				{
					this.SupportedVersions.Add(VersionControl.CurrentVersion);
				}
				else if (this.supportedVersions == null)
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing supported versions list in About.xml! (example: <supportedVersions><li>1.0</li></supportedVersions>)");
					}
					flag = true;
				}
				else if (this.supportedVersions.Count == 0)
				{
					if (logIssues)
					{
						Log.Error("Mod " + this.name + ": <supportedVersions> in mod About.xml must specify at least one version.");
					}
					flag = true;
				}
				else
				{
					for (int i = 0; i < this.supportedVersions.Count; i++)
					{
						flag |= !this.TryParseVersion(this.supportedVersions[i], logIssues);
					}
				}
				this.SupportedVersions = this.SupportedVersions.OrderBy(delegate(System.Version v)
				{
					if (!VersionControl.IsCompatible(v))
					{
						return 100;
					}
					return -100;
				}).ThenByDescending((System.Version v) => v.Major).ThenByDescending((System.Version v) => v.Minor).Distinct<System.Version>().ToList<System.Version>();
				return !flag;
			}

			// Token: 0x0600B61C RID: 46620 RVA: 0x0041571C File Offset: 0x0041391C
			public bool TryParsePackageId(bool isOfficial, bool logIssues = true)
			{
				bool flag = false;
				if (this.packageId.NullOrEmpty())
				{
					string text = "none";
					if (!this.description.NullOrEmpty())
					{
						text = GenText.StableStringHash(this.description).ToString().Replace("-", "");
						text = text.Substring(0, Math.Min(3, text.Length));
					}
					this.packageId = this.ConvertToASCII(this.author + text) + "." + this.ConvertToASCII(this.name);
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing packageId in About.xml! (example: <packageId>AuthorName.ModName.Specific</packageId>)");
					}
					flag = true;
				}
				if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(this.packageId))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Mod ",
							this.name,
							" <packageId> (",
							this.packageId,
							") is not in valid format."
						}));
					}
					flag = true;
				}
				if (!isOfficial && this.packageId.ToLower().Contains("ludeon"))
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " <packageId> contains word \"Ludeon\", which is reserved for official content.");
					}
					flag = true;
				}
				return !flag;
			}

			// Token: 0x0600B61D RID: 46621 RVA: 0x00415860 File Offset: 0x00413A60
			private string ConvertToASCII(string part)
			{
				StringBuilder stringBuilder = new StringBuilder("");
				foreach (char c in part)
				{
					if (!char.IsLetterOrDigit(c) || c >= '\u0080')
					{
						c = c % '\u0019' + 'A';
					}
					stringBuilder.Append(c);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x0600B61E RID: 46622 RVA: 0x004158B8 File Offset: 0x00413AB8
			public void ValidateDependencies(bool logIssues = true)
			{
				for (int i = this.modDependencies.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					ModDependency modDependency = this.modDependencies[i];
					if (modDependency.packageId.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with no <packageId> specified.");
						}
						flag = true;
					}
					else if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(modDependency.packageId))
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with invalid <packageId>: " + modDependency.packageId);
						}
						flag = true;
					}
					if (modDependency.displayName.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" has a dependency (",
								modDependency.packageId,
								") with empty display name."
							}));
						}
						flag = true;
					}
					if (modDependency.downloadUrl.NullOrEmpty() && modDependency.steamWorkshopUrl.NullOrEmpty() && !modDependency.packageId.ToLower().Contains("ludeon"))
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" dependency (",
								modDependency.packageId,
								") needs to have <downloadUrl> and/or <steamWorkshopUrl> specified."
							}));
						}
						flag = true;
					}
					if (flag)
					{
						this.modDependencies.Remove(modDependency);
					}
				}
			}

			// Token: 0x0600B61F RID: 46623 RVA: 0x00415A24 File Offset: 0x00413C24
			public void InitVersionedData()
			{
				string currentVersionStringWithoutBuild = VersionControl.CurrentVersionStringWithoutBuild;
				ModMetaData.VersionedData<string> versionedData = this.descriptionsByVersion;
				string text = (versionedData != null) ? versionedData.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (text != null)
				{
					this.description = text;
				}
				ModMetaData.VersionedData<List<ModDependency>> versionedData2 = this.modDependenciesByVersion;
				List<ModDependency> list = (versionedData2 != null) ? versionedData2.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list != null)
				{
					this.modDependencies = list;
				}
				ModMetaData.VersionedData<List<string>> versionedData3 = this.loadBeforeByVersion;
				List<string> list2 = (versionedData3 != null) ? versionedData3.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list2 != null)
				{
					this.loadBefore = list2;
				}
				ModMetaData.VersionedData<List<string>> versionedData4 = this.loadAfterByVersion;
				List<string> list3 = (versionedData4 != null) ? versionedData4.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list3 != null)
				{
					this.loadAfter = list3;
				}
				ModMetaData.VersionedData<List<string>> versionedData5 = this.incompatibleWithByVersion;
				List<string> list4 = (versionedData5 != null) ? versionedData5.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list4 != null)
				{
					this.incompatibleWith = list4;
				}
			}

			// Token: 0x04007597 RID: 30103
			public string packageId = "";

			// Token: 0x04007598 RID: 30104
			public string name = "";

			// Token: 0x04007599 RID: 30105
			public string shortName = "";

			// Token: 0x0400759A RID: 30106
			public string author = "Anonymous";

			// Token: 0x0400759B RID: 30107
			public List<string> authors;

			// Token: 0x0400759C RID: 30108
			public string modVersion = "";

			// Token: 0x0400759D RID: 30109
			public string url = "";

			// Token: 0x0400759E RID: 30110
			public string description = "No description provided.";

			// Token: 0x0400759F RID: 30111
			public int steamAppId;

			// Token: 0x040075A0 RID: 30112
			public List<string> supportedVersions;

			// Token: 0x040075A1 RID: 30113
			[Unsaved(true)]
			private string targetVersion;

			// Token: 0x040075A2 RID: 30114
			public List<ModDependency> modDependencies = new List<ModDependency>();

			// Token: 0x040075A3 RID: 30115
			public List<string> loadBefore = new List<string>();

			// Token: 0x040075A4 RID: 30116
			public List<string> loadAfter = new List<string>();

			// Token: 0x040075A5 RID: 30117
			public List<string> incompatibleWith = new List<string>();

			// Token: 0x040075A6 RID: 30118
			public List<string> forceLoadBefore = new List<string>();

			// Token: 0x040075A7 RID: 30119
			public List<string> forceLoadAfter = new List<string>();

			// Token: 0x040075A8 RID: 30120
			private ModMetaData.VersionedData<string> descriptionsByVersion;

			// Token: 0x040075A9 RID: 30121
			private ModMetaData.VersionedData<List<ModDependency>> modDependenciesByVersion;

			// Token: 0x040075AA RID: 30122
			private ModMetaData.VersionedData<List<string>> loadBeforeByVersion;

			// Token: 0x040075AB RID: 30123
			private ModMetaData.VersionedData<List<string>> loadAfterByVersion;

			// Token: 0x040075AC RID: 30124
			private ModMetaData.VersionedData<List<string>> incompatibleWithByVersion;

			// Token: 0x040075AD RID: 30125
			public static readonly Regex PackageIdFormatRegex = new Regex("(?=.{1,60}$)^(?!\\.)(?=.*?[.])(?!.*([.])\\1+)[a-zA-Z0-9.]{1,}[a-zA-Z0-9]{1}$");
		}

		// Token: 0x02001DCB RID: 7627
		private class VersionedData<T> where T : class
		{
			// Token: 0x0600B622 RID: 46626 RVA: 0x00415B88 File Offset: 0x00413D88
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				foreach (object obj in xmlRoot.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (!(xmlNode is XmlComment))
					{
						string text = xmlNode.Name.ToLower();
						if (text.StartsWith("v"))
						{
							text = text.Substring(1);
						}
						if (!this.itemForVersion.ContainsKey(text))
						{
							this.itemForVersion[text] = ((typeof(T) == typeof(string)) ? ((T)((object)xmlNode.FirstChild.Value)) : DirectXmlToObject.ObjectFromXml<T>(xmlNode, false));
						}
						else
						{
							Log.Warning("More than one value for a same version of " + typeof(T).Name + " named " + xmlRoot.Name);
						}
					}
				}
			}

			// Token: 0x0600B623 RID: 46627 RVA: 0x00415C88 File Offset: 0x00413E88
			public T GetItemForVersion(string ver)
			{
				if (this.itemForVersion.ContainsKey(ver))
				{
					return this.itemForVersion[ver];
				}
				return default(T);
			}

			// Token: 0x040075AF RID: 30127
			private Dictionary<string, T> itemForVersion = new Dictionary<string, T>();
		}
	}
}

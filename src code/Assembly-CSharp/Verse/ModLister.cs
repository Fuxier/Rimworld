using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200027D RID: 637
	public static class ModLister
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x0006ABB4 File Offset: 0x00068DB4
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x0006ABBC File Offset: 0x00068DBC
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x0006AC18 File Offset: 0x00068E18
		public static List<ExpansionDef> AllExpansions
		{
			get
			{
				if (ModLister.AllExpansionsCached.NullOrEmpty<ExpansionDef>())
				{
					ModLister.AllExpansionsCached = DefDatabase<ExpansionDef>.AllDefsListForReading.Where(delegate(ExpansionDef e)
					{
						ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(e.linkedMod, false);
						return modWithIdentifier == null || modWithIdentifier.Official;
					}).ToList<ExpansionDef>();
				}
				return ModLister.AllExpansionsCached;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x0006AC69 File Offset: 0x00068E69
		public static bool RoyaltyInstalled
		{
			get
			{
				return ModLister.royaltyInstalled && !Prefs.SimulateNotOwningRoyalty;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x0006AC7C File Offset: 0x00068E7C
		public static bool IdeologyInstalled
		{
			get
			{
				return ModLister.ideologyInstalled && !Prefs.SimulateNotOwningIdology;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x0006AC8F File Offset: 0x00068E8F
		public static bool BiotechInstalled
		{
			get
			{
				return ModLister.biotechInstalled && !Prefs.SimulateNotOwningBiotech;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001242 RID: 4674 RVA: 0x0006ACA2 File Offset: 0x00068EA2
		public static bool ShouldLogIssues
		{
			get
			{
				return !ModLister.modListBuilt && !ModLister.nestedRebuildInProgress;
			}
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0006ACB5 File Offset: 0x00068EB5
		static ModLister()
		{
			ModLister.RebuildModList();
			ModLister.modListBuilt = true;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x000034B7 File Offset: 0x000016B7
		public static void EnsureInit()
		{
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0006ACEC File Offset: 0x00068EEC
		public static void RebuildModList()
		{
			ModLister.nestedRebuildInProgress = ModLister.rebuildingModList;
			ModLister.rebuildingModList = true;
			string s = "Rebuilding mods list";
			ModLister.mods.Clear();
			ModLister.modsByPackageId.Clear();
			ModLister.modsByPackageIdIgnorePostfix.Clear();
			WorkshopItems.EnsureInit();
			s += "\nAdding official mods from content folder:";
			foreach (string localAbsPath in from d in new DirectoryInfo(GenFilePaths.OfficialModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData = new ModMetaData(localAbsPath, true);
				if (ModLister.TryAddMod(modMetaData))
				{
					s = s + "\n  Adding " + modMetaData.ToStringLong();
				}
			}
			s += "\nAdding mods from mods folder:";
			foreach (string localAbsPath2 in from d in new DirectoryInfo(GenFilePaths.ModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData2 = new ModMetaData(localAbsPath2, false);
				if (ModLister.TryAddMod(modMetaData2))
				{
					s = s + "\n  Adding " + modMetaData2.ToStringLong();
				}
			}
			s += "\nAdding mods from Steam:";
			foreach (WorkshopItem workshopItem in from it in WorkshopItems.AllSubscribedItems
			where it is WorkshopItem_Mod
			select it)
			{
				ModMetaData modMetaData3 = new ModMetaData(workshopItem);
				if (ModLister.TryAddMod(modMetaData3))
				{
					s = s + "\n  Adding " + modMetaData3.ToStringLong();
				}
			}
			s += "\nDeactivating not-installed mods:";
			ModsConfig.DeactivateNotInstalledMods(delegate(string log)
			{
				s = s + "\n   " + log;
			});
			if (Prefs.SimulateNotOwningRoyalty)
			{
				ModsConfig.SetActive("ludeon.rimworld.royalty", false);
			}
			if (Prefs.SimulateNotOwningIdology)
			{
				ModsConfig.SetActive("ludeon.rimworld.ideology", false);
			}
			if (Prefs.SimulateNotOwningBiotech)
			{
				ModsConfig.SetActive("ludeon.rimworld.biotech", false);
			}
			if (ModLister.mods.Count((ModMetaData m) => m.Active) == 0)
			{
				s += "\nThere are no active mods. Activating Core mod.";
				ModLister.mods.First((ModMetaData m) => m.IsCoreMod).Active = true;
			}
			ModLister.RecacheExpansionsInstalled();
			if (Prefs.LogVerbose)
			{
				Log.Message(s);
			}
			ModLister.rebuildingModList = false;
			ModLister.nestedRebuildInProgress = false;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0006B014 File Offset: 0x00069214
		public static int InstalledModsListHash(bool activeOnly)
		{
			int num = 17;
			int num2 = 0;
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				if (!activeOnly || ModsConfig.IsActive(modMetaData.PackageId))
				{
					num = num * 31 + modMetaData.GetHashCode();
					num = num * 31 + num2 * 2654241;
					num2++;
				}
			}
			return num;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0006B08C File Offset: 0x0006928C
		public static ModMetaData GetModWithIdentifier(string identifier, bool ignorePostfix = false)
		{
			if (ignorePostfix)
			{
				if (!ModLister.modsByPackageIdIgnorePostfix.ContainsKey(identifier))
				{
					return null;
				}
				return ModLister.modsByPackageIdIgnorePostfix[identifier].ElementAtOrDefault(0);
			}
			else
			{
				if (!ModLister.modsByPackageId.ContainsKey(identifier))
				{
					return null;
				}
				return ModLister.modsByPackageId[identifier].ElementAtOrDefault(0);
			}
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0006B0E0 File Offset: 0x000692E0
		public static ModMetaData GetActiveModWithIdentifier(string identifier, bool ignorePostfix = false)
		{
			Dictionary<string, List<ModMetaData>> dictionary = ignorePostfix ? ModLister.modsByPackageIdIgnorePostfix : ModLister.modsByPackageId;
			if (!dictionary.ContainsKey(identifier))
			{
				return null;
			}
			foreach (ModMetaData modMetaData in dictionary[identifier])
			{
				if (modMetaData.Active)
				{
					return modMetaData;
				}
			}
			return null;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0006B158 File Offset: 0x00069358
		public static ExpansionDef GetExpansionWithIdentifier(string packageId)
		{
			for (int i = 0; i < ModLister.AllExpansions.Count; i++)
			{
				if (ModLister.AllExpansions[i].linkedMod == packageId)
				{
					return ModLister.AllExpansions[i];
				}
			}
			return null;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0006B1A0 File Offset: 0x000693A0
		public static bool HasActiveModWithName(string name)
		{
			foreach (ModMetaData modMetaData in ModLister.mods)
			{
				if (modMetaData.Active && modMetaData.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0006B208 File Offset: 0x00069408
		public static bool AnyFromListActive(List<string> mods)
		{
			using (List<string>.Enumerator enumerator = mods.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ModLister.GetActiveModWithIdentifier(enumerator.Current, false) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0006B260 File Offset: 0x00069460
		private static void RecacheExpansionsInstalled()
		{
			ModLister.royaltyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.royalty");
			ModLister.ideologyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.ideology");
			ModLister.biotechInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.biotech");
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0006B2A0 File Offset: 0x000694A0
		private static bool TryAddMod(ModMetaData mod)
		{
			if (mod.Official && !mod.IsCoreMod && SteamManager.Initialized && mod.SteamAppId != 0)
			{
				bool flag = true;
				try
				{
					flag = SteamApps.BIsDlcInstalled(new AppId_t((uint)mod.SteamAppId));
				}
				catch (Exception arg)
				{
					Log.Error("Could not determine if a DLC is installed: " + arg);
				}
				if (!flag)
				{
					return false;
				}
			}
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(mod.PackageId, false);
			if (modWithIdentifier == null)
			{
				ModLister.mods.Add(mod);
				if (ModLister.modsByPackageId.ContainsKey(mod.PackageId))
				{
					ModLister.modsByPackageId[mod.PackageId].Add(mod);
				}
				else
				{
					ModLister.modsByPackageId.Add(mod.PackageId, new List<ModMetaData>
					{
						mod
					});
				}
				if (ModLister.modsByPackageIdIgnorePostfix.ContainsKey(mod.packageIdLowerCase))
				{
					ModLister.modsByPackageIdIgnorePostfix[mod.packageIdLowerCase].Add(mod);
				}
				else
				{
					ModLister.modsByPackageIdIgnorePostfix.Add(mod.packageIdLowerCase, new List<ModMetaData>
					{
						mod
					});
				}
				return true;
			}
			if (mod.RootDir.FullName != modWithIdentifier.RootDir.FullName)
			{
				if (mod.OnSteamWorkshop != modWithIdentifier.OnSteamWorkshop)
				{
					ModMetaData modMetaData = mod.OnSteamWorkshop ? mod : modWithIdentifier;
					if (!modMetaData.appendPackageIdSteamPostfix)
					{
						modMetaData.appendPackageIdSteamPostfix = true;
						return ModLister.TryAddMod(mod);
					}
				}
				Log.Error(string.Concat(new string[]
				{
					"Tried loading mod with the same packageId multiple times: ",
					mod.PackageIdPlayerFacing,
					". Ignoring the duplicates.\n",
					mod.RootDir.FullName,
					"\n",
					modWithIdentifier.RootDir.FullName
				}));
				return false;
			}
			return false;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0006B458 File Offset: 0x00069658
		private static bool CheckDLC(bool dlc, string featureName, string dlcNameIndef, string installedPropertyName)
		{
			if (!dlc)
			{
				Log.ErrorOnce(string.Format("{0} is {1}-specific game system. If you want to use this code please check ModLister.{2} before calling it.", featureName, dlcNameIndef, installedPropertyName), featureName.GetHashCode());
			}
			return dlc;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0006B476 File Offset: 0x00069676
		public static bool CheckRoyalty(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled, featureNameSingular, "a Royalty", "RoyaltyInstalled");
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0006B48D File Offset: 0x0006968D
		public static bool CheckIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.IdeologyInstalled, featureNameSingular, "an Ideology", "IdeologyInstalled");
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0006B4A4 File Offset: 0x000696A4
		public static bool CheckIdeologyOrBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.IdeologyInstalled || ModLister.BiotechInstalled, featureNameSingular, "a Ideology or Biotech", "IdeologyInstalled or BiotechInstalled");
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0006B4C5 File Offset: 0x000696C5
		public static bool CheckBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.BiotechInstalled, featureNameSingular, "a Biotech", "BiotechInstalled");
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0006B4DC File Offset: 0x000696DC
		public static bool CheckRoyaltyAndIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled && ModLister.IdeologyInstalled, featureNameSingular, "a Royalty and Ideology", "RoyaltyInstalled and IdeologyInstalled");
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0006B4FD File Offset: 0x000696FD
		public static bool CheckRoyaltyOrIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.IdeologyInstalled, featureNameSingular, "a Royalty or Ideology", "RoyaltyInstalled or IdeologyInstalled");
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0006B51E File Offset: 0x0006971E
		public static bool CheckRoyaltyOrBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.BiotechInstalled, featureNameSingular, "a Royalty or Biotech", "RoyaltyInstalled or BiotechInstalled");
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0006B53F File Offset: 0x0006973F
		public static bool CheckRoyaltyOrIdeologyOrBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.BiotechInstalled || ModLister.IdeologyInstalled, featureNameSingular, "a Royalty or Ideology or Biotech", "RoyaltyInstalled or IdeologyInstalled or BiotechInstalled");
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0006B567 File Offset: 0x00069767
		public static bool CheckAnyExpansion(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.IdeologyInstalled || ModLister.BiotechInstalled, featureNameSingular, "a Royalty or Ideology or Biotech", "RoyaltyInstalled or IdeologyInstalled or BiotechInstalled");
		}

		// Token: 0x04000F58 RID: 3928
		private static List<ModMetaData> mods = new List<ModMetaData>();

		// Token: 0x04000F59 RID: 3929
		private static Dictionary<string, List<ModMetaData>> modsByPackageId = new Dictionary<string, List<ModMetaData>>(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x04000F5A RID: 3930
		private static Dictionary<string, List<ModMetaData>> modsByPackageIdIgnorePostfix = new Dictionary<string, List<ModMetaData>>(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x04000F5B RID: 3931
		private static bool modListBuilt;

		// Token: 0x04000F5C RID: 3932
		private static bool rebuildingModList;

		// Token: 0x04000F5D RID: 3933
		private static bool nestedRebuildInProgress;

		// Token: 0x04000F5E RID: 3934
		private static List<ExpansionDef> AllExpansionsCached;

		// Token: 0x04000F5F RID: 3935
		private static bool royaltyInstalled;

		// Token: 0x04000F60 RID: 3936
		private static bool ideologyInstalled;

		// Token: 0x04000F61 RID: 3937
		private static bool biotechInstalled;
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000285 RID: 645
	public static class ModsConfig
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0006C694 File Offset: 0x0006A894
		public static IEnumerable<ModMetaData> ActiveModsInLoadOrder
		{
			get
			{
				ModLister.EnsureInit();
				if (ModsConfig.activeModsInLoadOrderCachedDirty)
				{
					ModsConfig.activeModsInLoadOrderCached.Clear();
					for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
					{
						ModsConfig.activeModsInLoadOrderCached.Add(ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false));
					}
					ModsConfig.activeModsInLoadOrderCachedDirty = false;
				}
				return ModsConfig.activeModsInLoadOrderCached;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x0006C6FC File Offset: 0x0006A8FC
		public static ExpansionDef LastInstalledExpansion
		{
			get
			{
				for (int i = ModsConfig.data.knownExpansions.Count - 1; i >= 0; i--)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(ModsConfig.data.knownExpansions[i]);
					if (expansionWithIdentifier != null && !expansionWithIdentifier.isCore && expansionWithIdentifier.Status != ExpansionStatus.NotInstalled)
					{
						return expansionWithIdentifier;
					}
				}
				return null;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x0006C752 File Offset: 0x0006A952
		public static bool RoyaltyActive
		{
			get
			{
				return ModsConfig.royaltyActive;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x0006C759 File Offset: 0x0006A959
		public static bool IdeologyActive
		{
			get
			{
				return ModsConfig.ideologyActive;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x0006C760 File Offset: 0x0006A960
		public static bool BiotechActive
		{
			get
			{
				return ModsConfig.biotechActive;
			}
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0006C768 File Offset: 0x0006A968
		static ModsConfig()
		{
			bool flag = false;
			bool flag2 = false;
			ModsConfig.data = DirectXmlLoader.ItemFromXmlFile<ModsConfig.ModsConfigData>(GenFilePaths.ModsConfigFilePath, true);
			if (ModsConfig.data.version != null)
			{
				bool flag3 = false;
				int num2;
				if (ModsConfig.data.version.Contains("."))
				{
					int num = VersionControl.MinorFromVersionString(ModsConfig.data.version);
					if (VersionControl.MajorFromVersionString(ModsConfig.data.version) != VersionControl.CurrentMajor || num != VersionControl.CurrentMinor)
					{
						flag3 = true;
					}
				}
				else if (ModsConfig.data.version.Length > 0 && ModsConfig.data.version.All((char x) => char.IsNumber(x)) && int.TryParse(ModsConfig.data.version, out num2) && num2 <= 2009)
				{
					flag3 = true;
				}
				if (flag3)
				{
					Log.Message(string.Concat(new string[]
					{
						"Mods config data is from version ",
						ModsConfig.data.version,
						" while we are running ",
						VersionControl.CurrentVersionStringWithRev,
						". Resetting."
					}));
					ModsConfig.data = new ModsConfig.ModsConfigData();
					flag = true;
				}
			}
			for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
			{
				string packageId = ModsConfig.data.activeMods[i];
				if (ModLister.GetModWithIdentifier(packageId, false) == null)
				{
					ModMetaData modMetaData = ModLister.AllInstalledMods.FirstOrDefault((ModMetaData m) => m.FolderName == packageId);
					if (modMetaData != null)
					{
						ModsConfig.data.activeMods[i] = modMetaData.PackageId;
						flag2 = true;
					}
					string text;
					if (ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(packageId, out text) && ModLister.GetModWithIdentifier(text, false) != null)
					{
						ModsConfig.data.activeMods[i] = text;
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModMetaData modMetaData2 in ModLister.AllInstalledMods)
			{
				if (modMetaData2.Active)
				{
					if (hashSet.Contains(modMetaData2.PackageIdNonUnique))
					{
						modMetaData2.Active = false;
						Log.Warning("There was more than one enabled instance of mod with PackageID: " + modMetaData2.PackageIdNonUnique + ". Disabling the duplicates.");
						continue;
					}
					hashSet.Add(modMetaData2.PackageIdNonUnique);
				}
				if (!modMetaData2.IsCoreMod && modMetaData2.Official && ModsConfig.IsExpansionNew(modMetaData2.PackageId))
				{
					ModsConfig.SetActive(modMetaData2.PackageId, true);
					ModsConfig.AddKnownExpansion(modMetaData2.PackageId);
					int num3 = ModsConfig.data.activeMods.IndexOf(modMetaData2.PackageId);
					if (!modMetaData2.ForceLoadAfter.NullOrEmpty<string>())
					{
						foreach (string identifier in modMetaData2.ForceLoadAfter)
						{
							ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(identifier, false);
							if (activeModWithIdentifier != null)
							{
								int num4 = ModsConfig.data.activeMods.IndexOf(activeModWithIdentifier.PackageId);
								if (num4 != -1 && num4 > num3)
								{
									string text2;
									ModsConfig.TryReorder(num3, num4, out text2);
									Gen.Swap<int>(ref num4, ref num3);
								}
							}
						}
					}
					if (!modMetaData2.ForceLoadBefore.NullOrEmpty<string>())
					{
						foreach (string identifier2 in modMetaData2.ForceLoadBefore)
						{
							ModMetaData activeModWithIdentifier2 = ModLister.GetActiveModWithIdentifier(identifier2, false);
							if (activeModWithIdentifier2 != null)
							{
								int num5 = ModsConfig.data.activeMods.IndexOf(activeModWithIdentifier2.PackageId);
								if (num5 != -1 && num5 < num3)
								{
									string text2;
									ModsConfig.TryReorder(num3, num5, out text2);
									Gen.Swap<int>(ref num5, ref num3);
								}
							}
						}
					}
					Prefs.Notify_NewExpansion();
					flag2 = true;
				}
			}
			if (ModsConfig.newKnownExpansions.Any<string>())
			{
				ModsConfig.newKnownExpansions.SortBy((string x) => ModsConfig.ExpansionsInReleaseOrder.IndexOf(x));
				ModsConfig.data.knownExpansions.AddRange(ModsConfig.newKnownExpansions);
				ModsConfig.newKnownExpansions.Clear();
			}
			if (!File.Exists(GenFilePaths.ModsConfigFilePath) || flag)
			{
				ModsConfig.Reset();
			}
			else if (flag2)
			{
				ModsConfig.Save();
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0006CC2C File Offset: 0x0006AE2C
		public static bool TryGetPackageIdWithoutExtraSteamPostfix(string packageId, out string nonSteamPackageId)
		{
			if (packageId.EndsWith(ModMetaData.SteamModPostfix))
			{
				nonSteamPackageId = packageId.Substring(0, packageId.Length - ModMetaData.SteamModPostfix.Length);
				return true;
			}
			nonSteamPackageId = null;
			return false;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0006CC5C File Offset: 0x0006AE5C
		public static void DeactivateNotInstalledMods(Action<string> logCallback = null)
		{
			for (int i = ModsConfig.data.activeMods.Count - 1; i >= 0; i--)
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false);
				string identifier;
				if (modWithIdentifier == null && ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(ModsConfig.data.activeMods[i], out identifier))
				{
					modWithIdentifier = ModLister.GetModWithIdentifier(identifier, false);
				}
				if (modWithIdentifier == null)
				{
					if (logCallback != null)
					{
						logCallback("Deactivating " + ModsConfig.data.activeMods[i]);
					}
					ModsConfig.data.activeMods.RemoveAt(i);
				}
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0006CCFC File Offset: 0x0006AEFC
		public static void Reset()
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add("ludeon.rimworld");
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				if (modMetaData.Official && !modMetaData.IsCoreMod && modMetaData.VersionCompatible)
				{
					ModsConfig.data.activeMods.Add(modMetaData.PackageId);
				}
			}
			ModsConfig.TrySortMods();
			ModsConfig.Save();
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0006CDA0 File Offset: 0x0006AFA0
		public static bool TryReorder(int modIndex, int newIndex, out string errorMessage)
		{
			errorMessage = null;
			if (modIndex == newIndex)
			{
				return false;
			}
			if (ModsConfig.ReorderConflict(modIndex, newIndex, out errorMessage))
			{
				return false;
			}
			ModsConfig.data.activeMods.Insert(newIndex, ModsConfig.data.activeMods[modIndex]);
			ModsConfig.data.activeMods.RemoveAt((modIndex < newIndex) ? modIndex : (modIndex + 1));
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
			return true;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0006CE04 File Offset: 0x0006B004
		private static bool ReorderConflict(int modIndex, int newIndex, out string errorMessage)
		{
			errorMessage = null;
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[modIndex], false);
			if (modWithIdentifier.IsCoreMod)
			{
				foreach (string text in ModsConfig.ExpansionsInReleaseOrder)
				{
					int num = ModsConfig.data.activeMods.IndexOf(text);
					if (num != -1 && num < newIndex)
					{
						errorMessage = "ModReorderConflict_MustLoadBefore".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(text, false).Name);
						return true;
					}
				}
			}
			if (modWithIdentifier.Source == ContentSource.OfficialModsFolder && ModsConfig.data.activeMods.IndexOf(ModsConfig.Core) >= newIndex)
			{
				errorMessage = "ModReorderConflict_MustLoadAfter".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.Core, false).Name);
				return true;
			}
			if (!modWithIdentifier.ForceLoadBefore.NullOrEmpty<string>())
			{
				foreach (string identifier in modWithIdentifier.ForceLoadBefore)
				{
					ModMetaData modWithIdentifier2 = ModLister.GetModWithIdentifier(identifier, false);
					if (modWithIdentifier2 != null)
					{
						for (int i = newIndex - 1; i >= 0; i--)
						{
							if (modWithIdentifier2.SamePackageId(ModsConfig.data.activeMods[i], false))
							{
								errorMessage = "ModReorderConflict_MustLoadBefore".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false).Name);
								return true;
							}
						}
					}
				}
			}
			if (!modWithIdentifier.ForceLoadAfter.NullOrEmpty<string>())
			{
				foreach (string identifier2 in modWithIdentifier.ForceLoadAfter)
				{
					ModMetaData modWithIdentifier3 = ModLister.GetModWithIdentifier(identifier2, false);
					if (modWithIdentifier3 != null)
					{
						for (int j = newIndex; j < ModsConfig.data.activeMods.Count; j++)
						{
							if (modWithIdentifier3.SamePackageId(ModsConfig.data.activeMods[j], false))
							{
								errorMessage = "ModReorderConflict_MustLoadAfter".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[j], false).Name);
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0006D0BC File Offset: 0x0006B2BC
		public static void Reorder(List<int> newIndices)
		{
			List<string> list = new List<string>();
			foreach (int index in newIndices)
			{
				list.Add(ModsConfig.data.activeMods[index]);
			}
			ModsConfig.data.activeMods = list;
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0006D130 File Offset: 0x0006B330
		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.IsActive(mod.PackageId);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0006D140 File Offset: 0x0006B340
		public static bool AreAllActive(string mods)
		{
			if (mods != null && mods.Contains(','))
			{
				string[] array = mods.ToLower().Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					if (!ModsConfig.IsActive(array[i].Trim()))
					{
						return false;
					}
				}
				return true;
			}
			return ModsConfig.IsActive(mods);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0006D198 File Offset: 0x0006B398
		public static bool IsAnyActiveOrEmpty(IEnumerable<string> mods, bool trimNames = false)
		{
			if (!mods.EnumerableNullOrEmpty<string>())
			{
				foreach (string text in mods)
				{
					if (ModsConfig.IsActive(trimNames ? text.Trim() : text))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0006D200 File Offset: 0x0006B400
		public static bool IsActive(string id)
		{
			return ModsConfig.activeModsHashSet.Contains(id.ToLower());
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0006D212 File Offset: 0x0006B412
		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.PackageId, active);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0006D220 File Offset: 0x0006B420
		public static void SetActive(string modIdentifier, bool active)
		{
			string item = modIdentifier.ToLower();
			if (active)
			{
				if (!ModsConfig.data.activeMods.Contains(item))
				{
					ModsConfig.data.activeMods.Add(item);
					ModsConfig.EnsureModAdheresToForcedLoadOrder(modIdentifier);
				}
			}
			else if (ModsConfig.data.activeMods.Contains(item))
			{
				ModsConfig.data.activeMods.Remove(item);
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0006D28C File Offset: 0x0006B48C
		public static void EnsureModAdheresToForcedLoadOrder(string modIdentifier)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(modIdentifier, false);
			if (modWithIdentifier == null)
			{
				return;
			}
			string item = modIdentifier.ToLower();
			if (!ModsConfig.data.activeMods.Contains(item))
			{
				return;
			}
			int? num = null;
			int num2 = ModsConfig.data.activeMods.IndexOf(item);
			if (!modWithIdentifier.ForceLoadAfter.NullOrEmpty<string>())
			{
				foreach (string text in modWithIdentifier.ForceLoadAfter)
				{
					int num3 = ModsConfig.data.activeMods.IndexOf(text.ToLower());
					if (num3 != -1)
					{
						num = new int?(Mathf.Max(num ?? int.MinValue, num3 + 1));
					}
				}
			}
			if (!modWithIdentifier.ForceLoadBefore.NullOrEmpty<string>())
			{
				foreach (string text2 in modWithIdentifier.ForceLoadBefore)
				{
					int num4 = ModsConfig.data.activeMods.IndexOf(text2.ToLower());
					if (num4 != -1)
					{
						num = new int?(Mathf.Min(num ?? int.MaxValue, num4));
					}
				}
			}
			if (num != null && num.Value != num2)
			{
				ModsConfig.data.activeMods.Remove(item);
				ModsConfig.data.activeMods.Insert(num.Value, item);
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x0006D438 File Offset: 0x0006B638
		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod, false) != null
			select mod).ToList<string>();
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0006D473 File Offset: 0x0006B673
		public static bool IsExpansionNew(string id)
		{
			return !ModsConfig.data.knownExpansions.Contains(id.ToLower());
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0006D48D File Offset: 0x0006B68D
		public static void AddKnownExpansion(string id)
		{
			if (ModsConfig.IsExpansionNew(id))
			{
				ModsConfig.newKnownExpansions.Add(id.ToLower());
			}
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0006D4A7 File Offset: 0x0006B6A7
		public static void Save()
		{
			ModsConfig.data.version = VersionControl.CurrentVersionStringWithRev;
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0006D4C7 File Offset: 0x0006B6C7
		public static void SaveFromList(List<string> mods)
		{
			DirectXmlSaver.SaveDataObject(new ModsConfig.ModsConfigData
			{
				version = VersionControl.CurrentVersionStringWithRev,
				activeMods = mods,
				knownExpansions = ModsConfig.data.knownExpansions
			}, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0006D4FC File Offset: 0x0006B6FC
		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0006D54C File Offset: 0x0006B74C
		public static Dictionary<string, string> GetModWarnings()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			for (int i = 0; i < mods.Count; i++)
			{
				int index = i;
				ModMetaData modMetaData = mods[index];
				StringBuilder stringBuilder = new StringBuilder("");
				ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(modMetaData.PackageId, false);
				if (activeModWithIdentifier != null && modMetaData != activeModWithIdentifier)
				{
					stringBuilder.AppendLine("ModWithSameIdAlreadyActive".Translate(activeModWithIdentifier.Name));
				}
				List<string> list = ModsConfig.FindConflicts(modMetaData.IncompatibleWith, null);
				if (list.Any<string>())
				{
					stringBuilder.AppendLine("ModIncompatibleWithTip".Translate(list.ToCommaList(true, false)));
				}
				List<string> list2 = ModsConfig.FindConflicts(modMetaData.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index);
				if (list2.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadBefore".Translate(list2.ToCommaList(true, false)));
				}
				List<string> list3 = ModsConfig.FindConflicts(modMetaData.ForceLoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index);
				if (list3.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadBefore".Translate(list3.ToCommaList(true, false)));
				}
				List<string> list4 = ModsConfig.FindConflicts(modMetaData.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index);
				if (list4.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadAfter".Translate(list4.ToCommaList(true, false)));
				}
				List<string> list5 = ModsConfig.FindConflicts(modMetaData.ForceLoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index);
				if (list5.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadAfter".Translate(list5.ToCommaList(true, false)));
				}
				if (modMetaData.Dependencies.Any<ModDependency>())
				{
					List<string> list6 = modMetaData.UnsatisfiedDependencies();
					if (list6.Any<string>())
					{
						stringBuilder.AppendLine("ModUnsatisfiedDependency".Translate(list6.ToCommaList(true, false)));
					}
				}
				dictionary.Add(modMetaData.PackageId, stringBuilder.ToString().TrimEndNewlines());
			}
			return dictionary;
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0006D7C4 File Offset: 0x0006B9C4
		public static bool ModHasAnyOrderingIssues(ModMetaData mod)
		{
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			int index = mods.IndexOf(mod);
			return index != -1 && (ModsConfig.FindConflicts(mod.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index).Count > 0 || ModsConfig.FindConflicts(mod.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index).Count > 0);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0006D848 File Offset: 0x0006BA48
		private static List<string> FindConflicts(List<string> modsToCheck, Func<ModMetaData, bool> predicate)
		{
			List<string> list = new List<string>();
			foreach (string identifier in modsToCheck)
			{
				ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(identifier, true);
				if (activeModWithIdentifier != null && (predicate == null || predicate(activeModWithIdentifier)))
				{
					list.Add(activeModWithIdentifier.Name);
				}
			}
			return list;
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0006D8B8 File Offset: 0x0006BAB8
		public static void TrySortMods()
		{
			List<ModMetaData> list = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			DirectedAcyclicGraph directedAcyclicGraph = new DirectedAcyclicGraph(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				ModMetaData modMetaData = list[i];
				using (IEnumerator<string> enumerator = modMetaData.LoadBefore.Concat(modMetaData.ForceLoadBefore).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string before = enumerator.Current;
						ModMetaData modMetaData2 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(before, true));
						if (modMetaData2 != null)
						{
							directedAcyclicGraph.AddEdge(list.IndexOf(modMetaData2), i);
						}
					}
				}
				using (IEnumerator<string> enumerator = modMetaData.LoadAfter.Concat(modMetaData.ForceLoadAfter).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string after = enumerator.Current;
						ModMetaData modMetaData3 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(after, true));
						if (modMetaData3 != null)
						{
							directedAcyclicGraph.AddEdge(i, list.IndexOf(modMetaData3));
						}
					}
				}
			}
			int num = directedAcyclicGraph.FindCycle();
			if (num != -1)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("ModCyclicDependency".Translate(list[num].Name), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				return;
			}
			ModsConfig.Reorder(directedAcyclicGraph.TopologicalSort());
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0006DA44 File Offset: 0x0006BC44
		private static void RecacheActiveMods()
		{
			ModsConfig.activeModsHashSet.Clear();
			foreach (string item in ModsConfig.data.activeMods)
			{
				ModsConfig.activeModsHashSet.Add(item);
			}
			ModsConfig.royaltyActive = ModsConfig.IsActive("ludeon.rimworld.royalty");
			ModsConfig.ideologyActive = ModsConfig.IsActive("ludeon.rimworld.ideology");
			ModsConfig.biotechActive = ModsConfig.IsActive("ludeon.rimworld.biotech");
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x04000F84 RID: 3972
		private static ModsConfig.ModsConfigData data;

		// Token: 0x04000F85 RID: 3973
		private static bool royaltyActive;

		// Token: 0x04000F86 RID: 3974
		private static bool ideologyActive;

		// Token: 0x04000F87 RID: 3975
		private static bool biotechActive;

		// Token: 0x04000F88 RID: 3976
		private static HashSet<string> activeModsHashSet = new HashSet<string>();

		// Token: 0x04000F89 RID: 3977
		private static List<ModMetaData> activeModsInLoadOrderCached = new List<ModMetaData>();

		// Token: 0x04000F8A RID: 3978
		private static bool activeModsInLoadOrderCachedDirty;

		// Token: 0x04000F8B RID: 3979
		private static List<string> newKnownExpansions = new List<string>();

		// Token: 0x04000F8C RID: 3980
		private static readonly string Core = "ludeon.rimworld";

		// Token: 0x04000F8D RID: 3981
		private static readonly List<string> ExpansionsInReleaseOrder = new List<string>
		{
			"ludeon.rimworld.royalty",
			"ludeon.rimworld.ideology",
			"ludeon.rimworld.biotech"
		};

		// Token: 0x02001DD0 RID: 7632
		private class ModsConfigData
		{
			// Token: 0x040075C5 RID: 30149
			[LoadAlias("buildNumber")]
			public string version;

			// Token: 0x040075C6 RID: 30150
			public List<string> activeMods = new List<string>();

			// Token: 0x040075C7 RID: 30151
			public List<string> knownExpansions = new List<string>();
		}
	}
}

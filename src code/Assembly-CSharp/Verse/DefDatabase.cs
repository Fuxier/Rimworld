using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A1 RID: 161
	public static class DefDatabase<T> where T : Def
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001DFF1 File Offset: 0x0001C1F1
		public static IEnumerable<T> AllDefs
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001DFF1 File Offset: 0x0001C1F1
		public static List<T> AllDefsListForReading
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001DFF8 File Offset: 0x0001C1F8
		public static int DefCount
		{
			get
			{
				return DefDatabase<T>.defsList.Count;
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001E004 File Offset: 0x0001C204
		public static void AddAllInMods()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModContentPack modContentPack in (from m in LoadedModManager.RunningMods
			orderby m.OverwritePriority
			select m).ThenBy((ModContentPack x) => LoadedModManager.RunningModsListForReading.IndexOf(x)))
			{
				string sourceName = modContentPack.ToString();
				hashSet.Clear();
				foreach (T t in GenDefDatabase.DefsToGoInDatabase<T>(modContentPack))
				{
					if (!hashSet.Add(t.defName))
					{
						Log.Error(string.Concat(new object[]
						{
							"Mod ",
							modContentPack,
							" has multiple ",
							typeof(T),
							"s named ",
							t.defName,
							". Skipping."
						}));
					}
					else
					{
						DefDatabase<T>.<AddAllInMods>g__AddDef|9_0(t, sourceName);
					}
				}
			}
			foreach (!0 def in LoadedModManager.PatchedDefsForReading.OfType<T>())
			{
				DefDatabase<T>.<AddAllInMods>g__AddDef|9_0(def, "Patches");
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001E19C File Offset: 0x0001C39C
		public static void Add(IEnumerable<T> defs)
		{
			foreach (!0 def in defs)
			{
				DefDatabase<T>.Add(def);
			}
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001E1E4 File Offset: 0x0001C3E4
		public static void Add(T def)
		{
			while (DefDatabase<T>.defsByName.ContainsKey(def.defName))
			{
				Log.Error(string.Concat(new object[]
				{
					"Adding duplicate ",
					typeof(T),
					" name: ",
					def.defName
				}));
				T t = def;
				t.defName += Mathf.RoundToInt(Rand.Value * 1000f);
			}
			DefDatabase<T>.defsList.Add(def);
			DefDatabase<T>.defsByName.Add(def.defName, def);
			if (DefDatabase<T>.defsList.Count > 65535)
			{
				Log.Error(string.Concat(new object[]
				{
					"Too many ",
					typeof(T),
					"; over ",
					ushort.MaxValue
				}));
			}
			def.index = (ushort)(DefDatabase<T>.defsList.Count - 1);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001E2F4 File Offset: 0x0001C4F4
		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(def.defName);
			DefDatabase<T>.defsList.Remove(def);
			DefDatabase<T>.SetIndices();
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001E31D File Offset: 0x0001C51D
		public static void Clear()
		{
			DefDatabase<T>.defsList.Clear();
			DefDatabase<T>.defsByName.Clear();
			DefDatabase<T>.defsByShortHash.Clear();
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001E340 File Offset: 0x0001C540
		public static void ClearCachedData()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].ClearCachedData();
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001E378 File Offset: 0x0001C578
		public static void ResolveAllReferences(bool onlyExactlyMyType = true, bool parallel = false)
		{
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ResolveAllReferences " + typeof(T).FullName);
			try
			{
				Action<T> action = delegate(T def)
				{
					if (onlyExactlyMyType && def.GetType() != typeof(T))
					{
						return;
					}
					DeepProfiler.Start("Resolver call");
					try
					{
						def.ResolveReferences();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error while resolving references for def ",
							def,
							": ",
							ex
						}));
					}
					finally
					{
						DeepProfiler.End();
					}
				};
				if (parallel)
				{
					GenThreading.ParallelForEach<T>(DefDatabase<T>.defsList, action, -1);
				}
				else
				{
					for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
					{
						action(DefDatabase<T>.defsList[i]);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001E454 File Offset: 0x0001C654
		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].index = (ushort)i;
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001E490 File Offset: 0x0001C690
		public static void ErrorCheckAllDefs()
		{
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				try
				{
					if (!t.ignoreConfigErrors)
					{
						foreach (string text in t.ConfigErrors())
						{
							Log.Error(string.Concat(new object[]
							{
								"Config error in ",
								t,
								": ",
								text
							}));
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception in ConfigErrors() of ",
						t.defName,
						": ",
						ex
					}));
				}
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001E594 File Offset: 0x0001C794
		public static T GetNamed(string defName, bool errorOnFail = true)
		{
			if (errorOnFail)
			{
				T result;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result))
				{
					return result;
				}
				Log.Error(string.Concat(new object[]
				{
					"Failed to find ",
					typeof(T),
					" named ",
					defName,
					". There are ",
					DefDatabase<T>.defsList.Count,
					" defs of this type loaded."
				}));
				return default(T);
			}
			else
			{
				T result2;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result2))
				{
					return result2;
				}
				return default(T);
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001E62C File Offset: 0x0001C82C
		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001E638 File Offset: 0x0001C838
		public static T GetByShortHash(ushort shortHash)
		{
			T result;
			if (DefDatabase<T>.defsByShortHash.TryGetValue(shortHash, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001E660 File Offset: 0x0001C860
		public static void InitializeShortHashDictionary()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsByShortHash.SetOrAdd(DefDatabase<T>.defsList[i].shortHash, DefDatabase<T>.defsList[i]);
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001E6AC File Offset: 0x0001C8AC
		public static T GetRandom()
		{
			return DefDatabase<T>.defsList.RandomElement<T>();
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001E6D8 File Offset: 0x0001C8D8
		[CompilerGenerated]
		internal static void <AddAllInMods>g__AddDef|9_0(T def, string sourceName)
		{
			if (def.defName == "UnnamedDef")
			{
				string text = "Unnamed" + typeof(T).Name + Rand.Range(1, 100000).ToString() + "A";
				Log.Error(string.Concat(new string[]
				{
					typeof(T).Name,
					" in ",
					sourceName,
					" with label ",
					def.label,
					" lacks a defName. Giving name ",
					text
				}));
				def.defName = text;
			}
			T def2;
			if (DefDatabase<T>.defsByName.TryGetValue(def.defName, out def2))
			{
				DefDatabase<T>.Remove(def2);
			}
			DefDatabase<T>.Add(def);
		}

		// Token: 0x04000291 RID: 657
		private static List<T> defsList = new List<T>();

		// Token: 0x04000292 RID: 658
		private static Dictionary<string, T> defsByName = new Dictionary<string, T>();

		// Token: 0x04000293 RID: 659
		private static Dictionary<ushort, T> defsByShortHash = new Dictionary<ushort, T>();
	}
}

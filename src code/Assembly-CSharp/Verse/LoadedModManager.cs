using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000274 RID: 628
	public static class LoadedModManager
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x0006853E File Offset: 0x0006673E
		public static List<ModContentPack> RunningModsListForReading
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x0006853E File Offset: 0x0006673E
		public static IEnumerable<ModContentPack> RunningMods
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x00068545 File Offset: 0x00066745
		public static List<Def> PatchedDefsForReading
		{
			get
			{
				return LoadedModManager.patchedDefs;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060011EB RID: 4587 RVA: 0x0006854C File Offset: 0x0006674C
		public static IEnumerable<Mod> ModHandles
		{
			get
			{
				return LoadedModManager.runningModClasses.Values;
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00068558 File Offset: 0x00066758
		public static void LoadAllActiveMods()
		{
			DeepProfiler.Start("XmlInheritance.Clear()");
			try
			{
				XmlInheritance.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("InitializeMods()");
			try
			{
				LoadedModManager.InitializeMods();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("LoadModContent()");
			try
			{
				LoadedModManager.LoadModContent();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("CreateModClasses()");
			try
			{
				LoadedModManager.CreateModClasses();
			}
			finally
			{
				DeepProfiler.End();
			}
			List<LoadableXmlAsset> xmls = null;
			DeepProfiler.Start("LoadModXML()");
			try
			{
				xmls = LoadedModManager.LoadModXML();
			}
			finally
			{
				DeepProfiler.End();
			}
			Dictionary<XmlNode, LoadableXmlAsset> assetlookup = new Dictionary<XmlNode, LoadableXmlAsset>();
			XmlDocument xmlDocument = null;
			DeepProfiler.Start("CombineIntoUnifiedXML()");
			try
			{
				xmlDocument = LoadedModManager.CombineIntoUnifiedXML(xmls, assetlookup);
			}
			finally
			{
				DeepProfiler.End();
			}
			TKeySystem.Clear();
			DeepProfiler.Start("TKeySystem.Parse()");
			try
			{
				TKeySystem.Parse(xmlDocument);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ErrorCheckPatches()");
			try
			{
				LoadedModManager.ErrorCheckPatches();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ApplyPatches()");
			try
			{
				LoadedModManager.ApplyPatches(xmlDocument, assetlookup);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ParseAndProcessXML()");
			try
			{
				LoadedModManager.ParseAndProcessXML(xmlDocument, assetlookup);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ClearCachedPatches()");
			try
			{
				LoadedModManager.ClearCachedPatches();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("XmlInheritance.Clear()");
			try
			{
				XmlInheritance.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00068728 File Offset: 0x00066928
		public static void InitializeMods()
		{
			int num = 0;
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>())
			{
				DeepProfiler.Start("Initializing " + modMetaData);
				try
				{
					if (!modMetaData.RootDir.Exists)
					{
						ModsConfig.SetActive(modMetaData.PackageId, false);
						Log.Warning(string.Concat(new object[]
						{
							"Failed to find active mod ",
							modMetaData.Name,
							"(",
							modMetaData.PackageIdPlayerFacing,
							") at ",
							modMetaData.RootDir
						}));
					}
					else
					{
						ModContentPack item = new ModContentPack(modMetaData.RootDir, modMetaData.PackageId, modMetaData.PackageIdPlayerFacing, num, modMetaData.Name, modMetaData.Official);
						num++;
						LoadedModManager.runningMods.Add(item);
						GenTypes.ClearCache();
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error initializing mod: " + arg);
					ModsConfig.SetActive(modMetaData.PackageId, false);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00068868 File Offset: 0x00066A68
		public static void LoadModContent()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DeepProfiler.Start("LoadModContent");
			});
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				DeepProfiler.Start("Loading " + modContentPack + " content");
				try
				{
					modContentPack.ReloadContent();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not reload mod content for mod ",
						modContentPack.PackageIdPlayerFacing,
						": ",
						ex
					}));
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DeepProfiler.End();
				for (int j = 0; j < LoadedModManager.runningMods.Count; j++)
				{
					ModContentPack modContentPack2 = LoadedModManager.runningMods[j];
					if (!modContentPack2.AnyContentLoaded())
					{
						Log.Error("Mod " + modContentPack2.Name + " did not load any content. Following load folders were used:\n" + modContentPack2.foldersToLoadDescendingOrder.ToLineList("  - "));
					}
				}
			});
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00068950 File Offset: 0x00066B50
		public static void CreateModClasses()
		{
			using (IEnumerator<Type> enumerator = typeof(Mod).InstantiableDescendantsAndSelf().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					DeepProfiler.Start("Loading " + type + " mod class");
					try
					{
						if (!LoadedModManager.runningModClasses.ContainsKey(type))
						{
							ModContentPack modContentPack = (from modpack in LoadedModManager.runningMods
							where modpack.assemblies.loadedAssemblies.Contains(type.Assembly)
							select modpack).FirstOrDefault<ModContentPack>();
							LoadedModManager.runningModClasses[type] = (Mod)Activator.CreateInstance(type, new object[]
							{
								modContentPack
							});
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error while instantiating a mod of type ",
							type,
							": ",
							ex
						}));
					}
					finally
					{
						DeepProfiler.End();
					}
				}
			}
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00068A74 File Offset: 0x00066C74
		public static List<LoadableXmlAsset> LoadModXML()
		{
			List<LoadableXmlAsset> list = new List<LoadableXmlAsset>();
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				DeepProfiler.Start("Loading " + modContentPack);
				try
				{
					list.AddRange(modContentPack.LoadDefs());
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not load defs for mod ",
						modContentPack.PackageIdPlayerFacing,
						": ",
						ex
					}));
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			return list;
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00068B1C File Offset: 0x00066D1C
		public static void ErrorCheckPatches()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				foreach (PatchOperation patchOperation in modContentPack.Patches)
				{
					try
					{
						foreach (string text in patchOperation.ConfigErrors())
						{
							Log.Error(string.Concat(new object[]
							{
								"Config error in ",
								modContentPack.Name,
								" patch ",
								patchOperation,
								": ",
								text
							}));
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception in ConfigErrors() of ",
							modContentPack.Name,
							" patch ",
							patchOperation,
							": ",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00068C6C File Offset: 0x00066E6C
		public static void ApplyPatches(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			foreach (PatchOperation patchOperation in LoadedModManager.runningMods.SelectMany((ModContentPack rm) => rm.Patches))
			{
				try
				{
					patchOperation.Apply(xmlDoc);
				}
				catch (Exception arg)
				{
					Log.Error("Error in patch.Apply(): " + arg);
				}
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00068D00 File Offset: 0x00066F00
		public static XmlDocument CombineIntoUnifiedXML(List<LoadableXmlAsset> xmls, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateElement("Defs"));
			foreach (LoadableXmlAsset loadableXmlAsset in xmls)
			{
				if (loadableXmlAsset.xmlDoc == null || loadableXmlAsset.xmlDoc.DocumentElement == null)
				{
					Log.Error(string.Format("{0}: unknown parse failure", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name));
				}
				else
				{
					if (loadableXmlAsset.xmlDoc.DocumentElement.Name != "Defs")
					{
						Log.Error(string.Format("{0}: root element named {1}; should be named Defs", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name, loadableXmlAsset.xmlDoc.DocumentElement.Name));
					}
					foreach (object obj in loadableXmlAsset.xmlDoc.DocumentElement.ChildNodes)
					{
						XmlNode node = (XmlNode)obj;
						XmlNode xmlNode = xmlDocument.ImportNode(node, true);
						assetlookup[xmlNode] = loadableXmlAsset;
						xmlDocument.DocumentElement.AppendChild(xmlNode);
					}
				}
			}
			return xmlDocument;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00068E80 File Offset: 0x00067080
		public static void ParseAndProcessXML(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
			List<XmlNode> list = new List<XmlNode>();
			foreach (object obj in childNodes)
			{
				list.Add(obj as XmlNode);
			}
			DeepProfiler.Start("Loading asset nodes " + list.Count);
			try
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].NodeType == XmlNodeType.Element)
					{
						LoadableXmlAsset loadableXmlAsset = null;
						DeepProfiler.Start("assetlookup.TryGetValue");
						try
						{
							assetlookup.TryGetValue(list[i], out loadableXmlAsset);
						}
						finally
						{
							DeepProfiler.End();
						}
						DeepProfiler.Start("XmlInheritance.TryRegister");
						try
						{
							XmlInheritance.TryRegister(list[i], (loadableXmlAsset != null) ? loadableXmlAsset.mod : null);
						}
						finally
						{
							DeepProfiler.End();
						}
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("XmlInheritance.Resolve()");
			try
			{
				XmlInheritance.Resolve();
			}
			finally
			{
				DeepProfiler.End();
			}
			LoadedModManager.runningMods.FirstOrDefault<ModContentPack>();
			DeepProfiler.Start("Loading defs for " + list.Count + " nodes");
			try
			{
				foreach (XmlNode xmlNode in list)
				{
					XmlAttributeCollection attributes = xmlNode.Attributes;
					string text;
					if (attributes == null)
					{
						text = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["MayRequire"];
						text = ((xmlAttribute != null) ? xmlAttribute.Value.ToLower() : null);
					}
					string text2 = text;
					if (text2 == null || ModsConfig.AreAllActive(text2))
					{
						XmlAttributeCollection attributes2 = xmlNode.Attributes;
						string[] array;
						if (attributes2 == null)
						{
							array = null;
						}
						else
						{
							XmlAttribute xmlAttribute2 = attributes2["MayRequireAnyOf"];
							array = ((xmlAttribute2 != null) ? xmlAttribute2.Value.ToLower().Split(new char[]
							{
								','
							}) : null);
						}
						string[] array2 = array;
						if (array2.NullOrEmpty<string>() || ModsConfig.IsAnyActiveOrEmpty(array2, false))
						{
							LoadableXmlAsset loadableXmlAsset2 = assetlookup.TryGetValue(xmlNode, null);
							Def def = DirectXmlLoader.DefFromNode(xmlNode, loadableXmlAsset2);
							if (def != null)
							{
								ModContentPack modContentPack = (loadableXmlAsset2 != null) ? loadableXmlAsset2.mod : null;
								if (modContentPack != null)
								{
									modContentPack.AddDef(def, loadableXmlAsset2.name);
								}
								else
								{
									LoadedModManager.patchedDefs.Add(def);
								}
							}
						}
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x00069168 File Offset: 0x00067368
		public static void ClearCachedPatches()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				foreach (PatchOperation patchOperation in modContentPack.Patches)
				{
					try
					{
						patchOperation.Complete(modContentPack.Name);
					}
					catch (Exception arg)
					{
						Log.Error("Error in patch.Complete(): " + arg);
					}
				}
				modContentPack.ClearPatchesCache();
			}
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00069220 File Offset: 0x00067420
		public static void ClearDestroy()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				try
				{
					modContentPack.ClearDestroy();
				}
				catch (Exception arg)
				{
					Log.Error("Error in mod.ClearDestroy(): " + arg);
				}
			}
			LoadedModManager.runningMods.Clear();
			GenTypes.ClearCache();
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x000692A4 File Offset: 0x000674A4
		public static T GetMod<T>() where T : Mod
		{
			return LoadedModManager.GetMod(typeof(T)) as T;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x000692C0 File Offset: 0x000674C0
		public static Mod GetMod(Type type)
		{
			if (LoadedModManager.runningModClasses.ContainsKey(type))
			{
				return LoadedModManager.runningModClasses[type];
			}
			return (from kvp in LoadedModManager.runningModClasses
			where type.IsAssignableFrom(kvp.Key)
			select kvp).FirstOrDefault<KeyValuePair<Type, Mod>>().Value;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00069320 File Offset: 0x00067520
		private static string GetSettingsFilename(string modIdentifier, string modHandleName)
		{
			return Path.Combine(GenFilePaths.ConfigFolderPath, GenText.SanitizeFilename(string.Format("Mod_{0}_{1}.xml", modIdentifier, modHandleName)));
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00069340 File Offset: 0x00067540
		public static T ReadModSettings<T>(string modIdentifier, string modHandleName) where T : ModSettings, new()
		{
			string settingsFilename = LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName);
			T t = default(T);
			try
			{
				if (File.Exists(settingsFilename))
				{
					Scribe.loader.InitLoading(settingsFilename);
					try
					{
						Scribe_Deep.Look<T>(ref t, "ModSettings", Array.Empty<object>());
					}
					finally
					{
						Scribe.loader.FinalizeLoading();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Caught exception while loading mod settings data for {0}. Generating fresh settings. The exception was: {1}", modIdentifier, ex.ToString()));
				t = default(T);
			}
			if (t == null)
			{
				t = Activator.CreateInstance<T>();
			}
			return t;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x000693DC File Offset: 0x000675DC
		public static void WriteModSettings(string modIdentifier, string modHandleName, ModSettings settings)
		{
			Scribe.saver.InitSaving(LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName), "SettingsBlock");
			try
			{
				Scribe_Deep.Look<ModSettings>(ref settings, "ModSettings", Array.Empty<object>());
			}
			finally
			{
				Scribe.saver.FinalizeSaving();
			}
		}

		// Token: 0x04000F2A RID: 3882
		private static List<ModContentPack> runningMods = new List<ModContentPack>();

		// Token: 0x04000F2B RID: 3883
		private static Dictionary<Type, Mod> runningModClasses = new Dictionary<Type, Mod>();

		// Token: 0x04000F2C RID: 3884
		private static List<Def> patchedDefs = new List<Def>();
	}
}

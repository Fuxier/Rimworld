using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using RimWorld;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200017F RID: 383
	public static class BackstoryTranslationUtility
	{
		// Token: 0x06000A89 RID: 2697 RVA: 0x00036D16 File Offset: 0x00034F16
		public static IEnumerable<XElement> BackstoryTranslationElements(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folders, List<string> loadErrors = null)
		{
			Dictionary<ModContentPack, HashSet<string>> alreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();
			foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in folders)
			{
				if (!alreadyLoadedFiles.ContainsKey(tuple.Item2))
				{
					alreadyLoadedFiles[tuple.Item2] = new HashSet<string>();
				}
				VirtualFile file = tuple.Item1.GetFile("Backstories/Backstories.xml");
				if (file.Exists)
				{
					if (!file.FullPath.StartsWith(tuple.Item3))
					{
						Log.Error("Failed to get a relative path for a file: " + file.FullPath + ", located in " + tuple.Item3);
					}
					else
					{
						string item = file.FullPath.Substring(tuple.Item3.Length);
						if (!alreadyLoadedFiles[tuple.Item2].Contains(item))
						{
							alreadyLoadedFiles[tuple.Item2].Add(item);
							XDocument xdocument;
							try
							{
								xdocument = file.LoadAsXDocument();
							}
							catch (Exception ex)
							{
								if (loadErrors != null)
								{
									loadErrors.Add(string.Concat(new object[]
									{
										"Exception loading backstory translation data from file ",
										file,
										": ",
										ex
									}));
								}
								yield break;
							}
							foreach (XElement xelement in xdocument.Root.Elements())
							{
								yield return xelement;
							}
							IEnumerator<XElement> enumerator2 = null;
						}
					}
				}
			}
			IEnumerator<Tuple<VirtualDirectory, ModContentPack, string>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00036D30 File Offset: 0x00034F30
		public static bool AnyLegacyBackstoryFiles(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folders)
		{
			using (IEnumerator<Tuple<VirtualDirectory, ModContentPack, string>> enumerator = folders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Item1.GetFile("Backstories/Backstories.xml").Exists)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00036D90 File Offset: 0x00034F90
		public static void LoadAndInjectBackstoryData(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folderPaths, List<string> loadErrors = null)
		{
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(folderPaths, loadErrors))
			{
				string text = "[unknown]";
				try
				{
					text = xelement.Name.ToString();
					string text2 = BackstoryTranslationUtility.GetText(xelement, "title");
					string text3 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
					string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
					string text5 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
					string text6 = BackstoryTranslationUtility.GetText(xelement, "desc");
					BackstoryDef backstoryDef;
					if (!BackstoryTranslationUtility.TryGetWithIdentifier(text, out backstoryDef, false))
					{
						throw new Exception("Backstory not found matching identifier " + text);
					}
					if (text2 == backstoryDef.title && text3 == backstoryDef.titleFemale && text4 == backstoryDef.titleShort && text5 == backstoryDef.titleShortFemale && text6 == backstoryDef.baseDesc)
					{
						throw new Exception("Backstory translation exactly matches default data: " + text);
					}
					if (text2 != null)
					{
						backstoryDef.SetTitle(text2, backstoryDef.titleFemale);
					}
					if (text3 != null)
					{
						backstoryDef.SetTitle(backstoryDef.title, text3);
					}
					if (text4 != null)
					{
						backstoryDef.SetTitleShort(text4, backstoryDef.titleShortFemale);
					}
					if (text5 != null)
					{
						backstoryDef.SetTitleShort(backstoryDef.titleShort, text5);
					}
					if (text6 != null)
					{
						backstoryDef.baseDesc = text6;
					}
				}
				catch (Exception ex)
				{
					if (loadErrors != null)
					{
						loadErrors.Add(string.Concat(new string[]
						{
							"Couldn't load backstory ",
							text,
							": ",
							ex.Message,
							"\nFull XML text:\n\n",
							xelement.ToString()
						}));
					}
				}
			}
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00036F74 File Offset: 0x00035174
		public static List<DefInjectionPackage.DefInjection> GetLegacyBackstoryTranslations(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folderPaths)
		{
			List<DefInjectionPackage.DefInjection> list = new List<DefInjectionPackage.DefInjection>();
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(folderPaths, null))
			{
				try
				{
					string text = xelement.Name.ToString();
					BackstoryDef backstoryDef;
					if (BackstoryTranslationUtility.TryGetWithIdentifier(text, out backstoryDef, true))
					{
						string text2 = BackstoryTranslationUtility.GetText(xelement, "title");
						string text3 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
						string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
						string text5 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
						string text6 = BackstoryTranslationUtility.GetText(xelement, "desc");
						if (!text2.NullOrEmpty() && text2 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection = new DefInjectionPackage.DefInjection();
							defInjection.path = text + ".title";
							defInjection.suggestedPath = defInjection.path;
							defInjection.replacedString = backstoryDef.untranslatedTitle;
							defInjection.injection = text2;
							defInjection.injected = true;
							list.Add(defInjection);
						}
						if (!text3.NullOrEmpty() && text3 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection2 = new DefInjectionPackage.DefInjection();
							defInjection2.path = text + ".titleFemale";
							defInjection2.suggestedPath = defInjection2.path;
							defInjection2.replacedString = backstoryDef.untranslatedTitleFemale;
							defInjection2.injection = text3;
							defInjection2.injected = true;
							list.Add(defInjection2);
						}
						if (!text4.NullOrEmpty() && text4 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection3 = new DefInjectionPackage.DefInjection();
							defInjection3.path = text + ".titleShort";
							defInjection3.suggestedPath = defInjection3.path;
							defInjection3.replacedString = backstoryDef.untranslatedTitleShort;
							defInjection3.injection = text4;
							defInjection3.injected = true;
							list.Add(defInjection3);
						}
						if (!text5.NullOrEmpty() && text5 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection4 = new DefInjectionPackage.DefInjection();
							defInjection4.path = text + ".titleShortFemale";
							defInjection4.suggestedPath = defInjection4.path;
							defInjection4.replacedString = backstoryDef.untranslatedTitleShortFemale;
							defInjection4.injection = text5;
							defInjection4.injected = true;
							list.Add(defInjection4);
						}
						if (!text6.NullOrEmpty() && text6 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection5 = new DefInjectionPackage.DefInjection();
							defInjection5.path = text + ".baseDesc";
							defInjection5.suggestedPath = defInjection5.path;
							defInjection5.replacedString = backstoryDef.untranslatedDesc;
							defInjection5.injection = text6;
							defInjection5.injected = true;
							list.Add(defInjection5);
						}
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error Getting legacy backstory translations: " + arg);
				}
			}
			return list;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00037260 File Offset: 0x00035460
		public static string StripNumericSuffix(string key)
		{
			return BackstoryTranslationUtility.regex.Match(key).Captures[0].Value;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00037280 File Offset: 0x00035480
		private static bool TryGetWithIdentifier(string identifier, out BackstoryDef backstory, bool closestMatchWarning = true)
		{
			backstory = DefDatabase<BackstoryDef>.AllDefs.FirstOrDefault((BackstoryDef b) => b.defName == identifier);
			if (backstory == null)
			{
				string strippedDefName = BackstoryTranslationUtility.StripNumericSuffix(identifier);
				backstory = BackstoryTranslationUtility.tmpAllBackstories.FirstOrDefault((BackstoryDef b) => BackstoryTranslationUtility.StripNumericSuffix(b.defName) == strippedDefName);
				if (backstory != null && closestMatchWarning)
				{
					Log.Warning("Couldn't find exact match for backstory " + identifier + ", using closest match " + backstory.identifier);
				}
			}
			return backstory != null;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00037314 File Offset: 0x00035514
		public static List<string> MissingBackstoryTranslations(LoadedLanguage lang)
		{
			List<string> list = new List<string>();
			BackstoryTranslationUtility.tmpAllBackstories.Clear();
			BackstoryTranslationUtility.tmpAllBackstories.AddRange(DefDatabase<BackstoryDef>.AllDefs);
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(lang.AllDirectories, null))
			{
				try
				{
					string text = xelement.Name.ToString();
					BackstoryDef backstoryDef;
					BackstoryTranslationUtility.TryGetWithIdentifier(text, out backstoryDef, true);
					if (backstoryDef == null)
					{
						list.Add("Translation doesn't correspond to any backstory: " + text);
					}
					else
					{
						BackstoryTranslationUtility.tmpAllBackstories.Remove(backstoryDef);
						string text2 = BackstoryTranslationUtility.GetText(xelement, "title");
						string text3 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
						string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
						string text5 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
						string text6 = BackstoryTranslationUtility.GetText(xelement, "desc");
						if (text2.NullOrEmpty())
						{
							list.Add(text + ".title missing");
						}
						if (!backstoryDef.titleFemale.NullOrEmpty() && text3.NullOrEmpty())
						{
							list.Add(text + ".titleFemale missing");
						}
						if (text4.NullOrEmpty())
						{
							list.Add(text + ".titleShort missing");
						}
						if (!backstoryDef.titleShortFemale.NullOrEmpty() && text5.NullOrEmpty())
						{
							list.Add(text + ".titleShortFemale missing");
						}
						if (text6.NullOrEmpty())
						{
							list.Add(text + ".desc missing");
						}
					}
				}
				catch (Exception ex)
				{
					list.Add(string.Concat(new object[]
					{
						"Exception reading ",
						xelement.Name,
						": ",
						ex.Message
					}));
				}
			}
			foreach (BackstoryDef backstoryDef2 in BackstoryTranslationUtility.tmpAllBackstories)
			{
				list.Add("Missing backstory: " + backstoryDef2.defName);
			}
			BackstoryTranslationUtility.tmpAllBackstories.Clear();
			return list;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00037568 File Offset: 0x00035768
		public static List<string> BackstoryTranslationsMatchingEnglish(LoadedLanguage lang)
		{
			List<string> list = new List<string>();
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(lang.AllDirectories, null))
			{
				try
				{
					string identifier = xelement.Name.ToString();
					BackstoryDef backstoryDef;
					if ((from b in DefDatabase<BackstoryDef>.AllDefs
					where b.defName == identifier
					select b).TryRandomElement(out backstoryDef))
					{
						string text = BackstoryTranslationUtility.GetText(xelement, "title");
						string text2 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
						string text3 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
						string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
						string text5 = BackstoryTranslationUtility.GetText(xelement, "desc");
						if (!text.NullOrEmpty() && text == backstoryDef.untranslatedTitle)
						{
							list.Add(identifier + ".title '" + text.Replace("\n", "\\n") + "'");
						}
						if (!text2.NullOrEmpty() && text2 == backstoryDef.untranslatedTitleFemale)
						{
							list.Add(identifier + ".titleFemale '" + text2.Replace("\n", "\\n") + "'");
						}
						if (!text3.NullOrEmpty() && text3 == backstoryDef.untranslatedTitleShort)
						{
							list.Add(identifier + ".titleShort '" + text3.Replace("\n", "\\n") + "'");
						}
						if (!text4.NullOrEmpty() && text4 == backstoryDef.untranslatedTitleShortFemale)
						{
							list.Add(identifier + ".titleShortFemale '" + text4.Replace("\n", "\\n") + "'");
						}
						if (!text5.NullOrEmpty() && text5 == backstoryDef.untranslatedDesc)
						{
							list.Add(identifier + ".desc '" + text5.Replace("\n", "\\n") + "'");
						}
					}
				}
				catch (Exception ex)
				{
					list.Add(string.Concat(new object[]
					{
						"Exception reading ",
						xelement.Name,
						": ",
						ex.Message
					}));
				}
			}
			return list;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x000377F4 File Offset: 0x000359F4
		public static List<string> ObsoleteBackstoryTranslations(LoadedLanguage lang)
		{
			List<string> list = new List<string>();
			using (IEnumerator<XElement> enumerator = BackstoryTranslationUtility.BackstoryTranslationElements(lang.AllDirectories, null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BackstoryDef backstoryDef;
					if (BackstoryTranslationUtility.TryGetWithIdentifier(enumerator.Current.Name.ToString(), out backstoryDef, true))
					{
						list.Add("Obsolete backstory format: " + backstoryDef.defName);
					}
				}
			}
			return list;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00037870 File Offset: 0x00035A70
		private static string GetText(XElement backstory, string fieldName)
		{
			XElement xelement = backstory.Element(fieldName);
			if (xelement == null || xelement.Value == "TODO")
			{
				return null;
			}
			return xelement.Value.Replace("\\n", "\n");
		}

		// Token: 0x04000A63 RID: 2659
		public const string BackstoriesFolder = "Backstories";

		// Token: 0x04000A64 RID: 2660
		public const string BackstoriesFileName = "Backstories.xml";

		// Token: 0x04000A65 RID: 2661
		public const string BackstoriesFolderLegacy = "Backstories DELETE_ME";

		// Token: 0x04000A66 RID: 2662
		private static Regex regex = new Regex("^[^0-9]*");

		// Token: 0x04000A67 RID: 2663
		private static List<BackstoryDef> tmpAllBackstories = new List<BackstoryDef>();
	}
}

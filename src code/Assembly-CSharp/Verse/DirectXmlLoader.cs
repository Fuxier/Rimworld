using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000399 RID: 921
	public static class DirectXmlLoader
	{
		// Token: 0x06001A68 RID: 6760 RVA: 0x0009F4B8 File Offset: 0x0009D6B8
		public static LoadableXmlAsset[] XmlAssetsInModFolder(ModContentPack mod, string folderPath, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
			for (int k = 0; k < list.Count; k++)
			{
				string text = list[k];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, folderPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
					{
						string key = fileInfo.FullName.Substring(text.Length + 1);
						if (!dictionary.ContainsKey(key))
						{
							dictionary.Add(key, fileInfo);
						}
					}
				}
			}
			if (dictionary.Count == 0)
			{
				return DirectXmlLoader.emptyXmlAssetsArray;
			}
			List<FileInfo> fileList = dictionary.Values.ToList<FileInfo>();
			LoadableXmlAsset[] assets = new LoadableXmlAsset[fileList.Count];
			GenThreading.ParallelFor(0, fileList.Count, delegate(int i)
			{
				FileInfo fileInfo2 = fileList[i];
				LoadableXmlAsset loadableXmlAsset = new LoadableXmlAsset(fileInfo2.Name, fileInfo2.Directory.FullName, File.ReadAllText(fileInfo2.FullName));
				loadableXmlAsset.mod = mod;
				assets[i] = loadableXmlAsset;
			}, -1);
			return assets;
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0009F5CB File Offset: 0x0009D7CB
		public static IEnumerable<T> LoadXmlDataInResourcesFolder<T>(string folderPath) where T : new()
		{
			XmlInheritance.Clear();
			DeepProfiler.Start("Resources.LoadAll<TextAsset>");
			TextAsset[] source = Resources.LoadAll<TextAsset>(folderPath);
			DeepProfiler.End();
			DeepProfiler.Start("Load XML");
			List<LoadableXmlAsset> assets = (from x in (from x in source
			select new
			{
				x.name,
				x.text
			}).ToList().AsParallel()
			select new LoadableXmlAsset(x.name, "", x.text)).ToList<LoadableXmlAsset>();
			DeepProfiler.End();
			DeepProfiler.Start("Resolve inheritance");
			foreach (LoadableXmlAsset xmlAsset in assets)
			{
				XmlInheritance.TryRegisterAllFrom(xmlAsset, null);
			}
			XmlInheritance.Resolve();
			DeepProfiler.End();
			DeepProfiler.Start("Read game items from XML");
			int num;
			for (int i = 0; i < assets.Count; i = num + 1)
			{
				foreach (T t in DirectXmlLoader.AllGameItemsFromAsset<T>(assets[i]))
				{
					yield return t;
				}
				IEnumerator<T> enumerator2 = null;
				num = i;
			}
			DeepProfiler.End();
			XmlInheritance.Clear();
			yield break;
			yield break;
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0009F5DB File Offset: 0x0009D7DB
		public static T ItemFromXmlFile<T>(string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (!new FileInfo(filePath).Exists)
			{
				return Activator.CreateInstance<T>();
			}
			return DirectXmlLoader.ItemFromXmlString<T>(File.ReadAllText(filePath), filePath, resolveCrossRefs);
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0009F5FD File Offset: 0x0009D7FD
		public static T ItemFromXmlFile<T>(VirtualDirectory directory, string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (!directory.FileExists(filePath))
			{
				return Activator.CreateInstance<T>();
			}
			return DirectXmlLoader.ItemFromXmlString<T>(directory.ReadAllText(filePath), directory.FullPath + "/" + filePath, resolveCrossRefs);
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0009F62C File Offset: 0x0009D82C
		public static T ItemFromXmlString<T>(string xmlContent, string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (resolveCrossRefs && DirectXmlCrossRefLoader.LoadingInProgress)
			{
				Log.Error("Cannot call ItemFromXmlString with resolveCrossRefs=true while loading is already in progress (forgot to resolve or clear cross refs from previous loading?).");
			}
			T result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xmlContent);
				T t = DirectXmlToObject.ObjectFromXml<T>(xmlDocument.DocumentElement, false);
				if (resolveCrossRefs)
				{
					try
					{
						DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
					}
					finally
					{
						DirectXmlCrossRefLoader.Clear();
					}
				}
				result = t;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading file at " + filePath + ". Loading defaults instead. Exception was: " + ex.ToString());
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0009F6BC File Offset: 0x0009D8BC
		public static Def DefFromNode(XmlNode node, LoadableXmlAsset loadingAsset)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				return null;
			}
			XmlAttribute xmlAttribute = node.Attributes["Abstract"];
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(node.Name, null);
			if (typeInAnyAssembly == null)
			{
				return null;
			}
			if (!GenTypes.IsDef(typeInAnyAssembly))
			{
				return null;
			}
			Func<XmlNode, bool, object> objectFromXmlMethod = DirectXmlToObject.GetObjectFromXmlMethod(typeInAnyAssembly);
			Def result = null;
			try
			{
				result = (Def)objectFromXmlMethod(node, true);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception loading def from file ",
					(loadingAsset != null) ? loadingAsset.name : "(unknown)",
					": ",
					ex
				}));
			}
			return result;
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x0009F788 File Offset: 0x0009D988
		public static IEnumerable<T> AllGameItemsFromAsset<T>(LoadableXmlAsset asset) where T : new()
		{
			if (asset.xmlDoc == null)
			{
				yield break;
			}
			XmlNodeList xmlNodeList = asset.xmlDoc.DocumentElement.SelectNodes(typeof(T).Name);
			bool gotData = false;
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlAttribute xmlAttribute = xmlNode.Attributes["Abstract"];
				if (xmlAttribute == null || !xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
				{
					T t;
					try
					{
						t = DirectXmlToObject.ObjectFromXml<T>(xmlNode, true);
						gotData = true;
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception loading data from file ",
							asset.name,
							": ",
							ex
						}));
						continue;
					}
					yield return t;
				}
			}
			IEnumerator enumerator = null;
			if (!gotData)
			{
				Log.Error(string.Concat(new object[]
				{
					"Found no usable data when trying to get ",
					typeof(T),
					"s from file ",
					asset.name
				}));
			}
			yield break;
			yield break;
		}

		// Token: 0x04001342 RID: 4930
		private static LoadableXmlAsset[] emptyXmlAssetsArray = new LoadableXmlAsset[0];
	}
}

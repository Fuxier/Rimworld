using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000280 RID: 640
	public class ModLoadFolders
	{
		// Token: 0x06001299 RID: 4761 RVA: 0x0006BE18 File Offset: 0x0006A018
		public List<LoadFolder> FoldersForVersion(string version)
		{
			if (this.foldersForVersion.ContainsKey(version))
			{
				return this.foldersForVersion[version];
			}
			return null;
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0006BE36 File Offset: 0x0006A036
		public List<string> DefinedVersions()
		{
			return this.foldersForVersion.Keys.ToList<string>();
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0006BE48 File Offset: 0x0006A048
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
					if (!this.foldersForVersion.ContainsKey(text))
					{
						this.foldersForVersion.Add(text, new List<LoadFolder>());
					}
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (!(xmlNode2 is XmlComment))
						{
							XmlAttributeCollection attributes = xmlNode2.Attributes;
							XmlAttribute xmlAttribute = (attributes != null) ? attributes["IfModActive"] : null;
							List<string> requiredPackageIds = null;
							if (xmlAttribute != null)
							{
								requiredPackageIds = (from s in xmlAttribute.Value.Split(new char[]
								{
									','
								})
								select s.Trim()).ToList<string>();
							}
							XmlAttributeCollection attributes2 = xmlNode2.Attributes;
							XmlAttribute xmlAttribute2 = (attributes2 != null) ? attributes2["IfModNotActive"] : null;
							List<string> disallowedPackageIds = null;
							if (xmlAttribute2 != null)
							{
								disallowedPackageIds = (from s in xmlAttribute2.Value.Split(new char[]
								{
									','
								})
								select s.Trim()).ToList<string>();
							}
							if (xmlNode2.InnerText == "/" || xmlNode2.InnerText == "\\")
							{
								this.foldersForVersion[text].Add(new LoadFolder("", requiredPackageIds, disallowedPackageIds));
							}
							else
							{
								string text2 = xmlNode2.InnerText;
								char directorySeparatorChar = Path.DirectorySeparatorChar;
								char altDirectorySeparatorChar = Path.AltDirectorySeparatorChar;
								if (altDirectorySeparatorChar != directorySeparatorChar)
								{
									text2 = text2.Replace(altDirectorySeparatorChar, directorySeparatorChar);
								}
								this.foldersForVersion[text].Add(new LoadFolder(text2, requiredPackageIds, disallowedPackageIds));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0006C0B0 File Offset: 0x0006A2B0
		public List<string> GetIssueList(ModMetaData mod)
		{
			List<string> list = new List<string>();
			if (this.foldersForVersion.Count > 0)
			{
				string text = null;
				foreach (string text2 in this.foldersForVersion.Keys)
				{
					if (this.foldersForVersion[text2].Count == 0)
					{
						list.Add("ModLoadFolderListEmpty".Translate(text2));
					}
					foreach (LoadFolder loadFolder in from f in this.foldersForVersion[text2]
					group f by f into g
					where g.Count<LoadFolder>() > 1
					select g.Key)
					{
						list.Add("ModLoadFolderRepeatingFolder".Translate(text2, loadFolder.folderName));
					}
					if (!VersionControl.IsWellFormattedVersionString(text2) && !text2.Equals("default", StringComparison.InvariantCultureIgnoreCase))
					{
						list.Add("ModLoadFolderMalformedVersion".Translate(text2));
					}
					if (text2.Equals("default"))
					{
						list.Add("ModLoadFolderDefaultDeprecated".Translate());
					}
					Version v;
					Version v2;
					if (text != null && VersionControl.TryParseVersionString(text2, out v) && VersionControl.TryParseVersionString(text, out v2) && v < v2)
					{
						list.Add("ModLoadFolderOutOfOrder".Translate(text2, text));
					}
					for (int i = 0; i < this.foldersForVersion[text2].Count; i++)
					{
						LoadFolder loadFolder2 = this.foldersForVersion[text2][i];
						if (!Directory.Exists(Path.Combine(mod.RootDir.FullName, loadFolder2.folderName)))
						{
							list.Add("ModLoadFolderDoesntExist".Translate(loadFolder2.folderName, text2));
						}
					}
					Version item;
					if (VersionControl.TryParseVersionString(text2, out item) && !mod.SupportedVersionsReadOnly.Contains(item))
					{
						list.Add("ModLoadFolderDefinesUnsupportedGameVersion".Translate(text2));
					}
					text = text2;
				}
			}
			return list;
		}

		// Token: 0x04000F79 RID: 3961
		private Dictionary<string, List<LoadFolder>> foldersForVersion = new Dictionary<string, List<LoadFolder>>();

		// Token: 0x04000F7A RID: 3962
		public const string defaultVersionName = "default";
	}
}

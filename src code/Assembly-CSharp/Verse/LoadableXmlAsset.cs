using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x020003A1 RID: 929
	public class LoadableXmlAsset
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x000A1158 File Offset: 0x0009F358
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar.ToString() + this.name;
			}
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x000A1184 File Offset: 0x0009F384
		public LoadableXmlAsset(string name, string fullFolderPath, string contents)
		{
			this.name = name;
			this.fullFolderPath = fullFolderPath;
			try
			{
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.IgnoreComments = true;
				xmlReaderSettings.IgnoreWhitespace = true;
				xmlReaderSettings.CheckCharacters = false;
				using (StringReader stringReader = new StringReader(contents))
				{
					using (XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
					{
						this.xmlDoc = new XmlDocument();
						this.xmlDoc.Load(xmlReader);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Exception reading ",
					name,
					" as XML: ",
					ex
				}));
				this.xmlDoc = null;
			}
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x000A125C File Offset: 0x0009F45C
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04001359 RID: 4953
		private static XmlReader reader;

		// Token: 0x0400135A RID: 4954
		public string name;

		// Token: 0x0400135B RID: 4955
		public string fullFolderPath;

		// Token: 0x0400135C RID: 4956
		public XmlDocument xmlDoc;

		// Token: 0x0400135D RID: 4957
		public ModContentPack mod;
	}
}

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200039A RID: 922
	public static class DirectXmlLoaderSimple
	{
		// Token: 0x06001A70 RID: 6768 RVA: 0x0009F7A5 File Offset: 0x0009D9A5
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(string fileContents)
		{
			return DirectXmlLoaderSimple.ValuesFromXmlFile(VirtualFileInfoExt.LoadAsXDocument(fileContents));
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0009F7B2 File Offset: 0x0009D9B2
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(VirtualFile file)
		{
			return DirectXmlLoaderSimple.ValuesFromXmlFile(file.LoadAsXDocument());
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0009F7BF File Offset: 0x0009D9BF
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(XDocument doc)
		{
			foreach (XElement xelement in doc.Root.Elements())
			{
				string key = xelement.Name.ToString();
				string text = xelement.Value;
				text = text.Replace("\\n", "\n");
				yield return new DirectXmlLoaderSimple.XmlKeyValuePair
				{
					key = key,
					value = text,
					lineNumber = ((IXmlLineInfo)xelement).LineNumber
				};
			}
			IEnumerator<XElement> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x02001E75 RID: 7797
		public struct XmlKeyValuePair
		{
			// Token: 0x04007801 RID: 30721
			public string key;

			// Token: 0x04007802 RID: 30722
			public string value;

			// Token: 0x04007803 RID: 30723
			public int lineNumber;
		}
	}
}

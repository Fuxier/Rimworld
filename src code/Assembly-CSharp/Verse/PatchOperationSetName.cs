using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x0200028E RID: 654
	public class PatchOperationSetName : PatchOperationPathed
	{
		// Token: 0x060012E7 RID: 4839 RVA: 0x0006E06C File Offset: 0x0006C26C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode xmlNode2 = xmlNode.OwnerDocument.CreateElement(this.name);
				xmlNode2.InnerXml = xmlNode.InnerXml;
				xmlNode.ParentNode.InsertBefore(xmlNode2, xmlNode);
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04000F99 RID: 3993
		protected string name;
	}
}

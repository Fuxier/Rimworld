using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200028A RID: 650
	public class PatchOperationAddModExtension : PatchOperationPathed
	{
		// Token: 0x060012DF RID: 4831 RVA: 0x0006DD14 File Offset: 0x0006BF14
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				XmlNode xmlNode2 = xmlNode["modExtensions"];
				if (xmlNode2 == null)
				{
					xmlNode2 = xmlNode.OwnerDocument.CreateElement("modExtensions");
					xmlNode.AppendChild(xmlNode2);
				}
				foreach (object obj2 in node.ChildNodes)
				{
					XmlNode node2 = (XmlNode)obj2;
					xmlNode2.AppendChild(xmlNode.OwnerDocument.ImportNode(node2, true));
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04000F95 RID: 3989
		private XmlContainer value;
	}
}

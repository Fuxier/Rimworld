using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x0200028D RID: 653
	public class PatchOperationReplace : PatchOperationPathed
	{
		// Token: 0x060012E5 RID: 4837 RVA: 0x0006DFA8 File Offset: 0x0006C1A8
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode parentNode = xmlNode.ParentNode;
				foreach (object obj in node.ChildNodes)
				{
					XmlNode node2 = (XmlNode)obj;
					parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node2, true), xmlNode);
				}
				parentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04000F98 RID: 3992
		private XmlContainer value;
	}
}

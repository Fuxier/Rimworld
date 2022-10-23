using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000289 RID: 649
	public class PatchOperationAdd : PatchOperationPathed
	{
		// Token: 0x060012DD RID: 4829 RVA: 0x0006DBEC File Offset: 0x0006BDEC
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				result = true;
				XmlNode xmlNode = obj as XmlNode;
				if (this.order == PatchOperationAdd.Order.Append)
				{
					using (IEnumerator enumerator2 = node.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode node2 = (XmlNode)obj2;
							xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(node2, true));
						}
						continue;
					}
				}
				if (this.order == PatchOperationAdd.Order.Prepend)
				{
					for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
					{
						xmlNode.PrependChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[i], true));
					}
				}
			}
			return result;
		}

		// Token: 0x04000F93 RID: 3987
		private XmlContainer value;

		// Token: 0x04000F94 RID: 3988
		private PatchOperationAdd.Order order;

		// Token: 0x02001DD9 RID: 7641
		private enum Order
		{
			// Token: 0x040075D9 RID: 30169
			Append,
			// Token: 0x040075DA RID: 30170
			Prepend
		}
	}
}

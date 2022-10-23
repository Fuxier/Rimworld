using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x0200028C RID: 652
	public class PatchOperationRemove : PatchOperationPathed
	{
		// Token: 0x060012E3 RID: 4835 RVA: 0x0006DF60 File Offset: 0x0006C160
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}
	}
}

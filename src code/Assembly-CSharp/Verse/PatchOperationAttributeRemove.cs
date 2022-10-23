using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000291 RID: 657
	public class PatchOperationAttributeRemove : PatchOperationAttribute
	{
		// Token: 0x060012ED RID: 4845 RVA: 0x0006E1A4 File Offset: 0x0006C3A4
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes.Remove(xmlNode.Attributes[this.attribute]);
					result = true;
				}
			}
			return result;
		}
	}
}

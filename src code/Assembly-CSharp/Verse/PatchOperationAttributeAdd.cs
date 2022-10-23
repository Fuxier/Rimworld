using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000290 RID: 656
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		// Token: 0x060012EB RID: 4843 RVA: 0x0006E0FC File Offset: 0x0006C2FC
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] == null)
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04000F9B RID: 3995
		protected string value;
	}
}

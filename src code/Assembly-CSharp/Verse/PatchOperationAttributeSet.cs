using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000292 RID: 658
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		// Token: 0x060012EF RID: 4847 RVA: 0x0006E234 File Offset: 0x0006C434
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes[this.attribute].Value = this.value;
				}
				else
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04000F9C RID: 3996
		protected string value;
	}
}

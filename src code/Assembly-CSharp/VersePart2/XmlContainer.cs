using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000286 RID: 646
	public class XmlContainer
	{
		// Token: 0x060012D4 RID: 4820 RVA: 0x0006DAE0 File Offset: 0x0006BCE0
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		// Token: 0x04000F8E RID: 3982
		public XmlNode node;
	}
}

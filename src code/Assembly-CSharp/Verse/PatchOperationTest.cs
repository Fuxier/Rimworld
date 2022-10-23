using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000294 RID: 660
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x060012F5 RID: 4853 RVA: 0x0006E3C6 File Offset: 0x0006C5C6
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}

using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000295 RID: 661
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x060012F7 RID: 4855 RVA: 0x0006E3D8 File Offset: 0x0006C5D8
		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (xml.SelectSingleNode(this.xpath) != null)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return this.match != null || this.nomatch != null;
		}

		// Token: 0x04000F9F RID: 3999
		private PatchOperation match;

		// Token: 0x04000FA0 RID: 4000
		private PatchOperation nomatch;
	}
}

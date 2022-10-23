using System;

namespace Verse
{
	// Token: 0x02000288 RID: 648
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x060012DB RID: 4827 RVA: 0x0006DBCA File Offset: 0x0006BDCA
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}

		// Token: 0x04000F92 RID: 3986
		protected string xpath;
	}
}

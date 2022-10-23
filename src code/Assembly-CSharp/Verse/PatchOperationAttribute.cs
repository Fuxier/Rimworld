using System;

namespace Verse
{
	// Token: 0x0200028F RID: 655
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x060012E9 RID: 4841 RVA: 0x0006E0E3 File Offset: 0x0006C2E3
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}

		// Token: 0x04000F9A RID: 3994
		protected string attribute;
	}
}

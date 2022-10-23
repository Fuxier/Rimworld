using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000293 RID: 659
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x060012F1 RID: 4849 RVA: 0x0006E2F0 File Offset: 0x0006C4F0
		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (PatchOperation patchOperation in this.operations)
			{
				if (!patchOperation.Apply(xml))
				{
					this.lastFailedOperation = patchOperation;
					return false;
				}
			}
			return true;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0006E354 File Offset: 0x0006C554
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0006E364 File Offset: 0x0006C564
		public override string ToString()
		{
			int num = (this.operations != null) ? this.operations.Count : 0;
			string text = string.Format("{0}(count={1}", base.ToString(), num);
			if (this.lastFailedOperation != null)
			{
				text = text + ", lastFailedOperation=" + this.lastFailedOperation;
			}
			return text + ")";
		}

		// Token: 0x04000F9D RID: 3997
		private List<PatchOperation> operations;

		// Token: 0x04000F9E RID: 3998
		private PatchOperation lastFailedOperation;
	}
}

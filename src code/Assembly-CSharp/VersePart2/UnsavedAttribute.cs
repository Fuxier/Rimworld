using System;

namespace Verse
{
	// Token: 0x0200046F RID: 1135
	[AttributeUsage(AttributeTargets.Field)]
	public class UnsavedAttribute : Attribute
	{
		// Token: 0x060022AC RID: 8876 RVA: 0x000DDC3D File Offset: 0x000DBE3D
		public UnsavedAttribute(bool allowLoading = false)
		{
			this.allowLoading = allowLoading;
		}

		// Token: 0x04001607 RID: 5639
		public bool allowLoading;
	}
}

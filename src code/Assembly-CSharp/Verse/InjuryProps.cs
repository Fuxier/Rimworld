using System;

namespace Verse
{
	// Token: 0x020000FB RID: 251
	public class InjuryProps
	{
		// Token: 0x040005FD RID: 1533
		public float painPerSeverity = 1f;

		// Token: 0x040005FE RID: 1534
		public float averagePainPerSeverityPermanent = 0.5f;

		// Token: 0x040005FF RID: 1535
		public float bleedRate;

		// Token: 0x04000600 RID: 1536
		public bool canMerge;

		// Token: 0x04000601 RID: 1537
		public string destroyedLabel;

		// Token: 0x04000602 RID: 1538
		public string destroyedOutLabel;

		// Token: 0x04000603 RID: 1539
		public bool useRemovedLabel;
	}
}

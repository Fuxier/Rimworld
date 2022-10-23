using System;

namespace Verse
{
	// Token: 0x020003AC RID: 940
	public enum LoadSaveMode : byte
	{
		// Token: 0x0400138E RID: 5006
		Inactive,
		// Token: 0x0400138F RID: 5007
		Saving,
		// Token: 0x04001390 RID: 5008
		LoadingVars,
		// Token: 0x04001391 RID: 5009
		ResolvingCrossRefs,
		// Token: 0x04001392 RID: 5010
		PostLoadInit
	}
}

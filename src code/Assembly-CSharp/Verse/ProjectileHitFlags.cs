using System;

namespace Verse
{
	// Token: 0x020003FB RID: 1019
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x04001487 RID: 5255
		None = 0,
		// Token: 0x04001488 RID: 5256
		IntendedTarget = 1,
		// Token: 0x04001489 RID: 5257
		NonTargetPawns = 2,
		// Token: 0x0400148A RID: 5258
		NonTargetWorld = 4,
		// Token: 0x0400148B RID: 5259
		All = -1
	}
}

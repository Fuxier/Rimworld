using System;

namespace Verse
{
	// Token: 0x0200024B RID: 587
	[Flags]
	public enum RegionType
	{
		// Token: 0x04000E85 RID: 3717
		None = 0,
		// Token: 0x04000E86 RID: 3718
		ImpassableFreeAirExchange = 1,
		// Token: 0x04000E87 RID: 3719
		Normal = 2,
		// Token: 0x04000E88 RID: 3720
		Portal = 4,
		// Token: 0x04000E89 RID: 3721
		Fence = 8,
		// Token: 0x04000E8A RID: 3722
		Set_Passable = 14,
		// Token: 0x04000E8B RID: 3723
		Set_Impassable = 1,
		// Token: 0x04000E8C RID: 3724
		Set_All = 15
	}
}

using System;

namespace Verse
{
	// Token: 0x020002A8 RID: 680
	[Flags]
	public enum PawnRenderFlags : uint
	{
		// Token: 0x04001016 RID: 4118
		None = 0U,
		// Token: 0x04001017 RID: 4119
		Portrait = 1U,
		// Token: 0x04001018 RID: 4120
		HeadStump = 2U,
		// Token: 0x04001019 RID: 4121
		Invisible = 4U,
		// Token: 0x0400101A RID: 4122
		DrawNow = 8U,
		// Token: 0x0400101B RID: 4123
		Cache = 16U,
		// Token: 0x0400101C RID: 4124
		Headgear = 32U,
		// Token: 0x0400101D RID: 4125
		Clothes = 64U,
		// Token: 0x0400101E RID: 4126
		NeverAimWeapon = 128U,
		// Token: 0x0400101F RID: 4127
		StylingStation = 256U
	}
}

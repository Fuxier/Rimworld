using System;

namespace Verse
{
	// Token: 0x020003B3 RID: 947
	public enum LookMode : byte
	{
		// Token: 0x040013A2 RID: 5026
		Undefined,
		// Token: 0x040013A3 RID: 5027
		Value,
		// Token: 0x040013A4 RID: 5028
		Deep,
		// Token: 0x040013A5 RID: 5029
		Reference,
		// Token: 0x040013A6 RID: 5030
		Def,
		// Token: 0x040013A7 RID: 5031
		LocalTargetInfo,
		// Token: 0x040013A8 RID: 5032
		TargetInfo,
		// Token: 0x040013A9 RID: 5033
		GlobalTargetInfo,
		// Token: 0x040013AA RID: 5034
		BodyPart
	}
}

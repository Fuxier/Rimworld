using System;

namespace Verse
{
	// Token: 0x020001FF RID: 511
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x04000D51 RID: 3409
		None = 0,
		// Token: 0x04000D52 RID: 3410
		Verts = 1,
		// Token: 0x04000D53 RID: 3411
		Tris = 2,
		// Token: 0x04000D54 RID: 3412
		Colors = 4,
		// Token: 0x04000D55 RID: 3413
		UVs = 8,
		// Token: 0x04000D56 RID: 3414
		All = 127
	}
}

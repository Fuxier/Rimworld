using System;

namespace Verse
{
	// Token: 0x02000201 RID: 513
	[Flags]
	public enum MapMeshFlag
	{
		// Token: 0x04000D62 RID: 3426
		None = 0,
		// Token: 0x04000D63 RID: 3427
		Things = 1,
		// Token: 0x04000D64 RID: 3428
		FogOfWar = 2,
		// Token: 0x04000D65 RID: 3429
		Buildings = 4,
		// Token: 0x04000D66 RID: 3430
		GroundGlow = 8,
		// Token: 0x04000D67 RID: 3431
		Terrain = 16,
		// Token: 0x04000D68 RID: 3432
		Roofs = 32,
		// Token: 0x04000D69 RID: 3433
		Snow = 64,
		// Token: 0x04000D6A RID: 3434
		Zone = 128,
		// Token: 0x04000D6B RID: 3435
		PowerGrid = 256,
		// Token: 0x04000D6C RID: 3436
		BuildingsDamage = 512,
		// Token: 0x04000D6D RID: 3437
		Gas = 1024,
		// Token: 0x04000D6E RID: 3438
		Pollution = 2048
	}
}

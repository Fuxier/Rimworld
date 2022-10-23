using System;

namespace Verse
{
	// Token: 0x02000151 RID: 337
	[Flags]
	public enum WorkTags
	{
		// Token: 0x0400095B RID: 2395
		None = 0,
		// Token: 0x0400095C RID: 2396
		ManualDumb = 2,
		// Token: 0x0400095D RID: 2397
		ManualSkilled = 4,
		// Token: 0x0400095E RID: 2398
		Violent = 8,
		// Token: 0x0400095F RID: 2399
		Caring = 16,
		// Token: 0x04000960 RID: 2400
		Social = 32,
		// Token: 0x04000961 RID: 2401
		Commoner = 64,
		// Token: 0x04000962 RID: 2402
		Intellectual = 128,
		// Token: 0x04000963 RID: 2403
		Animals = 256,
		// Token: 0x04000964 RID: 2404
		Artistic = 512,
		// Token: 0x04000965 RID: 2405
		Crafting = 1024,
		// Token: 0x04000966 RID: 2406
		Cooking = 2048,
		// Token: 0x04000967 RID: 2407
		Firefighting = 4096,
		// Token: 0x04000968 RID: 2408
		Cleaning = 8192,
		// Token: 0x04000969 RID: 2409
		Hauling = 16384,
		// Token: 0x0400096A RID: 2410
		PlantWork = 32768,
		// Token: 0x0400096B RID: 2411
		Mining = 65536,
		// Token: 0x0400096C RID: 2412
		Hunting = 131072,
		// Token: 0x0400096D RID: 2413
		Constructing = 262144,
		// Token: 0x0400096E RID: 2414
		Shooting = 524288,
		// Token: 0x0400096F RID: 2415
		AllWork = 1048576
	}
}

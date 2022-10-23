using System;

namespace Verse
{
	// Token: 0x0200042C RID: 1068
	[Flags]
	public enum AllowedGameStates
	{
		// Token: 0x0400155F RID: 5471
		Invalid = 0,
		// Token: 0x04001560 RID: 5472
		Entry = 1,
		// Token: 0x04001561 RID: 5473
		Playing = 2,
		// Token: 0x04001562 RID: 5474
		WorldRenderedNow = 4,
		// Token: 0x04001563 RID: 5475
		IsCurrentlyOnMap = 8,
		// Token: 0x04001564 RID: 5476
		HasGameCondition = 16,
		// Token: 0x04001565 RID: 5477
		PlayingOnMap = 10,
		// Token: 0x04001566 RID: 5478
		PlayingOnWorld = 6
	}
}

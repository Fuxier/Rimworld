using System;

namespace Verse
{
	// Token: 0x020003FF RID: 1023
	public enum DestroyMode : byte
	{
		// Token: 0x0400149E RID: 5278
		Vanish,
		// Token: 0x0400149F RID: 5279
		WillReplace,
		// Token: 0x040014A0 RID: 5280
		KillFinalize,
		// Token: 0x040014A1 RID: 5281
		KillFinalizeLeavingsOnly,
		// Token: 0x040014A2 RID: 5282
		Deconstruct,
		// Token: 0x040014A3 RID: 5283
		FailConstruction,
		// Token: 0x040014A4 RID: 5284
		Cancel,
		// Token: 0x040014A5 RID: 5285
		Refund,
		// Token: 0x040014A6 RID: 5286
		QuestLogic
	}
}

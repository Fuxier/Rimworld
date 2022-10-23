using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000406 RID: 1030
	public interface IThingHolderWithDrawnPawn : IThingHolder
	{
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001DCE RID: 7630
		float HeldPawnDrawPos_Y { get; }

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001DCF RID: 7631
		float HeldPawnBodyAngle { get; }

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001DD0 RID: 7632
		PawnPosture HeldPawnPosture { get; }
	}
}

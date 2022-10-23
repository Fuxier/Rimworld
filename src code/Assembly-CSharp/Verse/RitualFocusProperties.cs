using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000E1 RID: 225
	public class RitualFocusProperties
	{
		// Token: 0x040004AC RID: 1196
		public IntRange spectateDistance = new IntRange(2, 2);

		// Token: 0x040004AD RID: 1197
		public SpectateRectSide allowedSpectateSides = SpectateRectSide.Horizontal;

		// Token: 0x040004AE RID: 1198
		public bool consumable;
	}
}

using System;

namespace Verse
{
	// Token: 0x020002A9 RID: 681
	public static class PawnRenderFlagsExtension
	{
		// Token: 0x06001363 RID: 4963 RVA: 0x00074B26 File Offset: 0x00072D26
		public static bool FlagSet(this PawnRenderFlags flags, PawnRenderFlags flag)
		{
			return (flags & flag) > PawnRenderFlags.None;
		}
	}
}

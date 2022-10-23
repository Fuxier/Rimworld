using System;

namespace Verse
{
	// Token: 0x0200058F RID: 1423
	public static class AutomaticPauseModeExtension
	{
		// Token: 0x06002B9F RID: 11167 RVA: 0x00114E7C File Offset: 0x0011307C
		public static string ToStringHuman(this AutomaticPauseMode mode)
		{
			switch (mode)
			{
			case AutomaticPauseMode.Never:
				return "AutomaticPauseMode_Never".Translate();
			case AutomaticPauseMode.MajorThreat:
				return "AutomaticPauseMode_MajorThreat".Translate();
			case AutomaticPauseMode.AnyThreat:
				return "AutomaticPauseMode_AnyThreat".Translate();
			case AutomaticPauseMode.AnyLetter:
				return "AutomaticPauseMode_AnyLetter".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}

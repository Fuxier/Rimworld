using System;

namespace Verse
{
	// Token: 0x02000595 RID: 1429
	public static class ShowWeaponsUnderPortraitModeExtension
	{
		// Token: 0x06002BA4 RID: 11172 RVA: 0x00115058 File Offset: 0x00113258
		public static string ToStringHuman(this ShowWeaponsUnderPortraitMode mode)
		{
			switch (mode)
			{
			case ShowWeaponsUnderPortraitMode.Never:
				return "Never".Translate().CapitalizeFirst();
			case ShowWeaponsUnderPortraitMode.WhileDrafted:
				return "ShowWeapons_WhileDrafted".Translate().CapitalizeFirst();
			case ShowWeaponsUnderPortraitMode.Always:
				return "ShowWeapons_Always".Translate().CapitalizeFirst();
			default:
				throw new NotImplementedException();
			}
		}
	}
}

using System;

namespace Verse
{
	// Token: 0x02000593 RID: 1427
	public static class MechNameDisplayModeExtension
	{
		// Token: 0x06002BA2 RID: 11170 RVA: 0x00114F98 File Offset: 0x00113198
		public static string ToStringHuman(this MechNameDisplayMode mode)
		{
			switch (mode)
			{
			case MechNameDisplayMode.None:
				return "Never".Translate().CapitalizeFirst();
			case MechNameDisplayMode.WhileDrafted:
				return "MechNameDisplayMode_WhileDrafted".Translate().CapitalizeFirst();
			case MechNameDisplayMode.Always:
				return "MechNameDisplayMode_Always".Translate().CapitalizeFirst();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x00115008 File Offset: 0x00113208
		public static bool ShouldDisplayMechName(this MechNameDisplayMode mode, Pawn mech)
		{
			switch (mode)
			{
			case MechNameDisplayMode.None:
				return false;
			case MechNameDisplayMode.WhileDrafted:
				return mech.Name != null && mech.Drafted;
			case MechNameDisplayMode.Always:
				return mech.Name != null;
			default:
				throw new NotImplementedException(Prefs.MechNameMode.ToStringSafe<MechNameDisplayMode>());
			}
		}
	}
}

using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000087 RID: 135
	public static class WildManUtility
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x0001ADED File Offset: 0x00018FED
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001ADFC File Offset: 0x00018FFC
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001AE13 File Offset: 0x00019013
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001AE2A File Offset: 0x0001902A
		public static bool WildManShouldReachOutsideNow(Pawn p)
		{
			return p.IsWildMan() && !p.mindState.WildManEverReachedOutside && (!p.IsPrisoner || p.guest.Released);
		}
	}
}

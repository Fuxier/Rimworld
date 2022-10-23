using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000083 RID: 131
	public static class DangerUtility
	{
		// Token: 0x060004C6 RID: 1222 RVA: 0x0001AB38 File Offset: 0x00018D38
		public static Danger NormalMaxDanger(this Pawn p)
		{
			if (p.CurJob != null && p.CurJob.playerForced)
			{
				return Danger.Deadly;
			}
			if (FloatMenuMakerMap.makingFor == p)
			{
				return Danger.Deadly;
			}
			if (p.Faction != Faction.OfPlayer)
			{
				return Danger.Some;
			}
			if (p.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Minor) && GenTemperature.FactionOwnsPassableRoomInTemperatureRange(p.Faction, p.SafeTemperatureRange(), p.MapHeld))
			{
				return Danger.None;
			}
			return Danger.Some;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001ABA4 File Offset: 0x00018DA4
		public static Danger GetDangerFor(this IntVec3 c, Pawn p, Map map)
		{
			Map mapHeld = p.MapHeld;
			if (mapHeld == null || mapHeld != map)
			{
				return Danger.None;
			}
			Region region = c.GetRegion(mapHeld, RegionType.Set_All);
			if (region == null)
			{
				return Danger.None;
			}
			return region.DangerFor(p);
		}
	}
}

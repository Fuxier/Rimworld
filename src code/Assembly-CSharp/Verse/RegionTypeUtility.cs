using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200024C RID: 588
	public static class RegionTypeUtility
	{
		// Token: 0x060010A2 RID: 4258 RVA: 0x00061270 File Offset: 0x0005F470
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00061276 File Offset: 0x0005F476
		public static bool AllowsMultipleRegionsPerDistrict(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00061280 File Offset: 0x0005F480
		public static RegionType GetExpectedRegionType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return RegionType.None;
			}
			if (c.GetDoor(map) != null)
			{
				return RegionType.Portal;
			}
			if (c.GetFence(map) != null)
			{
				return RegionType.Fence;
			}
			if (c.WalkableByNormal(map))
			{
				return RegionType.Normal;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.Fillage == FillCategory.Full)
				{
					return RegionType.None;
				}
			}
			return RegionType.ImpassableFreeAirExchange;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x000612E9 File Offset: 0x0005F4E9
		public static bool Passable(this RegionType regionType)
		{
			return (regionType & RegionType.Set_Passable) > RegionType.None;
		}
	}
}

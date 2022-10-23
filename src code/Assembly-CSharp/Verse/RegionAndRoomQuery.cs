using System;

namespace Verse
{
	// Token: 0x0200023D RID: 573
	public static class RegionAndRoomQuery
	{
		// Token: 0x0600103A RID: 4154 RVA: 0x0005ED00 File Offset: 0x0005CF00
		public static Region RegionAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!c.InBounds(map))
			{
				return null;
			}
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt;
			}
			return null;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0005ED35 File Offset: 0x0005CF35
		public static Region GetRegion(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return RegionAndRoomQuery.RegionAt(thing.Position, thing.Map, allowedRegionTypes);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0005ED53 File Offset: 0x0005CF53
		public static Region GetRegionHeld(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(thing.PositionHeld, thing.MapHeld, allowedRegionTypes);
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0005ED68 File Offset: 0x0005CF68
		public static District DistrictAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			if (region == null)
			{
				return null;
			}
			return region.District;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0005ED8C File Offset: 0x0005CF8C
		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_All)
		{
			District district = RegionAndRoomQuery.DistrictAt(c, map, allowedRegionTypes);
			if (district == null)
			{
				return null;
			}
			return district.Room;
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0005EDB0 File Offset: 0x0005CFB0
		public static District GetDistrict(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Thing spawnedParentOrMe;
			if ((spawnedParentOrMe = thing.SpawnedParentOrMe) != null)
			{
				return RegionAndRoomQuery.DistrictAt(spawnedParentOrMe.Position, spawnedParentOrMe.Map, allowedRegionTypes);
			}
			return null;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0005EDDC File Offset: 0x0005CFDC
		public static Room GetRoom(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_All)
		{
			District district = thing.GetDistrict(allowedRegionTypes);
			if (district == null)
			{
				return null;
			}
			return district.Room;
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0005EDFC File Offset: 0x0005CFFC
		public static District DistirctAtFast(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt.District;
			}
			return null;
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0005EE2C File Offset: 0x0005D02C
		public static Room RoomAtOrAdjacent(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, allowedRegionTypes);
			if (room != null)
			{
				return room;
			}
			for (int i = 0; i < 8; i++)
			{
				room = RegionAndRoomQuery.RoomAt(c + GenAdj.AdjacentCells[i], map, allowedRegionTypes);
				if (room != null)
				{
					return room;
				}
			}
			return room;
		}
	}
}

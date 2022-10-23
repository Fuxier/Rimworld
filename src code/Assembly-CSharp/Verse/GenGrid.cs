using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000568 RID: 1384
	public static class GenGrid
	{
		// Token: 0x06002A9F RID: 10911 RVA: 0x0011055A File Offset: 0x0010E75A
		public static bool InNoBuildEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 10);
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x00110565 File Offset: 0x0010E765
		public static bool InNoZoneEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 5);
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x00110570 File Offset: 0x0010E770
		public static bool CloseToEdge(this IntVec3 c, Map map, int edgeDist)
		{
			IntVec3 size = map.Size;
			return c.x < edgeDist || c.z < edgeDist || c.x >= size.x - edgeDist || c.z >= size.z - edgeDist;
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x001105BC File Offset: 0x0010E7BC
		public static bool OnEdge(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return c.x == 0 || c.x == size.x - 1 || c.z == 0 || c.z == size.z - 1;
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x00110604 File Offset: 0x0010E804
		public static bool OnEdge(this IntVec3 c, Map map, Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				return c.z == 0;
			}
			if (dir == Rot4.South)
			{
				return c.z == map.Size.z - 1;
			}
			if (dir == Rot4.West)
			{
				return c.x == 0;
			}
			if (dir == Rot4.East)
			{
				return c.x == map.Size.x - 1;
			}
			Log.ErrorOnce("Invalid edge direction", 55370769);
			return false;
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x00110698 File Offset: 0x0010E898
		public static bool InBounds(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return (ulong)c.x < (ulong)((long)size.x) && (ulong)c.z < (ulong)((long)size.z);
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x001106D0 File Offset: 0x0010E8D0
		public static bool InBounds(this Vector3 v, Map map)
		{
			IntVec3 size = map.Size;
			return v.x >= 0f && v.z >= 0f && v.x < (float)size.x && v.z < (float)size.z;
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x0011071E File Offset: 0x0010E91E
		public static bool WalkableByNormal(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c);
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x00110736 File Offset: 0x0010E936
		public static bool WalkableByFenceBlocked(this IntVec3 c, Map map)
		{
			return map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x0011074E File Offset: 0x0010E94E
		public static bool WalkableBy(this IntVec3 c, Map map, Pawn pawn)
		{
			return map.pathing.For(pawn).pathGrid.Walkable(c);
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x00110767 File Offset: 0x0010E967
		public static bool WalkableByAny(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c) || map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x00110799 File Offset: 0x0010E999
		public static bool Walkable(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c) && map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x001107CC File Offset: 0x0010E9CC
		public static bool Standable(this IntVec3 c, Map map)
		{
			if (!c.Walkable(map))
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x00110818 File Offset: 0x0010EA18
		public static bool Impassable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability == Traversability.Impassable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x0011085A File Offset: 0x0010EA5A
		public static bool SupportsStructureType(this IntVec3 c, Map map, TerrainAffordanceDef surfaceType)
		{
			return c.GetTerrain(map).affordances.Contains(surfaceType);
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x00110870 File Offset: 0x0010EA70
		public static bool CanBeSeenOver(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x001108A0 File Offset: 0x0010EAA0
		public static bool CanBeSeenOverFast(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x001108C4 File Offset: 0x0010EAC4
		public static bool CanBeSeenOver(this Building b)
		{
			if (b.def.Fillage == FillCategory.Full)
			{
				Building_Door building_Door = b as Building_Door;
				return building_Door != null && building_Door.Open;
			}
			return true;
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x001108F8 File Offset: 0x0010EAF8
		public static SurfaceType GetSurfaceType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return SurfaceType.None;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.surfaceType != SurfaceType.None)
				{
					return thingList[i].def.surfaceType;
				}
			}
			return SurfaceType.None;
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x0011094F File Offset: 0x0010EB4F
		public static bool HasEatSurface(this IntVec3 c, Map map)
		{
			return c.GetSurfaceType(map) == SurfaceType.Eat;
		}

		// Token: 0x04001BEC RID: 7148
		public const int NoBuildEdgeWidth = 10;

		// Token: 0x04001BED RID: 7149
		public const int NoZoneEdgeWidth = 5;
	}
}

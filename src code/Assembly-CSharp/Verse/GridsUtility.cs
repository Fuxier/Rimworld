using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E8 RID: 488
	public static class GridsUtility
	{
		// Token: 0x06000DB7 RID: 3511 RVA: 0x0004C35C File Offset: 0x0004A55C
		public static float GetTemperature(this IntVec3 loc, Map map)
		{
			return GenTemperature.GetTemperatureForCell(loc, map);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0004C365 File Offset: 0x0004A565
		public static Region GetRegion(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0004C36F File Offset: 0x0004A56F
		public static District GetDistrict(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.DistrictAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0004C379 File Offset: 0x0004A579
		public static Room GetRoom(this IntVec3 loc, Map map)
		{
			return RegionAndRoomQuery.RoomAt(loc, map, RegionType.Set_All);
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0004C384 File Offset: 0x0004A584
		public static Room GetRoomOrAdjacent(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAtOrAdjacent(loc, map, allowedRegionTypes);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0004C38E File Offset: 0x0004A58E
		public static List<Thing> GetThingList(this IntVec3 c, Map map)
		{
			return map.thingGrid.ThingsListAt(c);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0004C39C File Offset: 0x0004A59C
		public static float GetSnowDepth(this IntVec3 c, Map map)
		{
			return map.snowGrid.GetDepth(c);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0004C3AA File Offset: 0x0004A5AA
		public static bool Fogged(this Thing t)
		{
			return t.Map.fogGrid.IsFogged(t.Position);
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0004C3C2 File Offset: 0x0004A5C2
		public static bool Fogged(this IntVec3 c, Map map)
		{
			return map.fogGrid.IsFogged(c);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0004C3D0 File Offset: 0x0004A5D0
		public static RoofDef GetRoof(this IntVec3 c, Map map)
		{
			return map.roofGrid.RoofAt(c);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0004C3DE File Offset: 0x0004A5DE
		public static bool Roofed(this IntVec3 c, Map map)
		{
			return map.roofGrid.Roofed(c);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0004C3EC File Offset: 0x0004A5EC
		public static bool Filled(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0004C414 File Offset: 0x0004A614
		public static TerrainDef GetTerrain(this IntVec3 c, Map map)
		{
			return map.terrainGrid.TerrainAt(c);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0004C422 File Offset: 0x0004A622
		public static Zone GetZone(this IntVec3 c, Map map)
		{
			return map.zoneManager.ZoneAt(c);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0004C430 File Offset: 0x0004A630
		public static Plant GetPlant(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Plant)
				{
					return (Plant)list[i];
				}
			}
			return null;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0004C480 File Offset: 0x0004A680
		public static Thing GetRoofHolderOrImpassable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.holdsRoof || thingList[i].def.passability == Traversability.Impassable)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0004C4D8 File Offset: 0x0004A6D8
		public static Thing GetFirstThing(this IntVec3 c, Map map, ThingDef def)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == def)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0004C518 File Offset: 0x0004A718
		public static ThingWithComps GetFirstThingWithComp<TComp>(this IntVec3 c, Map map) where TComp : ThingComp
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].TryGetComp<TComp>() != null)
				{
					return (ThingWithComps)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0004C560 File Offset: 0x0004A760
		public static T GetFirstThing<T>(this IntVec3 c, Map map) where T : Thing
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				T t = thingList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0004C5AC File Offset: 0x0004A7AC
		public static Thing GetFirstHaulable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.designateHaulable)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0004C5F4 File Offset: 0x0004A7F4
		public static Thing GetFirstItem(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0004C63C File Offset: 0x0004A83C
		public static IEnumerable<Thing> GetItems(this IntVec3 c, Map map)
		{
			List<Thing> thingList = map.thingGrid.ThingsListAt(c);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (thingList[i].def.category == ThingCategory.Item)
				{
					yield return thingList[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0004C654 File Offset: 0x0004A854
		public static int GetMaxItemsAllowedInCell(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null)
			{
				return edifice.MaxItemsInCell;
			}
			return 1;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0004C674 File Offset: 0x0004A874
		public static int GetItemCount(this IntVec3 c, Map map)
		{
			int num = 0;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0004C6BC File Offset: 0x0004A8BC
		public static int GetAllItemsStackCount(this IntVec3 c, Map map, ThingDef itemDef)
		{
			int num = 0;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == itemDef)
				{
					num += list[i].stackCount;
				}
			}
			return num;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0004C708 File Offset: 0x0004A908
		public static int GetItemStackSpaceLeftFor(this IntVec3 c, Map map, ThingDef itemDef)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					num2++;
				}
				if (list[i].def == itemDef)
				{
					num += Mathf.Max(list[i].def.stackLimit - list[i].stackCount, 0);
				}
			}
			return num + Mathf.Max(c.GetMaxItemsAllowedInCell(map) - num2, 0) * itemDef.stackLimit;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0004C7A0 File Offset: 0x0004A9A0
		public static Building GetFirstBuilding(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Building building = list[i] as Building;
				if (building != null)
				{
					return building;
				}
			}
			return null;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0004C7E0 File Offset: 0x0004A9E0
		public static Pawn GetFirstPawn(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0004C81C File Offset: 0x0004AA1C
		public static Mineable GetFirstMineable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Mineable mineable = thingList[i] as Mineable;
				if (mineable != null)
				{
					return mineable;
				}
			}
			return null;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0004C858 File Offset: 0x0004AA58
		public static Blight GetFirstBlight(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Blight blight = thingList[i] as Blight;
				if (blight != null)
				{
					return blight;
				}
			}
			return null;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0004C894 File Offset: 0x0004AA94
		public static Skyfaller GetFirstSkyfaller(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Skyfaller skyfaller = thingList[i] as Skyfaller;
				if (skyfaller != null)
				{
					return skyfaller;
				}
			}
			return null;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0004C8D0 File Offset: 0x0004AAD0
		public static IPlantToGrowSettable GetPlantToGrowSettable(this IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetEdifice(map) as IPlantToGrowSettable;
			if (plantToGrowSettable == null)
			{
				plantToGrowSettable = (c.GetZone(map) as IPlantToGrowSettable);
			}
			return plantToGrowSettable;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0004C8FC File Offset: 0x0004AAFC
		public static Building GetTransmitter(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.EverTransmitsPower)
				{
					return (Building)list[i];
				}
			}
			return null;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0004C948 File Offset: 0x0004AB48
		public static Building_Door GetDoor(this IntVec3 c, Map map)
		{
			Building_Door result;
			if ((result = (c.GetEdifice(map) as Building_Door)) != null)
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0004C968 File Offset: 0x0004AB68
		public static Building GetFence(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def.IsFence)
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0004C990 File Offset: 0x0004AB90
		public static Building GetEdifice(this IntVec3 c, Map map)
		{
			return map.edificeGrid[c];
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0004C99E File Offset: 0x0004AB9E
		public static Thing GetCover(this IntVec3 c, Map map)
		{
			return map.coverGrid[c];
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0004C9AC File Offset: 0x0004ABAC
		public static Gas GetGas(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Gas)
				{
					return (Gas)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0004C9F4 File Offset: 0x0004ABF4
		public static bool IsInPrisonCell(this IntVec3 c, Map map)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_Passable);
			if (roomOrAdjacent != null)
			{
				return roomOrAdjacent.IsPrisonCell;
			}
			Log.Error("Checking prison cell status of " + c + " which is not in or adjacent to a room.");
			return false;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0004CA30 File Offset: 0x0004AC30
		public static bool UsesOutdoorTemperature(this IntVec3 c, Map map)
		{
			Room room = c.GetRoom(map);
			if (room != null)
			{
				return room.UsesOutdoorTemperature;
			}
			Building edifice = c.GetEdifice(map);
			if (edifice != null)
			{
				IntVec3[] array = GenAdj.CellsAdjacent8Way(edifice).ToArray<IntVec3>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].InBounds(map))
					{
						room = array[i].GetRoom(map);
						if (room != null && room.UsesOutdoorTemperature)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0004CAA1 File Offset: 0x0004ACA1
		public static bool IsPolluted(this IntVec3 c, Map map)
		{
			return map.pollutionGrid.IsPolluted(c);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0004CAAF File Offset: 0x0004ACAF
		public static bool CanPollute(this IntVec3 c, Map map)
		{
			return map.pollutionGrid.CanPollute(c);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0004CABD File Offset: 0x0004ACBD
		public static bool CanUnpollute(this IntVec3 c, Map map)
		{
			return map.pollutionGrid.CanUnpollute(c);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0004CACB File Offset: 0x0004ACCB
		public static void Pollute(this IntVec3 c, Map map, bool silent = false)
		{
			map.pollutionGrid.SetPolluted(c, true, silent);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0004CADB File Offset: 0x0004ACDB
		public static void Unpollute(this IntVec3 c, Map map)
		{
			map.pollutionGrid.SetPolluted(c, false, false);
			if (map.snowGrid.GetDepth(c) > 1E-45f)
			{
				map.snowGrid.MakeMeshDirty(c);
			}
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0004CB0A File Offset: 0x0004AD0A
		public static float GetFertility(this IntVec3 c, Map map)
		{
			return map.fertilityGrid.FertilityAt(c);
		}
	}
}

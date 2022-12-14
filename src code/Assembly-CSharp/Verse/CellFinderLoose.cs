using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200055B RID: 1371
	public static class CellFinderLoose
	{
		// Token: 0x06002A15 RID: 10773 RVA: 0x0010C394 File Offset: 0x0010A594
		public static IntVec3 RandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries = 1000)
		{
			IntVec3 result;
			CellFinderLoose.TryGetRandomCellWith(validator, map, maxTries, out result);
			return result;
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x0010C3B0 File Offset: 0x0010A5B0
		public static bool TryGetRandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries, out IntVec3 result)
		{
			for (int i = 0; i < maxTries; i++)
			{
				result = CellFinder.RandomCell(map);
				if (validator(result))
				{
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x0010C3F4 File Offset: 0x0010A5F4
		public static bool TryFindRandomNotEdgeCellWith(int minEdgeDistance, Predicate<IntVec3> validator, Map map, out IntVec3 result)
		{
			for (int i = 0; i < 1000; i++)
			{
				result = CellFinder.RandomNotEdgeCell(minEdgeDistance, map);
				if (result.IsValid && validator(result))
				{
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x0010C444 File Offset: 0x0010A644
		public static bool GetFleeExitPosition(Pawn pawn, float radius, out IntVec3 position)
		{
			return CellFinder.TryFindRandomEdgeCellNearWith(pawn.Position, radius, pawn.Map, (IntVec3 p) => GenSight.LineOfSight(pawn.Position, p, pawn.Map, false, null, 0, 0) && pawn.CanReach(p, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn), out position);
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x0010C487 File Offset: 0x0010A687
		public static IntVec3 GetFleeDest(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			if (pawn.RaceProps.Animal)
			{
				return CellFinderLoose.GetFleeDestAnimal(pawn, threats, distance);
			}
			return CellFinderLoose.GetFleeDestToolUser(pawn, threats, distance);
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x0010C4A8 File Offset: 0x0010A6A8
		public static IntVec3 GetFleeDestAnimal(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			Vector3 normalized = (pawn.Position - threats[0].Position).ToVector3().normalized;
			float num = distance - pawn.Position.DistanceTo(threats[0].Position);
			for (float num2 = 200f; num2 <= 360f; num2 += 10f)
			{
				IntVec3 intVec = pawn.Position + (normalized.RotatedBy(Rand.Range(-num2 / 2f, num2 / 2f)) * num).ToIntVec3();
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec))
				{
					return intVec;
				}
			}
			float num3 = num;
			while (num3 * 3f > num)
			{
				IntVec3 intVec2 = pawn.Position + IntVec3Utility.RandomHorizontalOffset(num3);
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec2))
				{
					return intVec2;
				}
				num3 -= distance / 10f;
			}
			return pawn.Position;
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x0010C598 File Offset: 0x0010A798
		public static bool CanFleeToLocation(Pawn pawn, IntVec3 location)
		{
			return location.Standable(pawn.Map) && pawn.Map.pawnDestinationReservationManager.CanReserve(location, pawn, false) && location.GetRegion(pawn.Map, RegionType.Set_Passable).type != RegionType.Portal && pawn.CanReach(location, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x0010C5FC File Offset: 0x0010A7FC
		public static IntVec3 GetFleeDestToolUser(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			IntVec3 bestPos = pawn.Position;
			float bestScore = -1f;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
			RegionTraverser.BreadthFirstTraverse(pawn.GetRegion(RegionType.Set_Passable), (Region from, Region reg) => reg.Allows(traverseParms, false), delegate(Region reg)
			{
				Danger danger = reg.DangerFor(pawn);
				Map map = pawn.Map;
				foreach (IntVec3 intVec in reg.Cells)
				{
					if (intVec.Standable(map) && !reg.IsDoorway)
					{
						Thing thing = null;
						float num = 0f;
						for (int i = 0; i < threats.Count; i++)
						{
							float num2 = (float)intVec.DistanceToSquared(threats[i].Position);
							if (thing == null || num2 < num)
							{
								thing = threats[i];
								num = num2;
							}
						}
						float num3 = Mathf.Sqrt(num);
						float num4 = Mathf.Pow(Mathf.Min(num3, distance), 1.2f);
						num4 *= Mathf.InverseLerp(50f, 0f, (intVec - pawn.Position).LengthHorizontal);
						if (intVec.GetRoom(map) != thing.GetRoom(RegionType.Set_All))
						{
							num4 *= 4.2f;
						}
						else if (num3 < 8f)
						{
							num4 *= 0.05f;
						}
						if (!map.pawnDestinationReservationManager.CanReserve(intVec, pawn, false))
						{
							num4 *= 0.5f;
						}
						if (danger == Danger.Deadly)
						{
							num4 *= 0.8f;
						}
						if (num4 > bestScore)
						{
							bestPos = intVec;
							bestScore = num4;
						}
					}
				}
				return false;
			}, 20, RegionType.Set_Passable);
			return bestPos;
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x0010C68C File Offset: 0x0010A88C
		public static IntVec3 TryFindCentralCell(Map map, int tightness, int minCellCount, Predicate<IntVec3> extraValidator = null)
		{
			int debug_numStand = 0;
			int debug_numDistrict = 0;
			int debug_numTouch = 0;
			int debug_numDistrictCellCount = 0;
			int debug_numExtraValidator = 0;
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					int num2 = debug_numStand;
					debug_numStand = num2 + 1;
					return false;
				}
				District district = c.GetDistrict(map, RegionType.Set_Passable);
				if (district == null)
				{
					int num2 = debug_numDistrict;
					debug_numDistrict = num2 + 1;
					return false;
				}
				if (!district.TouchesMapEdge)
				{
					int num2 = debug_numTouch;
					debug_numTouch = num2 + 1;
					return false;
				}
				if (district.CellCount < minCellCount)
				{
					int num2 = debug_numDistrictCellCount;
					debug_numDistrictCellCount = num2 + 1;
					return false;
				}
				if (extraValidator != null && !extraValidator(c))
				{
					int num2 = debug_numExtraValidator;
					debug_numExtraValidator = num2 + 1;
					return false;
				}
				return true;
			};
			for (int i = tightness; i >= 1; i--)
			{
				int num = map.Size.x / i;
				IntVec3 result;
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith((map.Size.x - num) / 2, validator, map, out result))
				{
					return result;
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Found no good central spot. Choosing randomly. numStand=",
				debug_numStand,
				", numDistrict=",
				debug_numDistrict,
				", numTouch=",
				debug_numTouch,
				", numDistrictCellCount=",
				debug_numDistrictCellCount,
				", numExtraValidator=",
				debug_numExtraValidator
			}));
			return CellFinderLoose.RandomCellWith((IntVec3 x) => x.Standable(map), map, 1000);
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x0010C7C8 File Offset: 0x0010A9C8
		public static bool TryFindSkyfallerCell(ThingDef skyfaller, Map map, out IntVec3 cell, int minDistToEdge = 10, IntVec3 nearLoc = default(IntVec3), int nearLocMaxDist = -1, bool allowRoofedCells = true, bool allowCellsWithItems = false, bool allowCellsWithBuildings = false, bool colonyReachable = false, bool avoidColonistsIfExplosive = true, bool alwaysAvoidColonists = false, Predicate<IntVec3> extraValidator = null)
		{
			bool avoidColonists = (avoidColonistsIfExplosive && skyfaller.skyfaller.CausesExplosion) || alwaysAvoidColonists;
			Predicate<IntVec3> validator = delegate(IntVec3 x)
			{
				foreach (IntVec3 c in GenAdj.OccupiedRect(x, Rot4.North, skyfaller.size))
				{
					if (!c.InBounds(map) || c.Fogged(map) || !c.Standable(map) || (c.Roofed(map) && c.GetRoof(map).isThickRoof))
					{
						return false;
					}
					if (!allowRoofedCells && c.Roofed(map))
					{
						return false;
					}
					if (!allowCellsWithItems && c.GetFirstItem(map) != null)
					{
						return false;
					}
					if (!allowCellsWithBuildings && c.GetFirstBuilding(map) != null)
					{
						return false;
					}
					if (c.GetFirstSkyfaller(map) != null)
					{
						return false;
					}
					using (List<Thing>.Enumerator enumerator2 = c.GetThingList(map).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.preventSkyfallersLandingOn)
							{
								return false;
							}
						}
					}
				}
				return (!avoidColonists || !SkyfallerUtility.CanPossiblyFallOnColonist(skyfaller, x, map)) && (minDistToEdge <= 0 || x.DistanceToEdge(map) >= minDistToEdge) && (!colonyReachable || map.reachability.CanReachColony(x)) && (extraValidator == null || extraValidator(x));
			};
			if (nearLocMaxDist > 0)
			{
				return CellFinder.TryFindRandomCellNear(nearLoc, map, nearLocMaxDist, validator, out cell, -1);
			}
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(minDistToEdge, validator, map, out cell);
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x0010C870 File Offset: 0x0010AA70
		public static IntVec3 GetFallbackDest(Pawn pawn, List<Thing> threats, float maxDistance = 40f, float minDistanceFromThreat = 0f, float minDistanceFromRoot = 0f, int maxRegions = 20, Func<IntVec3, bool> validator = null)
		{
			IntVec3 bestPos = IntVec3.Invalid;
			float bestScore = -1f;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
			RegionTraverser.BreadthFirstTraverse(pawn.GetRegion(RegionType.Set_Passable), (Region from, Region reg) => reg.Allows(traverseParms, false), delegate(Region reg)
			{
				reg.DangerFor(pawn);
				Map map = pawn.Map;
				foreach (IntVec3 intVec in reg.Cells)
				{
					if ((validator == null || validator(intVec)) && intVec.Standable(map))
					{
						int num = intVec.DistanceToSquared(pawn.Position);
						if ((float)num >= minDistanceFromRoot * minDistanceFromRoot && (float)num <= maxDistance * maxDistance)
						{
							Thing thing = null;
							float num2 = 0f;
							for (int i = 0; i < threats.Count; i++)
							{
								int num3 = intVec.DistanceToSquared(threats[i].Position);
								if (thing == null || (float)num3 < num2)
								{
									thing = threats[i];
									num2 = (float)num3;
								}
							}
							if (num2 >= minDistanceFromThreat * minDistanceFromThreat)
							{
								float num4 = Mathf.Sqrt(num2);
								if (!map.pawnDestinationReservationManager.CanReserve(intVec, pawn, false))
								{
									num4 *= 0.5f;
								}
								num4 += CoverUtility.TotalSurroundingCoverScore(intVec, pawn.Map);
								if (num4 > bestScore)
								{
									bestPos = intVec;
									bestScore = num4;
								}
							}
						}
					}
				}
				return false;
			}, maxRegions, RegionType.Set_Passable);
			return bestPos;
		}
	}
}

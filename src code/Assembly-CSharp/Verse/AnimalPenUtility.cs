using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020001A7 RID: 423
	public static class AnimalPenUtility
	{
		// Token: 0x06000BB4 RID: 2996 RVA: 0x00041A04 File Offset: 0x0003FC04
		public static ThingFilter GetFixedAnimalFilter()
		{
			if (AnimalPenUtility.fixedFilter == null)
			{
				AnimalPenUtility.fixedFilter = new ThingFilter();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(AnimalPenUtility.IsRopeManagedAnimalDef)))
				{
					AnimalPenUtility.fixedFilter.SetAllow(thingDef, true);
				}
			}
			return AnimalPenUtility.fixedFilter;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00041A7C File Offset: 0x0003FC7C
		public static bool IsRopeManagedAnimalDef(ThingDef td)
		{
			return td.race != null && td.race.Roamer && td.IsWithinCategory(ThingCategoryDefOf.Animals);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00041AA0 File Offset: 0x0003FCA0
		private static bool ShouldBePennedByDefault(ThingDef td)
		{
			return AnimalPenUtility.IsRopeManagedAnimalDef(td) && td.race.FenceBlocked;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00041AB7 File Offset: 0x0003FCB7
		public static bool NeedsToBeManagedByRope(Pawn pawn)
		{
			return AnimalPenUtility.IsRopeManagedAnimalDef(pawn.def) && pawn.Spawned && pawn.Map.IsPlayerHome;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00041ADC File Offset: 0x0003FCDC
		public static ThingFilter GetDefaultAnimalFilter()
		{
			if (AnimalPenUtility.defaultFilter == null)
			{
				AnimalPenUtility.defaultFilter = new ThingFilter();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(AnimalPenUtility.ShouldBePennedByDefault)))
				{
					AnimalPenUtility.defaultFilter.SetAllow(thingDef, true);
				}
			}
			return AnimalPenUtility.defaultFilter;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00041B54 File Offset: 0x0003FD54
		public static void ResetStaticData()
		{
			AnimalPenUtility.defaultFilter = null;
			AnimalPenUtility.fixedFilter = null;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00041B64 File Offset: 0x0003FD64
		public static CompAnimalPenMarker GetCurrentPenOf(Pawn animal, bool allowUnenclosedPens)
		{
			Map map = animal.Map;
			if (!map.listerBuildings.allBuildingsAnimalPenMarkers.Any<Building>())
			{
				return null;
			}
			List<District> list = AnimalPenUtility.tmpConnectedDistrictsCalc.CalculateConnectedDistricts(animal.Position, map);
			if (!list.Any<District>())
			{
				return null;
			}
			CompAnimalPenMarker compAnimalPenMarker = null;
			foreach (Building building in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				if (list.Contains(building.Position.GetDistrict(map, RegionType.Set_Passable)))
				{
					CompAnimalPenMarker compAnimalPenMarker2 = building.TryGetComp<CompAnimalPenMarker>();
					if (AnimalPenUtility.CanUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, null) && (compAnimalPenMarker == null || (compAnimalPenMarker2.PenState.Enclosed && compAnimalPenMarker.PenState.Unenclosed) || map.cellIndices.CellToIndex(compAnimalPenMarker2.parent.Position) < map.cellIndices.CellToIndex(compAnimalPenMarker.parent.Position)))
					{
						compAnimalPenMarker = compAnimalPenMarker2;
					}
				}
			}
			AnimalPenUtility.tmpConnectedDistrictsCalc.Reset();
			return compAnimalPenMarker;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00041C7C File Offset: 0x0003FE7C
		public static bool AnySuitablePens(Pawn animal, bool allowUnenclosedPens)
		{
			foreach (Building thing in animal.Map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker penMarker = thing.TryGetComp<CompAnimalPenMarker>();
				if (AnimalPenUtility.CanUseAndReach(animal, penMarker, allowUnenclosedPens, null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00041CEC File Offset: 0x0003FEEC
		public static bool AnySuitableHitch(Pawn animal)
		{
			foreach (Building t in animal.Map.listerBuildings.allBuildingsHitchingPosts)
			{
				if (animal.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00041D5C File Offset: 0x0003FF5C
		public static CompAnimalPenMarker ClosestSuitablePen(Pawn animal, bool allowUnenclosedPens)
		{
			Map map = animal.Map;
			CompAnimalPenMarker compAnimalPenMarker = null;
			float num = 0f;
			foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker2 = thing.TryGetComp<CompAnimalPenMarker>();
				if (AnimalPenUtility.CanUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, null))
				{
					int num2 = animal.Position.DistanceToSquared(compAnimalPenMarker2.parent.Position);
					if (compAnimalPenMarker == null || (float)num2 < num)
					{
						compAnimalPenMarker = compAnimalPenMarker2;
						num = (float)num2;
					}
				}
			}
			return compAnimalPenMarker;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00041DF4 File Offset: 0x0003FFF4
		private static bool CanUseAndReach(Pawn animal, CompAnimalPenMarker penMarker, bool allowUnenclosedPens, Pawn roper = null)
		{
			bool flag = false;
			return AnimalPenUtility.CheckUseAndReach(animal, penMarker, allowUnenclosedPens, roper, ref flag, ref flag, ref flag);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00041E14 File Offset: 0x00040014
		private static bool CheckUseAndReach(Pawn animal, CompAnimalPenMarker penMarker, bool allowUnenclosedPens, Pawn roper, ref bool foundEnclosed, ref bool foundUsable, ref bool foundReachable)
		{
			if (!allowUnenclosedPens && penMarker.PenState.Unenclosed)
			{
				return false;
			}
			foundEnclosed = true;
			if (!penMarker.AcceptsToPen(animal))
			{
				return false;
			}
			if (roper == null && penMarker.parent.IsForbidden(Faction.OfPlayer))
			{
				return false;
			}
			if (roper != null && penMarker.parent.IsForbidden(roper))
			{
				return false;
			}
			foundUsable = true;
			bool flag;
			if (roper == null)
			{
				TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false).WithFenceblockedOf(animal);
				flag = animal.Map.reachability.CanReach(animal.Position, penMarker.parent, PathEndMode.Touch, traverseParams);
			}
			else
			{
				TraverseParms traverseParams2 = TraverseParms.For(roper, Danger.Deadly, TraverseMode.ByPawn, false, false, false).WithFenceblockedOf(animal);
				flag = animal.Map.reachability.CanReach(animal.Position, penMarker.parent, PathEndMode.Touch, traverseParams2);
			}
			if (!flag)
			{
				return false;
			}
			foundReachable = true;
			return true;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00041EF0 File Offset: 0x000400F0
		public static CompAnimalPenMarker GetPenAnimalShouldBeTakenTo(Pawn roper, Pawn animal, out string jobFailReason, bool forced = false, bool canInteractWhileSleeping = true, bool allowUnenclosedPens = false, bool ignoreSkillRequirements = true, RopingPriority mode = RopingPriority.Closest, AnimalPenBalanceCalculator balanceCalculator = null)
		{
			jobFailReason = null;
			if (allowUnenclosedPens && mode == RopingPriority.Balanced)
			{
				Log.Warning("Cannot allow unenclosed pens in balanced mode");
				return null;
			}
			if (animal == roper)
			{
				return null;
			}
			if (animal == null || !AnimalPenUtility.NeedsToBeManagedByRope(animal))
			{
				return null;
			}
			if (animal.Faction != roper.Faction)
			{
				return null;
			}
			if (!forced && animal.roping.IsRopedByPawn && animal.roping.RopedByPawn != roper)
			{
				return null;
			}
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(animal, allowUnenclosedPens);
			if (mode == RopingPriority.Closest && currentPenOf != null && currentPenOf.PenState.Enclosed)
			{
				return null;
			}
			if (!WorkGiver_InteractAnimal.CanInteractWithAnimal(roper, animal, out jobFailReason, forced, canInteractWhileSleeping, ignoreSkillRequirements, true))
			{
				return null;
			}
			Map map = animal.Map;
			AnimalPenBalanceCalculator animalPenBalanceCalculator = balanceCalculator ?? new AnimalPenBalanceCalculator(map, true);
			CompAnimalPenMarker compAnimalPenMarker = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker2 = thing.TryGetComp<CompAnimalPenMarker>();
				flag2 = true;
				if (AnimalPenUtility.CheckUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, roper, ref flag3, ref flag4, ref flag5))
				{
					if (mode == RopingPriority.Closest)
					{
						if (compAnimalPenMarker == null || (compAnimalPenMarker2.PenState.Enclosed && compAnimalPenMarker.PenState.Unenclosed) || AnimalPenUtility.PenIsCloser(compAnimalPenMarker2, compAnimalPenMarker, animal))
						{
							compAnimalPenMarker = compAnimalPenMarker2;
						}
					}
					else if (mode == RopingPriority.Balanced)
					{
						if (currentPenOf != null && !animalPenBalanceCalculator.IsBetterPen(compAnimalPenMarker2, currentPenOf, true, animal))
						{
							flag = true;
						}
						else if (compAnimalPenMarker == null || animalPenBalanceCalculator.IsBetterPen(compAnimalPenMarker2, compAnimalPenMarker, false, animal))
						{
							compAnimalPenMarker = compAnimalPenMarker2;
							flag = false;
						}
					}
				}
			}
			if (currentPenOf != null && compAnimalPenMarker == currentPenOf)
			{
				return null;
			}
			if (compAnimalPenMarker == null)
			{
				if (flag)
				{
					jobFailReason = "CantRopeAnimalAlreadyInBestPen".Translate();
				}
				else if (!flag2)
				{
					jobFailReason = "CantRopeAnimalNoUsableReachablePens".Translate();
				}
				else if (!flag3)
				{
					jobFailReason = "CantRopeAnimalNoEnclosedPens".Translate();
				}
				else if (!flag4)
				{
					jobFailReason = "CantRopeAnimalNoAllowedPens".Translate();
				}
				else if (!flag5)
				{
					jobFailReason = "CantRopeAnimalNoReachablePens".Translate();
				}
				else
				{
					jobFailReason = "CantRopeAnimalNoUsableReachablePens".Translate();
				}
				return null;
			}
			return compAnimalPenMarker;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0004214C File Offset: 0x0004034C
		public static Building GetHitchingPostAnimalShouldBeTakenTo(Pawn roper, Pawn animal, out string jobFailReason, bool forced = false)
		{
			jobFailReason = null;
			if (animal == roper)
			{
				return null;
			}
			if (animal == null || !AnimalPenUtility.IsRopeManagedAnimalDef(animal.def))
			{
				return null;
			}
			if (animal.Faction != roper.Faction)
			{
				return null;
			}
			if (!forced && animal.roping.IsRopedByPawn && animal.roping.RopedByPawn != roper)
			{
				return null;
			}
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			if (animal.roping.IsRopedToHitchingPost || AnimalPenUtility.GetCurrentPenOf(animal, false) != null)
			{
				return null;
			}
			if (!WorkGiver_InteractAnimal.CanInteractWithAnimal(roper, animal, out jobFailReason, forced, true, true, true))
			{
				return null;
			}
			Building building = null;
			foreach (Building building2 in animal.Map.listerBuildings.allBuildingsHitchingPosts)
			{
				if (!building2.IsForbidden(roper) && roper.CanReach(building2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && animal.Map.reachability.CanReach(animal.Position, building2, PathEndMode.Touch, TraverseParms.For(roper, Danger.Deadly, TraverseMode.ByPawn, false, false, false).WithFenceblockedOf(animal)) && (building == null || (animal.Position - building2.Position).LengthManhattan < (animal.Position - building.Position).LengthManhattan))
				{
					building = building2;
				}
			}
			return building;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000422E8 File Offset: 0x000404E8
		private static bool PenIsCloser(CompAnimalPenMarker markerA, CompAnimalPenMarker markerB, Pawn animal)
		{
			return animal.Position.DistanceToSquared(markerA.parent.Position) < animal.Position.DistanceToSquared(markerB.parent.Position);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00042318 File Offset: 0x00040518
		public static IntVec3 RopeAttachmentInteractionCell(Pawn roper, Pawn ropee)
		{
			if (!roper.Spawned || !ropee.Spawned)
			{
				return IntVec3.Invalid;
			}
			if (AnimalPenUtility.IsGoodRopeAttachmentInteractionCell(roper, ropee, roper.Position))
			{
				return roper.Position;
			}
			Map map = ropee.Map;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = ropee.Position + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(map) && AnimalPenUtility.MutuallyWalkable(roper, ropee, intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00042398 File Offset: 0x00040598
		public static bool IsGoodRopeAttachmentInteractionCell(Pawn roper, Pawn ropee, IntVec3 cell)
		{
			return ropee.Position.AdjacentToCardinal(cell) && AnimalPenUtility.MutuallyWalkable(roper, ropee, cell);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000423C0 File Offset: 0x000405C0
		private static bool MutuallyWalkable(Pawn roper, Pawn ropee, IntVec3 c)
		{
			Map map = ropee.Map;
			return c.WalkableBy(map, ropee) && c.WalkableBy(map, roper);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000423E8 File Offset: 0x000405E8
		public static IntVec3 FindPlaceInPenToStand(CompAnimalPenMarker marker, Pawn pawn)
		{
			AnimalPenUtility.<>c__DisplayClass21_0 CS$<>8__locals1 = new AnimalPenUtility.<>c__DisplayClass21_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.marker = marker;
			CS$<>8__locals1.map = CS$<>8__locals1.pawn.Map;
			CS$<>8__locals1.result = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(CS$<>8__locals1.marker.parent.Position, CS$<>8__locals1.map, (Region from, Region to) => CS$<>8__locals1.marker.PenState.ContainsConnectedRegion(to), new RegionProcessor(CS$<>8__locals1.<FindPlaceInPenToStand>g__RegionProcessor|1), 999999, RegionType.Set_Passable);
			return CS$<>8__locals1.result;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00042468 File Offset: 0x00040668
		public static bool IsUnnecessarilyRoped(Pawn animal)
		{
			if (!AnimalPenUtility.NeedsToBeManagedByRope(animal))
			{
				return false;
			}
			if (animal.roping.IsRopedByPawn || animal.roping.IsRopedToHitchingPost || !animal.roping.IsRopedToSpot)
			{
				return false;
			}
			Lord lord = animal.GetLord();
			int? num;
			if (lord == null)
			{
				num = null;
			}
			else
			{
				LordJob lordJob = lord.LordJob;
				num = ((lordJob != null) ? new bool?(lordJob.ManagesRopableAnimals) : null);
			}
			return (num ?? 0) == 0;
		}

		// Token: 0x04000AFE RID: 2814
		private static ThingFilter fixedFilter;

		// Token: 0x04000AFF RID: 2815
		private static ThingFilter defaultFilter;

		// Token: 0x04000B00 RID: 2816
		private static readonly AnimalPenConnectedDistrictsCalculator tmpConnectedDistrictsCalc = new AnimalPenConnectedDistrictsCalculator();
	}
}

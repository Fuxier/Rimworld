using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000239 RID: 569
	public static class ReachabilityImmediate
	{
		// Token: 0x06001015 RID: 4117 RVA: 0x0005DB94 File Offset: 0x0005BD94
		public static bool CanReachImmediate(IntVec3 start, LocalTargetInfo target, Map map, PathEndMode peMode, Pawn pawn)
		{
			if (!target.IsValid)
			{
				return false;
			}
			target = (LocalTargetInfo)GenPath.ResolvePathMode(pawn, target.ToTargetInfo(map), ref peMode);
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing.Spawned)
				{
					if (thing.Map != map)
					{
						return false;
					}
				}
				else
				{
					if (pawn != null)
					{
						if (pawn.carryTracker.innerContainer.Contains(thing))
						{
							return true;
						}
						if (pawn.inventory.innerContainer.Contains(thing))
						{
							return true;
						}
						if (pawn.apparel != null && pawn.apparel.Contains(thing))
						{
							return true;
						}
						if (pawn.equipment != null && pawn.equipment.Contains(thing))
						{
							return true;
						}
					}
					if (!thing.SpawnedOrAnyParentSpawned)
					{
						return false;
					}
					if (thing.MapHeld != map)
					{
						return false;
					}
				}
			}
			if (!target.HasThing || (target.Thing.def.size.x == 1 && target.Thing.def.size.z == 1))
			{
				if (start == target.Cell)
				{
					return true;
				}
			}
			else if (start.IsInside(target.Thing))
			{
				return true;
			}
			return peMode == PathEndMode.Touch && TouchPathEndModeUtility.IsAdjacentOrInsideAndAllowedToTouch(start, target, map.pathing.For(pawn));
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0005DCDE File Offset: 0x0005BEDE
		public static bool CanReachImmediate(this Pawn pawn, LocalTargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && ReachabilityImmediate.CanReachImmediate(pawn.Position, target, pawn.Map, peMode, pawn);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0005DCFE File Offset: 0x0005BEFE
		public static bool CanReachImmediateNonLocal(this Pawn pawn, TargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && (target.Map == null || target.Map == pawn.Map) && pawn.CanReachImmediate((LocalTargetInfo)target, peMode);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0005DD34 File Offset: 0x0005BF34
		public static bool CanReachImmediate(IntVec3 start, CellRect rect, Map map, PathEndMode peMode, Pawn pawn)
		{
			IntVec3 c = rect.ClosestCellTo(start);
			return ReachabilityImmediate.CanReachImmediate(start, c, map, peMode, pawn);
		}
	}
}

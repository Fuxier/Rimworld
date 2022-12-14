using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200055E RID: 1374
	public static class GenClosest
	{
		// Token: 0x06002A25 RID: 10789 RVA: 0x0010CAC4 File Offset: 0x0010ACC4
		private static bool EarlyOutSearch(IntVec3 start, Map map, ThingRequest thingReq, IEnumerable<Thing> customGlobalSearchSet, Predicate<Thing> validator)
		{
			if (thingReq.group == ThingRequestGroup.Everything)
			{
				Log.Error("Cannot do ClosestThingReachable searching everything without restriction.");
				return true;
			}
			if (!start.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Did FindClosestThing with start out of bounds (",
					start,
					"), thingReq=",
					thingReq
				}));
				return true;
			}
			return thingReq.group == ThingRequestGroup.Nothing || ((thingReq.IsUndefined || map.listerThings.ThingsMatching(thingReq).Count == 0) && customGlobalSearchSet.EnumerableNullOrEmpty<Thing>());
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x0010CB58 File Offset: 0x0010AD58
		public static Thing ClosestThingReachable(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, IEnumerable<Thing> customGlobalSearchSet = null, int searchRegionsMin = 0, int searchRegionsMax = -1, bool forceAllowGlobalSearch = false, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false)
		{
			bool flag = searchRegionsMax < 0 || forceAllowGlobalSearch;
			if (!flag && customGlobalSearchSet != null)
			{
				Log.ErrorOnce("searchRegionsMax >= 0 && customGlobalSearchSet != null && !forceAllowGlobalSearch. customGlobalSearchSet will never be used.", 634984);
			}
			if (!flag && !thingReq.IsUndefined && !thingReq.CanBeFoundInRegion)
			{
				Log.ErrorOnce("ClosestThingReachable with thing request group " + thingReq.group + " and global search not allowed. This will never find anything because this group is never stored in regions. Either allow global search or don't call this method at all.", 518498981);
				return null;
			}
			if (GenClosest.EarlyOutSearch(root, map, thingReq, customGlobalSearchSet, validator))
			{
				return null;
			}
			Thing thing = null;
			bool flag2 = false;
			if (!thingReq.IsUndefined && thingReq.CanBeFoundInRegion)
			{
				int num = (searchRegionsMax > 0) ? searchRegionsMax : 30;
				int num2;
				thing = GenClosest.RegionwiseBFSWorker(root, map, thingReq, peMode, traverseParams, validator, null, searchRegionsMin, num, maxDistance, out num2, traversableRegionTypes, ignoreEntirelyForbiddenRegions);
				flag2 = (thing == null && num2 < num);
			}
			if (thing == null && flag && !flag2)
			{
				if (traversableRegionTypes != RegionType.Set_Passable)
				{
					Log.ErrorOnce("ClosestThingReachable had to do a global search, but traversableRegionTypes is not set to passable only. It's not supported, because Reachability is based on passable regions only.", 14384767);
				}
				Predicate<Thing> validator2 = (Thing t) => map.reachability.CanReach(root, t, peMode, traverseParams) && (validator == null || validator(t));
				IEnumerable<Thing> searchSet = customGlobalSearchSet ?? map.listerThings.ThingsMatching(thingReq);
				thing = GenClosest.ClosestThing_Global(root, searchSet, maxDistance, validator2, null);
			}
			return thing;
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x0010CCC8 File Offset: 0x0010AEC8
		public static Thing ClosestThing_Regionwise_ReachablePrioritized(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null, int minRegions = 24, int maxRegions = 30)
		{
			if (!thingReq.IsUndefined && !thingReq.CanBeFoundInRegion)
			{
				Log.ErrorOnce("ClosestThing_Regionwise_ReachablePrioritized with thing request group " + thingReq.group + ". This will never find anything because this group is never stored in regions. Most likely a global search should have been used.", 738476712);
				return null;
			}
			if (GenClosest.EarlyOutSearch(root, map, thingReq, null, validator))
			{
				return null;
			}
			if (maxRegions < minRegions)
			{
				Log.ErrorOnce("maxRegions < minRegions", 754343);
			}
			Thing result = null;
			if (!thingReq.IsUndefined)
			{
				int num;
				result = GenClosest.RegionwiseBFSWorker(root, map, thingReq, peMode, traverseParams, validator, priorityGetter, minRegions, maxRegions, maxDistance, out num, RegionType.Set_Passable, false);
			}
			return result;
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x0010CD58 File Offset: 0x0010AF58
		public static Thing RegionwiseBFSWorker(IntVec3 root, Map map, ThingRequest req, PathEndMode peMode, TraverseParms traverseParams, Predicate<Thing> validator, Func<Thing, float> priorityGetter, int minRegions, int maxRegions, float maxDistance, out int regionsSeen, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false)
		{
			regionsSeen = 0;
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyableThings. Use ClosestThingGlobal.");
				return null;
			}
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyableThingsNotWater. Use ClosestThingGlobal.");
				return null;
			}
			if (!req.IsUndefined && !req.CanBeFoundInRegion)
			{
				Log.ErrorOnce("RegionwiseBFSWorker with thing request group " + req.group + ". This group is never stored in regions. Most likely a global search should have been used.", 385766189);
				return null;
			}
			Region region = root.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return null;
			}
			RegionProcessorClosestThingReachable regionProcessorClosestThingReachable = SimplePool<RegionProcessorClosestThingReachable>.Get();
			regionProcessorClosestThingReachable.SetParameters(traverseParams, maxDistance, root, ignoreEntirelyForbiddenRegions, req, peMode, priorityGetter, validator, minRegions, 9999999f, 0, float.MinValue, null);
			RegionTraverser.BreadthFirstTraverse(region, regionProcessorClosestThingReachable, maxRegions, traversableRegionTypes);
			regionsSeen = regionProcessorClosestThingReachable.regionsSeenScan;
			Thing closestThing = regionProcessorClosestThingReachable.closestThing;
			regionProcessorClosestThingReachable.Clear();
			SimplePool<RegionProcessorClosestThingReachable>.Return(regionProcessorClosestThingReachable);
			return closestThing;
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x0010CE2C File Offset: 0x0010B02C
		public static Thing ClosestThing_Global(IntVec3 center, IEnumerable searchSet, float maxDistance = 99999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null)
		{
			GenClosest.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.priorityGetter = priorityGetter;
			CS$<>8__locals1.validator = validator;
			if (searchSet == null)
			{
				return null;
			}
			CS$<>8__locals1.closestDistSquared = 2.1474836E+09f;
			CS$<>8__locals1.chosen = null;
			CS$<>8__locals1.bestPrio = float.MinValue;
			CS$<>8__locals1.maxDistanceSquared = maxDistance * maxDistance;
			IList<Thing> list;
			IList<Pawn> list2;
			IList<Building> list3;
			IList<IAttackTarget> list4;
			if ((list = (searchSet as IList<Thing>)) != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0(list[i], ref CS$<>8__locals1);
				}
			}
			else if ((list2 = (searchSet as IList<Pawn>)) != null)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0(list2[j], ref CS$<>8__locals1);
				}
			}
			else if ((list3 = (searchSet as IList<Building>)) != null)
			{
				for (int k = 0; k < list3.Count; k++)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0(list3[k], ref CS$<>8__locals1);
				}
			}
			else if ((list4 = (searchSet as IList<IAttackTarget>)) != null)
			{
				for (int l = 0; l < list4.Count; l++)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0((Thing)list4[l], ref CS$<>8__locals1);
				}
			}
			else
			{
				foreach (object obj in searchSet)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0((Thing)obj, ref CS$<>8__locals1);
				}
			}
			return CS$<>8__locals1.chosen;
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x0010CFA0 File Offset: 0x0010B1A0
		public static Thing ClosestThing_Global_Reachable(IntVec3 center, Map map, IEnumerable<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null)
		{
			GenClosest.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.priorityGetter = priorityGetter;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.peMode = peMode;
			CS$<>8__locals1.traverseParams = traverseParams;
			CS$<>8__locals1.validator = validator;
			if (searchSet == null)
			{
				return null;
			}
			CS$<>8__locals1.debug_changeCount = 0;
			CS$<>8__locals1.debug_scanCount = 0;
			CS$<>8__locals1.bestThing = null;
			CS$<>8__locals1.bestPrio = float.MinValue;
			CS$<>8__locals1.maxDistanceSquared = maxDistance * maxDistance;
			CS$<>8__locals1.closestDistSquared = 2.1474836E+09f;
			IList<Thing> list;
			IList<Pawn> list2;
			IList<Building> list3;
			if ((list = (searchSet as IList<Thing>)) != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(list[i], ref CS$<>8__locals1);
				}
			}
			else if ((list2 = (searchSet as IList<Pawn>)) != null)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(list2[j], ref CS$<>8__locals1);
				}
			}
			else if ((list3 = (searchSet as IList<Building>)) != null)
			{
				for (int k = 0; k < list3.Count; k++)
				{
					GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(list3[k], ref CS$<>8__locals1);
				}
			}
			else
			{
				foreach (Thing t in searchSet)
				{
					GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(t, ref CS$<>8__locals1);
				}
			}
			return CS$<>8__locals1.bestThing;
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x0010D0F8 File Offset: 0x0010B2F8
		[CompilerGenerated]
		internal static void <ClosestThing_Global>g__Process|5_0(Thing t, ref GenClosest.<>c__DisplayClass5_0 A_1)
		{
			if (!t.Spawned)
			{
				return;
			}
			float num = (float)(A_1.center - t.Position).LengthHorizontalSquared;
			if (num > A_1.maxDistanceSquared)
			{
				return;
			}
			if (A_1.priorityGetter != null || num < A_1.closestDistSquared)
			{
				if (A_1.validator != null && !A_1.validator(t))
				{
					return;
				}
				float num2 = 0f;
				if (A_1.priorityGetter != null)
				{
					num2 = A_1.priorityGetter(t);
					if (num2 < A_1.bestPrio)
					{
						return;
					}
					if (num2 == A_1.bestPrio && num >= A_1.closestDistSquared)
					{
						return;
					}
				}
				A_1.chosen = t;
				A_1.closestDistSquared = num;
				A_1.bestPrio = num2;
			}
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x0010D1A8 File Offset: 0x0010B3A8
		[CompilerGenerated]
		internal static void <ClosestThing_Global_Reachable>g__Process|6_0(Thing t, ref GenClosest.<>c__DisplayClass6_0 A_1)
		{
			if (t == null)
			{
				return;
			}
			if (!t.Spawned)
			{
				return;
			}
			int num = A_1.debug_scanCount;
			A_1.debug_scanCount = num + 1;
			float num2 = (float)(A_1.center - t.Position).LengthHorizontalSquared;
			if (num2 > A_1.maxDistanceSquared)
			{
				return;
			}
			if (A_1.priorityGetter != null || num2 < A_1.closestDistSquared)
			{
				if (!A_1.map.reachability.CanReach(A_1.center, t, A_1.peMode, A_1.traverseParams))
				{
					return;
				}
				if (A_1.validator != null && !A_1.validator(t))
				{
					return;
				}
				float num3 = 0f;
				if (A_1.priorityGetter != null)
				{
					num3 = A_1.priorityGetter(t);
					if (num3 < A_1.bestPrio)
					{
						return;
					}
					if (num3 == A_1.bestPrio && num2 >= A_1.closestDistSquared)
					{
						return;
					}
				}
				A_1.bestThing = t;
				A_1.closestDistSquared = num2;
				A_1.bestPrio = num3;
				num = A_1.debug_changeCount;
				A_1.debug_changeCount = num + 1;
			}
		}

		// Token: 0x04001B95 RID: 7061
		private const int DefaultLocalTraverseRegionsBeforeGlobal = 30;
	}
}

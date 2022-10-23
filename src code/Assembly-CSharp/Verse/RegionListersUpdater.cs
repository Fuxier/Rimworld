using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000245 RID: 581
	public static class RegionListersUpdater
	{
		// Token: 0x0600107B RID: 4219 RVA: 0x00060668 File Offset: 0x0005E868
		public static void DeregisterInRegions(Thing thing, Map map)
		{
			if (!ListerThings.EverListable(thing.def, ListerThingsUse.Region))
			{
				return;
			}
			RegionListersUpdater.GetTouchableRegions(thing, map, RegionListersUpdater.tmpRegions, true);
			for (int i = 0; i < RegionListersUpdater.tmpRegions.Count; i++)
			{
				ListerThings listerThings = RegionListersUpdater.tmpRegions[i].ListerThings;
				if (listerThings.Contains(thing))
				{
					listerThings.Remove(thing);
				}
			}
			RegionListersUpdater.tmpRegions.Clear();
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x000606D4 File Offset: 0x0005E8D4
		public static void RegisterInRegions(Thing thing, Map map)
		{
			if (!ListerThings.EverListable(thing.def, ListerThingsUse.Region))
			{
				return;
			}
			RegionListersUpdater.GetTouchableRegions(thing, map, RegionListersUpdater.tmpRegions, false);
			for (int i = 0; i < RegionListersUpdater.tmpRegions.Count; i++)
			{
				ListerThings listerThings = RegionListersUpdater.tmpRegions[i].ListerThings;
				if (!listerThings.Contains(thing))
				{
					listerThings.Add(thing);
				}
			}
			RegionListersUpdater.tmpRegions.Clear();
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00060740 File Offset: 0x0005E940
		public static void RegisterAllAt(IntVec3 c, Map map, HashSet<Thing> processedThings = null)
		{
			List<Thing> thingList = c.GetThingList(map);
			int count = thingList.Count;
			for (int i = 0; i < count; i++)
			{
				Thing thing = thingList[i];
				if (processedThings == null || processedThings.Add(thing))
				{
					RegionListersUpdater.RegisterInRegions(thing, map);
				}
			}
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00060784 File Offset: 0x0005E984
		public static void GetTouchableRegions(Thing thing, Map map, List<Region> outRegions, bool allowAdjacentEvenIfCantTouch = false)
		{
			outRegions.Clear();
			CellRect cellRect = thing.OccupiedRect();
			CellRect cellRect2 = cellRect;
			if (RegionListersUpdater.CanRegisterInAdjacentRegions(thing))
			{
				cellRect2 = cellRect2.ExpandedBy(1);
			}
			foreach (IntVec3 intVec in cellRect2)
			{
				if (intVec.InBounds(map))
				{
					Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(intVec);
					if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.type.Passable() && !outRegions.Contains(validRegionAt_NoRebuild))
					{
						if (cellRect.Contains(intVec))
						{
							outRegions.Add(validRegionAt_NoRebuild);
						}
						else if (allowAdjacentEvenIfCantTouch || ReachabilityImmediate.CanReachImmediate(intVec, thing, map, PathEndMode.Touch, null))
						{
							outRegions.Add(validRegionAt_NoRebuild);
						}
					}
				}
			}
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00002662 File Offset: 0x00000862
		private static bool CanRegisterInAdjacentRegions(Thing thing)
		{
			return true;
		}

		// Token: 0x04000E77 RID: 3703
		private static List<Region> tmpRegions = new List<Region>();
	}
}

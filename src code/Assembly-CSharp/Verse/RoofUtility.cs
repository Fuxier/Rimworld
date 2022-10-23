using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000256 RID: 598
	public static class RoofUtility
	{
		// Token: 0x06001120 RID: 4384 RVA: 0x00063FF0 File Offset: 0x000621F0
		public static Thing FirstBlockingThing(IntVec3 pos, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(pos);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.plant != null && list[i].def.plant.interferesWithRoof)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00064050 File Offset: 0x00062250
		public static bool IsAnyCellUnderRoof(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			bool result = false;
			RoofGrid roofGrid = thing.Map.roofGrid;
			foreach (IntVec3 c in cellRect)
			{
				if (roofGrid.Roofed(c))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x000640C0 File Offset: 0x000622C0
		public static bool CanHandleBlockingThing(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return true;
			}
			if (blocker.def.category == ThingCategory.Plant)
			{
				if (!PlantUtility.PawnWillingToCutPlant_Job(blocker, worker))
				{
					return false;
				}
				if (worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x000640FC File Offset: 0x000622FC
		public static Job HandleBlockingThingJob(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return null;
			}
			if (blocker.def.category != ThingCategory.Plant || !worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
			{
				return null;
			}
			if (!PlantUtility.PawnWillingToCutPlant_Job(blocker, worker))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.CutPlant, blocker);
		}
	}
}

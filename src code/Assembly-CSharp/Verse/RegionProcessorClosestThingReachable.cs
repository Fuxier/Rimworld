using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200055F RID: 1375
	public class RegionProcessorClosestThingReachable : RegionProcessorDelegateCache
	{
		// Token: 0x06002A2D RID: 10797 RVA: 0x0010D2AC File Offset: 0x0010B4AC
		public void SetParameters(TraverseParms traverseParams, float maxDistance, IntVec3 root, bool ignoreEntirelyForbiddenRegions, ThingRequest req, PathEndMode peMode, Func<Thing, float> priorityGetter, Predicate<Thing> validator, int minRegions, float closestDistSquared = 9999999f, int regionsSeenScan = 0, float bestPrio = -3.4028235E+38f, Thing closestThing = null)
		{
			this.traverseParams = traverseParams;
			this.maxDistance = maxDistance;
			this.root = root;
			this.regionsSeenScan = regionsSeenScan;
			this.ignoreEntirelyForbiddenRegions = ignoreEntirelyForbiddenRegions;
			this.req = req;
			this.peMode = peMode;
			this.priorityGetter = priorityGetter;
			this.validator = validator;
			this.bestPrio = bestPrio;
			this.closestDistSquared = closestDistSquared;
			this.closestThing = closestThing;
			this.minRegions = minRegions;
			this.maxDistSquared = maxDistance * maxDistance;
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x0010D328 File Offset: 0x0010B528
		public void Clear()
		{
			this.SetParameters(default(TraverseParms), 0f, default(IntVec3), false, default(ThingRequest), PathEndMode.None, null, null, 0, 0f, 0, 0f, null);
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x0010D36C File Offset: 0x0010B56C
		protected override bool RegionEntryPredicate(Region from, Region to)
		{
			return to.Allows(this.traverseParams, false) && (this.maxDistance > 5000f || to.extentsClose.ClosestDistSquaredTo(this.root) < this.maxDistSquared);
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x0010D3A8 File Offset: 0x0010B5A8
		protected override bool RegionProcessor(Region reg)
		{
			if (RegionTraverser.ShouldCountRegion(reg))
			{
				this.regionsSeenScan++;
			}
			if (!reg.IsDoorway && !reg.Allows(this.traverseParams, true))
			{
				return false;
			}
			if (!this.ignoreEntirelyForbiddenRegions || !reg.IsForbiddenEntirely(this.traverseParams.pawn))
			{
				List<Thing> list = reg.ListerThings.ThingsMatching(this.req);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, reg, this.peMode, this.traverseParams.pawn))
					{
						float num = (this.priorityGetter != null) ? this.priorityGetter(thing) : 0f;
						if (num >= this.bestPrio)
						{
							float num2 = (float)(thing.Position - this.root).LengthHorizontalSquared;
							if ((num > this.bestPrio || num2 < this.closestDistSquared) && num2 < this.maxDistSquared && (this.validator == null || this.validator(thing)))
							{
								this.closestThing = thing;
								this.closestDistSquared = num2;
								this.bestPrio = num;
							}
						}
					}
				}
			}
			return this.regionsSeenScan >= this.minRegions && this.closestThing != null;
		}

		// Token: 0x04001B96 RID: 7062
		private TraverseParms traverseParams;

		// Token: 0x04001B97 RID: 7063
		private float maxDistance;

		// Token: 0x04001B98 RID: 7064
		private IntVec3 root;

		// Token: 0x04001B99 RID: 7065
		public Thing closestThing;

		// Token: 0x04001B9A RID: 7066
		public int regionsSeenScan;

		// Token: 0x04001B9B RID: 7067
		private bool ignoreEntirelyForbiddenRegions;

		// Token: 0x04001B9C RID: 7068
		private ThingRequest req;

		// Token: 0x04001B9D RID: 7069
		private PathEndMode peMode;

		// Token: 0x04001B9E RID: 7070
		private Func<Thing, float> priorityGetter;

		// Token: 0x04001B9F RID: 7071
		private Predicate<Thing> validator;

		// Token: 0x04001BA0 RID: 7072
		private float bestPrio;

		// Token: 0x04001BA1 RID: 7073
		private float closestDistSquared;

		// Token: 0x04001BA2 RID: 7074
		private int minRegions;

		// Token: 0x04001BA3 RID: 7075
		private float maxDistSquared;
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200024A RID: 586
	public static class RegionTraverser
	{
		// Token: 0x06001098 RID: 4248 RVA: 0x00060F64 File Offset: 0x0005F164
		public static District FloodAndSetDistricts(Region root, Map map, District existingRoom)
		{
			District floodingDistrict;
			if (existingRoom == null)
			{
				floodingDistrict = District.MakeNew(map);
			}
			else
			{
				floodingDistrict = existingRoom;
			}
			root.District = floodingDistrict;
			if (!root.type.AllowsMultipleRegionsPerDistrict())
			{
				return floodingDistrict;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.District != floodingDistrict;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.District = floodingDistrict;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
			return floodingDistrict;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00060FF4 File Offset: 0x0005F1F4
		public static void FloodAndSetNewRegionIndex(Region root, int newRegionGroupIndex)
		{
			root.newRegionGroupIndex = newRegionGroupIndex;
			if (!root.type.AllowsMultipleRegionsPerDistrict())
			{
				return;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.newRegionGroupIndex < 0;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.newRegionGroupIndex = newRegionGroupIndex;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00061068 File Offset: 0x0005F268
		public static bool WithinRegions(this IntVec3 A, IntVec3 B, Map map, int regionLookCount, TraverseParms traverseParams, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = A.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return false;
			}
			Region regB = B.GetRegion(map, traversableRegionTypes);
			if (regB == null)
			{
				return false;
			}
			if (region == regB)
			{
				return true;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParams, false);
			bool found = false;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r == regB)
				{
					found = true;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, regionLookCount, traversableRegionTypes);
			return found;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x000610E8 File Offset: 0x0005F2E8
		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00061118 File Offset: 0x0005F318
		public static bool ShouldCountRegion(Region r)
		{
			return !r.IsDoorway;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00061123 File Offset: 0x0005F323
		static RegionTraverser()
		{
			RegionTraverser.RecreateWorkers();
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00061150 File Offset: 0x0005F350
		public static void RecreateWorkers()
		{
			RegionTraverser.freeWorkers.Clear();
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00061188 File Offset: 0x0005F388
		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x000611AF File Offset: 0x0005F3AF
		public static void BreadthFirstTraverse(Region root, RegionProcessorDelegateCache processor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, processor.RegionEntryPredicateDelegate, processor.RegionProcessorDelegate, maxRegions, traversableRegionTypes);
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x000611C8 File Offset: 0x0005F3C8
		public static void BreadthFirstTraverse(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (RegionTraverser.freeWorkers.Count == 0)
			{
				Log.Error("No free workers for breadth-first traversal. Either BFS recurred deeper than " + RegionTraverser.NumWorkers + ", or a bug has put this system in an inconsistent state. Resetting.");
				return;
			}
			if (root == null)
			{
				Log.Error("BreadthFirstTraverse with null root region.");
				return;
			}
			RegionTraverser.BFSWorker bfsworker = RegionTraverser.freeWorkers.Dequeue();
			try
			{
				bfsworker.BreadthFirstTraverseWork(root, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			}
			catch (Exception ex)
			{
				Log.Error("Exception in BreadthFirstTraverse: " + ex.ToString());
			}
			finally
			{
				bfsworker.Clear();
				RegionTraverser.freeWorkers.Enqueue(bfsworker);
			}
		}

		// Token: 0x04000E81 RID: 3713
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		// Token: 0x04000E82 RID: 3714
		public static int NumWorkers = 8;

		// Token: 0x04000E83 RID: 3715
		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		// Token: 0x02001D9C RID: 7580
		private class BFSWorker
		{
			// Token: 0x0600B535 RID: 46389 RVA: 0x00412C30 File Offset: 0x00410E30
			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			// Token: 0x0600B536 RID: 46390 RVA: 0x00412C51 File Offset: 0x00410E51
			public void Clear()
			{
				this.open.Clear();
			}

			// Token: 0x0600B537 RID: 46391 RVA: 0x00412C60 File Offset: 0x00410E60
			private void QueueNewOpenRegion(Region region)
			{
				if (region.closedIndex[this.closedArrayPos] == this.closedIndex)
				{
					throw new InvalidOperationException("Region is already closed; you can't open it. Region: " + region.ToString());
				}
				this.open.Enqueue(region);
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			// Token: 0x0600B538 RID: 46392 RVA: 0x000034B7 File Offset: 0x000016B7
			private void FinalizeSearch()
			{
			}

			// Token: 0x0600B539 RID: 46393 RVA: 0x00412CB8 File Offset: 0x00410EB8
			public void BreadthFirstTraverseWork(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions, RegionType traversableRegionTypes)
			{
				if ((root.type & traversableRegionTypes) == RegionType.None)
				{
					return;
				}
				this.closedIndex += 1U;
				this.open.Clear();
				this.numRegionsProcessed = 0;
				this.QueueNewOpenRegion(root);
				while (this.open.Count > 0)
				{
					Region region = this.open.Dequeue();
					if (DebugViewSettings.drawRegionTraversal)
					{
						region.Debug_Notify_Traversed();
					}
					if (regionProcessor != null && regionProcessor(region))
					{
						this.FinalizeSearch();
						return;
					}
					if (RegionTraverser.ShouldCountRegion(region))
					{
						this.numRegionsProcessed++;
					}
					if (this.numRegionsProcessed >= maxRegions)
					{
						this.FinalizeSearch();
						return;
					}
					for (int i = 0; i < region.links.Count; i++)
					{
						RegionLink regionLink = region.links[i];
						for (int j = 0; j < 2; j++)
						{
							Region region2 = regionLink.regions[j];
							if (region2 != null && region2.closedIndex[this.closedArrayPos] != this.closedIndex && (region2.type & traversableRegionTypes) != RegionType.None && (entryCondition == null || entryCondition(region, region2)))
							{
								this.QueueNewOpenRegion(region2);
							}
						}
					}
				}
				this.FinalizeSearch();
			}

			// Token: 0x040074E2 RID: 29922
			private Queue<Region> open = new Queue<Region>();

			// Token: 0x040074E3 RID: 29923
			private int numRegionsProcessed;

			// Token: 0x040074E4 RID: 29924
			private uint closedIndex = 1U;

			// Token: 0x040074E5 RID: 29925
			private int closedArrayPos;

			// Token: 0x040074E6 RID: 29926
			private const int skippableRegionSize = 4;
		}
	}
}

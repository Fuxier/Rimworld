using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001A1 RID: 417
	public abstract class AnimalPenEnclosureCalculator
	{
		// Token: 0x06000B79 RID: 2937 RVA: 0x00040F0B File Offset: 0x0003F10B
		protected AnimalPenEnclosureCalculator()
		{
			this.regionProcessor = new RegionProcessor(this.ProcessRegion);
			this.regionEntryPredicate = new RegionEntryPredicate(this.EnterRegion);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x000034B7 File Offset: 0x000016B7
		protected virtual void VisitDirectlyConnectedRegion(Region r)
		{
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000034B7 File Offset: 0x000016B7
		protected virtual void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x000034B7 File Offset: 0x000016B7
		protected virtual void VisitPassableDoorway(Region r)
		{
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x000034B7 File Offset: 0x000016B7
		protected virtual void VisitImpassableDoorway(Region r)
		{
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00040F38 File Offset: 0x0003F138
		protected bool VisitPen(IntVec3 position, Map map)
		{
			this.rootDistrict = position.GetDistrict(map, RegionType.Set_Passable);
			if (this.rootDistrict == null || this.rootDistrict.TouchesMapEdge)
			{
				return false;
			}
			this.isEnclosed = true;
			RegionTraverser.BreadthFirstTraverse(position, map, this.regionEntryPredicate, this.regionProcessor, 999999, RegionType.Set_Passable);
			return this.isEnclosed;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00040F92 File Offset: 0x0003F192
		public static bool RoamerCanPass(Building_Door door)
		{
			return door.FreePassage || AnimalPenEnclosureCalculator.RoamerCanPass(door.def);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00040FA9 File Offset: 0x0003F1A9
		public static bool RoamerCanPass(ThingDef thingDef)
		{
			return thingDef.building.roamerCanOpen;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00040FB6 File Offset: 0x0003F1B6
		private bool EnterRegion(Region from, Region to)
		{
			return (!from.IsDoorway || AnimalPenEnclosureCalculator.RoamerCanPass(from.door)) && (to.type == RegionType.Normal || to.IsDoorway);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00040FE0 File Offset: 0x0003F1E0
		private bool ProcessRegion(Region reg)
		{
			if (reg.touchesMapEdge)
			{
				this.isEnclosed = false;
				return true;
			}
			if (reg.type == RegionType.Normal)
			{
				if (reg.District == this.rootDistrict)
				{
					this.VisitDirectlyConnectedRegion(reg);
				}
				else
				{
					this.VisitIndirectlyDirectlyConnectedRegion(reg);
				}
			}
			else if (reg.IsDoorway)
			{
				if (AnimalPenEnclosureCalculator.RoamerCanPass(reg.door))
				{
					this.VisitPassableDoorway(reg);
				}
				else
				{
					this.VisitImpassableDoorway(reg);
				}
			}
			return false;
		}

		// Token: 0x04000AE0 RID: 2784
		private District rootDistrict;

		// Token: 0x04000AE1 RID: 2785
		private bool isEnclosed;

		// Token: 0x04000AE2 RID: 2786
		private readonly RegionProcessor regionProcessor;

		// Token: 0x04000AE3 RID: 2787
		private readonly RegionEntryPredicate regionEntryPredicate;
	}
}

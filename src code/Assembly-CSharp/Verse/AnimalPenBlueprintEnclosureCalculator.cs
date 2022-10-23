using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001A4 RID: 420
	public class AnimalPenBlueprintEnclosureCalculator
	{
		// Token: 0x06000B9A RID: 2970 RVA: 0x0004132A File Offset: 0x0003F52A
		public AnimalPenBlueprintEnclosureCalculator()
		{
			this.passCheck = new Predicate<IntVec3>(this.PassCheck);
			this.cellProcessor = new Func<IntVec3, bool>(this.CellProcessor);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00041364 File Offset: 0x0003F564
		public void VisitPen(IntVec3 position, Map map)
		{
			int num = Gen.HashCombineInt(map.listerThings.StateHashOfGroup(ThingRequestGroup.Blueprint), map.listerThings.StateHashOfGroup(ThingRequestGroup.BuildingFrame), map.listerThings.StateHashOfGroup(ThingRequestGroup.BuildingArtificial), 42);
			if (this.map == null || this.map != map || !this.last_position.Equals(position) || this.last_stateHash != num)
			{
				this.map = map;
				this.last_position = position;
				this.last_stateHash = num;
				this.isEnclosed = true;
				this.cellsFound.Clear();
				this.FloodFill(position);
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x000413FB File Offset: 0x0003F5FB
		private void FloodFill(IntVec3 position)
		{
			this.map.floodFiller.FloodFill(position, this.passCheck, this.cellProcessor, int.MaxValue, false, null);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00041421 File Offset: 0x0003F621
		private bool CellProcessor(IntVec3 c)
		{
			this.cellsFound.Add(c);
			if (c.OnEdge(this.map))
			{
				this.isEnclosed = false;
				return true;
			}
			return false;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00041448 File Offset: 0x0003F648
		private bool PassCheck(IntVec3 c)
		{
			if (!c.WalkableByFenceBlocked(this.map))
			{
				return false;
			}
			foreach (Thing thing in c.GetThingList(this.map))
			{
				ThingDef def = thing.def;
				if (def.passability == Traversability.Impassable)
				{
					return false;
				}
				Building_Door door;
				ThingDef thingDef;
				if ((door = (thing as Building_Door)) != null)
				{
					if (AnimalPenEnclosureCalculator.RoamerCanPass(door))
					{
						return true;
					}
					return false;
				}
				else if ((def.IsBlueprint || def.IsFrame) && (thingDef = (def.entityDefToBuild as ThingDef)) != null)
				{
					if (thingDef.IsFence || thingDef.passability == Traversability.Impassable)
					{
						return false;
					}
					if (thingDef.IsDoor)
					{
						if (AnimalPenEnclosureCalculator.RoamerCanPass(thingDef))
						{
							return true;
						}
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04000AED RID: 2797
		private readonly Predicate<IntVec3> passCheck;

		// Token: 0x04000AEE RID: 2798
		private readonly Func<IntVec3, bool> cellProcessor;

		// Token: 0x04000AEF RID: 2799
		public bool isEnclosed;

		// Token: 0x04000AF0 RID: 2800
		public List<IntVec3> cellsFound = new List<IntVec3>();

		// Token: 0x04000AF1 RID: 2801
		private Map map;

		// Token: 0x04000AF2 RID: 2802
		private IntVec3 last_position;

		// Token: 0x04000AF3 RID: 2803
		private int last_stateHash;
	}
}

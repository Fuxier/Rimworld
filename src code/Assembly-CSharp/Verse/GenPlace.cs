using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000570 RID: 1392
	public static class GenPlace
	{
		// Token: 0x06002AD4 RID: 10964 RVA: 0x00111750 File Offset: 0x0010F950
		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, Rot4 rot = default(Rot4))
		{
			Thing thing2;
			return GenPlace.TryPlaceThing(thing, center, map, mode, out thing2, placedAction, nearPlaceValidator, rot);
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x00111770 File Offset: 0x0010F970
		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, Rot4 rot = default(Rot4))
		{
			if (map == null)
			{
				Log.Error("Tried to place thing " + thing + " in a null map.");
				lastResultingThing = null;
				return false;
			}
			if (thing.def.category == ThingCategory.Filth)
			{
				mode = ThingPlaceMode.Direct;
			}
			if (mode == ThingPlaceMode.Direct)
			{
				return GenPlace.TryPlaceDirect(thing, center, rot, map, out lastResultingThing, placedAction);
			}
			if (mode == ThingPlaceMode.Near)
			{
				lastResultingThing = null;
				for (;;)
				{
					int stackCount = thing.stackCount;
					IntVec3 loc;
					if (!GenPlace.TryFindPlaceSpotNear(center, rot, map, thing, true, out loc, nearPlaceValidator))
					{
						break;
					}
					if (GenPlace.TryPlaceDirect(thing, loc, rot, map, out lastResultingThing, placedAction))
					{
						return true;
					}
					if (thing.stackCount == stackCount)
					{
						goto Block_7;
					}
				}
				return false;
				Block_7:
				Log.Error(string.Concat(new object[]
				{
					"Failed to place ",
					thing,
					" at ",
					center,
					" in mode ",
					mode,
					"."
				}));
				lastResultingThing = null;
				return false;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x00111854 File Offset: 0x0010FA54
		private static bool TryFindPlaceSpotNear(IntVec3 center, Rot4 rot, Map map, Thing thing, bool allowStacking, out IntVec3 bestSpot, Predicate<IntVec3> extraValidator = null)
		{
			GenPlace.PlaceSpotQuality placeSpotQuality = GenPlace.PlaceSpotQuality.Unusable;
			bestSpot = center;
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			if (placeSpotQuality >= GenPlace.PlaceSpotQuality.Okay)
			{
				return true;
			}
			for (int j = 0; j < GenPlace.PlaceNearMiddleRadialCells; j++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[j];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			if (placeSpotQuality >= GenPlace.PlaceSpotQuality.Okay)
			{
				return true;
			}
			for (int k = 0; k < GenPlace.PlaceNearMaxRadialCells; k++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[k];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			if (placeSpotQuality > GenPlace.PlaceSpotQuality.Unusable)
			{
				return true;
			}
			bestSpot = center;
			return false;
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x00111958 File Offset: 0x0010FB58
		private static GenPlace.PlaceSpotQuality PlaceSpotQualityAt(IntVec3 c, Rot4 rot, Map map, Thing thing, IntVec3 center, bool allowStacking, Predicate<IntVec3> extraValidator = null)
		{
			if (!c.InBounds(map) || !c.Walkable(map))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			if (!GenAdj.OccupiedRect(c, rot, thing.def.Size).InBounds(map))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			if (extraValidator != null && !extraValidator(c))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			bool flag = false;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing.def.saveCompressible && thing2.def.saveCompressible)
				{
					return GenPlace.PlaceSpotQuality.Unusable;
				}
				if (thing.def.category == ThingCategory.Item && thing2.def.category == ThingCategory.Item && allowStacking && thing2.stackCount < thing2.def.stackLimit && thing2.CanStackWith(thing))
				{
					flag = true;
				}
			}
			if (thing.def.category == ThingCategory.Item && !flag && c.GetItemCount(map) >= c.GetMaxItemsAllowedInCell(map))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			if (thing is Building)
			{
				foreach (IntVec3 c2 in GenAdj.OccupiedRect(c, rot, thing.def.size))
				{
					Building edifice = c2.GetEdifice(map);
					if (edifice != null && GenSpawn.SpawningWipes(thing.def, edifice.def))
					{
						return GenPlace.PlaceSpotQuality.Awful;
					}
				}
			}
			if (c.GetRoom(map) == center.GetRoom(map))
			{
				if (allowStacking)
				{
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing3 = list[j];
						if (thing3.def.category == ThingCategory.Item && thing3.CanStackWith(thing) && thing3.stackCount < thing3.def.stackLimit)
						{
							return GenPlace.PlaceSpotQuality.Perfect;
						}
					}
				}
				Pawn pawn = thing as Pawn;
				bool flag2 = pawn != null && pawn.Downed;
				GenPlace.PlaceSpotQuality placeSpotQuality = GenPlace.PlaceSpotQuality.Perfect;
				for (int k = 0; k < list.Count; k++)
				{
					Thing thing4 = list[k];
					if (thing4.def.IsDoor)
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					if (thing4 is Building_WorkTable)
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					Pawn pawn2;
					if ((pawn2 = (thing4 as Pawn)) != null && (pawn2.Downed || flag2))
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					if (thing4.def.category == ThingCategory.Plant && thing4.def.selectable && placeSpotQuality > GenPlace.PlaceSpotQuality.Okay)
					{
						placeSpotQuality = GenPlace.PlaceSpotQuality.Okay;
					}
				}
				return placeSpotQuality;
			}
			if (!map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly))
			{
				return GenPlace.PlaceSpotQuality.Awful;
			}
			return GenPlace.PlaceSpotQuality.Bad;
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x00111BF4 File Offset: 0x0010FDF4
		private static bool SplitAndSpawnOneStackOnCell(Thing thing, IntVec3 loc, Rot4 rot, Map map, out Thing resultingThing, Action<Thing, int> placedAction)
		{
			Thing thing2;
			if (thing.stackCount > thing.def.stackLimit)
			{
				thing2 = thing.SplitOff(thing.def.stackLimit);
			}
			else
			{
				thing2 = thing;
			}
			resultingThing = GenSpawn.Spawn(thing2, loc, map, rot, WipeMode.Vanish, false);
			if (placedAction != null)
			{
				placedAction(thing2, thing2.stackCount);
			}
			return thing2 == thing;
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x00111C50 File Offset: 0x0010FE50
		private static bool TryPlaceDirect(Thing thing, IntVec3 loc, Rot4 rot, Map map, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			resultingThing = null;
			GenPlace.cellThings.Clear();
			GenPlace.cellThings.AddRange(loc.GetThingList(map));
			GenPlace.cellThings.Sort((Thing lhs, Thing rhs) => rhs.stackCount.CompareTo(lhs.stackCount));
			if (thing.def.stackLimit > 1)
			{
				for (int i = 0; i < GenPlace.cellThings.Count; i++)
				{
					Thing thing2 = GenPlace.cellThings[i];
					if (thing2.CanStackWith(thing))
					{
						int stackCount = thing.stackCount;
						if (thing2.TryAbsorbStack(thing, true))
						{
							resultingThing = thing2;
							if (placedAction != null)
							{
								placedAction(thing2, stackCount);
							}
							return true;
						}
						if (placedAction != null && stackCount != thing.stackCount)
						{
							placedAction(thing2, stackCount - thing.stackCount);
						}
					}
				}
			}
			int num2;
			if (thing.def.category == ThingCategory.Item)
			{
				int num = GenPlace.cellThings.Count((Thing cellThing) => cellThing.def.category == ThingCategory.Item);
				num2 = loc.GetMaxItemsAllowedInCell(map) - num;
			}
			else
			{
				num2 = thing.stackCount + 1;
			}
			if (num2 <= 0 && thing.def.stackLimit <= 1)
			{
				num2 = 1;
			}
			for (int j = 0; j < num2; j++)
			{
				if (GenPlace.SplitAndSpawnOneStackOnCell(thing, loc, rot, map, out resultingThing, placedAction))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x00111DA4 File Offset: 0x0010FFA4
		public static Thing HaulPlaceBlockerIn(Thing haulThing, IntVec3 c, Map map, bool checkBlueprintsAndFrames)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (checkBlueprintsAndFrames && (thing.def.IsBlueprint || thing.def.IsFrame))
				{
					return thing;
				}
				if ((thing.def.category != ThingCategory.Plant || thing.def.passability != Traversability.Standable) && thing.def.category != ThingCategory.Filth && (haulThing == null || thing.def.category != ThingCategory.Item || !thing.CanStackWith(haulThing) || thing.def.stackLimit - thing.stackCount < haulThing.stackCount))
				{
					if (thing.def.EverHaulable)
					{
						return thing;
					}
					if (haulThing != null && GenSpawn.SpawningWipes(haulThing.def, thing.def))
					{
						return thing;
					}
					if (thing.def.passability != Traversability.Standable && thing.def.surfaceType != SurfaceType.Item)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x04001BFD RID: 7165
		private static readonly int PlaceNearMaxRadialCells = GenRadial.NumCellsInRadius(12.9f);

		// Token: 0x04001BFE RID: 7166
		private static readonly int PlaceNearMiddleRadialCells = GenRadial.NumCellsInRadius(3f);

		// Token: 0x04001BFF RID: 7167
		private static List<Thing> cellThings = new List<Thing>(8);

		// Token: 0x02002138 RID: 8504
		private enum PlaceSpotQuality : byte
		{
			// Token: 0x040083CD RID: 33741
			Unusable,
			// Token: 0x040083CE RID: 33742
			Awful,
			// Token: 0x040083CF RID: 33743
			Bad,
			// Token: 0x040083D0 RID: 33744
			Okay,
			// Token: 0x040083D1 RID: 33745
			Perfect
		}
	}
}

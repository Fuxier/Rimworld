using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000572 RID: 1394
	public static class GenSpawn
	{
		// Token: 0x06002ADC RID: 10972 RVA: 0x00111ECC File Offset: 0x001100CC
		public static Thing Spawn(ThingDef def, IntVec3 loc, Map map, WipeMode wipeMode = WipeMode.Vanish)
		{
			return GenSpawn.Spawn(ThingMaker.MakeThing(def, null), loc, map, wipeMode);
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x00111EDD File Offset: 0x001100DD
		public static Thing Spawn(Thing newThing, IntVec3 loc, Map map, WipeMode wipeMode = WipeMode.Vanish)
		{
			return GenSpawn.Spawn(newThing, loc, map, Rot4.North, wipeMode, false);
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x00111EF0 File Offset: 0x001100F0
		public static Thing Spawn(Thing newThing, IntVec3 loc, Map map, Rot4 rot, WipeMode wipeMode = WipeMode.Vanish, bool respawningAfterLoad = false)
		{
			if (map == null)
			{
				Log.Error("Tried to spawn " + newThing.ToStringSafe<Thing>() + " in a null map.");
				return null;
			}
			if (!loc.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to spawn ",
					newThing.ToStringSafe<Thing>(),
					" out of bounds at ",
					loc,
					"."
				}));
				return null;
			}
			if (newThing.def.randomizeRotationOnSpawn)
			{
				rot = Rot4.Random;
			}
			CellRect occupiedRect = GenAdj.OccupiedRect(loc, rot, newThing.def.Size);
			if (!occupiedRect.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to spawn ",
					newThing.ToStringSafe<Thing>(),
					" out of bounds at ",
					loc,
					" (out of bounds because size is ",
					newThing.def.Size,
					")."
				}));
				return null;
			}
			if (newThing.Spawned)
			{
				Log.Error("Tried to spawn " + newThing + " but it's already spawned.");
				return newThing;
			}
			if (wipeMode == WipeMode.Vanish)
			{
				GenSpawn.WipeExistingThings(loc, rot, newThing.def, map, DestroyMode.Vanish);
			}
			else if (wipeMode == WipeMode.FullRefund)
			{
				GenSpawn.WipeAndRefundExistingThings(loc, rot, newThing.def, map);
			}
			else if (wipeMode == WipeMode.VanishOrMoveAside)
			{
				GenSpawn.CheckMoveItemsAside(loc, rot, newThing.def, map);
				GenSpawn.WipeExistingThings(loc, rot, newThing.def, map, DestroyMode.Vanish);
			}
			if (newThing.def.category == ThingCategory.Item && Current.ProgramState == ProgramState.Playing && loc.GetItemCount(map) >= loc.GetMaxItemsAllowedInCell(map))
			{
				Predicate<IntVec3> <>9__0;
				foreach (Thing thing in loc.GetThingList(map).ToList<Thing>())
				{
					if (thing != newThing && thing.def.category == ThingCategory.Item)
					{
						thing.DeSpawn(DestroyMode.Vanish);
						Thing thing2 = thing;
						ThingPlaceMode mode = ThingPlaceMode.Near;
						Action<Thing, int> placedAction = null;
						Predicate<IntVec3> nearPlaceValidator;
						if ((nearPlaceValidator = <>9__0) == null)
						{
							nearPlaceValidator = (<>9__0 = ((IntVec3 x) => !occupiedRect.Contains(x)));
						}
						if (!GenPlace.TryPlaceThing(thing2, loc, map, mode, placedAction, nearPlaceValidator, default(Rot4)))
						{
							thing.Destroy(DestroyMode.Vanish);
							break;
						}
						break;
					}
				}
			}
			newThing.Rotation = rot;
			newThing.Position = loc;
			if (newThing.holdingOwner != null)
			{
				newThing.holdingOwner.Remove(newThing);
			}
			newThing.SpawnSetup(map, respawningAfterLoad);
			if (newThing.Spawned && newThing.stackCount == 0)
			{
				Log.Error("Spawned thing with 0 stackCount: " + newThing);
				newThing.Destroy(DestroyMode.Vanish);
				return null;
			}
			if (newThing.def.passability == Traversability.Impassable)
			{
				foreach (IntVec3 c in occupiedRect)
				{
					foreach (Thing thing3 in c.GetThingList(map).ToList<Thing>())
					{
						if (thing3 != newThing)
						{
							Pawn pawn = thing3 as Pawn;
							if (pawn != null)
							{
								pawn.pather.TryRecoverFromUnwalkablePosition(false);
							}
						}
					}
				}
			}
			return newThing;
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x00112234 File Offset: 0x00110434
		public static void SpawnBuildingAsPossible(Building building, Map map, bool respawningAfterLoad = false)
		{
			bool flag = false;
			if (!building.OccupiedRect().InBounds(map))
			{
				flag = true;
			}
			else
			{
				foreach (IntVec3 c in building.OccupiedRect())
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] is Pawn && building.def.passability == Traversability.Impassable)
						{
							flag = true;
							break;
						}
						if ((thingList[i].def.category == ThingCategory.Building || thingList[i].def.category == ThingCategory.Item) && GenSpawn.SpawningWipes(building.def, thingList[i].def))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				GenSpawn.Refund(building, map, CellRect.Empty);
				return;
			}
			GenSpawn.Spawn(building, building.Position, map, building.Rotation, WipeMode.FullRefund, respawningAfterLoad);
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x00112354 File Offset: 0x00110554
		public static void Refund(Thing thing, Map map, CellRect avoidThisRect)
		{
			bool flag = false;
			if (thing.def.Minifiable)
			{
				MinifiedThing minifiedThing = thing.MakeMinified();
				if (GenPlace.TryPlaceThing(minifiedThing, thing.Position, map, ThingPlaceMode.Near, null, (IntVec3 x) => !avoidThisRect.Contains(x), default(Rot4)))
				{
					flag = true;
				}
				else
				{
					minifiedThing.GetDirectlyHeldThings().Clear();
					minifiedThing.Destroy(DestroyMode.Vanish);
				}
			}
			if (!flag)
			{
				GenLeaving.DoLeavingsFor(thing, map, DestroyMode.Refund, thing.OccupiedRect(), (IntVec3 x) => !avoidThisRect.Contains(x), null);
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x001123E8 File Offset: 0x001105E8
		public static void WipeExistingThings(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map, DestroyMode mode)
		{
			foreach (IntVec3 c in GenAdj.CellsOccupiedBy(thingPos, thingRot, thingDef.Size))
			{
				foreach (Thing thing in map.thingGrid.ThingsAt(c).ToList<Thing>())
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def))
					{
						thing.Destroy(mode);
					}
				}
			}
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x00112494 File Offset: 0x00110694
		public static void WipeAndRefundExistingThings(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map)
		{
			CellRect occupiedRect = GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size);
			Predicate<IntVec3> <>9__0;
			foreach (IntVec3 intVec in occupiedRect)
			{
				foreach (Thing thing in intVec.GetThingList(map).ToList<Thing>())
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def))
					{
						if (thing.def.category == ThingCategory.Item)
						{
							thing.DeSpawn(DestroyMode.Vanish);
							Thing thing2 = thing;
							IntVec3 center = intVec;
							ThingPlaceMode mode = ThingPlaceMode.Near;
							Action<Thing, int> placedAction = null;
							Predicate<IntVec3> nearPlaceValidator;
							if ((nearPlaceValidator = <>9__0) == null)
							{
								nearPlaceValidator = (<>9__0 = ((IntVec3 x) => !occupiedRect.Contains(x)));
							}
							if (!GenPlace.TryPlaceThing(thing2, center, map, mode, placedAction, nearPlaceValidator, default(Rot4)))
							{
								thing.Destroy(DestroyMode.Vanish);
							}
						}
						else
						{
							GenSpawn.Refund(thing, map, occupiedRect);
						}
					}
				}
			}
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x001125BC File Offset: 0x001107BC
		public static void CheckMoveItemsAside(IntVec3 thingPos, Rot4 thingRot, ThingDef thingDef, Map map)
		{
			if (thingDef.surfaceType != SurfaceType.None || thingDef.passability == Traversability.Standable)
			{
				return;
			}
			CellRect occupiedRect = GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size);
			Predicate<IntVec3> <>9__0;
			foreach (IntVec3 intVec in occupiedRect)
			{
				foreach (Thing thing in intVec.GetThingList(map).ToList<Thing>())
				{
					if (thing.def.category == ThingCategory.Item)
					{
						thing.DeSpawn(DestroyMode.Vanish);
						Thing thing2 = thing;
						IntVec3 center = intVec;
						ThingPlaceMode mode = ThingPlaceMode.Near;
						Action<Thing, int> placedAction = null;
						Predicate<IntVec3> nearPlaceValidator;
						if ((nearPlaceValidator = <>9__0) == null)
						{
							nearPlaceValidator = (<>9__0 = ((IntVec3 x) => !occupiedRect.Contains(x)));
						}
						if (!GenPlace.TryPlaceThing(thing2, center, map, mode, placedAction, nearPlaceValidator, default(Rot4)))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x001126D4 File Offset: 0x001108D4
		public static bool WouldWipeAnythingWith(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map, Predicate<Thing> predicate)
		{
			return GenSpawn.WouldWipeAnythingWith(GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size), thingDef, map, predicate);
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x001126EC File Offset: 0x001108EC
		public static bool WouldWipeAnythingWith(CellRect cellRect, BuildableDef thingDef, Map map, Predicate<Thing> predicate)
		{
			foreach (IntVec3 c in cellRect)
			{
				foreach (Thing thing in map.thingGrid.ThingsAt(c).ToList<Thing>())
				{
					if (GenSpawn.SpawningWipes(thingDef, thing.def) && predicate(thing))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x0011279C File Offset: 0x0011099C
		public static bool SpawningWipes(BuildableDef newEntDef, BuildableDef oldEntDef)
		{
			ThingDef thingDef = newEntDef as ThingDef;
			ThingDef thingDef2 = oldEntDef as ThingDef;
			if (thingDef == null || thingDef2 == null)
			{
				return false;
			}
			if (thingDef.category == ThingCategory.Attachment || thingDef.category == ThingCategory.Mote || thingDef.category == ThingCategory.Filth || thingDef.category == ThingCategory.Projectile)
			{
				return false;
			}
			if (!thingDef2.destroyable)
			{
				return false;
			}
			if (thingDef.category == ThingCategory.Plant)
			{
				return false;
			}
			if (thingDef.category == ThingCategory.PsychicEmitter)
			{
				return thingDef2.category == ThingCategory.PsychicEmitter;
			}
			if (thingDef2.category == ThingCategory.Filth && thingDef.passability != Traversability.Standable)
			{
				return true;
			}
			if (thingDef2.category == ThingCategory.Item && thingDef.passability == Traversability.Impassable && thingDef.surfaceType == SurfaceType.None)
			{
				return true;
			}
			if (thingDef.EverTransmitsPower && thingDef2 == ThingDefOf.PowerConduit)
			{
				return true;
			}
			if (thingDef.IsFrame && GenSpawn.SpawningWipes(thingDef.entityDefToBuild, oldEntDef))
			{
				return true;
			}
			BuildableDef buildableDef = GenConstruct.BuiltDefOf(thingDef);
			BuildableDef buildableDef2 = GenConstruct.BuiltDefOf(thingDef2);
			if (buildableDef == null || buildableDef2 == null)
			{
				return false;
			}
			ThingDef thingDef3 = thingDef.entityDefToBuild as ThingDef;
			if (thingDef2.IsBlueprint)
			{
				if (thingDef.IsBlueprint)
				{
					ThingDef thingDef4;
					if (thingDef3 != null && thingDef3.building != null && thingDef3.building.canPlaceOverWall && (thingDef4 = (thingDef2.entityDefToBuild as ThingDef)) != null && thingDef4.building != null && thingDef4.building.isPlaceOverableWall)
					{
						return true;
					}
					if (thingDef2.entityDefToBuild is TerrainDef)
					{
						if (thingDef.entityDefToBuild is ThingDef && ((ThingDef)thingDef.entityDefToBuild).coversFloor)
						{
							return true;
						}
						if (thingDef.entityDefToBuild is TerrainDef)
						{
							return true;
						}
					}
				}
				return thingDef2.entityDefToBuild == ThingDefOf.PowerConduit && thingDef.entityDefToBuild is ThingDef && (thingDef.entityDefToBuild as ThingDef).EverTransmitsPower;
			}
			if ((thingDef2.IsFrame || thingDef2.IsBlueprint) && thingDef2.entityDefToBuild is TerrainDef)
			{
				ThingDef thingDef5 = buildableDef as ThingDef;
				if (thingDef5 != null && !thingDef5.CoexistsWithFloors)
				{
					return true;
				}
			}
			if (thingDef2 == ThingDefOf.ActiveDropPod || thingDef == ThingDefOf.ActiveDropPod)
			{
				return false;
			}
			if (thingDef.blockPlants && thingDef.category == ThingCategory.Building && thingDef2.category == ThingCategory.Plant)
			{
				return true;
			}
			if (thingDef.IsEdifice())
			{
				if (thingDef.BlocksPlanting(false) && thingDef2.category == ThingCategory.Plant)
				{
					return true;
				}
				if (thingDef2.category == ThingCategory.PsychicEmitter)
				{
					return true;
				}
				if (!(buildableDef is TerrainDef) && buildableDef2.IsEdifice())
				{
					return true;
				}
			}
			return (thingDef.blocksAltitudes != null && thingDef.blocksAltitudes.Contains(thingDef2.altitudeLayer)) || (thingDef2.blocksAltitudes != null && thingDef2.blocksAltitudes.Contains(thingDef.altitudeLayer));
		}
	}
}

using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200055C RID: 1372
	public static class GenDrop
	{
		// Token: 0x06002A20 RID: 10784 RVA: 0x0010C910 File Offset: 0x0010AB10
		public static bool TryDropSpawn(Thing thing, IntVec3 dropCell, Map map, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			if (map == null)
			{
				Log.Error("Dropped " + thing + " in a null map.");
				resultingThing = null;
				return false;
			}
			if (!dropCell.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Dropped ",
					thing,
					" out of bounds at ",
					dropCell
				}));
				resultingThing = null;
				return false;
			}
			if (thing.def.destroyOnDrop)
			{
				thing.Destroy(DestroyMode.Vanish);
				resultingThing = null;
				return true;
			}
			if (playDropSound && thing.def.soundDrop != null)
			{
				thing.def.soundDrop.PlayOneShot(new TargetInfo(dropCell, map, false));
			}
			return GenPlace.TryPlaceThing(thing, dropCell, map, mode, out resultingThing, placedAction, nearPlaceValidator, default(Rot4));
		}
	}
}

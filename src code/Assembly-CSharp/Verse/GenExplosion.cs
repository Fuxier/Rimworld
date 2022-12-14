using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000565 RID: 1381
	public static class GenExplosion
	{
		// Token: 0x06002A61 RID: 10849 RVA: 0x0010EB5C File Offset: 0x0010CD5C
		public static void DoExplosion(IntVec3 center, Map map, float radius, DamageDef damType, Thing instigator, int damAmount = -1, float armorPenetration = -1f, SoundDef explosionSound = null, ThingDef weapon = null, ThingDef projectile = null, Thing intendedTarget = null, ThingDef postExplosionSpawnThingDef = null, float postExplosionSpawnChance = 0f, int postExplosionSpawnThingCount = 1, GasType? postExplosionGasType = null, bool applyDamageToExplosionCellsNeighbors = false, ThingDef preExplosionSpawnThingDef = null, float preExplosionSpawnChance = 0f, int preExplosionSpawnThingCount = 1, float chanceToStartFire = 0f, bool damageFalloff = false, float? direction = null, List<Thing> ignoredThings = null, FloatRange? affectedAngle = null, bool doVisualEffects = true, float propagationSpeed = 1f, float excludeRadius = 0f, bool doSoundEffects = true, ThingDef postExplosionSpawnThingDefWater = null, float screenShakeFactor = 1f)
		{
			if (map == null)
			{
				Log.Warning("Tried to do explosion in a null map.");
				return;
			}
			if (damAmount < 0)
			{
				damAmount = damType.defaultDamage;
				armorPenetration = damType.defaultArmorPenetration;
				if (damAmount < 0)
				{
					Log.ErrorOnce("Attempted to trigger an explosion without defined damage", 91094882);
					damAmount = 1;
				}
			}
			if (armorPenetration < 0f)
			{
				armorPenetration = (float)damAmount * 0.015f;
			}
			Explosion explosion = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, center, map, WipeMode.Vanish);
			IntVec3? needLOSToCell = null;
			IntVec3? needLOSToCell2 = null;
			if (direction != null)
			{
				GenExplosion.CalculateNeededLOSToCells(center, map, direction.Value, out needLOSToCell, out needLOSToCell2);
			}
			explosion.radius = radius;
			explosion.damType = damType;
			explosion.instigator = instigator;
			explosion.damAmount = damAmount;
			explosion.armorPenetration = armorPenetration;
			explosion.weapon = weapon;
			explosion.projectile = projectile;
			explosion.intendedTarget = intendedTarget;
			explosion.preExplosionSpawnThingDef = preExplosionSpawnThingDef;
			explosion.preExplosionSpawnChance = preExplosionSpawnChance;
			explosion.preExplosionSpawnThingCount = preExplosionSpawnThingCount;
			explosion.postExplosionSpawnThingDef = postExplosionSpawnThingDef;
			explosion.postExplosionSpawnThingDefWater = postExplosionSpawnThingDefWater;
			explosion.postExplosionSpawnChance = postExplosionSpawnChance;
			explosion.postExplosionSpawnThingCount = postExplosionSpawnThingCount;
			explosion.postExplosionGasType = postExplosionGasType;
			explosion.applyDamageToExplosionCellsNeighbors = applyDamageToExplosionCellsNeighbors;
			explosion.chanceToStartFire = chanceToStartFire;
			explosion.damageFalloff = damageFalloff;
			explosion.needLOSToCell1 = needLOSToCell;
			explosion.needLOSToCell2 = needLOSToCell2;
			explosion.excludeRadius = excludeRadius;
			explosion.affectedAngle = affectedAngle;
			explosion.doSoundEffects = doSoundEffects;
			explosion.screenShakeFactor = screenShakeFactor;
			explosion.doVisualEffects = doVisualEffects;
			explosion.propagationSpeed = propagationSpeed;
			explosion.StartExplosion(explosionSound, ignoredThings);
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x0010ECD4 File Offset: 0x0010CED4
		private static void CalculateNeededLOSToCells(IntVec3 position, Map map, float direction, out IntVec3? needLOSToCell1, out IntVec3? needLOSToCell2)
		{
			needLOSToCell1 = null;
			needLOSToCell2 = null;
			if (position.CanBeSeenOverFast(map))
			{
				return;
			}
			direction = GenMath.PositiveMod(direction, 360f);
			IntVec3 intVec = position;
			intVec.z++;
			IntVec3 intVec2 = position;
			intVec2.z--;
			IntVec3 intVec3 = position;
			intVec3.x--;
			IntVec3 intVec4 = position;
			intVec4.x++;
			if (direction < 90f)
			{
				if (intVec3.InBounds(map) && intVec3.CanBeSeenOverFast(map))
				{
					needLOSToCell1 = new IntVec3?(intVec3);
				}
				if (intVec.InBounds(map) && intVec.CanBeSeenOverFast(map))
				{
					needLOSToCell2 = new IntVec3?(intVec);
					return;
				}
			}
			else if (direction < 180f)
			{
				if (intVec.InBounds(map) && intVec.CanBeSeenOverFast(map))
				{
					needLOSToCell1 = new IntVec3?(intVec);
				}
				if (intVec4.InBounds(map) && intVec4.CanBeSeenOverFast(map))
				{
					needLOSToCell2 = new IntVec3?(intVec4);
					return;
				}
			}
			else if (direction < 270f)
			{
				if (intVec4.InBounds(map) && intVec4.CanBeSeenOverFast(map))
				{
					needLOSToCell1 = new IntVec3?(intVec4);
				}
				if (intVec2.InBounds(map) && intVec2.CanBeSeenOverFast(map))
				{
					needLOSToCell2 = new IntVec3?(intVec2);
					return;
				}
			}
			else
			{
				if (intVec2.InBounds(map) && intVec2.CanBeSeenOverFast(map))
				{
					needLOSToCell1 = new IntVec3?(intVec2);
				}
				if (intVec3.InBounds(map) && intVec3.CanBeSeenOverFast(map))
				{
					needLOSToCell2 = new IntVec3?(intVec3);
				}
			}
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x0010EE5C File Offset: 0x0010D05C
		public static void RenderPredictedAreaOfEffect(IntVec3 loc, float radius)
		{
			GenDraw.DrawFieldEdges(DamageDefOf.Bomb.Worker.ExplosionCellsToHit(loc, Find.CurrentMap, radius, null, null, null).ToList<IntVec3>());
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x0010EEA4 File Offset: 0x0010D0A4
		public static void NotifyNearbyPawnsOfDangerousExplosive(Thing exploder, DamageDef damage, Faction onlyFaction = null, Thing instigator = null)
		{
			GenExplosion.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.onlyFaction = onlyFaction;
			CS$<>8__locals1.damage = damage;
			GenExplosion.exploderOverlapRooms.Clear();
			if (exploder.def.passability == Traversability.Impassable)
			{
				using (IEnumerator<IntVec3> enumerator = exploder.OccupiedRect().ExpandedBy(1).EdgeCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 loc = enumerator.Current;
						Room room = loc.GetRoom(exploder.Map);
						if (!GenExplosion.exploderOverlapRooms.Contains(room))
						{
							GenExplosion.exploderOverlapRooms.Add(room);
						}
					}
					goto IL_9A;
				}
			}
			GenExplosion.exploderOverlapRooms.Add(exploder.GetRoom(RegionType.Set_All));
			IL_9A:
			Pawn pawn = instigator as Pawn;
			if (pawn != null && pawn.Spawned && GenExplosion.<NotifyNearbyPawnsOfDangerousExplosive>g__CanNotifyPawn|5_0(pawn, ref CS$<>8__locals1))
			{
				pawn.mindState.Notify_DangerousExploderAboutToExplode(exploder);
			}
			for (int i = 0; i < GenExplosion.PawnNotifyCellCount; i++)
			{
				IntVec3 c = exploder.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(exploder.MapHeld))
				{
					List<Thing> thingList = c.GetThingList(exploder.MapHeld);
					for (int j = 0; j < thingList.Count; j++)
					{
						Pawn pawn2 = thingList[j] as Pawn;
						if (pawn2 != null && GenExplosion.<NotifyNearbyPawnsOfDangerousExplosive>g__CanNotifyPawn|5_0(pawn2, ref CS$<>8__locals1) && pawn2 != pawn)
						{
							Room room2 = pawn2.GetRoom(RegionType.Set_All);
							if (room2 == null || room2.CellCount == 1 || (GenExplosion.exploderOverlapRooms.Contains(room2) && GenSight.LineOfSightToThing(pawn2.Position, exploder, exploder.MapHeld, true, null)))
							{
								pawn2.mindState.Notify_DangerousExploderAboutToExplode(exploder);
							}
						}
					}
				}
			}
			GenExplosion.exploderOverlapRooms.Clear();
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x0010F083 File Offset: 0x0010D283
		[CompilerGenerated]
		internal static bool <NotifyNearbyPawnsOfDangerousExplosive>g__CanNotifyPawn|5_0(Pawn p, ref GenExplosion.<>c__DisplayClass5_0 A_1)
		{
			return p.RaceProps.intelligence >= Intelligence.Humanlike && (A_1.onlyFaction == null || p.Faction == A_1.onlyFaction) && A_1.damage.ExternalViolenceFor(p);
		}

		// Token: 0x04001BCD RID: 7117
		private static readonly int PawnNotifyCellCount = GenRadial.NumCellsInRadius(4.5f);

		// Token: 0x04001BCE RID: 7118
		private static List<Room> exploderOverlapRooms = new List<Room>();
	}
}

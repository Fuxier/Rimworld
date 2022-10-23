using System;

namespace Verse
{
	// Token: 0x020003FD RID: 1021
	public class Projectile_Explosive : Projectile
	{
		// Token: 0x06001D1B RID: 7451 RVA: 0x000B0A74 File Offset: 0x000AEC74
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToDetonation, "ticksToDetonation", 0, false);
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000B0A8E File Offset: 0x000AEC8E
		public override void Tick()
		{
			base.Tick();
			if (this.ticksToDetonation > 0)
			{
				this.ticksToDetonation--;
				if (this.ticksToDetonation <= 0)
				{
					this.Explode();
				}
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x000B0ABC File Offset: 0x000AECBC
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			if (blockedByShield || this.def.projectile.explosionDelay == 0)
			{
				this.Explode();
				return;
			}
			this.landed = true;
			this.ticksToDetonation = this.def.projectile.explosionDelay;
			GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this, this.def.projectile.damageDef, this.launcher.Faction, this.launcher);
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000B0B2C File Offset: 0x000AED2C
		protected virtual void Explode()
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.def.projectile.explosionEffect != null)
			{
				Effecter effecter = this.def.projectile.explosionEffect.Spawn();
				effecter.Trigger(new TargetInfo(base.Position, map, false), new TargetInfo(base.Position, map, false), -1);
				effecter.Cleanup();
			}
			IntVec3 position = base.Position;
			Map map2 = map;
			float explosionRadius = this.def.projectile.explosionRadius;
			DamageDef damageDef = this.def.projectile.damageDef;
			Thing launcher = this.launcher;
			int damageAmount = this.DamageAmount;
			float armorPenetration = this.ArmorPenetration;
			SoundDef soundExplode = this.def.projectile.soundExplode;
			ThingDef equipmentDef = this.equipmentDef;
			ThingDef def = this.def;
			Thing thing = this.intendedTarget.Thing;
			ThingDef postExplosionSpawnThingDef = this.def.projectile.postExplosionSpawnThingDef;
			ThingDef postExplosionSpawnThingDefWater = this.def.projectile.postExplosionSpawnThingDefWater;
			float postExplosionSpawnChance = this.def.projectile.postExplosionSpawnChance;
			int postExplosionSpawnThingCount = this.def.projectile.postExplosionSpawnThingCount;
			GasType? postExplosionGasType = this.def.projectile.postExplosionGasType;
			ThingDef preExplosionSpawnThingDef = this.def.projectile.preExplosionSpawnThingDef;
			float preExplosionSpawnChance = this.def.projectile.preExplosionSpawnChance;
			int preExplosionSpawnThingCount = this.def.projectile.preExplosionSpawnThingCount;
			GenExplosion.DoExplosion(position, map2, explosionRadius, damageDef, launcher, damageAmount, armorPenetration, soundExplode, equipmentDef, def, thing, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, postExplosionGasType, this.def.projectile.applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, preExplosionSpawnChance, preExplosionSpawnThingCount, this.def.projectile.explosionChanceToStartFire, this.def.projectile.explosionDamageFalloff, new float?(this.origin.AngleToFlat(this.destination)), null, null, true, this.def.projectile.damageDef.expolosionPropagationSpeed, 0f, true, postExplosionSpawnThingDefWater, this.def.projectile.screenShakeFactor);
		}

		// Token: 0x0400149B RID: 5275
		private int ticksToDetonation;
	}
}

using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005B1 RID: 1457
	public class Verb_SpewFire : Verb
	{
		// Token: 0x06002CA6 RID: 11430 RVA: 0x0011BA5C File Offset: 0x00119C5C
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			if (base.EquipmentSource != null)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompReloadable comp2 = base.EquipmentSource.GetComp<CompReloadable>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			IntVec3 position = this.caster.Position;
			float num = Mathf.Atan2((float)(-(float)(this.currentTarget.Cell.z - position.z)), (float)(this.currentTarget.Cell.x - position.x)) * 57.29578f;
			FloatRange value = new FloatRange(num - 13f, num + 13f);
			GenExplosion.DoExplosion(position, this.caster.MapHeld, this.verbProps.range, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, ThingDefOf.Filth_FlammableBile, 1f, 1, null, false, null, 0f, 1, 1f, false, null, null, new FloatRange?(value), false, 0.6f, 0f, false, null, 1f);
			base.AddEffecterToMaintain(EffecterDefOf.Fire_SpewShort.Spawn(this.caster.Position, this.currentTarget.Cell, this.caster.Map, 1f), this.caster.Position, this.currentTarget.Cell, 14, this.caster.Map);
			this.lastShotTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x0011BC04 File Offset: 0x00119E04
		public override bool Available()
		{
			if (!base.Available())
			{
				return false;
			}
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Faction != Faction.OfPlayer && casterPawn.mindState.MeleeThreatStillThreat && casterPawn.mindState.meleeThreat.Position.AdjacentTo8WayOrInside(casterPawn.Position))
				{
					return false;
				}
			}
			return true;
		}
	}
}

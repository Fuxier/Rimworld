using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002BD RID: 701
	public class DamageWorker_Vaporize : DamageWorker_AddInjury
	{
		// Token: 0x060013EA RID: 5098 RVA: 0x00079A3C File Offset: 0x00077C3C
		public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			bool flag = c.DistanceTo(explosion.Position) <= 2.9f;
			Thing firstThing = c.GetFirstThing(explosion.Map, ThingDefOf.Filth_FireFoam);
			if (firstThing != null)
			{
				firstThing.Destroy(DestroyMode.Vanish);
			}
			base.ExplosionAffectCell(explosion, c, damagedThings, ignoredThings, canThrowMotes && flag);
			FireUtility.TryStartFireIn(c, explosion.Map, DamageWorker_Vaporize.FireSizeRange.RandomInRange);
			if (flag)
			{
				FleckMaker.ThrowSmoke(c.ToVector3Shifted(), explosion.Map, 2f);
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00079AC0 File Offset: 0x00077CC0
		protected override void ExplosionDamageThing(Explosion explosion, Thing t, List<Thing> damagedThings, List<Thing> ignoredThings, IntVec3 cell)
		{
			if (cell.DistanceTo(explosion.Position) <= 2.9f)
			{
				base.ExplosionDamageThing(explosion, t, damagedThings, ignoredThings, cell);
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00079AE3 File Offset: 0x00077CE3
		public override void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			base.ExplosionStart(explosion, cellsToAffect);
			Effecter effecter = EffecterDefOf.Vaporize_Heatwave.Spawn();
			effecter.Trigger(new TargetInfo(explosion.Position, explosion.Map, false), TargetInfo.Invalid, -1);
			effecter.Cleanup();
		}

		// Token: 0x04001051 RID: 4177
		private const float VaporizeRadius = 2.9f;

		// Token: 0x04001052 RID: 4178
		private static readonly FloatRange FireSizeRange = new FloatRange(0.4f, 0.8f);
	}
}

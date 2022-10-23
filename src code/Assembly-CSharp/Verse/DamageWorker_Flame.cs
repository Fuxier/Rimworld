using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002B8 RID: 696
	public class DamageWorker_Flame : DamageWorker_AddInjury
	{
		// Token: 0x060013DD RID: 5085 RVA: 0x000795E8 File Offset: 0x000777E8
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			Pawn pawn = victim as Pawn;
			if (pawn != null && pawn.Faction == Faction.OfPlayer)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			Map map = victim.Map;
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			if (!damageResult.deflected && !dinfo.InstantPermanentInjury && Rand.Chance(FireUtility.ChanceToAttachFireFromEvent(victim)))
			{
				victim.TryAttachFire(Rand.Range(0.15f, 0.25f));
			}
			if (victim.Destroyed && map != null && pawn == null)
			{
				foreach (IntVec3 c in victim.OccupiedRect())
				{
					FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Ash, 1, FilthSourceFlags.None, true);
				}
				Plant plant;
				if ((plant = (victim as Plant)) != null && plant.LifeStage != PlantLifeStage.Sowing)
				{
					plant.TrySpawnStump(PlantDestructionMode.Flame);
				}
			}
			return damageResult;
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x000796DC File Offset: 0x000778DC
		public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			base.ExplosionAffectCell(explosion, c, damagedThings, ignoredThings, canThrowMotes);
			if (this.def == DamageDefOf.Flame && Rand.Chance(FireUtility.ChanceToStartFireIn(c, explosion.Map)))
			{
				FireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f));
			}
		}
	}
}

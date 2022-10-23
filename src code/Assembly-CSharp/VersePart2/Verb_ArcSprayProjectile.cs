using System;

namespace Verse
{
	// Token: 0x020005B4 RID: 1460
	public class Verb_ArcSprayProjectile : Verb_ArcSpray
	{
		// Token: 0x06002CB4 RID: 11444 RVA: 0x0011C018 File Offset: 0x0011A218
		protected override void HitCell(IntVec3 cell)
		{
			base.HitCell(cell);
			Map map = this.caster.Map;
			if (GenSight.LineOfSight(this.caster.Position, cell, map, true, null, 0, 0))
			{
				((Projectile)GenSpawn.Spawn(this.verbProps.defaultProjectile, this.caster.Position, map, WipeMode.Vanish)).Launch(this.caster, this.caster.DrawPos, cell, cell, ProjectileHitFlags.IntendedTarget, false, null, null);
			}
		}
	}
}

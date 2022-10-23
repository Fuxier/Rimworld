using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020005AF RID: 1455
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x0011AFF1 File Offset: 0x001191F1
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x0011B000 File Offset: 0x00119200
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Pawn pawn = this.currentTarget.Thing as Pawn;
			if (pawn != null && !pawn.Downed && this.CasterIsPawn && this.CasterPawn.skills != null)
			{
				float num = pawn.HostileTo(this.caster) ? 170f : 20f;
				float num2 = this.verbProps.AdjustedFullCycleTime(this, this.CasterPawn);
				this.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false);
			}
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x0011B08B File Offset: 0x0011928B
		protected override bool TryCastShot()
		{
			bool flag = base.TryCastShot();
			if (flag && this.CasterIsPawn)
			{
				this.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
			}
			return flag;
		}
	}
}

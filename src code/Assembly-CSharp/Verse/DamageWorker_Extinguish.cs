using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C0 RID: 704
	public class DamageWorker_Extinguish : DamageWorker
	{
		// Token: 0x060013F3 RID: 5107 RVA: 0x00079D4C File Offset: 0x00077F4C
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult result = new DamageWorker.DamageResult();
			Fire fire = victim as Fire;
			if (fire == null || fire.Destroyed)
			{
				Thing thing = (victim != null) ? victim.GetAttachment(ThingDefOf.Fire) : null;
				if (thing != null)
				{
					fire = (Fire)thing;
				}
			}
			if (fire != null && !fire.Destroyed)
			{
				base.Apply(dinfo, victim);
				fire.fireSize -= dinfo.Amount * 0.01f;
				if (fire.fireSize < 0.1f)
				{
					fire.Destroy(DestroyMode.Vanish);
				}
			}
			Pawn pawn = victim as Pawn;
			if (pawn != null)
			{
				Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn, null);
				hediff.Severity = dinfo.Amount;
				pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
			}
			return result;
		}

		// Token: 0x04001055 RID: 4181
		private const float DamageAmountToFireSizeRatio = 0.01f;
	}
}

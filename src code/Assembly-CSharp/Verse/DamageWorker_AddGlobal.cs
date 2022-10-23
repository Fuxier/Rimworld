using System;

namespace Verse
{
	// Token: 0x020002BF RID: 703
	public class DamageWorker_AddGlobal : DamageWorker
	{
		// Token: 0x060013F1 RID: 5105 RVA: 0x00079CF8 File Offset: 0x00077EF8
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn, null);
				hediff.Severity = dinfo.Amount;
				pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
			}
			return new DamageWorker.DamageResult();
		}
	}
}

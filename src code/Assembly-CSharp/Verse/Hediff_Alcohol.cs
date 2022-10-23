using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200033E RID: 830
	public class Hediff_Alcohol : Hediff_High
	{
		// Token: 0x0600162C RID: 5676 RVA: 0x00083044 File Offset: 0x00081244
		public override void Tick()
		{
			base.Tick();
			if (this.CurStageIndex >= 3 && this.pawn.IsHashIntervalTick(300) && this.HangoverSusceptible(this.pawn))
			{
				Hediff hediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hangover, false);
				if (hediff != null)
				{
					hediff.Severity = 1f;
					return;
				}
				hediff = HediffMaker.MakeHediff(HediffDefOf.Hangover, this.pawn, null);
				hediff.Severity = 1f;
				this.pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00002662 File Offset: 0x00000862
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}

		// Token: 0x04001183 RID: 4483
		private const int HangoverCheckInterval = 300;
	}
}

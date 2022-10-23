using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002FF RID: 767
	public class HediffComp_KillAfterDays : HediffComp
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0007F8AC File Offset: 0x0007DAAC
		public HediffCompProperties_KillAfterDays Props
		{
			get
			{
				return (HediffCompProperties_KillAfterDays)this.props;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x0007F8BC File Offset: 0x0007DABC
		public override string CompTipStringExtra
		{
			get
			{
				if (this.ticksLeft <= 0)
				{
					return null;
				}
				return "DeathIn".Translate(this.ticksLeft.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)).Resolve().CapitalizeFirst();
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0007F90A File Offset: 0x0007DB0A
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.ticksLeft = 60000 * this.Props.days;
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0007F924 File Offset: 0x0007DB24
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				base.Pawn.Kill(null, this.parent);
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0007F962 File Offset: 0x0007DB62
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x04001111 RID: 4369
		private int ticksLeft;
	}
}

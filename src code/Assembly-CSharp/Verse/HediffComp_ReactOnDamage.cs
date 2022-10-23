using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000315 RID: 789
	public class HediffComp_ReactOnDamage : HediffComp
	{
		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600156C RID: 5484 RVA: 0x000806D4 File Offset: 0x0007E8D4
		public HediffCompProperties_ReactOnDamage Props
		{
			get
			{
				return (HediffCompProperties_ReactOnDamage)this.props;
			}
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x000806E1 File Offset: 0x0007E8E1
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.damageDefIncoming == dinfo.Def)
			{
				this.React();
			}
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00080700 File Offset: 0x0007E900
		private void React()
		{
			if (this.Props.createHediff != null)
			{
				BodyPartRecord part = this.parent.Part;
				if (this.Props.createHediffOn != null)
				{
					part = this.parent.pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == this.Props.createHediffOn, null);
				}
				this.parent.pawn.health.AddHediff(this.Props.createHediff, part, null, null);
			}
			if (this.Props.vomit)
			{
				this.parent.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true);
			}
		}
	}
}

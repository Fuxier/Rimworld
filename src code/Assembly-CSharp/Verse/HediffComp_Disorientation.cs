using System;
using System.Linq;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002DD RID: 733
	public class HediffComp_Disorientation : HediffComp
	{
		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0007DFAF File Offset: 0x0007C1AF
		private HediffCompProperties_Disorientation Props
		{
			get
			{
				return (HediffCompProperties_Disorientation)this.props;
			}
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0007DFBC File Offset: 0x0007C1BC
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Props.wanderMtbHours > 0f && base.Pawn.Spawned && !base.Pawn.Downed && base.Pawn.Awake() && base.Pawn.CurJobDef.suspendable && this.Props.wanderMtbHours > 0f && base.Pawn.IsHashIntervalTick(60) && Rand.MTBEventOccurs(this.Props.wanderMtbHours, 2500f, 60f) && base.Pawn.CurJob.def != JobDefOf.GotoMindControlled)
			{
				IntVec3 c2 = (from c in GenRadial.RadialCellsAround(base.Pawn.Position, this.Props.wanderRadius, false)
				where c.Standable(base.Pawn.MapHeld) && base.Pawn.CanReach(c, PathEndMode.OnCell, Danger.Unspecified, false, false, TraverseMode.ByPawn)
				select c).RandomElementWithFallback(IntVec3.Invalid);
				if (c2.IsValid)
				{
					MoteMaker.MakeThoughtBubble(base.Pawn, "Things/Mote/Disoriented", false);
					Job job = JobMaker.MakeJob(JobDefOf.GotoMindControlled, c2);
					job.expiryInterval = this.Props.singleWanderDurationTicks;
					base.Pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true);
				}
			}
		}

		// Token: 0x040010D5 RID: 4309
		private const string moteTexPath = "Things/Mote/Disoriented";
	}
}

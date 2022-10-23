using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000088 RID: 136
	public class PriorityWork : IExposable
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0001AE58 File Offset: 0x00019058
		public bool IsPrioritized
		{
			get
			{
				if (this.prioritizedCell.IsValid)
				{
					if (Find.TickManager.TicksGame < this.prioritizeTick + 30000)
					{
						return true;
					}
					this.Clear();
				}
				return false;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0001AE88 File Offset: 0x00019088
		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0001AE90 File Offset: 0x00019090
		public WorkGiverDef WorkGiver
		{
			get
			{
				return this.prioritizedWorkGiver;
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001AE98 File Offset: 0x00019098
		public PriorityWork()
		{
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001AEBB File Offset: 0x000190BB
		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001AEE8 File Offset: 0x000190E8
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.prioritizedWorkGiver, "prioritizedWorkGiver");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001AF31 File Offset: 0x00019131
		public void Set(IntVec3 prioritizedCell, WorkGiverDef prioritizedWorkGiver)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkGiver = prioritizedWorkGiver;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001AF51 File Offset: 0x00019151
		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkGiver = null;
			this.prioritizeTick = 0;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001AF6C File Offset: 0x0001916C
		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.ClearQueuedJobs(true);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001AF85 File Offset: 0x00019185
		public IEnumerable<Gizmo> GetGizmos()
		{
			if ((this.IsPrioritized || (this.pawn.CurJob != null && this.pawn.CurJob.playerForced && this.pawn.jobs.IsCurrentJobPlayerInterruptible()) || this.pawn.jobs.jobQueue.AnyPlayerForced) && !this.pawn.Drafted && !this.pawn.Deathresting)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandClearPrioritizedWork".Translate(),
					defaultDesc = "CommandClearPrioritizedWorkDesc".Translate(),
					icon = TexCommand.ClearPrioritizedWork,
					activateSound = SoundDefOf.Tick_Low,
					action = delegate()
					{
						this.ClearPrioritizedWorkAndJobQueue();
						if (this.pawn.CurJob.playerForced && this.pawn.jobs.IsCurrentJobPlayerInterruptible())
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					},
					hotKey = KeyBindingDefOf.Designator_Cancel,
					groupKeyIgnoreContent = 6165612
				};
			}
			yield break;
		}

		// Token: 0x0400022B RID: 555
		private Pawn pawn;

		// Token: 0x0400022C RID: 556
		private IntVec3 prioritizedCell = IntVec3.Invalid;

		// Token: 0x0400022D RID: 557
		private WorkGiverDef prioritizedWorkGiver;

		// Token: 0x0400022E RID: 558
		private int prioritizeTick = Find.TickManager.TicksGame;

		// Token: 0x0400022F RID: 559
		private const int Timeout = 30000;
	}
}

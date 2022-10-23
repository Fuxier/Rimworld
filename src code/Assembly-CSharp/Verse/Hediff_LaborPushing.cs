using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000347 RID: 839
	public class Hediff_LaborPushing : HediffWithParents
	{
		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x00084118 File Offset: 0x00082318
		public float Progress
		{
			get
			{
				HediffComp_Disappears hediffComp_Disappears = this.TryGetComp<HediffComp_Disappears>();
				return 1f - (float)hediffComp_Disappears.ticksToDisappear / (float)Mathf.Max(1, hediffComp_Disappears.disappearsAfterTicks + this.laborDuration);
			}
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x0008414E File Offset: 0x0008234E
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return PregnancyUtility.BirthQualityGizmo(this.pawn);
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force stillborn",
					action = delegate()
					{
						this.<GetGizmos>g__ForceBirth|5_0(-1);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force infant illness",
					action = delegate()
					{
						this.<GetGizmos>g__ForceBirth|5_0(0);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force healthy",
					action = delegate()
					{
						this.<GetGizmos>g__ForceBirth|5_0(1);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force end",
					action = delegate()
					{
						this.pawn.health.RemoveHediff(this);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00084160 File Offset: 0x00082360
		public override void PreRemoved()
		{
			base.PreRemoved();
			Lord lord = this.pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			Precept_Ritual precept_Ritual = (Precept_Ritual)this.pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
			if (lordJob_Ritual == null || lordJob_Ritual.Ritual == null || lordJob_Ritual.Ritual.def != PreceptDefOf.ChildBirth || lordJob_Ritual.assignments.FirstAssignedPawn("mother") != this.pawn)
			{
				float birthQualityFor = PregnancyUtility.GetBirthQualityFor(this.pawn);
				OutcomeChance outcome = this.debugForceBirthOutcome ?? ((RitualOutcomeEffectWorker_ChildBirth)precept_Ritual.outcomeEffect).GetOutcome(birthQualityFor, null);
				RitualRoleAssignments assignments = PregnancyUtility.RitualAssignmentsForBirth(precept_Ritual, this.pawn);
				float quality = birthQualityFor;
				Precept_Ritual ritual = precept_Ritual;
				GeneSet geneSet = this.geneSet;
				PregnancyUtility.ApplyBirthOutcome(outcome, quality, ritual, (geneSet != null) ? geneSet.GenesListForReading : null, base.Mother ?? this.pawn, this.pawn, base.Father, null, null, assignments);
				return;
			}
			if (lordJob_Ritual != null)
			{
				lordJob_Ritual.ApplyOutcome(1f, true, true, false);
			}
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00084260 File Offset: 0x00082460
		public override void Tick()
		{
			if (this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.progressBar == null)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
				}
				this.progressBar.EffectTick(this.pawn, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
				if (mote != null)
				{
					mote.progress = this.Progress;
				}
			}
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x000842D8 File Offset: 0x000824D8
		public override void PostRemoved()
		{
			base.PostRemoved();
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x000842F8 File Offset: 0x000824F8
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00084318 File Offset: 0x00082518
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.laborDuration, "laborDuration", 0, false);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00084334 File Offset: 0x00082534
		[CompilerGenerated]
		private void <GetGizmos>g__ForceBirth|5_0(int positivityIndex)
		{
			Precept_Ritual precept_Ritual = (Precept_Ritual)this.pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
			this.debugForceBirthOutcome = precept_Ritual.outcomeEffect.def.outcomeChances.FirstOrDefault((OutcomeChance o) => o.positivityIndex == positivityIndex);
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x04001192 RID: 4498
		public int laborDuration;

		// Token: 0x04001193 RID: 4499
		private Effecter progressBar;

		// Token: 0x04001194 RID: 4500
		private OutcomeChance debugForceBirthOutcome;
	}
}

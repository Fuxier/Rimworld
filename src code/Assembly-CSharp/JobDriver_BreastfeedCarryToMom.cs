using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

// Token: 0x02000005 RID: 5
public class JobDriver_BreastfeedCarryToMom : JobDriver
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000014 RID: 20 RVA: 0x00002483 File Offset: 0x00000683
	private Pawn Baby
	{
		get
		{
			return (Pawn)base.TargetThingA;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000015 RID: 21 RVA: 0x00002490 File Offset: 0x00000690
	private Pawn Mom
	{
		get
		{
			return (Pawn)base.TargetThingB;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000016 RID: 22 RVA: 0x0000249D File Offset: 0x0000069D
	protected virtual bool MomMustBeImmobile
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000024A0 File Offset: 0x000006A0
	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return this.pawn.Reserve(this.Baby, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Mom, this.job, 1, -1, null, errorOnFailed);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000024F1 File Offset: 0x000006F1
	protected override IEnumerable<Toil> MakeNewToils()
	{
		this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
		this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
		base.AddFailCondition(delegate
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			return (this.MomMustBeImmobile && !this.Mom.Downed && !this.Mom.IsPrisoner) || !ChildcareUtility.CanMomBreastfeedBaby(this.Mom, this.Baby, out breastfeedFailReason);
		});
		base.SetFinalizerJob(delegate(JobCondition condition)
		{
			if (!this.pawn.IsCarryingPawn(this.Baby))
			{
				return null;
			}
			return ChildcareUtility.MakeBringBabyToSafetyJob(this.pawn, this.Baby);
		});
		Toil carryingBabyStart = Toils_General.Label();
		yield return Toils_Jump.JumpIf(carryingBabyStart, () => this.pawn.IsCarryingPawn(this.Baby));
		yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
		yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true);
		yield return carryingBabyStart;
		yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
		yield return Toils_Reserve.Release(TargetIndex.A);
		yield return Toils_Reserve.Release(TargetIndex.B);
		yield return this.StartBreastfeedJobOnMom();
		yield break;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002501 File Offset: 0x00000701
	private Toil StartBreastfeedJobOnMom()
	{
		Toil toil = ToilMaker.MakeToil("StartBreastfeedJobOnMom");
		toil.initAction = delegate()
		{
			this.Mom.jobs.SuspendCurrentJob(JobCondition.InterruptForced, true, new bool?(false));
			if (!this.pawn.carryTracker.innerContainer.TryTransferToContainer(this.pawn.carryTracker.CarriedThing, this.Mom.carryTracker.innerContainer, true))
			{
				this.Mom.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				base.EndJobWith(JobCondition.Incompletable);
				return;
			}
			this.Mom.jobs.StartJob(ChildcareUtility.MakeBreastfeedJob(this.Baby, this.Mom.CurrentBed()), JobCondition.InterruptForced, null, false, true, null, null, false, false, new bool?(true), false, true);
		};
		return toil;
	}

	// Token: 0x04000008 RID: 8
	private const TargetIndex BabyInd = TargetIndex.A;

	// Token: 0x04000009 RID: 9
	private const TargetIndex MomInd = TargetIndex.B;
}

using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000370 RID: 880
	public class Pawn_CallTracker
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x0600191B RID: 6427 RVA: 0x00096DC4 File Offset: 0x00094FC4
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x0600191C RID: 6428 RVA: 0x00096E54 File Offset: 0x00095054
		private float IdleCallVolumeFactor
		{
			get
			{
				switch (Find.TickManager.CurTimeSpeed)
				{
				case TimeSpeed.Paused:
					return 1f;
				case TimeSpeed.Normal:
					return 1f;
				case TimeSpeed.Fast:
					return 1f;
				case TimeSpeed.Superfast:
					return 0.25f;
				case TimeSpeed.Ultrafast:
					return 0.25f;
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x00096EAB File Offset: 0x000950AB
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x00096EC1 File Offset: 0x000950C1
		public void CallTrackerTick()
		{
			if (this.ticksToNextCall < 0)
			{
				this.ResetTicksToNextCall();
			}
			this.ticksToNextCall--;
			if (this.ticksToNextCall <= 0)
			{
				this.TryDoCall();
				this.ResetTicksToNextCall();
			}
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x00096EF8 File Offset: 0x000950F8
		private void ResetTicksToNextCall()
		{
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall = (int)((float)this.ticksToNextCall * this.pawn.def.race.soundCallIntervalAggressiveFactor);
				return;
			}
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				this.ticksToNextCall = (int)((float)this.ticksToNextCall * this.pawn.def.race.soundCallIntervalFriendlyFactor);
			}
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x00096F98 File Offset: 0x00095198
		private void TryDoCall()
		{
			if (!Find.CameraDriver.CurrentViewRect.ExpandedBy(10).Contains(this.pawn.Position))
			{
				return;
			}
			if (this.pawn.Downed || !this.pawn.Awake())
			{
				return;
			}
			if (this.pawn.Position.Fogged(this.pawn.Map))
			{
				return;
			}
			if (this.pawn.IsColonyMech && this.pawn.IsCharging())
			{
				return;
			}
			this.DoCall();
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0009702C File Offset: 0x0009522C
		public void DoCall()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (this.PawnAggressive)
			{
				LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge ls) => ls.soundAngry, null, 1f);
				return;
			}
			LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge ls) => ls.soundCall, (GeneDef g) => g.soundCall, this.IdleCallVolumeFactor);
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x000970D0 File Offset: 0x000952D0
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x000970F0 File Offset: 0x000952F0
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0009711C File Offset: 0x0009531C
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}

		// Token: 0x04001296 RID: 4758
		public Pawn pawn;

		// Token: 0x04001297 RID: 4759
		private int ticksToNextCall = -1;

		// Token: 0x04001298 RID: 4760
		private static readonly IntRange CallOnAggroDelayRange = new IntRange(0, 120);

		// Token: 0x04001299 RID: 4761
		private static readonly IntRange CallOnMeleeDelayRange = new IntRange(0, 20);

		// Token: 0x0400129A RID: 4762
		private const float AngryCallOnMeleeChance = 0.5f;

		// Token: 0x0400129B RID: 4763
		private const int AggressiveDurationAfterEngagingTarget = 360;
	}
}

using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000377 RID: 887
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0009B27B File Offset: 0x0009947B
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0009B297 File Offset: 0x00099497
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
			this.stagger = new StaggerHandler(this.pawn);
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0009B2D3 File Offset: 0x000994D3
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			this.stagger.StaggerHandlerTick();
			this.curStance.StanceTick();
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0009B2F6 File Offset: 0x000994F6
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0009B304 File Offset: 0x00099504
		public void ExposeData()
		{
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<StaggerHandler>(ref this.stagger, "stagger", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<Stance>(ref this.curStance, "curStance", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.curStance != null)
			{
				this.curStance.stanceTracker = this;
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.stagger == null)
			{
				this.stagger = new StaggerHandler(this.pawn);
				int num = 0;
				Scribe_Values.Look<int>(ref num, "staggerUntilTick", 0, false);
				if (num > Find.TickManager.TicksGame)
				{
					this.stagger.StaggerFor(num - Find.TickManager.TicksGame, 0.17f);
				}
			}
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0009B3DB File Offset: 0x000995DB
		[Obsolete("Use pawn.stances.stagger.StaggerFor instead")]
		public void StaggerFor(int ticks)
		{
			this.stagger.StaggerFor(ticks, 0.17f);
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0009B3EF File Offset: 0x000995EF
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0009B409 File Offset: 0x00099609
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0009B418 File Offset: 0x00099618
		public void SetStance(Stance newStance)
		{
			if (this.debugLog)
			{
				Log.Message(string.Concat(new object[]
				{
					Find.TickManager.TicksGame,
					" ",
					this.pawn,
					" SetStance ",
					this.curStance,
					" -> ",
					newStance
				}));
			}
			newStance.stanceTracker = this;
			this.curStance = newStance;
			if (this.pawn.jobs.curDriver != null)
			{
				this.pawn.jobs.curDriver.Notify_StanceChanged();
			}
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x000034B7 File Offset: 0x000016B7
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x040012BF RID: 4799
		public Pawn pawn;

		// Token: 0x040012C0 RID: 4800
		public Stance curStance = new Stance_Mobile();

		// Token: 0x040012C1 RID: 4801
		public StunHandler stunner;

		// Token: 0x040012C2 RID: 4802
		public StaggerHandler stagger;

		// Token: 0x040012C3 RID: 4803
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x040012C4 RID: 4804
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x040012C5 RID: 4805
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x040012C6 RID: 4806
		public bool debugLog;
	}
}

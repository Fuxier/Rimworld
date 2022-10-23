using System;

namespace Verse
{
	// Token: 0x0200037A RID: 890
	public abstract class Stance_Busy : Stance
	{
		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x00002662 File Offset: 0x00000862
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0009B4C7 File Offset: 0x000996C7
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
			this.startedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0009B4F0 File Offset: 0x000996F0
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.startedTick = Find.TickManager.TicksGame;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0009B528 File Offset: 0x00099728
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0009B538 File Offset: 0x00099738
		private void SetPieSizeFactor()
		{
			if (this.ticksLeft < 300)
			{
				this.pieSizeFactor = 1f;
				return;
			}
			if (this.ticksLeft < 450)
			{
				this.pieSizeFactor = 0.75f;
				return;
			}
			this.pieSizeFactor = 0.5f;
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x0009B578 File Offset: 0x00099778
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.startedTick, "startedTick", 0, false);
			Scribe_TargetInfo.Look(ref this.focusTarg, "focusTarg");
			Scribe_Values.Look<bool>(ref this.neverAimWeapon, "neverAimWeapon", false, false);
			Scribe_References.Look<Verb>(ref this.verb, "verb", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetPieSizeFactor();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verb != null && this.verb.BuggedAfterLoading)
			{
				this.verb = null;
				Log.Warning(base.GetType() + " had a bugged verb after loading.");
			}
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x0009B629 File Offset: 0x00099829
		public override void StanceTick()
		{
			if (this.stanceTracker.stunner.Stunned)
			{
				return;
			}
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x0009B65B File Offset: 0x0009985B
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x040012C8 RID: 4808
		public int ticksLeft;

		// Token: 0x040012C9 RID: 4809
		public int startedTick;

		// Token: 0x040012CA RID: 4810
		public Verb verb;

		// Token: 0x040012CB RID: 4811
		public LocalTargetInfo focusTarg;

		// Token: 0x040012CC RID: 4812
		public bool neverAimWeapon;

		// Token: 0x040012CD RID: 4813
		protected float pieSizeFactor = 1f;
	}
}

using System;

namespace Verse
{
	// Token: 0x0200037D RID: 893
	public class Stance_WarmupAbilityWorld : Stance_Warmup
	{
		// Token: 0x060019D7 RID: 6615 RVA: 0x0009BE40 File Offset: 0x0009A040
		public Stance_WarmupAbilityWorld()
		{
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0009BE48 File Offset: 0x0009A048
		public Stance_WarmupAbilityWorld(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x0009BE53 File Offset: 0x0009A053
		protected override void Expire()
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}
	}
}

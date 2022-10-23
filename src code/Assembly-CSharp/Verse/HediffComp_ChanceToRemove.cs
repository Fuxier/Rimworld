using System;

namespace Verse
{
	// Token: 0x020002CA RID: 714
	public class HediffComp_ChanceToRemove : HediffComp
	{
		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0007D124 File Offset: 0x0007B324
		public HediffCompProperties_ChanceToRemove Props
		{
			get
			{
				return (HediffCompProperties_ChanceToRemove)this.props;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x0007D131 File Offset: 0x0007B331
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.removeNextInterval && this.currentInterval <= 0);
			}
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0007D154 File Offset: 0x0007B354
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.CompShouldRemove)
			{
				return;
			}
			if (this.currentInterval > 0)
			{
				this.currentInterval--;
				return;
			}
			if (Rand.Chance(this.Props.chance))
			{
				this.removeNextInterval = true;
				this.currentInterval = Rand.Range(0, this.Props.intervalTicks);
				return;
			}
			this.currentInterval = this.Props.intervalTicks;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0007D1C4 File Offset: 0x0007B3C4
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<bool>(ref this.removeNextInterval, "removeNextInterval", false, false);
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0007D1EA File Offset: 0x0007B3EA
		public override string CompDebugString()
		{
			return string.Format("currentInterval: {0}\nremove: {1}", this.currentInterval, this.removeNextInterval);
		}

		// Token: 0x040010AE RID: 4270
		public int currentInterval;

		// Token: 0x040010AF RID: 4271
		public bool removeNextInterval;
	}
}

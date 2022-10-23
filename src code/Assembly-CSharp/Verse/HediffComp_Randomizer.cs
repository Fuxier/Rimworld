using System;

namespace Verse
{
	// Token: 0x02000311 RID: 785
	public abstract class HediffComp_Randomizer : HediffComp
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600155F RID: 5471 RVA: 0x000804EC File Offset: 0x0007E6EC
		private HediffCompProperties_Randomizer Props
		{
			get
			{
				return (HediffCompProperties_Randomizer)this.props;
			}
		}

		// Token: 0x06001560 RID: 5472
		public abstract void Randomize();

		// Token: 0x06001561 RID: 5473 RVA: 0x000804F9 File Offset: 0x0007E6F9
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.SetNextRandomizationTick();
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00080507 File Offset: 0x0007E707
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (Find.TickManager.TicksGame >= this.nextRandomizationTick)
			{
				this.Randomize();
				this.SetNextRandomizationTick();
			}
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0008052E File Offset: 0x0007E72E
		public void SetNextRandomizationTick()
		{
			this.nextRandomizationTick = Find.TickManager.TicksGame + this.Props.ticksToRandomize.RandomInRange;
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00080551 File Offset: 0x0007E751
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.nextRandomizationTick, "nextRandomizationTick", 0, false);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0008056B File Offset: 0x0007E76B
		public override string CompDebugString()
		{
			return string.Format("ticks until randomization: {0}", this.nextRandomizationTick - Find.TickManager.TicksGame);
		}

		// Token: 0x04001137 RID: 4407
		private int nextRandomizationTick;
	}
}

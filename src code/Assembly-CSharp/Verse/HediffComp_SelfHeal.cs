using System;

namespace Verse
{
	// Token: 0x0200031D RID: 797
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x00080A03 File Offset: 0x0007EC03
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00080A10 File Offset: 0x0007EC10
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00080A24 File Offset: 0x0007EC24
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksSinceHeal++;
			if (this.ticksSinceHeal > this.Props.healIntervalTicksStanding)
			{
				severityAdjustment -= this.Props.healAmount;
				this.ticksSinceHeal = 0;
			}
		}

		// Token: 0x04001145 RID: 4421
		public int ticksSinceHeal;
	}
}

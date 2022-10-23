using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000419 RID: 1049
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x000B7868 File Offset: 0x000B5A68
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000B78CC File Offset: 0x000B5ACC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}

		// Token: 0x040014EF RID: 5359
		protected CompPowerTrader powerComp;

		// Token: 0x040014F0 RID: 5360
		protected CompRefuelable refuelableComp;

		// Token: 0x040014F1 RID: 5361
		protected CompBreakdownable breakdownableComp;
	}
}

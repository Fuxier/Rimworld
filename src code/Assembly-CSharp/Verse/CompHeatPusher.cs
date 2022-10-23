using System;

namespace Verse
{
	// Token: 0x02000418 RID: 1048
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x000B7784 File Offset: 0x000B5984
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x000B7794 File Offset: 0x000B5994
		protected virtual bool ShouldPushHeatNow
		{
			get
			{
				if (!this.parent.SpawnedOrAnyParentSpawned)
				{
					return false;
				}
				CompProperties_HeatPusher props = this.Props;
				float ambientTemperature = this.parent.AmbientTemperature;
				return ambientTemperature < props.heatPushMaxTemperature && ambientTemperature > props.heatPushMinTemperature;
			}
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000B77D8 File Offset: 0x000B59D8
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000B7829 File Offset: 0x000B5A29
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.1666665f);
			}
		}

		// Token: 0x040014EE RID: 5358
		private const int HeatPushInterval = 60;
	}
}

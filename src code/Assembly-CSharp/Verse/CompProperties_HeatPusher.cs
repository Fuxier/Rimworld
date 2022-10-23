using System;

namespace Verse
{
	// Token: 0x020000C1 RID: 193
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x0600060A RID: 1546 RVA: 0x0002087D File Offset: 0x0001EA7D
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}

		// Token: 0x0400037C RID: 892
		public float heatPerSecond;

		// Token: 0x0400037D RID: 893
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x0400037E RID: 894
		public float heatPushMinTemperature = -99999f;
	}
}

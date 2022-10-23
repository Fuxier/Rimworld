using System;

namespace Verse
{
	// Token: 0x020002F5 RID: 757
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x06001505 RID: 5381 RVA: 0x0007EE84 File Offset: 0x0007D084
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}

		// Token: 0x040010F9 RID: 4345
		public float severityPerDayGrowing;

		// Token: 0x040010FA RID: 4346
		public float severityPerDayRemission;

		// Token: 0x040010FB RID: 4347
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x040010FC RID: 4348
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);
	}
}

using System;

namespace Verse
{
	// Token: 0x020002D4 RID: 724
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x0600149F RID: 5279 RVA: 0x0007D776 File Offset: 0x0007B976
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		// Token: 0x040010C1 RID: 4289
		public IntRange disappearsAfterTicks;

		// Token: 0x040010C2 RID: 4290
		public bool showRemainingTime;

		// Token: 0x040010C3 RID: 4291
		public bool canUseDecimalsShortForm;

		// Token: 0x040010C4 RID: 4292
		public MentalStateDef requiredMentalState;

		// Token: 0x040010C5 RID: 4293
		[MustTranslate]
		public string messageOnDisappear;
	}
}

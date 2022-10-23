using System;

namespace Verse
{
	// Token: 0x020002C7 RID: 711
	public class HediffCompProperties_CauseMentalState : HediffCompProperties
	{
		// Token: 0x06001473 RID: 5235 RVA: 0x0007CD71 File Offset: 0x0007AF71
		public HediffCompProperties_CauseMentalState()
		{
			this.compClass = typeof(HediffComp_CauseMentalState);
		}

		// Token: 0x040010A6 RID: 4262
		public MentalStateDef animalMentalState;

		// Token: 0x040010A7 RID: 4263
		public MentalStateDef animalMentalStateAlias;

		// Token: 0x040010A8 RID: 4264
		public MentalStateDef humanMentalState;

		// Token: 0x040010A9 RID: 4265
		public LetterDef letterDef;

		// Token: 0x040010AA RID: 4266
		public float mtbDaysToCauseMentalState;

		// Token: 0x040010AB RID: 4267
		public bool endMentalStateOnCure = true;
	}
}

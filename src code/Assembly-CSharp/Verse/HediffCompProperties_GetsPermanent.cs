using System;

namespace Verse
{
	// Token: 0x020002E4 RID: 740
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x060014D7 RID: 5335 RVA: 0x0007E368 File Offset: 0x0007C568
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}

		// Token: 0x040010DB RID: 4315
		public float becomePermanentChanceFactor = 1f;

		// Token: 0x040010DC RID: 4316
		[MustTranslate]
		public string permanentLabel;

		// Token: 0x040010DD RID: 4317
		[MustTranslate]
		public string instantlyPermanentLabel;
	}
}

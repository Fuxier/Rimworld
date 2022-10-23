using System;

namespace Verse
{
	// Token: 0x02000357 RID: 855
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x060016DF RID: 5855 RVA: 0x00086281 File Offset: 0x00084481
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		// Token: 0x040011B2 RID: 4530
		private float chance = 1f;
	}
}

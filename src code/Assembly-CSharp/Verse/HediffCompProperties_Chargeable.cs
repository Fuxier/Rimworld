using System;

namespace Verse
{
	// Token: 0x020002D0 RID: 720
	public class HediffCompProperties_Chargeable : HediffCompProperties
	{
		// Token: 0x0600148D RID: 5261 RVA: 0x0007D452 File Offset: 0x0007B652
		public HediffCompProperties_Chargeable()
		{
			this.compClass = typeof(HediffComp_Chargeable);
		}

		// Token: 0x040010B9 RID: 4281
		public int ticksToFullCharge = -1;

		// Token: 0x040010BA RID: 4282
		public float initialCharge;

		// Token: 0x040010BB RID: 4283
		public float fullChargeAmount = 1f;

		// Token: 0x040010BC RID: 4284
		public float minChargeToActivate;

		// Token: 0x040010BD RID: 4285
		public string labelInBrackets;
	}
}

using System;

namespace Verse
{
	// Token: 0x020002FB RID: 763
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x0600151A RID: 5402 RVA: 0x0007F454 File Offset: 0x0007D654
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}

		// Token: 0x04001107 RID: 4359
		public float infectionChance = 0.5f;
	}
}

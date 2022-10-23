using System;

namespace Verse
{
	// Token: 0x020002FE RID: 766
	public class HediffCompProperties_KillAfterDays : HediffCompProperties
	{
		// Token: 0x06001528 RID: 5416 RVA: 0x0007F894 File Offset: 0x0007DA94
		public HediffCompProperties_KillAfterDays()
		{
			this.compClass = typeof(HediffComp_KillAfterDays);
		}

		// Token: 0x04001110 RID: 4368
		public int days;
	}
}

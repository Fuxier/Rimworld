using System;

namespace Verse
{
	// Token: 0x020002E9 RID: 745
	public class HediffCompProperties_GiveHediff : HediffCompProperties
	{
		// Token: 0x060014E8 RID: 5352 RVA: 0x0007E7E1 File Offset: 0x0007C9E1
		public HediffCompProperties_GiveHediff()
		{
			this.compClass = typeof(HediffComp_GiveHediff);
		}

		// Token: 0x040010E9 RID: 4329
		public HediffDef hediffDef;

		// Token: 0x040010EA RID: 4330
		public bool skipIfAlreadyExists;
	}
}

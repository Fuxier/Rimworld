using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002CE RID: 718
	public class HediffCompProperties_ChangeNeed : HediffCompProperties
	{
		// Token: 0x06001488 RID: 5256 RVA: 0x0007D3CF File Offset: 0x0007B5CF
		public HediffCompProperties_ChangeNeed()
		{
			this.compClass = typeof(HediffComp_ChangeNeed);
		}

		// Token: 0x040010B6 RID: 4278
		public NeedDef needDef;

		// Token: 0x040010B7 RID: 4279
		public float percentPerDay;
	}
}

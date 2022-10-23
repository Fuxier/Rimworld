using System;

namespace Verse
{
	// Token: 0x020002C9 RID: 713
	public class HediffCompProperties_ChanceToRemove : HediffCompProperties
	{
		// Token: 0x06001479 RID: 5241 RVA: 0x0007D10C File Offset: 0x0007B30C
		public HediffCompProperties_ChanceToRemove()
		{
			this.compClass = typeof(HediffComp_ChanceToRemove);
		}

		// Token: 0x040010AC RID: 4268
		public int intervalTicks;

		// Token: 0x040010AD RID: 4269
		public float chance;
	}
}

using System;

namespace Verse
{
	// Token: 0x0200031C RID: 796
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x0600157F RID: 5503 RVA: 0x000809D8 File Offset: 0x0007EBD8
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		// Token: 0x04001143 RID: 4419
		public int healIntervalTicksStanding = 50;

		// Token: 0x04001144 RID: 4420
		public float healAmount = 1f;
	}
}

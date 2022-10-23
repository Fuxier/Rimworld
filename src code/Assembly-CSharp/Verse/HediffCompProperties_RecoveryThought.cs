using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000316 RID: 790
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x06001571 RID: 5489 RVA: 0x000807E8 File Offset: 0x0007E9E8
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		// Token: 0x0400113D RID: 4413
		public ThoughtDef thought;
	}
}

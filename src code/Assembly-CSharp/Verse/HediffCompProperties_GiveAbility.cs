using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002E7 RID: 743
	public class HediffCompProperties_GiveAbility : HediffCompProperties
	{
		// Token: 0x060014E3 RID: 5347 RVA: 0x0007E69D File Offset: 0x0007C89D
		public HediffCompProperties_GiveAbility()
		{
			this.compClass = typeof(HediffComp_GiveAbility);
		}

		// Token: 0x040010E7 RID: 4327
		public AbilityDef abilityDef;

		// Token: 0x040010E8 RID: 4328
		public List<AbilityDef> abilityDefs;
	}
}

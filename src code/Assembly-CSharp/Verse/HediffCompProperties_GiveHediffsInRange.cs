using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002ED RID: 749
	public class HediffCompProperties_GiveHediffsInRange : HediffCompProperties
	{
		// Token: 0x060014F2 RID: 5362 RVA: 0x0007EA84 File Offset: 0x0007CC84
		public HediffCompProperties_GiveHediffsInRange()
		{
			this.compClass = typeof(HediffComp_GiveHediffsInRange);
		}

		// Token: 0x040010EE RID: 4334
		public float range;

		// Token: 0x040010EF RID: 4335
		public TargetingParameters targetingParameters;

		// Token: 0x040010F0 RID: 4336
		public HediffDef hediff;

		// Token: 0x040010F1 RID: 4337
		public ThingDef mote;

		// Token: 0x040010F2 RID: 4338
		public bool hideMoteWhenNotDrafted;

		// Token: 0x040010F3 RID: 4339
		public float initialSeverity = 1f;

		// Token: 0x040010F4 RID: 4340
		public bool onlyPawnsInSameFaction = true;
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000333 RID: 819
	public class HediffCompProperties_RandomizeSeverityPhases : HediffCompProperties_Randomizer
	{
		// Token: 0x060015E1 RID: 5601 RVA: 0x00081DA7 File Offset: 0x0007FFA7
		public HediffCompProperties_RandomizeSeverityPhases()
		{
			this.compClass = typeof(HediffComp_RandomizeSeverityPhases);
		}

		// Token: 0x04001172 RID: 4466
		public List<HediffCompProperties_RandomizeSeverityPhases.Phase> phases;

		// Token: 0x04001173 RID: 4467
		[MustTranslate]
		public string notifyMessage;

		// Token: 0x02001E0A RID: 7690
		public class Phase
		{
			// Token: 0x04007684 RID: 30340
			public HediffCompProperties comp;

			// Token: 0x04007685 RID: 30341
			[MustTranslate]
			public string labelPrefix;

			// Token: 0x04007686 RID: 30342
			[MustTranslate]
			public string descriptionExtra;

			// Token: 0x04007687 RID: 30343
			public float severityPerDay;
		}
	}
}

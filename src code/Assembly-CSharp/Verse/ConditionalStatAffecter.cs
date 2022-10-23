using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200007D RID: 125
	public abstract class ConditionalStatAffecter
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004B9 RID: 1209
		public abstract string Label { get; }

		// Token: 0x060004BA RID: 1210
		public abstract bool Applies(StatRequest req);

		// Token: 0x0400021E RID: 542
		public List<StatModifier> statFactors;

		// Token: 0x0400021F RID: 543
		public List<StatModifier> statOffsets;
	}
}

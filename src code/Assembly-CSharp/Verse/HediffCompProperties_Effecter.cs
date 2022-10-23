using System;

namespace Verse
{
	// Token: 0x020002E0 RID: 736
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x060014CF RID: 5327 RVA: 0x0007E22C File Offset: 0x0007C42C
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}

		// Token: 0x040010D8 RID: 4312
		public EffecterDef stateEffecter;

		// Token: 0x040010D9 RID: 4313
		public IntRange severityIndices = new IntRange(-1, -1);
	}
}

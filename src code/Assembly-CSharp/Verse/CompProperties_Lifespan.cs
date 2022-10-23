using System;

namespace Verse
{
	// Token: 0x020000C2 RID: 194
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x0600060B RID: 1547 RVA: 0x000208AB File Offset: 0x0001EAAB
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}

		// Token: 0x0400037F RID: 895
		public int lifespanTicks = 100;

		// Token: 0x04000380 RID: 896
		public EffecterDef expireEffect;

		// Token: 0x04000381 RID: 897
		public ThingDef plantDefToSpawn;
	}
}

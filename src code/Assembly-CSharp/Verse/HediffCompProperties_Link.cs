using System;

namespace Verse
{
	// Token: 0x02000304 RID: 772
	public class HediffCompProperties_Link : HediffCompProperties
	{
		// Token: 0x06001539 RID: 5433 RVA: 0x0007FBC5 File Offset: 0x0007DDC5
		public HediffCompProperties_Link()
		{
			this.compClass = typeof(HediffComp_Link);
		}

		// Token: 0x04001116 RID: 4374
		public bool showName = true;

		// Token: 0x04001117 RID: 4375
		public float maxDistance = -1f;

		// Token: 0x04001118 RID: 4376
		public bool requireLinkOnOtherPawn = true;

		// Token: 0x04001119 RID: 4377
		public ThingDef customMote;
	}
}

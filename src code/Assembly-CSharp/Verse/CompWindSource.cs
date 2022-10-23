using System;

namespace Verse
{
	// Token: 0x0200041C RID: 1052
	public class CompWindSource : ThingComp
	{
		// Token: 0x06001ED6 RID: 7894 RVA: 0x000B7B69 File Offset: 0x000B5D69
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		// Token: 0x040014F3 RID: 5363
		public float wind;
	}
}

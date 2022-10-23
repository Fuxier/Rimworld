using System;

namespace Verse
{
	// Token: 0x0200013C RID: 316
	public class StyleCategoryPair : IExposable
	{
		// Token: 0x06000813 RID: 2067 RVA: 0x00028BD6 File Offset: 0x00026DD6
		public void ExposeData()
		{
			Scribe_Defs.Look<StyleCategoryDef>(ref this.category, "category");
			Scribe_Defs.Look<ThingStyleDef>(ref this.styleDef, "styleDef");
		}

		// Token: 0x04000827 RID: 2087
		public StyleCategoryDef category;

		// Token: 0x04000828 RID: 2088
		public ThingStyleDef styleDef;
	}
}

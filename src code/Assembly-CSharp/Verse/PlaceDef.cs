using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000121 RID: 289
	public class PlaceDef : Def
	{
		// Token: 0x04000770 RID: 1904
		public RulePack placeRules;

		// Token: 0x04000771 RID: 1905
		[NoTranslate]
		public List<string> tags = new List<string>();
	}
}

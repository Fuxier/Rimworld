using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000105 RID: 261
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x06000710 RID: 1808 RVA: 0x000255DF File Offset: 0x000237DF
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x0400065F RID: 1631
		public bool isGameUniversal;

		// Token: 0x04000660 RID: 1632
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x04000661 RID: 1633
		public bool selfConflicting = true;
	}
}

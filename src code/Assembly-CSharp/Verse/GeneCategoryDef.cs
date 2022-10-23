using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000063 RID: 99
	public class GeneCategoryDef : Def
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x00018317 File Offset: 0x00016517
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (DefDatabase<GeneCategoryDef>.AllDefs.Any((GeneCategoryDef x) => x != this && x.displayPriorityInXenotype == this.displayPriorityInXenotype))
			{
				yield return "Multiple gene categories share the same displayPriorityInXenotype. This can cause display order issues.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000176 RID: 374
		public float displayPriorityInXenotype;

		// Token: 0x04000177 RID: 375
		public float displayPriorityInGenepack;
	}
}

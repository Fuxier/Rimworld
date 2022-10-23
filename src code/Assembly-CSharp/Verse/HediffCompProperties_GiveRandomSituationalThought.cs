using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002F3 RID: 755
	public class HediffCompProperties_GiveRandomSituationalThought : HediffCompProperties
	{
		// Token: 0x060014FD RID: 5373 RVA: 0x0007EDFC File Offset: 0x0007CFFC
		public HediffCompProperties_GiveRandomSituationalThought()
		{
			this.compClass = typeof(HediffComp_GiveRandomSituationalThought);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0007EE14 File Offset: 0x0007D014
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.thoughtDefs.NullOrEmpty<ThoughtDef>())
			{
				yield return "There must be at least one item defined in thoughtDefs";
			}
			yield break;
			yield break;
		}

		// Token: 0x040010F7 RID: 4343
		public List<ThoughtDef> thoughtDefs;
	}
}

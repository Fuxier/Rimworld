using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000CC RID: 204
	public class HediffCompProperties
	{
		// Token: 0x06000626 RID: 1574 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostLoad()
		{
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ResolveReferences(HediffDef parent)
		{
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00021B68 File Offset: 0x0001FD68
		public virtual IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "compClass is null";
			}
			int num;
			for (int i = 0; i < parentDef.comps.Count; i = num + 1)
			{
				if (parentDef.comps[i] != this && parentDef.comps[i].compClass == this.compClass)
				{
					yield return "two comps with same compClass: " + this.compClass;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x040003E0 RID: 992
		[TranslationHandle]
		public Type compClass;
	}
}

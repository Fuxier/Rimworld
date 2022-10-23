using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000114 RID: 276
	public class OrderedTakeGroupDef : Def
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x00025DEB File Offset: 0x00023FEB
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.max <= 0)
			{
				yield return "Max should be greather than zero.";
			}
			yield break;
			yield break;
		}

		// Token: 0x040006C6 RID: 1734
		public int max = 3;
	}
}

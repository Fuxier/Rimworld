using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000455 RID: 1109
	public class DebugTabMenuDef : Def
	{
		// Token: 0x06002230 RID: 8752 RVA: 0x000DA0D1 File Offset: 0x000D82D1
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(DebugTabMenu).IsAssignableFrom(this.menuClass))
			{
				yield return "menuClass does not derive from DebugTabMenu.";
			}
			yield break;
			yield break;
		}

		// Token: 0x040015BF RID: 5567
		public Type menuClass;

		// Token: 0x040015C0 RID: 5568
		public int displayOrder = 99999;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000A5 RID: 165
	public class Editable
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostLoad()
		{
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00019CDC File Offset: 0x00017EDC
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}
	}
}

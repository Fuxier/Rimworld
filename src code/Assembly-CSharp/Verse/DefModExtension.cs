using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000276 RID: 630
	public abstract class DefModExtension
	{
		// Token: 0x06001203 RID: 4611 RVA: 0x00019CDC File Offset: 0x00017EDC
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}
	}
}

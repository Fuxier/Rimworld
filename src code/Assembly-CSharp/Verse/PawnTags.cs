using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200005F RID: 95
	public class PawnTags : IExposable
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x000181EE File Offset: 0x000163EE
		public bool Contains(string tag)
		{
			return this.tags.Contains(tag);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000181FC File Offset: 0x000163FC
		public void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0400016D RID: 365
		public List<string> tags;
	}
}

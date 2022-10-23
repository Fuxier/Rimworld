using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200006B RID: 107
	public class TagFilter
	{
		// Token: 0x06000473 RID: 1139 RVA: 0x000199C8 File Offset: 0x00017BC8
		public bool Allows(List<string> otherTags)
		{
			if (otherTags != null)
			{
				for (int i = 0; i < otherTags.Count; i++)
				{
					if (this.tags.Contains(otherTags[i]))
					{
						return this.whitelist;
					}
				}
			}
			return !this.whitelist;
		}

		// Token: 0x040001E7 RID: 487
		public List<string> tags = new List<string>();

		// Token: 0x040001E8 RID: 488
		public bool whitelist = true;
	}
}

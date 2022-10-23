using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000244 RID: 580
	public class RegionLinkDatabase
	{
		// Token: 0x06001077 RID: 4215 RVA: 0x0006058C File Offset: 0x0005E78C
		public RegionLink LinkFrom(EdgeSpan span)
		{
			ulong key = span.UniqueHashCode();
			RegionLink regionLink;
			if (!this.links.TryGetValue(key, out regionLink))
			{
				regionLink = new RegionLink();
				regionLink.span = span;
				this.links.Add(key, regionLink);
			}
			return regionLink;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x000605CC File Offset: 0x0005E7CC
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x000605E0 File Offset: 0x0005E7E0
		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ulong, RegionLink> keyValuePair in this.links)
			{
				stringBuilder.AppendLine(keyValuePair.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04000E76 RID: 3702
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();
	}
}

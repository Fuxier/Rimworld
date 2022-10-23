using System;

namespace Verse
{
	// Token: 0x02000249 RID: 585
	public abstract class RegionProcessorDelegateCache
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x00060F25 File Offset: 0x0005F125
		public RegionProcessor RegionProcessorDelegate
		{
			get
			{
				return this.regionProcessor;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00060F2D File Offset: 0x0005F12D
		public RegionEntryPredicate RegionEntryPredicateDelegate
		{
			get
			{
				return this.regionEntryPredicate;
			}
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00060F35 File Offset: 0x0005F135
		public RegionProcessorDelegateCache()
		{
			this.regionProcessor = new RegionProcessor(this.RegionProcessor);
			this.regionEntryPredicate = new RegionEntryPredicate(this.RegionEntryPredicate);
		}

		// Token: 0x06001096 RID: 4246
		protected abstract bool RegionEntryPredicate(Region from, Region to);

		// Token: 0x06001097 RID: 4247
		protected abstract bool RegionProcessor(Region reg);

		// Token: 0x04000E7F RID: 3711
		private RegionProcessor regionProcessor;

		// Token: 0x04000E80 RID: 3712
		private RegionEntryPredicate regionEntryPredicate;
	}
}

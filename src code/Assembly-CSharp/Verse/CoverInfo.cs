using System;

namespace Verse
{
	// Token: 0x0200059A RID: 1434
	public struct CoverInfo
	{
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x001155A6 File Offset: 0x001137A6
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002BB5 RID: 11189 RVA: 0x001155AE File Offset: 0x001137AE
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002BB6 RID: 11190 RVA: 0x001155B6 File Offset: 0x001137B6
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x001155C3 File Offset: 0x001137C3
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x04001CB2 RID: 7346
		private Thing thingInt;

		// Token: 0x04001CB3 RID: 7347
		private float blockChanceInt;
	}
}

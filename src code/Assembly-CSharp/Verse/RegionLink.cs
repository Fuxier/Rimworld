using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000243 RID: 579
	public class RegionLink
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x00060313 File Offset: 0x0005E513
		// (set) Token: 0x0600106E RID: 4206 RVA: 0x0006031D File Offset: 0x0005E51D
		public Region RegionA
		{
			get
			{
				return this.regions[0];
			}
			set
			{
				this.regions[0] = value;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x00060328 File Offset: 0x0005E528
		// (set) Token: 0x06001070 RID: 4208 RVA: 0x00060332 File Offset: 0x0005E532
		public Region RegionB
		{
			get
			{
				return this.regions[1];
			}
			set
			{
				this.regions[1] = value;
			}
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00060340 File Offset: 0x0005E540
		public void Register(Region reg)
		{
			if (this.regions[0] == reg || this.regions[1] == reg)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to double-register region ",
					reg.ToString(),
					" in ",
					this
				}));
				return;
			}
			if (this.RegionA == null || !this.RegionA.valid)
			{
				this.RegionA = reg;
				return;
			}
			if (this.RegionB == null || !this.RegionB.valid)
			{
				this.RegionB = reg;
				return;
			}
			Log.Error(string.Concat(new object[]
			{
				"Could not register region ",
				reg.ToString(),
				" in link ",
				this,
				": > 2 regions on link!\nRegionA: ",
				this.RegionA.DebugString,
				"\nRegionB: ",
				this.RegionB.DebugString
			}));
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00060424 File Offset: 0x0005E624
		public void Deregister(Region reg)
		{
			if (this.RegionA == reg)
			{
				this.RegionA = null;
				if (this.RegionB == null)
				{
					reg.Map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
					return;
				}
			}
			else if (this.RegionB == reg)
			{
				this.RegionB = null;
				if (this.RegionA == null)
				{
					reg.Map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
				}
			}
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00060484 File Offset: 0x0005E684
		public Region GetOtherRegion(Region reg)
		{
			if (reg != this.RegionA)
			{
				return this.RegionA;
			}
			return this.RegionB;
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0006049C File Offset: 0x0005E69C
		public ulong UniqueHashCode()
		{
			return this.span.UniqueHashCode();
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x000604AC File Offset: 0x0005E6AC
		public override string ToString()
		{
			string text = (from r in this.regions
			where r != null
			select r.id.ToString()).ToCommaList(false, false);
			string text2 = string.Concat(new object[]
			{
				"span=",
				this.span.ToString(),
				" hash=",
				this.UniqueHashCode()
			});
			return string.Concat(new string[]
			{
				"(",
				text2,
				", regions=",
				text,
				")"
			});
		}

		// Token: 0x04000E74 RID: 3700
		public Region[] regions = new Region[2];

		// Token: 0x04000E75 RID: 3701
		public EdgeSpan span;
	}
}

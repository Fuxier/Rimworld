using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001A5 RID: 421
	public class PenMarkerState
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x0004153C File Offset: 0x0003F73C
		public bool Enclosed
		{
			get
			{
				return this.Calc().Enclosed;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00041549 File Offset: 0x0003F749
		public bool Unenclosed
		{
			get
			{
				return !this.Enclosed;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00041554 File Offset: 0x0003F754
		public bool PassableDoors
		{
			get
			{
				return this.Calc().PassableDoors;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00041561 File Offset: 0x0003F761
		public bool HasOutsideAccess
		{
			get
			{
				return !this.Enclosed || this.Calc().ImpassableDoors;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x00041578 File Offset: 0x0003F778
		public List<Region> DirectlyConnectedRegions
		{
			get
			{
				return this.Calc().DirectlyConnectedRegions;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x00041585 File Offset: 0x0003F785
		public HashSet<Region> ConnectedRegions
		{
			get
			{
				return this.Calc().ConnectedRegions;
			}
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00041592 File Offset: 0x0003F792
		public bool ContainsConnectedRegion(Region r)
		{
			return this.Calc().ContainsConnectedRegion(r);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x000415A0 File Offset: 0x0003F7A0
		public PenMarkerState(CompAnimalPenMarker marker)
		{
			this.marker = marker;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x000415B0 File Offset: 0x0003F7B0
		private AnimalPenEnclosureStateCalculator Calc()
		{
			if (this.state == null)
			{
				this.state = new AnimalPenEnclosureStateCalculator();
				this.state.Recalulate(this.marker.parent.Position, this.marker.parent.Map);
			}
			else if (this.state.NeedsRecalculation())
			{
				this.state.Recalulate(this.marker.parent.Position, this.marker.parent.Map);
			}
			return this.state;
		}

		// Token: 0x04000AF4 RID: 2804
		private readonly CompAnimalPenMarker marker;

		// Token: 0x04000AF5 RID: 2805
		private AnimalPenEnclosureStateCalculator state;
	}
}

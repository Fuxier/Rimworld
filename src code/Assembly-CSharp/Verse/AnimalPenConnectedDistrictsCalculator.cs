using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020001A2 RID: 418
	public class AnimalPenConnectedDistrictsCalculator : AnimalPenEnclosureCalculator
	{
		// Token: 0x06000B83 RID: 2947 RVA: 0x0004104D File Offset: 0x0003F24D
		protected override void VisitDirectlyConnectedRegion(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0004104D File Offset: 0x0003F24D
		protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0004104D File Offset: 0x0003F24D
		protected override void VisitPassableDoorway(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00041058 File Offset: 0x0003F258
		private void AddDistrict(Region r)
		{
			District district = r.District;
			if (!this.districtsTmp.Contains(district))
			{
				this.districtsTmp.Add(district);
			}
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00041086 File Offset: 0x0003F286
		public static void InvalidateDistrictCache(District district)
		{
			AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached.Remove(district);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00041094 File Offset: 0x0003F294
		public List<District> CalculateConnectedDistricts(IntVec3 position, Map map)
		{
			District district = position.GetDistrict(map, RegionType.Set_Passable);
			if (Find.TickManager.TicksGame == AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCachedTick)
			{
				if (AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached.ContainsKey(district))
				{
					return AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached[district];
				}
			}
			else
			{
				AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached.Clear();
				AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCachedTick = Find.TickManager.TicksGame;
			}
			this.districtsTmp.Clear();
			if (!base.VisitPen(position, map))
			{
				this.districtsTmp.Clear();
			}
			AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached[district] = this.districtsTmp.ToList<District>();
			return this.districtsTmp;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0004112A File Offset: 0x0003F32A
		public void Reset()
		{
			this.districtsTmp.Clear();
		}

		// Token: 0x04000AE4 RID: 2788
		private readonly List<District> districtsTmp = new List<District>();

		// Token: 0x04000AE5 RID: 2789
		private static readonly Dictionary<District, List<District>> connectedDistrictsForRootCached = new Dictionary<District, List<District>>();

		// Token: 0x04000AE6 RID: 2790
		private static int connectedDistrictsForRootCachedTick = -1;
	}
}

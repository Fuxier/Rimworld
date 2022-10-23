using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001A3 RID: 419
	public class AnimalPenEnclosureStateCalculator : AnimalPenEnclosureCalculator
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x0004115C File Offset: 0x0003F35C
		public bool Enclosed
		{
			get
			{
				return this.enclosed;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x00041164 File Offset: 0x0003F364
		public bool IndirectlyConnected
		{
			get
			{
				return this.indirectlyConnected;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0004116C File Offset: 0x0003F36C
		public bool PassableDoors
		{
			get
			{
				return this.passableDoors.Any<Building_Door>();
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x00041179 File Offset: 0x0003F379
		public bool ImpassableDoors
		{
			get
			{
				return this.impassableDoors.Any<Building_Door>();
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00041186 File Offset: 0x0003F386
		public List<Region> DirectlyConnectedRegions
		{
			get
			{
				return this.directlyConnectedRegions;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0004118E File Offset: 0x0003F38E
		public HashSet<Region> ConnectedRegions
		{
			get
			{
				return this.connectedRegions;
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00041196 File Offset: 0x0003F396
		public bool ContainsConnectedRegion(Region r)
		{
			return this.connectedRegions.Contains(r);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x000411A4 File Offset: 0x0003F3A4
		public bool NeedsRecalculation()
		{
			using (List<Building_Door>.Enumerator enumerator = this.passableDoors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!AnimalPenEnclosureCalculator.RoamerCanPass(enumerator.Current))
					{
						return true;
					}
				}
			}
			using (List<Building_Door>.Enumerator enumerator = this.impassableDoors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (AnimalPenEnclosureCalculator.RoamerCanPass(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00041244 File Offset: 0x0003F444
		public void Recalulate(IntVec3 position, Map map)
		{
			this.indirectlyConnected = false;
			this.passableDoors.Clear();
			this.impassableDoors.Clear();
			this.connectedRegions.Clear();
			this.directlyConnectedRegions.Clear();
			this.enclosed = base.VisitPen(position, map);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00041292 File Offset: 0x0003F492
		protected override void VisitDirectlyConnectedRegion(Region r)
		{
			this.connectedRegions.Add(r);
			this.directlyConnectedRegions.Add(r);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x000412AD File Offset: 0x0003F4AD
		protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
			this.indirectlyConnected = true;
			this.connectedRegions.Add(r);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000412C3 File Offset: 0x0003F4C3
		protected override void VisitPassableDoorway(Region r)
		{
			this.connectedRegions.Add(r);
			this.passableDoors.Add(r.door);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x000412E3 File Offset: 0x0003F4E3
		protected override void VisitImpassableDoorway(Region r)
		{
			this.impassableDoors.Add(r.door);
		}

		// Token: 0x04000AE7 RID: 2791
		private bool enclosed;

		// Token: 0x04000AE8 RID: 2792
		private bool indirectlyConnected;

		// Token: 0x04000AE9 RID: 2793
		private List<Building_Door> passableDoors = new List<Building_Door>();

		// Token: 0x04000AEA RID: 2794
		private List<Building_Door> impassableDoors = new List<Building_Door>();

		// Token: 0x04000AEB RID: 2795
		private List<Region> directlyConnectedRegions = new List<Region>();

		// Token: 0x04000AEC RID: 2796
		private HashSet<Region> connectedRegions = new HashSet<Region>();
	}
}

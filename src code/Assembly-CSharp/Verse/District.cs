using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000232 RID: 562
	public sealed class District
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0005C1E4 File Offset: 0x0005A3E4
		public Map Map
		{
			get
			{
				if (this.mapIndex >= 0)
				{
					return Find.Maps[(int)this.mapIndex];
				}
				return null;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0005C201 File Offset: 0x0005A401
		public RegionType RegionType
		{
			get
			{
				if (!this.regions.Any<Region>())
				{
					return RegionType.None;
				}
				return this.regions[0].type;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x0005C223 File Offset: 0x0005A423
		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x0005C22B File Offset: 0x0005A42B
		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x0005C238 File Offset: 0x0005A438
		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x0005C243 File Offset: 0x0005A443
		public bool Passable
		{
			get
			{
				return this.RegionType.Passable();
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x0005C250 File Offset: 0x0005A450
		public bool IsDoorway
		{
			get
			{
				return this.regions.Count == 1 && this.regions[0].IsDoorway;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0005C273 File Offset: 0x0005A473
		// (set) Token: 0x06000FDB RID: 4059 RVA: 0x0005C27B File Offset: 0x0005A47B
		public Room Room
		{
			get
			{
				return this.roomInt;
			}
			set
			{
				if (value == this.roomInt)
				{
					return;
				}
				if (this.roomInt != null)
				{
					this.roomInt.RemoveDistrict(this);
				}
				this.roomInt = value;
				if (this.roomInt != null)
				{
					this.roomInt.AddDistrict(this);
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x0005C2B8 File Offset: 0x0005A4B8
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.regions.Count; i++)
					{
						this.cachedCellCount += this.regions[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x0005C310 File Offset: 0x0005A510
		public List<District> Neighbors
		{
			get
			{
				this.uniqueNeighborsSet.Clear();
				this.uniqueNeighbors.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					foreach (Region region in this.regions[i].Neighbors)
					{
						if (this.uniqueNeighborsSet.Add(region.District) && region.District != this)
						{
							this.uniqueNeighbors.Add(region.District);
						}
					}
				}
				this.uniqueNeighborsSet.Clear();
				return this.uniqueNeighbors;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0005C3CC File Offset: 0x0005A5CC
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.regions.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.regions[i].Cells)
					{
						yield return intVec;
					}
					IEnumerator<IntVec3> enumerator = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0005C3DC File Offset: 0x0005A5DC
		public static District MakeNew(Map map)
		{
			District district = new District();
			district.mapIndex = (sbyte)map.Index;
			district.ID = District.nextDistrictID;
			District.nextDistrictID++;
			return district;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0005C408 File Offset: 0x0005A608
		public void AddRegion(Region r)
		{
			if (this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same region twice to District. region=",
					r,
					", district=",
					this
				}));
				return;
			}
			this.regions.Add(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge++;
			}
			if (this.regions.Count == 1)
			{
				this.Map.regionGrid.allDistricts.Add(this);
			}
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0005C494 File Offset: 0x0005A694
		public void RemoveRegion(Region r)
		{
			if (!this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove region from District but this region is not here. region=",
					r,
					", district=",
					this
				}));
				return;
			}
			this.regions.Remove(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge--;
			}
			if (this.regions.Count == 0)
			{
				this.Room = null;
				this.cachedOpenRoofCount = -1;
				this.cachedOpenRoofState = null;
				this.Map.regionGrid.allDistricts.Remove(this);
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0005C534 File Offset: 0x0005A734
		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0005C53D File Offset: 0x0005A73D
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			this.Room.Notify_RoofChanged();
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0005C558 File Offset: 0x0005A758
		public void Notify_RoomShapeOrContainedBedsChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			AnimalPenConnectedDistrictsCalculator.InvalidateDistrictCache(this);
			this.lastChangeTick = Find.TickManager.TicksGame;
			FacilitiesUtility.NotifyFacilitiesAboutChangedLOSBlockers(this.regions);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0005C590 File Offset: 0x0005A790
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for district ",
					this.ID,
					", but mapIndex=",
					this.mapIndex
				}));
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0005C5F4 File Offset: 0x0005A7F4
		public int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount == -1 && this.cachedOpenRoofState == null)
			{
				this.cachedOpenRoofCount = 0;
				this.cachedOpenRoofState = this.Cells.GetEnumerator();
			}
			if (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState != null)
			{
				RoofGrid roofGrid = this.Map.roofGrid;
				while (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState.MoveNext())
				{
					if (!roofGrid.Roofed(this.cachedOpenRoofState.Current))
					{
						this.cachedOpenRoofCount++;
					}
				}
				if (this.cachedOpenRoofCount < threshold)
				{
					this.cachedOpenRoofState = null;
				}
			}
			return this.cachedOpenRoofCount;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0005C698 File Offset: 0x0005A898
		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0005C6F4 File Offset: 0x0005A8F4
		internal string DebugString()
		{
			return string.Concat(new object[]
			{
				"District ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  RegionCount=",
				this.RegionCount,
				"\n  RegionType=",
				this.RegionType,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCountStopAt(int.MaxValue),
				"\n  numRegionsTouchingMapEdge=",
				this.numRegionsTouchingMapEdge,
				"\n  lastChangeTick=",
				this.lastChangeTick,
				"\n  Room=",
				(this.Room != null) ? this.Room.ID.ToString() : "null"
			});
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0005C7FC File Offset: 0x0005A9FC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"District(districtID=",
				this.ID,
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				", lastChangeTick=",
				this.lastChangeTick,
				")"
			});
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0005C886 File Offset: 0x0005AA86
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		// Token: 0x04000E11 RID: 3601
		public sbyte mapIndex = -1;

		// Token: 0x04000E12 RID: 3602
		private Room roomInt;

		// Token: 0x04000E13 RID: 3603
		private List<Region> regions = new List<Region>();

		// Token: 0x04000E14 RID: 3604
		public int ID = -16161616;

		// Token: 0x04000E15 RID: 3605
		public int lastChangeTick = -1;

		// Token: 0x04000E16 RID: 3606
		private int numRegionsTouchingMapEdge;

		// Token: 0x04000E17 RID: 3607
		private int cachedOpenRoofCount = -1;

		// Token: 0x04000E18 RID: 3608
		private IEnumerator<IntVec3> cachedOpenRoofState;

		// Token: 0x04000E19 RID: 3609
		private int cachedCellCount = -1;

		// Token: 0x04000E1A RID: 3610
		public int newOrReusedRoomIndex = -1;

		// Token: 0x04000E1B RID: 3611
		private static int nextDistrictID;

		// Token: 0x04000E1C RID: 3612
		private HashSet<District> uniqueNeighborsSet = new HashSet<District>();

		// Token: 0x04000E1D RID: 3613
		private List<District> uniqueNeighbors = new List<District>();
	}
}

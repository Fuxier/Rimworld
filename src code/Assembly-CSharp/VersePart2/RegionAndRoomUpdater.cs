using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200023E RID: 574
	public class RegionAndRoomUpdater
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x0005EE72 File Offset: 0x0005D072
		// (set) Token: 0x06001044 RID: 4164 RVA: 0x0005EE7A File Offset: 0x0005D07A
		public bool Enabled
		{
			get
			{
				return this.enabledInt;
			}
			set
			{
				this.enabledInt = value;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0005EE83 File Offset: 0x0005D083
		public bool AnythingToRebuild
		{
			get
			{
				return this.map.regionDirtyer.AnyDirty || !this.initialized;
			}
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0005EEA4 File Offset: 0x0005D0A4
		public RegionAndRoomUpdater(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0005EF28 File Offset: 0x0005D128
		public void RebuildAllRegionsAndRooms()
		{
			if (!this.Enabled)
			{
				Log.Warning("Called RebuildAllRegionsAndRooms() but RegionAndRoomUpdater is disabled. Regions won't be rebuilt.");
			}
			this.map.temperatureCache.ResetTemperatureCache();
			this.map.regionDirtyer.SetAllDirty();
			this.TryRebuildDirtyRegionsAndRooms();
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0005EF64 File Offset: 0x0005D164
		public void TryRebuildDirtyRegionsAndRooms()
		{
			if (this.working || !this.Enabled)
			{
				return;
			}
			this.working = true;
			if (!this.initialized)
			{
				this.RebuildAllRegionsAndRooms();
			}
			if (!this.map.regionDirtyer.AnyDirty)
			{
				this.working = false;
				return;
			}
			try
			{
				this.RegenerateNewRegionsFromDirtyCells();
				this.CreateOrUpdateRooms();
			}
			catch (Exception arg)
			{
				Log.Error("Exception while rebuilding dirty regions: " + arg);
			}
			this.newRegions.Clear();
			this.map.regionDirtyer.SetAllClean();
			this.initialized = true;
			this.working = false;
			if (DebugSettings.detectRegionListersBugs)
			{
				Autotests_RegionListers.CheckBugs(this.map);
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0005F020 File Offset: 0x0005D220
		private void RegenerateNewRegionsFromDirtyCells()
		{
			this.newRegions.Clear();
			List<IntVec3> dirtyCells = this.map.regionDirtyer.DirtyCells;
			for (int i = 0; i < dirtyCells.Count; i++)
			{
				IntVec3 intVec = dirtyCells[i];
				if (intVec.GetRegion(this.map, RegionType.Set_All) == null)
				{
					Region region = this.map.regionMaker.TryGenerateRegionFrom(intVec);
					if (region != null)
					{
						this.newRegions.Add(region);
					}
				}
			}
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0005F094 File Offset: 0x0005D294
		private void CreateOrUpdateRooms()
		{
			this.newDistricts.Clear();
			this.reusedOldDistricts.Clear();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			int numRegionGroups = this.CombineNewRegionsIntoContiguousGroups();
			this.CreateOrAttachToExistingDistricts(numRegionGroups);
			int numRooms = this.CombineNewAndReusedDistrictsIntoContiguousRooms();
			this.CreateOrAttachToExistingRooms(numRooms);
			this.NotifyAffectedDistrictsAndRoomsAndUpdateTemperature();
			this.newDistricts.Clear();
			this.reusedOldDistricts.Clear();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0005F11C File Offset: 0x0005D31C
		private int CombineNewRegionsIntoContiguousGroups()
		{
			int num = 0;
			for (int i = 0; i < this.newRegions.Count; i++)
			{
				if (this.newRegions[i].newRegionGroupIndex < 0)
				{
					RegionTraverser.FloodAndSetNewRegionIndex(this.newRegions[i], num);
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0005F16C File Offset: 0x0005D36C
		private void CreateOrAttachToExistingDistricts(int numRegionGroups)
		{
			for (int i = 0; i < numRegionGroups; i++)
			{
				this.currentRegionGroup.Clear();
				for (int j = 0; j < this.newRegions.Count; j++)
				{
					if (this.newRegions[j].newRegionGroupIndex == i)
					{
						this.currentRegionGroup.Add(this.newRegions[j]);
					}
				}
				if (!this.currentRegionGroup[0].type.AllowsMultipleRegionsPerDistrict())
				{
					if (this.currentRegionGroup.Count != 1)
					{
						Log.Error("Region type doesn't allow multiple regions per room but there are >1 regions in this group.");
					}
					District district = District.MakeNew(this.map);
					this.currentRegionGroup[0].District = district;
					this.newDistricts.Add(district);
				}
				else
				{
					bool flag;
					District district2 = this.FindCurrentRegionGroupNeighborWithMostRegions(out flag);
					if (district2 == null)
					{
						District item = RegionTraverser.FloodAndSetDistricts(this.currentRegionGroup[0], this.map, null);
						this.newDistricts.Add(item);
					}
					else if (!flag)
					{
						for (int k = 0; k < this.currentRegionGroup.Count; k++)
						{
							this.currentRegionGroup[k].District = district2;
						}
						this.reusedOldDistricts.Add(district2);
					}
					else
					{
						RegionTraverser.FloodAndSetDistricts(this.currentRegionGroup[0], this.map, district2);
						this.reusedOldDistricts.Add(district2);
					}
				}
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0005F2D4 File Offset: 0x0005D4D4
		private int CombineNewAndReusedDistrictsIntoContiguousRooms()
		{
			int num = 0;
			foreach (District district in this.reusedOldDistricts)
			{
				district.newOrReusedRoomIndex = -1;
			}
			foreach (District district2 in this.reusedOldDistricts.Concat(this.newDistricts))
			{
				if (district2.newOrReusedRoomIndex < 0)
				{
					this.tmpDistrictStack.Clear();
					this.tmpDistrictStack.Push(district2);
					district2.newOrReusedRoomIndex = num;
					while (this.tmpDistrictStack.Count != 0)
					{
						District district3 = this.tmpDistrictStack.Pop();
						foreach (District district4 in district3.Neighbors)
						{
							if (district4.newOrReusedRoomIndex < 0 && this.ShouldBeInTheSameRoom(district3, district4))
							{
								district4.newOrReusedRoomIndex = num;
								this.tmpDistrictStack.Push(district4);
							}
						}
					}
					this.tmpDistrictStack.Clear();
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0005F42C File Offset: 0x0005D62C
		private void CreateOrAttachToExistingRooms(int numRooms)
		{
			for (int i = 0; i < numRooms; i++)
			{
				this.currentDistrictGroup.Clear();
				foreach (District district in this.reusedOldDistricts)
				{
					if (district.newOrReusedRoomIndex == i)
					{
						this.currentDistrictGroup.Add(district);
					}
				}
				for (int j = 0; j < this.newDistricts.Count; j++)
				{
					if (this.newDistricts[j].newOrReusedRoomIndex == i)
					{
						this.currentDistrictGroup.Add(this.newDistricts[j]);
					}
				}
				bool flag;
				Room room = this.FindCurrentRoomNeighborWithMostRegions(out flag);
				if (room == null)
				{
					Room room2 = Room.MakeNew(this.map);
					this.FloodAndSetRooms(this.currentDistrictGroup[0], room2);
					this.newRooms.Add(room2);
				}
				else if (!flag)
				{
					for (int k = 0; k < this.currentDistrictGroup.Count; k++)
					{
						this.currentDistrictGroup[k].Room = room;
					}
					this.reusedOldRooms.Add(room);
				}
				else
				{
					this.FloodAndSetRooms(this.currentDistrictGroup[0], room);
					this.reusedOldRooms.Add(room);
				}
			}
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0005F590 File Offset: 0x0005D790
		private void FloodAndSetRooms(District start, Room room)
		{
			this.tmpDistrictStack.Clear();
			this.tmpDistrictStack.Push(start);
			this.tmpVisitedDistricts.Clear();
			this.tmpVisitedDistricts.Add(start);
			while (this.tmpDistrictStack.Count != 0)
			{
				District district = this.tmpDistrictStack.Pop();
				district.Room = room;
				foreach (District district2 in district.Neighbors)
				{
					if (!this.tmpVisitedDistricts.Contains(district2) && this.ShouldBeInTheSameRoom(district, district2))
					{
						this.tmpDistrictStack.Push(district2);
						this.tmpVisitedDistricts.Add(district2);
					}
				}
			}
			this.tmpVisitedDistricts.Clear();
			this.tmpDistrictStack.Clear();
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0005F674 File Offset: 0x0005D874
		private void NotifyAffectedDistrictsAndRoomsAndUpdateTemperature()
		{
			foreach (District district in this.reusedOldDistricts)
			{
				district.Notify_RoomShapeOrContainedBedsChanged();
			}
			for (int i = 0; i < this.newDistricts.Count; i++)
			{
				this.newDistricts[i].Notify_RoomShapeOrContainedBedsChanged();
			}
			foreach (Room room in this.reusedOldRooms)
			{
				room.Notify_RoomShapeChanged();
			}
			for (int j = 0; j < this.newRooms.Count; j++)
			{
				Room room2 = this.newRooms[j];
				room2.Notify_RoomShapeChanged();
				float temperature;
				if (this.map.temperatureCache.TryGetAverageCachedRoomTemp(room2, out temperature))
				{
					room2.Temperature = temperature;
				}
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0005F774 File Offset: 0x0005D974
		private District FindCurrentRegionGroupNeighborWithMostRegions(out bool multipleOldNeighborDistricts)
		{
			multipleOldNeighborDistricts = false;
			District district = null;
			for (int i = 0; i < this.currentRegionGroup.Count; i++)
			{
				foreach (Region region in this.currentRegionGroup[i].NeighborsOfSameType)
				{
					if (region.District != null && !this.reusedOldDistricts.Contains(region.District))
					{
						if (district == null)
						{
							district = region.District;
						}
						else if (region.District != district)
						{
							multipleOldNeighborDistricts = true;
							if (region.District.RegionCount > district.RegionCount)
							{
								district = region.District;
							}
						}
					}
				}
			}
			return district;
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0005F834 File Offset: 0x0005DA34
		private Room FindCurrentRoomNeighborWithMostRegions(out bool multipleOldNeighborRooms)
		{
			multipleOldNeighborRooms = false;
			Room room = null;
			for (int i = 0; i < this.currentDistrictGroup.Count; i++)
			{
				foreach (District district in this.currentDistrictGroup[i].Neighbors)
				{
					if (district.Room != null && this.ShouldBeInTheSameRoom(this.currentDistrictGroup[i], district) && !this.reusedOldRooms.Contains(district.Room))
					{
						if (room == null)
						{
							room = district.Room;
						}
						else if (district.Room != room)
						{
							multipleOldNeighborRooms = true;
							if (district.Room.RegionCount > room.RegionCount)
							{
								room = district.Room;
							}
						}
					}
				}
			}
			return room;
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0005F910 File Offset: 0x0005DB10
		private bool ShouldBeInTheSameRoom(District a, District b)
		{
			RegionType regionType = a.RegionType;
			RegionType regionType2 = b.RegionType;
			return (regionType == RegionType.Normal || regionType == RegionType.ImpassableFreeAirExchange || regionType == RegionType.Fence) && (regionType2 == RegionType.Normal || regionType2 == RegionType.ImpassableFreeAirExchange || regionType2 == RegionType.Fence);
		}

		// Token: 0x04000E56 RID: 3670
		private Map map;

		// Token: 0x04000E57 RID: 3671
		private List<Region> newRegions = new List<Region>();

		// Token: 0x04000E58 RID: 3672
		private List<District> newDistricts = new List<District>();

		// Token: 0x04000E59 RID: 3673
		private HashSet<District> reusedOldDistricts = new HashSet<District>();

		// Token: 0x04000E5A RID: 3674
		private List<Room> newRooms = new List<Room>();

		// Token: 0x04000E5B RID: 3675
		private HashSet<Room> reusedOldRooms = new HashSet<Room>();

		// Token: 0x04000E5C RID: 3676
		private List<Region> currentRegionGroup = new List<Region>();

		// Token: 0x04000E5D RID: 3677
		private List<District> currentDistrictGroup = new List<District>();

		// Token: 0x04000E5E RID: 3678
		private Stack<District> tmpDistrictStack = new Stack<District>();

		// Token: 0x04000E5F RID: 3679
		private HashSet<District> tmpVisitedDistricts = new HashSet<District>();

		// Token: 0x04000E60 RID: 3680
		private bool initialized;

		// Token: 0x04000E61 RID: 3681
		private bool working;

		// Token: 0x04000E62 RID: 3682
		private bool enabledInt = true;
	}
}

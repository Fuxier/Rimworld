using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024D RID: 589
	public class Room
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x000612F2 File Offset: 0x0005F4F2
		public List<District> Districts
		{
			get
			{
				return this.districts;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x000612FA File Offset: 0x0005F4FA
		public Map Map
		{
			get
			{
				if (!this.districts.Any<District>())
				{
					return null;
				}
				return this.districts[0].Map;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x0006131C File Offset: 0x0005F51C
		public int DistrictCount
		{
			get
			{
				return this.districts.Count;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00061329 File Offset: 0x0005F529
		public RoomTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x00061331 File Offset: 0x0005F531
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x0006133E File Offset: 0x0005F53E
		public float Temperature
		{
			get
			{
				return this.tempTracker.Temperature;
			}
			set
			{
				this.tempTracker.Temperature = value;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x0006134C File Offset: 0x0005F54C
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.TouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00061375 File Offset: 0x0005F575
		public bool Dereferenced
		{
			get
			{
				return this.RegionCount == 0;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x00061380 File Offset: 0x0005F580
		public bool IsHuge
		{
			get
			{
				return this.RegionCount > 60;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x0006138C File Offset: 0x0005F58C
		public bool IsPrisonCell
		{
			get
			{
				return this.isPrisonCell;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x00061394 File Offset: 0x0005F594
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.districts.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.districts[i].Cells)
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

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x000613A4 File Offset: 0x0005F5A4
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.districts.Count; i++)
					{
						this.cachedCellCount += this.districts[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x000613FC File Offset: 0x0005F5FC
		public Region FirstRegion
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					List<Region> regions = this.districts[i].Regions;
					if (regions.Count > 0)
					{
						return regions[0];
					}
				}
				return null;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00061444 File Offset: 0x0005F644
		public List<Region> Regions
		{
			get
			{
				this.tmpRegions.Clear();
				for (int i = 0; i < this.districts.Count; i++)
				{
					List<Region> regions = this.districts[i].Regions;
					for (int j = 0; j < regions.Count; j++)
					{
						this.tmpRegions.Add(regions[j]);
					}
				}
				return this.tmpRegions;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x000614B0 File Offset: 0x0005F6B0
		public int RegionCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.districts.Count; i++)
				{
					num += this.districts[i].RegionCount;
				}
				return num;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x000614EC File Offset: 0x0005F6EC
		public CellRect ExtentsClose
		{
			get
			{
				CellRect cellRect = new CellRect(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
				foreach (Region region in this.Regions)
				{
					if (region.extentsClose.minX < cellRect.minX)
					{
						cellRect.minX = region.extentsClose.minX;
					}
					if (region.extentsClose.minZ < cellRect.minZ)
					{
						cellRect.minZ = region.extentsClose.minZ;
					}
					if (region.extentsClose.maxX > cellRect.maxX)
					{
						cellRect.maxX = region.extentsClose.maxX;
					}
					if (region.extentsClose.maxZ > cellRect.maxZ)
					{
						cellRect.maxZ = region.extentsClose.maxZ;
					}
				}
				return cellRect;
			}
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x000615F0 File Offset: 0x0005F7F0
		private int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount != -1)
			{
				return this.cachedOpenRoofCount;
			}
			int num = 0;
			for (int i = 0; i < this.districts.Count; i++)
			{
				num += this.districts[i].OpenRoofCountStopAt(threshold);
				if (num >= threshold)
				{
					return num;
				}
				threshold -= num;
			}
			return num;
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x00061646 File Offset: 0x0005F846
		public int OpenRoofCount
		{
			get
			{
				if (this.cachedOpenRoofCount == -1)
				{
					this.cachedOpenRoofCount = this.OpenRoofCountStopAt(int.MaxValue);
				}
				return this.cachedOpenRoofCount;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x00061668 File Offset: 0x0005F868
		public IEnumerable<IntVec3> BorderCells
		{
			get
			{
				foreach (IntVec3 c in this.Cells)
				{
					int num;
					for (int i = 0; i < 8; i = num)
					{
						IntVec3 intVec = c + GenAdj.AdjacentCells[i];
						Region region = (c + GenAdj.AdjacentCells[i]).GetRegion(this.Map, RegionType.Set_Passable);
						if (region == null || region.Room != this)
						{
							yield return intVec;
						}
						num = i + 1;
					}
				}
				IEnumerator<IntVec3> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x00061678 File Offset: 0x0005F878
		public bool TouchesMapEdge
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].TouchesMapEdge)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x000616B1 File Offset: 0x0005F8B1
		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.TouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x000616E8 File Offset: 0x0005F8E8
		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0006170F File Offset: 0x0005F90F
		public IEnumerable<Pawn> Owners
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					yield break;
				}
				if (this.IsHuge)
				{
					yield break;
				}
				if (this.Role != RoomRoleDefOf.Bedroom && this.Role != RoomRoleDefOf.PrisonCell && this.Role != RoomRoleDefOf.Barracks && this.Role != RoomRoleDefOf.PrisonBarracks)
				{
					yield break;
				}
				IEnumerable<Building_Bed> enumerable = from x in this.ContainedBeds
				where x.def.building.bed_humanlike
				select x;
				if (enumerable.Count<Building_Bed>() > 1 && (this.Role == RoomRoleDefOf.Barracks || this.Role == RoomRoleDefOf.PrisonBarracks))
				{
					if ((from b in enumerable
					where b.OwnersForReading.Any<Pawn>()
					select b).Count<Building_Bed>() > 1)
					{
						yield break;
					}
				}
				foreach (Building_Bed building_Bed in enumerable)
				{
					List<Pawn> assignedPawns = building_Bed.OwnersForReading;
					int num;
					for (int i = 0; i < assignedPawns.Count; i = num + 1)
					{
						yield return assignedPawns[i];
						num = i;
					}
					assignedPawns = null;
				}
				IEnumerator<Building_Bed> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x0006171F File Offset: 0x0005F91F
		public IEnumerable<Building_Bed> ContainedBeds
		{
			get
			{
				List<Thing> things = this.ContainedAndAdjacentThings;
				int num;
				for (int i = 0; i < things.Count; i = num + 1)
				{
					Building_Bed building_Bed = things[i] as Building_Bed;
					if (building_Bed != null)
					{
						yield return building_Bed;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0006172F File Offset: 0x0005F92F
		public bool Fogged
		{
			get
			{
				return this.RegionCount != 0 && this.FirstRegion.AnyCell.Fogged(this.Map);
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x00061751 File Offset: 0x0005F951
		public bool IsDoorway
		{
			get
			{
				return this.districts.Count == 1 && this.districts[0].IsDoorway;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x00061774 File Offset: 0x0005F974
		public List<Thing> ContainedAndAdjacentThings
		{
			get
			{
				this.uniqueContainedThingsSet.Clear();
				this.uniqueContainedThings.Clear();
				List<Region> regions = this.Regions;
				for (int i = 0; i < regions.Count; i++)
				{
					List<Thing> allThings = regions[i].ListerThings.AllThings;
					if (allThings != null)
					{
						for (int j = 0; j < allThings.Count; j++)
						{
							Thing item = allThings[j];
							if (this.uniqueContainedThingsSet.Add(item))
							{
								this.uniqueContainedThings.Add(item);
							}
						}
					}
				}
				this.uniqueContainedThingsSet.Clear();
				return this.uniqueContainedThings;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060010C1 RID: 4289 RVA: 0x0006180B File Offset: 0x0005FA0B
		public RoomRoleDef Role
		{
			get
			{
				if (this.statsAndRoleDirty)
				{
					this.UpdateRoomStatsAndRole();
				}
				return this.role;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x00061824 File Offset: 0x0005FA24
		public bool AnyPassable
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].Passable)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x00061860 File Offset: 0x0005FA60
		public bool ProperRoom
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					return false;
				}
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].RegionType == RegionType.Normal)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x000618A4 File Offset: 0x0005FAA4
		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.ID = Room.nextRoomID;
			room.tempTracker = new RoomTempTracker(room, map);
			Room.nextRoomID++;
			return room;
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x000618D0 File Offset: 0x0005FAD0
		public void AddDistrict(District district)
		{
			if (this.districts.Contains(district))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same district twice to Room. district=",
					district,
					", room=",
					this
				}));
				return;
			}
			this.districts.Add(district);
			if (this.districts.Count == 1)
			{
				this.Map.regionGrid.allRooms.Add(this);
			}
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00061944 File Offset: 0x0005FB44
		public void RemoveDistrict(District district)
		{
			if (!this.districts.Contains(district))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove district from Room but this district is not here. district=",
					district,
					", room=",
					this
				}));
				return;
			}
			Map map = this.Map;
			this.districts.Remove(district);
			if (this.districts.Count == 0)
			{
				map.regionGrid.allRooms.Remove(this);
			}
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x000619C2 File Offset: 0x0005FBC2
		public bool PushHeat(float energy)
		{
			if (this.UsesOutdoorTemperature)
			{
				return false;
			}
			this.Temperature += energy / (float)this.CellCount;
			return true;
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x000619E8 File Offset: 0x0005FBE8
		public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
		{
			if (th.def.category != ThingCategory.Mote && th.def.category != ThingCategory.Projectile && th.def.category != ThingCategory.Ethereal && th.def.category != ThingCategory.Pawn)
			{
				if (this.IsDoorway)
				{
					List<Region> regions = this.districts[0].Regions;
					for (int i = 0; i < regions[0].links.Count; i++)
					{
						Region otherRegion = regions[0].links[i].GetOtherRegion(regions[0]);
						if (otherRegion != null && !otherRegion.IsDoorway)
						{
							otherRegion.Room.Notify_ContainedThingSpawnedOrDespawned(th);
						}
					}
				}
				this.statsAndRoleDirty = true;
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00061AAB File Offset: 0x0005FCAB
		public void Notify_TerrainChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00061AAB File Offset: 0x0005FCAB
		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00061AB4 File Offset: 0x0005FCB4
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoofChanged();
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00061AC8 File Offset: 0x0005FCC8
		public void Notify_RoomShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			if (this.Dereferenced)
			{
				this.isPrisonCell = false;
				this.statsAndRoleDirty = true;
				return;
			}
			this.tempTracker.RoomChanged();
			if (Current.ProgramState == ProgramState.Playing && !this.Fogged)
			{
				this.Map.autoBuildRoofAreaSetter.TryGenerateAreaFor(this);
			}
			this.isPrisonCell = false;
			if (Building_Bed.RoomCanBePrisonCell(this))
			{
				List<Thing> containedAndAdjacentThings = this.ContainedAndAdjacentThings;
				for (int i = 0; i < containedAndAdjacentThings.Count; i++)
				{
					Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
					if (building_Bed != null && building_Bed.ForPrisoners)
					{
						this.isPrisonCell = true;
						break;
					}
				}
			}
			List<Thing> list = this.Map.listerThings.ThingsOfDef(ThingDefOf.NutrientPasteDispenser);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Notify_ColorChanged();
			}
			if (Current.ProgramState == ProgramState.Playing && this.isPrisonCell)
			{
				foreach (Building_Bed building_Bed2 in this.ContainedBeds)
				{
					building_Bed2.ForPrisoners = true;
				}
			}
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00061C00 File Offset: 0x0005FE00
		public bool ContainsCell(IntVec3 cell)
		{
			return this.Map != null && cell.GetRoom(this.Map) == this;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00061C1C File Offset: 0x0005FE1C
		public bool ContainsThing(ThingDef def)
		{
			List<Region> regions = this.Regions;
			for (int i = 0; i < regions.Count; i++)
			{
				if (regions[i].ListerThings.ThingsOfDef(def).Any<Thing>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00061C5D File Offset: 0x0005FE5D
		public IEnumerable<Thing> ContainedThings(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			List<Region> regions = this.Regions;
			int num;
			for (int i = 0; i < regions.Count; i = num)
			{
				List<Thing> things = regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < things.Count; j = num)
				{
					if (this.uniqueContainedThingsOfDef.Add(things[j]))
					{
						yield return things[j];
					}
					num = j + 1;
				}
				things = null;
				num = i + 1;
			}
			this.uniqueContainedThingsOfDef.Clear();
			yield break;
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00061C74 File Offset: 0x0005FE74
		public IEnumerable<Thing> ContainedThingsList(IEnumerable<ThingDef> thingDefs)
		{
			foreach (ThingDef def in thingDefs)
			{
				foreach (Thing thing in this.ContainedThings(def))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator2 = null;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00061C8C File Offset: 0x0005FE8C
		public int ThingCount(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			List<Region> regions = this.Regions;
			int num = 0;
			for (int i = 0; i < regions.Count; i++)
			{
				List<Thing> list = regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < list.Count; j++)
				{
					if (this.uniqueContainedThingsOfDef.Add(list[j]))
					{
						num += list[j].stackCount;
					}
				}
			}
			this.uniqueContainedThingsOfDef.Clear();
			return num;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00061D17 File Offset: 0x0005FF17
		public float GetStat(RoomStatDef roomStat)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			if (this.stats == null)
			{
				return roomStat.roomlessScore;
			}
			return this.stats[roomStat];
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00061D44 File Offset: 0x0005FF44
		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = this.isPrisonCell ? Room.PrisonFieldColor : Room.NonPrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color, null);
			Room.fields.Clear();
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00061DB4 File Offset: 0x0005FFB4
		private void UpdateRoomStatsAndRole()
		{
			this.statsAndRoleDirty = false;
			if (this.ProperRoom && this.RegionCount <= 36)
			{
				if (this.stats == null)
				{
					this.stats = new DefMap<RoomStatDef, float>();
				}
				foreach (RoomStatDef roomStatDef in from x in DefDatabase<RoomStatDef>.AllDefs
				orderby x.updatePriority descending
				select x)
				{
					this.stats[roomStatDef] = roomStatDef.Worker.GetScore(this);
				}
				this.role = DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this));
				return;
			}
			this.stats = null;
			this.role = RoomRoleDefOf.None;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00061E98 File Offset: 0x00060098
		public string GetRoomRoleLabel()
		{
			Pawn pawn = null;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in this.Owners)
			{
				if (pawn == null)
				{
					pawn = pawn3;
				}
				else
				{
					pawn2 = pawn3;
				}
			}
			TaggedString taggedString;
			if (pawn == null)
			{
				taggedString = this.Role.PostProcessedLabelCap;
			}
			else if (pawn2 == null)
			{
				taggedString = "SomeonesRoom".Translate(pawn.LabelShort, this.Role.label, pawn.Named("PAWN"));
			}
			else
			{
				taggedString = "CouplesRoom".Translate(pawn.LabelShort, pawn2.LabelShort, this.Role.label, pawn.Named("PAWN1"), pawn2.Named("PAWN2"));
			}
			return taggedString;
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00061F88 File Offset: 0x00060188
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"Room ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  DistrictCount=",
				this.DistrictCount,
				"\n  RegionCount=",
				this.RegionCount,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCount,
				"\n  PsychologicallyOutdoors=",
				this.PsychologicallyOutdoors.ToString(),
				"\n  OutdoorsForWork=",
				this.OutdoorsForWork.ToString(),
				"\n  WellEnclosed=",
				this.ProperRoom.ToString(),
				"\n  ",
				this.tempTracker.DebugString(),
				DebugViewSettings.writeRoomRoles ? ("\n" + this.DebugRolesString()) : ""
			});
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x000620B8 File Offset: 0x000602B8
		private string DebugRolesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ValueTuple<float, RoomRoleDef> valueTuple in from x in DefDatabase<RoomRoleDef>.AllDefs
			select new ValueTuple<float, RoomRoleDef>(x.Worker.GetScore(this), x) into tuple
			orderby tuple.Item1 descending
			select tuple)
			{
				float item = valueTuple.Item1;
				RoomRoleDef item2 = valueTuple.Item2;
				stringBuilder.AppendLine(string.Format("{0}: {1}", item, item2));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00062164 File Offset: 0x00060364
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x000621D4 File Offset: 0x000603D4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Room(roomID=",
				this.ID,
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				")"
			});
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00062247 File Offset: 0x00060447
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478891);
		}

		// Token: 0x04000E8D RID: 3725
		public int ID = -1;

		// Token: 0x04000E8E RID: 3726
		private List<District> districts = new List<District>();

		// Token: 0x04000E8F RID: 3727
		private RoomTempTracker tempTracker;

		// Token: 0x04000E90 RID: 3728
		private int cachedOpenRoofCount = -1;

		// Token: 0x04000E91 RID: 3729
		private int cachedCellCount = -1;

		// Token: 0x04000E92 RID: 3730
		private bool isPrisonCell;

		// Token: 0x04000E93 RID: 3731
		private bool statsAndRoleDirty = true;

		// Token: 0x04000E94 RID: 3732
		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		// Token: 0x04000E95 RID: 3733
		private RoomRoleDef role;

		// Token: 0x04000E96 RID: 3734
		private static int nextRoomID;

		// Token: 0x04000E97 RID: 3735
		private const int RegionCountHuge = 60;

		// Token: 0x04000E98 RID: 3736
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;

		// Token: 0x04000E99 RID: 3737
		private const int MaxRegionsToAssignRoomRole = 36;

		// Token: 0x04000E9A RID: 3738
		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		// Token: 0x04000E9B RID: 3739
		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		// Token: 0x04000E9C RID: 3740
		private List<Region> tmpRegions = new List<Region>();

		// Token: 0x04000E9D RID: 3741
		private HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		// Token: 0x04000E9E RID: 3742
		private List<Thing> uniqueContainedThings = new List<Thing>();

		// Token: 0x04000E9F RID: 3743
		private HashSet<Thing> uniqueContainedThingsOfDef = new HashSet<Thing>();

		// Token: 0x04000EA0 RID: 3744
		private static List<IntVec3> fields = new List<IntVec3>();
	}
}

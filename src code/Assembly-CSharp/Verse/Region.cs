using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200023C RID: 572
	public sealed class Region
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600101A RID: 4122 RVA: 0x0005DE60 File Offset: 0x0005C060
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

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x0005DE7D File Offset: 0x0005C07D
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				RegionGrid regions = this.Map.regionGrid;
				int num;
				for (int z = this.extentsClose.minZ; z <= this.extentsClose.maxZ; z = num + 1)
				{
					for (int x = this.extentsClose.minX; x <= this.extentsClose.maxX; x = num + 1)
					{
						IntVec3 intVec = new IntVec3(x, 0, z);
						if (regions.GetRegionAt_NoRebuild_InvalidAllowed(intVec) == this)
						{
							yield return intVec;
						}
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600101C RID: 4124 RVA: 0x0005DE90 File Offset: 0x0005C090
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					RegionGrid regionGrid = this.Map.regionGrid;
					for (int i = this.extentsClose.minZ; i <= this.extentsClose.maxZ; i++)
					{
						for (int j = this.extentsClose.minX; j <= this.extentsClose.maxX; j++)
						{
							IntVec3 c = new IntVec3(j, 0, i);
							if (regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c) == this)
							{
								this.cachedCellCount++;
							}
						}
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x0005DF21 File Offset: 0x0005C121
		public IEnumerable<Region> Neighbors
		{
			get
			{
				int num;
				for (int li = 0; li < this.links.Count; li = num + 1)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri = num + 1)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].valid)
						{
							yield return link.regions[ri];
						}
						num = ri;
					}
					link = null;
					num = li;
				}
				yield break;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x0005DF31 File Offset: 0x0005C131
		public IEnumerable<Region> NeighborsOfSameType
		{
			get
			{
				int num;
				for (int li = 0; li < this.links.Count; li = num + 1)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri = num + 1)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].type == this.type && link.regions[ri].valid)
						{
							yield return link.regions[ri];
						}
						num = ri;
					}
					link = null;
					num = li;
				}
				yield break;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x0600101F RID: 4127 RVA: 0x0005DF41 File Offset: 0x0005C141
		public Room Room
		{
			get
			{
				District district = this.District;
				if (district == null)
				{
					return null;
				}
				return district.Room;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0005DF54 File Offset: 0x0005C154
		// (set) Token: 0x06001021 RID: 4129 RVA: 0x0005DF5C File Offset: 0x0005C15C
		public District District
		{
			get
			{
				return this.districtInt;
			}
			set
			{
				if (value == this.districtInt)
				{
					return;
				}
				if (this.districtInt != null)
				{
					this.districtInt.RemoveRegion(this);
				}
				this.districtInt = value;
				if (this.districtInt != null)
				{
					this.districtInt.AddRegion(this);
				}
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001022 RID: 4130 RVA: 0x0005DF98 File Offset: 0x0005C198
		public IntVec3 RandomCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				for (int i = 0; i < 1000; i++)
				{
					IntVec3 randomCell = this.extentsClose.RandomCell;
					if (directGrid[cellIndices.CellToIndex(randomCell)] == this)
					{
						return randomCell;
					}
				}
				return this.AnyCell;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x0005DFF0 File Offset: 0x0005C1F0
		public IntVec3 AnyCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				foreach (IntVec3 intVec in this.extentsClose)
				{
					if (directGrid[cellIndices.CellToIndex(intVec)] == this)
					{
						return intVec;
					}
				}
				Log.Error("Couldn't find any cell in region " + this.ToString());
				return this.extentsClose.RandomCell;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x0005E088 File Offset: 0x0005C288
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("id: " + this.id);
				stringBuilder.AppendLine("mapIndex: " + this.mapIndex);
				stringBuilder.AppendLine("links count: " + this.links.Count);
				stringBuilder.AppendLine("type: " + this.type);
				foreach (RegionLink regionLink in this.links)
				{
					stringBuilder.AppendLine("  --" + regionLink.ToString());
				}
				stringBuilder.AppendLine("valid: " + this.valid.ToString());
				stringBuilder.AppendLine("makeTick: " + this.debug_makeTick);
				stringBuilder.AppendLine("districtID: " + ((this.District != null) ? this.District.ID.ToString() : "null district!"));
				stringBuilder.AppendLine("roomID: " + ((this.Room != null) ? this.Room.ID.ToString() : "null room!"));
				stringBuilder.AppendLine("extentsClose: " + this.extentsClose);
				stringBuilder.AppendLine("extentsLimit: " + this.extentsLimit);
				stringBuilder.AppendLine("ListerThings:");
				if (this.listerThings.AllThings != null)
				{
					for (int i = 0; i < this.listerThings.AllThings.Count; i++)
					{
						stringBuilder.AppendLine("  --" + this.listerThings.AllThings[i]);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0005E294 File Offset: 0x0005C494
		public bool DebugIsNew
		{
			get
			{
				return this.debug_makeTick > Find.TickManager.TicksGame - 60;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x0005E2AB File Offset: 0x0005C4AB
		public ListerThings ListerThings
		{
			get
			{
				return this.listerThings;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x0005E2B3 File Offset: 0x0005C4B3
		public bool IsDoorway
		{
			get
			{
				return this.door != null;
			}
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0005E2C0 File Offset: 0x0005C4C0
		private Region()
		{
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0005E350 File Offset: 0x0005C550
		public static Region MakeNewUnfilled(IntVec3 root, Map map)
		{
			Region region = new Region();
			region.debug_makeTick = Find.TickManager.TicksGame;
			region.id = Region.nextId;
			Region.nextId++;
			region.mapIndex = (sbyte)map.Index;
			region.precalculatedHashCode = Gen.HashCombineInt(region.id, 1295813358);
			region.extentsClose.minX = root.x;
			region.extentsClose.maxX = root.x;
			region.extentsClose.minZ = root.z;
			region.extentsClose.maxZ = root.z;
			region.extentsLimit.minX = root.x - root.x % 12;
			region.extentsLimit.maxX = root.x + 12 - (root.x + 12) % 12 - 1;
			region.extentsLimit.minZ = root.z - root.z % 12;
			region.extentsLimit.maxZ = root.z + 12 - (root.z + 12) % 12 - 1;
			region.extentsLimit.ClipInsideMap(map);
			return region;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0005E47C File Offset: 0x0005C67C
		public bool Allows(TraverseParms tp, bool isDestination)
		{
			if (tp.mode != TraverseMode.PassAllDestroyableThings && tp.mode != TraverseMode.PassAllDestroyableThingsNotWater && !this.type.Passable())
			{
				return false;
			}
			if (tp.maxDanger < Danger.Deadly && tp.pawn != null)
			{
				Danger danger = this.DangerFor(tp.pawn);
				if (isDestination || danger == Danger.Deadly)
				{
					Region region = tp.pawn.GetRegion(RegionType.Set_All);
					if ((region == null || danger > region.DangerFor(tp.pawn)) && danger > tp.maxDanger)
					{
						return false;
					}
				}
			}
			bool flag = this.type == RegionType.Fence && tp.fenceBlocked && !tp.canBashFences;
			switch (tp.mode)
			{
			case TraverseMode.ByPawn:
			{
				if (this.door == null)
				{
					return !flag;
				}
				ByteGrid avoidGrid = tp.pawn.GetAvoidGrid(true);
				if (avoidGrid != null && avoidGrid[this.door.Position] == 255)
				{
					return false;
				}
				if (tp.pawn.HostileTo(this.door))
				{
					return this.door.CanPhysicallyPass(tp.pawn) || tp.canBashDoors;
				}
				return this.door.CanPhysicallyPass(tp.pawn) && !this.door.IsForbiddenToPass(tp.pawn);
			}
			case TraverseMode.PassDoors:
				return !flag;
			case TraverseMode.NoPassClosedDoors:
			case TraverseMode.NoPassClosedDoorsOrWater:
				return (this.door == null || this.door.FreePassage) && !flag;
			case TraverseMode.PassAllDestroyableThings:
				return true;
			case TraverseMode.PassAllDestroyableThingsNotWater:
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0005E608 File Offset: 0x0005C808
		public Danger DangerFor(Pawn p)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.cachedDangersForFrame != RealTime.frameCount)
				{
					this.cachedDangers.Clear();
					this.cachedDangersForFrame = RealTime.frameCount;
				}
				else
				{
					for (int i = 0; i < this.cachedDangers.Count; i++)
					{
						if (this.cachedDangers[i].Key == p)
						{
							return this.cachedDangers[i].Value;
						}
					}
				}
			}
			float temperature = this.Room.Temperature;
			FloatRange value;
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Region.cachedSafeTemperatureRangesForFrame != RealTime.frameCount)
				{
					Region.cachedSafeTemperatureRanges.Clear();
					Region.cachedSafeTemperatureRangesForFrame = RealTime.frameCount;
				}
				if (!Region.cachedSafeTemperatureRanges.TryGetValue(p, out value))
				{
					value = p.SafeTemperatureRange();
					Region.cachedSafeTemperatureRanges.Add(p, value);
				}
			}
			else
			{
				value = p.SafeTemperatureRange();
			}
			Danger danger;
			if (value.Includes(temperature))
			{
				danger = Danger.None;
			}
			else if (value.ExpandedBy(80f).Includes(temperature))
			{
				danger = Danger.Some;
			}
			else
			{
				danger = Danger.Deadly;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.cachedDangers.Add(new KeyValuePair<Pawn, Danger>(p, danger));
			}
			return danger;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0005E72C File Offset: 0x0005C92C
		public float GetBaseDesiredPlantsCount(bool allowCache = true)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (allowCache && ticksGame - this.cachedBaseDesiredPlantsCountForTick < 2500)
			{
				return this.cachedBaseDesiredPlantsCount;
			}
			this.cachedBaseDesiredPlantsCount = 0f;
			Map map = this.Map;
			foreach (IntVec3 c in this.Cells)
			{
				this.cachedBaseDesiredPlantsCount += map.wildPlantSpawner.GetBaseDesiredPlantsCountAt(c);
			}
			this.cachedBaseDesiredPlantsCountForTick = ticksGame;
			return this.cachedBaseDesiredPlantsCount;
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0005E7D0 File Offset: 0x0005C9D0
		public AreaOverlap OverlapWith(Area a)
		{
			if (a.TrueCount == 0)
			{
				return AreaOverlap.None;
			}
			if (this.Map != a.Map)
			{
				return AreaOverlap.None;
			}
			if (this.cachedAreaOverlaps == null)
			{
				this.cachedAreaOverlaps = new Dictionary<Area, AreaOverlap>();
			}
			AreaOverlap areaOverlap;
			if (!this.cachedAreaOverlaps.TryGetValue(a, out areaOverlap))
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.Cells)
				{
					num2++;
					if (a[c])
					{
						num++;
					}
				}
				if (num == 0)
				{
					areaOverlap = AreaOverlap.None;
				}
				else if (num == num2)
				{
					areaOverlap = AreaOverlap.Entire;
				}
				else
				{
					areaOverlap = AreaOverlap.Partial;
				}
				this.cachedAreaOverlaps.Add(a, areaOverlap);
			}
			return areaOverlap;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0005E88C File Offset: 0x0005CA8C
		public void Notify_AreaChanged(Area a)
		{
			if (this.cachedAreaOverlaps == null)
			{
				return;
			}
			if (this.cachedAreaOverlaps.ContainsKey(a))
			{
				this.cachedAreaOverlaps.Remove(a);
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0005E8B4 File Offset: 0x0005CAB4
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for region ",
					this.id,
					", but mapIndex=",
					this.mapIndex
				}));
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0005E916 File Offset: 0x0005CB16
		public void Notify_MyMapRemoved()
		{
			this.listerThings.Clear();
			this.mapIndex = -1;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0005E92A File Offset: 0x0005CB2A
		public static void ClearStaticData()
		{
			Region.cachedSafeTemperatureRanges.Clear();
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0005E938 File Offset: 0x0005CB38
		public override string ToString()
		{
			string str;
			if (this.door != null)
			{
				str = this.door.ToString();
			}
			else
			{
				str = "null";
			}
			return string.Concat(new object[]
			{
				"Region(id=",
				this.id,
				", mapIndex=",
				this.mapIndex,
				", center=",
				this.extentsClose.CenterCell,
				", links=",
				this.links.Count,
				", cells=",
				this.CellCount,
				(this.door != null) ? (", portal=" + str) : null,
				")"
			});
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0005EA0C File Offset: 0x0005CC0C
		public void DebugDraw()
		{
			if (DebugViewSettings.drawRegionTraversal && Find.TickManager.TicksGame < this.debug_lastTraverseTick + 60)
			{
				float a = 1f - (float)(Find.TickManager.TicksGame - this.debug_lastTraverseTick) / 60f;
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), new Color(0f, 0f, 1f, a), null);
			}
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0005EA84 File Offset: 0x0005CC84
		public void DebugDrawMouseover()
		{
			int num = Mathf.RoundToInt(Time.realtimeSinceStartup * 2f) % 2;
			if (DebugViewSettings.drawRegions)
			{
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), this.DebugColor(), null);
				foreach (Region region in this.Neighbors)
				{
					GenDraw.DrawFieldEdges(region.Cells.ToList<IntVec3>(), Color.grey, null);
				}
			}
			if (DebugViewSettings.drawRegionLinks)
			{
				foreach (RegionLink regionLink in this.links)
				{
					if (num == 1)
					{
						List<IntVec3> list = regionLink.span.Cells.ToList<IntVec3>();
						Material mat = DebugSolidColorMats.MaterialOf(Color.magenta * new Color(1f, 1f, 1f, 0.25f));
						foreach (IntVec3 c in list)
						{
							CellRenderer.RenderCell(c, mat);
						}
						GenDraw.DrawFieldEdges(list, Color.white, null);
					}
				}
			}
			if (DebugViewSettings.drawRegionThings)
			{
				foreach (Thing thing in this.listerThings.AllThings)
				{
					CellRenderer.RenderSpot(thing.TrueCenter(), (float)(thing.thingIDNumber % 256) / 256f, 0.15f);
				}
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0005EC70 File Offset: 0x0005CE70
		private Color DebugColor()
		{
			Color result;
			if (!this.valid)
			{
				result = Color.red;
			}
			else if (this.DebugIsNew)
			{
				result = Color.yellow;
			}
			else
			{
				result = Color.green;
			}
			return result;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0005ECA4 File Offset: 0x0005CEA4
		public void Debug_Notify_Traversed()
		{
			this.debug_lastTraverseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0005ECB6 File Offset: 0x0005CEB6
		public override int GetHashCode()
		{
			return this.precalculatedHashCode;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0005ECC0 File Offset: 0x0005CEC0
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Region region = obj as Region;
			return region != null && region.id == this.id;
		}

		// Token: 0x04000E3A RID: 3642
		public RegionType type = RegionType.Normal;

		// Token: 0x04000E3B RID: 3643
		public int id = -1;

		// Token: 0x04000E3C RID: 3644
		public sbyte mapIndex = -1;

		// Token: 0x04000E3D RID: 3645
		private District districtInt;

		// Token: 0x04000E3E RID: 3646
		public List<RegionLink> links = new List<RegionLink>();

		// Token: 0x04000E3F RID: 3647
		public CellRect extentsClose;

		// Token: 0x04000E40 RID: 3648
		public CellRect extentsLimit;

		// Token: 0x04000E41 RID: 3649
		public Building_Door door;

		// Token: 0x04000E42 RID: 3650
		private int precalculatedHashCode;

		// Token: 0x04000E43 RID: 3651
		public bool touchesMapEdge;

		// Token: 0x04000E44 RID: 3652
		private int cachedCellCount = -1;

		// Token: 0x04000E45 RID: 3653
		public bool valid = true;

		// Token: 0x04000E46 RID: 3654
		private ListerThings listerThings = new ListerThings(ListerThingsUse.Region);

		// Token: 0x04000E47 RID: 3655
		public uint[] closedIndex = new uint[RegionTraverser.NumWorkers];

		// Token: 0x04000E48 RID: 3656
		public uint reachedIndex;

		// Token: 0x04000E49 RID: 3657
		public int newRegionGroupIndex = -1;

		// Token: 0x04000E4A RID: 3658
		private Dictionary<Area, AreaOverlap> cachedAreaOverlaps;

		// Token: 0x04000E4B RID: 3659
		public int mark;

		// Token: 0x04000E4C RID: 3660
		private List<KeyValuePair<Pawn, Danger>> cachedDangers = new List<KeyValuePair<Pawn, Danger>>();

		// Token: 0x04000E4D RID: 3661
		private int cachedDangersForFrame;

		// Token: 0x04000E4E RID: 3662
		private float cachedBaseDesiredPlantsCount;

		// Token: 0x04000E4F RID: 3663
		private int cachedBaseDesiredPlantsCountForTick = -999999;

		// Token: 0x04000E50 RID: 3664
		private static Dictionary<Pawn, FloatRange> cachedSafeTemperatureRanges = new Dictionary<Pawn, FloatRange>();

		// Token: 0x04000E51 RID: 3665
		private static int cachedSafeTemperatureRangesForFrame;

		// Token: 0x04000E52 RID: 3666
		private int debug_makeTick = -1000;

		// Token: 0x04000E53 RID: 3667
		private int debug_lastTraverseTick = -1000;

		// Token: 0x04000E54 RID: 3668
		private static int nextId = 1;

		// Token: 0x04000E55 RID: 3669
		public const int GridSize = 12;
	}
}

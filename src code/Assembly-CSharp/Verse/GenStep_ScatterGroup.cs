using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000221 RID: 545
	public class GenStep_ScatterGroup : GenStep_Scatterer
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000F78 RID: 3960 RVA: 0x00059784 File Offset: 0x00057984
		public override int SeedPart
		{
			get
			{
				return 1237834582;
			}
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0005978B File Offset: 0x0005798B
		private int GetSeed(IntVec3 loc, Map map)
		{
			return loc.GetHashCode() * map.ConstantRandSeed * Find.World.info.Seed;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x000597B4 File Offset: 0x000579B4
		public override void Generate(Map map, GenStepParams parms)
		{
			try
			{
				base.Generate(map, parms);
			}
			finally
			{
				this.dontChooseIndoor = false;
			}
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000597E4 File Offset: 0x000579E4
		private bool IndoorRuinSpot(CellRect rect, Map map)
		{
			float sqrMagnitude = ((new Vector2((float)rect.minX, (float)rect.minZ) - new Vector2((float)rect.maxX, (float)rect.maxZ)) * 2f).sqrMagnitude;
			if (rect.CenterCell.GetTerrain(map).BuildableByPlayer)
			{
				using (List<Thing>.Enumerator enumerator = map.listerThings.ThingsOfDef(ThingDefOf.Wall).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if ((float)enumerator.Current.Position.DistanceToSquared(rect.CenterCell) < sqrMagnitude)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x000598A8 File Offset: 0x00057AA8
		private GenStep_ScatterGroup.ScatterGroup GetGroup(Map map)
		{
			GenStep_ScatterGroup.tmpScatterGroups.Clear();
			foreach (GenStep_ScatterGroup.ScatterGroup scatterGroup in this.groups)
			{
				if (!this.dontChooseIndoor || !scatterGroup.indoorRuin)
				{
					GenStep_ScatterGroup.tmpScatterGroups.Add(scatterGroup);
				}
			}
			Rand.PushState(Gen.HashCombineInt(Find.World.info.Seed, this.usedSpots.Count));
			GenStep_ScatterGroup.ScatterGroup result;
			try
			{
				result = GenStep_ScatterGroup.tmpScatterGroups.RandomElement<GenStep_ScatterGroup.ScatterGroup>();
			}
			finally
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00059960 File Offset: 0x00057B60
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			bool warnOnFail = this.warnOnFail;
			this.warnOnFail = false;
			try
			{
				this.dontChooseIndoor = false;
				if (this.GetGroup(map).indoorRuin)
				{
					if (base.TryFindScatterCell(map, out result))
					{
						return true;
					}
					this.dontChooseIndoor = true;
				}
				if (this.GetGroup(map) != null && base.TryFindScatterCell(map, out result))
				{
					return true;
				}
			}
			finally
			{
				this.warnOnFail = warnOnFail;
			}
			if (this.warnOnFail)
			{
				Log.Warning("Scatterer " + this.ToString() + " could not find cell to generate at.");
			}
			result = default(IntVec3);
			return false;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00059A04 File Offset: 0x00057C04
		private bool CalculateScatterInformation(IntVec3 loc, Map map, out CellRect rect, List<GenStep_ScatterGroup.ThingSpawn> outThingSpawns)
		{
			GenStep_ScatterGroup.<>c__DisplayClass18_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.outThingSpawns = outThingSpawns;
			GenStep_ScatterGroup.tmpWeightedThings.Clear();
			GenStep_ScatterGroup.ScatterGroup group = this.GetGroup(CS$<>8__locals1.map);
			rect = CellRect.CenteredOn(loc, group.clusterRectRadius.RandomInRange);
			CS$<>8__locals1.localRect = rect;
			if (!rect.InBounds(CS$<>8__locals1.map))
			{
				return false;
			}
			if (group.indoorRuin != this.IndoorRuinSpot(rect, CS$<>8__locals1.map))
			{
				return false;
			}
			GenStep_ScatterGroup.tmpWeightedThings.AddRange(group.things);
			if (group.spawnAtCenter != null)
			{
				if (!GenStep_ScatterGroup.<CalculateScatterInformation>g__CanSpawn|18_0(group.spawnAtCenter, rect.CenterCell, Rot4.North, GenAdj.OccupiedRect(rect.CenterCell, Rot4.North, group.spawnAtCenter.size), ref CS$<>8__locals1))
				{
					return false;
				}
				CS$<>8__locals1.outThingSpawns.Add(new GenStep_ScatterGroup.ThingSpawn
				{
					def = group.spawnAtCenter,
					occupiedRect = GenAdj.OccupiedRect(rect.CenterCell, Rot4.North, group.spawnAtCenter.size),
					pos = rect.CenterCell,
					rotation = Rot4.North,
					sourceGroup = group
				});
				if (group.spawnAtCenterFilthDef != null)
				{
					CS$<>8__locals1.outThingSpawns.Add(new GenStep_ScatterGroup.ThingSpawn
					{
						def = group.spawnAtCenterFilthDef,
						occupiedRect = GenAdj.OccupiedRect(rect.CenterCell, Rot4.North, group.spawnAtCenterFilthDef.size),
						pos = rect.CenterCell,
						rotation = Rot4.North,
						sourceGroup = group
					});
				}
			}
			int num = 0;
			foreach (IntVec3 intVec in rect)
			{
				bool flag = false;
				foreach (GenStep_ScatterGroup.ThingWeight thingWeight in GenStep_ScatterGroup.tmpWeightedThings)
				{
					GenStep_ScatterGroup.<CalculateScatterInformation>g__WritePossibleRotations|18_1(thingWeight.thing);
					foreach (Rot4 rot in GenStep_ScatterGroup.tmpPossibleRotations)
					{
						if (GenStep_ScatterGroup.<CalculateScatterInformation>g__CanSpawn|18_0(thingWeight.thing, intVec, rot, GenAdj.OccupiedRect(intVec, rot, thingWeight.thing.size), ref CS$<>8__locals1))
						{
							num++;
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			FloatRange coveredCellsPer10Cells = group.coveredCellsPer10Cells;
			int num2 = (int)((float)num / 10f * coveredCellsPer10Cells.TrueMin);
			float num3 = (float)num / 10f;
			float trueMax = coveredCellsPer10Cells.TrueMax;
			int num4 = (int)((float)num / 10f * coveredCellsPer10Cells.RandomInRange);
			int i = 0;
			while (i < num4)
			{
				GenStep_ScatterGroup.tmpWeightedThingsRandom.Clear();
				foreach (GenStep_ScatterGroup.ThingWeight thingWeight2 in GenStep_ScatterGroup.tmpWeightedThings)
				{
					GenStep_ScatterGroup.tmpWeightedThingsRandom.Add(thingWeight2, thingWeight2.weight * Rand.Value);
				}
				GenStep_ScatterGroup.tmpWeightedThings.SortByDescending((GenStep_ScatterGroup.ThingWeight e) => GenStep_ScatterGroup.tmpWeightedThingsRandom[e]);
				bool flag2 = false;
				foreach (GenStep_ScatterGroup.ThingWeight thingWeight3 in GenStep_ScatterGroup.tmpWeightedThings)
				{
					GenStep_ScatterGroup.<CalculateScatterInformation>g__WritePossibleRotations|18_1(thingWeight3.thing);
					foreach (IntVec3 intVec2 in rect.Cells.InRandomOrder(GenStep_ScatterGroup.tmpCellsRandomOrderWorkingList))
					{
						foreach (Rot4 rot2 in GenStep_ScatterGroup.tmpPossibleRotations)
						{
							CellRect occupiedRect = GenAdj.OccupiedRect(intVec2, rot2, thingWeight3.thing.size);
							if (GenStep_ScatterGroup.<CalculateScatterInformation>g__CanSpawn|18_0(thingWeight3.thing, intVec2, rot2, occupiedRect, ref CS$<>8__locals1))
							{
								CS$<>8__locals1.outThingSpawns.Add(new GenStep_ScatterGroup.ThingSpawn
								{
									def = thingWeight3.thing,
									occupiedRect = occupiedRect,
									pos = intVec2,
									rotation = rot2,
									sourceGroup = group
								});
								i += occupiedRect.Area;
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
					if (flag2)
					{
						break;
					}
				}
				if (!flag2)
				{
					break;
				}
			}
			return i > num2;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00059F5C File Offset: 0x0005815C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Rand.PushState(this.GetSeed(loc, map));
			try
			{
				CellRect cellRect;
				if (!this.CalculateScatterInformation(loc, map, out cellRect, GenStep_ScatterGroup.tmpSpawns))
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried scattering group at ",
						loc,
						" on map ",
						map,
						" which is an invalid location!"
					}));
				}
				else
				{
					foreach (GenStep_ScatterGroup.ThingSpawn thingSpawn in GenStep_ScatterGroup.tmpSpawns)
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(thingSpawn.def, null), thingSpawn.pos, map, thingSpawn.rotation, WipeMode.Vanish, false);
						if (thingSpawn.sourceGroup.filthDef != null && !thingSpawn.def.IsFilth)
						{
							CellRect occupiedRect = thingSpawn.occupiedRect;
							foreach (IntVec3 c in occupiedRect.ExpandedBy(thingSpawn.sourceGroup.filthExpandBy))
							{
								if (Rand.Chance(thingSpawn.sourceGroup.filthChance) && c.InBounds(map))
								{
									FilthMaker.TryMakeFilth(c, map, thingSpawn.sourceGroup.filthDef, 1, FilthSourceFlags.None, true);
								}
							}
						}
					}
				}
			}
			finally
			{
				Rand.PopState();
				GenStep_ScatterGroup.tmpSpawns.Clear();
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0005A10C File Offset: 0x0005830C
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			Rand.PushState(this.GetSeed(loc, map));
			bool result;
			try
			{
				CellRect cellRect;
				result = this.CalculateScatterInformation(loc, map, out cellRect, GenStep_ScatterGroup.tmpSpawns);
			}
			finally
			{
				Rand.PopState();
				GenStep_ScatterGroup.tmpSpawns.Clear();
			}
			return result;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0005A1B0 File Offset: 0x000583B0
		[CompilerGenerated]
		internal static bool <CalculateScatterInformation>g__CanSpawn|18_0(ThingDef def, IntVec3 cell, Rot4 rot, CellRect occupiedRect, ref GenStep_ScatterGroup.<>c__DisplayClass18_0 A_4)
		{
			if (!occupiedRect.InBounds(A_4.map) || !occupiedRect.FullyContainedWithin(A_4.localRect))
			{
				return false;
			}
			foreach (GenStep_ScatterGroup.ThingSpawn thingSpawn in A_4.outThingSpawns)
			{
				CellRect occupiedRect2 = thingSpawn.occupiedRect;
				if (occupiedRect2.Overlaps(occupiedRect))
				{
					return false;
				}
			}
			if (GenSpawn.WouldWipeAnythingWith(cell, rot, def, A_4.map, (Thing x) => x.def == def || (x.def.category != ThingCategory.Plant && x.def.category != ThingCategory.Filth)))
			{
				return false;
			}
			foreach (IntVec3 c in occupiedRect)
			{
				foreach (Thing thing in c.GetThingList(A_4.map))
				{
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn || !thing.def.destroyable)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0005A318 File Offset: 0x00058518
		[CompilerGenerated]
		internal static void <CalculateScatterInformation>g__WritePossibleRotations|18_1(ThingDef thing)
		{
			GenStep_ScatterGroup.tmpPossibleRotations.Clear();
			if (thing.rotatable)
			{
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.North);
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.East);
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.South);
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.West);
			}
			else
			{
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.North);
			}
			GenStep_ScatterGroup.tmpPossibleRotations.Shuffle<Rot4>();
		}

		// Token: 0x04000DC4 RID: 3524
		public List<GenStep_ScatterGroup.ScatterGroup> groups;

		// Token: 0x04000DC5 RID: 3525
		[Unsaved(false)]
		private bool dontChooseIndoor;

		// Token: 0x04000DC6 RID: 3526
		private static List<GenStep_ScatterGroup.ThingSpawn> tmpSpawns = new List<GenStep_ScatterGroup.ThingSpawn>();

		// Token: 0x04000DC7 RID: 3527
		private static List<GenStep_ScatterGroup.ScatterGroup> tmpScatterGroups = new List<GenStep_ScatterGroup.ScatterGroup>();

		// Token: 0x04000DC8 RID: 3528
		private static List<GenStep_ScatterGroup.ThingWeight> tmpWeightedThings = new List<GenStep_ScatterGroup.ThingWeight>();

		// Token: 0x04000DC9 RID: 3529
		private static Dictionary<GenStep_ScatterGroup.ThingWeight, float> tmpWeightedThingsRandom = new Dictionary<GenStep_ScatterGroup.ThingWeight, float>();

		// Token: 0x04000DCA RID: 3530
		private static List<IntVec3> tmpCellsRandomOrderWorkingList = new List<IntVec3>();

		// Token: 0x04000DCB RID: 3531
		private static List<Rot4> tmpPossibleRotations = new List<Rot4>();

		// Token: 0x02001D7C RID: 7548
		private struct ThingSpawn
		{
			// Token: 0x04007469 RID: 29801
			public IntVec3 pos;

			// Token: 0x0400746A RID: 29802
			public Rot4 rotation;

			// Token: 0x0400746B RID: 29803
			public CellRect occupiedRect;

			// Token: 0x0400746C RID: 29804
			public ThingDef def;

			// Token: 0x0400746D RID: 29805
			public GenStep_ScatterGroup.ScatterGroup sourceGroup;
		}

		// Token: 0x02001D7D RID: 7549
		public class ThingWeight
		{
			// Token: 0x0600B4AA RID: 46250 RVA: 0x00411812 File Offset: 0x0040FA12
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thing", xmlRoot.Name, null, null, null);
				this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
			}

			// Token: 0x0400746E RID: 29806
			public ThingDef thing;

			// Token: 0x0400746F RID: 29807
			public float weight;
		}

		// Token: 0x02001D7E RID: 7550
		public class ScatterGroup
		{
			// Token: 0x04007470 RID: 29808
			public List<GenStep_ScatterGroup.ThingWeight> things;

			// Token: 0x04007471 RID: 29809
			public ThingDef spawnAtCenter;

			// Token: 0x04007472 RID: 29810
			public ThingDef spawnAtCenterFilthDef;

			// Token: 0x04007473 RID: 29811
			public bool indoorRuin;

			// Token: 0x04007474 RID: 29812
			public FloatRange coveredCellsPer10Cells;

			// Token: 0x04007475 RID: 29813
			public ThingDef filthDef;

			// Token: 0x04007476 RID: 29814
			public int filthExpandBy;

			// Token: 0x04007477 RID: 29815
			public float filthChance = 0.5f;

			// Token: 0x04007478 RID: 29816
			public IntRange clusterRectRadius;
		}
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000222 RID: 546
	public class GenStep_ScatterThings : GenStep_Scatterer
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x0005A38E File Offset: 0x0005858E
		public override int SeedPart
		{
			get
			{
				return 1158116095;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0005A398 File Offset: 0x00058598
		private List<Rot4> PossibleRotations
		{
			get
			{
				if (this.possibleRotationsInt == null)
				{
					this.possibleRotationsInt = new List<Rot4>();
					if (this.thingDef.rotatable)
					{
						this.possibleRotationsInt.Add(Rot4.North);
						this.possibleRotationsInt.Add(Rot4.East);
						this.possibleRotationsInt.Add(Rot4.South);
						this.possibleRotationsInt.Add(Rot4.West);
					}
					else
					{
						this.possibleRotationsInt.Add(Rot4.North);
					}
				}
				return this.possibleRotationsInt;
			}
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0005A420 File Offset: 0x00058620
		public override void Generate(Map map, GenStepParams parms)
		{
			if (this.ShouldSkipMap(map))
			{
				return;
			}
			int count = base.CalculateFinalCount(map);
			IntRange one;
			if (this.thingDef.ingestible != null && this.thingDef.ingestible.IsMeal && this.thingDef.stackLimit <= 10)
			{
				one = IntRange.one;
			}
			else if (this.thingDef.stackLimit > 5)
			{
				one = new IntRange(Mathf.RoundToInt((float)this.thingDef.stackLimit * 0.5f), this.thingDef.stackLimit);
			}
			else
			{
				one = new IntRange(this.thingDef.stackLimit, this.thingDef.stackLimit);
			}
			List<int> list = GenStep_ScatterThings.CountDividedIntoStacks(count, one);
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, list[i]);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
			this.clusterCenter = IntVec3.Invalid;
			this.leftInCluster = 0;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0005A528 File Offset: 0x00058728
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.clusterSize > 1)
			{
				if (this.leftInCluster <= 0)
				{
					if (!base.TryFindScatterCell(map, out this.clusterCenter))
					{
						Log.Error("Could not find cluster center to scatter " + this.thingDef);
					}
					this.leftInCluster = this.clusterSize;
				}
				this.leftInCluster--;
				result = CellFinder.RandomClosewalkCellNear(this.clusterCenter, map, 4, delegate(IntVec3 x)
				{
					Rot4 rot;
					return this.TryGetRandomValidRotation(x, map, out rot);
				});
				return result.IsValid;
			}
			return base.TryFindScatterCell(map, out result);
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0005A5D8 File Offset: 0x000587D8
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			Rot4 rot;
			if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				Log.Warning("Could not find any valid rotation for " + this.thingDef);
				return;
			}
			if (this.clearSpaceSize > 0)
			{
				foreach (IntVec3 c in GridShapeMaker.IrregularLump(loc, map, this.clearSpaceSize))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
			}
			Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
			if (this.thingDef.Minifiable)
			{
				thing = thing.MakeMinified();
			}
			if (thing.def.category == ThingCategory.Item)
			{
				thing.stackCount = stackCount;
				thing.SetForbidden(true, false);
				Thing thing2;
				GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Near, out thing2, null, null, default(Rot4));
				if (this.nearPlayerStart && thing2 != null && thing2.def.category == ThingCategory.Item && TutorSystem.TutorialMode)
				{
					Find.TutorialState.AddStartingItem(thing2);
				}
			}
			else
			{
				GenSpawn.Spawn(thing, loc, map, rot, WipeMode.Vanish, false);
			}
			if (this.filthDef != null)
			{
				foreach (IntVec3 c2 in thing.OccupiedRect().ExpandedBy(this.filthExpandBy))
				{
					if (Rand.Chance(this.filthChance) && c2.InBounds(thing.Map))
					{
						FilthMaker.TryMakeFilth(c2, thing.Map, this.filthDef, 1, FilthSourceFlags.None, true);
					}
				}
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0005A784 File Offset: 0x00058984
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			Rot4 rot;
			if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				return false;
			}
			if (this.terrainValidationRadius > 0f)
			{
				foreach (IntVec3 c in GenRadial.RadialCellsAround(loc, this.terrainValidationRadius, true))
				{
					if (c.InBounds(map))
					{
						TerrainDef terrain = c.GetTerrain(map);
						for (int i = 0; i < this.terrainValidationDisallowed.Count; i++)
						{
							if (terrain.HasTag(this.terrainValidationDisallowed[i]))
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0005A844 File Offset: 0x00058A44
		private bool TryGetRandomValidRotation(IntVec3 loc, Map map, out Rot4 rot)
		{
			List<Rot4> possibleRotations = this.PossibleRotations;
			for (int i = 0; i < possibleRotations.Count; i++)
			{
				if (this.IsRotationValid(loc, possibleRotations[i], map))
				{
					GenStep_ScatterThings.tmpRotations.Add(possibleRotations[i]);
				}
			}
			if (GenStep_ScatterThings.tmpRotations.TryRandomElement(out rot))
			{
				GenStep_ScatterThings.tmpRotations.Clear();
				return true;
			}
			rot = Rot4.Invalid;
			return false;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0005A8B0 File Offset: 0x00058AB0
		private bool IsRotationValid(IntVec3 loc, Rot4 rot, Map map)
		{
			return GenAdj.OccupiedRect(loc, rot, this.thingDef.size).InBounds(map) && !GenSpawn.WouldWipeAnythingWith(loc, rot, this.thingDef, map, (Thing x) => x.def == this.thingDef || (x.def.category != ThingCategory.Plant && x.def.category != ThingCategory.Filth));
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0005A8FC File Offset: 0x00058AFC
		public static List<int> CountDividedIntoStacks(int count, IntRange stackSizeRange)
		{
			List<int> list = new List<int>();
			while (count > 0)
			{
				int num = Mathf.Min(count, stackSizeRange.RandomInRange);
				count -= num;
				list.Add(num);
			}
			if (stackSizeRange.max > 2)
			{
				for (int i = 0; i < list.Count * 4; i++)
				{
					int num2 = Rand.RangeInclusive(0, list.Count - 1);
					int num3 = Rand.RangeInclusive(0, list.Count - 1);
					if (num2 != num3 && list[num2] > list[num3])
					{
						int num4 = (int)((float)(list[num2] - list[num3]) * Rand.Value);
						List<int> list2 = list;
						int index = num2;
						list2[index] -= num4;
						list2 = list;
						index = num3;
						list2[index] += num4;
					}
				}
			}
			return list;
		}

		// Token: 0x04000DCC RID: 3532
		public ThingDef thingDef;

		// Token: 0x04000DCD RID: 3533
		public ThingDef stuff;

		// Token: 0x04000DCE RID: 3534
		public int clearSpaceSize;

		// Token: 0x04000DCF RID: 3535
		public int clusterSize = 1;

		// Token: 0x04000DD0 RID: 3536
		public ThingDef filthDef;

		// Token: 0x04000DD1 RID: 3537
		public int filthExpandBy;

		// Token: 0x04000DD2 RID: 3538
		public float filthChance = 0.5f;

		// Token: 0x04000DD3 RID: 3539
		public float terrainValidationRadius;

		// Token: 0x04000DD4 RID: 3540
		[NoTranslate]
		private List<string> terrainValidationDisallowed;

		// Token: 0x04000DD5 RID: 3541
		[Unsaved(false)]
		private IntVec3 clusterCenter;

		// Token: 0x04000DD6 RID: 3542
		[Unsaved(false)]
		private int leftInCluster;

		// Token: 0x04000DD7 RID: 3543
		private const int ClusterRadius = 4;

		// Token: 0x04000DD8 RID: 3544
		private List<Rot4> possibleRotationsInt;

		// Token: 0x04000DD9 RID: 3545
		private static List<Rot4> tmpRotations = new List<Rot4>();
	}
}

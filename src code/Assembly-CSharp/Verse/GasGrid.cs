using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E6 RID: 486
	public class GasGrid : IExposable
	{
		// Token: 0x06000D96 RID: 3478 RVA: 0x0004B6C4 File Offset: 0x000498C4
		public GasGrid(Map map)
		{
			this.map = map;
			this.gasDensity = new int[map.cellIndices.NumGridCells];
			this.cardinalDirections = new List<IntVec3>();
			this.cardinalDirections.AddRange(GenAdj.CardinalDirections);
			this.cycleIndexDiffusion = Rand.Range(0, map.Area / 2);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0004B724 File Offset: 0x00049924
		public void RecalculateEverHadGas()
		{
			this.anyGasEverAdded = false;
			for (int i = 0; i < this.gasDensity.Length; i++)
			{
				if (this.gasDensity[i] > 0)
				{
					this.anyGasEverAdded = true;
					return;
				}
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0004B760 File Offset: 0x00049960
		public void Tick()
		{
			if (!this.anyGasEverAdded)
			{
				return;
			}
			int area = this.map.Area;
			int num = Mathf.CeilToInt((float)area * 0.015625f);
			this.cellsInRandomOrder = this.map.cellsInRandomOrder.GetAll();
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndexDissipation >= area)
				{
					this.cycleIndexDissipation = 0;
				}
				this.TryDissipateGasses(CellIndicesUtility.CellToIndex(this.cellsInRandomOrder[this.cycleIndexDissipation], this.map.Size.x));
				this.cycleIndexDissipation++;
			}
			num = Mathf.CeilToInt((float)area * 0.03125f);
			for (int j = 0; j < num; j++)
			{
				if (this.cycleIndexDiffusion >= area)
				{
					this.cycleIndexDiffusion = 0;
				}
				this.TryDiffuseGasses(this.cellsInRandomOrder[this.cycleIndexDiffusion]);
				this.cycleIndexDiffusion++;
			}
			if (this.map.IsHashIntervalTick(600))
			{
				this.RecalculateEverHadGas();
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0004B863 File Offset: 0x00049A63
		public bool AnyGasAt(IntVec3 cell)
		{
			return this.AnyGasAt(CellIndicesUtility.CellToIndex(cell, this.map.Size.x));
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0004B881 File Offset: 0x00049A81
		private bool AnyGasAt(int idx)
		{
			return this.gasDensity[idx] > 0;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0004B88E File Offset: 0x00049A8E
		public byte DensityAt(IntVec3 cell, GasType gasType)
		{
			return this.DensityAt(CellIndicesUtility.CellToIndex(cell, this.map.Size.x), gasType);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0004B8AD File Offset: 0x00049AAD
		private byte DensityAt(int index, GasType gasType)
		{
			return (byte)(this.gasDensity[index] >> (int)gasType & 255);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0004B8C3 File Offset: 0x00049AC3
		public float DensityPercentAt(IntVec3 cell, GasType gasType)
		{
			return (float)this.DensityAt(cell, gasType) / 255f;
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0004B8D4 File Offset: 0x00049AD4
		public void AddGas(IntVec3 cell, GasType gasType, int amount, bool canOverflow = true)
		{
			if (amount <= 0 || !this.GasCanMoveTo(cell))
			{
				return;
			}
			this.anyGasEverAdded = true;
			int index = CellIndicesUtility.CellToIndex(cell, this.map.Size.x);
			byte b = this.DensityAt(index, GasType.BlindSmoke);
			byte b2 = this.DensityAt(index, GasType.ToxGas);
			byte b3 = this.DensityAt(index, GasType.RotStink);
			int num = 0;
			if (gasType != GasType.BlindSmoke)
			{
				if (gasType != GasType.ToxGas)
				{
					if (gasType != GasType.RotStink)
					{
						Log.Error("Trying to add unknown gas type.");
						return;
					}
					b3 = this.AdjustedDensity((int)b3 + amount, out num);
				}
				else
				{
					if (!ModLister.CheckBiotech("Tox gas"))
					{
						return;
					}
					b2 = this.AdjustedDensity((int)b2 + amount, out num);
				}
			}
			else
			{
				b = this.AdjustedDensity((int)b + amount, out num);
			}
			this.SetDirect(index, b, b2, b3);
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlag.Gas);
			if (canOverflow && num > 0)
			{
				this.Overflow(cell, gasType, num);
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0004B9AF File Offset: 0x00049BAF
		private byte AdjustedDensity(int newDensity, out int overflow)
		{
			if (newDensity > 255)
			{
				overflow = newDensity - 255;
				return byte.MaxValue;
			}
			overflow = 0;
			if (newDensity < 0)
			{
				return 0;
			}
			return (byte)newDensity;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0004B9D4 File Offset: 0x00049BD4
		public Color ColorAt(IntVec3 cell)
		{
			int index = CellIndicesUtility.CellToIndex(cell, this.map.Size.x);
			float num = (float)this.DensityAt(index, GasType.BlindSmoke);
			float num2 = (float)this.DensityAt(index, GasType.ToxGas);
			float num3 = (float)this.DensityAt(index, GasType.RotStink);
			float num4 = num + num2 + num3;
			Color result = GasGrid.SmokeColor * (num / num4) + (GasGrid.ToxColor * (num2 / num4) + GasGrid.RotColor * (num3 / num4));
			result.a = GasGrid.AlphaRange.LerpThroughRange(num4 / 765f);
			return result;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0004BA74 File Offset: 0x00049C74
		public void Notify_ThingSpawned(Thing thing)
		{
			if (thing.Spawned && thing.def.Fillage == FillCategory.Full)
			{
				foreach (IntVec3 intVec in thing.OccupiedRect())
				{
					if (this.AnyGasAt(intVec))
					{
						this.gasDensity[CellIndicesUtility.CellToIndex(intVec, this.map.Size.x)] = 0;
						this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlag.Gas);
					}
				}
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0004BB18 File Offset: 0x00049D18
		private void SetDirect(int index, byte smoke, byte toxic, byte rotStink)
		{
			if (!ModsConfig.BiotechActive)
			{
				toxic = 0;
			}
			this.gasDensity[index] = ((int)rotStink << 16 | (int)toxic << 8 | (int)smoke);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0004BB38 File Offset: 0x00049D38
		private void Overflow(IntVec3 cell, GasType gasType, int amount)
		{
			if (amount <= 0)
			{
				return;
			}
			int remainingAmount = amount;
			this.map.floodFiller.FloodFill(cell, (IntVec3 c) => this.GasCanMoveTo(c), delegate(IntVec3 c)
			{
				int num = Mathf.Min(remainingAmount, (int)(byte.MaxValue - this.DensityAt(c, gasType)));
				if (num > 0)
				{
					this.AddGas(c, gasType, num, false);
					remainingAmount -= num;
				}
				return remainingAmount <= 0;
			}, GenRadial.NumCellsInRadius(40f), true, null);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0004BB9C File Offset: 0x00049D9C
		private void TryDissipateGasses(int index)
		{
			if (!this.AnyGasAt(index))
			{
				return;
			}
			bool flag = false;
			int num = (int)this.DensityAt(index, GasType.BlindSmoke);
			if (num > 0)
			{
				num = Math.Max(num - 4, 0);
				if (num == 0)
				{
					flag = true;
				}
			}
			int num2 = (int)this.DensityAt(index, GasType.ToxGas);
			if (num2 > 0)
			{
				num2 = Math.Max(num2 - 3, 0);
				if (num2 == 0)
				{
					flag = true;
				}
			}
			int num3 = (int)this.DensityAt(index, GasType.RotStink);
			if (num3 > 0)
			{
				num3 = Math.Max(num3 - 4, 0);
				if (num3 == 0)
				{
					flag = true;
				}
			}
			this.SetDirect(index, (byte)num, (byte)num2, (byte)num3);
			if (flag)
			{
				this.map.mapDrawer.MapMeshDirty(CellIndicesUtility.IndexToCell(index, this.map.Size.x), MapMeshFlag.Gas);
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0004BC48 File Offset: 0x00049E48
		private void TryDiffuseGasses(IntVec3 cell)
		{
			int index = CellIndicesUtility.CellToIndex(cell, this.map.Size.x);
			int num = (int)this.DensityAt(index, GasType.ToxGas);
			int num2 = (int)this.DensityAt(index, GasType.RotStink);
			if (num + num2 < 17)
			{
				return;
			}
			bool flag = false;
			this.cardinalDirections.Shuffle<IntVec3>();
			for (int i = 0; i < this.cardinalDirections.Count; i++)
			{
				IntVec3 intVec = cell + this.cardinalDirections[i];
				if (this.GasCanMoveTo(intVec))
				{
					int index2 = CellIndicesUtility.CellToIndex(intVec, this.map.Size.x);
					int num3 = (int)this.DensityAt(index2, GasType.ToxGas);
					int num4 = (int)this.DensityAt(index2, GasType.RotStink);
					if (false | this.TryDiffuseIndividualGas(ref num, ref num3) | this.TryDiffuseIndividualGas(ref num2, ref num4))
					{
						this.SetDirect(index2, this.DensityAt(index2, GasType.BlindSmoke), (byte)num3, (byte)num4);
						this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlag.Gas);
						flag = true;
						if (num + num2 < 17)
						{
							break;
						}
					}
				}
			}
			if (flag)
			{
				this.SetDirect(index, this.DensityAt(index, GasType.BlindSmoke), (byte)num, (byte)num2);
				this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlag.Gas);
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0004BD84 File Offset: 0x00049F84
		private bool TryDiffuseIndividualGas(ref int gasA, ref int gasB)
		{
			if (gasA < 17)
			{
				return false;
			}
			int num = Mathf.Abs(gasA - gasB) / 2;
			if (gasA > gasB && num >= 17)
			{
				gasA -= num;
				gasB += num;
				return true;
			}
			return false;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0004BDC0 File Offset: 0x00049FC0
		private bool GasCanMoveTo(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return false;
			}
			if (cell.Filled(this.map))
			{
				Building_Door door = cell.GetDoor(this.map);
				return door != null && door.Open;
			}
			return true;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0004BE08 File Offset: 0x0004A008
		public void Debug_ClearAll()
		{
			for (int i = 0; i < this.gasDensity.Length; i++)
			{
				this.gasDensity[i] = 0;
			}
			this.anyGasEverAdded = false;
			this.map.mapDrawer.WholeMapChanged(MapMeshFlag.Gas);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0004BE50 File Offset: 0x0004A050
		public void Debug_FillAll()
		{
			for (int i = 0; i < this.gasDensity.Length; i++)
			{
				if (this.GasCanMoveTo(this.map.cellIndices.IndexToCell(i)))
				{
					this.SetDirect(i, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				}
			}
			this.anyGasEverAdded = true;
			this.map.mapDrawer.WholeMapChanged(MapMeshFlag.Gas);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0004BEBC File Offset: 0x0004A0BC
		public void ExposeData()
		{
			MapExposeUtility.ExposeInt(this.map, (IntVec3 c) => this.gasDensity[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, int val)
			{
				this.gasDensity[this.map.cellIndices.CellToIndex(c)] = val;
			}, "gasDensity");
			Scribe_Values.Look<int>(ref this.cycleIndexDiffusion, "cycleIndexDiffusion", 0, false);
			Scribe_Values.Look<int>(ref this.cycleIndexDissipation, "cycleIndexDissipation", 0, false);
		}

		// Token: 0x04000C3C RID: 3132
		private int[] gasDensity;

		// Token: 0x04000C3D RID: 3133
		private Map map;

		// Token: 0x04000C3E RID: 3134
		private int cycleIndexDiffusion;

		// Token: 0x04000C3F RID: 3135
		private int cycleIndexDissipation;

		// Token: 0x04000C40 RID: 3136
		[Unsaved(false)]
		private List<IntVec3> cardinalDirections;

		// Token: 0x04000C41 RID: 3137
		[Unsaved(false)]
		private List<IntVec3> cellsInRandomOrder;

		// Token: 0x04000C42 RID: 3138
		[Unsaved(false)]
		private bool anyGasEverAdded;

		// Token: 0x04000C43 RID: 3139
		public const int MaxGasPerCell = 255;

		// Token: 0x04000C44 RID: 3140
		private const float CellsToDissipatePerTickFactor = 0.015625f;

		// Token: 0x04000C45 RID: 3141
		private const float CellsToDiffusePerTickFactor = 0.03125f;

		// Token: 0x04000C46 RID: 3142
		private const float MaxOverflowFloodfillRadius = 40f;

		// Token: 0x04000C47 RID: 3143
		private const int DissipationAmount_BlindSmoke = 4;

		// Token: 0x04000C48 RID: 3144
		private const int DissipationAmount_ToxGas = 3;

		// Token: 0x04000C49 RID: 3145
		private const int DissipationAmount_RotStink = 4;

		// Token: 0x04000C4A RID: 3146
		private const int MinDiffusion = 17;

		// Token: 0x04000C4B RID: 3147
		private const int AnyGasCheckIntervalTicks = 600;

		// Token: 0x04000C4C RID: 3148
		private static readonly FloatRange AlphaRange = new FloatRange(0.2f, 0.8f);

		// Token: 0x04000C4D RID: 3149
		private static Color SmokeColor = new Color32(200, 200, 200, byte.MaxValue);

		// Token: 0x04000C4E RID: 3150
		private static Color ToxColor = new Color32(180, 214, 24, byte.MaxValue);

		// Token: 0x04000C4F RID: 3151
		private static Color RotColor = new Color32(214, 90, 24, byte.MaxValue);
	}
}

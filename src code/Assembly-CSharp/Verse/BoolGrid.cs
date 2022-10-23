using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001DD RID: 477
	public class BoolGrid : IExposable
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x0004A5E4 File Offset: 0x000487E4
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x0004A5EC File Offset: 0x000487EC
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				if (this.trueCountInt == 0)
				{
					yield break;
				}
				int yieldedCount = 0;
				bool canSetMinPossibleTrueIndex = this.minPossibleTrueIndexDirty;
				int num = this.minPossibleTrueIndexDirty ? 0 : this.minPossibleTrueIndexCached;
				int num2;
				for (int i = num; i < this.arr.Length; i = num2 + 1)
				{
					if (this.arr[i])
					{
						if (canSetMinPossibleTrueIndex && this.minPossibleTrueIndexDirty)
						{
							canSetMinPossibleTrueIndex = false;
							this.minPossibleTrueIndexDirty = false;
							this.minPossibleTrueIndexCached = i;
						}
						yield return CellIndicesUtility.IndexToCell(i, this.mapSizeX);
						num2 = yieldedCount;
						yieldedCount = num2 + 1;
						if (yieldedCount >= this.trueCountInt)
						{
							yield break;
						}
					}
					num2 = i;
				}
				yield break;
			}
		}

		// Token: 0x17000282 RID: 642
		public bool this[int index]
		{
			get
			{
				return this.arr[index];
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x17000283 RID: 643
		public bool this[IntVec3 c]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x17000284 RID: 644
		public bool this[int x, int z]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.Set(CellIndicesUtility.CellToIndex(x, z, this.mapSizeX), value);
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0004A65B File Offset: 0x0004885B
		public BoolGrid()
		{
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0004A66A File Offset: 0x0004886A
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0004A680 File Offset: 0x00048880
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0004A6AC File Offset: 0x000488AC
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.arr != null)
			{
				this.Clear();
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.arr = new bool[this.mapSizeX * this.mapSizeZ];
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0004A720 File Offset: 0x00048920
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.BoolArray(ref this.arr, this.mapSizeX * this.mapSizeZ, "arr");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.minPossibleTrueIndexDirty = true;
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0004A78F File Offset: 0x0004898F
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0004A7BA File Offset: 0x000489BA
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.mapSizeX), value);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0004A7D0 File Offset: 0x000489D0
		public virtual void Set(int index, bool value)
		{
			if (this.arr[index] == value)
			{
				return;
			}
			this.arr[index] = value;
			if (value)
			{
				this.trueCountInt++;
				if (this.trueCountInt == 1 || index < this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexCached = index;
					return;
				}
			}
			else
			{
				this.trueCountInt--;
				if (index == this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexDirty = true;
				}
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0004A83C File Offset: 0x00048A3C
		public void Invert()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = !this.arr[i];
			}
			this.trueCountInt = this.arr.Length - this.trueCountInt;
			this.minPossibleTrueIndexDirty = true;
		}

		// Token: 0x04000C1B RID: 3099
		private bool[] arr;

		// Token: 0x04000C1C RID: 3100
		private int trueCountInt;

		// Token: 0x04000C1D RID: 3101
		private int mapSizeX;

		// Token: 0x04000C1E RID: 3102
		private int mapSizeZ;

		// Token: 0x04000C1F RID: 3103
		private int minPossibleTrueIndexCached = -1;

		// Token: 0x04000C20 RID: 3104
		private bool minPossibleTrueIndexDirty;
	}
}

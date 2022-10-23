using System;

namespace Verse
{
	// Token: 0x020001DF RID: 479
	public class CellGrid
	{
		// Token: 0x17000289 RID: 649
		public IntVec3 this[IntVec3 c]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x1700028A RID: 650
		public IntVec3 this[int index]
		{
			get
			{
				return CellIndicesUtility.IndexToCell(this.grid[index], this.mapSizeX);
			}
			set
			{
				this.grid[index] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x1700028B RID: 651
		public IntVec3 this[int x, int z]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x0004AB5C File Offset: 0x00048D5C
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00003724 File Offset: 0x00001924
		public CellGrid()
		{
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0004AB66 File Offset: 0x00048D66
		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0004AB75 File Offset: 0x00048D75
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0004ABA0 File Offset: 0x00048DA0
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear();
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
			this.Clear();
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0004AC08 File Offset: 0x00048E08
		public void Clear()
		{
			int num = CellIndicesUtility.CellToIndex(IntVec3.Invalid, this.mapSizeX);
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = num;
			}
		}

		// Token: 0x04000C24 RID: 3108
		private int[] grid;

		// Token: 0x04000C25 RID: 3109
		private int mapSizeX;

		// Token: 0x04000C26 RID: 3110
		private int mapSizeZ;
	}
}

using System;

namespace Verse
{
	// Token: 0x020001E9 RID: 489
	public sealed class IntGrid
	{
		// Token: 0x17000297 RID: 663
		public int this[IntVec3 c]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = value;
			}
		}

		// Token: 0x17000298 RID: 664
		public int this[int index]
		{
			get
			{
				return this.grid[index];
			}
			set
			{
				this.grid[index] = value;
			}
		}

		// Token: 0x17000299 RID: 665
		public int this[int x, int z]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)] = value;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x0004CB95 File Offset: 0x0004AD95
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00003724 File Offset: 0x00001924
		public IntGrid()
		{
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0004CB9F File Offset: 0x0004AD9F
		public IntGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0004CBAE File Offset: 0x0004ADAE
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0004CBD8 File Offset: 0x0004ADD8
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0004CC38 File Offset: 0x0004AE38
		public void Clear(int value = 0)
		{
			if (value == 0)
			{
				Array.Clear(this.grid, 0, this.grid.Length);
				return;
			}
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = value;
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0004CC7C File Offset: 0x0004AE7C
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				int num = this.grid[i];
				if (num > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)(num % 100) / 100f * 0.5f);
				}
			}
		}

		// Token: 0x04000C56 RID: 3158
		private int[] grid;

		// Token: 0x04000C57 RID: 3159
		private int mapSizeX;

		// Token: 0x04000C58 RID: 3160
		private int mapSizeZ;
	}
}

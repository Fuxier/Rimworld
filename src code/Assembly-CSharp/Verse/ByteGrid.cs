using System;

namespace Verse
{
	// Token: 0x020001DE RID: 478
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x17000285 RID: 645
		public byte this[IntVec3 c]
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

		// Token: 0x17000286 RID: 646
		public byte this[int index]
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

		// Token: 0x17000287 RID: 647
		public byte this[int x, int z]
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

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x0004A905 File Offset: 0x00048B05
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00003724 File Offset: 0x00001924
		public ByteGrid()
		{
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0004A90F File Offset: 0x00048B0F
		public ByteGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0004A91E File Offset: 0x00048B1E
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0004A948 File Offset: 0x00048B48
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new byte[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0004A9A8 File Offset: 0x00048BA8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.ByteArray(ref this.grid, "grid");
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0004A9E0 File Offset: 0x00048BE0
		public void Clear(byte value = 0)
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

		// Token: 0x06000D5D RID: 3421 RVA: 0x0004AA24 File Offset: 0x00048C24
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				byte b = this.grid[i];
				if (b > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)b / 255f * 0.5f);
				}
			}
		}

		// Token: 0x04000C21 RID: 3105
		private byte[] grid;

		// Token: 0x04000C22 RID: 3106
		private int mapSizeX;

		// Token: 0x04000C23 RID: 3107
		private int mapSizeZ;
	}
}

using System;

namespace Verse
{
	// Token: 0x02000227 RID: 551
	public class MapGenFloatGrid
	{
		// Token: 0x170002FD RID: 765
		public float this[IntVec3 c]
		{
			get
			{
				return this.grid[this.map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.grid[this.map.cellIndices.CellToIndex(c)] = value;
			}
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0005AF9D File Offset: 0x0005919D
		public MapGenFloatGrid(Map map)
		{
			this.map = map;
			this.grid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x04000DF2 RID: 3570
		private Map map;

		// Token: 0x04000DF3 RID: 3571
		private float[] grid;
	}
}

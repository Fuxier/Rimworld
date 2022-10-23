using System;

namespace Verse
{
	// Token: 0x020001E2 RID: 482
	public sealed class EdificeGrid
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0004B296 File Offset: 0x00049496
		public Building[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x17000291 RID: 657
		public Building this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x17000292 RID: 658
		public Building this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0004B2C2 File Offset: 0x000494C2
		public EdificeGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Building[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0004B2E8 File Offset: 0x000494E8
		public void Register(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.innerArray[cellIndices.CellToIndex(c)] = ed;
				}
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0004B350 File Offset: 0x00049550
		public void DeRegister(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.innerArray[cellIndices.CellToIndex(j, i)] = null;
				}
			}
		}

		// Token: 0x04000C30 RID: 3120
		private Map map;

		// Token: 0x04000C31 RID: 3121
		private Building[] innerArray;
	}
}

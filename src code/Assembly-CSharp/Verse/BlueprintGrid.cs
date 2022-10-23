using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001DC RID: 476
	public sealed class BlueprintGrid
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x0004A4B4 File Offset: 0x000486B4
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0004A4BC File Offset: 0x000486BC
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0004A4E4 File Offset: 0x000486E4
		public void Register(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					if (this.innerArray[num] == null)
					{
						this.innerArray[num] = new List<Blueprint>();
					}
					this.innerArray[num].Add(ed);
				}
			}
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0004A564 File Offset: 0x00048764
		public void DeRegister(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					this.innerArray[num].Remove(ed);
					if (this.innerArray[num].Count == 0)
					{
						this.innerArray[num] = null;
					}
				}
			}
		}

		// Token: 0x04000C19 RID: 3097
		private Map map;

		// Token: 0x04000C1A RID: 3098
		private List<Blueprint>[] innerArray;
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003EF RID: 1007
	public class LinkGrid
	{
		// Token: 0x06001CAB RID: 7339 RVA: 0x000AE54D File Offset: 0x000AC74D
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000AE572 File Offset: 0x000AC772
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x000AE58C File Offset: 0x000AC78C
		public void Notify_LinkerCreatedOrDestroyed(Thing linker)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in linker.OccupiedRect())
			{
				LinkFlags linkFlags = LinkFlags.None;
				List<Thing> list = this.map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.graphicData != null)
					{
						linkFlags |= list[i].def.graphicData.linkFlags;
					}
				}
				this.linkGrid[cellIndices.CellToIndex(c)] = linkFlags;
			}
		}

		// Token: 0x04001457 RID: 5207
		private Map map;

		// Token: 0x04001458 RID: 5208
		private LinkFlags[] linkGrid;
	}
}

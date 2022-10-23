using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200057D RID: 1405
	public class MapCellsInRandomOrder
	{
		// Token: 0x06002B17 RID: 11031 RVA: 0x001139CF File Offset: 0x00111BCF
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x001139DE File Offset: 0x00111BDE
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x001139EC File Offset: 0x00111BEC
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x00113A00 File Offset: 0x00111C00
		private void CreateListIfShould()
		{
			if (this.randomizedCells != null)
			{
				return;
			}
			this.randomizedCells = new List<IntVec3>(this.map.Area);
			foreach (IntVec3 item in this.map.AllCells)
			{
				this.randomizedCells.Add(item);
			}
			Rand.PushState();
			Rand.Seed = (Find.World.info.Seed ^ this.map.Tile);
			this.randomizedCells.Shuffle<IntVec3>();
			Rand.PopState();
		}

		// Token: 0x04001C19 RID: 7193
		private Map map;

		// Token: 0x04001C1A RID: 7194
		private List<IntVec3> randomizedCells;
	}
}

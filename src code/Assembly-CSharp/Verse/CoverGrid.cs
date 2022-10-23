using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001E0 RID: 480
	public sealed class CoverGrid
	{
		// Token: 0x1700028D RID: 653
		public Thing this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x1700028E RID: 654
		public Thing this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0004AC66 File Offset: 0x00048E66
		public CoverGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Thing[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0004AC8C File Offset: 0x00048E8C
		public void Register(Thing t)
		{
			if (t.def.Fillage == FillCategory.None)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculateCell(c, null);
				}
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0004ACEC File Offset: 0x00048EEC
		public void DeRegister(Thing t)
		{
			if (t.def.Fillage == FillCategory.None)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculateCell(c, t);
				}
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0004AD4C File Offset: 0x00048F4C
		private void RecalculateCell(IntVec3 c, Thing ignoreThing = null)
		{
			Thing thing = null;
			float num = 0.001f;
			List<Thing> list = this.map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2 != ignoreThing && !thing2.Destroyed && thing2.Spawned && thing2.def.fillPercent > num)
				{
					thing = thing2;
					num = thing2.def.fillPercent;
				}
			}
			this.innerArray[this.map.cellIndices.CellToIndex(c)] = thing;
		}

		// Token: 0x04000C27 RID: 3111
		private Map map;

		// Token: 0x04000C28 RID: 3112
		private Thing[] innerArray;
	}
}

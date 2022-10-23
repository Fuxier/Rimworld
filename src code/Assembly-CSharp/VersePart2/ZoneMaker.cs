using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000271 RID: 625
	public static class ZoneMaker
	{
		// Token: 0x060011D9 RID: 4569 RVA: 0x00067FB0 File Offset: 0x000661B0
		public static Zone MakeZoneWithCells(Zone z, IEnumerable<IntVec3> cells)
		{
			if (cells != null)
			{
				foreach (IntVec3 c in cells)
				{
					z.AddCell(c);
				}
			}
			return z;
		}
	}
}

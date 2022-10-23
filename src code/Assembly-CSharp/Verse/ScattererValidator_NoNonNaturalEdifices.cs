using System;

namespace Verse
{
	// Token: 0x02000226 RID: 550
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x06000F9F RID: 3999 RVA: 0x0005AF00 File Offset: 0x00059100
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (new IntVec3(j, 0, i).GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04000DF1 RID: 3569
		public int radius = 1;
	}
}

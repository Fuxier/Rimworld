using System;

namespace Verse
{
	// Token: 0x02000225 RID: 549
	public class ScattererValidator_Buildable : ScattererValidator
	{
		// Token: 0x06000F9D RID: 3997 RVA: 0x0005AE64 File Offset: 0x00059064
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c2 = new IntVec3(j, 0, i);
					if (!c2.InBounds(map))
					{
						return false;
					}
					if (c2.InNoBuildEdgeArea(map))
					{
						return false;
					}
					if (this.affordance != null && !c2.GetTerrain(map).affordances.Contains(this.affordance))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04000DEF RID: 3567
		public int radius = 1;

		// Token: 0x04000DF0 RID: 3568
		public TerrainAffordanceDef affordance;
	}
}

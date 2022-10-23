using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000220 RID: 544
	public class GenStep_TerrainPatches : GenStep
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x000596CB File Offset: 0x000578CB
		public override int SeedPart
		{
			get
			{
				return 1370184742;
			}
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x000596D4 File Offset: 0x000578D4
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = Mathf.RoundToInt((float)map.Area / 10000f * this.patchesPer10kCellsRange.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				float randomInRange = this.patchSizeRange.RandomInRange;
				IntVec3 a = CellFinder.RandomCell(map);
				foreach (IntVec3 b in GenRadial.RadialPatternInRadius(randomInRange / 2f))
				{
					IntVec3 c = a + b;
					if (c.InBounds(map))
					{
						map.terrainGrid.SetTerrain(c, this.terrainDef);
					}
				}
			}
		}

		// Token: 0x04000DC1 RID: 3521
		public TerrainDef terrainDef;

		// Token: 0x04000DC2 RID: 3522
		public FloatRange patchesPer10kCellsRange;

		// Token: 0x04000DC3 RID: 3523
		public FloatRange patchSizeRange;
	}
}

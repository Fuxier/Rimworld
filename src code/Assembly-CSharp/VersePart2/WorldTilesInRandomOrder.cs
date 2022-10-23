using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000073 RID: 115
	public class WorldTilesInRandomOrder
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0001A0A8 File Offset: 0x000182A8
		public List<int> Tiles
		{
			get
			{
				if (this.randomizedTiles == null)
				{
					this.randomizedTiles = new List<int>();
					for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
					{
						this.randomizedTiles.Add(i);
					}
					Rand.PushState();
					Rand.Seed = Find.World.info.Seed;
					this.randomizedTiles.Shuffle<int>();
					Rand.PopState();
				}
				return this.randomizedTiles;
			}
		}

		// Token: 0x04000210 RID: 528
		private List<int> randomizedTiles;
	}
}

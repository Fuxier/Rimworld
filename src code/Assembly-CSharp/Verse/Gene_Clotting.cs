using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002AE RID: 686
	public class Gene_Clotting : Gene
	{
		// Token: 0x060013A6 RID: 5030 RVA: 0x000779D0 File Offset: 0x00075BD0
		public override void Tick()
		{
			base.Tick();
			if (this.pawn.IsHashIntervalTick(360))
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = hediffs.Count - 1; i >= 0; i--)
				{
					if (hediffs[i].Bleeding)
					{
						hediffs[i].Tended(Gene_Clotting.TendingQualityRange.RandomInRange, Gene_Clotting.TendingQualityRange.TrueMax, 1);
					}
				}
			}
		}

		// Token: 0x0400104A RID: 4170
		private const int ClotCheckInterval = 360;

		// Token: 0x0400104B RID: 4171
		private static readonly FloatRange TendingQualityRange = new FloatRange(0.2f, 0.7f);
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000061 RID: 97
	public class RitualStagePositions : IExposable
	{
		// Token: 0x06000458 RID: 1112 RVA: 0x000182B8 File Offset: 0x000164B8
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn, PawnStagePosition>(ref this.positions, "positions", LookMode.Reference, LookMode.Deep, ref this.pawnListTmp, ref this.positionListTmp, true);
		}

		// Token: 0x04000172 RID: 370
		public Dictionary<Pawn, PawnStagePosition> positions = new Dictionary<Pawn, PawnStagePosition>();

		// Token: 0x04000173 RID: 371
		private List<Pawn> pawnListTmp;

		// Token: 0x04000174 RID: 372
		private List<PawnStagePosition> positionListTmp;
	}
}

using System;

namespace Verse
{
	// Token: 0x020002F2 RID: 754
	public class HediffComp_GiveNeurocharge : HediffComp
	{
		// Token: 0x060014FB RID: 5371 RVA: 0x0007EDD4 File Offset: 0x0007CFD4
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.parent.pawn.health.lastReceivedNeuralSuperchargeTick = Find.TickManager.TicksGame;
		}
	}
}

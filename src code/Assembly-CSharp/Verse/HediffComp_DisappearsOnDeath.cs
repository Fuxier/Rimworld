using System;

namespace Verse
{
	// Token: 0x020002D9 RID: 729
	public class HediffComp_DisappearsOnDeath : HediffComp
	{
		// Token: 0x060014B5 RID: 5301 RVA: 0x0007DA8A File Offset: 0x0007BC8A
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			base.Pawn.health.RemoveHediff(this.parent);
		}
	}
}

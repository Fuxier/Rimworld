using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002D6 RID: 726
	public class HediffComp_DisappearsPausable : HediffComp_Disappears
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x0000249D File Offset: 0x0000069D
		protected virtual bool Paused
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x0007D9CF File Offset: 0x0007BBCF
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!base.Props.showRemainingTime || this.Paused)
				{
					return null;
				}
				return this.ticksToDisappear.ToStringTicksToPeriod(true, true, true, true, true);
			}
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0007D9F8 File Offset: 0x0007BBF8
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(120) && !this.Paused)
			{
				this.ticksToDisappear -= 120;
			}
		}

		// Token: 0x040010CB RID: 4299
		private const int PauseCheckInterval = 120;
	}
}

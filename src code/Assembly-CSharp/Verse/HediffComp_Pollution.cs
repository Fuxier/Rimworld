using System;

namespace Verse
{
	// Token: 0x0200030D RID: 781
	public class HediffComp_Pollution : HediffComp
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001553 RID: 5459 RVA: 0x00080196 File Offset: 0x0007E396
		public HediffCompProperties_Pollution Props
		{
			get
			{
				return (HediffCompProperties_Pollution)this.props;
			}
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x000801A4 File Offset: 0x0007E3A4
		public override void CompPostTick(ref float severityAdjustment)
		{
			Pawn pawn = this.parent.pawn;
			if (pawn.IsHashIntervalTick(this.Props.interval))
			{
				if (pawn.Spawned && pawn.Position.IsPolluted(pawn.Map))
				{
					severityAdjustment += this.Props.pollutedSeverity;
					return;
				}
				severityAdjustment += this.Props.unpollutedSeverity;
			}
		}
	}
}

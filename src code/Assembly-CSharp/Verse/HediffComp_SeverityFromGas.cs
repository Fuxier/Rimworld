using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000321 RID: 801
	public class HediffComp_SeverityFromGas : HediffComp
	{
		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x00080ADD File Offset: 0x0007ECDD
		public HediffCompProperties_SeverityFromGas Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromGas)this.props;
			}
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x00080AEC File Offset: 0x0007ECEC
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Props.gasType == GasType.ToxGas && !ModsConfig.BiotechActive)
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned && pawn.IsHashIntervalTick(this.Props.intervalTicks))
			{
				if (pawn.Position.AnyGas(pawn.Map, this.Props.gasType))
				{
					float num = (float)pawn.Position.GasDentity(pawn.Map, this.Props.gasType) / 255f * this.Props.severityGasDensityFactor;
					if (this.Props.exposureStatFactor != null)
					{
						num *= 1f - pawn.GetStatValue(this.Props.exposureStatFactor, true, -1);
					}
					severityAdjustment += num;
					return;
				}
				severityAdjustment += this.Props.severityNotExposed;
			}
		}
	}
}

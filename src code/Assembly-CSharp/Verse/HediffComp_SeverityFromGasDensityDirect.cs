using System;

namespace Verse
{
	// Token: 0x02000323 RID: 803
	public class HediffComp_SeverityFromGasDensityDirect : HediffComp
	{
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x00080C0C File Offset: 0x0007EE0C
		public HediffCompProperties_SeverityFromGasDensityDirect Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromGasDensityDirect)this.props;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001591 RID: 5521 RVA: 0x00080C1C File Offset: 0x0007EE1C
		public override bool CompShouldRemove
		{
			get
			{
				return base.Pawn.Dead || !base.Pawn.Spawned || !base.Pawn.Position.AnyGas(base.Pawn.Map, this.Props.gasType);
			}
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00080C6E File Offset: 0x0007EE6E
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.UpdateSeverity();
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x00080C76 File Offset: 0x0007EE76
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(this.Props.intervalTicks))
			{
				this.UpdateSeverity();
			}
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x00080C98 File Offset: 0x0007EE98
		private void UpdateSeverity()
		{
			if (!base.Pawn.Spawned)
			{
				return;
			}
			if (this.Props.gasType == GasType.ToxGas && !ModsConfig.BiotechActive)
			{
				return;
			}
			float num = base.Pawn.Map.gasGrid.DensityPercentAt(base.Pawn.Position, this.Props.gasType);
			for (int i = 0; i <= this.Props.densityStages.Count; i++)
			{
				if (num <= this.Props.densityStages[i])
				{
					this.parent.Severity = (float)(i + 1);
					return;
				}
			}
		}
	}
}

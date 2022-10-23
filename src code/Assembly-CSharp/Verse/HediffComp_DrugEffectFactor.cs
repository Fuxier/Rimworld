using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002DF RID: 735
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060014C9 RID: 5321 RVA: 0x0007E169 File Offset: 0x0007C369
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x0007E176 File Offset: 0x0007C376
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x0007E190 File Offset: 0x0007C390
		public override string CompTipStringExtra
		{
			get
			{
				return "DrugEffectMultiplier".Translate(this.Props.chemical.label, this.CurrentFactor.ToStringPercent()).CapitalizeFirst();
			}
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0007E1D9 File Offset: 0x0007C3D9
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}

		// Token: 0x040010D7 RID: 4311
		private static readonly SimpleCurve EffectFactorSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};
	}
}

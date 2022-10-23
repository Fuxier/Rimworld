using System;

namespace Verse
{
	// Token: 0x020002D1 RID: 721
	[StaticConstructorOnStartup]
	public class HediffComp_Chargeable : HediffComp
	{
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0007D47C File Offset: 0x0007B67C
		public HediffCompProperties_Chargeable Props
		{
			get
			{
				return (HediffCompProperties_Chargeable)this.props;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0007D489 File Offset: 0x0007B689
		// (set) Token: 0x06001490 RID: 5264 RVA: 0x0007D491 File Offset: 0x0007B691
		public float Charge
		{
			get
			{
				return this.charge;
			}
			protected set
			{
				this.charge = value;
				if (this.charge > this.Props.fullChargeAmount)
				{
					this.charge = this.Props.fullChargeAmount;
				}
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x0007D4BE File Offset: 0x0007B6BE
		public bool CanActivate
		{
			get
			{
				return this.charge >= this.Props.minChargeToActivate;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0007D4D8 File Offset: 0x0007B6D8
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.Props.labelInBrackets.Formatted(this.charge.Named("CHARGE"), (this.charge / this.Props.fullChargeAmount).Named("CHARGEFACTOR"));
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0007D530 File Offset: 0x0007B730
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.charge = this.Props.initialCharge;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0007D549 File Offset: 0x0007B749
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.charge, "charge", this.Props.initialCharge, false);
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0007D56D File Offset: 0x0007B76D
		public virtual void TryCharge(float desiredChargeAmount)
		{
			this.Charge += desiredChargeAmount;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0007D580 File Offset: 0x0007B780
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.Props.ticksToFullCharge > 0)
			{
				this.TryCharge(this.Props.fullChargeAmount / (float)this.Props.ticksToFullCharge);
				return;
			}
			if (this.Props.ticksToFullCharge == 0)
			{
				this.TryCharge(this.Props.fullChargeAmount);
			}
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0007D5E0 File Offset: 0x0007B7E0
		public float GreedyConsume(float desiredCharge)
		{
			float num;
			if (desiredCharge >= this.charge)
			{
				num = this.charge;
				this.charge = 0f;
			}
			else
			{
				num = desiredCharge;
				this.charge -= num;
			}
			return num;
		}

		// Token: 0x040010BE RID: 4286
		private float charge;
	}
}

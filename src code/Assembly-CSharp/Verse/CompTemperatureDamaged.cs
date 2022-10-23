using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200041B RID: 1051
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x000B7ADD File Offset: 0x000B5CDD
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000B7AEA File Offset: 0x000B5CEA
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000B7B04 File Offset: 0x000B5D04
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000B7B0C File Offset: 0x000B5D0C
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}
	}
}

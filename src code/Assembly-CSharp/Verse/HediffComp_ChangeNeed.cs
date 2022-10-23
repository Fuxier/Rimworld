using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002CF RID: 719
	public class HediffComp_ChangeNeed : HediffComp
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0007D3E7 File Offset: 0x0007B5E7
		public HediffCompProperties_ChangeNeed Props
		{
			get
			{
				return (HediffCompProperties_ChangeNeed)this.props;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x0007D3F4 File Offset: 0x0007B5F4
		private Need Need
		{
			get
			{
				if (this.needCached == null)
				{
					this.needCached = base.Pawn.needs.TryGetNeed(this.Props.needDef);
				}
				return this.needCached;
			}
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0007D425 File Offset: 0x0007B625
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Need != null)
			{
				this.Need.CurLevelPercentage += this.Props.percentPerDay / 60000f;
			}
		}

		// Token: 0x040010B8 RID: 4280
		private Need needCached;
	}
}

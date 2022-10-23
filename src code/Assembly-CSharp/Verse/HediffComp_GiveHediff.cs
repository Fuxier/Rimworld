using System;

namespace Verse
{
	// Token: 0x020002EA RID: 746
	public class HediffComp_GiveHediff : HediffComp
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x0007E7F9 File Offset: 0x0007C9F9
		private HediffCompProperties_GiveHediff Props
		{
			get
			{
				return (HediffCompProperties_GiveHediff)this.props;
			}
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0007E808 File Offset: 0x0007CA08
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.Props.skipIfAlreadyExists && this.parent.pawn.health.hediffSet.HasHediff(this.Props.hediffDef, false))
			{
				return;
			}
			this.parent.pawn.health.AddHediff(this.Props.hediffDef, null, null, null);
		}
	}
}

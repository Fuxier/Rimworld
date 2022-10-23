using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002F0 RID: 752
	public class HediffComp_GiveHemogen : HediffComp
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060014F7 RID: 5367 RVA: 0x0007ED90 File Offset: 0x0007CF90
		public HediffCompProperties_GiveHemogen Props
		{
			get
			{
				return (HediffCompProperties_GiveHemogen)this.props;
			}
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x0007ED9D File Offset: 0x0007CF9D
		public override void CompPostTick(ref float severityAdjustment)
		{
			GeneUtility.OffsetHemogen(base.Pawn, this.Props.amountPerDay / 60000f, true);
		}
	}
}

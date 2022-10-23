using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000326 RID: 806
	public class HediffComp_SeverityFromHemogen : HediffComp
	{
		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001599 RID: 5529 RVA: 0x00080D70 File Offset: 0x0007EF70
		public HediffCompProperties_SeverityFromHemogen Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromHemogen)this.props;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600159A RID: 5530 RVA: 0x00080D7D File Offset: 0x0007EF7D
		public override bool CompShouldRemove
		{
			get
			{
				Pawn_GeneTracker genes = base.Pawn.genes;
				return ((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) == null;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600159B RID: 5531 RVA: 0x00080D99 File Offset: 0x0007EF99
		private Gene_Hemogen Hemogen
		{
			get
			{
				if (this.cachedHemogenGene == null)
				{
					this.cachedHemogenGene = base.Pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
				}
				return this.cachedHemogenGene;
			}
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00080DBF File Offset: 0x0007EFBF
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			severityAdjustment += ((this.Hemogen.Value > 0f) ? this.Props.severityPerHourHemogen : this.Props.severityPerHourEmpty) / 2500f;
		}

		// Token: 0x04001150 RID: 4432
		private Gene_Hemogen cachedHemogenGene;
	}
}

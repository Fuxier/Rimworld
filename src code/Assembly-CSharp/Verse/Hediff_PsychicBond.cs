using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200034C RID: 844
	public class Hediff_PsychicBond : HediffWithTarget
	{
		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060016B0 RID: 5808 RVA: 0x0008555F File Offset: 0x0008375F
		public override string LabelBase
		{
			get
			{
				string labelBase = base.LabelBase;
				string str = " (";
				Thing target = this.target;
				return labelBase + str + ((target != null) ? target.LabelShortCap : null) + ")";
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060016B1 RID: 5809 RVA: 0x00085588 File Offset: 0x00083788
		public override bool ShouldRemove
		{
			get
			{
				return base.ShouldRemove || this.pawn.Dead;
			}
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x000855A0 File Offset: 0x000837A0
		public override void PostRemoved()
		{
			Pawn_GeneTracker genes = this.pawn.genes;
			Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			if (gene_PsychicBonding != null)
			{
				gene_PsychicBonding.RemoveBond();
				return;
			}
			Pawn pawn;
			if (this.target != null && (pawn = (this.target as Pawn)) != null)
			{
				Pawn_GeneTracker genes2 = pawn.genes;
				if (genes2 == null)
				{
					return;
				}
				Gene_PsychicBonding firstGeneOfType = genes2.GetFirstGeneOfType<Gene_PsychicBonding>();
				if (firstGeneOfType == null)
				{
					return;
				}
				firstGeneOfType.RemoveBond();
			}
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00085600 File Offset: 0x00083800
		public override void PostTick()
		{
			base.PostTick();
			if (this.pawn.IsHashIntervalTick(65))
			{
				this.Severity = (ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(this.pawn, this) ? 0.5f : 1.5f);
			}
		}

		// Token: 0x0400119E RID: 4510
		private const int HediffCheckInterval = 65;
	}
}

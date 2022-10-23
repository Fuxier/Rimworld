using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000341 RID: 833
	public class Hediff_ChemicalDependency : HediffWithComps
	{
		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001639 RID: 5689 RVA: 0x000832B2 File Offset: 0x000814B2
		public override string LabelBase
		{
			get
			{
				return "ChemicalDependency".Translate(this.chemical.Named("CHEMICAL"));
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600163A RID: 5690 RVA: 0x0000249D File Offset: 0x0000069D
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600163B RID: 5691 RVA: 0x000832D3 File Offset: 0x000814D3
		public override bool Visible
		{
			get
			{
				return (this.LinkedGene == null || this.LinkedGene.Active) && base.Visible;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x000832F2 File Offset: 0x000814F2
		public bool ShouldSatify
		{
			get
			{
				return this.Severity >= this.def.stages[1].minSeverity - 0.1f;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600163D RID: 5693 RVA: 0x0008331C File Offset: 0x0008151C
		public Gene_ChemicalDependency LinkedGene
		{
			get
			{
				if (this.cachedDependencyGene == null && this.pawn.genes != null)
				{
					List<Gene> genesListForReading = this.pawn.genes.GenesListForReading;
					for (int i = 0; i < genesListForReading.Count; i++)
					{
						Gene_ChemicalDependency gene_ChemicalDependency;
						if ((gene_ChemicalDependency = (genesListForReading[i] as Gene_ChemicalDependency)) != null && gene_ChemicalDependency.def.chemical == this.chemical)
						{
							this.cachedDependencyGene = gene_ChemicalDependency;
							break;
						}
					}
				}
				return this.cachedDependencyGene;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600163E RID: 5694 RVA: 0x00083394 File Offset: 0x00081594
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				Gene_ChemicalDependency linkedGene = this.LinkedGene;
				if (linkedGene != null)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n\n";
					}
					text += "GeneDefChemicalNeedDurationDesc".Translate(this.chemical.label, this.pawn.Named("PAWN"), "PeriodDays".Translate(5f).Named("DEFICIENCYDURATION"), "PeriodDays".Translate(30f).Named("COMADURATION"), "PeriodDays".Translate(60f).Named("DEATHDURATION")).Resolve();
					text = text + "\n\n" + "LastIngestedDurationAgo".Translate(this.chemical.Named("CHEMICAL"), (Find.TickManager.TicksGame - linkedGene.lastIngestedTick).ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")).Resolve();
				}
				return text;
			}
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x000834C0 File Offset: 0x000816C0
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_ChemicalDependency hediff_ChemicalDependency;
			return (hediff_ChemicalDependency = (other as Hediff_ChemicalDependency)) != null && hediff_ChemicalDependency.chemical == this.chemical && base.TryMergeWith(other);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000834F0 File Offset: 0x000816F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ChemicalDef>(ref this.chemical, "chemical");
		}

		// Token: 0x04001187 RID: 4487
		public ChemicalDef chemical;

		// Token: 0x04001188 RID: 4488
		[Unsaved(false)]
		private Gene_ChemicalDependency cachedDependencyGene;
	}
}

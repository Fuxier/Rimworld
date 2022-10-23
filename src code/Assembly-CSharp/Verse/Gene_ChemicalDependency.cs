using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002AF RID: 687
	public class Gene_ChemicalDependency : Gene
	{
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x00077A6C File Offset: 0x00075C6C
		public Hediff_ChemicalDependency LinkedHediff
		{
			get
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_ChemicalDependency hediff_ChemicalDependency;
					if ((hediff_ChemicalDependency = (hediffs[i] as Hediff_ChemicalDependency)) != null && hediff_ChemicalDependency.chemical == this.def.chemical)
					{
						return hediff_ChemicalDependency;
					}
				}
				return null;
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00077AC8 File Offset: 0x00075CC8
		public override void PostAdd()
		{
			if (!ModLister.CheckBiotech("Chemical dependency"))
			{
				return;
			}
			base.PostAdd();
			Hediff_ChemicalDependency hediff_ChemicalDependency = (Hediff_ChemicalDependency)HediffMaker.MakeHediff(HediffDefOf.GeneticDrugNeed, this.pawn, null);
			hediff_ChemicalDependency.chemical = this.def.chemical;
			this.pawn.health.AddHediff(hediff_ChemicalDependency, null, null, null);
			this.lastIngestedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00077B3C File Offset: 0x00075D3C
		public override void PostRemove()
		{
			Hediff_ChemicalDependency linkedHediff = this.LinkedHediff;
			if (linkedHediff != null)
			{
				this.pawn.health.RemoveHediff(linkedHediff);
			}
			base.PostRemove();
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00077B6C File Offset: 0x00075D6C
		public override void Notify_IngestedThing(Thing thing, int numTaken)
		{
			if (!thing.def.thingCategories.NullOrEmpty<ThingCategoryDef>() && !thing.def.thingCategories.Contains(ThingCategoryDefOf.Drugs))
			{
				return;
			}
			CompDrug compDrug = thing.TryGetComp<CompDrug>();
			if (compDrug == null || compDrug.Props.chemical != this.def.chemical)
			{
				return;
			}
			this.Reset();
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00077BCC File Offset: 0x00075DCC
		public override void Reset()
		{
			Hediff_ChemicalDependency linkedHediff = this.LinkedHediff;
			if (linkedHediff != null)
			{
				linkedHediff.Severity = linkedHediff.def.initialSeverity;
			}
			this.lastIngestedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00077C04 File Offset: 0x00075E04
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastIngestedTick, "lastIngestedTick", 0, false);
		}

		// Token: 0x0400104C RID: 4172
		public int lastIngestedTick;
	}
}

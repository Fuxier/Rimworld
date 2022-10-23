using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200033B RID: 827
	public class HediffWithParents : HediffWithComps
	{
		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x00082DCA File Offset: 0x00080FCA
		public Pawn Father
		{
			get
			{
				return this.father;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600161D RID: 5661 RVA: 0x00082DD2 File Offset: 0x00080FD2
		public Pawn Mother
		{
			get
			{
				return this.mother;
			}
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x00082DDA File Offset: 0x00080FDA
		public void SetParents(Pawn mother, Pawn father, GeneSet geneSet)
		{
			this.mother = mother;
			this.father = father;
			this.geneSet = geneSet;
			Find.WorldPawns.AddPreservedPawnHediff(mother, this);
			Find.WorldPawns.AddPreservedPawnHediff(father, this);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00082E09 File Offset: 0x00081009
		public override void PreRemoved()
		{
			base.PreRemoved();
			Find.WorldPawns.RemovePreservedPawnHediff(this.mother, this);
			Find.WorldPawns.RemovePreservedPawnHediff(this.father, this);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00082E33 File Offset: 0x00081033
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.geneSet != null)
			{
				foreach (StatDrawEntry statDrawEntry2 in this.geneSet.SpecialDisplayStats(null))
				{
					yield return statDrawEntry2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00082E4A File Offset: 0x0008104A
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (ModsConfig.BiotechActive && !this.pawn.Drafted)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "InspectBabyGenes".Translate() + "...";
				command_Action.defaultDesc = "InspectGenesHediffDesc".Translate();
				command_Action.icon = GeneSetHolderBase.GeneticInfoTex.Texture;
				command_Action.action = delegate()
				{
					InspectPaneUtility.OpenTab(typeof(ITab_GenesPregnancy));
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00082E5C File Offset: 0x0008105C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.father, "father", true);
			Scribe_References.Look<Pawn>(ref this.mother, "mother", true);
			Scribe_Deep.Look<GeneSet>(ref this.geneSet, "geneSet", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Find.WorldPawns.AddPreservedPawnHediff(this.mother, this);
				Find.WorldPawns.AddPreservedPawnHediff(this.father, this);
			}
		}

		// Token: 0x0400117F RID: 4479
		private Pawn father;

		// Token: 0x04001180 RID: 4480
		private Pawn mother;

		// Token: 0x04001181 RID: 4481
		public GeneSet geneSet;
	}
}

using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000349 RID: 841
	public class Hediff_Mechlink : HediffWithComps
	{
		// Token: 0x06001689 RID: 5769 RVA: 0x00084544 File Offset: 0x00082744
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (!ModLister.CheckBiotech("Mechlink"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
			if (this.pawn.Spawned)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelMechlinkInstalled".Translate() + ": " + this.pawn.LabelShortCap, "LetterMechlinkInstalled".Translate(this.pawn.Named("PAWN")), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
			}
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x000845EB File Offset: 0x000827EB
		public override void PostRemoved()
		{
			base.PostRemoved();
			Pawn_MechanitorTracker mechanitor = this.pawn.mechanitor;
			if (mechanitor == null)
			{
				return;
			}
			mechanitor.Notify_MechlinkRemoved();
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x00084608 File Offset: 0x00082808
		public override void PostTick()
		{
			base.PostTick();
			if (this.pawn.Spawned && this.pawn.IsHashIntervalTick(300))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.Mechanitors, OpportunityType.Important);
			}
		}

		// Token: 0x04001196 RID: 4502
		private const int LearningOpportunityCheckInterval = 300;
	}
}

using System;

namespace Verse
{
	// Token: 0x0200031B RID: 795
	public class HediffComp_RemoveIfOtherHediff : HediffComp_MessageBase
	{
		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x000808E4 File Offset: 0x0007EAE4
		protected HediffCompProperties_RemoveIfOtherHediff Props
		{
			get
			{
				return (HediffCompProperties_RemoveIfOtherHediff)this.props;
			}
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x000808F1 File Offset: 0x0007EAF1
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.ShouldRemove())
			{
				this.Message();
				this.parent.pawn.health.RemoveHediff(this.parent);
			}
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00080924 File Offset: 0x0007EB24
		private bool ShouldRemove()
		{
			if (base.CompShouldRemove)
			{
				return true;
			}
			Hediff firstHediffOfDef;
			if ((firstHediffOfDef = base.Pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.otherHediff, false)) == null)
			{
				return false;
			}
			if (this.Props.stages != null && !this.Props.stages.Value.Includes(firstHediffOfDef.CurStageIndex))
			{
				return false;
			}
			if (this.Props.mtbHours > 0)
			{
				if (!base.Pawn.IsHashIntervalTick(1000))
				{
					return false;
				}
				if (!Rand.MTBEventOccurs((float)this.Props.mtbHours, 2500f, 1000f))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001142 RID: 4418
		private const int MtbRemovalCheckInterval = 1000;
	}
}

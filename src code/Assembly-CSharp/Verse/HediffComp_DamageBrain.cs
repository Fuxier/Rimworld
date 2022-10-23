using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002D3 RID: 723
	public class HediffComp_DamageBrain : HediffComp
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x0007D655 File Offset: 0x0007B855
		public HediffCompProperties_DamageBrain Props
		{
			get
			{
				return (HediffCompProperties_DamageBrain)this.props;
			}
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0007D664 File Offset: 0x0007B864
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Props.mtbDaysPerStage[this.parent.CurStageIndex] > 0f && base.Pawn.IsHashIntervalTick(60) && Rand.MTBEventOccurs(this.Props.mtbDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
			{
				BodyPartRecord brain = base.Pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				int randomInRange = this.Props.damageAmount.RandomInRange;
				base.Pawn.TakeDamage(new DamageInfo(DamageDefOf.Burn, (float)randomInRange, 0f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
				Messages.Message("MessageReceivedBrainDamageFromHediff".Translate(base.Pawn.Named("PAWN"), randomInRange, this.parent.Label), base.Pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}
	}
}

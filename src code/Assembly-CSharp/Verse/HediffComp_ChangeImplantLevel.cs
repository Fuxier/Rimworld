using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002CD RID: 717
	public class HediffComp_ChangeImplantLevel : HediffComp
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x0007D244 File Offset: 0x0007B444
		public HediffCompProperties_ChangeImplantLevel Props
		{
			get
			{
				return (HediffCompProperties_ChangeImplantLevel)this.props;
			}
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0007D254 File Offset: 0x0007B454
		public override void CompPostTick(ref float severityAdjustment)
		{
			float mtbDays = this.Props.probabilityPerStage[this.parent.CurStageIndex].mtbDays;
			if (mtbDays > 0f && base.Pawn.IsHashIntervalTick(60))
			{
				ChangeImplantLevel_Probability changeImplantLevel_Probability = this.Props.probabilityPerStage[this.parent.CurStageIndex];
				if ((this.lastChangeLevelTick < 0 || (float)(Find.TickManager.TicksGame - this.lastChangeLevelTick) >= changeImplantLevel_Probability.minIntervalDays * 60000f) && Rand.MTBEventOccurs(mtbDays, 60000f, 60f))
				{
					Hediff_Level hediff_Level = this.parent.pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.implant, false) as Hediff_Level;
					if (hediff_Level != null)
					{
						hediff_Level.ChangeLevel(this.Props.levelOffset);
						this.lastChangeLevelTick = Find.TickManager.TicksGame;
						Messages.Message("MessageLostImplantLevelFromHediff".Translate(this.parent.pawn.Named("PAWN"), hediff_Level.LabelBase, this.parent.Label), this.parent.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
				}
			}
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0007D3A6 File Offset: 0x0007B5A6
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastChangeLevelTick, "lastChangeLevelTick", 0, false);
		}

		// Token: 0x040010B5 RID: 4277
		public int lastChangeLevelTick = -1;
	}
}

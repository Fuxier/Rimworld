using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000313 RID: 787
	public class HediffComp_RandomizeStageWithInterval : HediffComp_Randomizer
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x000805A5 File Offset: 0x0007E7A5
		public HediffCompProperties_RandomizeStageWithInterval Props
		{
			get
			{
				return (HediffCompProperties_RandomizeStageWithInterval)this.props;
			}
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x000805B4 File Offset: 0x0007E7B4
		public override void Randomize()
		{
			int curStageIndex = this.parent.CurStageIndex;
			this.parent.Severity = this.parent.def.stages.RandomElement<HediffStage>().minSeverity;
			int curStageIndex2 = this.parent.CurStageIndex;
			if (curStageIndex != curStageIndex2 && !this.Props.notifyMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
			{
				Messages.Message(this.Props.notifyMessage.Formatted(this.parent.pawn.Named("PAWN"), this.parent.def.stages[curStageIndex].label, this.parent.def.stages[curStageIndex2].label), this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}
	}
}

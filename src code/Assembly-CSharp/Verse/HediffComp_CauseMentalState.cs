using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C8 RID: 712
	public class HediffComp_CauseMentalState : HediffComp
	{
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x0007CD90 File Offset: 0x0007AF90
		public HediffCompProperties_CauseMentalState Props
		{
			get
			{
				return (HediffCompProperties_CauseMentalState)this.props;
			}
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0007CDA0 File Offset: 0x0007AFA0
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(60))
			{
				if (base.Pawn.RaceProps.Humanlike)
				{
					if (base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.humanMentalState && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.humanMentalState, this.parent.def.LabelCap, false, false, null, true, false, false) && base.Pawn.Spawned)
					{
						this.SendLetter(this.Props.humanMentalState);
						return;
					}
				}
				else if (base.Pawn.RaceProps.Animal && base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalState && (this.Props.animalMentalStateAlias == null || base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalStateAlias) && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.animalMentalState, this.parent.def.LabelCap, false, false, null, true, false, false) && base.Pawn.Spawned)
				{
					this.SendLetter(this.Props.animalMentalState);
				}
			}
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0007CF7C File Offset: 0x0007B17C
		public override void CompPostPostRemoved()
		{
			if (this.Props.endMentalStateOnCure && ((base.Pawn.RaceProps.Humanlike && base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.humanMentalState) || (base.Pawn.RaceProps.Animal && (base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalState || base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalStateAlias))) && !base.Pawn.mindState.mentalStateHandler.CurState.causedByMood)
			{
				base.Pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0007D05C File Offset: 0x0007B25C
		private void SendLetter(MentalStateDef mentalStateDef)
		{
			Find.LetterStack.ReceiveLetter((mentalStateDef.beginLetterLabel ?? mentalStateDef.LabelCap).CapitalizeFirst() + ": " + base.Pawn.LabelShortCap, base.Pawn.mindState.mentalStateHandler.CurState.GetBeginLetterText() + "\n\n" + "CausedByHediff".Translate(this.parent.LabelCap), this.Props.letterDef, base.Pawn, null, null, null, null);
		}
	}
}

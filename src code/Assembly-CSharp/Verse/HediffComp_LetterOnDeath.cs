using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000303 RID: 771
	public class HediffComp_LetterOnDeath : HediffComp
	{
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x0007FAD4 File Offset: 0x0007DCD4
		public HediffCompProperties_LetterOnDeath Props
		{
			get
			{
				return (HediffCompProperties_LetterOnDeath)this.props;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0007FAE1 File Offset: 0x0007DCE1
		private bool ShouldSendLetter
		{
			get
			{
				return (!this.Props.onlyIfNoMechanitorDied || !Find.History.mechanitorEverDied) && (this.parent.pawn == null || !PawnGenerator.IsBeingGenerated(this.parent.pawn));
			}
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0007FB20 File Offset: 0x0007DD20
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			if (this.ShouldSendLetter)
			{
				Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(this.parent.Named("HEDIFF")), this.Props.letterText.Formatted(this.parent.pawn.Named("PAWN"), this.parent.Named("HEDIFF")), this.Props.letterDef ?? LetterDefOf.NeutralEvent, this.parent.pawn, null, null, null, null);
			}
		}
	}
}

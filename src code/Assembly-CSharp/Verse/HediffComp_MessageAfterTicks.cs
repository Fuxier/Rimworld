using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000307 RID: 775
	public class HediffComp_MessageAfterTicks : HediffComp
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001544 RID: 5444 RVA: 0x0007FE5C File Offset: 0x0007E05C
		protected HediffCompProperties_MessageAfterTicks Props
		{
			get
			{
				return (HediffCompProperties_MessageAfterTicks)this.props;
			}
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0007FE69 File Offset: 0x0007E069
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksUntilMessage = this.Props.ticks;
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0007FE84 File Offset: 0x0007E084
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.ticksUntilMessage == 0)
			{
				if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
				{
					if (this.Props.messageType != null)
					{
						Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType, true);
					}
					if (this.Props.letterType != null)
					{
						Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(base.Pawn), this.GetLetterText(), this.Props.letterType, base.Pawn, null, null, null, null);
					}
				}
				this.ticksUntilMessage--;
				return;
			}
			if (this.ticksUntilMessage > 0)
			{
				this.ticksUntilMessage--;
			}
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0007FF74 File Offset: 0x0007E174
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.ticksUntilMessage, "ticksUntilMessage", 0, false);
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0007FF90 File Offset: 0x0007E190
		private TaggedString GetLetterText()
		{
			Hediff_Pregnant hediff_Pregnant;
			if ((hediff_Pregnant = (this.parent as Hediff_Pregnant)) != null && hediff_Pregnant.Mother != null && hediff_Pregnant.Mother != hediff_Pregnant.pawn)
			{
				TaggedString taggedString = "IvfPregnancyLetterText".Translate(hediff_Pregnant.pawn.NameFullColored);
				if (hediff_Pregnant.Mother != null && hediff_Pregnant.Father != null)
				{
					taggedString += "\n\n" + "IvfPregnancyLetterParents".Translate(hediff_Pregnant.Mother.NameFullColored, hediff_Pregnant.Father.NameFullColored);
				}
				return taggedString;
			}
			return this.Props.letterText.Formatted(base.Pawn);
		}

		// Token: 0x04001123 RID: 4387
		private int ticksUntilMessage;
	}
}

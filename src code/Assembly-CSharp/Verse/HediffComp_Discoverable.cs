using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002DB RID: 731
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060014B8 RID: 5304 RVA: 0x0007DAC0 File Offset: 0x0007BCC0
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0007DACD File Offset: 0x0007BCCD
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0007DAE1 File Offset: 0x0007BCE1
		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0007DAEC File Offset: 0x0007BCEC
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0007DB03 File Offset: 0x0007BD03
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0007DB0C File Offset: 0x0007BD0C
		private void CheckDiscovered()
		{
			if (this.discovered)
			{
				return;
			}
			if (!this.parent.CurStage.becomeVisible)
			{
				return;
			}
			this.discovered = true;
			if (this.Props.sendLetterWhenDiscovered && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				if (base.Pawn.RaceProps.Humanlike)
				{
					string str;
					if (!this.Props.discoverLetterLabel.NullOrEmpty())
					{
						str = string.Format(this.Props.discoverLetterLabel, base.Pawn.LabelShortCap).CapitalizeFirst();
					}
					else
					{
						str = "LetterLabelNewDisease".Translate() + ": " + base.Def.LabelCap;
					}
					string str2;
					if (!this.Props.discoverLetterText.NullOrEmpty())
					{
						str2 = this.Props.discoverLetterText.Formatted(base.Pawn.LabelIndefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else if (this.parent.Part == null)
					{
						str2 = "NewDisease".Translate(base.Pawn.Named("PAWN"), base.Def.label, base.Pawn.LabelDefinite()).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else
					{
						str2 = "NewPartDisease".Translate(base.Pawn.Named("PAWN"), this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.label).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					Find.LetterStack.ReceiveLetter(str, str2, (this.Props.letterType != null) ? this.Props.letterType : LetterDefOf.NegativeEvent, base.Pawn, null, null, null, null);
					return;
				}
				string text;
				if (!this.Props.discoverLetterText.NullOrEmpty())
				{
					string value = base.Pawn.KindLabelIndefinite();
					if (base.Pawn.Name.IsValid && !base.Pawn.Name.Numerical)
					{
						value = string.Concat(new object[]
						{
							base.Pawn.Name,
							" (",
							base.Pawn.KindLabel,
							")"
						});
					}
					text = this.Props.discoverLetterText.Formatted(value, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else if (this.parent.Part == null)
				{
					text = "NewDiseaseAnimal".Translate(base.Pawn.LabelShort, base.Def.LabelCap, base.Pawn.LabelDefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else
				{
					text = "NewPartDiseaseAnimal".Translate(base.Pawn.LabelShort, this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.LabelCap, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				Messages.Message(text, base.Pawn, (this.Props.messageType != null) ? this.Props.messageType : MessageTypeDefOf.NegativeHealthEvent, true);
			}
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0007DB03 File Offset: 0x0007BD03
		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0007DF57 File Offset: 0x0007C157
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered.ToString();
		}

		// Token: 0x040010D1 RID: 4305
		private bool discovered;
	}
}

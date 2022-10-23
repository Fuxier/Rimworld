using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200030F RID: 783
	public class HediffComp_PregnantHuman : HediffComp
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x0008020C File Offset: 0x0007E40C
		public PregnancyAttitude? Attitude
		{
			get
			{
				return this.pregnancyAttitude;
			}
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00080214 File Offset: 0x0007E414
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.SetupPregnancyAttitude();
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00080223 File Offset: 0x0007E423
		private void SetupPregnancyAttitude()
		{
			this.pregnancyAttitude = new PregnancyAttitude?((Rand.Value < 0.5f) ? PregnancyAttitude.Positive : PregnancyAttitude.Negative);
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00080240 File Offset: 0x0007E440
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (!ModLister.CheckBiotech("Human pregnancy"))
			{
				return;
			}
			if (base.Pawn.IsHashIntervalTick(60))
			{
				if ((this.lastGivenMorningSicknessTick == -1 || GenTicks.TicksGame - this.lastGivenMorningSicknessTick >= 48000) && Rand.MTBEventOccurs(HediffComp_PregnantHuman.MorningSicknessMTBDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
				{
					base.Pawn.health.AddHediff(HediffDefOf.MorningSickness, null, null, null);
					this.lastGivenMorningSicknessTick = GenTicks.TicksGame;
				}
				if ((this.lastGivenPregnancyMoodTick == -1 || GenTicks.TicksGame - this.lastGivenPregnancyMoodTick >= 48000) && Rand.MTBEventOccurs(HediffComp_PregnantHuman.PregnancyMoodMTBDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
				{
					base.Pawn.health.AddHediff(HediffDefOf.PregnancyMood, base.Pawn.health.hediffSet.GetBrain(), null, null);
					this.lastGivenPregnancyMoodTick = GenTicks.TicksGame;
				}
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0008035C File Offset: 0x0007E55C
		public override string CompTipStringExtra
		{
			get
			{
				Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)this.parent;
				TaggedString t = "\n" + "FatherTip".Translate() + ": " + ((hediff_Pregnant.Father != null) ? hediff_Pregnant.Father.LabelShort.Colorize(ColoredText.NameColor) : "Unknown".Translate().ToString()).CapitalizeFirst();
				if (hediff_Pregnant.Mother != null && hediff_Pregnant.Mother != this.parent.pawn)
				{
					t += "\n" + "MotherTip".Translate() + ": " + hediff_Pregnant.Mother.LabelShort.CapitalizeFirst().Colorize(ColoredText.NameColor);
				}
				return t.Resolve();
			}
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0008043C File Offset: 0x0007E63C
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastGivenMorningSicknessTick, "lastGivenMorningSicknessTick", -1, false);
			Scribe_Values.Look<int>(ref this.lastGivenPregnancyMoodTick, "lastGivenPregnancyMoodTick", -1, false);
			Scribe_Values.Look<PregnancyAttitude?>(ref this.pregnancyAttitude, "pregnancyAttitude", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pregnancyAttitude == null)
			{
				this.SetupPregnancyAttitude();
			}
		}

		// Token: 0x0400112F RID: 4399
		private int lastGivenMorningSicknessTick = -1;

		// Token: 0x04001130 RID: 4400
		private int lastGivenPregnancyMoodTick = -1;

		// Token: 0x04001131 RID: 4401
		private PregnancyAttitude? pregnancyAttitude;

		// Token: 0x04001132 RID: 4402
		private const int MorningSicknessMinIntervalTicks = 48000;

		// Token: 0x04001133 RID: 4403
		private static readonly float[] MorningSicknessMTBDaysPerStage = new float[]
		{
			4f,
			8f,
			8f
		};

		// Token: 0x04001134 RID: 4404
		private const int PregnancyMoodMinIntervalTicks = 48000;

		// Token: 0x04001135 RID: 4405
		private static readonly float[] PregnancyMoodMTBDaysPerStage = new float[]
		{
			4f,
			8f,
			8f
		};
	}
}

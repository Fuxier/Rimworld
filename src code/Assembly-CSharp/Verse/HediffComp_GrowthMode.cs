using System;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020002F8 RID: 760
	public class HediffComp_GrowthMode : HediffComp_SeverityModifierBase
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x0007EF2A File Offset: 0x0007D12A
		public HediffCompProperties_GrowthMode Props
		{
			get
			{
				return (HediffCompProperties_GrowthMode)this.props;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x0007EF37 File Offset: 0x0007D137
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.growthMode.GetLabel();
			}
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x0007EF44 File Offset: 0x0007D144
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<HediffGrowthMode>(ref this.growthMode, "growthMode", HediffGrowthMode.Growing, false);
			Scribe_Values.Look<float>(ref this.severityPerDayGrowingRandomFactor, "severityPerDayGrowingRandomFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.severityPerDayRemissionRandomFactor, "severityPerDayRemissionRandomFactor", 1f, false);
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x0007EF98 File Offset: 0x0007D198
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.growthMode = ((HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))).RandomElement<HediffGrowthMode>();
			this.severityPerDayGrowingRandomFactor = this.Props.severityPerDayGrowingRandomFactor.RandomInRange;
			this.severityPerDayRemissionRandomFactor = this.Props.severityPerDayRemissionRandomFactor.RandomInRange;
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0007EFF7 File Offset: 0x0007D1F7
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(5000) && Rand.MTBEventOccurs(100f, 60000f, 5000f))
			{
				this.ChangeGrowthMode();
			}
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0007F030 File Offset: 0x0007D230
		public override float SeverityChangePerDay()
		{
			switch (this.growthMode)
			{
			case HediffGrowthMode.Growing:
			{
				float num = this.Props.severityPerDayGrowing * this.severityPerDayGrowingRandomFactor;
				if (ModsConfig.BiotechActive && this.parent.def == HediffDefOf.Carcinoma)
				{
					num *= base.Pawn.GetStatValue(StatDefOf.CancerRate, true, -1);
				}
				return num;
			}
			case HediffGrowthMode.Stable:
				return 0f;
			case HediffGrowthMode.Remission:
				return this.Props.severityPerDayRemission * this.severityPerDayRemissionRandomFactor;
			default:
				throw new NotImplementedException("GrowthMode");
			}
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0007F0C0 File Offset: 0x0007D2C0
		private void ChangeGrowthMode()
		{
			this.growthMode = (from x in (HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))
			where x != this.growthMode
			select x).RandomElement<HediffGrowthMode>();
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				switch (this.growthMode)
				{
				case HediffGrowthMode.Growing:
					Messages.Message("DiseaseGrowthModeChanged_Growing".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					return;
				case HediffGrowthMode.Stable:
					Messages.Message("DiseaseGrowthModeChanged_Stable".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NeutralEvent, true);
					return;
				case HediffGrowthMode.Remission:
					Messages.Message("DiseaseGrowthModeChanged_Remission".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0007F228 File Offset: 0x0007D428
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			stringBuilder.AppendLine("severity: " + this.parent.Severity.ToString("F3") + ((this.parent.Severity >= base.Def.maxSeverity) ? " (reached max)" : ""));
			stringBuilder.AppendLine("severityPerDayGrowingRandomFactor: " + this.severityPerDayGrowingRandomFactor.ToString("0.##"));
			stringBuilder.AppendLine("severityPerDayRemissionRandomFactor: " + this.severityPerDayRemissionRandomFactor.ToString("0.##"));
			return stringBuilder.ToString();
		}

		// Token: 0x04001101 RID: 4353
		private const int CheckGrowthModeChangeInterval = 5000;

		// Token: 0x04001102 RID: 4354
		private const float GrowthModeChangeMtbDays = 100f;

		// Token: 0x04001103 RID: 4355
		public HediffGrowthMode growthMode;

		// Token: 0x04001104 RID: 4356
		private float severityPerDayGrowingRandomFactor = 1f;

		// Token: 0x04001105 RID: 4357
		private float severityPerDayRemissionRandomFactor = 1f;
	}
}

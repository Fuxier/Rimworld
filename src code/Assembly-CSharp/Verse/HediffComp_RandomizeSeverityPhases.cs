using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000334 RID: 820
	public class HediffComp_RandomizeSeverityPhases : HediffComp_Randomizer
	{
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x00081DBF File Offset: 0x0007FFBF
		public HediffCompProperties_RandomizeSeverityPhases Props
		{
			get
			{
				return (HediffCompProperties_RandomizeSeverityPhases)this.props;
			}
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00081DCC File Offset: 0x0007FFCC
		public HediffCompProperties_RandomizeSeverityPhases.Phase CurrentOption()
		{
			int? num = this.randomPhaseIndex;
			int num2 = 0;
			if (num.GetValueOrDefault() >= num2 & num != null)
			{
				num = this.randomPhaseIndex;
				List<HediffCompProperties_RandomizeSeverityPhases.Phase> phases = this.Props.phases;
				int? num3 = (phases != null) ? new int?(phases.Count) : null;
				if (num.GetValueOrDefault() < num3.GetValueOrDefault() & (num != null & num3 != null))
				{
					return this.Props.phases[this.randomPhaseIndex.Value];
				}
			}
			return null;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00081E64 File Offset: 0x00080064
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.Randomize();
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x00081E74 File Offset: 0x00080074
		public override void Randomize()
		{
			if (this.Props.phases == null || this.Props.phases.Count == 0)
			{
				return;
			}
			int num = Rand.Range(0, this.Props.phases.Count);
			if (this.randomPhaseIndex != null)
			{
				int num2 = num;
				int? num3 = this.randomPhaseIndex;
				if (!(num2 == num3.GetValueOrDefault() & num3 != null) && !this.Props.notifyMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
				{
					Messages.Message(this.Props.notifyMessage.Formatted(this.parent.pawn.Named("PAWN"), this.Props.phases[this.randomPhaseIndex.Value].labelPrefix, this.Props.phases[num].labelPrefix), this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			}
			this.randomPhaseIndex = new int?(num);
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00081FA4 File Offset: 0x000801A4
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int?>(ref this.randomPhaseIndex, "randomPhaseIndex", null, false);
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00081FD1 File Offset: 0x000801D1
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			float num = severityAdjustment;
			HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
			severityAdjustment = num + ((phase != null) ? phase.severityPerDay : 0f) / 60000f;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x00081FFC File Offset: 0x000801FC
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (!base.Pawn.Dead)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "severity/day: ";
				HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
				stringBuilder2.AppendLine(str + ((phase != null) ? phase.severityPerDay : 0f).ToString("F3"));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x00082068 File Offset: 0x00080268
		public override string CompLabelPrefix
		{
			get
			{
				HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
				if (phase == null)
				{
					return null;
				}
				return phase.labelPrefix;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x0008207B File Offset: 0x0008027B
		public override string CompDescriptionExtra
		{
			get
			{
				HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
				if (phase == null)
				{
					return null;
				}
				return phase.descriptionExtra;
			}
		}

		// Token: 0x04001174 RID: 4468
		private int? randomPhaseIndex;
	}
}

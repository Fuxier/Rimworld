using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020002EC RID: 748
	public class HediffComp_GiveHediffLungRot : HediffComp
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0007E8A2 File Offset: 0x0007CAA2
		private HediffCompProperties_GiveHediffLungRot Props
		{
			get
			{
				return (HediffCompProperties_GiveHediffLungRot)this.props;
			}
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x000034B7 File Offset: 0x000016B7
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00002662 File Offset: 0x00000862
		public override bool CompDisallowVisible()
		{
			return true;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0007E8B0 File Offset: 0x0007CAB0
		public override void CompPostTick(ref float severityAdjustment)
		{
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned && this.parent.Severity >= this.Props.minSeverity && pawn.Position.AnyGas(pawn.Map, GasType.RotStink) && !pawn.health.hediffSet.HasHediff(HediffDefOf.LungRot, false) && pawn.IsHashIntervalTick(this.Props.mtbCheckDuration) && !pawn.health.immunity.AnyGeneMakesFullyImmuneTo(HediffDefOf.LungRot) && Rand.MTBEventOccurs(this.Props.mtbOverRotGasExposureCurve.Evaluate(this.parent.Severity), 2500f, (float)this.Props.mtbCheckDuration))
			{
				IEnumerable<BodyPartRecord> lungRotAffectedBodyParts = GasUtility.GetLungRotAffectedBodyParts(pawn);
				if (lungRotAffectedBodyParts.Any<BodyPartRecord>())
				{
					foreach (BodyPartRecord part in lungRotAffectedBodyParts)
					{
						pawn.health.AddHediff(HediffDefOf.LungRot, part, null, null);
					}
					if (PawnUtility.ShouldSendNotificationAbout(pawn))
					{
						TaggedString label = "LetterLabelNewDisease".Translate() + ": " + HediffDefOf.LungRot.LabelCap;
						TaggedString text = "LetterTextLungRot".Translate(pawn.Named("PAWN"), HediffDefOf.LungRot.label, pawn.LabelDefinite()).AdjustedFor(pawn, "PAWN", true).CapitalizeFirst();
						Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, pawn, null, null, null, null);
					}
				}
			}
		}
	}
}

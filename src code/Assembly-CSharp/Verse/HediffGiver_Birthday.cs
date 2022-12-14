using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000354 RID: 852
	public class HediffGiver_Birthday : HediffGiver
	{
		// Token: 0x060016D5 RID: 5845 RVA: 0x00085F64 File Offset: 0x00084164
		public void TryApplyAndSimulateSeverityChange(Pawn pawn, float gotAtAge, bool tryNotToKillPawn)
		{
			HediffGiver_Birthday.addedHediffs.Clear();
			if (!base.TryApply(pawn, HediffGiver_Birthday.addedHediffs))
			{
				return;
			}
			if (this.averageSeverityPerDayBeforeGeneration != 0f)
			{
				float num = (pawn.ageTracker.AgeBiologicalYearsFloat - gotAtAge) * 60f;
				if (num < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"daysPassed < 0, pawn=",
						pawn,
						", gotAtAge=",
						gotAtAge
					}));
					return;
				}
				for (int i = 0; i < HediffGiver_Birthday.addedHediffs.Count; i++)
				{
					this.SimulateSeverityChange(pawn, HediffGiver_Birthday.addedHediffs[i], num, tryNotToKillPawn);
				}
			}
			HediffGiver_Birthday.addedHediffs.Clear();
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00086014 File Offset: 0x00084214
		private void SimulateSeverityChange(Pawn pawn, Hediff hediff, float daysPassed, bool tryNotToKillPawn)
		{
			float num = this.averageSeverityPerDayBeforeGeneration * daysPassed;
			num *= Rand.Range(0.5f, 1.4f);
			num += hediff.def.initialSeverity;
			if (tryNotToKillPawn)
			{
				this.AvoidLifeThreateningStages(ref num, hediff.def.stages);
			}
			hediff.Severity = num;
			pawn.health.Notify_HediffChanged(hediff);
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00086074 File Offset: 0x00084274
		private void AvoidLifeThreateningStages(ref float severity, List<HediffStage> stages)
		{
			if (stages.NullOrEmpty<HediffStage>())
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < stages.Count; i++)
			{
				if (stages[i].lifeThreatening)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				if (num == 0)
				{
					severity = Mathf.Min(severity, stages[num].minSeverity);
					return;
				}
				severity = Mathf.Min(severity, (stages[num].minSeverity + stages[num - 1].minSeverity) / 2f);
			}
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x000860F8 File Offset: 0x000842F8
		public float DebugChanceToHaveAtAge(Pawn pawn, int age)
		{
			float num = 1f;
			for (int i = 1; i <= age; i++)
			{
				float x = (float)i / pawn.RaceProps.lifeExpectancy;
				num *= 1f - this.ageFractionChanceCurve.Evaluate(x);
			}
			return 1f - num;
		}

		// Token: 0x040011AC RID: 4524
		public SimpleCurve ageFractionChanceCurve;

		// Token: 0x040011AD RID: 4525
		public float averageSeverityPerDayBeforeGeneration;

		// Token: 0x040011AE RID: 4526
		private static List<Hediff> addedHediffs = new List<Hediff>();
	}
}

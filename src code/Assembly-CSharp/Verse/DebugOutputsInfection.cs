using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000448 RID: 1096
	public static class DebugOutputsInfection
	{
		// Token: 0x06002187 RID: 8583 RVA: 0x000D0318 File Offset: 0x000CE518
		private static List<Pawn> GenerateDoctorArray()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, (Pawn p) => !p.WorkTypeIsDisabled(WorkTypeDefOf.Doctor) && p.health.hediffSet.hediffs.Count == 0, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i <= 20; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				pawn.skills.GetSkill(SkillDefOf.Medicine).Level = i;
				list.Add(pawn);
			}
			return list;
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x000D040A File Offset: 0x000CE60A
		private static IEnumerable<HediffDef> InfectionList()
		{
			return from hediff in DefDatabase<HediffDef>.AllDefs
			where hediff.tendable && hediff.HasComp(typeof(HediffComp_TendDuration)) && hediff.HasComp(typeof(HediffComp_Immunizable)) && hediff.lethalSeverity > 0f
			select hediff;
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x000D0438 File Offset: 0x000CE638
		[DebugOutput]
		public static void Infections()
		{
			DebugOutputsInfection.<>c__DisplayClass3_0 CS$<>8__locals1 = new DebugOutputsInfection.<>c__DisplayClass3_0();
			CS$<>8__locals1.ilc = delegate(DebugOutputsInfection.InfectionLuck il)
			{
				float result = 1f;
				if (il == DebugOutputsInfection.InfectionLuck.Bad)
				{
					result = 0.8f;
				}
				if (il == DebugOutputsInfection.InfectionLuck.Good)
				{
					result = 1.2f;
				}
				return result;
			};
			CS$<>8__locals1.stringizeWithLuck = ((Func<DebugOutputsInfection.InfectionLuck, float> func) => string.Format("{0:F2} / {1:F2}", func(DebugOutputsInfection.InfectionLuck.Bad), func(DebugOutputsInfection.InfectionLuck.Good)));
			CS$<>8__locals1.baseImmunityIncrease = ((HediffDef d, DebugOutputsInfection.InfectionLuck il) => d.CompProps<HediffCompProperties_Immunizable>().immunityPerDaySick * CS$<>8__locals1.ilc(il));
			CS$<>8__locals1.tendedSeverityIncrease = ((HediffDef d, float tend) => DebugOutputsInfection.<Infections>g__baseSeverityIncrease|3_3(d) + d.CompProps<HediffCompProperties_TendDuration>().severityPerDayTended * tend);
			CS$<>8__locals1.immunityIncrease = delegate(HediffDef d, DebugOutputsInfection.InfectionLuck il, bool bedridden)
			{
				float b = DebugOutputsInfection.<Infections>g__isAnimal|3_2(d) ? 1f : ThingDefOf.Bed.GetStatValueAbstract(StatDefOf.ImmunityGainSpeedFactor, null);
				float num = Mathf.Lerp(1f, b, bedridden ? 1f : 0.3f) * StatDefOf.ImmunityGainSpeed.GetStatPart<StatPart_Resting>().factor;
				return CS$<>8__locals1.baseImmunityIncrease(d, il) * num;
			};
			CS$<>8__locals1.immunityOnLethality = delegate(HediffDef d, DebugOutputsInfection.InfectionLuck il, float tend)
			{
				if (CS$<>8__locals1.tendedSeverityIncrease(d, tend) <= 0f)
				{
					return float.PositiveInfinity;
				}
				return d.lethalSeverity / CS$<>8__locals1.tendedSeverityIncrease(d, tend) * CS$<>8__locals1.immunityIncrease(d, il, true);
			};
			List<TableDataGetter<HediffDef>> list = new List<TableDataGetter<HediffDef>>();
			list.Add(new TableDataGetter<HediffDef>("defName", (HediffDef d) => d.defName + (d.stages.Any((HediffStage stage) => stage.capMods.Any((PawnCapacityModifier cap) => cap.capacity == PawnCapacityDefOf.BloodFiltration)) ? " (inaccurate)" : "")));
			list.Add(new TableDataGetter<HediffDef>("lethal\nseverity", (HediffDef d) => d.lethalSeverity.ToString("F2")));
			list.Add(new TableDataGetter<HediffDef>("base\nseverity\nincrease", (HediffDef d) => DebugOutputsInfection.<Infections>g__baseSeverityIncrease|3_3(d).ToString("F2")));
			list.Add(new TableDataGetter<HediffDef>("base\nimmunity\nincrease", (HediffDef d) => CS$<>8__locals1.stringizeWithLuck((DebugOutputsInfection.InfectionLuck il) => CS$<>8__locals1.baseImmunityIncrease(d, il))));
			List<Pawn> source = DebugOutputsInfection.GenerateDoctorArray();
			float tendquality;
			for (tendquality = 0f; tendquality <= 1.01f; tendquality += 0.1f)
			{
				tendquality = Mathf.Clamp01(tendquality);
				Pawn arg = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg2 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg3 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg4 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg5 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= tendquality, null);
				Pawn arg6 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= tendquality, null);
				Pawn arg7 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= tendquality, null);
				Pawn arg8 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= tendquality, null);
				Pawn arg9 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Pawn arg10 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Pawn arg11 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Pawn arg12 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Func<Pawn, Pawn, Pawn, string> func2 = delegate(Pawn low, Pawn exp, Pawn high)
				{
					string arg13 = (low != null) ? low.skills.GetSkill(SkillDefOf.Medicine).Level.ToString() : "X";
					string arg14 = (exp != null) ? exp.skills.GetSkill(SkillDefOf.Medicine).Level.ToString() : "X";
					string arg15 = (high != null) ? high.skills.GetSkill(SkillDefOf.Medicine).Level.ToString() : "X";
					return string.Format("{0}-{1}-{2}", arg13, arg14, arg15);
				};
				string text = func2(arg, arg5, arg9);
				string text2 = func2(arg2, arg6, arg10);
				string text3 = func2(arg3, arg7, arg11);
				string text4 = func2(arg4, arg8, arg12);
				float tq = tendquality;
				list.Add(new TableDataGetter<HediffDef>(string.Format("survival chance at\ntend quality {0}\n\ndoc skill needed:\nno meds:  {1}\nherbal:  {2}\nnormal:  {3}\nglitter:  {4}", new object[]
				{
					tq.ToStringPercent(),
					text,
					text2,
					text3,
					text4
				}), delegate(HediffDef d)
				{
					float num = CS$<>8__locals1.immunityOnLethality(d, DebugOutputsInfection.InfectionLuck.Bad, tq);
					float num2 = CS$<>8__locals1.immunityOnLethality(d, DebugOutputsInfection.InfectionLuck.Good, tq);
					if (num == float.PositiveInfinity)
					{
						return float.PositiveInfinity.ToString();
					}
					return Mathf.Clamp01((num2 - 1f) / (num2 - num)).ToStringPercent();
				}));
			}
			DebugTables.MakeTablesDialog<HediffDef>(DebugOutputsInfection.InfectionList(), list.ToArray());
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x000D0818 File Offset: 0x000CEA18
		[DebugOutput]
		public static void InfectionSimulator()
		{
			LongEventHandler.QueueLongEvent(DebugOutputsInfection.InfectionSimulatorWorker(), "Simulating . . .", null, true);
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000D082B File Offset: 0x000CEA2B
		private static IEnumerable InfectionSimulatorWorker()
		{
			int trials = 2;
			List<Pawn> doctors = DebugOutputsInfection.GenerateDoctorArray();
			List<int> testSkill = new List<int>
			{
				4,
				10,
				16
			};
			List<ThingDef> testMedicine = new List<ThingDef>
			{
				null,
				ThingDefOf.MedicineHerbal,
				ThingDefOf.MedicineIndustrial,
				ThingDefOf.MedicineUltratech
			};
			PawnGenerationRequest pawngen = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false);
			int originalTicks = Find.TickManager.TicksGame;
			List<DebugOutputsInfection.InfectionSimRow> results = new List<DebugOutputsInfection.InfectionSimRow>();
			int totalTests = DebugOutputsInfection.InfectionList().Count<HediffDef>() * testMedicine.Count<ThingDef>() * testSkill.Count<int>() * trials;
			int currentTest = 0;
			foreach (HediffDef hediff in DebugOutputsInfection.InfectionList())
			{
				foreach (ThingDef meds in testMedicine)
				{
					foreach (int num in testSkill)
					{
						DebugOutputsInfection.InfectionSimRow result = default(DebugOutputsInfection.InfectionSimRow);
						result.illness = hediff;
						result.skill = num;
						result.medicine = meds;
						Pawn doctor = doctors[num];
						int num2;
						for (int i = 0; i < trials; i = num2)
						{
							Pawn patient = PawnGenerator.GeneratePawn(pawngen);
							int startTicks = Find.TickManager.TicksGame;
							patient.health.AddHediff(result.illness, null, null, null);
							Hediff activeHediff = patient.health.hediffSet.GetFirstHediffOfDef(result.illness, false);
							while (!patient.Dead && patient.health.hediffSet.HasHediff(result.illness, false))
							{
								float maxQuality = (meds != null) ? meds.GetStatValueAbstract(StatDefOf.MedicalQualityMax, null) : 0.7f;
								if (activeHediff.TendableNow(false))
								{
									activeHediff.Tended(TendUtility.CalculateBaseTendQuality(doctor, patient, meds), maxQuality, 0);
									result.medicineUsed += 1f;
								}
								foreach (Hediff hediff2 in patient.health.hediffSet.GetHediffsTendable())
								{
									hediff2.Tended(TendUtility.CalculateBaseTendQuality(doctor, patient, meds), maxQuality, 0);
								}
								Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
								patient.health.HealthTick();
								if (Find.TickManager.TicksGame % 900 == 0)
								{
									yield return null;
								}
							}
							if (patient.Dead)
							{
								result.deathChance += 1f;
							}
							else
							{
								result.recoveryTimeDays += (Find.TickManager.TicksGame - startTicks).TicksToDays();
							}
							num2 = currentTest + 1;
							currentTest = num2;
							LongEventHandler.SetCurrentEventText(string.Format("Simulating ({0}/{1})", currentTest, totalTests));
							yield return null;
							patient = null;
							activeHediff = null;
							num2 = i + 1;
						}
						result.recoveryTimeDays /= (float)trials - result.deathChance;
						result.deathChance /= (float)trials;
						result.medicineUsed /= (float)trials;
						results.Add(result);
						result = default(DebugOutputsInfection.InfectionSimRow);
						doctor = null;
					}
					List<int>.Enumerator enumerator3 = default(List<int>.Enumerator);
					meds = null;
				}
				List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
				hediff = null;
			}
			IEnumerator<HediffDef> enumerator = null;
			IEnumerable<DebugOutputsInfection.InfectionSimRow> dataSources = results;
			TableDataGetter<DebugOutputsInfection.InfectionSimRow>[] array = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>[6];
			array[0] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("defName", (DebugOutputsInfection.InfectionSimRow isr) => isr.illness.defName);
			array[1] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("meds", delegate(DebugOutputsInfection.InfectionSimRow isr)
			{
				if (isr.medicine == null)
				{
					return "(none)";
				}
				return isr.medicine.defName;
			});
			array[2] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("skill", (DebugOutputsInfection.InfectionSimRow isr) => isr.skill.ToString());
			array[3] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("death chance", (DebugOutputsInfection.InfectionSimRow isr) => isr.deathChance.ToStringPercent());
			array[4] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("recovery time (days)", (DebugOutputsInfection.InfectionSimRow isr) => isr.recoveryTimeDays.ToString("F1"));
			array[5] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("medicine used", (DebugOutputsInfection.InfectionSimRow isr) => isr.medicineUsed.ToString());
			DebugTables.MakeTablesDialog<DebugOutputsInfection.InfectionSimRow>(dataSources, array);
			Find.TickManager.DebugSetTicksGame(originalTicks);
			yield break;
			yield break;
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000D0834 File Offset: 0x000CEA34
		[CompilerGenerated]
		internal static bool <Infections>g__isAnimal|3_2(HediffDef d)
		{
			return d.defName.Contains("Animal");
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000D0846 File Offset: 0x000CEA46
		[CompilerGenerated]
		internal static float <Infections>g__baseSeverityIncrease|3_3(HediffDef d)
		{
			return d.CompProps<HediffCompProperties_Immunizable>().severityPerDayNotImmune;
		}

		// Token: 0x02002008 RID: 8200
		private enum InfectionLuck
		{
			// Token: 0x04007E4C RID: 32332
			Bad,
			// Token: 0x04007E4D RID: 32333
			Normal,
			// Token: 0x04007E4E RID: 32334
			Good
		}

		// Token: 0x02002009 RID: 8201
		private struct InfectionSimRow
		{
			// Token: 0x04007E4F RID: 32335
			public HediffDef illness;

			// Token: 0x04007E50 RID: 32336
			public int skill;

			// Token: 0x04007E51 RID: 32337
			public ThingDef medicine;

			// Token: 0x04007E52 RID: 32338
			public float deathChance;

			// Token: 0x04007E53 RID: 32339
			public float recoveryTimeDays;

			// Token: 0x04007E54 RID: 32340
			public float medicineUsed;
		}
	}
}

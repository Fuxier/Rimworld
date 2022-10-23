using System;

namespace Verse
{
	// Token: 0x020002C1 RID: 705
	public class HealthTuning
	{
		// Token: 0x04001056 RID: 4182
		public const int StandardInterval = 60;

		// Token: 0x04001057 RID: 4183
		public const float SmallPawnFragmentedDamageHealthScaleThreshold = 0.5f;

		// Token: 0x04001058 RID: 4184
		public const int SmallPawnFragmentedDamageMinimumDamageAmount = 10;

		// Token: 0x04001059 RID: 4185
		public static float ChanceToAdditionallyDamageInnerSolidPart = 0.2f;

		// Token: 0x0400105A RID: 4186
		public const float MinBleedingRateToBleed = 0.1f;

		// Token: 0x0400105B RID: 4187
		public const float BleedSeverityRecoveryPerInterval = 0.00033333333f;

		// Token: 0x0400105C RID: 4188
		public const float BloodFilthDropChanceFactorStanding = 0.004f;

		// Token: 0x0400105D RID: 4189
		public const float BloodFilthDropChanceFactorLaying = 0.0004f;

		// Token: 0x0400105E RID: 4190
		public const int BaseTicksAfterInjuryToStopBleeding = 90000;

		// Token: 0x0400105F RID: 4191
		public const int TicksAfterMissingBodyPartToStopBeingFresh = 90000;

		// Token: 0x04001060 RID: 4192
		public const float DefaultPainShockThreshold = 0.8f;

		// Token: 0x04001061 RID: 4193
		public const int InjuryHealInterval = 600;

		// Token: 0x04001062 RID: 4194
		public const float InjuryHealPerDay_Base = 8f;

		// Token: 0x04001063 RID: 4195
		public const float InjuryHealPerDayOffset_Laying = 4f;

		// Token: 0x04001064 RID: 4196
		public const float InjuryHealPerDayOffset_Tended = 8f;

		// Token: 0x04001065 RID: 4197
		public const int InjurySeverityTendedPerMedicine = 20;

		// Token: 0x04001066 RID: 4198
		public const float BaseTotalDamageLethalThreshold = 150f;

		// Token: 0x04001067 RID: 4199
		public const float BecomePermanentBaseChance = 0.02f;

		// Token: 0x04001068 RID: 4200
		public static readonly SimpleCurve BecomePermanentChanceFactorBySeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(4f, 0f),
				true
			},
			{
				new CurvePoint(14f, 1f),
				true
			}
		};

		// Token: 0x04001069 RID: 4201
		public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[]
		{
			new HealthTuning.PainCategoryWeighted(PainCategory.Painless, 0.5f),
			new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.HighPain, 0.1f)
		};

		// Token: 0x0400106A RID: 4202
		public const float MinDamagePartPctForInfection = 0.2f;

		// Token: 0x0400106B RID: 4203
		public static readonly IntRange InfectionDelayRange = new IntRange(15000, 45000);

		// Token: 0x0400106C RID: 4204
		public const float AnimalsInfectionChanceFactor = 0.1f;

		// Token: 0x0400106D RID: 4205
		public const float HypothermiaGrowthPerDegreeUnder = 6.45E-05f;

		// Token: 0x0400106E RID: 4206
		public const float HeatstrokeGrowthPerDegreeOver = 6.45E-05f;

		// Token: 0x0400106F RID: 4207
		public const float MinHeatstrokeProgressPerInterval = 0.000375f;

		// Token: 0x04001070 RID: 4208
		public const float MinHypothermiaProgress = 0.00075f;

		// Token: 0x04001071 RID: 4209
		public const float HarmfulTemperatureOffset = 10f;

		// Token: 0x04001072 RID: 4210
		public const float MinTempOverComfyMaxForBurn = 150f;

		// Token: 0x04001073 RID: 4211
		public const float BurnDamagePerTempOverage = 0.06f;

		// Token: 0x04001074 RID: 4212
		public const int MinBurnDamage = 3;

		// Token: 0x04001075 RID: 4213
		public const float ImmunityGainRandomFactorMin = 0.8f;

		// Token: 0x04001076 RID: 4214
		public const float ImmunityGainRandomFactorMax = 1.2f;

		// Token: 0x04001077 RID: 4215
		public const float ImpossibleToFallSickIfAboveThisImmunityLevel = 0.6f;

		// Token: 0x04001078 RID: 4216
		public const int HediffGiverUpdateInterval = 60;

		// Token: 0x04001079 RID: 4217
		public const int VomitCheckInterval = 600;

		// Token: 0x0400107A RID: 4218
		public const int DeathCheckInterval = 200;

		// Token: 0x0400107B RID: 4219
		public const int ForgetRandomMemoryThoughtCheckInterval = 400;

		// Token: 0x0400107C RID: 4220
		public const float PawnBaseHealthForSummary = 75f;

		// Token: 0x0400107D RID: 4221
		public const float DeathOnDownedChance_NonColonyAnimal = 0.5f;

		// Token: 0x0400107E RID: 4222
		public const float DeathOnDownedChance_NonColonyMechanoid = 1f;

		// Token: 0x0400107F RID: 4223
		public static readonly SimpleCurve DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.8667f),
				true
			},
			{
				new CurvePoint(0f, 0.8f),
				true
			},
			{
				new CurvePoint(1f, 0.5657f),
				true
			},
			{
				new CurvePoint(2f, 0.5f),
				true
			},
			{
				new CurvePoint(8f, 0.2632f),
				true
			}
		};

		// Token: 0x04001080 RID: 4224
		public static readonly SimpleCurve DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve_WaveringPrisoners = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.92f),
				true
			},
			{
				new CurvePoint(0f, 0.85f),
				true
			},
			{
				new CurvePoint(1f, 0.62f),
				true
			},
			{
				new CurvePoint(2f, 0.55f),
				true
			},
			{
				new CurvePoint(8f, 0.3f),
				true
			}
		};

		// Token: 0x04001081 RID: 4225
		public const int MaxPopulationAlwaysRecruitable = 3;

		// Token: 0x04001082 RID: 4226
		public static readonly SimpleCurve NonRecruitableChanceOverPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.4f),
				true
			},
			{
				new CurvePoint(0f, 0.25f),
				true
			},
			{
				new CurvePoint(1f, 0.13f),
				true
			},
			{
				new CurvePoint(2f, 0.1f),
				true
			},
			{
				new CurvePoint(8f, 0.05f),
				true
			}
		};

		// Token: 0x04001083 RID: 4227
		public const float TendPriority_LifeThreateningDisease = 1f;

		// Token: 0x04001084 RID: 4228
		public const float TendPriority_PerBleedRate = 1.5f;

		// Token: 0x04001085 RID: 4229
		public const float TendPriority_DiseaseSeverityDecreasesWhenTended = 0.025f;

		// Token: 0x02001DF2 RID: 7666
		public struct PainCategoryWeighted
		{
			// Token: 0x0600B6B7 RID: 46775 RVA: 0x0041730C File Offset: 0x0041550C
			public PainCategoryWeighted(PainCategory category, float weight)
			{
				this.category = category;
				this.weight = weight;
			}

			// Token: 0x0400762B RID: 30251
			public PainCategory category;

			// Token: 0x0400762C RID: 30252
			public float weight;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200036F RID: 879
	public class Pawn_AgeTracker : IExposable
	{
		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x00095773 File Offset: 0x00093973
		// (set) Token: 0x060018DC RID: 6364 RVA: 0x0009577B File Offset: 0x0009397B
		public long BirthAbsTicks
		{
			get
			{
				return this.birthAbsTicksInt;
			}
			set
			{
				this.birthAbsTicksInt = value;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x060018DD RID: 6365 RVA: 0x00095784 File Offset: 0x00093984
		public int AgeBiologicalYears
		{
			get
			{
				return (int)(this.ageBiologicalTicksInt / 3600000L);
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x060018DE RID: 6366 RVA: 0x00095794 File Offset: 0x00093994
		public float AgeBiologicalYearsFloat
		{
			get
			{
				return (float)this.ageBiologicalTicksInt / 3600000f;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x000957A3 File Offset: 0x000939A3
		// (set) Token: 0x060018E0 RID: 6368 RVA: 0x000957AB File Offset: 0x000939AB
		public long AgeBiologicalTicks
		{
			get
			{
				return this.ageBiologicalTicksInt;
			}
			set
			{
				this.ageBiologicalTicksInt = value;
				this.CalculateInitialGrowth();
				this.RecalculateLifeStageIndex();
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060018E1 RID: 6369 RVA: 0x000957C0 File Offset: 0x000939C0
		// (set) Token: 0x060018E2 RID: 6370 RVA: 0x000957CF File Offset: 0x000939CF
		public long AgeChronologicalTicks
		{
			get
			{
				return (long)GenTicks.TicksAbs - this.birthAbsTicksInt;
			}
			set
			{
				this.BirthAbsTicks = (long)GenTicks.TicksAbs - value;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x000957DF File Offset: 0x000939DF
		public int AgeChronologicalYears
		{
			get
			{
				return (int)(this.AgeChronologicalTicks / 3600000L);
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060018E4 RID: 6372 RVA: 0x000957EF File Offset: 0x000939EF
		public float AgeChronologicalYearsFloat
		{
			get
			{
				return (float)this.AgeChronologicalTicks / 3600000f;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x000957FE File Offset: 0x000939FE
		public int BirthYear
		{
			get
			{
				return GenDate.Year(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x00095810 File Offset: 0x00093A10
		public int BirthDayOfSeasonZeroBased
		{
			get
			{
				return GenDate.DayOfSeason(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x00095822 File Offset: 0x00093A22
		public int BirthDayOfYear
		{
			get
			{
				return GenDate.DayOfYear(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060018E8 RID: 6376 RVA: 0x00095834 File Offset: 0x00093A34
		public Quadrum BirthQuadrum
		{
			get
			{
				return GenDate.Quadrum(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060018E9 RID: 6377 RVA: 0x00095848 File Offset: 0x00093A48
		public string AgeNumberString
		{
			get
			{
				string text = this.AgeBiologicalYearsFloat.ToStringApproxAge();
				if (this.AgeChronologicalYears != this.AgeBiologicalYears)
				{
					text = string.Concat(new object[]
					{
						text,
						" (",
						this.AgeChronologicalYears,
						")"
					});
				}
				return text;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060018EA RID: 6378 RVA: 0x000958A0 File Offset: 0x00093AA0
		public string AgeTooltipString
		{
			get
			{
				int value;
				int value2;
				int value3;
				float num;
				this.ageBiologicalTicksInt.TicksToPeriod(out value, out value2, out value3, out num);
				int value4;
				int value5;
				int value6;
				((long)GenTicks.TicksAbs - this.birthAbsTicksInt).TicksToPeriod(out value4, out value5, out value6, out num);
				string value7 = "FullDate".Translate(Find.ActiveLanguageWorker.OrdinalNumber(this.BirthDayOfSeasonZeroBased + 1, Gender.None), this.BirthQuadrum.Label(), this.BirthYear);
				string text = "Born".Translate(value7) + "\n" + "AgeChronological".Translate(value4, value5, value6) + "\n" + "AgeBiological".Translate(value, value2, value3);
				if (Prefs.DevMode)
				{
					text += "\n\nDev mode info:";
					text = text + "\nageBiologicalTicksInt: " + this.ageBiologicalTicksInt;
					text = text + "\nbirthAbsTicksInt: " + this.birthAbsTicksInt;
					text = text + "\nBiologicalTicksPerTick: " + this.BiologicalTicksPerTick;
					text = text + "\ngrowth: " + this.growth;
					text = string.Concat(new object[]
					{
						text,
						"\nage reversal demand deadline: ",
						((int)Math.Abs(this.AgeReversalDemandedDeadlineTicks)).ToStringTicksToPeriod(true, false, true, true, false),
						(this.AgeReversalDemandedDeadlineTicks < 0L) ? " past deadline" : " in future",
						"(",
						this.AgeReversalDemandedDeadlineTicks,
						")"
					});
					text = text + "\nlife stage: " + this.CurLifeStage;
					text = text + "\nsterile: " + this.pawn.Sterile(false).ToString();
				}
				return text;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x00095AAD File Offset: 0x00093CAD
		public int CurLifeStageIndex
		{
			get
			{
				if (this.cachedLifeStageIndex < 0)
				{
					this.RecalculateLifeStageIndex();
				}
				return this.cachedLifeStageIndex;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060018EC RID: 6380 RVA: 0x00095AC4 File Offset: 0x00093CC4
		public LifeStageDef CurLifeStage
		{
			get
			{
				return this.CurLifeStageRace.def;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060018ED RID: 6381 RVA: 0x00095AD1 File Offset: 0x00093CD1
		public LifeStageAge CurLifeStageRace
		{
			get
			{
				return this.GetLifeStageAge(this.CurLifeStageIndex);
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060018EE RID: 6382 RVA: 0x00095AE0 File Offset: 0x00093CE0
		public PawnKindLifeStage CurKindLifeStage
		{
			get
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					Log.ErrorOnce("Tried to get CurKindLifeStage from humanlike pawn " + this.pawn, 8888811);
					return null;
				}
				return this.pawn.kindDef.lifeStages[this.CurLifeStageIndex];
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x00095B36 File Offset: 0x00093D36
		public int MaxRaceLifeStageIndex
		{
			get
			{
				return this.pawn.RaceProps.lifeStageAges.Count - 1;
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060018F0 RID: 6384 RVA: 0x00095B4F File Offset: 0x00093D4F
		public float Growth
		{
			get
			{
				return this.growth;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x00095B58 File Offset: 0x00093D58
		public float AdultMinAge
		{
			get
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					LifeStageAge lifeStageAge2 = this.pawn.RaceProps.lifeStageAges.FirstOrDefault((LifeStageAge lifeStageAge) => lifeStageAge.def.developmentalStage.Adult());
					if (lifeStageAge2 == null)
					{
						return 0f;
					}
					return lifeStageAge2.minAge;
				}
				else
				{
					if (this.pawn.RaceProps.lifeStageAges.Count <= 0)
					{
						return 0f;
					}
					return this.pawn.RaceProps.lifeStageAges.Last<LifeStageAge>().minAge;
				}
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060018F2 RID: 6386 RVA: 0x00095BF3 File Offset: 0x00093DF3
		public long AdultMinAgeTicks
		{
			get
			{
				return (long)Mathf.FloorToInt(this.AdultMinAge * 3600000f);
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x00095C07 File Offset: 0x00093E07
		private long TicksToAdulthood
		{
			get
			{
				return this.AdultMinAgeTicks - this.AgeBiologicalTicks;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060018F4 RID: 6388 RVA: 0x00095C16 File Offset: 0x00093E16
		public bool Adult
		{
			get
			{
				return this.TicksToAdulthood <= 0L;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x00095C25 File Offset: 0x00093E25
		public long AgeReversalDemandedDeadlineTicks
		{
			get
			{
				return this.ageReversalDemandedAtAgeTicks - this.AgeBiologicalTicks;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060018F6 RID: 6390 RVA: 0x00095C34 File Offset: 0x00093E34
		public Pawn_AgeTracker.AgeReversalReason LastAgeReversalReason
		{
			get
			{
				return this.lastAgeReversalReason;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x00095C3C File Offset: 0x00093E3C
		public float BiologicalTicksPerTick
		{
			get
			{
				if (ModsConfig.BiotechActive && this.pawn.ParentHolder != null && this.pawn.ParentHolder is Building_GrowthVat)
				{
					return 1f;
				}
				float num = 1f;
				if (this.pawn.DevelopmentalStage.Adult())
				{
					num *= this.AdultAgingMultiplier;
				}
				else
				{
					num *= this.ChildAgingMultiplier;
				}
				if (this.pawn.genes != null)
				{
					num *= this.pawn.genes.BiologicalAgeTickFactor;
				}
				return num;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060018F8 RID: 6392 RVA: 0x00095CC2 File Offset: 0x00093EC2
		public float AdultAgingMultiplier
		{
			get
			{
				return Find.Storyteller.difficulty.adultAgingRate;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x00095CD3 File Offset: 0x00093ED3
		public float ChildAgingMultiplier
		{
			get
			{
				return Find.Storyteller.difficulty.childAgingRate;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060018FA RID: 6394 RVA: 0x00095CE4 File Offset: 0x00093EE4
		public bool AtMaxGrowthTier
		{
			get
			{
				return this.GrowthTier >= GrowthUtility.GrowthTierPointsRequirements.Length - 1;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x00095CFC File Offset: 0x00093EFC
		public float PercentToNextGrowthTier
		{
			get
			{
				if (this.growthPoints <= 0f)
				{
					return 0f;
				}
				if (this.AtMaxGrowthTier)
				{
					return 1f;
				}
				int growthTier = this.GrowthTier;
				return (this.growthPoints - GrowthUtility.GrowthTierPointsRequirements[growthTier]) / GrowthUtility.GrowthTierPointsRequirements[growthTier + 1];
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x00095D4C File Offset: 0x00093F4C
		public int GrowthTier
		{
			get
			{
				float[] growthTierPointsRequirements = GrowthUtility.GrowthTierPointsRequirements;
				for (int i = growthTierPointsRequirements.Length - 1; i >= 0; i--)
				{
					if (this.growthPoints >= growthTierPointsRequirements[i])
					{
						return i;
					}
				}
				return 0;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x00095D80 File Offset: 0x00093F80
		public float GrowthPointsPerDay
		{
			get
			{
				if (!ModsConfig.BiotechActive)
				{
					return 0f;
				}
				float result;
				if (this.pawn.Suspended || this.pawn.ParentHolder is Building_GrowthVat)
				{
					result = 2f;
				}
				else
				{
					Need_Learning learning = this.pawn.needs.learning;
					if (learning != null)
					{
						result = this.GrowthPointsPerDayAtLearningLevel(learning.CurLevel);
					}
					else
					{
						result = 0f;
					}
				}
				return result;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060018FE RID: 6398 RVA: 0x00095DEB File Offset: 0x00093FEB
		private float GrowthPointsFactor
		{
			get
			{
				if ((float)this.AgeBiologicalYears < 7f)
				{
					return 0.75f;
				}
				return 1f;
			}
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x00095E08 File Offset: 0x00094008
		public Pawn_AgeTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x00095E78 File Offset: 0x00094078
		public float LifeStageMinAge(LifeStageDef lifeStage)
		{
			foreach (LifeStageAge lifeStageAge in this.pawn.RaceProps.lifeStageAges)
			{
				if (lifeStageAge.def == lifeStage)
				{
					return lifeStageAge.minAge;
				}
			}
			Log.Error(string.Format("Life stage def {0} not found while searching for min age of {1}", lifeStage, this.pawn));
			return 0f;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x00095F00 File Offset: 0x00094100
		private float GrowthPointsPerDayAtLearningLevel(float level)
		{
			return level * this.GrowthPointsFactor * this.pawn.ageTracker.ChildAgingMultiplier;
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x00095F1C File Offset: 0x0009411C
		public void TrySimulateGrowthPoints()
		{
			if (!ModsConfig.BiotechActive || !this.pawn.RaceProps.Humanlike || this.AgeBiologicalYears >= 13)
			{
				return;
			}
			if (Pawn_AgeTracker.growthMomentAges == null)
			{
				Pawn_AgeTracker.growthMomentAges = new List<int>();
				Pawn_AgeTracker.growthMomentAges.Add(3);
				Pawn_AgeTracker.growthMomentAges.AddRange(GrowthUtility.GrowthMomentAges);
			}
			this.growthPoints = 0f;
			int ageBiologicalYears = this.AgeBiologicalYears;
			for (int i = Pawn_AgeTracker.growthMomentAges.Count - 1; i >= 0; i--)
			{
				if (ageBiologicalYears >= Pawn_AgeTracker.growthMomentAges[i])
				{
					float num = this.GrowthPointsPerDayAtLearningLevel(Rand.Range(0.2f, 0.5f)) * (((float)Pawn_AgeTracker.growthMomentAges[i] < 7f) ? 0.75f : 1f);
					int num2 = Pawn_AgeTracker.growthMomentAges[i] * 3600000;
					long num3 = this.AgeBiologicalTicks;
					while (num3 > (long)num2)
					{
						num3 -= 60000L;
						this.growthPoints += num;
					}
					return;
				}
			}
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x00096028 File Offset: 0x00094228
		public void ExposeData()
		{
			Scribe_Values.Look<long>(ref this.ageBiologicalTicksInt, "ageBiologicalTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.birthAbsTicksInt, "birthAbsTicks", 0L, false);
			Scribe_Values.Look<float>(ref this.growth, "growth", -1f, false);
			Scribe_Values.Look<float>(ref this.progressToNextBiologicalTick, "progressToNextBiologicalTick", 0f, false);
			Scribe_Values.Look<long>(ref this.nextGrowthCheckTick, "nextGrowthCheckTick", -1L, false);
			Scribe_Values.Look<long>(ref this.vatGrowTicks, "vatGrowTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.ageReversalDemandedAtAgeTicks, "ageReversalDemandedAtAgeTicks", long.MaxValue, false);
			Scribe_Values.Look<Pawn_AgeTracker.AgeReversalReason>(ref this.lastAgeReversalReason, "lastAgeReversalReason", Pawn_AgeTracker.AgeReversalReason.Initial, false);
			Scribe_Values.Look<bool>(ref this.initializedAgeReversalDemand, "initializedAgeReversalDemand", false, false);
			Scribe_Values.Look<bool>(ref this.lifeStageChange, "lifeStageChange", false, false);
			Scribe_Values.Look<float>(ref this.growthPoints, "growthPoints", -1f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.cachedLifeStageIndex = -1;
				if (this.growthPoints < 0f)
				{
					this.TrySimulateGrowthPoints();
				}
			}
			if (this.ageReversalDemandedAtAgeTicks == 9223372036854775807L)
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
			}
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x00096150 File Offset: 0x00094350
		private void TickBiologicalAge(int interval)
		{
			this.progressToNextBiologicalTick += this.BiologicalTicksPerTick * (float)interval;
			int num = Mathf.FloorToInt(this.progressToNextBiologicalTick);
			if (num > 0)
			{
				this.progressToNextBiologicalTick -= (float)num;
				this.ageBiologicalTicksInt += (long)num;
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x000961A4 File Offset: 0x000943A4
		public void AgeTick()
		{
			if (this.lifeStageChange)
			{
				this.PostResolveLifeStageChange();
			}
			int ageBiologicalYears = this.AgeBiologicalYears;
			this.TickBiologicalAge(1);
			if ((long)Find.TickManager.TicksGame >= this.nextGrowthCheckTick)
			{
				this.CalculateGrowth(240);
			}
			if (this.pawn.IsHashIntervalTick(60000))
			{
				this.CheckAgeReversalDemand();
			}
			if (ageBiologicalYears < this.AgeBiologicalYears)
			{
				this.BirthdayBiological(this.AgeBiologicalYears);
			}
			if (this.initializedAgeReversalDemand && this.pawn.MapHeld != null && this.pawn.IsHashIntervalTick(2500) && ExpectationsUtility.CurrentExpectationFor(this.pawn.MapHeld).order < ThoughtDefOf.AgeReversalDemanded.minExpectation.order)
			{
				this.ageReversalDemandedAtAgeTicks += 2500L;
			}
			if (this.pawn.IsFreeColonist && this.pawn.IsHashIntervalTick(1000))
			{
				if (this.CurLifeStage.developmentalStage == DevelopmentalStage.Baby)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Babies, OpportunityType.Important);
					return;
				}
				if (this.CurLifeStage.developmentalStage == DevelopmentalStage.Child)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Children, OpportunityType.Important);
				}
			}
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x000962C8 File Offset: 0x000944C8
		public void AgeTickMothballed(int interval)
		{
			long num = this.ageBiologicalTicksInt;
			this.TickBiologicalAge(interval);
			this.CalculateGrowth(interval);
			this.CheckAgeReversalDemand();
			for (int i = (int)(num / 3600000L) + 1; i <= this.AgeBiologicalYears; i++)
			{
				this.BirthdayBiological(i);
			}
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x00096310 File Offset: 0x00094510
		public void Notify_TickedInGrowthVat(int ticks)
		{
			this.vatGrowTicks += 1L;
			this.AgeTickMothballed(ticks);
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x00096328 File Offset: 0x00094528
		private void CheckAgeReversalDemand()
		{
			if (!ModsConfig.IdeologyActive || this.initializedAgeReversalDemand || this.pawn.Faction != Faction.OfPlayer || this.pawn.MapHeld == null)
			{
				return;
			}
			this.ResetAgeReversalDemand(this.lastAgeReversalReason, false);
			this.initializedAgeReversalDemand = true;
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x00096378 File Offset: 0x00094578
		private void CalculateInitialGrowth()
		{
			this.growth = Mathf.Clamp01(this.AgeBiologicalYearsFloat / this.pawn.RaceProps.lifeStageAges[this.pawn.RaceProps.lifeStageAges.Count - 1].minAge);
			this.nextGrowthCheckTick = (long)(Find.TickManager.TicksGame + 240);
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x000963E0 File Offset: 0x000945E0
		private void CalculateGrowth(int interval)
		{
			if (this.growth >= 1f)
			{
				this.nextGrowthCheckTick = long.MaxValue;
				return;
			}
			this.growth += PawnUtility.BodyResourceGrowthSpeed(this.pawn) * (float)interval / Mathf.Max((float)this.AdultMinAgeTicks, 1f);
			this.growth = Mathf.Min(this.growth, 1f);
			this.nextGrowthCheckTick = (long)(Find.TickManager.TicksGame + 240);
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0009646C File Offset: 0x0009466C
		private void RecalculateLifeStageIndex()
		{
			int num = -1;
			if (this.growth < 0f)
			{
				this.CalculateInitialGrowth();
			}
			float num2;
			if (this.pawn.RaceProps.Humanlike)
			{
				num2 = (float)this.AgeBiologicalYears;
			}
			else
			{
				num2 = Mathf.Lerp(0f, this.pawn.RaceProps.lifeStageAges[this.pawn.RaceProps.lifeStageAges.Count - 1].minAge, this.growth);
			}
			List<LifeStageAge> lifeStageAges = this.pawn.RaceProps.lifeStageAges;
			for (int i = lifeStageAges.Count - 1; i >= 0; i--)
			{
				if (lifeStageAges[i].minAge <= num2 + 1E-06f)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			int index = this.cachedLifeStageIndex;
			bool flag = this.cachedLifeStageIndex != num;
			this.cachedLifeStageIndex = num;
			if (flag)
			{
				this.lifeStageChange = true;
				if (!this.pawn.RaceProps.Humanlike || ModsConfig.BiotechActive)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						this.pawn.Drawer.renderer.graphics.SetAllGraphicsDirty();
						if (this.pawn.IsColonist)
						{
							PortraitsCache.SetDirty(this.pawn);
						}
					});
					this.CheckChangePawnKindName();
				}
				LifeStageWorker worker = this.CurLifeStage.Worker;
				Pawn pawn = this.pawn;
				LifeStageAge lifeStageAge = this.GetLifeStageAge(index);
				worker.Notify_LifeStageStarted(pawn, (lifeStageAge != null) ? lifeStageAge.def : null);
				if (this.pawn.SpawnedOrAnyParentSpawned)
				{
					PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
				}
			}
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x000965CC File Offset: 0x000947CC
		private void BirthdayBiological(int birthdayAge)
		{
			Pawn_AgeTracker.tmpHediffsGained.Clear();
			Pawn_AgeTracker.tmpEnabledWorkTypes.Clear();
			bool flag = (float)birthdayAge == this.AdultMinAge;
			bool flag2 = this.pawn.DevelopmentalStage.Child() && (float)birthdayAge == this.CurLifeStageRace.minAge;
			float age = (float)birthdayAge / this.pawn.GetStatValue(StatDefOf.LifespanFactor, true, -1);
			foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(this.pawn, age))
			{
				if (hediffGiver_Birthday.TryApply(this.pawn, null))
				{
					Pawn_AgeTracker.tmpHediffsGained.Add(hediffGiver_Birthday.hediff);
				}
			}
			List<LifeStageWorkSettings> lifeStageWorkSettings = this.pawn.RaceProps.lifeStageWorkSettings;
			for (int i = 0; i < lifeStageWorkSettings.Count; i++)
			{
				if (lifeStageWorkSettings[i].minAge == birthdayAge)
				{
					Pawn_AgeTracker.tmpEnabledWorkTypes.Add(lifeStageWorkSettings[i].workType);
				}
			}
			if (Pawn_AgeTracker.tmpEnabledWorkTypes.Count > 0)
			{
				this.pawn.Notify_DisabledWorkTypesChanged();
			}
			int passionChoiceCount;
			int num;
			int num2;
			this.TryChildGrowthMoment(birthdayAge, out passionChoiceCount, out num, out num2);
			bool flag3 = !flag2 && (!Pawn_AgeTracker.tmpEnabledWorkTypes.NullOrEmpty<WorkTypeDef>() || num2 > 0 || num > 0);
			bool flag4 = PawnUtility.ShouldSendNotificationAbout(this.pawn) && this.pawn.Faction == Faction.OfPlayer;
			if (this.pawn.RaceProps.Humanlike && !flag4)
			{
				if (num2 > 0)
				{
					SkillDef skillDef = ChoiceLetter_GrowthMoment.PassionOptions(this.pawn, num2).FirstOrFallback(null);
					if (skillDef != null)
					{
						SkillRecord skill = this.pawn.skills.GetSkill(skillDef);
						if (skill != null)
						{
							skill.passion = skill.passion.IncrementPassion();
						}
					}
				}
				if (num > 0)
				{
					Trait trait = PawnGenerator.GenerateTraitsFor(this.pawn, 1, null, true).FirstOrFallback(null);
					if (trait != null)
					{
						this.pawn.story.traits.GainTrait(trait, false);
					}
				}
			}
			if (this.pawn.RaceProps.Humanlike && flag4 && (Pawn_AgeTracker.tmpHediffsGained.Count > 0 || flag3))
			{
				TaggedString taggedString = "LetterBirthdayBiological".Translate(this.pawn, birthdayAge);
				if (Pawn_AgeTracker.tmpHediffsGained.Count > 0)
				{
					taggedString += "\n\n" + "BirthdayBiologicalAgeInjuries".Translate(this.pawn);
					taggedString += ":\n\n" + (from h in Pawn_AgeTracker.tmpHediffsGained
					select h.LabelCap.Resolve()).ToLineList("- ", false);
				}
				if (ModsConfig.BiotechActive && flag3 && this.pawn.Spawned && (this.pawn.DevelopmentalStage.Juvenile() || flag))
				{
					EffecterDefOf.Birthday.SpawnAttached(this.pawn, this.pawn.Map, 1f);
				}
				if (Pawn_AgeTracker.tmpHediffsGained.Count > 0)
				{
					LetterDef letterDef = LetterDefOf.NegativeEvent;
					Find.LetterStack.ReceiveLetter("LetterLabelBirthday".Translate(), taggedString, letterDef, this.pawn, null, null, null, null);
				}
				if (ModsConfig.BiotechActive && flag3)
				{
					Name name = this.pawn.Name;
					NameTriple nameTriple;
					if ((nameTriple = (this.pawn.Name as NameTriple)) != null && !nameTriple.NickSet)
					{
						Rand.PushState(Gen.HashCombine<int>(this.pawn.thingIDNumber, birthdayAge));
						try
						{
							if (Rand.Chance(0.5f))
							{
								string name2 = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard).GetName(PawnNameSlot.Nick, this.pawn.gender, true);
								this.pawn.Name = new NameTriple(nameTriple.First, name2, nameTriple.Last);
							}
						}
						finally
						{
							Rand.PopState();
						}
					}
					LetterDef letterDef = flag ? LetterDefOf.ChildToAdult : LetterDefOf.ChildBirthday;
					ChoiceLetter_GrowthMoment choiceLetter_GrowthMoment = (ChoiceLetter_GrowthMoment)LetterMaker.MakeLetter(letterDef);
					List<string> enabledWorkTypes = (from w in Pawn_AgeTracker.tmpEnabledWorkTypes
					select w.labelShort.CapitalizeFirst()).ToList<string>();
					choiceLetter_GrowthMoment.ConfigureGrowthLetter(this.pawn, passionChoiceCount, num, num2, enabledWorkTypes, name);
					choiceLetter_GrowthMoment.Label = (flag ? "LetterLabelBecameAdult".Translate(this.pawn) : "BirthdayGrowthMoment".Translate(this.pawn, name.ToStringShort.Named("PAWNNAME")));
					choiceLetter_GrowthMoment.StartTimeout(120000);
					Find.LetterStack.ReceiveLetter(choiceLetter_GrowthMoment, null);
				}
			}
			Pawn_AgeTracker.tmpEnabledWorkTypes.Clear();
			Pawn_AgeTracker.tmpHediffsGained.Clear();
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00096ACC File Offset: 0x00094CCC
		public void TryChildGrowthMoment(int birthdayAge, out int newPassionOptions, out int newTraitOptions, out int passionGainsCount)
		{
			newPassionOptions = 0;
			newTraitOptions = 0;
			passionGainsCount = 0;
			if (ModsConfig.BiotechActive && GrowthUtility.IsGrowthBirthday(birthdayAge))
			{
				int growthTier = this.GrowthTier;
				newPassionOptions = GrowthUtility.PassionChoicesPerTier[growthTier];
				passionGainsCount = GrowthUtility.PassionGainsPerTier[growthTier];
				newTraitOptions = GrowthUtility.TraitChoicesPerTier[growthTier];
			}
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00096B18 File Offset: 0x00094D18
		public void ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason reason, bool cancelInitialization = false)
		{
			int num;
			if (reason == Pawn_AgeTracker.AgeReversalReason.Recruited)
			{
				num = this.RecruitedPawnAgeReversalDemandInDays.RandomInRange;
			}
			else if (reason == Pawn_AgeTracker.AgeReversalReason.ViaTreatment)
			{
				num = 60;
			}
			else
			{
				num = this.NewPawnAgeReversalDemandInDays.RandomInRange;
			}
			long num2 = (long)(num * 60000);
			long num3 = Math.Max(this.AgeBiologicalTicks, 90000000L) + num2;
			if (reason == Pawn_AgeTracker.AgeReversalReason.Recruited && num3 < this.ageReversalDemandedAtAgeTicks)
			{
				return;
			}
			this.ageReversalDemandedAtAgeTicks = num3;
			this.lastAgeReversalReason = reason;
			if (cancelInitialization)
			{
				this.initializedAgeReversalDemand = false;
			}
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00096B98 File Offset: 0x00094D98
		public void PostResolveLifeStageChange()
		{
			this.pawn.health.CheckForStateChange(null, null);
			this.lifeStageChange = false;
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00096BC6 File Offset: 0x00094DC6
		public void DebugForceAgeReversalDemandNow()
		{
			this.ageReversalDemandedAtAgeTicks = this.AgeBiologicalTicks;
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x00096BD4 File Offset: 0x00094DD4
		public void DebugResetAgeReversalDemand()
		{
			this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00096BDE File Offset: 0x00094DDE
		public void Notify_IdeoChanged()
		{
			Ideo ideo = this.pawn.Ideo;
			if (ideo != null && ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded))
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Recruited, false);
			}
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00096C06 File Offset: 0x00094E06
		public void DebugForceBirthdayBiological()
		{
			this.BirthdayBiological(this.AgeBiologicalYears);
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x00096C14 File Offset: 0x00094E14
		public void DebugSetAge(long finalAgeTicks)
		{
			long num = finalAgeTicks - this.AgeBiologicalTicks;
			while (num > 0L)
			{
				int ageBiologicalYears = this.AgeBiologicalYears;
				long num2 = Pawn_AgeTracker.<DebugSetAge>g__NextBirthdayTick|120_0(this.AgeBiologicalTicks) - this.AgeBiologicalTicks;
				long num3 = (num2 > num) ? num : num2;
				this.AgeBiologicalTicks += num3;
				num -= num3;
				if (this.AgeBiologicalYears > ageBiologicalYears)
				{
					this.BirthdayBiological(this.AgeBiologicalYears);
				}
			}
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x00096C7B File Offset: 0x00094E7B
		public void DebugSetGrowth(float val)
		{
			this.growth = val;
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x00096C8C File Offset: 0x00094E8C
		public void CheckChangePawnKindName()
		{
			NameSingle nameSingle = this.pawn.Name as NameSingle;
			if (nameSingle == null || !nameSingle.Numerical)
			{
				return;
			}
			string kindLabel = this.pawn.KindLabel;
			if (nameSingle.NameWithoutNumber == kindLabel)
			{
				return;
			}
			int number = nameSingle.Number;
			string text = this.pawn.KindLabel + " " + number;
			if (!NameUseChecker.NameSingleIsUsed(text))
			{
				this.pawn.Name = new NameSingle(text, true);
				return;
			}
			this.pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(this.pawn, NameStyle.Numeric, null, false, null);
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x00096D29 File Offset: 0x00094F29
		private LifeStageAge GetLifeStageAge(int index)
		{
			if (index < 0 || index >= this.pawn.RaceProps.lifeStageAges.Count)
			{
				return null;
			}
			return this.pawn.RaceProps.lifeStageAges[index];
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x00096DAF File Offset: 0x00094FAF
		[CompilerGenerated]
		internal static long <DebugSetAge>g__NextBirthdayTick|120_0(long ageTicks)
		{
			return (ageTicks / 3600000L + 1L) * 3600000L;
		}

		// Token: 0x04001279 RID: 4729
		private Pawn pawn;

		// Token: 0x0400127A RID: 4730
		private long ageBiologicalTicksInt = -1L;

		// Token: 0x0400127B RID: 4731
		private long birthAbsTicksInt = -1L;

		// Token: 0x0400127C RID: 4732
		private long nextGrowthCheckTick = -1L;

		// Token: 0x0400127D RID: 4733
		private float growth = -1f;

		// Token: 0x0400127E RID: 4734
		private float progressToNextBiologicalTick;

		// Token: 0x0400127F RID: 4735
		private long ageReversalDemandedAtAgeTicks;

		// Token: 0x04001280 RID: 4736
		public long vatGrowTicks;

		// Token: 0x04001281 RID: 4737
		private Pawn_AgeTracker.AgeReversalReason lastAgeReversalReason;

		// Token: 0x04001282 RID: 4738
		private bool initializedAgeReversalDemand;

		// Token: 0x04001283 RID: 4739
		private bool lifeStageChange;

		// Token: 0x04001284 RID: 4740
		public float growthPoints = -1f;

		// Token: 0x04001285 RID: 4741
		private int cachedLifeStageIndex = -1;

		// Token: 0x04001286 RID: 4742
		private const float BornAtLongitude = 0f;

		// Token: 0x04001287 RID: 4743
		private const int GrowthInterval = 240;

		// Token: 0x04001288 RID: 4744
		public const int AgeReversalDemandMinAgeYears = 25;

		// Token: 0x04001289 RID: 4745
		private const int TreatedPawnAgeReversalDemandInDays = 60;

		// Token: 0x0400128A RID: 4746
		public const int MinGrowthBirthday = 3;

		// Token: 0x0400128B RID: 4747
		public const int MaxGrowthBirthday = 13;

		// Token: 0x0400128C RID: 4748
		private const float NicknameGainChance = 0.5f;

		// Token: 0x0400128D RID: 4749
		private readonly IntRange NewPawnAgeReversalDemandInDays = new IntRange(20, 40);

		// Token: 0x0400128E RID: 4750
		private readonly IntRange RecruitedPawnAgeReversalDemandInDays = new IntRange(15, 20);

		// Token: 0x0400128F RID: 4751
		private const int ColonistDevelopmentStageLessonInterval = 1000;

		// Token: 0x04001290 RID: 4752
		private const float GrowthPointsSuspended = 2f;

		// Token: 0x04001291 RID: 4753
		private const float YoungAgeCutoff = 7f;

		// Token: 0x04001292 RID: 4754
		private const float GrowthPointsFactor_Young = 0.75f;

		// Token: 0x04001293 RID: 4755
		private static List<int> growthMomentAges = null;

		// Token: 0x04001294 RID: 4756
		private static List<WorkTypeDef> tmpEnabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04001295 RID: 4757
		private static List<HediffDef> tmpHediffsGained = new List<HediffDef>();

		// Token: 0x02001E49 RID: 7753
		public enum AgeReversalReason
		{
			// Token: 0x04007763 RID: 30563
			Initial,
			// Token: 0x04007764 RID: 30564
			Recruited,
			// Token: 0x04007765 RID: 30565
			ViaTreatment
		}
	}
}

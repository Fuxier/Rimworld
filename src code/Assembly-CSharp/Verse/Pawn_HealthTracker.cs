using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000374 RID: 884
	public class Pawn_HealthTracker : IExposable
	{
		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x00097D09 File Offset: 0x00095F09
		public PawnHealthState State
		{
			get
			{
				return this.healthState;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x00097D11 File Offset: 0x00095F11
		public bool Downed
		{
			get
			{
				return this.healthState == PawnHealthState.Down;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x00097D1C File Offset: 0x00095F1C
		public bool Dead
		{
			get
			{
				return this.healthState == PawnHealthState.Dead;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x00097D27 File Offset: 0x00095F27
		public float LethalDamageThreshold
		{
			get
			{
				return 150f * this.pawn.HealthScale;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x00097D3A File Offset: 0x00095F3A
		public bool InPainShock
		{
			get
			{
				return this.hediffSet.PainTotal >= this.pawn.GetStatValue(StatDefOf.PainShockThreshold, true, -1);
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00097D60 File Offset: 0x00095F60
		public Pawn_HealthTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.hediffSet = new HediffSet(pawn);
			this.capacities = new PawnCapacitiesHandler(pawn);
			this.summaryHealth = new SummaryHealthHandler(pawn);
			this.surgeryBills = new BillStack(pawn);
			this.immunity = new ImmunityHandler(pawn);
			this.beCarriedByCaravanIfSick = pawn.RaceProps.Humanlike;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x00097DEC File Offset: 0x00095FEC
		public void Reset()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.Clear();
			this.capacities.Clear();
			this.summaryHealth.Notify_HealthChanged();
			this.surgeryBills.Clear();
			this.immunity = new ImmunityHandler(this.pawn);
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x00097E40 File Offset: 0x00096040
		public void ExposeData()
		{
			Scribe_Values.Look<PawnHealthState>(ref this.healthState, "healthState", PawnHealthState.Mobile, false);
			Scribe_Values.Look<bool>(ref this.forceDowned, "forceDowned", false, false);
			Scribe_Values.Look<bool>(ref this.beCarriedByCaravanIfSick, "beCarriedByCaravanIfSick", true, false);
			Scribe_Values.Look<bool>(ref this.killedByRitual, "killedByRitual", false, false);
			Scribe_Values.Look<int>(ref this.lastReceivedNeuralSuperchargeTick, "lastReceivedNeuralSuperchargeTick", -1, false);
			Scribe_Deep.Look<HediffSet>(ref this.hediffSet, "hediffSet", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<BillStack>(ref this.surgeryBills, "surgeryBills", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<ImmunityHandler>(ref this.immunity, "immunity", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x00097F04 File Offset: 0x00096104
		public Hediff AddHediff(HediffDef def, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, null);
			this.AddHediff(hediff, part, dinfo, result);
			return hediff;
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x00097F2C File Offset: 0x0009612C
		public void AddHediff(Hediff hediff, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			if (part != null)
			{
				hediff.Part = part;
			}
			this.hediffSet.AddDirect(hediff, dinfo, result);
			this.CheckForStateChange(dinfo, hediff);
			if (this.pawn.RaceProps.hediffGiverSets != null)
			{
				for (int i = 0; i < this.pawn.RaceProps.hediffGiverSets.Count; i++)
				{
					HediffGiverSetDef hediffGiverSetDef = this.pawn.RaceProps.hediffGiverSets[i];
					for (int j = 0; j < hediffGiverSetDef.hediffGivers.Count; j++)
					{
						hediffGiverSetDef.hediffGivers[j].OnHediffAdded(this.pawn, hediff);
					}
				}
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00097FD2 File Offset: 0x000961D2
		public void RemoveHediff(Hediff hediff)
		{
			hediff.PreRemoved();
			this.hediffSet.hediffs.Remove(hediff);
			hediff.PostRemoved();
			this.Notify_HediffChanged(hediff);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x00097FFC File Offset: 0x000961FC
		public void RemoveAllHediffs()
		{
			for (int i = this.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				this.RemoveHediff(this.hediffSet.hediffs[i]);
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00098040 File Offset: 0x00096240
		public void Notify_HediffChanged(Hediff hediff)
		{
			this.hediffSet.DirtyCache();
			this.CheckForStateChange(null, hediff);
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00098068 File Offset: 0x00096268
		public void Notify_UsedVerb(Verb verb, LocalTargetInfo target)
		{
			foreach (Hediff hediff in this.hediffSet.hediffs)
			{
				hediff.Notify_PawnUsedVerb(verb, target);
			}
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x000980C0 File Offset: 0x000962C0
		public void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			Faction homeFaction = this.pawn.HomeFaction;
			if (dinfo.Instigator != null && homeFaction != null && homeFaction.IsPlayer && !this.pawn.InAggroMentalState && !dinfo.Def.consideredHelpful)
			{
				Pawn pawn = dinfo.Instigator as Pawn;
				if (dinfo.InstigatorGuilty && pawn != null && pawn.guilt != null && pawn.mindState != null)
				{
					pawn.guilt.Notify_Guilty(60000);
				}
			}
			if (this.pawn.Spawned)
			{
				if (!this.pawn.Position.Fogged(this.pawn.Map))
				{
					this.pawn.mindState.Active = true;
				}
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnDamaged(this.pawn, dinfo);
				}
				if (dinfo.Def.ExternalViolenceFor(this.pawn))
				{
					GenClamor.DoClamor(this.pawn, 18f, ClamorDefOf.Harm);
				}
				this.pawn.jobs.Notify_DamageTaken(dinfo);
			}
			if (homeFaction != null)
			{
				homeFaction.Notify_MemberTookDamage(this.pawn, dinfo);
				if (Current.ProgramState == ProgramState.Playing && homeFaction == Faction.OfPlayer && dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.SpawnedOrAnyParentSpawned)
				{
					this.pawn.MapHeld.dangerWatcher.Notify_ColonistHarmedExternally();
				}
			}
			if (this.pawn.apparel != null && !dinfo.IgnoreArmor)
			{
				List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].CheckPreAbsorbDamage(dinfo))
					{
						absorbed = true;
						return;
					}
				}
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.Notify_DamageTaken(dinfo);
				this.pawn.stances.stunner.Notify_DamageApplied(dinfo);
			}
			if (this.pawn.RaceProps.IsFlesh && dinfo.Def.ExternalViolenceFor(this.pawn))
			{
				Pawn pawn2 = dinfo.Instigator as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.HostileTo(this.pawn))
					{
						this.pawn.relations.canGetRescuedThought = true;
					}
					if (this.pawn.RaceProps.Humanlike && pawn2.RaceProps.Humanlike && this.pawn.needs.mood != null && (!pawn2.HostileTo(this.pawn) || (pawn2.Faction == homeFaction && pawn2.InMentalState)))
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HarmedMe, pawn2, null);
					}
				}
				ThingDef thingDef = (pawn2 != null && dinfo.Weapon != pawn2.def) ? dinfo.Weapon : null;
				TaleRecorder.RecordTale(TaleDefOf.Wounded, new object[]
				{
					this.pawn,
					pawn2,
					thingDef
				});
			}
			absorbed = false;
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x000983D4 File Offset: 0x000965D4
		public void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.ShouldBeDead())
			{
				if (!this.ShouldBeDeathrestingOrInComa())
				{
					if (!this.pawn.Destroyed)
					{
						this.pawn.Kill(new DamageInfo?(dinfo), null);
					}
					return;
				}
				this.ForceDeathrestOrComa(new DamageInfo?(dinfo), null);
			}
			Pawn pawn;
			if (dinfo.Def.additionalHediffs != null && (dinfo.Def.applyAdditionalHediffsIfHuntingForFood || (pawn = (dinfo.Instigator as Pawn)) == null || pawn.CurJob == null || pawn.CurJob.def != JobDefOf.PredatorHunt))
			{
				List<DamageDefAdditionalHediff> additionalHediffs = dinfo.Def.additionalHediffs;
				for (int i = 0; i < additionalHediffs.Count; i++)
				{
					DamageDefAdditionalHediff damageDefAdditionalHediff = additionalHediffs[i];
					if (damageDefAdditionalHediff.hediff != null)
					{
						float num = (damageDefAdditionalHediff.severityFixed <= 0f) ? (totalDamageDealt * damageDefAdditionalHediff.severityPerDamageDealt) : damageDefAdditionalHediff.severityFixed;
						if (damageDefAdditionalHediff.victimSeverityScalingByInvBodySize)
						{
							num *= 1f / this.pawn.BodySize;
						}
						if (damageDefAdditionalHediff.victimSeverityScaling != null)
						{
							num *= (damageDefAdditionalHediff.inverseStatScaling ? Mathf.Max(1f - this.pawn.GetStatValue(damageDefAdditionalHediff.victimSeverityScaling, true, -1), 0f) : this.pawn.GetStatValue(damageDefAdditionalHediff.victimSeverityScaling, true, -1));
						}
						if (num >= 0f)
						{
							Hediff hediff = HediffMaker.MakeHediff(damageDefAdditionalHediff.hediff, this.pawn, null);
							hediff.Severity = num;
							this.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
							if (this.Dead)
							{
								return;
							}
						}
					}
				}
			}
			for (int j = 0; j < this.hediffSet.hediffs.Count; j++)
			{
				this.hediffSet.hediffs[j].Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			}
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x000985A4 File Offset: 0x000967A4
		public void RestorePart(BodyPartRecord part, Hediff diffException = null, bool checkStateChange = true)
		{
			if (part == null)
			{
				Log.Error("Tried to restore null body part.");
				return;
			}
			this.RestorePartRecursiveInt(part, diffException);
			this.hediffSet.DirtyCache();
			if (checkStateChange)
			{
				this.CheckForStateChange(null, null);
			}
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000985E8 File Offset: 0x000967E8
		private void RestorePartRecursiveInt(BodyPartRecord part, Hediff diffException = null)
		{
			List<Hediff> hediffs = this.hediffSet.hediffs;
			for (int i = hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff = hediffs[i];
				if (hediff.Part == part && hediff != diffException && !hediff.def.keepOnBodyPartRestoration)
				{
					hediffs.RemoveAt(i);
					hediff.PostRemoved();
				}
			}
			for (int j = 0; j < part.parts.Count; j++)
			{
				this.RestorePartRecursiveInt(part.parts[j], diffException);
			}
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0009866C File Offset: 0x0009686C
		public void CheckForStateChange(DamageInfo? dinfo, Hediff hediff)
		{
			if (!this.Dead)
			{
				if (ModsConfig.BiotechActive && this.pawn.mechanitor != null)
				{
					this.pawn.mechanitor.Notify_HediffStateChange(hediff);
				}
				if (hediff != null && hediff.def.blocksSleeping && !this.pawn.Awake())
				{
					RestUtility.WakeUp(this.pawn);
					return;
				}
				if (this.ShouldBeDead())
				{
					if (this.ShouldBeDeathrestingOrInComa())
					{
						this.ForceDeathrestOrComa(dinfo, hediff);
						return;
					}
					if (!this.pawn.Destroyed)
					{
						this.pawn.Kill(dinfo, hediff);
						return;
					}
				}
				else if (!this.Downed)
				{
					if (this.ShouldBeDowned())
					{
						if (!this.forceDowned && dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this.pawn) && !this.pawn.IsWildMan() && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && (this.pawn.HostFaction == null || !this.pawn.HostFaction.IsPlayer))
						{
							bool flag = ModsConfig.BiotechActive && this.pawn.genes != null && this.pawn.genes.HasGene(GeneDefOf.Deathless);
							float num;
							if (flag && this.pawn.Faction == Faction.OfPlayer)
							{
								num = 0f;
							}
							else if (this.pawn.RaceProps.Animal)
							{
								num = 0.5f;
							}
							else if (this.pawn.RaceProps.IsMechanoid)
							{
								num = 1f;
							}
							else
							{
								num = (Find.Storyteller.difficulty.unwaveringPrisoners ? HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve : HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve_WaveringPrisoners).Evaluate(StorytellerUtilityPopulation.PopulationIntent) * Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor;
							}
							if (Rand.Chance(num))
							{
								if (DebugViewSettings.logCauseOfDeath)
								{
									Log.Message("CauseOfDeath: chance on downed " + num.ToStringPercent());
								}
								if (flag && !this.pawn.Dead)
								{
									this.pawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.pawn.health.hediffSet.GetBrain(), dinfo, null);
									return;
								}
								this.pawn.Kill(dinfo, null);
								return;
							}
						}
						this.forceDowned = false;
						this.MakeDowned(dinfo, hediff);
						return;
					}
					if (!this.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						if (this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null && this.pawn.jobs != null && this.pawn.CurJob != null)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
						if (this.pawn.equipment != null && this.pawn.equipment.Primary != null)
						{
							if (this.pawn.kindDef.destroyGearOnDrop)
							{
								this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
								return;
							}
							if (this.pawn.InContainerEnclosed)
							{
								this.pawn.equipment.TryTransferEquipmentToContainer(this.pawn.equipment.Primary, this.pawn.holdingOwner);
								return;
							}
							if (this.pawn.SpawnedOrAnyParentSpawned)
							{
								ThingWithComps thingWithComps;
								this.pawn.equipment.TryDropEquipment(this.pawn.equipment.Primary, out thingWithComps, this.pawn.PositionHeld, true);
								return;
							}
							if (!this.pawn.IsCaravanMember())
							{
								this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
								return;
							}
							ThingWithComps primary = this.pawn.equipment.Primary;
							this.pawn.equipment.Remove(primary);
							if (!this.pawn.inventory.innerContainer.TryAdd(primary, true))
							{
								primary.Destroy(DestroyMode.Vanish);
								return;
							}
						}
					}
				}
				else if (!this.ShouldBeDowned())
				{
					this.MakeUndowned(hediff);
					return;
				}
			}
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00098A94 File Offset: 0x00096C94
		private bool ShouldBeDeathrestingOrInComa()
		{
			return ModsConfig.BiotechActive && SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(this.pawn);
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00098AAF File Offset: 0x00096CAF
		private bool ShouldBeDowned()
		{
			return this.InPainShock || !this.capacities.CanBeAwake || !this.capacities.CapableOf(PawnCapacityDefOf.Moving) || this.pawn.ageTracker.CurLifeStage.alwaysDowned;
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00098AF0 File Offset: 0x00096CF0
		public bool ShouldBeDead()
		{
			if (this.Dead)
			{
				return true;
			}
			for (int i = 0; i < this.hediffSet.hediffs.Count; i++)
			{
				if (this.hediffSet.hediffs[i].CauseDeathNow())
				{
					return true;
				}
			}
			if (this.ShouldBeDeadFromRequiredCapacity() != null)
			{
				return true;
			}
			if (PawnCapacityUtility.CalculatePartEfficiency(this.hediffSet, this.pawn.RaceProps.body.corePart, false, null) <= 0.0001f)
			{
				if (DebugViewSettings.logCauseOfDeath)
				{
					Log.Message("CauseOfDeath: zero efficiency of " + this.pawn.RaceProps.body.corePart.Label);
				}
				return true;
			}
			return this.ShouldBeDeadFromLethalDamageThreshold();
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00098BAC File Offset: 0x00096DAC
		public PawnCapacityDef ShouldBeDeadFromRequiredCapacity()
		{
			List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				PawnCapacityDef pawnCapacityDef = allDefsListForReading[i];
				if ((this.pawn.RaceProps.IsFlesh ? pawnCapacityDef.lethalFlesh : pawnCapacityDef.lethalMechanoids) && !this.capacities.CapableOf(pawnCapacityDef))
				{
					if (DebugViewSettings.logCauseOfDeath)
					{
						Log.Message("CauseOfDeath: no longer capable of " + pawnCapacityDef.defName);
					}
					return pawnCapacityDef;
				}
			}
			return null;
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00098C28 File Offset: 0x00096E28
		public bool ShouldBeDeadFromLethalDamageThreshold()
		{
			float num = 0f;
			for (int i = 0; i < this.hediffSet.hediffs.Count; i++)
			{
				if (this.hediffSet.hediffs[i] is Hediff_Injury)
				{
					num += this.hediffSet.hediffs[i].Severity;
				}
			}
			bool flag = num >= this.LethalDamageThreshold;
			if (flag && DebugViewSettings.logCauseOfDeath)
			{
				Log.Message(string.Concat(new object[]
				{
					"CauseOfDeath: lethal damage ",
					num,
					" >= ",
					this.LethalDamageThreshold
				}));
			}
			return flag;
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x00098CD8 File Offset: 0x00096ED8
		public bool WouldLosePartAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.CheckPredicateAfterAddingHediff(hediff, () => this.hediffSet.PartIsMissing(part));
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00098D28 File Offset: 0x00096F28
		public bool WouldDieAfterAddingHediff(Hediff hediff)
		{
			if (this.Dead)
			{
				return true;
			}
			bool flag = this.CheckPredicateAfterAddingHediff(hediff, new Func<bool>(this.ShouldBeDead));
			if (flag && DebugViewSettings.logCauseOfDeath)
			{
				Log.Message("CauseOfDeath: WouldDieAfterAddingHediff=true for " + this.pawn.Name);
			}
			return flag;
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x00098D78 File Offset: 0x00096F78
		public bool WouldDieAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldDieAfterAddingHediff(hediff);
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00098DA1 File Offset: 0x00096FA1
		public bool WouldBeDownedAfterAddingHediff(Hediff hediff)
		{
			return !this.Dead && this.CheckPredicateAfterAddingHediff(hediff, new Func<bool>(this.ShouldBeDowned));
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x00098DC0 File Offset: 0x00096FC0
		public bool WouldBeDownedAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldBeDownedAfterAddingHediff(hediff);
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x00098DE9 File Offset: 0x00096FE9
		public void SetDead()
		{
			if (this.Dead)
			{
				Log.Error(this.pawn + " set dead while already dead.");
			}
			this.healthState = PawnHealthState.Dead;
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00098E10 File Offset: 0x00097010
		private bool CheckPredicateAfterAddingHediff(Hediff hediff, Func<bool> pred)
		{
			HashSet<Hediff> missing = this.CalculateMissingPartHediffsFromInjury(hediff);
			this.hediffSet.hediffs.Add(hediff);
			if (missing != null)
			{
				this.hediffSet.hediffs.AddRange(missing);
			}
			this.hediffSet.DirtyCache();
			bool result = pred();
			if (missing != null)
			{
				this.hediffSet.hediffs.RemoveAll((Hediff x) => missing.Contains(x));
			}
			this.hediffSet.hediffs.Remove(hediff);
			this.hediffSet.DirtyCache();
			return result;
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00098EB4 File Offset: 0x000970B4
		private HashSet<Hediff> CalculateMissingPartHediffsFromInjury(Hediff hediff)
		{
			Pawn_HealthTracker.<>c__DisplayClass49_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.hediff = hediff;
			CS$<>8__locals1.missing = null;
			if (CS$<>8__locals1.hediff.Part != null && CS$<>8__locals1.hediff.Part != this.pawn.RaceProps.body.corePart && CS$<>8__locals1.hediff.Severity >= this.hediffSet.GetPartHealth(CS$<>8__locals1.hediff.Part))
			{
				CS$<>8__locals1.missing = new HashSet<Hediff>();
				this.<CalculateMissingPartHediffsFromInjury>g__AddAllParts|49_0(CS$<>8__locals1.hediff.Part, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.missing;
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00098F50 File Offset: 0x00097150
		private void ForceDeathrestOrComa(DamageInfo? dinfo, Hediff hediff)
		{
			if (this.pawn.CanDeathrest())
			{
				if (!this.pawn.Deathresting)
				{
					SanguophageUtility.TryStartDeathrest(this.pawn, DeathrestStartReason.LethalDamage);
					GeneUtility.OffsetHemogen(this.pawn, -9999f, true);
					if (!this.Downed)
					{
						this.forceDowned = true;
						this.MakeDowned(dinfo, hediff);
						return;
					}
				}
			}
			else if (!this.hediffSet.HasHediff(HediffDefOf.RegenerationComa, false))
			{
				this.AddHediff(HediffDefOf.RegenerationComa, null, null, null);
				if (!this.Downed)
				{
					this.forceDowned = true;
					this.MakeDowned(dinfo, hediff);
				}
			}
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00098FF0 File Offset: 0x000971F0
		private void MakeDowned(DamageInfo? dinfo, Hediff hediff)
		{
			if (this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeDowned while already downed.");
				return;
			}
			if (this.pawn.guilt != null && this.pawn.GetLord() != null && this.pawn.GetLord().LordJob != null && this.pawn.GetLord().LordJob.GuiltyOnDowned)
			{
				this.pawn.guilt.Notify_Guilty(60000);
			}
			this.healthState = PawnHealthState.Down;
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this.pawn, dinfo, PawnDiedOrDownedThoughtsKind.Downed);
			if (this.pawn.InMentalState && this.pawn.MentalStateDef.recoverFromDowned)
			{
				this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
			this.pawn.mindState.droppedWeapon = null;
			if (this.pawn.Spawned)
			{
				this.pawn.DropAndForbidEverything(true, true);
				this.pawn.stances.CancelBusyStanceSoft();
			}
			if (!this.pawn.DutyActiveWhenDown(true))
			{
				this.pawn.ClearMind(true, false, false);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.pawn, PawnLostCondition.Incapped, dinfo);
				}
			}
			if (this.pawn.Drafted)
			{
				this.pawn.drafter.Drafted = false;
			}
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			if (this.pawn.SpawnedOrAnyParentSpawned)
			{
				GenHostility.Notify_PawnLostForTutor(this.pawn, this.pawn.MapHeld);
			}
			if (this.pawn.RaceProps.Humanlike && Current.ProgramState == ProgramState.Playing && this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.pawn.HostileTo(Faction.OfPlayer))
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Capturing, this.pawn, OpportunityType.Important);
				}
				if (this.pawn.Faction == Faction.OfPlayer)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Rescuing, this.pawn, OpportunityType.Critical);
				}
			}
			if (dinfo != null && dinfo.Value.Instigator != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null)
				{
					RecordsUtility.Notify_PawnDowned(this.pawn, pawn);
				}
			}
			if (this.pawn.Spawned && (hediff == null || hediff.def.recordDownedTale))
			{
				TaleRecorder.RecordTale(TaleDefOf.Downed, new object[]
				{
					this.pawn,
					(dinfo != null) ? (dinfo.Value.Instigator as Pawn) : null,
					(dinfo != null) ? dinfo.Value.Weapon : null
				});
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this.pawn, RulePackDefOf.Transition_Downed, (dinfo != null) ? (dinfo.Value.Instigator as Pawn) : null, hediff, (dinfo != null) ? dinfo.Value.HitPart : null));
			}
			Find.Storyteller.Notify_PawnEvent(this.pawn, AdaptationEvent.Downed, dinfo);
			Pawn_MechanitorTracker mechanitor = this.pawn.mechanitor;
			if (mechanitor == null)
			{
				return;
			}
			mechanitor.Notify_Downed();
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0009933C File Offset: 0x0009753C
		private void MakeUndowned(Hediff hediff)
		{
			if (!this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeUndowned when already undowned.");
				return;
			}
			this.healthState = PawnHealthState.Mobile;
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) && (hediff == null || hediff.def != HediffDefOf.Deathrest))
			{
				Messages.Message("MessageNoLongerDowned".Translate(this.pawn.LabelCap, this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			if (this.pawn.guest != null)
			{
				this.pawn.guest.Notify_PawnUndowned();
			}
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00099400 File Offset: 0x00097600
		public void NotifyPlayerOfKilled(DamageInfo? dinfo, Hediff hediff, Caravan caravan)
		{
			TaggedString taggedString = "";
			if (dinfo != null)
			{
				taggedString = dinfo.Value.Def.deathMessage.Formatted(this.pawn.LabelShortCap, this.pawn.Named("PAWN"));
			}
			else if (hediff != null)
			{
				taggedString = "PawnDiedBecauseOf".Translate(this.pawn.LabelShortCap, hediff.def.LabelCap, this.pawn.Named("PAWN"));
			}
			else
			{
				taggedString = "PawnDied".Translate(this.pawn.LabelShortCap, this.pawn.Named("PAWN"));
			}
			Quest quest = null;
			if (this.pawn.IsBorrowedByAnyFaction())
			{
				foreach (QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction in QuestUtility.GetAllQuestPartsOfType<QuestPart_LendColonistsToFaction>(true))
				{
					if (questPart_LendColonistsToFaction.LentColonistsListForReading.Contains(this.pawn))
					{
						taggedString += "\n\n" + "LentColonistDied".Translate(this.pawn.Named("PAWN"), questPart_LendColonistsToFaction.lendColonistsToFaction.Named("FACTION"));
						quest = questPart_LendColonistsToFaction.quest;
						break;
					}
				}
			}
			taggedString = taggedString.AdjustedFor(this.pawn, "PAWN", true);
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				TaggedString taggedString2 = "Death".Translate() + ": " + this.pawn.LabelShortCap;
				if (caravan != null)
				{
					Messages.Message("MessageCaravanDeathCorpseAddedToInventory".Translate(this.pawn.Named("PAWN")), caravan, MessageTypeDefOf.PawnDeath, true);
				}
				if (this.pawn.Ideo != null)
				{
					foreach (Precept precept in this.pawn.Ideo.PreceptsListForReading)
					{
						if (!string.IsNullOrWhiteSpace(precept.def.extraTextPawnDeathLetter))
						{
							taggedString += "\n\n" + precept.def.extraTextPawnDeathLetter.Formatted(this.pawn.Named("PAWN"));
						}
					}
				}
				if (this.pawn.Name != null && !this.pawn.Name.Numerical && this.pawn.RaceProps.Animal)
				{
					taggedString2 += " (" + this.pawn.KindLabel + ")";
				}
				Pawn_RelationsTracker relations = this.pawn.relations;
				if (relations != null)
				{
					relations.CheckAppendBondedAnimalDiedInfo(ref taggedString, ref taggedString2);
				}
				Find.LetterStack.ReceiveLetter(taggedString2, taggedString, LetterDefOf.Death, this.pawn, null, quest, null, null);
				return;
			}
			Messages.Message(taggedString, this.pawn, MessageTypeDefOf.PawnDeath, true);
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00099738 File Offset: 0x00097938
		public void Notify_Resurrected()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x.TryGetComp<HediffComp_Immunizable>() != null);
			this.hediffSet.hediffs.RemoveAll(delegate(Hediff x)
			{
				if (!x.def.everCurableByItem)
				{
					return false;
				}
				if (x.def.lethalSeverity >= 0f)
				{
					return true;
				}
				if (x.def.stages != null)
				{
					return x.def.stages.Any((HediffStage y) => y.lifeThreatening);
				}
				return false;
			});
			if (!this.pawn.RaceProps.IsMechanoid)
			{
				this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x is Hediff_Injury && !x.IsPermanent());
			}
			else
			{
				this.tmpMechInjuries.Clear();
				this.hediffSet.GetHediffs<Hediff_Injury>(ref this.tmpMechInjuries, (Hediff_Injury x) => x != null && x.def.everCurableByItem && !x.IsPermanent());
				if (this.tmpMechInjuries.Count > 0)
				{
					float num = this.tmpMechInjuries.Sum((Hediff_Injury x) => x.Severity) * 0.5f / (float)this.tmpMechInjuries.Count;
					for (int i = 0; i < this.tmpMechInjuries.Count; i++)
					{
						this.tmpMechInjuries[i].Severity -= num;
					}
					this.tmpMechInjuries.Clear();
				}
			}
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x is Hediff_Injury && x.IsPermanent() && this.hediffSet.GetPartHealth(x.Part) <= 0f);
			for (;;)
			{
				Hediff_MissingPart hediff_MissingPart = (from x in this.hediffSet.GetMissingPartsCommonAncestors()
				where !this.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x.Part)
				select x).FirstOrDefault<Hediff_MissingPart>();
				if (hediff_MissingPart == null)
				{
					break;
				}
				this.RestorePart(hediff_MissingPart.Part, null, false);
			}
			for (int j = this.hediffSet.hediffs.Count - 1; j >= 0; j--)
			{
				this.hediffSet.hediffs[j].Notify_Resurrected();
			}
			this.hediffSet.DirtyCache();
			if (this.ShouldBeDead())
			{
				this.hediffSet.hediffs.RemoveAll((Hediff h) => !h.def.keepOnBodyPartRestoration);
			}
			this.Notify_HediffChanged(null);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00099984 File Offset: 0x00097B84
		public void HealthTick()
		{
			if (this.Dead)
			{
				return;
			}
			for (int i = this.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff = this.hediffSet.hediffs[i];
				try
				{
					hediff.Tick();
					hediff.PostTick();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception ticking hediff ",
						hediff.ToStringSafe<Hediff>(),
						" for pawn ",
						this.pawn.ToStringSafe<Pawn>(),
						". Removing hediff... Exception: ",
						ex
					}));
					try
					{
						this.RemoveHediff(hediff);
					}
					catch (Exception arg)
					{
						Log.Error("Error while removing hediff: " + arg);
					}
				}
				if (this.Dead)
				{
					return;
				}
			}
			bool flag = false;
			for (int j = this.hediffSet.hediffs.Count - 1; j >= 0; j--)
			{
				Hediff hediff2 = this.hediffSet.hediffs[j];
				if (hediff2.ShouldRemove)
				{
					hediff2.PreRemoved();
					this.hediffSet.hediffs.RemoveAt(j);
					hediff2.PostRemoved();
					flag = true;
				}
			}
			if (flag)
			{
				this.Notify_HediffChanged(null);
			}
			if (this.Dead)
			{
				return;
			}
			this.immunity.ImmunityHandlerTick();
			if (this.pawn.RaceProps.IsFlesh && this.pawn.IsHashIntervalTick(600) && (this.pawn.needs.food == null || !this.pawn.needs.food.Starving))
			{
				bool flag2 = false;
				if (this.hediffSet.HasNaturallyHealingInjury())
				{
					float num = 8f;
					if (this.pawn.GetPosture() != PawnPosture.Standing)
					{
						num += 4f;
						Building_Bed building_Bed = this.pawn.CurrentBed();
						if (building_Bed != null)
						{
							num += building_Bed.def.building.bed_healPerDay;
						}
					}
					foreach (Hediff hediff3 in this.hediffSet.hediffs)
					{
						HediffStage curStage = hediff3.CurStage;
						if (curStage != null && curStage.naturalHealingFactor != -1f)
						{
							num *= curStage.naturalHealingFactor;
						}
					}
					this.hediffSet.GetHediffs<Hediff_Injury>(ref this.tmpHediffInjuries, (Hediff_Injury h) => h.CanHealNaturally());
					this.tmpHediffInjuries.RandomElement<Hediff_Injury>().Heal(num * this.pawn.HealthScale * 0.01f * this.pawn.GetStatValue(StatDefOf.InjuryHealingFactor, true, -1));
					flag2 = true;
				}
				if (this.hediffSet.HasTendedAndHealingInjury())
				{
					Need_Food food = this.pawn.needs.food;
					if (food == null || !food.Starving)
					{
						this.hediffSet.GetHediffs<Hediff_Injury>(ref this.tmpHediffInjuries, (Hediff_Injury h) => h.CanHealFromTending());
						Hediff_Injury hediff_Injury = this.tmpHediffInjuries.RandomElement<Hediff_Injury>();
						float tendQuality = hediff_Injury.TryGetComp<HediffComp_TendDuration>().tendQuality;
						float num2 = GenMath.LerpDouble(0f, 1f, 0.5f, 1.5f, Mathf.Clamp01(tendQuality));
						hediff_Injury.Heal(8f * num2 * this.pawn.HealthScale * 0.01f * this.pawn.GetStatValue(StatDefOf.InjuryHealingFactor, true, -1));
						flag2 = true;
					}
				}
				if (flag2 && !this.HasHediffsNeedingTendByPlayer(false) && !HealthAIUtility.ShouldSeekMedicalRest(this.pawn) && PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageFullyHealed".Translate(this.pawn.LabelCap, this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			if (this.pawn.RaceProps.IsFlesh && this.hediffSet.BleedRateTotal >= 0.1f)
			{
				float num3 = this.hediffSet.BleedRateTotal * this.pawn.BodySize;
				if (this.pawn.GetPosture() == PawnPosture.Standing)
				{
					num3 *= 0.004f;
				}
				else
				{
					num3 *= 0.0004f;
				}
				if (Rand.Value < num3)
				{
					this.DropBloodFilth();
				}
			}
			if (this.pawn.IsHashIntervalTick(60))
			{
				List<HediffGiverSetDef> hediffGiverSets = this.pawn.RaceProps.hediffGiverSets;
				if (hediffGiverSets != null)
				{
					for (int k = 0; k < hediffGiverSets.Count; k++)
					{
						List<HediffGiver> hediffGivers = hediffGiverSets[k].hediffGivers;
						for (int l = 0; l < hediffGivers.Count; l++)
						{
							hediffGivers[l].OnIntervalPassed(this.pawn, null);
							if (this.pawn.Dead)
							{
								return;
							}
						}
					}
				}
				if (this.pawn.story != null)
				{
					List<Trait> allTraits = this.pawn.story.traits.allTraits;
					for (int m = 0; m < allTraits.Count; m++)
					{
						if (!allTraits[m].Suppressed)
						{
							TraitDegreeData currentData = allTraits[m].CurrentData;
							if (currentData.randomDiseaseMtbDays > 0f && Rand.MTBEventOccurs(currentData.randomDiseaseMtbDays, 60000f, 60f))
							{
								BiomeDef biome;
								if (this.pawn.Tile != -1)
								{
									biome = Find.WorldGrid[this.pawn.Tile].biome;
								}
								else
								{
									biome = DefDatabase<BiomeDef>.GetRandom();
								}
								IncidentDef incidentDef = (from d in DefDatabase<IncidentDef>.AllDefs
								where d.category == IncidentCategoryDefOf.DiseaseHuman
								select d).RandomElementByWeightWithFallback((IncidentDef d) => biome.CommonalityOfDisease(d), null);
								if (incidentDef != null)
								{
									string text;
									List<Pawn> list = ((IncidentWorker_Disease)incidentDef.Worker).ApplyToPawns(Gen.YieldSingle<Pawn>(this.pawn), out text);
									if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
									{
										if (list.Contains(this.pawn))
										{
											Find.LetterStack.ReceiveLetter("LetterLabelTraitDisease".Translate(incidentDef.diseaseIncident.label), "LetterTraitDisease".Translate(this.pawn.LabelCap, incidentDef.diseaseIncident.label, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true), LetterDefOf.NegativeEvent, this.pawn, null, null, null, null);
										}
										else if (!text.NullOrEmpty())
										{
											Messages.Message(text, this.pawn, MessageTypeDefOf.NeutralEvent, true);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x0009A0A0 File Offset: 0x000982A0
		public bool HasHediffsNeedingTend(bool forAlert = false)
		{
			return this.hediffSet.HasTendableHediff(forAlert);
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0009A0B0 File Offset: 0x000982B0
		public bool HasHediffsNeedingTendByPlayer(bool forAlert = false)
		{
			if (this.HasHediffsNeedingTend(forAlert))
			{
				if (this.pawn.NonHumanlikeOrWildMan())
				{
					if (this.pawn.Faction == Faction.OfPlayer)
					{
						return true;
					}
					Building_Bed building_Bed = this.pawn.CurrentBed();
					if (building_Bed != null && building_Bed.Faction == Faction.OfPlayer)
					{
						return true;
					}
				}
				else if ((this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null) || this.pawn.HostFaction == Faction.OfPlayer)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0009A13C File Offset: 0x0009833C
		public void DropBloodFilth()
		{
			if ((this.pawn.Spawned || this.pawn.ParentHolder is Pawn_CarryTracker) && this.pawn.SpawnedOrAnyParentSpawned && this.pawn.RaceProps.BloodDef != null)
			{
				FilthMaker.TryMakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, this.pawn.RaceProps.BloodDef, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0009A1C0 File Offset: 0x000983C0
		public IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Hediff hediff in this.hediffSet.hediffs)
			{
				IEnumerable<Gizmo> gizmos = hediff.GetGizmos();
				if (gizmos != null)
				{
					foreach (Gizmo gizmo in gizmos)
					{
						yield return gizmo;
					}
					IEnumerator<Gizmo> enumerator2 = null;
				}
			}
			List<Hediff>.Enumerator enumerator = default(List<Hediff>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0009A1D0 File Offset: 0x000983D0
		[CompilerGenerated]
		private void <CalculateMissingPartHediffsFromInjury>g__AddAllParts|49_0(BodyPartRecord part, ref Pawn_HealthTracker.<>c__DisplayClass49_0 A_2)
		{
			Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
			hediff_MissingPart.lastInjury = A_2.hediff.def;
			hediff_MissingPart.Part = part;
			A_2.missing.Add(hediff_MissingPart);
			foreach (BodyPartRecord part2 in part.parts)
			{
				this.<CalculateMissingPartHediffsFromInjury>g__AddAllParts|49_0(part2, ref A_2);
			}
		}

		// Token: 0x040012A6 RID: 4774
		private Pawn pawn;

		// Token: 0x040012A7 RID: 4775
		private PawnHealthState healthState = PawnHealthState.Mobile;

		// Token: 0x040012A8 RID: 4776
		[Unsaved(false)]
		public Effecter woundedEffecter;

		// Token: 0x040012A9 RID: 4777
		[Unsaved(false)]
		public Effecter deflectionEffecter;

		// Token: 0x040012AA RID: 4778
		[LoadAlias("forceIncap")]
		public bool forceDowned;

		// Token: 0x040012AB RID: 4779
		public bool beCarriedByCaravanIfSick;

		// Token: 0x040012AC RID: 4780
		public bool killedByRitual;

		// Token: 0x040012AD RID: 4781
		public int lastReceivedNeuralSuperchargeTick = -1;

		// Token: 0x040012AE RID: 4782
		public HediffSet hediffSet;

		// Token: 0x040012AF RID: 4783
		public PawnCapacitiesHandler capacities;

		// Token: 0x040012B0 RID: 4784
		public BillStack surgeryBills;

		// Token: 0x040012B1 RID: 4785
		public SummaryHealthHandler summaryHealth;

		// Token: 0x040012B2 RID: 4786
		public ImmunityHandler immunity;

		// Token: 0x040012B3 RID: 4787
		private List<Hediff_Injury> tmpMechInjuries = new List<Hediff_Injury>();

		// Token: 0x040012B4 RID: 4788
		private List<Hediff_Injury> tmpHediffInjuries = new List<Hediff_Injury>();
	}
}

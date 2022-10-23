using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200034B RID: 843
	public class Hediff_Pregnant : HediffWithParents
	{
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x00084B98 File Offset: 0x00082D98
		// (set) Token: 0x0600169F RID: 5791 RVA: 0x00084BA0 File Offset: 0x00082DA0
		public float GestationProgress
		{
			get
			{
				return this.Severity;
			}
			private set
			{
				this.Severity = value;
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060016A0 RID: 5792 RVA: 0x00084BAC File Offset: 0x00082DAC
		private bool IsSeverelyWounded
		{
			get
			{
				float num = 0f;
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i] is Hediff_Injury && !hediffs[i].IsPermanent())
					{
						num += hediffs[i].Severity;
					}
				}
				List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
				{
					if (missingPartsCommonAncestors[j].IsFreshNonSolidExtremity)
					{
						num += missingPartsCommonAncestors[j].Part.def.GetMaxHealth(this.pawn);
					}
				}
				return num > 38f * this.pawn.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060016A1 RID: 5793 RVA: 0x00084C84 File Offset: 0x00082E84
		public PregnancyAttitude? Attitude
		{
			get
			{
				if (this.comps == null || !this.pawn.RaceProps.Humanlike)
				{
					return null;
				}
				for (int i = 0; i < this.comps.Count; i++)
				{
					HediffComp_PregnantHuman hediffComp_PregnantHuman;
					if ((hediffComp_PregnantHuman = (this.comps[i] as HediffComp_PregnantHuman)) != null)
					{
						return hediffComp_PregnantHuman.Attitude;
					}
				}
				return null;
			}
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00084CF0 File Offset: 0x00082EF0
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.lastStage = this.CurStageIndex;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00084D08 File Offset: 0x00082F08
		public override void Tick()
		{
			this.ageTicks++;
			if (this.CurStageIndex != this.lastStage)
			{
				this.NotifyPlayerOfTrimesterPassing();
				this.lastStage = this.CurStageIndex;
			}
			if ((!this.pawn.RaceProps.Humanlike || !Find.Storyteller.difficulty.babiesAreHealthy) && this.pawn.IsHashIntervalTick(1000))
			{
				if (this.pawn.needs.food != null && this.pawn.needs.food.CurCategory == HungerCategory.Starving)
				{
					Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
					if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.1f && Rand.MTBEventOccurs(2f, 60000f, 1000f))
					{
						if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
						{
							string value = this.pawn.Name.Numerical ? this.pawn.LabelShort : (this.pawn.LabelShort + " (" + this.pawn.kindDef.label + ")");
							Messages.Message("MessageMiscarriedStarvation".Translate(value, this.pawn), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
						}
						this.Miscarry();
						return;
					}
				}
				if (this.IsSeverelyWounded && Rand.MTBEventOccurs(2f, 60000f, 1000f))
				{
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						string value2 = this.pawn.Name.Numerical ? this.pawn.LabelShort : (this.pawn.LabelShort + " (" + this.pawn.kindDef.label + ")");
						Messages.Message("MessageMiscarriedPoorHealth".Translate(value2, this.pawn), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					}
					this.Miscarry();
					return;
				}
			}
			float num = PawnUtility.BodyResourceGrowthSpeed(this.pawn) / (this.pawn.RaceProps.gestationPeriodDays * 60000f);
			this.GestationProgress += num;
			if (this.GestationProgress >= 1f)
			{
				if (!this.pawn.RaceProps.Humanlike)
				{
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageGaveBirth".Translate(this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
					}
					Hediff_Pregnant.DoBirthSpawn(this.pawn, base.Father);
				}
				else
				{
					this.StartLabor();
				}
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00085004 File Offset: 0x00083204
		private void Miscarry()
		{
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.Miscarried, null, null);
						}
					}
				}
			}
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00085060 File Offset: 0x00083260
		public static void DoBirthSpawn(Pawn mother, Pawn father)
		{
			if (mother.RaceProps.Humanlike && !ModsConfig.BiotechActive)
			{
				return;
			}
			int num = (mother.RaceProps.litterSizeCurve != null) ? Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve)) : 1;
			if (num < 1)
			{
				num = 1;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, -1, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false);
			Pawn pawn = null;
			for (int i = 0; i < num; i++)
			{
				pawn = PawnGenerator.GeneratePawn(request);
				if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, mother, null))
				{
					if (pawn.playerSettings != null && mother.playerSettings != null)
					{
						pawn.playerSettings.AreaRestriction = mother.playerSettings.AreaRestriction;
					}
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
						if (father != null)
						{
							pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
						}
					}
					if (mother.Spawned)
					{
						Lord lord = mother.GetLord();
						if (lord != null)
						{
							lord.AddPawn(pawn);
						}
					}
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				TaleRecorder.RecordTale(TaleDefOf.GaveBirth, new object[]
				{
					mother,
					pawn
				});
			}
			if (mother.Spawned)
			{
				FilthMaker.TryMakeFilth(mother.Position, mother.Map, ThingDefOf.Filth_AmnioticFluid, mother.LabelIndefinite(), 5, FilthSourceFlags.None);
				if (mother.caller != null)
				{
					mother.caller.DoCall();
				}
				if (pawn.caller != null)
				{
					pawn.caller.DoCall();
				}
			}
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00085254 File Offset: 0x00083454
		public void StartLabor()
		{
			if (!ModLister.CheckBiotech("labor"))
			{
				return;
			}
			((Hediff_Labor)this.pawn.health.AddHediff(HediffDefOf.PregnancyLabor, null, null, null)).SetParents(base.Mother, base.Father, this.geneSet);
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorningSickness, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff firstHediffOfDef2 = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnancyMood, false);
			if (firstHediffOfDef2 != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef2);
			}
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Find.LetterStack.ReceiveLetter("LetterColonistPregnancyLaborLabel".Translate(this.pawn), "LetterColonistPregnancyLabor".Translate(this.pawn), LetterDefOf.NeutralEvent, null);
			}
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00085351 File Offset: 0x00083551
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastStage, "lastStage", 0, false);
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x0008536C File Offset: 0x0008356C
		private void NotifyPlayerOfTrimesterPassing()
		{
			if (this.pawn.RaceProps.Humanlike && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message(((this.lastStage == 0) ? "MessageColonistReaching2ndTrimesterPregnancy" : "MessageColonistReaching3rdTrimesterPregnancy").Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				if (this.lastStage == 1 && !Find.History.everThirdTrimesterPregnancy)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelThirdTrimester".Translate(this.pawn), "LetterTextThirdTrimester".Translate(this.pawn), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
					Find.History.everThirdTrimesterPregnancy = true;
				}
			}
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x0008544F File Offset: 0x0008364F
		public override void PostDebugAdd()
		{
			if (ModsConfig.BiotechActive && this.pawn.RaceProps.Humanlike)
			{
				base.SetParents(this.pawn, null, PregnancyUtility.GetInheritedGeneSet(null, this.pawn));
			}
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x00085484 File Offset: 0x00083684
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.DebugString());
			stringBuilder.AppendLine("Gestation progress: " + this.GestationProgress.ToStringPercent());
			stringBuilder.AppendLine("Time left: " + ((int)((1f - this.GestationProgress) * this.pawn.RaceProps.gestationPeriodDays * 60000f)).ToStringTicksToPeriod(true, false, true, true, false));
			return stringBuilder.ToString();
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00085503 File Offset: 0x00083703
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				if (this.CurStageIndex < 2)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Next trimester",
						action = delegate()
						{
							HediffStage hediffStage = this.def.stages[this.CurStageIndex + 1];
							this.severityInt = hediffStage.minSeverity;
						}
					};
				}
				if (ModsConfig.BiotechActive && this.pawn.RaceProps.Humanlike && this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnancyLabor, false) == null)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Start Labor",
						action = delegate()
						{
							this.StartLabor();
							this.pawn.health.RemoveHediff(this);
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04001199 RID: 4505
		private int lastStage;

		// Token: 0x0400119A RID: 4506
		private const int MiscarryCheckInterval = 1000;

		// Token: 0x0400119B RID: 4507
		private const float MTBMiscarryStarvingDays = 2f;

		// Token: 0x0400119C RID: 4508
		private const float MTBMiscarryWoundedDays = 2f;

		// Token: 0x0400119D RID: 4509
		private const float MalnutritionMinSeverityForMiscarry = 0.1f;
	}
}

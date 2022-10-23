using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002C5 RID: 709
	public class Hediff : IExposable, ILoadReferenceable
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0007BB48 File Offset: 0x00079D48
		public virtual string LabelBase
		{
			get
			{
				HediffStage curStage = this.CurStage;
				return ((curStage != null) ? curStage.overrideLabel : null) ?? this.def.label;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x0007BB6B File Offset: 0x00079D6B
		public string LabelBaseCap
		{
			get
			{
				return this.LabelBase.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0007BB80 File Offset: 0x00079D80
		public virtual string Label
		{
			get
			{
				string labelInBrackets = this.LabelInBrackets;
				return this.LabelBase + (labelInBrackets.NullOrEmpty() ? "" : (" (" + labelInBrackets + ")"));
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0007BBBE File Offset: 0x00079DBE
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0007BBD1 File Offset: 0x00079DD1
		public virtual Color LabelColor
		{
			get
			{
				return this.def.defaultLabelColor;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x0007BBDE File Offset: 0x00079DDE
		public virtual string LabelInBrackets
		{
			get
			{
				if (this.CurStage != null && !this.CurStage.label.NullOrEmpty())
				{
					return this.CurStage.label;
				}
				return null;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0007BC07 File Offset: 0x00079E07
		public virtual string SeverityLabel
		{
			get
			{
				if (this.def.lethalSeverity > 0f)
				{
					return (this.Severity / this.def.lethalSeverity).ToStringPercent();
				}
				return null;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x0007BC34 File Offset: 0x00079E34
		public virtual int UIGroupKey
		{
			get
			{
				return this.Label.GetHashCode();
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0007BC44 File Offset: 0x00079E44
		public virtual string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (StatDrawEntry statDrawEntry in HediffStatsUtility.SpecialDisplayStats(this.CurStage, this))
				{
					if (statDrawEntry.ShouldDisplay)
					{
						stringBuilder.AppendLine("  - " + statDrawEntry.LabelCap + ": " + statDrawEntry.ValueString);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x0007BCC8 File Offset: 0x00079EC8
		public virtual HediffStage CurStage
		{
			get
			{
				if (!this.def.stages.NullOrEmpty<HediffStage>())
				{
					return this.def.stages[this.CurStageIndex];
				}
				return null;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x0007BCF4 File Offset: 0x00079EF4
		public virtual bool ShouldRemove
		{
			get
			{
				return this.Severity <= 0f;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x0007BD06 File Offset: 0x00079F06
		public virtual bool Visible
		{
			get
			{
				return this.visible || this.CurStage == null || this.CurStage.becomeVisible;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x00004E2A File Offset: 0x0000302A
		public virtual float BleedRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0007BD25 File Offset: 0x00079F25
		public virtual float BleedRateScaled
		{
			get
			{
				return this.BleedRate / this.pawn.HealthScale;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0007BD39 File Offset: 0x00079F39
		public bool Bleeding
		{
			get
			{
				return this.BleedRate > 1E-05f;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x0007BD48 File Offset: 0x00079F48
		public virtual float PainOffset
		{
			get
			{
				if (this.CurStage != null && !this.causesNoPain)
				{
					return this.CurStage.painOffset;
				}
				return 0f;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x0007BD6B File Offset: 0x00079F6B
		public virtual float PainFactor
		{
			get
			{
				if (this.CurStage != null)
				{
					return this.CurStage.painFactor;
				}
				return 1f;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x0007BD86 File Offset: 0x00079F86
		public List<PawnCapacityModifier> CapMods
		{
			get
			{
				if (this.CurStage != null)
				{
					return this.CurStage.capMods;
				}
				return null;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x00004E2A File Offset: 0x0000302A
		public virtual float SummaryHealthPercentImpact
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0007BDA0 File Offset: 0x00079FA0
		public virtual float TendPriority
		{
			get
			{
				float num = 0f;
				HediffStage curStage = this.CurStage;
				if (curStage != null && curStage.lifeThreatening)
				{
					num = Mathf.Max(num, 1f);
				}
				num = Mathf.Max(num, this.BleedRate * 1.5f);
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && hediffComp_TendDuration.TProps.severityPerDayTended < 0f)
				{
					num = Mathf.Max(num, 0.025f);
				}
				return num;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x0007BE0D File Offset: 0x0007A00D
		public virtual TextureAndColor StateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x0007BE14 File Offset: 0x0007A014
		public virtual int CurStageIndex
		{
			get
			{
				return this.def.StageAtSeverity(this.Severity);
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0007BE27 File Offset: 0x0007A027
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x0007BE30 File Offset: 0x0007A030
		public virtual float Severity
		{
			get
			{
				return this.severityInt;
			}
			set
			{
				bool flag = false;
				if (this.def.lethalSeverity > 0f && value >= this.def.lethalSeverity)
				{
					value = this.def.lethalSeverity;
					flag = true;
				}
				bool flag2 = this is Hediff_Injury && value > this.severityInt && Mathf.RoundToInt(value) != Mathf.RoundToInt(this.severityInt);
				int curStageIndex = this.CurStageIndex;
				this.severityInt = Mathf.Clamp(value, this.def.minSeverity, this.def.maxSeverity);
				if ((this.CurStageIndex != curStageIndex || flag || flag2) && this.pawn.health.hediffSet.hediffs.Contains(this))
				{
					this.pawn.health.Notify_HediffChanged(this);
					if (!this.pawn.Dead && this.pawn.needs.mood != null)
					{
						this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
					}
				}
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x0007BF40 File Offset: 0x0007A140
		// (set) Token: 0x06001433 RID: 5171 RVA: 0x0007BF48 File Offset: 0x0007A148
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.pawn == null && this.part != null)
				{
					Log.Error("Hediff: Cannot set Part without setting pawn first.");
					return;
				}
				this.part = value;
			}
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0007BF6C File Offset: 0x0007A16C
		public virtual bool TendableNow(bool ignoreTimer = false)
		{
			if (!this.def.tendable || this.Severity <= 0f || this.FullyImmune() || !this.Visible || this.IsPermanent() || !this.pawn.RaceProps.IsFlesh)
			{
				return false;
			}
			if (!ignoreTimer)
			{
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && !hediffComp_TendDuration.AllowTend)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0007BFD8 File Offset: 0x0007A1D8
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.combatLogEntry != null)
			{
				LogEntry target = this.combatLogEntry.Target;
				if (target == null || !Current.Game.battleLog.IsEntryActive(target))
				{
					this.combatLogEntry = null;
				}
			}
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<HediffDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.source, "source");
			Scribe_Defs.Look<BodyPartGroupDef>(ref this.sourceBodyPartGroup, "sourceBodyPartGroup");
			Scribe_Defs.Look<HediffDef>(ref this.sourceHediffDef, "sourceHediffDef");
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Values.Look<float>(ref this.severityInt, "severity", 0f, false);
			Scribe_Values.Look<bool>(ref this.recordedTale, "recordedTale", false, false);
			Scribe_Values.Look<bool>(ref this.causesNoPain, "causesNoPain", false, false);
			Scribe_Values.Look<bool>(ref this.visible, "visible", false, false);
			Scribe_References.Look<LogEntry>(ref this.combatLogEntry, "combatLogEntry", false);
			Scribe_Values.Look<string>(ref this.combatLogText, "combatLogText", null, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0007C108 File Offset: 0x0007A308
		public virtual void Tick()
		{
			this.ageTicks++;
			if (this.def.hediffGivers != null && this.pawn.IsHashIntervalTick(60))
			{
				for (int i = 0; i < this.def.hediffGivers.Count; i++)
				{
					this.def.hediffGivers[i].OnIntervalPassed(this.pawn, this);
				}
			}
			if (this.Visible && !this.visible)
			{
				this.visible = true;
				if (this.def.taleOnVisible != null)
				{
					TaleRecorder.RecordTale(this.def.taleOnVisible, new object[]
					{
						this.pawn,
						this.def
					});
				}
			}
			HediffStage curStage = this.CurStage;
			if (curStage != null)
			{
				if (curStage.hediffGivers != null && this.pawn.IsHashIntervalTick(60))
				{
					for (int j = 0; j < curStage.hediffGivers.Count; j++)
					{
						curStage.hediffGivers[j].OnIntervalPassed(this.pawn, this);
					}
				}
				if (curStage.mentalStateGivers != null && this.pawn.IsHashIntervalTick(60) && !this.pawn.InMentalState)
				{
					for (int k = 0; k < curStage.mentalStateGivers.Count; k++)
					{
						MentalStateGiver mentalStateGiver = curStage.mentalStateGivers[k];
						if (Rand.MTBEventOccurs(mentalStateGiver.mtbDays, 60000f, 60f))
						{
							this.pawn.mindState.mentalStateHandler.TryStartMentalState(mentalStateGiver.mentalState, "MentalStateReason_Hediff".Translate(this.Label), false, false, null, false, false, false);
						}
					}
				}
				if (curStage.mentalBreakMtbDays > 0f && this.pawn.IsHashIntervalTick(60) && !this.pawn.InMentalState && !this.pawn.Downed && Rand.MTBEventOccurs(curStage.mentalBreakMtbDays, 60000f, 60f))
				{
					this.TryDoRandomMentalBreak();
				}
				if (curStage.vomitMtbDays > 0f && this.pawn.IsHashIntervalTick(600) && Rand.MTBEventOccurs(curStage.vomitMtbDays, 60000f, 600f) && this.pawn.Spawned && this.pawn.Awake() && this.pawn.RaceProps.IsFlesh)
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true);
				}
				Thought_Memory th;
				if (curStage.forgetMemoryThoughtMtbDays > 0f && this.pawn.needs != null && this.pawn.needs.mood != null && this.pawn.IsHashIntervalTick(400) && Rand.MTBEventOccurs(curStage.forgetMemoryThoughtMtbDays, 60000f, 400f) && this.pawn.needs.mood.thoughts.memories.Memories.TryRandomElement(out th))
				{
					this.pawn.needs.mood.thoughts.memories.RemoveMemory(th);
				}
				if (!this.recordedTale && curStage.tale != null)
				{
					TaleRecorder.RecordTale(curStage.tale, new object[]
					{
						this.pawn
					});
					this.recordedTale = true;
				}
				if (curStage.destroyPart && this.Part != null && this.Part != this.pawn.RaceProps.body.corePart)
				{
					this.pawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.Part, null, null);
				}
				if (curStage.deathMtbDays > 0f && this.pawn.IsHashIntervalTick(200) && Rand.MTBEventOccurs(curStage.deathMtbDays, 60000f, 200f))
				{
					this.DoMTBDeath();
				}
			}
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0007C510 File Offset: 0x0007A710
		private void DoMTBDeath()
		{
			HediffStage curStage = this.CurStage;
			if (!curStage.mtbDeathDestroysBrain && ModsConfig.BiotechActive)
			{
				Pawn_GeneTracker genes = this.pawn.genes;
				if (genes != null && genes.HasGene(GeneDefOf.Deathless))
				{
					return;
				}
			}
			this.pawn.Kill(null, this);
			if (curStage.mtbDeathDestroysBrain)
			{
				BodyPartRecord brain = this.pawn.health.hediffSet.GetBrain();
				if (brain != null)
				{
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, brain);
					this.pawn.health.AddHediff(hediff, brain, null, null);
				}
			}
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0007C5B8 File Offset: 0x0007A7B8
		private void TryDoRandomMentalBreak()
		{
			HediffStage curStage = this.CurStage;
			if (curStage == null)
			{
				return;
			}
			MentalBreakDef mentalBreakDef;
			if ((from x in DefDatabase<MentalBreakDef>.AllDefsListForReading
			where x.Worker.BreakCanOccur(this.pawn) && (curStage.allowedMentalBreakIntensities == null || curStage.allowedMentalBreakIntensities.Contains(x.intensity))
			select x).TryRandomElementByWeight((MentalBreakDef x) => x.Worker.CommonalityFor(this.pawn, false), out mentalBreakDef))
			{
				TaggedString t = "MentalStateReason_Hediff".Translate(this.Label);
				if (!curStage.mentalBreakExplanation.NullOrEmpty())
				{
					t += "\n\n" + curStage.mentalBreakExplanation.Formatted(this.pawn.Named("PAWN"));
				}
				mentalBreakDef.Worker.TryStart(this.pawn, t.Resolve(), false);
			}
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0007C685 File Offset: 0x0007A885
		public virtual void PostMake()
		{
			this.Severity = Mathf.Max(this.Severity, this.def.initialSeverity);
			this.causesNoPain = (Rand.Value < this.def.chanceToCauseNoPain);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0007C6BC File Offset: 0x0007A8BC
		public virtual void PostAdd(DamageInfo? dinfo)
		{
			if (!this.def.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
			if (!this.def.removeWithTags.NullOrEmpty<string>())
			{
				for (int i = this.pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
				{
					Hediff hediff = this.pawn.health.hediffSet.hediffs[i];
					if (hediff != this && !hediff.def.tags.NullOrEmpty<string>())
					{
						for (int j = 0; j < this.def.removeWithTags.Count; j++)
						{
							if (hediff.def.tags.Contains(this.def.removeWithTags[j]))
							{
								this.pawn.health.RemoveHediff(hediff);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PreRemoved()
		{
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0007C7B0 File Offset: 0x0007A9B0
		public virtual void PostRemoved()
		{
			if ((this.def.causesNeed != null || !this.def.disablesNeeds.NullOrEmpty<NeedDef>()) && !this.pawn.Dead)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostTick()
		{
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0007C7EE File Offset: 0x0007A9EE
		public virtual void Heal(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			this.Severity -= amount;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0007C818 File Offset: 0x0007AA18
		public virtual bool TryMergeWith(Hediff other)
		{
			if (other == null || other.def != this.def || other.Part != this.Part)
			{
				return false;
			}
			this.Severity += other.Severity;
			this.ageTicks = 0;
			return true;
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0007C858 File Offset: 0x0007AA58
		public virtual bool CauseDeathNow()
		{
			if (this.def.lethalSeverity >= 0f)
			{
				bool flag = this.Severity >= this.def.lethalSeverity;
				if (flag && DebugViewSettings.logCauseOfDeath)
				{
					Log.Message(string.Concat(new object[]
					{
						"CauseOfDeath: lethal severity exceeded ",
						this.Severity,
						" >= ",
						this.def.lethalSeverity
					}));
				}
				return flag;
			}
			return false;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo targets)
		{
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
		{
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_IngestedThing(Thing thing, int amount)
		{
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_Resurrected()
		{
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return null;
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x0007C8DC File Offset: 0x0007AADC
		public virtual string Description
		{
			get
			{
				return this.def.Description;
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0007C8EC File Offset: 0x0007AAEC
		public virtual string GetTooltip(Pawn pawn, bool showHediffsDebugInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (string.IsNullOrWhiteSpace(this.def.overrideTooltip))
			{
				HediffStage curStage = this.CurStage;
				if (string.IsNullOrWhiteSpace((curStage != null) ? curStage.overrideTooltip : null))
				{
					string severityLabel = this.SeverityLabel;
					bool flag = showHediffsDebugInfo && !this.DebugString().NullOrEmpty();
					string description = this.Description;
					if (!this.Label.NullOrEmpty() || !severityLabel.NullOrEmpty() || !this.CapMods.NullOrEmpty<PawnCapacityModifier>() || flag || !description.NullOrEmpty())
					{
						stringBuilder.AppendTagged(this.LabelCap.Colorize(ColoredText.TipSectionTitleColor));
						if (!severityLabel.NullOrEmpty())
						{
							stringBuilder.Append(": " + severityLabel);
						}
						if (!description.NullOrEmpty())
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendInNewLine(description);
						}
						stringBuilder.AppendLine();
						string tipStringExtra = this.TipStringExtra;
						if (!tipStringExtra.NullOrEmpty())
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine(tipStringExtra.TrimEndNewlines());
						}
						if (flag)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine(this.DebugString().TrimEndNewlines());
						}
					}
					string text = this.<GetTooltip>g__Cause|93_0();
					if (text != null)
					{
						stringBuilder.AppendLine().AppendTagged(("Cause".Translate() + ": " + text).Colorize(ColoredText.SubtleGrayColor));
					}
				}
				else
				{
					stringBuilder.Append(this.CurStage.overrideTooltip.Formatted(pawn.Named("PAWN"), this.TipStringExtra.Named("TipStringExtra"), this.<GetTooltip>g__Cause|93_0().Named("CAUSE")));
				}
			}
			else
			{
				stringBuilder.Append(this.def.overrideTooltip.Formatted(pawn.Named("PAWN"), this.TipStringExtra.Named("TipStringExtra"), this.<GetTooltip>g__Cause|93_0().Named("CAUSE")));
			}
			if (!string.IsNullOrWhiteSpace(this.def.extraTooltip))
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(this.def.extraTooltip.Formatted(pawn.Named("PAWN"), this.TipStringExtra.Named("TipStringExtra"), this.<GetTooltip>g__Cause|93_0().Named("CAUSE")));
			}
			HediffStage curStage2 = this.CurStage;
			if (!string.IsNullOrWhiteSpace((curStage2 != null) ? curStage2.extraTooltip : null))
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(this.CurStage.extraTooltip.Formatted(pawn.Named("PAWN"), this.TipStringExtra.Named("TipStringExtra"), this.<GetTooltip>g__Cause|93_0().Named("CAUSE")));
			}
			return stringBuilder.ToString().TrimEnd(Array.Empty<char>());
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostDebugAdd()
		{
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0007CBD0 File Offset: 0x0007ADD0
		public virtual string DebugString()
		{
			string text = "";
			if (!this.Visible)
			{
				text += "hidden\n";
			}
			text = text + "severity: " + this.Severity.ToString("F3") + ((this.Severity >= this.def.maxSeverity) ? " (reached max)" : "");
			if (this.TendableNow(false))
			{
				text = text + "\ntend priority: " + this.TendPriority;
			}
			return text.Indented("    ");
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0007CC60 File Offset: 0x0007AE60
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in this.def.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0007CC78 File Offset: 0x0007AE78
		public override string ToString()
		{
			object[] array = new object[6];
			array[0] = "(";
			int num = 1;
			HediffDef hediffDef = this.def;
			array[num] = (((hediffDef != null) ? hediffDef.defName : null) ?? base.GetType().Name);
			array[2] = ((this.part != null) ? (" " + this.part.Label) : "");
			array[3] = " ticksSinceCreation=";
			array[4] = this.ageTicks;
			array[5] = ")";
			return string.Concat(array);
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0007CD01 File Offset: 0x0007AF01
		public string GetUniqueLoadID()
		{
			return "Hediff_" + this.loadID;
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0007CD30 File Offset: 0x0007AF30
		[CompilerGenerated]
		private string <GetTooltip>g__Cause|93_0()
		{
			TaggedString taggedString;
			LogEntry logEntry;
			if (HealthCardUtility.GetCombatLogInfo(Gen.YieldSingle<Hediff>(this), out taggedString, out logEntry))
			{
				return taggedString.Resolve();
			}
			return null;
		}

		// Token: 0x04001095 RID: 4245
		public HediffDef def;

		// Token: 0x04001096 RID: 4246
		public int ageTicks;

		// Token: 0x04001097 RID: 4247
		private BodyPartRecord part;

		// Token: 0x04001098 RID: 4248
		public ThingDef source;

		// Token: 0x04001099 RID: 4249
		public BodyPartGroupDef sourceBodyPartGroup;

		// Token: 0x0400109A RID: 4250
		public HediffDef sourceHediffDef;

		// Token: 0x0400109B RID: 4251
		public int loadID = -1;

		// Token: 0x0400109C RID: 4252
		protected float severityInt;

		// Token: 0x0400109D RID: 4253
		private bool recordedTale;

		// Token: 0x0400109E RID: 4254
		protected bool causesNoPain;

		// Token: 0x0400109F RID: 4255
		private bool visible;

		// Token: 0x040010A0 RID: 4256
		public WeakReference<LogEntry> combatLogEntry;

		// Token: 0x040010A1 RID: 4257
		public string combatLogText;

		// Token: 0x040010A2 RID: 4258
		public int temp_partIndexToSetLater = -1;

		// Token: 0x040010A3 RID: 4259
		[Unsaved(false)]
		public Pawn pawn;
	}
}

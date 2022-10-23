using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000346 RID: 838
	public class Hediff_Labor : HediffWithParents
	{
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001663 RID: 5731 RVA: 0x00083D71 File Offset: 0x00081F71
		public override string LabelBase
		{
			get
			{
				return "Labor".Translate();
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001664 RID: 5732 RVA: 0x00083D84 File Offset: 0x00081F84
		public override string LabelInBrackets
		{
			get
			{
				string labelInBrackets = base.LabelInBrackets;
				if (!labelInBrackets.NullOrEmpty())
				{
					return "LaborDilation".Translate() + ", " + labelInBrackets;
				}
				return "LaborDilation".Translate();
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001665 RID: 5733 RVA: 0x00083DD0 File Offset: 0x00081FD0
		public float Progress
		{
			get
			{
				HediffComp_Disappears hediffComp_Disappears = this.TryGetComp<HediffComp_Disappears>();
				return 1f - (float)(hediffComp_Disappears.ticksToDisappear + this.laborPushingDuration) / (float)Mathf.Max(1, hediffComp_Disappears.disappearsAfterTicks + this.laborPushingDuration);
			}
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00083E0D File Offset: 0x0008200D
		private static int GetRandomLaborPushingDuration()
		{
			return HediffDefOf.PregnancyLaborPushing.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00083E23 File Offset: 0x00082023
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.Severity = this.def.stages.RandomElement<HediffStage>().minSeverity;
			this.laborPushingDuration = Hediff_Labor.GetRandomLaborPushingDuration();
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x00083E52 File Offset: 0x00082052
		private static TargetInfo BestBed(Pawn mother)
		{
			return mother.CurrentBed() ?? RestUtility.FindPatientBedFor(mother);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x00083E69 File Offset: 0x00082069
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Lord lord = this.pawn.GetLord();
			if (lord == null || lord.LordJob == null)
			{
				Precept_Ritual precept_Ritual = (Precept_Ritual)this.pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
				TargetInfo targetInfo = Hediff_Labor.BestBed(this.pawn);
				Command_Ritual command_Ritual = new Command_Ritual(precept_Ritual, targetInfo, null, new Dictionary<string, Pawn>
				{
					{
						"mother",
						this.pawn
					}
				});
				if (!targetInfo.IsValid)
				{
					command_Ritual.disabled = true;
					command_Ritual.disabledReason = "NoAppropriateBedChildBirth".Translate();
				}
				else if (!this.FoundBirthDoctor(precept_Ritual))
				{
					command_Ritual.disabled = true;
					command_Ritual.disabledReason = "NoDoctorChildBirth".Translate();
				}
				yield return command_Ritual;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return PregnancyUtility.BirthQualityGizmo(this.pawn);
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force progress to labor pushing",
					action = delegate()
					{
						this.pawn.health.RemoveHediff(this);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00083E7C File Offset: 0x0008207C
		public override void PreRemoved()
		{
			base.PreRemoved();
			Hediff_LaborPushing hediff_LaborPushing = (Hediff_LaborPushing)this.pawn.health.AddHediff(HediffDefOf.PregnancyLaborPushing, null, null, null);
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageColonistInFinalStagesOfLabor".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			hediff_LaborPushing.SetParents(base.Mother, base.Father, this.geneSet);
			HediffComp_Disappears hediffComp_Disappears = hediff_LaborPushing.TryGetComp<HediffComp_Disappears>();
			hediffComp_Disappears.disappearsAfterTicks = this.laborPushingDuration;
			hediffComp_Disappears.ticksToDisappear = this.laborPushingDuration;
			hediff_LaborPushing.laborDuration = this.TryGetComp<HediffComp_Disappears>().disappearsAfterTicks;
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00083F3C File Offset: 0x0008213C
		private bool FoundBirthDoctor(Precept_Ritual birthRitual)
		{
			if (this.testForDoctor == null)
			{
				this.testForDoctor = birthRitual.behavior.def.roles.First((RitualRole r) => r.id == "doctor");
			}
			foreach (Pawn pawn in this.pawn.MapHeld.mapPawns.FreeColonistsSpawned)
			{
				bool flag;
				if (pawn != this.pawn && RitualRoleAssignments.PawnNotAssignableReason(pawn, this.testForDoctor, birthRitual, null, null, out flag) == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x00084004 File Offset: 0x00082204
		public override void Tick()
		{
			if (this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.progressBar == null)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
				}
				this.progressBar.EffectTick(this.pawn, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
				if (mote != null)
				{
					mote.progress = this.Progress;
				}
			}
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0008407C File Offset: 0x0008227C
		public override void PostRemoved()
		{
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
			base.PostRemoved();
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x0008409C File Offset: 0x0008229C
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x000840BC File Offset: 0x000822BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.laborPushingDuration, "laborPushingDuration", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.laborPushingDuration == -1)
			{
				this.laborPushingDuration = Hediff_Labor.GetRandomLaborPushingDuration();
			}
		}

		// Token: 0x0400118F RID: 4495
		private RitualRole testForDoctor;

		// Token: 0x04001190 RID: 4496
		private int laborPushingDuration;

		// Token: 0x04001191 RID: 4497
		private Effecter progressBar;
	}
}

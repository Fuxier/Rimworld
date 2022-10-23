using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000160 RID: 352
	public class BattleLogEntry_StateTransition : LogEntry
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x0002DAE1 File Offset: 0x0002BCE1
		private string SubjectName
		{
			get
			{
				if (this.subjectPawn == null)
				{
					return "null";
				}
				return this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002C32E File Offset: 0x0002A52E
		public BattleLogEntry_StateTransition() : base(null)
		{
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002DAFC File Offset: 0x0002BCFC
		public BattleLogEntry_StateTransition(Thing subject, RulePackDef transitionDef, Pawn initiator, Hediff culpritHediff, BodyPartRecord culpritTargetDef) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			this.transitionDef = transitionDef;
			this.initiator = initiator;
			if (culpritHediff != null)
			{
				this.culpritHediffDef = culpritHediff.def;
				if (culpritHediff.Part != null)
				{
					this.culpritHediffTargetPart = culpritHediff.Part;
				}
			}
			this.culpritTargetPart = culpritTargetDef;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002DB72 File Offset: 0x0002BD72
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiator;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002DB88 File Offset: 0x0002BD88
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.subjectPawn != null)
			{
				yield return this.subjectPawn;
			}
			yield break;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002DB98 File Offset: 0x0002BD98
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiator)) || (pov == this.initiator && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0002DBD2 File Offset: 0x0002BDD2
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.subjectPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0002DC10 File Offset: 0x0002BE10
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (pov == null || pov == this.subjectPawn)
			{
				if (this.transitionDef != RulePackDefOf.Transition_Downed)
				{
					return LogEntry.Skull;
				}
				return LogEntry.Downed;
			}
			else
			{
				if (pov != this.initiator)
				{
					return null;
				}
				if (this.transitionDef != RulePackDefOf.Transition_Downed)
				{
					return LogEntry.SkullTarget;
				}
				return LogEntry.DownedTarget;
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0002DC68 File Offset: 0x0002BE68
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants, true, true));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			result.Includes.Add(this.transitionDef);
			if (this.initiator != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants, true, true));
			}
			if (this.culpritHediffDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForHediffDef("CULPRITHEDIFF", this.culpritHediffDef, this.culpritHediffTargetPart));
			}
			if (this.culpritHediffTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_target", this.culpritHediffTargetPart));
			}
			if (this.culpritTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_originaltarget", this.culpritTargetPart));
			}
			return result;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0002DD84 File Offset: 0x0002BF84
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.transitionDef, "transitionDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_Defs.Look<HediffDef>(ref this.culpritHediffDef, "culpritHediffDef");
			Scribe_BodyParts.Look(ref this.culpritHediffTargetPart, "culpritHediffTargetPart", null);
			Scribe_BodyParts.Look(ref this.culpritTargetPart, "culpritTargetPart", null);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0002DE0B File Offset: 0x0002C00B
		public override string ToString()
		{
			return this.transitionDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x040009BA RID: 2490
		private RulePackDef transitionDef;

		// Token: 0x040009BB RID: 2491
		private Pawn subjectPawn;

		// Token: 0x040009BC RID: 2492
		private ThingDef subjectThing;

		// Token: 0x040009BD RID: 2493
		private Pawn initiator;

		// Token: 0x040009BE RID: 2494
		private HediffDef culpritHediffDef;

		// Token: 0x040009BF RID: 2495
		private BodyPartRecord culpritHediffTargetPart;

		// Token: 0x040009C0 RID: 2496
		private BodyPartRecord culpritTargetPart;
	}
}

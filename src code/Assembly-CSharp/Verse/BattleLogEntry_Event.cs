using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200015A RID: 346
	public class BattleLogEntry_Event : LogEntry
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0002C313 File Offset: 0x0002A513
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

		// Token: 0x06000910 RID: 2320 RVA: 0x0002C32E File Offset: 0x0002A52E
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002C338 File Offset: 0x0002A538
		public BattleLogEntry_Event(Thing subject, RulePackDef eventDef, Thing initiator) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			this.eventDef = eventDef;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0002C39D File Offset: 0x0002A59D
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002C3B3 File Offset: 0x0002A5B3
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.subjectPawn != null)
			{
				yield return this.subjectPawn;
			}
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			yield break;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002C3C3 File Offset: 0x0002A5C3
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiatorPawn)) || (pov == this.initiatorPawn && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0002C3FD File Offset: 0x0002A5FD
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.subjectPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0002C43C File Offset: 0x0002A63C
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Includes.Add(this.eventDef);
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants, true, true));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, true, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			return result;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0002C50C File Offset: 0x0002A70C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0002C571 File Offset: 0x0002A771
		public override string ToString()
		{
			return this.eventDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x04000990 RID: 2448
		protected RulePackDef eventDef;

		// Token: 0x04000991 RID: 2449
		protected Pawn subjectPawn;

		// Token: 0x04000992 RID: 2450
		protected ThingDef subjectThing;

		// Token: 0x04000993 RID: 2451
		protected Pawn initiatorPawn;

		// Token: 0x04000994 RID: 2452
		protected ThingDef initiatorThing;
	}
}

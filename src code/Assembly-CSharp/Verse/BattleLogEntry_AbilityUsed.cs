using System;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000158 RID: 344
	public class BattleLogEntry_AbilityUsed : BattleLogEntry_Event
	{
		// Token: 0x060008FF RID: 2303 RVA: 0x0002C116 File Offset: 0x0002A316
		public BattleLogEntry_AbilityUsed()
		{
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0002C11E File Offset: 0x0002A31E
		public BattleLogEntry_AbilityUsed(Pawn caster, Thing target, AbilityDef ability, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.abilityUsed = ability;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0002C131 File Offset: 0x0002A331
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<AbilityDef>(ref this.abilityUsed, "abilityUsed");
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0002C14C File Offset: 0x0002A34C
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ABILITY", this.abilityUsed));
			if (this.subjectPawn == null && this.subjectThing == null)
			{
				result.Rules.Add(new Rule_String("SUBJECT_definite", "AreaLower".Translate()));
			}
			return result;
		}

		// Token: 0x0400098C RID: 2444
		public AbilityDef abilityUsed;
	}
}

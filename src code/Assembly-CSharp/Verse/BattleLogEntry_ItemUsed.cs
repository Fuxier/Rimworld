using System;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200015C RID: 348
	public class BattleLogEntry_ItemUsed : BattleLogEntry_Event
	{
		// Token: 0x06000926 RID: 2342 RVA: 0x0002C116 File Offset: 0x0002A316
		public BattleLogEntry_ItemUsed()
		{
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0002C98A File Offset: 0x0002AB8A
		public BattleLogEntry_ItemUsed(Pawn caster, Thing target, ThingDef itemUsed, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.itemUsed = itemUsed;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002C99D File Offset: 0x0002AB9D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.itemUsed, "itemUsed");
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0002C9B8 File Offset: 0x0002ABB8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ITEM", this.itemUsed));
			return result;
		}

		// Token: 0x0400099C RID: 2460
		public ThingDef itemUsed;
	}
}

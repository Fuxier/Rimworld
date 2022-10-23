using System;

namespace Verse
{
	// Token: 0x02000109 RID: 265
	public class ManeuverDef : Def
	{
		// Token: 0x04000677 RID: 1655
		public ToolCapacityDef requiredCapacity;

		// Token: 0x04000678 RID: 1656
		public VerbProperties verb;

		// Token: 0x04000679 RID: 1657
		public RulePackDef combatLogRulesHit;

		// Token: 0x0400067A RID: 1658
		public RulePackDef combatLogRulesDeflect;

		// Token: 0x0400067B RID: 1659
		public RulePackDef combatLogRulesMiss;

		// Token: 0x0400067C RID: 1660
		public RulePackDef combatLogRulesDodge;

		// Token: 0x0400067D RID: 1661
		public LogEntryDef logEntryDef;
	}
}

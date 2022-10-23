using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000328 RID: 808
	public class HediffComp_SkillDecay : HediffComp
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x0600159F RID: 5535 RVA: 0x00080E15 File Offset: 0x0007F015
		public HediffCompProperties_SkillDecay Props
		{
			get
			{
				return (HediffCompProperties_SkillDecay)this.props;
			}
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x00080E24 File Offset: 0x0007F024
		public override void CompPostTick(ref float severityAdjustment)
		{
			Pawn_SkillTracker skills = base.Pawn.skills;
			if (skills == null)
			{
				return;
			}
			for (int i = 0; i < skills.skills.Count; i++)
			{
				SkillRecord skillRecord = skills.skills[i];
				float num = this.parent.Severity * this.Props.decayPerDayPercentageLevelCurve.Evaluate((float)skillRecord.GetLevel(false));
				float num2 = skillRecord.XpRequiredForLevelUp * num / 60000f;
				skillRecord.Learn(-num2, false);
			}
		}
	}
}

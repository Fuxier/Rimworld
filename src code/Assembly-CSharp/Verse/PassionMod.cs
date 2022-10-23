using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200006A RID: 106
	public class PassionMod
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x00003724 File Offset: 0x00001924
		public PassionMod()
		{
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00019976 File Offset: 0x00017B76
		public PassionMod(SkillDef skill, PassionMod.PassionModType modType)
		{
			this.skill = skill;
			this.modType = modType;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001998C File Offset: 0x00017B8C
		public Passion NewPassionFor(SkillRecord skillRecord)
		{
			PassionMod.PassionModType passionModType = this.modType;
			if (passionModType != PassionMod.PassionModType.AddOneLevel)
			{
				if (passionModType == PassionMod.PassionModType.DropAll)
				{
					return Passion.None;
				}
			}
			else
			{
				Passion passion = skillRecord.passion;
				if (passion == Passion.None)
				{
					return Passion.Minor;
				}
				if (passion == Passion.Minor)
				{
					return Passion.Major;
				}
			}
			return skillRecord.passion;
		}

		// Token: 0x040001E5 RID: 485
		public SkillDef skill;

		// Token: 0x040001E6 RID: 486
		public PassionMod.PassionModType modType;

		// Token: 0x02001C98 RID: 7320
		public enum PassionModType
		{
			// Token: 0x040070B3 RID: 28851
			None,
			// Token: 0x040070B4 RID: 28852
			AddOneLevel,
			// Token: 0x040070B5 RID: 28853
			DropAll
		}
	}
}

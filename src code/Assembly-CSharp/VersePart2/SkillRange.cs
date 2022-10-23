using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200011E RID: 286
	public class SkillRange
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x000265DF File Offset: 0x000247DF
		public SkillDef Skill
		{
			get
			{
				return this.skill;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x000265E7 File Offset: 0x000247E7
		public IntRange Range
		{
			get
			{
				return this.range;
			}
		}

		// Token: 0x04000767 RID: 1895
		private SkillDef skill;

		// Token: 0x04000768 RID: 1896
		private IntRange range = IntRange.one;
	}
}

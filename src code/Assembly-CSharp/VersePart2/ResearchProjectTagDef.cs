using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200012B RID: 299
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x060007D6 RID: 2006 RVA: 0x000281D4 File Offset: 0x000263D4
		public int CompletedProjects()
		{
			int num = 0;
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ResearchProjectDef researchProjectDef = allDefsListForReading[i];
				if (researchProjectDef.IsFinished && researchProjectDef.HasTag(this))
				{
					num++;
				}
			}
			return num;
		}
	}
}

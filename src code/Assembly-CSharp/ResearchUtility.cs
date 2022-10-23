using System;
using RimWorld;
using Verse;

// Token: 0x0200000A RID: 10
public static class ResearchUtility
{
	// Token: 0x06000024 RID: 36 RVA: 0x00002788 File Offset: 0x00000988
	public static void ApplyPlayerStartingResearch()
	{
		if (Faction.OfPlayer.def.startingResearchTags != null)
		{
			foreach (ResearchProjectTagDef tag in Faction.OfPlayer.def.startingResearchTags)
			{
				foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
				{
					if (researchProjectDef.HasTag(tag))
					{
						Find.ResearchManager.FinishProject(researchProjectDef, false, null, false);
					}
				}
			}
		}
		if (ModLister.IdeologyInstalled)
		{
			FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
			Ideo ideo;
			if ((ideo = ((ideos != null) ? ideos.PrimaryIdeo : null)) != null)
			{
				foreach (MemeDef memeDef in ideo.memes)
				{
					foreach (ResearchProjectDef proj in memeDef.startingResearchProjects)
					{
						Find.ResearchManager.FinishProject(proj, false, null, false);
					}
				}
			}
		}
		if (Faction.OfPlayer.def.startingTechprintsResearchTags != null)
		{
			foreach (ResearchProjectTagDef tag2 in Faction.OfPlayer.def.startingTechprintsResearchTags)
			{
				foreach (ResearchProjectDef researchProjectDef2 in DefDatabase<ResearchProjectDef>.AllDefs)
				{
					if (researchProjectDef2.HasTag(tag2))
					{
						int techprints = Find.ResearchManager.GetTechprints(researchProjectDef2);
						if (techprints < researchProjectDef2.TechprintCount)
						{
							Find.ResearchManager.AddTechprints(researchProjectDef2, researchProjectDef2.TechprintCount - techprints);
						}
					}
				}
			}
		}
	}
}

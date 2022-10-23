using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

// Token: 0x02000011 RID: 17
public static class ScenarioUtility
{
	// Token: 0x0600005F RID: 95 RVA: 0x00004D2C File Offset: 0x00002F2C
	public static bool AllowsChildSelection(Scenario scenario)
	{
		List<ScenPart> parts = scenario.parts;
		for (int i = 0; i < parts.Count; i++)
		{
			ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs scenPart_ConfigPage_ConfigureStartingPawns_KindDefs;
			if ((scenPart_ConfigPage_ConfigureStartingPawns_KindDefs = (parts[i] as ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs)) != null)
			{
				using (List<PawnKindCount>.Enumerator enumerator = scenPart_ConfigPage_ConfigureStartingPawns_KindDefs.kindCounts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!ScenarioUtility.CanBeChild(enumerator.Current.kindDef))
						{
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00004DB4 File Offset: 0x00002FB4
	private static bool CanBeChild(PawnKindDef kindDef)
	{
		if (!kindDef.apparelRequired.NullOrEmpty<ThingDef>())
		{
			for (int i = 0; i < kindDef.apparelRequired.Count; i++)
			{
				if (kindDef.apparelRequired[i].IsApparel && !kindDef.apparelRequired[i].apparel.developmentalStageFilter.Juvenile())
				{
					return false;
				}
			}
		}
		return true;
	}
}

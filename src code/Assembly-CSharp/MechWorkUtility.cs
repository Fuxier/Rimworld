using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

// Token: 0x0200000E RID: 14
public static class MechWorkUtility
{
	// Token: 0x0600003A RID: 58 RVA: 0x0000411C File Offset: 0x0000231C
	public static IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
	{
		if (!parentDef.race.IsMechanoid || parentDef.race.mechEnabledWorkTypes.NullOrEmpty<WorkTypeDef>())
		{
			yield break;
		}
		TaggedString taggedString = "MechWorkActivitiesExplanation".Translate() + ":\n";
		foreach (WorkTypeDef workTypeDef in from wt in parentDef.race.mechEnabledWorkTypes
		orderby wt.label
		select wt)
		{
			IEnumerable<WorkGiverDef> source = workTypeDef.workGiversByPriority.Where((WorkGiverDef wg) => wg.canBeDoneByMechs);
			if (source.Any<WorkGiverDef>())
			{
				taggedString += "\n - " + workTypeDef.gerundLabel.CapitalizeFirst();
				foreach (WorkGiverDef workGiverDef in source.OrderBy((WorkGiverDef wg) => wg.label))
				{
					taggedString += "\n  - " + workGiverDef.LabelCap;
				}
			}
		}
		yield return new StatDrawEntry(StatCategoryDefOf.PawnWork, "MechWorkActivities".Translate(), (from w in parentDef.race.mechEnabledWorkTypes
		select w.gerundLabel).ToCommaList(true, false).CapitalizeFirst(), taggedString, 502, null, null, false);
		yield return new StatDrawEntry(StatCategoryDefOf.PawnWork, "MechWorkSkill".Translate(), parentDef.race.mechFixedSkillLevel.ToString(), "MechWorkSkillDesc".Translate(), 501, null, null, false);
		yield break;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x0000412C File Offset: 0x0000232C
	public static bool AnyWorkMechCouldDo(RecipeDef recipe)
	{
		IEnumerable<WorkTypeDef> mechEnabledWorkTypes = (from p in DefDatabase<PawnKindDef>.AllDefs
		where p.RaceProps.IsWorkMech
		select p).SelectMany((PawnKindDef p) => p.RaceProps.mechEnabledWorkTypes).Distinct<WorkTypeDef>();
		if (recipe.requiredGiverWorkType != null && !mechEnabledWorkTypes.Contains(recipe.requiredGiverWorkType))
		{
			return false;
		}
		IEnumerable<ThingDef> recipeUsers = recipe.AllRecipeUsers;
		return (from wg in DefDatabase<WorkGiverDef>.AllDefs
		where !wg.fixedBillGiverDefs.NullOrEmpty<ThingDef>() && wg.fixedBillGiverDefs.Intersect(recipeUsers).Any<ThingDef>()
		select wg).Any((WorkGiverDef wg) => mechEnabledWorkTypes.Contains(wg.workType));
	}

	// Token: 0x04000018 RID: 24
	private const int JumpToTargetCheckInverval = 60;
}

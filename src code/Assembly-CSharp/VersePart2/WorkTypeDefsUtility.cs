using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000153 RID: 339
	public static class WorkTypeDefsUtility
	{
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0002B50F File Offset: 0x0002970F
		public static IEnumerable<WorkTypeDef> WorkTypeDefsInPriorityOrder
		{
			get
			{
				return from wt in DefDatabase<WorkTypeDef>.AllDefs
				orderby wt.naturalPriority descending
				select wt;
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002B53C File Offset: 0x0002973C
		public static string LabelTranslated(this WorkTags tags)
		{
			if (tags <= WorkTags.Crafting)
			{
				if (tags <= WorkTags.Social)
				{
					if (tags <= WorkTags.Violent)
					{
						switch (tags)
						{
						case WorkTags.None:
							return "WorkTagNone".Translate();
						case (WorkTags)1:
						case (WorkTags)3:
							break;
						case WorkTags.ManualDumb:
							return "WorkTagManualDumb".Translate();
						case WorkTags.ManualSkilled:
							return "WorkTagManualSkilled".Translate();
						default:
							if (tags == WorkTags.Violent)
							{
								return "WorkTagViolent".Translate();
							}
							break;
						}
					}
					else
					{
						if (tags == WorkTags.Caring)
						{
							return "WorkTagCaring".Translate();
						}
						if (tags == WorkTags.Social)
						{
							return "WorkTagSocial".Translate();
						}
					}
				}
				else if (tags <= WorkTags.Intellectual)
				{
					if (tags == WorkTags.Commoner)
					{
						return "WorkTagCommoner".Translate();
					}
					if (tags == WorkTags.Intellectual)
					{
						return "WorkTagIntellectual".Translate();
					}
				}
				else
				{
					if (tags == WorkTags.Animals)
					{
						return "WorkTagAnimals".Translate();
					}
					if (tags == WorkTags.Artistic)
					{
						return "WorkTagArtistic".Translate();
					}
					if (tags == WorkTags.Crafting)
					{
						return "WorkTagCrafting".Translate();
					}
				}
			}
			else if (tags <= WorkTags.PlantWork)
			{
				if (tags <= WorkTags.Firefighting)
				{
					if (tags == WorkTags.Cooking)
					{
						return "WorkTagCooking".Translate();
					}
					if (tags == WorkTags.Firefighting)
					{
						return "WorkTagFirefighting".Translate();
					}
				}
				else
				{
					if (tags == WorkTags.Cleaning)
					{
						return "WorkTagCleaning".Translate();
					}
					if (tags == WorkTags.Hauling)
					{
						return "WorkTagHauling".Translate();
					}
					if (tags == WorkTags.PlantWork)
					{
						return "WorkTagPlantWork".Translate();
					}
				}
			}
			else if (tags <= WorkTags.Hunting)
			{
				if (tags == WorkTags.Mining)
				{
					return "WorkTagMining".Translate();
				}
				if (tags == WorkTags.Hunting)
				{
					return "WorkTagHunting".Translate();
				}
			}
			else
			{
				if (tags == WorkTags.Constructing)
				{
					return "WorkTagConstructing".Translate();
				}
				if (tags == WorkTags.Shooting)
				{
					return "WorkTagShooting".Translate();
				}
				if (tags == WorkTags.AllWork)
				{
					return "WorkTagAllWork".Translate();
				}
			}
			Log.Error("Unknown or mixed worktags for naming: " + (int)tags);
			return "Worktag";
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002B7E4 File Offset: 0x000299E4
		public static bool ExactlyOneWorkTagSet(this WorkTags workTags)
		{
			return workTags != WorkTags.None && (workTags & workTags - 1) == WorkTags.None;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002B800 File Offset: 0x00029A00
		public static bool OverlapsWithOnAnyWorkType(this WorkTags a, WorkTags b)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if ((workTypeDef.workTags & a) != WorkTags.None && (workTypeDef.workTags & b) != WorkTags.None)
				{
					return true;
				}
			}
			return false;
		}
	}
}

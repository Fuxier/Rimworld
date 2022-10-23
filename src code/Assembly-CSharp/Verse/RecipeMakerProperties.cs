using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000DC RID: 220
	public class RecipeMakerProperties
	{
		// Token: 0x04000493 RID: 1171
		public int productCount = 1;

		// Token: 0x04000494 RID: 1172
		public int targetCountAdjustment = 1;

		// Token: 0x04000495 RID: 1173
		public int bulkRecipeCount = -1;

		// Token: 0x04000496 RID: 1174
		public bool useIngredientsForColor = true;

		// Token: 0x04000497 RID: 1175
		public int workAmount = -1;

		// Token: 0x04000498 RID: 1176
		public StatDef workSpeedStat;

		// Token: 0x04000499 RID: 1177
		public StatDef efficiencyStat;

		// Token: 0x0400049A RID: 1178
		public ThingDef unfinishedThingDef;

		// Token: 0x0400049B RID: 1179
		public ThingFilter defaultIngredientFilter;

		// Token: 0x0400049C RID: 1180
		public List<SkillRequirement> skillRequirements;

		// Token: 0x0400049D RID: 1181
		public SkillDef workSkill;

		// Token: 0x0400049E RID: 1182
		public float workSkillLearnPerTick = 1f;

		// Token: 0x0400049F RID: 1183
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x040004A0 RID: 1184
		public EffecterDef effectWorking;

		// Token: 0x040004A1 RID: 1185
		public SoundDef soundWorking;

		// Token: 0x040004A2 RID: 1186
		public List<ThingDef> recipeUsers;

		// Token: 0x040004A3 RID: 1187
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x040004A4 RID: 1188
		public List<MemeDef> memePrerequisitesAny;

		// Token: 0x040004A5 RID: 1189
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040004A6 RID: 1190
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x040004A7 RID: 1191
		public bool mechanitorOnlyRecipe;

		// Token: 0x040004A8 RID: 1192
		public bool fromIdeoBuildingPreceptOnly;

		// Token: 0x040004A9 RID: 1193
		public int displayPriority = 99999;
	}
}

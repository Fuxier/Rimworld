using System;

namespace Verse
{
	// Token: 0x0200011C RID: 284
	public class PawnKindLifeStage
	{
		// Token: 0x0600075F RID: 1887 RVA: 0x000263E2 File Offset: 0x000245E2
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelMale = this.labelMale;
			this.untranslatedLabelFemale = this.labelFemale;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00026408 File Offset: 0x00024608
		public void ResolveReferences()
		{
			if (this.bodyGraphicData != null && this.bodyGraphicData.graphicClass == null)
			{
				this.bodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleGraphicData != null && this.femaleGraphicData.graphicClass == null)
			{
				this.femaleGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.dessicatedBodyGraphicData != null && this.dessicatedBodyGraphicData.graphicClass == null)
			{
				this.dessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleDessicatedBodyGraphicData != null && this.femaleDessicatedBodyGraphicData.graphicClass == null)
			{
				this.femaleDessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.corpseGraphicData != null && this.corpseGraphicData.graphicClass == null)
			{
				this.corpseGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleCorpseGraphicData != null && this.femaleCorpseGraphicData.graphicClass == null)
			{
				this.femaleCorpseGraphicData.graphicClass = typeof(Graphic_Multi);
			}
		}

		// Token: 0x0400074B RID: 1867
		[MustTranslate]
		public string label;

		// Token: 0x0400074C RID: 1868
		[MustTranslate]
		public string labelPlural;

		// Token: 0x0400074D RID: 1869
		[MustTranslate]
		public string labelMale;

		// Token: 0x0400074E RID: 1870
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x0400074F RID: 1871
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04000750 RID: 1872
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x04000751 RID: 1873
		[Unsaved(false)]
		[TranslationHandle(Priority = 200)]
		public string untranslatedLabel;

		// Token: 0x04000752 RID: 1874
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabelMale;

		// Token: 0x04000753 RID: 1875
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelFemale;

		// Token: 0x04000754 RID: 1876
		public GraphicData bodyGraphicData;

		// Token: 0x04000755 RID: 1877
		public GraphicData femaleGraphicData;

		// Token: 0x04000756 RID: 1878
		public GraphicData dessicatedBodyGraphicData;

		// Token: 0x04000757 RID: 1879
		public GraphicData femaleDessicatedBodyGraphicData;

		// Token: 0x04000758 RID: 1880
		public GraphicData corpseGraphicData;

		// Token: 0x04000759 RID: 1881
		public GraphicData femaleCorpseGraphicData;

		// Token: 0x0400075A RID: 1882
		public BodyPartToDrop butcherBodyPart;
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E6 RID: 230
	public class StuffProperties
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00023714 File Offset: 0x00021914
		public ThingDef SourceNaturalRock
		{
			get
			{
				if (!this.sourceNaturalRockCached)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					List<RecipeDef> allDefsListForReading2 = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].category == ThingCategory.Building && allDefsListForReading[i].building.isNaturalRock && allDefsListForReading[i].building.mineableThing != null && !allDefsListForReading[i].IsSmoothed)
						{
							if (allDefsListForReading[i].building.mineableThing == this.parent)
							{
								this.sourceNaturalRockCachedValue = allDefsListForReading[i];
								break;
							}
							for (int j = 0; j < allDefsListForReading2.Count; j++)
							{
								if (allDefsListForReading2[j].IsIngredient(allDefsListForReading[i].building.mineableThing))
								{
									bool flag = false;
									for (int k = 0; k < allDefsListForReading2[j].products.Count; k++)
									{
										if (allDefsListForReading2[j].products[k].thingDef == this.parent)
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										this.sourceNaturalRockCachedValue = allDefsListForReading[i];
										break;
									}
								}
							}
						}
					}
					this.sourceNaturalRockCached = true;
				}
				return this.sourceNaturalRockCachedValue;
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00023864 File Offset: 0x00021A64
		public bool CanMake(BuildableDef t)
		{
			if (!t.MadeFromStuff)
			{
				return false;
			}
			for (int i = 0; i < t.stuffCategories.Count; i++)
			{
				for (int j = 0; j < this.categories.Count; j++)
				{
					if (t.stuffCategories[i] == this.categories[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x000238C4 File Offset: 0x00021AC4
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}

		// Token: 0x040004D6 RID: 1238
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x040004D7 RID: 1239
		public string stuffAdjective;

		// Token: 0x040004D8 RID: 1240
		public float commonality = 1f;

		// Token: 0x040004D9 RID: 1241
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		// Token: 0x040004DA RID: 1242
		public List<StatModifier> statOffsets;

		// Token: 0x040004DB RID: 1243
		public List<StatModifier> statFactors;

		// Token: 0x040004DC RID: 1244
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x040004DD RID: 1245
		public EffecterDef constructEffect;

		// Token: 0x040004DE RID: 1246
		public StuffAppearanceDef appearance;

		// Token: 0x040004DF RID: 1247
		public bool allowColorGenerators;

		// Token: 0x040004E0 RID: 1248
		public bool canSuggestUseDefaultStuff;

		// Token: 0x040004E1 RID: 1249
		public SoundDef soundImpactStuff;

		// Token: 0x040004E2 RID: 1250
		public SoundDef soundMeleeHitSharp;

		// Token: 0x040004E3 RID: 1251
		public SoundDef soundMeleeHitBlunt;

		// Token: 0x040004E4 RID: 1252
		[Unsaved(false)]
		private bool sourceNaturalRockCached;

		// Token: 0x040004E5 RID: 1253
		[Unsaved(false)]
		private ThingDef sourceNaturalRockCachedValue;
	}
}

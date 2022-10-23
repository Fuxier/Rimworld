using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200013D RID: 317
	public class StyleCategoryDef : Def
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x00028BF8 File Offset: 0x00026DF8
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					if (this.iconPath.NullOrEmpty())
					{
						this.cachedIcon = BaseContent.BadTex;
					}
					else
					{
						this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00028C48 File Offset: 0x00026E48
		public List<BuildableDef> AllDesignatorBuildables
		{
			get
			{
				if (this.cachedAllDesignatorBuildables == null)
				{
					this.cachedAllDesignatorBuildables = new List<BuildableDef>();
					if (this.addDesignators != null)
					{
						foreach (BuildableDef item in this.addDesignators)
						{
							this.cachedAllDesignatorBuildables.Add(item);
						}
					}
					if (this.addDesignatorGroups != null)
					{
						foreach (DesignatorDropdownGroupDef designatorDropdownGroupDef in this.addDesignatorGroups)
						{
							this.cachedAllDesignatorBuildables.AddRange(designatorDropdownGroupDef.BuildablesWithoutDefaultDesignators());
						}
					}
				}
				return this.cachedAllDesignatorBuildables;
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00028D18 File Offset: 0x00026F18
		public ThingStyleDef GetStyleForThingDef(BuildableDef thingDef, Precept precept = null)
		{
			ThingStyleDef result;
			try
			{
				for (int i = 0; i < this.thingDefStyles.Count; i++)
				{
					if (this.thingDefStyles[i].ThingDef == thingDef)
					{
						StyleCategoryDef.tmpAvailableStyles.Add(this.thingDefStyles[i].StyleDef);
					}
				}
				if (StyleCategoryDef.tmpAvailableStyles.Count == 0)
				{
					result = null;
				}
				else if (StyleCategoryDef.tmpAvailableStyles.Count == 1 || precept == null)
				{
					result = StyleCategoryDef.tmpAvailableStyles[0];
				}
				else
				{
					result = StyleCategoryDef.tmpAvailableStyles[Rand.RangeSeeded(0, StyleCategoryDef.tmpAvailableStyles.Count, precept.randomSeed)];
				}
			}
			finally
			{
				StyleCategoryDef.tmpAvailableStyles.Clear();
			}
			return result;
		}

		// Token: 0x04000829 RID: 2089
		public List<ThingDefStyle> thingDefStyles;

		// Token: 0x0400082A RID: 2090
		[NoTranslate]
		public string iconPath;

		// Token: 0x0400082B RID: 2091
		public List<BuildableDef> addDesignators;

		// Token: 0x0400082C RID: 2092
		public List<DesignatorDropdownGroupDef> addDesignatorGroups;

		// Token: 0x0400082D RID: 2093
		public SoundDef soundOngoingRitual;

		// Token: 0x0400082E RID: 2094
		public RitualVisualEffectDef ritualVisualEffectDef;

		// Token: 0x0400082F RID: 2095
		private Texture2D cachedIcon;

		// Token: 0x04000830 RID: 2096
		private List<BuildableDef> cachedAllDesignatorBuildables;

		// Token: 0x04000831 RID: 2097
		private static List<ThingStyleDef> tmpAvailableStyles = new List<ThingStyleDef>();
	}
}

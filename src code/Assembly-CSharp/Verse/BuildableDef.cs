using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B4 RID: 180
	public abstract class BuildableDef : Def
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0000831C File Offset: 0x0000651C
		public virtual IntVec2 Size
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001FE44 File Offset: 0x0001E044
		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty<StuffCategoryDef>();
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0001FE54 File Offset: 0x0001E054
		public bool BuildableByPlayer
		{
			get
			{
				return this.designationCategory != null;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001FE5F File Offset: 0x0001E05F
		public Material DrawMatSingle
		{
			get
			{
				if (this.graphic == null)
				{
					return null;
				}
				return this.graphic.MatSingle;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0001FE76 File Offset: 0x0001E076
		public float Altitude
		{
			get
			{
				return this.altitudeLayer.AltitudeFor();
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001FE83 File Offset: 0x0001E083
		public bool AffectsFertility
		{
			get
			{
				return this.fertility >= 0f;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001FE98 File Offset: 0x0001E098
		public List<PlaceWorker> PlaceWorkers
		{
			get
			{
				if (this.placeWorkers == null)
				{
					return null;
				}
				if (this.placeWorkersInstantiatedInt == null)
				{
					this.placeWorkersInstantiatedInt = new List<PlaceWorker>();
					foreach (Type type in this.placeWorkers)
					{
						this.placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(type));
					}
				}
				return this.placeWorkersInstantiatedInt;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001FF20 File Offset: 0x0001E120
		public bool IsResearchFinished
		{
			get
			{
				if (this.researchPrerequisites != null)
				{
					for (int i = 0; i < this.researchPrerequisites.Count; i++)
					{
						if (!this.researchPrerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001FF61 File Offset: 0x0001E161
		public List<ThingDefCountClass> CostList
		{
			get
			{
				if (this.costListForDifficulty != null && this.costListForDifficulty.Applies)
				{
					return this.costListForDifficulty.costList;
				}
				return this.costList;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001FF8A File Offset: 0x0001E18A
		public int CostStuffCount
		{
			get
			{
				if (this.costListForDifficulty != null && this.costListForDifficulty.Applies)
				{
					return this.costListForDifficulty.costStuffCount;
				}
				return this.costStuffCount;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001FFB4 File Offset: 0x0001E1B4
		public bool ForceAllowPlaceOver(BuildableDef other)
		{
			if (this.PlaceWorkers == null)
			{
				return false;
			}
			for (int i = 0; i < this.PlaceWorkers.Count; i++)
			{
				if (this.PlaceWorkers[i].ForceAllowPlaceOver(other))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001FFF8 File Offset: 0x0001E1F8
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
				else
				{
					this.ResolveIcon();
				}
				if (this.uiIconPathsStuff != null)
				{
					this.stuffUiIcons = new Dictionary<StuffAppearanceDef, Texture2D>();
					foreach (IconForStuffAppearance iconForStuffAppearance in this.uiIconPathsStuff)
					{
						this.stuffUiIcons.Add(iconForStuffAppearance.Appearance, ContentFinder<Texture2D>.Get(iconForStuffAppearance.IconPath, true));
					}
				}
			});
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00020014 File Offset: 0x0001E214
		protected virtual void ResolveIcon()
		{
			if (this.graphic != null && this.graphic != BaseContent.BadGraphic)
			{
				ThingDef thingDef;
				if ((thingDef = (this as ThingDef)) != null && thingDef.mote != null)
				{
					return;
				}
				Graphic outerGraphic = this.graphic;
				if (this.uiIconForStackCount >= 1 && this is ThingDef)
				{
					Graphic_StackCount graphic_StackCount = this.graphic as Graphic_StackCount;
					if (graphic_StackCount != null)
					{
						outerGraphic = graphic_StackCount.SubGraphicForStackCount(this.uiIconForStackCount, (ThingDef)this);
					}
				}
				Material material = outerGraphic.ExtractInnerGraphicFor(null, null).MatAt(this.defaultPlacingRot, null);
				this.uiIcon = (Texture2D)material.mainTexture;
				this.uiIconColor = material.color;
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000200C4 File Offset: 0x0001E2C4
		public Texture2D GetUIIconForStuff(ThingDef stuff)
		{
			Texture2D result;
			if (this.stuffUiIcons == null || stuff == null || stuff.stuffProps.appearance == null || !this.stuffUiIcons.TryGetValue(stuff.stuffProps.appearance, out result))
			{
				return this.uiIcon;
			}
			return result;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0002010C File Offset: 0x0001E30C
		public Color GetColorForStuff(ThingDef stuff)
		{
			if (this.colorPerStuff.NullOrEmpty<ColorForStuff>())
			{
				return stuff.stuffProps.color;
			}
			for (int i = 0; i < this.colorPerStuff.Count; i++)
			{
				ColorForStuff colorForStuff = this.colorPerStuff[i];
				if (colorForStuff.Stuff == stuff)
				{
					return colorForStuff.Color;
				}
			}
			return stuff.stuffProps.color;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00020170 File Offset: 0x0001E370
		public override void ResolveReferences()
		{
			base.ResolveReferences();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00020178 File Offset: 0x0001E378
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.useStuffTerrainAffordance && !this.MadeFromStuff)
			{
				yield return "useStuffTerrainAffordance is true but it's not made from stuff";
			}
			if (this.costListForDifficulty != null && this.costListForDifficulty.difficultyVar.NullOrEmpty())
			{
				yield return "costListForDifficulty is not referencing a difficulty.";
			}
			yield break;
			yield break;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00020188 File Offset: 0x0001E388
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.BuildableByPlayer)
			{
				IEnumerable<TerrainAffordanceDef> enumerable = Enumerable.Empty<TerrainAffordanceDef>();
				if (this.PlaceWorkers != null)
				{
					enumerable = enumerable.Concat(this.PlaceWorkers.SelectMany((PlaceWorker pw) => pw.DisplayAffordances()));
				}
				TerrainAffordanceDef terrainAffordanceNeed = this.GetTerrainAffordanceNeed(req.StuffDef);
				if (terrainAffordanceNeed != null)
				{
					enumerable = enumerable.Concat(terrainAffordanceNeed);
				}
				string[] array = (from ta in enumerable.Distinct<TerrainAffordanceDef>()
				orderby ta.order
				select ta.label).ToArray<string>();
				if (array.Length != 0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "TerrainRequirement".Translate(), array.ToCommaList(false, false).CapitalizeFirst(), "Stat_Thing_TerrainRequirement_Desc".Translate(), 1101, null, null, false);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001F511 File Offset: 0x0001D711
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001F519 File Offset: 0x0001D719
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x040002F2 RID: 754
		public List<StatModifier> statBases;

		// Token: 0x040002F3 RID: 755
		public Traversability passability;

		// Token: 0x040002F4 RID: 756
		public int pathCost;

		// Token: 0x040002F5 RID: 757
		public bool pathCostIgnoreRepeat = true;

		// Token: 0x040002F6 RID: 758
		public float fertility = -1f;

		// Token: 0x040002F7 RID: 759
		public List<ThingDefCountClass> costList;

		// Token: 0x040002F8 RID: 760
		public int costStuffCount;

		// Token: 0x040002F9 RID: 761
		public List<StuffCategoryDef> stuffCategories;

		// Token: 0x040002FA RID: 762
		[MustTranslate]
		public string stuffCategorySummary;

		// Token: 0x040002FB RID: 763
		public CostListForDifficulty costListForDifficulty;

		// Token: 0x040002FC RID: 764
		public int placingDraggableDimensions;

		// Token: 0x040002FD RID: 765
		public bool clearBuildingArea = true;

		// Token: 0x040002FE RID: 766
		public Rot4 defaultPlacingRot = Rot4.North;

		// Token: 0x040002FF RID: 767
		public float resourcesFractionWhenDeconstructed = 0.5f;

		// Token: 0x04000300 RID: 768
		public List<AltitudeLayer> blocksAltitudes;

		// Token: 0x04000301 RID: 769
		public StyleCategoryDef dominantStyleCategory;

		// Token: 0x04000302 RID: 770
		public bool isAltar;

		// Token: 0x04000303 RID: 771
		public bool useStuffTerrainAffordance;

		// Token: 0x04000304 RID: 772
		public TerrainAffordanceDef terrainAffordanceNeeded;

		// Token: 0x04000305 RID: 773
		public List<ThingDef> buildingPrerequisites;

		// Token: 0x04000306 RID: 774
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04000307 RID: 775
		public int constructionSkillPrerequisite;

		// Token: 0x04000308 RID: 776
		public int artisticSkillPrerequisite;

		// Token: 0x04000309 RID: 777
		public TechLevel minTechLevelToBuild;

		// Token: 0x0400030A RID: 778
		public TechLevel maxTechLevelToBuild;

		// Token: 0x0400030B RID: 779
		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		// Token: 0x0400030C RID: 780
		public EffecterDef repairEffect;

		// Token: 0x0400030D RID: 781
		public EffecterDef constructEffect;

		// Token: 0x0400030E RID: 782
		public List<ColorForStuff> colorPerStuff;

		// Token: 0x0400030F RID: 783
		public bool canGenerateDefaultDesignator = true;

		// Token: 0x04000310 RID: 784
		public bool ideoBuilding;

		// Token: 0x04000311 RID: 785
		public float specialDisplayRadius;

		// Token: 0x04000312 RID: 786
		public List<Type> placeWorkers;

		// Token: 0x04000313 RID: 787
		public DesignationCategoryDef designationCategory;

		// Token: 0x04000314 RID: 788
		public DesignatorDropdownGroupDef designatorDropdown;

		// Token: 0x04000315 RID: 789
		public KeyBindingDef designationHotKey;

		// Token: 0x04000316 RID: 790
		public float uiOrder = 2999f;

		// Token: 0x04000317 RID: 791
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x04000318 RID: 792
		public List<IconForStuffAppearance> uiIconPathsStuff;

		// Token: 0x04000319 RID: 793
		public Vector2 uiIconOffset;

		// Token: 0x0400031A RID: 794
		public Color uiIconColor = Color.white;

		// Token: 0x0400031B RID: 795
		public int uiIconForStackCount = -1;

		// Token: 0x0400031C RID: 796
		[Unsaved(false)]
		public ThingDef blueprintDef;

		// Token: 0x0400031D RID: 797
		[Unsaved(false)]
		public ThingDef installBlueprintDef;

		// Token: 0x0400031E RID: 798
		[Unsaved(false)]
		public ThingDef frameDef;

		// Token: 0x0400031F RID: 799
		[Unsaved(false)]
		private List<PlaceWorker> placeWorkersInstantiatedInt;

		// Token: 0x04000320 RID: 800
		[Unsaved(false)]
		public Graphic graphic = BaseContent.BadGraphic;

		// Token: 0x04000321 RID: 801
		[Unsaved(false)]
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x04000322 RID: 802
		[Unsaved(false)]
		public Dictionary<StuffAppearanceDef, Texture2D> stuffUiIcons;

		// Token: 0x04000323 RID: 803
		[Unsaved(false)]
		public float uiIconAngle;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020000EC RID: 236
	public class DesignationCategoryDef : Def
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x000245DC File Offset: 0x000227DC
		public IEnumerable<Designator> ResolvedAllowedDesignators
		{
			get
			{
				GameRules rules = Current.Game.Rules;
				int num;
				for (int i = 0; i < this.resolvedDesignators.Count; i = num + 1)
				{
					Designator designator = this.resolvedDesignators[i];
					if (rules == null || rules.DesignatorAllowed(designator))
					{
						yield return designator;
					}
					num = i;
				}
				foreach (Designator designator2 in this.AllIdeoDesignators)
				{
					yield return designator2;
				}
				IEnumerator<Designator> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x000245EC File Offset: 0x000227EC
		public List<Designator> AllResolvedDesignators
		{
			get
			{
				return this.resolvedDesignators;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x000245F4 File Offset: 0x000227F4
		public IEnumerable<Designator> AllIdeoDesignators
		{
			get
			{
				if (ModsConfig.IdeologyActive)
				{
					if (this.cachedPlayerFaction != Faction.OfPlayer)
					{
						this.ideoBuildingDesignatorsCached.Clear();
						this.ideoDropdownsCached.Clear();
						this.cachedPlayerFaction = Faction.OfPlayer;
					}
					foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
					{
						int num;
						for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
						{
							Precept precept = ideo.PreceptsListForReading[i];
							bool flag = precept is Precept_Building;
							bool flag2 = precept is Precept_RitualSeat;
							if (flag || flag2)
							{
								Precept_ThingDef precept_ThingDef = (Precept_ThingDef)precept;
								if (precept_ThingDef.ThingDef.designationCategory == this)
								{
									yield return this.<get_AllIdeoDesignators>g__GetCachedDesignator|17_0(precept_ThingDef.ThingDef, precept_ThingDef as Precept_Building);
								}
							}
							num = i;
						}
						for (int i = 0; i < ideo.thingStyleCategories.Count; i = num + 1)
						{
							ThingStyleCategoryWithPriority thingStyleCategoryWithPriority = ideo.thingStyleCategories[i];
							foreach (Designator designator in this.<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|17_2(thingStyleCategoryWithPriority.category))
							{
								yield return designator;
							}
							IEnumerator<Designator> enumerator2 = null;
							num = i;
						}
						for (int i = 0; i < ideo.memes.Count; i = num + 1)
						{
							MemeDef meme = ideo.memes[i];
							if (meme.addDesignators != null)
							{
								for (int j = 0; j < meme.addDesignators.Count; j = num + 1)
								{
									if (meme.addDesignators[j].designationCategory == this)
									{
										yield return this.<get_AllIdeoDesignators>g__GetCachedDesignator|17_0(meme.addDesignators[j], null);
									}
									num = j;
								}
							}
							if (meme.addDesignatorGroups != null)
							{
								for (int j = 0; j < meme.addDesignatorGroups.Count; j = num + 1)
								{
									Designator_Dropdown designator_Dropdown = this.<get_AllIdeoDesignators>g__GetCachedDropdown|17_1(meme.addDesignatorGroups[j]);
									if (designator_Dropdown != null)
									{
										yield return designator_Dropdown;
									}
									num = j;
								}
							}
							meme = null;
							num = i;
						}
						ideo = null;
					}
					IEnumerator<Ideo> enumerator = null;
					if (Find.IdeoManager.classicMode)
					{
						foreach (StyleCategoryDef categoryDef in Find.IdeoManager.selectedStyleCategories)
						{
							foreach (Designator designator2 in this.<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|17_2(categoryDef))
							{
								yield return designator2;
							}
							IEnumerator<Designator> enumerator2 = null;
						}
						List<StyleCategoryDef>.Enumerator enumerator3 = default(List<StyleCategoryDef>.Enumerator);
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00024604 File Offset: 0x00022804
		public IEnumerable<Designator> AllResolvedAndIdeoDesignators
		{
			get
			{
				foreach (Designator designator in this.resolvedDesignators)
				{
					yield return designator;
				}
				List<Designator>.Enumerator enumerator = default(List<Designator>.Enumerator);
				foreach (Designator designator2 in this.AllIdeoDesignators)
				{
					yield return designator2;
				}
				IEnumerator<Designator> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00024614 File Offset: 0x00022814
		public bool Visible
		{
			get
			{
				if (DebugSettings.godMode)
				{
					return true;
				}
				if (this.researchPrerequisites == null)
				{
					return true;
				}
				using (List<ResearchProjectDef>.Enumerator enumerator = this.researchPrerequisites.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00024680 File Offset: 0x00022880
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ResolveDesignators();
			});
			this.cachedHighlightClosedTag = "DesignationCategoryButton-" + this.defName + "-Closed";
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000246B4 File Offset: 0x000228B4
		private void ResolveDesignators()
		{
			this.resolvedDesignators.Clear();
			foreach (Type type in this.specialDesignatorClasses)
			{
				Designator designator = null;
				try
				{
					designator = (Designator)Activator.CreateInstance(type);
					designator.isOrder = true;
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"DesignationCategoryDef",
						this.defName,
						" could not instantiate special designator from class ",
						type,
						".\n Exception: \n",
						ex.ToString()
					}));
				}
				if (designator != null)
				{
					this.resolvedDesignators.Add(designator);
				}
			}
			IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
			where tDef.designationCategory == this && tDef.canGenerateDefaultDesignator
			select tDef;
			Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> dictionary = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();
			foreach (BuildableDef buildableDef in enumerable)
			{
				if (buildableDef.designatorDropdown != null)
				{
					if (!dictionary.ContainsKey(buildableDef.designatorDropdown))
					{
						dictionary[buildableDef.designatorDropdown] = new Designator_Dropdown();
						dictionary[buildableDef.designatorDropdown].Order = buildableDef.uiOrder;
						this.resolvedDesignators.Add(dictionary[buildableDef.designatorDropdown]);
					}
					dictionary[buildableDef.designatorDropdown].Add(new Designator_Build(buildableDef));
				}
				else
				{
					this.resolvedDesignators.Add(new Designator_Build(buildableDef));
				}
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00024874 File Offset: 0x00022A74
		public void DirtyCache()
		{
			this.ideoBuildingDesignatorsCached.Clear();
			this.ideoDropdownsCached.Clear();
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002488C File Offset: 0x00022A8C
		private static bool RequirementSatisfied(string requirement)
		{
			if (!ModLister.HasActiveModWithName(requirement))
			{
				ResearchProjectDef namedSilentFail = DefDatabase<ResearchProjectDef>.GetNamedSilentFail(requirement);
				return namedSilentFail != null && namedSilentFail.IsFinished;
			}
			return true;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000248E0 File Offset: 0x00022AE0
		[CompilerGenerated]
		private Designator <get_AllIdeoDesignators>g__GetCachedDesignator|17_0(BuildableDef def, Precept_Building buildingPrecept)
		{
			DesignationCategoryDef.BuildablePreceptBuilding key = new DesignationCategoryDef.BuildablePreceptBuilding(def, buildingPrecept);
			Designator designator;
			if (!this.ideoBuildingDesignatorsCached.TryGetValue(key, out designator))
			{
				Designator_Build designator_Build = new Designator_Build(def);
				designator = designator_Build;
				if (buildingPrecept != null)
				{
					designator_Build.sourcePrecept = buildingPrecept;
				}
				this.ideoBuildingDesignatorsCached[key] = designator;
			}
			return designator;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00024928 File Offset: 0x00022B28
		[CompilerGenerated]
		private Designator_Dropdown <get_AllIdeoDesignators>g__GetCachedDropdown|17_1(DesignatorDropdownGroupDef group)
		{
			Designator_Dropdown result;
			if (!this.ideoDropdownsCached.TryGetValue(group, out result))
			{
				IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
				where tDef.designationCategory == this && !tDef.canGenerateDefaultDesignator && tDef.designatorDropdown == @group
				select tDef;
				if (!enumerable.Any<BuildableDef>())
				{
					this.ideoDropdownsCached[group] = null;
					return this.ideoDropdownsCached[group];
				}
				foreach (BuildableDef buildableDef in enumerable)
				{
					if (!this.ideoDropdownsCached.ContainsKey(buildableDef.designatorDropdown))
					{
						this.ideoDropdownsCached[buildableDef.designatorDropdown] = new Designator_Dropdown();
						this.ideoDropdownsCached[buildableDef.designatorDropdown].Order = buildableDef.uiOrder;
					}
					this.ideoDropdownsCached[buildableDef.designatorDropdown].Add(new Designator_Build(buildableDef));
				}
				result = this.ideoDropdownsCached[group];
			}
			return result;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00024A68 File Offset: 0x00022C68
		[CompilerGenerated]
		private IEnumerable<Designator> <get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|17_2(StyleCategoryDef categoryDef)
		{
			if (categoryDef.addDesignators != null)
			{
				int num;
				for (int i = 0; i < categoryDef.addDesignators.Count; i = num + 1)
				{
					if (categoryDef.addDesignators[i].designationCategory == this)
					{
						yield return this.<get_AllIdeoDesignators>g__GetCachedDesignator|17_0(categoryDef.addDesignators[i], null);
					}
					num = i;
				}
			}
			if (categoryDef.addDesignatorGroups != null)
			{
				int num;
				for (int i = 0; i < categoryDef.addDesignatorGroups.Count; i = num + 1)
				{
					Designator_Dropdown designator_Dropdown = this.<get_AllIdeoDesignators>g__GetCachedDropdown|17_1(categoryDef.addDesignatorGroups[i]);
					if (designator_Dropdown != null)
					{
						yield return designator_Dropdown;
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04000559 RID: 1369
		public List<Type> specialDesignatorClasses = new List<Type>();

		// Token: 0x0400055A RID: 1370
		public int order;

		// Token: 0x0400055B RID: 1371
		public bool showPowerGrid;

		// Token: 0x0400055C RID: 1372
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x0400055D RID: 1373
		public int? preferredColumn;

		// Token: 0x0400055E RID: 1374
		[Unsaved(false)]
		private List<Designator> resolvedDesignators = new List<Designator>();

		// Token: 0x0400055F RID: 1375
		[Unsaved(false)]
		public KeyBindingCategoryDef bindingCatDef;

		// Token: 0x04000560 RID: 1376
		[Unsaved(false)]
		public string cachedHighlightClosedTag;

		// Token: 0x04000561 RID: 1377
		[Unsaved(false)]
		private Dictionary<DesignationCategoryDef.BuildablePreceptBuilding, Designator> ideoBuildingDesignatorsCached = new Dictionary<DesignationCategoryDef.BuildablePreceptBuilding, Designator>();

		// Token: 0x04000562 RID: 1378
		[Unsaved(false)]
		private Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> ideoDropdownsCached = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();

		// Token: 0x04000563 RID: 1379
		[Unsaved(false)]
		private Faction cachedPlayerFaction;

		// Token: 0x02001CC8 RID: 7368
		public struct BuildablePreceptBuilding
		{
			// Token: 0x17001DC4 RID: 7620
			// (get) Token: 0x0600B0FE RID: 45310 RVA: 0x00402EEB File Offset: 0x004010EB
			public BuildableDef Buildable
			{
				get
				{
					return this.buildable;
				}
			}

			// Token: 0x17001DC5 RID: 7621
			// (get) Token: 0x0600B0FF RID: 45311 RVA: 0x00402EF3 File Offset: 0x004010F3
			public Precept_Building Precept
			{
				get
				{
					return this.precept;
				}
			}

			// Token: 0x0600B100 RID: 45312 RVA: 0x00402EFB File Offset: 0x004010FB
			public BuildablePreceptBuilding(BuildableDef buildable, Precept_Building precept)
			{
				this.buildable = buildable;
				this.precept = precept;
			}

			// Token: 0x0400717D RID: 29053
			private BuildableDef buildable;

			// Token: 0x0400717E RID: 29054
			private Precept_Building precept;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200012A RID: 298
	public class ResearchProjectDef : Def
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0002748E File Offset: 0x0002568E
		public float ResearchViewX
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00027496 File Offset: 0x00025696
		public float ResearchViewY
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x0002749E File Offset: 0x0002569E
		public float CostApparent
		{
			get
			{
				return this.baseCost * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x000274BC File Offset: 0x000256BC
		public float ProgressReal
		{
			get
			{
				return Find.ResearchManager.GetProgress(this);
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000274C9 File Offset: 0x000256C9
		public float ProgressApparent
		{
			get
			{
				return this.ProgressReal * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000274E7 File Offset: 0x000256E7
		public float ProgressPercent
		{
			get
			{
				return Find.ResearchManager.GetProgress(this) / this.baseCost;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x000274FB File Offset: 0x000256FB
		public bool IsFinished
		{
			get
			{
				return this.ProgressReal >= this.baseCost;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0002750E File Offset: 0x0002570E
		public bool CanStartNow
		{
			get
			{
				return !this.IsFinished && this.PrerequisitesCompleted && this.TechprintRequirementMet && (this.requiredResearchBuilding == null || this.PlayerHasAnyAppropriateResearchBench) && this.PlayerMechanitorRequirementMet && this.StudiedThingsRequirementsMet;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00027548 File Offset: 0x00025748
		public bool PrerequisitesCompleted
		{
			get
			{
				if (this.prerequisites != null)
				{
					for (int i = 0; i < this.prerequisites.Count; i++)
					{
						if (!this.prerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				if (this.hiddenPrerequisites != null)
				{
					for (int j = 0; j < this.hiddenPrerequisites.Count; j++)
					{
						if (!this.hiddenPrerequisites[j].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x000275BC File Offset: 0x000257BC
		public int TechprintCount
		{
			get
			{
				if (!ModLister.RoyaltyInstalled)
				{
					return 0;
				}
				return this.techprintCount;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x000275CD File Offset: 0x000257CD
		public int TechprintsApplied
		{
			get
			{
				return Find.ResearchManager.GetTechprints(this);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x000275DA File Offset: 0x000257DA
		public bool TechprintRequirementMet
		{
			get
			{
				return this.TechprintCount <= 0 || Find.ResearchManager.GetTechprints(this) >= this.TechprintCount;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x000275FC File Offset: 0x000257FC
		public ThingDef Techprint
		{
			get
			{
				if (this.TechprintCount <= 0)
				{
					return null;
				}
				if (this.cachedTechprint == null)
				{
					this.cachedTechprint = DefDatabase<ThingDef>.AllDefs.FirstOrDefault(delegate(ThingDef x)
					{
						CompProperties_Techprint compProperties = x.GetCompProperties<CompProperties_Techprint>();
						return compProperties != null && compProperties.project == this;
					});
					if (this.cachedTechprint == null)
					{
						Log.ErrorOnce("Could not find techprint for research project " + this, (int)this.shortHash ^ 873231450);
					}
				}
				return this.cachedTechprint;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00027662 File Offset: 0x00025862
		public int RequiredStudiedThingCount
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return 0;
				}
				List<ThingDef> list = this.requiredStudied;
				if (list == null)
				{
					return 0;
				}
				return list.Count;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00027680 File Offset: 0x00025880
		public int StudiedThingsCompleted
		{
			get
			{
				if (this.requiredStudied.NullOrEmpty<ThingDef>())
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this.requiredStudied.Count; i++)
				{
					if (Find.StudyManager.StudyComplete(this.requiredStudied[i]))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x000276D1 File Offset: 0x000258D1
		public bool StudiedThingsRequirementsMet
		{
			get
			{
				return this.RequiredStudiedThingCount <= 0 || this.StudiedThingsCompleted >= this.RequiredStudiedThingCount;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x000276F0 File Offset: 0x000258F0
		public List<Def> UnlockedDefs
		{
			get
			{
				if (this.cachedUnlockedDefs == null)
				{
					this.cachedUnlockedDefs = (from x in (from x in DefDatabase<RecipeDef>.AllDefs
					where x.researchPrerequisite == this || (x.researchPrerequisites != null && x.researchPrerequisites.Contains(this))
					select x).SelectMany((RecipeDef x) => from y in x.products
					select y.thingDef)
					orderby x.label
					select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					where x.plant != null && x.plant.sowResearchPrerequisites != null && x.plant.sowResearchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<TerrainDef>.AllDefs
					where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<RecipeDef>.AllDefs
					where x.IsSurgery && x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Distinct<Def>().ToList<Def>();
				}
				return this.cachedUnlockedDefs;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00027878 File Offset: 0x00025A78
		public List<Dialog_InfoCard.Hyperlink> InfoCardHyperlinks
		{
			get
			{
				if (this.cachedHyperlinks == null)
				{
					this.cachedHyperlinks = new List<Dialog_InfoCard.Hyperlink>();
					List<Def> unlockedDefs = this.UnlockedDefs;
					if (unlockedDefs != null)
					{
						for (int i = 0; i < unlockedDefs.Count; i++)
						{
							this.cachedHyperlinks.Add(new Dialog_InfoCard.Hyperlink(unlockedDefs[i], -1));
						}
					}
				}
				return this.cachedHyperlinks;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x000278D4 File Offset: 0x00025AD4
		public bool PlayerHasAnyAppropriateResearchBench
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Building> allBuildingsColonist = maps[i].listerBuildings.allBuildingsColonist;
					for (int j = 0; j < allBuildingsColonist.Count; j++)
					{
						Building_ResearchBench building_ResearchBench = allBuildingsColonist[j] as Building_ResearchBench;
						if (building_ResearchBench != null && this.CanBeResearchedAt(building_ResearchBench, true))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0002793B File Offset: 0x00025B3B
		public bool PlayerMechanitorRequirementMet
		{
			get
			{
				return !ModsConfig.BiotechActive || !this.requiresMechanitor || MechanitorUtility.AnyMechanitorInPlayerFaction();
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00027953 File Offset: 0x00025B53
		public override void ResolveReferences()
		{
			if (this.tab == null)
			{
				this.tab = ResearchTabDefOf.Main;
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00027968 File Offset: 0x00025B68
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return "techLevel is Undefined";
			}
			if (this.ResearchViewX < 0f || this.ResearchViewY < 0f)
			{
				yield return "researchViewX and/or researchViewY not set";
			}
			if (this.techprintCount == 0 && !this.heldByFactionCategoryTags.NullOrEmpty<string>())
			{
				yield return "requires no techprints but has heldByFactionCategoryTags.";
			}
			if (this.techprintCount > 0 && this.heldByFactionCategoryTags.NullOrEmpty<string>())
			{
				yield return "requires techprints but has no heldByFactionCategoryTags.";
			}
			List<ResearchProjectDef> rpDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < rpDefs.Count; i = num + 1)
			{
				if (rpDefs[i] != this && rpDefs[i].tab == this.tab && rpDefs[i].ResearchViewX == this.ResearchViewX && rpDefs[i].ResearchViewY == this.ResearchViewY)
				{
					yield return string.Concat(new object[]
					{
						"same research view coords and tab as ",
						rpDefs[i],
						": ",
						this.ResearchViewX,
						", ",
						this.ResearchViewY,
						"(",
						this.tab,
						")"
					});
				}
				num = i;
			}
			if (!ModLister.RoyaltyInstalled && this.techprintCount > 0)
			{
				yield return "defines techprintCount, but techprints are a Royalty-specific game system and only work with Royalty installed.";
			}
			if (!ModLister.BiotechInstalled && !this.requiredStudied.NullOrEmpty<ThingDef>())
			{
				yield return "defines requiredStudied, but study requirements are a Biotech-specific game system and only work with Biotech installed.";
			}
			if (!this.requiredStudied.NullOrEmpty<ThingDef>())
			{
				foreach (ThingDef thingDef in this.requiredStudied)
				{
					if (!thingDef.HasComp(typeof(CompStudiable)))
					{
						yield return string.Concat(new string[]
						{
							"requires studying ",
							thingDef.label,
							" but ",
							thingDef.label,
							" cannot be studied."
						});
					}
				}
				List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00027978 File Offset: 0x00025B78
		public override void PostLoad()
		{
			base.PostLoad();
			if (!ModLister.RoyaltyInstalled)
			{
				this.techprintCount = 0;
			}
			if (!ModLister.BiotechInstalled)
			{
				this.requiredStudied = null;
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002799C File Offset: 0x00025B9C
		public string GetTip()
		{
			if (this.cachedTip == null)
			{
				this.cachedTip = this.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n" + this.description;
				if (this.TechprintCount > 0)
				{
					this.cachedTip = this.cachedTip + "\n\n" + ("RequiredTechprintTip".Translate() + ": " + this.Techprint.LabelCap).ToString();
				}
				if (this.RequiredStudiedThingCount > 0)
				{
					string[] array = new string[5];
					array[0] = this.cachedTip;
					array[1] = "\n\n";
					array[2] = "StudyRequirementTip".Translate().ToString();
					array[3] = ": ";
					array[4] = (from t in this.requiredStudied
					select t.label).ToCommaList(true, false).CapitalizeFirst();
					this.cachedTip = string.Concat(array);
				}
				if (this.modContentPack != null && !this.modContentPack.IsCoreMod)
				{
					Color color = this.modContentPack.IsOfficialMod ? ModLister.GetExpansionWithIdentifier(this.modContentPack.PackageId.ToLower()).primaryColor : ColoredText.SubtleGrayColor;
					this.cachedTip = this.cachedTip + "\n\n" + ("Stat_Source_Label".Translate().ToString() + ": " + this.modContentPack.Name).Colorize(color);
				}
			}
			return this.cachedTip;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00027B48 File Offset: 0x00025D48
		public float CostFactor(TechLevel researcherTechLevel)
		{
			TechLevel techLevel = (TechLevel)Mathf.Min((int)this.techLevel, 4);
			if (researcherTechLevel >= techLevel)
			{
				return 1f;
			}
			int num = (int)(techLevel - researcherTechLevel);
			return 1f + (float)num * 0.5f;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00027B7F File Offset: 0x00025D7F
		public bool HasTag(ResearchProjectTagDef tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00027B98 File Offset: 0x00025D98
		public bool CanBeResearchedAt(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
		{
			if (this.requiredResearchBuilding != null && bench.def != this.requiredResearchBuilding)
			{
				return false;
			}
			if (!ignoreResearchBenchPowerStatus)
			{
				CompPowerTrader comp = bench.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
			}
			if (!this.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			{
				ResearchProjectDef.<>c__DisplayClass80_0 CS$<>8__locals1 = new ResearchProjectDef.<>c__DisplayClass80_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.affectedByFacilities = bench.TryGetComp<CompAffectedByFacilities>();
				if (CS$<>8__locals1.affectedByFacilities == null)
				{
					return false;
				}
				List<Thing> linkedFacilitiesListForReading = CS$<>8__locals1.affectedByFacilities.LinkedFacilitiesListForReading;
				int j;
				int i;
				for (i = 0; i < this.requiredResearchFacilities.Count; i = j + 1)
				{
					if (linkedFacilitiesListForReading.Find((Thing x) => x.def == CS$<>8__locals1.<>4__this.requiredResearchFacilities[i] && CS$<>8__locals1.affectedByFacilities.IsFacilityActive(x)) == null)
					{
						return false;
					}
					j = i;
				}
			}
			return true;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00027C68 File Offset: 0x00025E68
		public void ReapplyAllMods()
		{
			if (this.researchMods != null)
			{
				for (int i = 0; i < this.researchMods.Count; i++)
				{
					try
					{
						this.researchMods[i].Apply();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception applying research mod for project ",
							this,
							": ",
							ex.ToString()
						}));
					}
				}
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00027CE8 File Offset: 0x00025EE8
		public static ResearchProjectDef Named(string defName)
		{
			return DefDatabase<ResearchProjectDef>.GetNamed(defName, true);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00027CF4 File Offset: 0x00025EF4
		public static void GenerateNonOverlappingCoordinates()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				researchProjectDef.x = researchProjectDef.researchViewX;
				researchProjectDef.y = researchProjectDef.researchViewY;
			}
			int num = 0;
			do
			{
				bool flag = false;
				foreach (ResearchProjectDef researchProjectDef2 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
				{
					foreach (ResearchProjectDef researchProjectDef3 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
					{
						if (researchProjectDef2 != researchProjectDef3 && researchProjectDef2.tab == researchProjectDef3.tab)
						{
							bool flag2 = Mathf.Abs(researchProjectDef2.x - researchProjectDef3.x) < 0.5f;
							bool flag3 = Mathf.Abs(researchProjectDef2.y - researchProjectDef3.y) < 0.25f;
							if (flag2 && flag3)
							{
								flag = true;
								if (researchProjectDef2.x <= researchProjectDef3.x)
								{
									researchProjectDef2.x -= 0.1f;
									researchProjectDef3.x += 0.1f;
								}
								else
								{
									researchProjectDef2.x += 0.1f;
									researchProjectDef3.x -= 0.1f;
								}
								if (researchProjectDef2.y <= researchProjectDef3.y)
								{
									researchProjectDef2.y -= 0.1f;
									researchProjectDef3.y += 0.1f;
								}
								else
								{
									researchProjectDef2.y += 0.1f;
									researchProjectDef3.y -= 0.1f;
								}
								researchProjectDef2.x += 0.001f;
								researchProjectDef2.y += 0.001f;
								researchProjectDef3.x -= 0.001f;
								researchProjectDef3.y -= 0.001f;
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef2);
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef3);
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
				num++;
			}
			while (num <= 200);
			Log.Error("Couldn't relax research project coordinates apart after " + 200 + " passes.");
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00027F98 File Offset: 0x00026198
		private static void ClampInCoordinateLimits(ResearchProjectDef rp)
		{
			if (rp.x < 0f)
			{
				rp.x = 0f;
			}
			if (rp.y < 0f)
			{
				rp.y = 0f;
			}
			if (rp.y > 6.5f)
			{
				rp.y = 6.5f;
			}
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00027FF0 File Offset: 0x000261F0
		public void Debug_ApplyPositionDelta(Vector2 delta)
		{
			bool flag = Mathf.Abs(delta.x) > 0.01f;
			bool flag2 = Mathf.Abs(delta.y) > 0.01f;
			if (flag)
			{
				this.x += delta.x;
			}
			if (flag2)
			{
				this.y += delta.y;
			}
			if (flag || flag2)
			{
				this.positionModified = true;
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002805C File Offset: 0x0002625C
		public void Debug_SnapPositionData()
		{
			this.x = Mathf.Round(this.x * 1f) / 1f;
			this.y = Mathf.Round(this.y * 20f) / 20f;
			ResearchProjectDef.ClampInCoordinateLimits(this);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x000280A9 File Offset: 0x000262A9
		public bool Debug_IsPositionModified()
		{
			return this.positionModified;
		}

		// Token: 0x040007C2 RID: 1986
		public float baseCost = 100f;

		// Token: 0x040007C3 RID: 1987
		public List<ResearchProjectDef> prerequisites;

		// Token: 0x040007C4 RID: 1988
		public List<ResearchProjectDef> hiddenPrerequisites;

		// Token: 0x040007C5 RID: 1989
		public TechLevel techLevel;

		// Token: 0x040007C6 RID: 1990
		public List<ResearchProjectDef> requiredByThis;

		// Token: 0x040007C7 RID: 1991
		private List<ResearchMod> researchMods;

		// Token: 0x040007C8 RID: 1992
		public ThingDef requiredResearchBuilding;

		// Token: 0x040007C9 RID: 1993
		public List<ThingDef> requiredResearchFacilities;

		// Token: 0x040007CA RID: 1994
		public List<ResearchProjectTagDef> tags;

		// Token: 0x040007CB RID: 1995
		public ResearchTabDef tab;

		// Token: 0x040007CC RID: 1996
		public float researchViewX = 1f;

		// Token: 0x040007CD RID: 1997
		public float researchViewY = 1f;

		// Token: 0x040007CE RID: 1998
		[MustTranslate]
		public string discoveredLetterTitle;

		// Token: 0x040007CF RID: 1999
		[MustTranslate]
		public string discoveredLetterText;

		// Token: 0x040007D0 RID: 2000
		[Obsolete]
		public int discoveredLetterMinDifficulty;

		// Token: 0x040007D1 RID: 2001
		public DifficultyConditionConfig discoveredLetterDisabledWhen = new DifficultyConditionConfig();

		// Token: 0x040007D2 RID: 2002
		[Obsolete]
		public bool unlockExtremeDifficulty;

		// Token: 0x040007D3 RID: 2003
		public int techprintCount;

		// Token: 0x040007D4 RID: 2004
		public float techprintCommonality = 1f;

		// Token: 0x040007D5 RID: 2005
		public float techprintMarketValue = 1000f;

		// Token: 0x040007D6 RID: 2006
		public List<string> heldByFactionCategoryTags;

		// Token: 0x040007D7 RID: 2007
		public DifficultyConditionConfig hideWhen = new DifficultyConditionConfig();

		// Token: 0x040007D8 RID: 2008
		public bool requiresMechanitor;

		// Token: 0x040007D9 RID: 2009
		public List<ThingDef> requiredStudied;

		// Token: 0x040007DA RID: 2010
		public bool recalculatePower;

		// Token: 0x040007DB RID: 2011
		[Unsaved(false)]
		private float x = 1f;

		// Token: 0x040007DC RID: 2012
		[Unsaved(false)]
		private float y = 1f;

		// Token: 0x040007DD RID: 2013
		[Unsaved(false)]
		private bool positionModified;

		// Token: 0x040007DE RID: 2014
		[Unsaved(false)]
		private ThingDef cachedTechprint;

		// Token: 0x040007DF RID: 2015
		[Unsaved(false)]
		private List<Def> cachedUnlockedDefs;

		// Token: 0x040007E0 RID: 2016
		[Unsaved(false)]
		private List<Dialog_InfoCard.Hyperlink> cachedHyperlinks;

		// Token: 0x040007E1 RID: 2017
		[Unsaved(false)]
		private string cachedTip;

		// Token: 0x040007E2 RID: 2018
		public const TechLevel MaxEffectiveTechLevel = TechLevel.Industrial;

		// Token: 0x040007E3 RID: 2019
		private const float ResearchCostFactorPerTechLevelDiff = 0.5f;
	}
}

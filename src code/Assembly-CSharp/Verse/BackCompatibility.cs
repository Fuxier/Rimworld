using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000509 RID: 1289
	public static class BackCompatibility
	{
		// Token: 0x0600275E RID: 10078 RVA: 0x000FC974 File Offset: 0x000FAB74
		public static bool IsSaveCompatibleWith(string version)
		{
			if (VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) == VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) >= 1 && VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) <= VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) == 0 && VersionControl.CurrentMajor == 0)
			{
				int num = VersionControl.MinorFromVersionString(version);
				int currentMinor = VersionControl.CurrentMinor;
				for (int i = 0; i < BackCompatibility.SaveCompatibleMinorVersions.Length; i++)
				{
					if (BackCompatibility.SaveCompatibleMinorVersions[i].First == num && BackCompatibility.SaveCompatibleMinorVersions[i].Second == currentMinor)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000FCA1C File Offset: 0x000FAC1C
		public static void PreLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PreLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PreLoadSavegame of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000FCAB0 File Offset: 0x000FACB0
		public static void PostLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PostLoadSavegame of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000FCB44 File Offset: 0x000FAD44
		public static string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (Scribe.mode != LoadSaveMode.Inactive && VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild)
			{
				return defName;
			}
			if (GenDefDatabase.GetDefSilentFail(defType, defName, false) != null)
			{
				return defName;
			}
			string text = defName;
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (Scribe.mode == LoadSaveMode.Inactive || BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						string text2 = BackCompatibility.conversionChain[i].BackCompatibleDefName(defType, text, forDefInjections, node);
						if (text2 != null)
						{
							text = text2;
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in BackCompatibleDefName of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}));
					}
				}
			}
			return text;
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000FCC10 File Offset: 0x000FAE10
		public static object BackCompatibleEnum(Type enumType, string enumName)
		{
			if (enumType == typeof(QualityCategory))
			{
				if (enumName == "Shoddy")
				{
					return QualityCategory.Poor;
				}
				if (enumName == "Superior")
				{
					return QualityCategory.Excellent;
				}
			}
			return null;
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000FCC50 File Offset: 0x000FAE50
		public static Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild)
			{
				return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						Type backCompatibleType = BackCompatibility.conversionChain[i].GetBackCompatibleType(baseType, providedClassName, node);
						if (backCompatibleType != null)
						{
							return backCompatibleType;
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in GetBackCompatibleType of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}));
					}
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000FCD14 File Offset: 0x000FAF14
		public static Type GetBackCompatibleTypeDirect(Type baseType, string providedClassName)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToVersion(VersionControl.CurrentMajor, VersionControl.CurrentMinor))
				{
					try
					{
						Type backCompatibleType = BackCompatibility.conversionChain[i].GetBackCompatibleType(baseType, providedClassName, null);
						if (backCompatibleType != null)
						{
							return backCompatibleType;
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in GetBackCompatibleType of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}));
					}
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000FCDCC File Offset: 0x000FAFCC
		public static int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			if (VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild)
			{
				return index;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						index = BackCompatibility.conversionChain[i].GetBackCompatibleBodyPartIndex(body, index);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in GetBackCompatibleBodyPartIndex of ",
							body,
							"\n",
							ex
						}));
					}
				}
			}
			return index;
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000FCE64 File Offset: 0x000FB064
		public static bool WasDefRemoved(string defName, Type type)
		{
			foreach (Tuple<string, Type> tuple in BackCompatibility.RemovedDefs)
			{
				if (tuple.Item1 == defName && tuple.Item2 == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000FCED4 File Offset: 0x000FB0D4
		public static void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return;
			}
			if (VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild)
			{
				return;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostExposeData(obj);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PostExposeData of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000FCF80 File Offset: 0x000FB180
		public static void PostCouldntLoadDef(string defName)
		{
			if (VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild)
			{
				return;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostCouldntLoadDef(defName);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PostCouldntLoadDef of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000FD020 File Offset: 0x000FB220
		public static void PawnTrainingTrackerPostLoadInit(Pawn_TrainingTracker tracker, ref DefMap<TrainableDef, bool> wantedTrainables, ref DefMap<TrainableDef, int> steps, ref DefMap<TrainableDef, bool> learned)
		{
			if (wantedTrainables == null)
			{
				wantedTrainables = new DefMap<TrainableDef, bool>();
			}
			if (steps == null)
			{
				steps = new DefMap<TrainableDef, int>();
			}
			if (learned == null)
			{
				learned = new DefMap<TrainableDef, bool>();
			}
			if (tracker.GetSteps(TrainableDefOf.Tameness) == 0 && DefDatabase<TrainableDef>.AllDefsListForReading.Any((TrainableDef td) => tracker.GetSteps(td) != 0))
			{
				tracker.Train(TrainableDefOf.Tameness, null, true);
			}
			foreach (TrainableDef trainableDef in DefDatabase<TrainableDef>.AllDefsListForReading)
			{
				if (!tracker.CanAssignToTrain(trainableDef).Accepted)
				{
					wantedTrainables[trainableDef] = false;
					learned[trainableDef] = false;
					steps[trainableDef] = 0;
					if (trainableDef == TrainableDefOf.Obedience && tracker.pawn.playerSettings != null)
					{
						tracker.pawn.playerSettings.Master = null;
						tracker.pawn.playerSettings.followDrafted = false;
						tracker.pawn.playerSettings.followFieldwork = false;
					}
				}
				if (tracker.GetSteps(trainableDef) == trainableDef.steps)
				{
					tracker.Train(trainableDef, null, true);
				}
			}
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000FD18C File Offset: 0x000FB38C
		public static void TriggerDataFractionColonyDamageTakenNull(Trigger_FractionColonyDamageTaken trigger, Map map)
		{
			trigger.data = new TriggerData_FractionColonyDamageTaken();
			((TriggerData_FractionColonyDamageTaken)trigger.data).startColonyDamage = map.damageWatcher.DamageTakenEver;
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000FD1B4 File Offset: 0x000FB3B4
		public static void TriggerDataPawnCycleIndNull(Trigger_KidnapVictimPresent trigger)
		{
			trigger.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000FD1C1 File Offset: 0x000FB3C1
		public static void TriggerDataTicksPassedNull(Trigger_TicksPassed trigger)
		{
			trigger.data = new TriggerData_TicksPassed();
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000FD1CE File Offset: 0x000FB3CE
		public static TerrainDef BackCompatibleTerrainWithShortHash(ushort hash)
		{
			if (hash == 16442)
			{
				return TerrainDefOf.WaterMovingChestDeep;
			}
			return null;
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000FD1DF File Offset: 0x000FB3DF
		public static ThingDef BackCompatibleThingDefWithShortHash(ushort hash)
		{
			if (hash == 62520)
			{
				return ThingDefOf.MineableComponentsIndustrial;
			}
			return null;
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000FD1F0 File Offset: 0x000FB3F0
		public static ThingDef BackCompatibleThingDefWithShortHash_Force(ushort hash, int major, int minor)
		{
			if (major == 0 && minor <= 18 && hash == 27292)
			{
				return ThingDefOf.MineableSteel;
			}
			return null;
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000FD20C File Offset: 0x000FB40C
		public static bool CheckSpawnBackCompatibleThingAfterLoading(Thing thing, Map map)
		{
			if (ScribeMetaHeaderUtility.loadedGameVersionMajor == 0 && ScribeMetaHeaderUtility.loadedGameVersionMinor <= 18 && thing.stackCount > thing.def.stackLimit && thing.stackCount != 1 && thing.def.stackLimit != 1)
			{
				BackCompatibility.tmpThingsToSpawnLater.Add(thing);
				return true;
			}
			return false;
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000FD261 File Offset: 0x000FB461
		public static void PreCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000FD270 File Offset: 0x000FB470
		public static void PostCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			for (int i = 0; i < BackCompatibility.tmpThingsToSpawnLater.Count; i++)
			{
				GenPlace.TryPlaceThing(BackCompatibility.tmpThingsToSpawnLater[i], BackCompatibility.tmpThingsToSpawnLater[i].Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
			}
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000FD2CC File Offset: 0x000FB4CC
		public static void FactionManagerPostLoadInit()
		{
			if (ModsConfig.RoyaltyActive && Find.FactionManager.FirstFactionOfDef(FactionDefOf.Empire) == null && Find.World.info.factions == null)
			{
				Faction faction = FactionGenerator.NewGeneratedFaction(new FactionGeneratorParms(FactionDefOf.Empire, default(IdeoGenerationParms), null));
				Find.FactionManager.Add(faction);
			}
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000FD330 File Offset: 0x000FB530
		public static void ResearchManagerPostLoadInit()
		{
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if ((allDefsListForReading[i].IsFinished || allDefsListForReading[i].ProgressReal > 0f) && allDefsListForReading[i].TechprintsApplied < allDefsListForReading[i].TechprintCount)
				{
					Find.ResearchManager.AddTechprints(allDefsListForReading[i], allDefsListForReading[i].TechprintCount - allDefsListForReading[i].TechprintsApplied);
				}
			}
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000FD3B9 File Offset: 0x000FB5B9
		public static void PrefsDataPostLoad(PrefsData prefsData)
		{
			if (prefsData.pauseOnUrgentLetter != null)
			{
				if (prefsData.pauseOnUrgentLetter.Value)
				{
					prefsData.automaticPauseMode = AutomaticPauseMode.MajorThreat;
				}
				else
				{
					prefsData.automaticPauseMode = AutomaticPauseMode.Never;
				}
			}
			if (prefsData.debugActionPalette == null)
			{
				prefsData.debugActionPalette = new List<string>();
			}
		}

		// Token: 0x040019F5 RID: 6645
		public static readonly Pair<int, int>[] SaveCompatibleMinorVersions = new Pair<int, int>[]
		{
			new Pair<int, int>(17, 18)
		};

		// Token: 0x040019F6 RID: 6646
		private static List<BackCompatibilityConverter> conversionChain = new List<BackCompatibilityConverter>
		{
			new BackCompatibilityConverter_0_17_AndLower(),
			new BackCompatibilityConverter_0_18(),
			new BackCompatibilityConverter_0_19(),
			new BackCompatibilityConverter_1_0(),
			new BackCompatibilityConverter_1_2(),
			new BackCompatibilityConverter_1_3(),
			new BackCompatibilityConverter_Universal()
		};

		// Token: 0x040019F7 RID: 6647
		private static readonly List<Tuple<string, Type>> RemovedDefs = new List<Tuple<string, Type>>
		{
			new Tuple<string, Type>("PsychicSilencer", typeof(ThingDef)),
			new Tuple<string, Type>("PsychicSilencer", typeof(HediffDef))
		};

		// Token: 0x040019F8 RID: 6648
		private static List<Thing> tmpThingsToSpawnLater = new List<Thing>();
	}
}

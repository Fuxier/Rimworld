using System;
using System.IO;
using RimWorld;
using Verse.Profile;

namespace Verse
{
	// Token: 0x020003A4 RID: 932
	public static class GameDataSaveLoader
	{
		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001AA1 RID: 6817 RVA: 0x000A1F25 File Offset: 0x000A0125
		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001AA2 RID: 6818 RVA: 0x000A1F3B File Offset: 0x000A013B
		public static bool SavingIsTemporarilyDisabled
		{
			get
			{
				return Find.TilePicker.Active || Find.WindowStack.WindowsPreventSave;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x000A1F55 File Offset: 0x000A0155
		public static bool IsSavingOrLoadingExternalIdeo
		{
			get
			{
				return GameDataSaveLoader.isSavingOrLoadingExternalIdeo;
			}
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000A1F5C File Offset: 0x000A015C
		public static void SaveScenario(Scenario scen, string absFilePath)
		{
			try
			{
				scen.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedscenario", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving world: " + ex.ToString());
			}
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x000A1FCC File Offset: 0x000A01CC
		public static bool TryLoadScenario(string absPath, ScenarioCategory category, out Scenario scen)
		{
			scen = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, true);
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				scen.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
				scen.Category = category;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading scenario: " + ex.ToString());
				scen = null;
				Scribe.ForceStop();
			}
			return scen != null;
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x000A2074 File Offset: 0x000A0274
		public static void SaveGame(string fileName)
		{
			try
			{
				SafeSaver.Save(GenFilePaths.FilePathForSavedGame(fileName), "savegame", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Game game = Current.Game;
					Scribe_Deep.Look<Game>(ref game, "game", Array.Empty<object>());
				}, Find.GameInfo.permadeathMode);
				GameDataSaveLoader.lastSaveTick = Find.TickManager.TicksGame;
			}
			catch (Exception arg)
			{
				Log.Error("Exception while saving game: " + arg);
			}
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x000A20F0 File Offset: 0x000A02F0
		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			}, false);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x000A2128 File Offset: 0x000A0328
		public static void LoadGame(string saveFileName)
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = new Game();
				Current.Game.InitData = new GameInitData();
				Current.Game.InitData.gameToLoad = saveFileName;
			}, "Play", "LoadingLongEvent", true, null, true);
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x000A2153 File Offset: 0x000A0353
		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x000A2168 File Offset: 0x000A0368
		public static void SaveIdeo(Ideo ideo, string absFilePath)
		{
			try
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = true;
				ideo.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedideo", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<Ideo>(ref ideo, "ideo", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving ideo: " + ex.ToString());
			}
			finally
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = false;
			}
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x000A21F0 File Offset: 0x000A03F0
		public static bool TryLoadIdeo(string absPath, out Ideo ideo)
		{
			ideo = null;
			try
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = true;
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Ideo, true);
					Scribe_Deep.Look<Ideo>(ref ideo, "ideo", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				ideo.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading ideo: " + ex.ToString());
				ideo = null;
				Scribe.ForceStop();
			}
			finally
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = false;
			}
			return ideo != null;
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000A22AC File Offset: 0x000A04AC
		public static void SaveXenotype(CustomXenotype xenotype, string absFilePath)
		{
			try
			{
				xenotype.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedXenotype", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<CustomXenotype>(ref xenotype, "xenotype", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving xenotype: " + ex.ToString());
			}
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x000A231C File Offset: 0x000A051C
		public static bool TryLoadXenotype(string absPath, out CustomXenotype xenotype)
		{
			xenotype = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Xenotype, true);
					Scribe_Deep.Look<CustomXenotype>(ref xenotype, "xenotype", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				xenotype.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading xenotype: " + ex.ToString());
				xenotype = null;
				Scribe.ForceStop();
			}
			return xenotype != null;
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x000A23BC File Offset: 0x000A05BC
		public static void SaveModList(ModList modList, string absFilePath)
		{
			try
			{
				modList.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedModList", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<ModList>(ref modList, "modList", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving mod list: " + ex.ToString());
			}
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x000A242C File Offset: 0x000A062C
		public static bool TryLoadModList(string absPath, out ModList modList)
		{
			modList = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.ModList, true);
					Scribe_Deep.Look<ModList>(ref modList, "modList", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				modList.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading mod list: " + ex.ToString());
				modList = null;
				Scribe.ForceStop();
			}
			return modList != null;
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x000A24CC File Offset: 0x000A06CC
		public static void SaveCameraConfig(CameraMapConfig config, string absFilePath)
		{
			try
			{
				config.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "cameraConfig", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<CameraMapConfig>(ref config, "camConfig", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving camera config: " + ex.ToString());
			}
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x000A253C File Offset: 0x000A073C
		public static bool TryLoadCameraConfig(string absPath, out CameraMapConfig config)
		{
			config = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.CameraConfig, true);
					Scribe_Deep.Look<CameraMapConfig>(ref config, "camConfig", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				config.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading camera config: " + ex.ToString());
				config = null;
				Scribe.ForceStop();
			}
			return config != null;
		}

		// Token: 0x04001366 RID: 4966
		private static int lastSaveTick = -9999;

		// Token: 0x04001367 RID: 4967
		private static bool isSavingOrLoadingExternalIdeo;

		// Token: 0x04001368 RID: 4968
		public const string SavedScenarioParentNodeName = "savedscenario";

		// Token: 0x04001369 RID: 4969
		public const string SavedWorldParentNodeName = "savedworld";

		// Token: 0x0400136A RID: 4970
		public const string SavedGameParentNodeName = "savegame";

		// Token: 0x0400136B RID: 4971
		public const string SavedIdeoParentNodeName = "savedideo";

		// Token: 0x0400136C RID: 4972
		public const string SavedXenotypeParentNodeName = "savedXenotype";

		// Token: 0x0400136D RID: 4973
		public const string SavedXenogermParentNode = "savedXenogerm";

		// Token: 0x0400136E RID: 4974
		public const string SavedModListParentNodeName = "savedModList";

		// Token: 0x0400136F RID: 4975
		public const string SavedCameraConfigParentNodeName = "cameraConfig";

		// Token: 0x04001370 RID: 4976
		public const string GameNodeName = "game";

		// Token: 0x04001371 RID: 4977
		public const string WorldNodeName = "world";

		// Token: 0x04001372 RID: 4978
		public const string ScenarioNodeName = "scenario";

		// Token: 0x04001373 RID: 4979
		public const string IdeoNodeName = "ideo";

		// Token: 0x04001374 RID: 4980
		public const string XenotypeNodeName = "xenotype";

		// Token: 0x04001375 RID: 4981
		public const string XenogermNodeName = "xenogerm";

		// Token: 0x04001376 RID: 4982
		public const string ModListNodeName = "modList";

		// Token: 0x04001377 RID: 4983
		public const string CameraConfigNodeName = "camConfig";

		// Token: 0x04001378 RID: 4984
		public const string AutosavePrefix = "Autosave";

		// Token: 0x04001379 RID: 4985
		public const string AutostartSaveName = "autostart";
	}
}

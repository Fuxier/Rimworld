using System;

namespace Verse
{
	// Token: 0x0200053F RID: 1343
	public static class DebugViewSettings
	{
		// Token: 0x06002945 RID: 10565 RVA: 0x00107F7D File Offset: 0x0010617D
		public static void drawTerrainWaterToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged(MapMeshFlag.Terrain);
			}
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x00107F97 File Offset: 0x00106197
		public static void drawShadowsToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged((MapMeshFlag)(-1));
			}
		}

		// Token: 0x04001AE5 RID: 6885
		public static bool drawFog = true;

		// Token: 0x04001AE6 RID: 6886
		public static bool drawSnow = true;

		// Token: 0x04001AE7 RID: 6887
		public static bool drawTerrain = true;

		// Token: 0x04001AE8 RID: 6888
		public static bool drawTerrainWater = true;

		// Token: 0x04001AE9 RID: 6889
		public static bool drawThingsDynamic = true;

		// Token: 0x04001AEA RID: 6890
		public static bool drawThingsPrinted = true;

		// Token: 0x04001AEB RID: 6891
		public static bool drawShadows = true;

		// Token: 0x04001AEC RID: 6892
		public static bool drawLightingOverlay = true;

		// Token: 0x04001AED RID: 6893
		public static bool drawWorldOverlays = true;

		// Token: 0x04001AEE RID: 6894
		public static bool drawGas = true;

		// Token: 0x04001AEF RID: 6895
		public static bool drawPaths = false;

		// Token: 0x04001AF0 RID: 6896
		public static bool drawCastPositionSearch = false;

		// Token: 0x04001AF1 RID: 6897
		public static bool drawDestSearch = false;

		// Token: 0x04001AF2 RID: 6898
		public static bool drawStyleSearch = false;

		// Token: 0x04001AF3 RID: 6899
		public static bool drawSectionEdges = false;

		// Token: 0x04001AF4 RID: 6900
		public static bool drawRiverDebug = false;

		// Token: 0x04001AF5 RID: 6901
		public static bool drawPawnDebug = false;

		// Token: 0x04001AF6 RID: 6902
		public static bool drawPawnRotatorTarget = false;

		// Token: 0x04001AF7 RID: 6903
		public static bool drawRegions = false;

		// Token: 0x04001AF8 RID: 6904
		public static bool drawRegionLinks = false;

		// Token: 0x04001AF9 RID: 6905
		public static bool drawRegionDirties = false;

		// Token: 0x04001AFA RID: 6906
		public static bool drawRegionTraversal = false;

		// Token: 0x04001AFB RID: 6907
		public static bool drawRegionThings = false;

		// Token: 0x04001AFC RID: 6908
		public static bool drawDistricts = false;

		// Token: 0x04001AFD RID: 6909
		public static bool drawRooms = false;

		// Token: 0x04001AFE RID: 6910
		public static bool drawPower = false;

		// Token: 0x04001AFF RID: 6911
		public static bool drawPowerNetGrid = false;

		// Token: 0x04001B00 RID: 6912
		public static bool drawOpportunisticJobs = false;

		// Token: 0x04001B01 RID: 6913
		public static bool drawTooltipEdges = false;

		// Token: 0x04001B02 RID: 6914
		public static bool drawRecordedNoise = false;

		// Token: 0x04001B03 RID: 6915
		public static bool drawFoodSearchFromMouse = false;

		// Token: 0x04001B04 RID: 6916
		public static bool drawPreyInfo = false;

		// Token: 0x04001B05 RID: 6917
		public static bool drawGlow = false;

		// Token: 0x04001B06 RID: 6918
		public static bool drawAvoidGrid = false;

		// Token: 0x04001B07 RID: 6919
		public static bool drawBreachingGrid = false;

		// Token: 0x04001B08 RID: 6920
		public static bool drawBreachingNoise = false;

		// Token: 0x04001B09 RID: 6921
		public static bool drawLords = false;

		// Token: 0x04001B0A RID: 6922
		public static bool drawDuties = false;

		// Token: 0x04001B0B RID: 6923
		public static bool drawShooting = false;

		// Token: 0x04001B0C RID: 6924
		public static bool drawInfestationChance = false;

		// Token: 0x04001B0D RID: 6925
		public static bool drawStealDebug = false;

		// Token: 0x04001B0E RID: 6926
		public static bool drawDeepResources = false;

		// Token: 0x04001B0F RID: 6927
		public static bool drawAttackTargetScores = false;

		// Token: 0x04001B10 RID: 6928
		public static bool drawInteractionCells = false;

		// Token: 0x04001B11 RID: 6929
		public static bool drawDoorsDebug = false;

		// Token: 0x04001B12 RID: 6930
		public static bool drawDestReservations = false;

		// Token: 0x04001B13 RID: 6931
		public static bool drawDamageRects = false;

		// Token: 0x04001B14 RID: 6932
		public static bool drawDissolutionCells = false;

		// Token: 0x04001B15 RID: 6933
		public static bool drawUnpollutionCells = false;

		// Token: 0x04001B16 RID: 6934
		public static bool writeGame = false;

		// Token: 0x04001B17 RID: 6935
		public static bool writeSteamItems = false;

		// Token: 0x04001B18 RID: 6936
		public static bool writeConcepts = false;

		// Token: 0x04001B19 RID: 6937
		public static bool writeReservations = false;

		// Token: 0x04001B1A RID: 6938
		public static bool writePathCosts = false;

		// Token: 0x04001B1B RID: 6939
		public static bool writeFertility = false;

		// Token: 0x04001B1C RID: 6940
		public static bool writeLinkFlags = false;

		// Token: 0x04001B1D RID: 6941
		public static bool writeCover = false;

		// Token: 0x04001B1E RID: 6942
		public static bool writeCellContents = false;

		// Token: 0x04001B1F RID: 6943
		public static bool writeMusicManagerPlay = false;

		// Token: 0x04001B20 RID: 6944
		public static bool writeStoryteller = false;

		// Token: 0x04001B21 RID: 6945
		public static bool writePlayingSounds = false;

		// Token: 0x04001B22 RID: 6946
		public static bool writeSoundEventsRecord = false;

		// Token: 0x04001B23 RID: 6947
		public static bool writeMoteSaturation = false;

		// Token: 0x04001B24 RID: 6948
		public static bool writeSnowDepth = false;

		// Token: 0x04001B25 RID: 6949
		public static bool writeEcosystem = false;

		// Token: 0x04001B26 RID: 6950
		public static bool writeRecentStrikes = false;

		// Token: 0x04001B27 RID: 6951
		public static bool writeBeauty = false;

		// Token: 0x04001B28 RID: 6952
		public static bool writeListRepairableBldgs = false;

		// Token: 0x04001B29 RID: 6953
		public static bool writeListFilthInHomeArea = false;

		// Token: 0x04001B2A RID: 6954
		public static bool writeListHaulables = false;

		// Token: 0x04001B2B RID: 6955
		public static bool writeListMergeables = false;

		// Token: 0x04001B2C RID: 6956
		public static bool writeTotalSnowDepth = false;

		// Token: 0x04001B2D RID: 6957
		public static bool writeCanReachColony = false;

		// Token: 0x04001B2E RID: 6958
		public static bool writeMentalStateCalcs = false;

		// Token: 0x04001B2F RID: 6959
		public static bool writeWind = false;

		// Token: 0x04001B30 RID: 6960
		public static bool writeTerrain = false;

		// Token: 0x04001B31 RID: 6961
		public static bool writeApparelScore = false;

		// Token: 0x04001B32 RID: 6962
		public static bool writeWorkSettings = false;

		// Token: 0x04001B33 RID: 6963
		public static bool writeSkyManager = false;

		// Token: 0x04001B34 RID: 6964
		public static bool writeMemoryUsage = false;

		// Token: 0x04001B35 RID: 6965
		public static bool writeMapGameConditions = false;

		// Token: 0x04001B36 RID: 6966
		public static bool writeAttackTargets = false;

		// Token: 0x04001B37 RID: 6967
		public static bool writeRopesAndPens = false;

		// Token: 0x04001B38 RID: 6968
		public static bool writeRoomRoles = false;

		// Token: 0x04001B39 RID: 6969
		public static bool logIncapChance = false;

		// Token: 0x04001B3A RID: 6970
		public static bool logInput = false;

		// Token: 0x04001B3B RID: 6971
		public static bool logApparelGeneration = false;

		// Token: 0x04001B3C RID: 6972
		public static bool logLordToilTransitions = false;

		// Token: 0x04001B3D RID: 6973
		public static bool logGrammarResolution = false;

		// Token: 0x04001B3E RID: 6974
		public static bool logCombatLogMouseover = false;

		// Token: 0x04001B3F RID: 6975
		public static bool logCauseOfDeath = false;

		// Token: 0x04001B40 RID: 6976
		public static bool logMapLoad = false;

		// Token: 0x04001B41 RID: 6977
		public static bool logTutor = false;

		// Token: 0x04001B42 RID: 6978
		public static bool logSignals = false;

		// Token: 0x04001B43 RID: 6979
		public static bool logWorldPawnGC = false;

		// Token: 0x04001B44 RID: 6980
		public static bool logTaleRecording = false;

		// Token: 0x04001B45 RID: 6981
		public static bool logHourlyScreenshot = false;

		// Token: 0x04001B46 RID: 6982
		public static bool logFilthSummary = false;

		// Token: 0x04001B47 RID: 6983
		public static bool logCarriedBetweenJobs = false;

		// Token: 0x04001B48 RID: 6984
		public static bool logComplexGenPoints = false;

		// Token: 0x04001B49 RID: 6985
		public static bool debugApparelOptimize = false;

		// Token: 0x04001B4A RID: 6986
		public static bool showAllRoomStats = false;

		// Token: 0x04001B4B RID: 6987
		public static bool showFloatMenuWorkGivers = false;

		// Token: 0x04001B4C RID: 6988
		public static bool neverForceNormalSpeed = false;

		// Token: 0x04001B4D RID: 6989
		public static bool showArchitectMenuOrder = false;
	}
}

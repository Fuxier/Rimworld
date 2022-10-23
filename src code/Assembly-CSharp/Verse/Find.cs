using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000558 RID: 1368
	public static class Find
	{
		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x060029A9 RID: 10665 RVA: 0x0010A3A2 File Offset: 0x001085A2
		public static Root Root
		{
			get
			{
				return Current.Root;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x0010A3A9 File Offset: 0x001085A9
		public static SoundRoot SoundRoot
		{
			get
			{
				return Current.Root.soundRoot;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x0010A3B5 File Offset: 0x001085B5
		public static UIRoot UIRoot
		{
			get
			{
				if (!(Current.Root != null))
				{
					return null;
				}
				return Current.Root.uiRoot;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x0010A3D0 File Offset: 0x001085D0
		public static MusicManagerEntry MusicManagerEntry
		{
			get
			{
				return ((Root_Entry)Current.Root).musicManagerEntry;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x060029AD RID: 10669 RVA: 0x0010A3E1 File Offset: 0x001085E1
		public static MusicManagerPlay MusicManagerPlay
		{
			get
			{
				return ((Root_Play)Current.Root).musicManagerPlay;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x0010A3F2 File Offset: 0x001085F2
		public static LanguageWorker ActiveLanguageWorker
		{
			get
			{
				return LanguageDatabase.activeLanguage.Worker;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x060029AF RID: 10671 RVA: 0x0010A3FE File Offset: 0x001085FE
		public static Camera Camera
		{
			get
			{
				return Current.Camera;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x0010A405 File Offset: 0x00108605
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.CameraDriver;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x060029B1 RID: 10673 RVA: 0x0010A40C File Offset: 0x0010860C
		public static ColorCorrectionCurves CameraColor
		{
			get
			{
				return Current.ColorCorrectionCurves;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x0010A413 File Offset: 0x00108613
		public static Camera PawnCacheCamera
		{
			get
			{
				return PawnCacheCameraManager.PawnCacheCamera;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060029B3 RID: 10675 RVA: 0x0010A41A File Offset: 0x0010861A
		public static PawnCacheRenderer PawnCacheRenderer
		{
			get
			{
				return PawnCacheCameraManager.PawnCacheRenderer;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x0010A421 File Offset: 0x00108621
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.WorldCamera;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x0010A428 File Offset: 0x00108628
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.WorldCameraDriver;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x0010A42F File Offset: 0x0010862F
		public static WindowStack WindowStack
		{
			get
			{
				if (Find.UIRoot == null)
				{
					return null;
				}
				return Find.UIRoot.windows;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x0010A444 File Offset: 0x00108644
		public static ScreenshotModeHandler ScreenshotModeHandler
		{
			get
			{
				return Find.UIRoot.screenshotMode;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x0010A450 File Offset: 0x00108650
		public static MainButtonsRoot MainButtonsRoot
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mainButtonsRoot;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060029B9 RID: 10681 RVA: 0x0010A461 File Offset: 0x00108661
		public static MainTabsRoot MainTabsRoot
		{
			get
			{
				return Find.MainButtonsRoot.tabs;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x0010A46D File Offset: 0x0010866D
		public static MapInterface MapUI
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mapUI;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060029BB RID: 10683 RVA: 0x0010A47E File Offset: 0x0010867E
		public static AlertsReadout Alerts
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).alerts;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060029BC RID: 10684 RVA: 0x0010A48F File Offset: 0x0010868F
		public static Selector Selector
		{
			get
			{
				return Find.MapUI.selector;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060029BD RID: 10685 RVA: 0x0010A49B File Offset: 0x0010869B
		public static Targeter Targeter
		{
			get
			{
				return Find.MapUI.targeter;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060029BE RID: 10686 RVA: 0x0010A4A7 File Offset: 0x001086A7
		public static ColonistBar ColonistBar
		{
			get
			{
				return Find.MapUI.colonistBar;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060029BF RID: 10687 RVA: 0x0010A4B3 File Offset: 0x001086B3
		public static DesignatorManager DesignatorManager
		{
			get
			{
				return Find.MapUI.designatorManager;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060029C0 RID: 10688 RVA: 0x0010A4BF File Offset: 0x001086BF
		public static ReverseDesignatorDatabase ReverseDesignatorDatabase
		{
			get
			{
				return Find.MapUI.reverseDesignatorDatabase;
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x060029C1 RID: 10689 RVA: 0x0010A4CB File Offset: 0x001086CB
		public static GameInitData GameInitData
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.InitData;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x060029C2 RID: 10690 RVA: 0x0010A4E0 File Offset: 0x001086E0
		public static GameInfo GameInfo
		{
			get
			{
				return Current.Game.Info;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060029C3 RID: 10691 RVA: 0x0010A4EC File Offset: 0x001086EC
		public static Scenario Scenario
		{
			get
			{
				if (Current.Game != null && Current.Game.Scenario != null)
				{
					return Current.Game.Scenario;
				}
				if (ScenarioMaker.GeneratingScenario != null)
				{
					return ScenarioMaker.GeneratingScenario;
				}
				if (Find.UIRoot != null)
				{
					Page_ScenarioEditor page_ScenarioEditor = Find.WindowStack.WindowOfType<Page_ScenarioEditor>();
					if (page_ScenarioEditor != null)
					{
						return page_ScenarioEditor.EditingScenario;
					}
				}
				return null;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060029C4 RID: 10692 RVA: 0x0010A541 File Offset: 0x00108741
		public static World World
		{
			get
			{
				if (Current.Game == null || Current.Game.World == null)
				{
					return Current.CreatingWorld;
				}
				return Current.Game.World;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x0010A566 File Offset: 0x00108766
		public static List<Map> Maps
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.Maps;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060029C6 RID: 10694 RVA: 0x0010A57B File Offset: 0x0010877B
		public static Map CurrentMap
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.CurrentMap;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060029C7 RID: 10695 RVA: 0x0010A590 File Offset: 0x00108790
		public static Map AnyPlayerHomeMap
		{
			get
			{
				return Current.Game.AnyPlayerHomeMap;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060029C8 RID: 10696 RVA: 0x0010A59C File Offset: 0x0010879C
		public static Map RandomPlayerHomeMap
		{
			get
			{
				return Current.Game.RandomPlayerHomeMap;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x0010A5A8 File Offset: 0x001087A8
		public static StoryWatcher StoryWatcher
		{
			get
			{
				return Current.Game.storyWatcher;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x0010A5B4 File Offset: 0x001087B4
		public static ResearchManager ResearchManager
		{
			get
			{
				return Current.Game.researchManager;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x0010A5C0 File Offset: 0x001087C0
		public static Storyteller Storyteller
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.storyteller;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x0010A5D5 File Offset: 0x001087D5
		public static GameEnder GameEnder
		{
			get
			{
				return Current.Game.gameEnder;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060029CD RID: 10701 RVA: 0x0010A5E1 File Offset: 0x001087E1
		public static LetterStack LetterStack
		{
			get
			{
				return Current.Game.letterStack;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x0010A5ED File Offset: 0x001087ED
		public static Archive Archive
		{
			get
			{
				if (Find.History == null)
				{
					return null;
				}
				return Find.History.archive;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060029CF RID: 10703 RVA: 0x0010A602 File Offset: 0x00108802
		public static PlaySettings PlaySettings
		{
			get
			{
				return Current.Game.playSettings;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x0010A60E File Offset: 0x0010880E
		public static History History
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.history;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060029D1 RID: 10705 RVA: 0x0010A623 File Offset: 0x00108823
		public static TaleManager TaleManager
		{
			get
			{
				return Current.Game.taleManager;
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060029D2 RID: 10706 RVA: 0x0010A62F File Offset: 0x0010882F
		public static PlayLog PlayLog
		{
			get
			{
				return Current.Game.playLog;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x060029D3 RID: 10707 RVA: 0x0010A63B File Offset: 0x0010883B
		public static BattleLog BattleLog
		{
			get
			{
				return Current.Game.battleLog;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x060029D4 RID: 10708 RVA: 0x0010A647 File Offset: 0x00108847
		public static TickManager TickManager
		{
			get
			{
				return Current.Game.tickManager;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060029D5 RID: 10709 RVA: 0x0010A653 File Offset: 0x00108853
		public static Tutor Tutor
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.tutor;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060029D6 RID: 10710 RVA: 0x0010A668 File Offset: 0x00108868
		public static TutorialState TutorialState
		{
			get
			{
				return Current.Game.tutor.tutorialState;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x060029D7 RID: 10711 RVA: 0x0010A679 File Offset: 0x00108879
		public static ActiveLessonHandler ActiveLesson
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.tutor.activeLesson;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x060029D8 RID: 10712 RVA: 0x0010A693 File Offset: 0x00108893
		public static Autosaver Autosaver
		{
			get
			{
				return Current.Game.autosaver;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060029D9 RID: 10713 RVA: 0x0010A69F File Offset: 0x0010889F
		public static DateNotifier DateNotifier
		{
			get
			{
				return Current.Game.dateNotifier;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060029DA RID: 10714 RVA: 0x0010A6AB File Offset: 0x001088AB
		public static SignalManager SignalManager
		{
			get
			{
				return Current.Game.signalManager;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x0010A6B7 File Offset: 0x001088B7
		public static UniqueIDsManager UniqueIDsManager
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.uniqueIDsManager;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x0010A6CC File Offset: 0x001088CC
		public static QuestManager QuestManager
		{
			get
			{
				return Current.Game.questManager;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060029DD RID: 10717 RVA: 0x0010A6D8 File Offset: 0x001088D8
		public static TransportShipManager TransportShipManager
		{
			get
			{
				return Current.Game.transportShipManager;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x0010A6E4 File Offset: 0x001088E4
		public static GameComponent_Bossgroup BossgroupManager
		{
			get
			{
				return Current.Game.GetComponent<GameComponent_Bossgroup>();
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060029DF RID: 10719 RVA: 0x0010A6F0 File Offset: 0x001088F0
		public static StudyManager StudyManager
		{
			get
			{
				return Current.Game.studyManager;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060029E0 RID: 10720 RVA: 0x0010A6FC File Offset: 0x001088FC
		public static CustomXenogermDatabase CustomXenogermDatabase
		{
			get
			{
				return Current.Game.customXenogermDatabase;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060029E1 RID: 10721 RVA: 0x0010A708 File Offset: 0x00108908
		public static FactionManager FactionManager
		{
			get
			{
				return Find.World.factionManager;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060029E2 RID: 10722 RVA: 0x0010A714 File Offset: 0x00108914
		public static IdeoManager IdeoManager
		{
			get
			{
				return Find.World.ideoManager;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060029E3 RID: 10723 RVA: 0x0010A720 File Offset: 0x00108920
		public static WorldPawns WorldPawns
		{
			get
			{
				return Find.World.worldPawns;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x0010A72C File Offset: 0x0010892C
		public static WorldObjectsHolder WorldObjects
		{
			get
			{
				return Find.World.worldObjects;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060029E5 RID: 10725 RVA: 0x0010A738 File Offset: 0x00108938
		public static WorldGrid WorldGrid
		{
			get
			{
				return Find.World.grid;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060029E6 RID: 10726 RVA: 0x0010A744 File Offset: 0x00108944
		public static WorldDebugDrawer WorldDebugDrawer
		{
			get
			{
				return Find.World.debugDrawer;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060029E7 RID: 10727 RVA: 0x0010A750 File Offset: 0x00108950
		public static WorldPathGrid WorldPathGrid
		{
			get
			{
				return Find.World.pathGrid;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060029E8 RID: 10728 RVA: 0x0010A75C File Offset: 0x0010895C
		public static WorldDynamicDrawManager WorldDynamicDrawManager
		{
			get
			{
				return Find.World.dynamicDrawManager;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060029E9 RID: 10729 RVA: 0x0010A768 File Offset: 0x00108968
		public static WorldPathFinder WorldPathFinder
		{
			get
			{
				return Find.World.pathFinder;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060029EA RID: 10730 RVA: 0x0010A774 File Offset: 0x00108974
		public static WorldPathPool WorldPathPool
		{
			get
			{
				return Find.World.pathPool;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060029EB RID: 10731 RVA: 0x0010A780 File Offset: 0x00108980
		public static WorldReachability WorldReachability
		{
			get
			{
				return Find.World.reachability;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060029EC RID: 10732 RVA: 0x0010A78C File Offset: 0x0010898C
		public static WorldFloodFiller WorldFloodFiller
		{
			get
			{
				return Find.World.floodFiller;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x0010A798 File Offset: 0x00108998
		public static WorldFeatures WorldFeatures
		{
			get
			{
				return Find.World.features;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x0010A7A4 File Offset: 0x001089A4
		public static WorldInterface WorldInterface
		{
			get
			{
				return Find.World.UI;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060029EF RID: 10735 RVA: 0x0010A7B0 File Offset: 0x001089B0
		public static WorldSelector WorldSelector
		{
			get
			{
				return Find.WorldInterface.selector;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060029F0 RID: 10736 RVA: 0x0010A7BC File Offset: 0x001089BC
		public static WorldTargeter WorldTargeter
		{
			get
			{
				return Find.WorldInterface.targeter;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x0010A7C8 File Offset: 0x001089C8
		public static WorldRoutePlanner WorldRoutePlanner
		{
			get
			{
				return Find.WorldInterface.routePlanner;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x0010A7D4 File Offset: 0x001089D4
		public static TilePicker TilePicker
		{
			get
			{
				return Find.WorldInterface.tilePicker;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x0010A7E0 File Offset: 0x001089E0
		public static HistoryEventsManager HistoryEventsManager
		{
			get
			{
				return Find.History.historyEventsManager;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x060029F4 RID: 10740 RVA: 0x0010A7EC File Offset: 0x001089EC
		public static GoodwillSituationManager GoodwillSituationManager
		{
			get
			{
				return Find.FactionManager.goodwillSituationManager;
			}
		}
	}
}

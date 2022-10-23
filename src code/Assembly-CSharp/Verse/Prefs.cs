using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200058B RID: 1419
	public static class Prefs
	{
		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002B3D RID: 11069 RVA: 0x0011451E File Offset: 0x0011271E
		// (set) Token: 0x06002B3E RID: 11070 RVA: 0x0011452A File Offset: 0x0011272A
		public static float VolumeMaster
		{
			get
			{
				return Prefs.data.volumeMaster;
			}
			set
			{
				if (Prefs.data.volumeMaster == value)
				{
					return;
				}
				Prefs.data.volumeMaster = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06002B3F RID: 11071 RVA: 0x0011454A File Offset: 0x0011274A
		// (set) Token: 0x06002B40 RID: 11072 RVA: 0x00114556 File Offset: 0x00112756
		public static float VolumeGame
		{
			get
			{
				return Prefs.data.volumeGame;
			}
			set
			{
				if (Prefs.data.volumeGame == value)
				{
					return;
				}
				Prefs.data.volumeGame = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06002B41 RID: 11073 RVA: 0x00114576 File Offset: 0x00112776
		// (set) Token: 0x06002B42 RID: 11074 RVA: 0x00114582 File Offset: 0x00112782
		public static float VolumeMusic
		{
			get
			{
				return Prefs.data.volumeMusic;
			}
			set
			{
				if (Prefs.data.volumeMusic == value)
				{
					return;
				}
				Prefs.data.volumeMusic = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002B43 RID: 11075 RVA: 0x001145A2 File Offset: 0x001127A2
		// (set) Token: 0x06002B44 RID: 11076 RVA: 0x001145AE File Offset: 0x001127AE
		public static float VolumeAmbient
		{
			get
			{
				return Prefs.data.volumeAmbient;
			}
			set
			{
				if (Prefs.data.volumeAmbient == value)
				{
					return;
				}
				Prefs.data.volumeAmbient = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06002B45 RID: 11077 RVA: 0x001145CE File Offset: 0x001127CE
		// (set) Token: 0x06002B46 RID: 11078 RVA: 0x001145DA File Offset: 0x001127DA
		public static float VolumeUI
		{
			get
			{
				return Prefs.data.volumeUI;
			}
			set
			{
				if (Prefs.data.volumeUI == value)
				{
					return;
				}
				Prefs.data.volumeUI = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06002B47 RID: 11079 RVA: 0x001145FA File Offset: 0x001127FA
		// (set) Token: 0x06002B48 RID: 11080 RVA: 0x00114606 File Offset: 0x00112806
		[Obsolete]
		public static bool ExtremeDifficultyUnlocked
		{
			get
			{
				return Prefs.data.extremeDifficultyUnlocked;
			}
			set
			{
				if (Prefs.data.extremeDifficultyUnlocked == value)
				{
					return;
				}
				Prefs.data.extremeDifficultyUnlocked = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002B49 RID: 11081 RVA: 0x00114626 File Offset: 0x00112826
		// (set) Token: 0x06002B4A RID: 11082 RVA: 0x00114632 File Offset: 0x00112832
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.data.adaptiveTrainingEnabled;
			}
			set
			{
				if (Prefs.data.adaptiveTrainingEnabled == value)
				{
					return;
				}
				Prefs.data.adaptiveTrainingEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002B4B RID: 11083 RVA: 0x00114652 File Offset: 0x00112852
		// (set) Token: 0x06002B4C RID: 11084 RVA: 0x0011465E File Offset: 0x0011285E
		public static bool SteamDeckKeyboardMode
		{
			get
			{
				return Prefs.data.steamDeckKeyboardMode;
			}
			set
			{
				if (Prefs.data.steamDeckKeyboardMode == value)
				{
					return;
				}
				Prefs.data.steamDeckKeyboardMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002B4D RID: 11085 RVA: 0x0011467E File Offset: 0x0011287E
		// (set) Token: 0x06002B4E RID: 11086 RVA: 0x0011468A File Offset: 0x0011288A
		public static bool EdgeScreenScroll
		{
			get
			{
				return Prefs.data.edgeScreenScroll;
			}
			set
			{
				if (Prefs.data.edgeScreenScroll == value)
				{
					return;
				}
				Prefs.data.edgeScreenScroll = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002B4F RID: 11087 RVA: 0x001146AA File Offset: 0x001128AA
		// (set) Token: 0x06002B50 RID: 11088 RVA: 0x001146B6 File Offset: 0x001128B6
		public static float ScreenShakeIntensity
		{
			get
			{
				return Prefs.data.screenShakeIntensity;
			}
			set
			{
				if (Prefs.data.screenShakeIntensity == value)
				{
					return;
				}
				Prefs.data.screenShakeIntensity = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002B51 RID: 11089 RVA: 0x001146D6 File Offset: 0x001128D6
		// (set) Token: 0x06002B52 RID: 11090 RVA: 0x001146E2 File Offset: 0x001128E2
		public static bool RunInBackground
		{
			get
			{
				return Prefs.data.runInBackground;
			}
			set
			{
				if (Prefs.data.runInBackground == value)
				{
					return;
				}
				Prefs.data.runInBackground = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002B53 RID: 11091 RVA: 0x00114702 File Offset: 0x00112902
		// (set) Token: 0x06002B54 RID: 11092 RVA: 0x0011470E File Offset: 0x0011290E
		public static TemperatureDisplayMode TemperatureMode
		{
			get
			{
				return Prefs.data.temperatureMode;
			}
			set
			{
				if (Prefs.data.temperatureMode == value)
				{
					return;
				}
				Prefs.data.temperatureMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002B55 RID: 11093 RVA: 0x0011472E File Offset: 0x0011292E
		// (set) Token: 0x06002B56 RID: 11094 RVA: 0x0011473A File Offset: 0x0011293A
		public static float AutosaveIntervalDays
		{
			get
			{
				return Prefs.data.autosaveIntervalDays;
			}
			set
			{
				if (Prefs.data.autosaveIntervalDays == value)
				{
					return;
				}
				Prefs.data.autosaveIntervalDays = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002B57 RID: 11095 RVA: 0x0011475A File Offset: 0x0011295A
		// (set) Token: 0x06002B58 RID: 11096 RVA: 0x00114766 File Offset: 0x00112966
		public static bool CustomCursorEnabled
		{
			get
			{
				return Prefs.data.customCursorEnabled;
			}
			set
			{
				if (Prefs.data.customCursorEnabled == value)
				{
					return;
				}
				Prefs.data.customCursorEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002B59 RID: 11097 RVA: 0x00114786 File Offset: 0x00112986
		// (set) Token: 0x06002B5A RID: 11098 RVA: 0x00114792 File Offset: 0x00112992
		public static AnimalNameDisplayMode AnimalNameMode
		{
			get
			{
				return Prefs.data.animalNameMode;
			}
			set
			{
				if (Prefs.data.animalNameMode == value)
				{
					return;
				}
				Prefs.data.animalNameMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x001147B2 File Offset: 0x001129B2
		// (set) Token: 0x06002B5C RID: 11100 RVA: 0x001147BE File Offset: 0x001129BE
		public static MechNameDisplayMode MechNameMode
		{
			get
			{
				return Prefs.data.mechNameMode;
			}
			set
			{
				if (Prefs.data.mechNameMode == value)
				{
					return;
				}
				Prefs.data.mechNameMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x001147DE File Offset: 0x001129DE
		// (set) Token: 0x06002B5E RID: 11102 RVA: 0x001147EA File Offset: 0x001129EA
		public static ShowWeaponsUnderPortraitMode ShowWeaponsUnderPortraitMode
		{
			get
			{
				return Prefs.data.showWeaponsUnderPortraitMode;
			}
			set
			{
				if (Prefs.data.showWeaponsUnderPortraitMode == value)
				{
					return;
				}
				Prefs.data.showWeaponsUnderPortraitMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002B5F RID: 11103 RVA: 0x0011480A File Offset: 0x00112A0A
		// (set) Token: 0x06002B60 RID: 11104 RVA: 0x00114820 File Offset: 0x00112A20
		public static bool DevMode
		{
			get
			{
				return Prefs.data == null || Prefs.data.devMode;
			}
			set
			{
				if (Prefs.data.devMode == value)
				{
					return;
				}
				Prefs.data.devMode = value;
				if (!Prefs.data.devMode)
				{
					Prefs.data.logVerbose = false;
					Prefs.data.resetModsConfigOnCrash = true;
					DebugSettings.godMode = false;
				}
				Prefs.Apply();
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x00114873 File Offset: 0x00112A73
		// (set) Token: 0x06002B62 RID: 11106 RVA: 0x00114888 File Offset: 0x00112A88
		public static bool ResetModsConfigOnCrash
		{
			get
			{
				return Prefs.data == null || Prefs.data.resetModsConfigOnCrash;
			}
			set
			{
				if (Prefs.data.resetModsConfigOnCrash == value)
				{
					return;
				}
				Prefs.data.resetModsConfigOnCrash = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x001148A8 File Offset: 0x00112AA8
		// (set) Token: 0x06002B64 RID: 11108 RVA: 0x001148BD File Offset: 0x00112ABD
		public static bool SimulateNotOwningRoyalty
		{
			get
			{
				return Prefs.data != null && Prefs.data.simulateNotOwningRoyalty;
			}
			set
			{
				if (Prefs.data.simulateNotOwningRoyalty == value)
				{
					return;
				}
				Prefs.data.simulateNotOwningRoyalty = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002B65 RID: 11109 RVA: 0x001148DD File Offset: 0x00112ADD
		// (set) Token: 0x06002B66 RID: 11110 RVA: 0x001148F2 File Offset: 0x00112AF2
		public static bool SimulateNotOwningIdology
		{
			get
			{
				return Prefs.data != null && Prefs.data.simulateNotOwningIdeology;
			}
			set
			{
				if (Prefs.data.simulateNotOwningIdeology == value)
				{
					return;
				}
				Prefs.data.simulateNotOwningIdeology = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x00114912 File Offset: 0x00112B12
		// (set) Token: 0x06002B68 RID: 11112 RVA: 0x00114927 File Offset: 0x00112B27
		public static bool SimulateNotOwningBiotech
		{
			get
			{
				return Prefs.data != null && Prefs.data.simulateNotOwningBiotech;
			}
			set
			{
				if (Prefs.data.simulateNotOwningBiotech == value)
				{
					return;
				}
				Prefs.data.simulateNotOwningBiotech = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002B69 RID: 11113 RVA: 0x00114947 File Offset: 0x00112B47
		// (set) Token: 0x06002B6A RID: 11114 RVA: 0x00114953 File Offset: 0x00112B53
		public static List<string> PreferredNames
		{
			get
			{
				return Prefs.data.preferredNames;
			}
			set
			{
				if (Prefs.data.preferredNames == value)
				{
					return;
				}
				Prefs.data.preferredNames = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002B6B RID: 11115 RVA: 0x00114973 File Offset: 0x00112B73
		// (set) Token: 0x06002B6C RID: 11116 RVA: 0x0011497F File Offset: 0x00112B7F
		public static string LangFolderName
		{
			get
			{
				return Prefs.data.langFolderName;
			}
			set
			{
				if (Prefs.data.langFolderName == value)
				{
					return;
				}
				Prefs.data.langFolderName = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x001149A4 File Offset: 0x00112BA4
		// (set) Token: 0x06002B6E RID: 11118 RVA: 0x001149B0 File Offset: 0x00112BB0
		public static bool LogVerbose
		{
			get
			{
				return Prefs.data.logVerbose;
			}
			set
			{
				if (Prefs.data.logVerbose == value)
				{
					return;
				}
				Prefs.data.logVerbose = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002B6F RID: 11119 RVA: 0x001149D0 File Offset: 0x00112BD0
		// (set) Token: 0x06002B70 RID: 11120 RVA: 0x001149E5 File Offset: 0x00112BE5
		public static bool PauseOnError
		{
			get
			{
				return Prefs.data != null && Prefs.data.pauseOnError;
			}
			set
			{
				Prefs.data.pauseOnError = value;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002B71 RID: 11121 RVA: 0x001149F2 File Offset: 0x00112BF2
		// (set) Token: 0x06002B72 RID: 11122 RVA: 0x001149FE File Offset: 0x00112BFE
		public static bool PauseOnLoad
		{
			get
			{
				return Prefs.data.pauseOnLoad;
			}
			set
			{
				Prefs.data.pauseOnLoad = value;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002B73 RID: 11123 RVA: 0x00114A0B File Offset: 0x00112C0B
		// (set) Token: 0x06002B74 RID: 11124 RVA: 0x00114A17 File Offset: 0x00112C17
		public static AutomaticPauseMode AutomaticPauseMode
		{
			get
			{
				return Prefs.data.automaticPauseMode;
			}
			set
			{
				Prefs.data.automaticPauseMode = value;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x00114A24 File Offset: 0x00112C24
		// (set) Token: 0x06002B76 RID: 11126 RVA: 0x00114A30 File Offset: 0x00112C30
		public static bool ShowRealtimeClock
		{
			get
			{
				return Prefs.data.showRealtimeClock;
			}
			set
			{
				Prefs.data.showRealtimeClock = value;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002B77 RID: 11127 RVA: 0x00114A3D File Offset: 0x00112C3D
		// (set) Token: 0x06002B78 RID: 11128 RVA: 0x00114A49 File Offset: 0x00112C49
		public static bool DisableTinyText
		{
			get
			{
				return Prefs.data.disableTinyText;
			}
			set
			{
				Prefs.data.disableTinyText = value;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002B79 RID: 11129 RVA: 0x00114A56 File Offset: 0x00112C56
		// (set) Token: 0x06002B7A RID: 11130 RVA: 0x00114A62 File Offset: 0x00112C62
		public static bool TestMapSizes
		{
			get
			{
				return Prefs.data.testMapSizes;
			}
			set
			{
				Prefs.data.testMapSizes = value;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002B7B RID: 11131 RVA: 0x00114A6F File Offset: 0x00112C6F
		// (set) Token: 0x06002B7C RID: 11132 RVA: 0x00114A7B File Offset: 0x00112C7B
		public static int MaxNumberOfPlayerSettlements
		{
			get
			{
				return Prefs.data.maxNumberOfPlayerSettlements;
			}
			set
			{
				Prefs.data.maxNumberOfPlayerSettlements = value;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002B7D RID: 11133 RVA: 0x00114A88 File Offset: 0x00112C88
		// (set) Token: 0x06002B7E RID: 11134 RVA: 0x00114A94 File Offset: 0x00112C94
		public static bool PlantWindSway
		{
			get
			{
				return Prefs.data.plantWindSway;
			}
			set
			{
				Prefs.data.plantWindSway = value;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002B7F RID: 11135 RVA: 0x00114AA1 File Offset: 0x00112CA1
		// (set) Token: 0x06002B80 RID: 11136 RVA: 0x00114AAD File Offset: 0x00112CAD
		public static bool TextureCompression
		{
			get
			{
				return Prefs.data.textureCompression;
			}
			set
			{
				Prefs.data.textureCompression = value;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002B81 RID: 11137 RVA: 0x00114ABA File Offset: 0x00112CBA
		// (set) Token: 0x06002B82 RID: 11138 RVA: 0x00114AC6 File Offset: 0x00112CC6
		public static bool ResourceReadoutCategorized
		{
			get
			{
				return Prefs.data.resourceReadoutCategorized;
			}
			set
			{
				if (value == Prefs.data.resourceReadoutCategorized)
				{
					return;
				}
				Prefs.data.resourceReadoutCategorized = value;
				Prefs.Save();
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002B83 RID: 11139 RVA: 0x00114AE6 File Offset: 0x00112CE6
		// (set) Token: 0x06002B84 RID: 11140 RVA: 0x00114AF2 File Offset: 0x00112CF2
		public static float UIScale
		{
			get
			{
				return Prefs.data.uiScale;
			}
			set
			{
				Prefs.data.uiScale = value;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002B85 RID: 11141 RVA: 0x00114AFF File Offset: 0x00112CFF
		// (set) Token: 0x06002B86 RID: 11142 RVA: 0x00114B0B File Offset: 0x00112D0B
		public static int ScreenWidth
		{
			get
			{
				return Prefs.data.screenWidth;
			}
			set
			{
				Prefs.data.screenWidth = value;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002B87 RID: 11143 RVA: 0x00114B18 File Offset: 0x00112D18
		// (set) Token: 0x06002B88 RID: 11144 RVA: 0x00114B24 File Offset: 0x00112D24
		public static int ScreenHeight
		{
			get
			{
				return Prefs.data.screenHeight;
			}
			set
			{
				Prefs.data.screenHeight = value;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002B89 RID: 11145 RVA: 0x00114B31 File Offset: 0x00112D31
		// (set) Token: 0x06002B8A RID: 11146 RVA: 0x00114B3D File Offset: 0x00112D3D
		public static bool FullScreen
		{
			get
			{
				return Prefs.data.fullscreen;
			}
			set
			{
				Prefs.data.fullscreen = value;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002B8B RID: 11147 RVA: 0x00114B4A File Offset: 0x00112D4A
		// (set) Token: 0x06002B8C RID: 11148 RVA: 0x00114B56 File Offset: 0x00112D56
		public static bool HatsOnlyOnMap
		{
			get
			{
				return Prefs.data.hatsOnlyOnMap;
			}
			set
			{
				if (Prefs.data.hatsOnlyOnMap == value)
				{
					return;
				}
				Prefs.data.hatsOnlyOnMap = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002B8D RID: 11149 RVA: 0x00114B76 File Offset: 0x00112D76
		// (set) Token: 0x06002B8E RID: 11150 RVA: 0x00114B82 File Offset: 0x00112D82
		public static float MapDragSensitivity
		{
			get
			{
				return Prefs.data.mapDragSensitivity;
			}
			set
			{
				Prefs.data.mapDragSensitivity = value;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002B8F RID: 11151 RVA: 0x00114B90 File Offset: 0x00112D90
		// (set) Token: 0x06002B90 RID: 11152 RVA: 0x00114BD7 File Offset: 0x00112DD7
		public static ExpansionDef BackgroundImageExpansion
		{
			get
			{
				if (Prefs.data.backgroundExpansionId != null)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(Prefs.data.backgroundExpansionId);
					if (expansionWithIdentifier != null && expansionWithIdentifier.Status != ExpansionStatus.NotInstalled)
					{
						return expansionWithIdentifier;
					}
				}
				ExpansionDef lastInstalledExpansion = ModsConfig.LastInstalledExpansion;
				if (lastInstalledExpansion != null)
				{
					return lastInstalledExpansion;
				}
				return ExpansionDefOf.Core;
			}
			set
			{
				Prefs.data.backgroundExpansionId = ((value != null) ? value.linkedMod : null);
				((UI_BackgroundMain)UIMenuBackgroundManager.background).overrideBGImage = ((value != null) ? value.BackgroundImage : null);
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002B91 RID: 11153 RVA: 0x00114C0A File Offset: 0x00112E0A
		// (set) Token: 0x06002B92 RID: 11154 RVA: 0x00114C16 File Offset: 0x00112E16
		public static bool RandomBackgroundImage
		{
			get
			{
				return Prefs.data.randomBackground;
			}
			set
			{
				Prefs.data.randomBackground = value;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002B93 RID: 11155 RVA: 0x00114C23 File Offset: 0x00112E23
		// (set) Token: 0x06002B94 RID: 11156 RVA: 0x00114C2F File Offset: 0x00112E2F
		public static List<string> DebugActionsPalette
		{
			get
			{
				return Prefs.data.debugActionPalette;
			}
			set
			{
				if (Prefs.data.debugActionPalette != value)
				{
					Prefs.data.debugActionPalette = value;
					Prefs.Save();
				}
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002B95 RID: 11157 RVA: 0x00114C4E File Offset: 0x00112E4E
		// (set) Token: 0x06002B96 RID: 11158 RVA: 0x00114C5A File Offset: 0x00112E5A
		public static Vector2 DevPalettePosition
		{
			get
			{
				return Prefs.data.devPalettePosition;
			}
			set
			{
				if (Prefs.data.devPalettePosition != value)
				{
					Prefs.data.devPalettePosition = value;
					Prefs.Save();
				}
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002B97 RID: 11159 RVA: 0x00114C7E File Offset: 0x00112E7E
		// (set) Token: 0x06002B98 RID: 11160 RVA: 0x00114C8A File Offset: 0x00112E8A
		public static bool SmoothCameraJumps
		{
			get
			{
				return Prefs.data.smoothCameraJumps;
			}
			set
			{
				if (Prefs.data.smoothCameraJumps == value)
				{
					return;
				}
				Prefs.data.smoothCameraJumps = value;
				Prefs.Apply();
			}
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x00114CAC File Offset: 0x00112EAC
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.PrefsFilePath).Exists;
			Prefs.data = new PrefsData();
			Prefs.data = DirectXmlLoader.ItemFromXmlFile<PrefsData>(GenFilePaths.PrefsFilePath, true);
			BackCompatibility.PrefsDataPostLoad(Prefs.data);
			if (flag)
			{
				Prefs.data.langFolderName = LanguageDatabase.SystemLanguageFolderName();
				Prefs.data.uiScale = ResolutionUtility.GetRecommendedUIScale(Prefs.data.screenWidth, Prefs.data.screenHeight);
			}
			if (DevModePermanentlyDisabledUtility.Disabled)
			{
				Prefs.DevMode = false;
			}
			Prefs.Apply();
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x00114D38 File Offset: 0x00112F38
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(Prefs.data, typeof(PrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.PrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.PrefsFilePath, ex.ToString()));
				Log.Error("Exception saving prefs: " + ex);
			}
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x00114DC0 File Offset: 0x00112FC0
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x00114DCC File Offset: 0x00112FCC
		public static void Notify_NewExpansion()
		{
			Prefs.data.backgroundExpansionId = null;
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x00114DDC File Offset: 0x00112FDC
		public static NameTriple RandomPreferredName()
		{
			string rawName;
			if ((from name in Prefs.PreferredNames
			where !name.NullOrEmpty()
			select name).TryRandomElement(out rawName))
			{
				return NameTriple.FromString(rawName, false);
			}
			return null;
		}

		// Token: 0x04001C61 RID: 7265
		private static PrefsData data;
	}
}

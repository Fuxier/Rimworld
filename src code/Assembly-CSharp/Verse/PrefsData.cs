using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000596 RID: 1430
	public class PrefsData
	{
		// Token: 0x06002BA5 RID: 11173 RVA: 0x001150C8 File Offset: 0x001132C8
		public void Apply()
		{
			if (!UnityData.IsInMainThread)
			{
				return;
			}
			if (this.customCursorEnabled)
			{
				CustomCursor.Activate();
			}
			else
			{
				CustomCursor.Deactivate();
			}
			AudioListener.volume = this.volumeMaster;
			Application.runInBackground = this.runInBackground;
			if (this.screenWidth == 0 || this.screenHeight == 0)
			{
				ResolutionUtility.SetNativeResolutionRaw();
				return;
			}
			ResolutionUtility.SetResolutionRaw(this.screenWidth, this.screenHeight, !ResolutionUtility.BorderlessFullscreen && this.fullscreen);
		}

		// Token: 0x04001C77 RID: 7287
		public float volumeMaster = 0.8f;

		// Token: 0x04001C78 RID: 7288
		public float volumeGame = 1f;

		// Token: 0x04001C79 RID: 7289
		public float volumeMusic = 0.4f;

		// Token: 0x04001C7A RID: 7290
		public float volumeAmbient = 1f;

		// Token: 0x04001C7B RID: 7291
		public float volumeUI = 1f;

		// Token: 0x04001C7C RID: 7292
		public int screenWidth;

		// Token: 0x04001C7D RID: 7293
		public int screenHeight;

		// Token: 0x04001C7E RID: 7294
		public bool fullscreen;

		// Token: 0x04001C7F RID: 7295
		public float uiScale = 1f;

		// Token: 0x04001C80 RID: 7296
		public bool customCursorEnabled = true;

		// Token: 0x04001C81 RID: 7297
		public bool hatsOnlyOnMap;

		// Token: 0x04001C82 RID: 7298
		public bool plantWindSway = true;

		// Token: 0x04001C83 RID: 7299
		public float screenShakeIntensity = 1f;

		// Token: 0x04001C84 RID: 7300
		public bool textureCompression = true;

		// Token: 0x04001C85 RID: 7301
		public bool showRealtimeClock;

		// Token: 0x04001C86 RID: 7302
		public bool disableTinyText;

		// Token: 0x04001C87 RID: 7303
		public AnimalNameDisplayMode animalNameMode;

		// Token: 0x04001C88 RID: 7304
		public MechNameDisplayMode mechNameMode = MechNameDisplayMode.WhileDrafted;

		// Token: 0x04001C89 RID: 7305
		public string backgroundExpansionId;

		// Token: 0x04001C8A RID: 7306
		public bool randomBackground;

		// Token: 0x04001C8B RID: 7307
		public ShowWeaponsUnderPortraitMode showWeaponsUnderPortraitMode = ShowWeaponsUnderPortraitMode.WhileDrafted;

		// Token: 0x04001C8C RID: 7308
		[Obsolete]
		public bool extremeDifficultyUnlocked;

		// Token: 0x04001C8D RID: 7309
		public bool adaptiveTrainingEnabled = true;

		// Token: 0x04001C8E RID: 7310
		public bool steamDeckKeyboardMode;

		// Token: 0x04001C8F RID: 7311
		public List<string> preferredNames = new List<string>();

		// Token: 0x04001C90 RID: 7312
		public bool resourceReadoutCategorized;

		// Token: 0x04001C91 RID: 7313
		public bool runInBackground;

		// Token: 0x04001C92 RID: 7314
		public bool edgeScreenScroll = true;

		// Token: 0x04001C93 RID: 7315
		public TemperatureDisplayMode temperatureMode;

		// Token: 0x04001C94 RID: 7316
		public float autosaveIntervalDays = 1f;

		// Token: 0x04001C95 RID: 7317
		public bool testMapSizes;

		// Token: 0x04001C96 RID: 7318
		[LoadAlias("maxNumberOfPlayerHomes")]
		public int maxNumberOfPlayerSettlements = 1;

		// Token: 0x04001C97 RID: 7319
		public bool pauseOnLoad;

		// Token: 0x04001C98 RID: 7320
		public AutomaticPauseMode automaticPauseMode = AutomaticPauseMode.MajorThreat;

		// Token: 0x04001C99 RID: 7321
		public float mapDragSensitivity = 1.3f;

		// Token: 0x04001C9A RID: 7322
		public bool smoothCameraJumps = true;

		// Token: 0x04001C9B RID: 7323
		[Unsaved(true)]
		public bool? pauseOnUrgentLetter;

		// Token: 0x04001C9C RID: 7324
		public bool devMode;

		// Token: 0x04001C9D RID: 7325
		public List<string> debugActionPalette = new List<string>();

		// Token: 0x04001C9E RID: 7326
		public Vector2 devPalettePosition;

		// Token: 0x04001C9F RID: 7327
		public string langFolderName = "unknown";

		// Token: 0x04001CA0 RID: 7328
		public bool logVerbose;

		// Token: 0x04001CA1 RID: 7329
		public bool pauseOnError;

		// Token: 0x04001CA2 RID: 7330
		public bool resetModsConfigOnCrash = true;

		// Token: 0x04001CA3 RID: 7331
		public bool simulateNotOwningRoyalty;

		// Token: 0x04001CA4 RID: 7332
		public bool simulateNotOwningIdeology;

		// Token: 0x04001CA5 RID: 7333
		public bool simulateNotOwningBiotech;
	}
}

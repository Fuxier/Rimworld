using System;
using System.IO;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000599 RID: 1433
	public static class ScreenshotTaker
	{
		// Token: 0x06002BAF RID: 11183 RVA: 0x001153FC File Offset: 0x001135FC
		public static void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (KeyBindingDefOf.TakeScreenshot.JustPressed || ScreenshotTaker.takeScreenshot)
			{
				ScreenshotTaker.TakeShot();
				ScreenshotTaker.takeScreenshot = false;
			}
			if (Time.frameCount == ScreenshotTaker.lastShotFrame + 1)
			{
				if (ScreenshotTaker.suppressMessage)
				{
					ScreenshotTaker.suppressMessage = false;
					return;
				}
				Messages.Message("MessageScreenshotSavedAs".Translate(ScreenshotTaker.lastShotFilePath), MessageTypeDefOf.TaskCompletion, false);
			}
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x0011546F File Offset: 0x0011366F
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = (ScreenshotTaker.suppressMessage = true);
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x00115480 File Offset: 0x00113680
		private static void TakeShot()
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				try
				{
					SteamScreenshots.TriggerScreenshot();
					return;
				}
				catch (Exception arg)
				{
					Log.Warning("Could not take Steam screenshot. Steam offline? Taking normal screenshot. Exception: " + arg);
					ScreenshotTaker.TakeNonSteamShot();
					return;
				}
			}
			ScreenshotTaker.TakeNonSteamShot();
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x001154D0 File Offset: 0x001136D0
		private static void TakeNonSteamShot()
		{
			string screenshotFolderPath = GenFilePaths.ScreenshotFolderPath;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(screenshotFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				string text;
				do
				{
					ScreenshotTaker.screenshotCount++;
					text = string.Concat(new object[]
					{
						screenshotFolderPath,
						Path.DirectorySeparatorChar.ToString(),
						"screenshot",
						ScreenshotTaker.screenshotCount,
						".png"
					});
				}
				while (File.Exists(text));
				ScreenCapture.CaptureScreenshot(text);
				ScreenshotTaker.lastShotFrame = Time.frameCount;
				ScreenshotTaker.lastShotFilePath = text;
			}
			catch (Exception ex)
			{
				Log.Error("Failed to save screenshot in " + screenshotFolderPath + "\nException follows:\n" + ex.ToString());
			}
		}

		// Token: 0x04001CAD RID: 7341
		private static int lastShotFrame = -999;

		// Token: 0x04001CAE RID: 7342
		private static int screenshotCount = 0;

		// Token: 0x04001CAF RID: 7343
		private static string lastShotFilePath;

		// Token: 0x04001CB0 RID: 7344
		private static bool suppressMessage;

		// Token: 0x04001CB1 RID: 7345
		private static bool takeScreenshot;
	}
}

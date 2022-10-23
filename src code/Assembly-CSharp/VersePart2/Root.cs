using System;
using System.IO;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Analytics;
using Verse.AI;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000177 RID: 375
	public abstract class Root : MonoBehaviour
	{
		// Token: 0x06000A2C RID: 2604 RVA: 0x000313E8 File Offset: 0x0002F5E8
		public virtual void Start()
		{
			try
			{
				CultureInfoUtility.EnsureEnglish();
				Current.Notify_LoadedSceneChanged();
				GlobalTextureAtlasManager.FreeAllRuntimeAtlases();
				Root.CheckGlobalInit();
				Action action = delegate()
				{
					DeepProfiler.Start("Misc Init (InitializingInterface)");
					try
					{
						this.soundRoot = new SoundRoot();
						DeepProfiler.Start("Instantiate UIRoot");
						if (GenScene.InPlayScene)
						{
							this.uiRoot = new UIRoot_Play();
						}
						else if (GenScene.InEntryScene)
						{
							this.uiRoot = new UIRoot_Entry();
						}
						DeepProfiler.End();
						this.uiRoot.Init();
						Messages.Notify_LoadedLevelChanged();
						if (Current.SubcameraDriver != null)
						{
							Current.SubcameraDriver.Init();
						}
					}
					finally
					{
						DeepProfiler.End();
					}
				};
				if (!PlayDataLoader.Loaded)
				{
					Application.runInBackground = true;
					LongEventHandler.QueueLongEvent(delegate()
					{
						PlayDataLoader.LoadAllPlayData(false);
					}, null, true, null, true);
					LongEventHandler.QueueLongEvent(action, "InitializingInterface", false, null, true);
				}
				else
				{
					action();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg);
			}
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00031488 File Offset: 0x0002F688
		private static void CheckGlobalInit()
		{
			if (Root.globalInitDone)
			{
				return;
			}
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs != null && commandLineArgs.Length > 1)
			{
				Log.Message("Command line arguments: " + GenText.ToSpaceList(commandLineArgs.Skip(1)));
			}
			PerformanceReporting.enabled = false;
			Application.targetFrameRate = 60;
			UnityDataInitializer.CopyUnityData();
			SteamManager.InitIfNeeded();
			VersionControl.LogVersionNumber();
			Prefs.Init();
			Application.logMessageReceivedThreaded += Log.Notify_MessageReceivedThreadedInternal;
			if (Prefs.DevMode)
			{
				StaticConstructorOnStartupUtility.ReportProbablyMissingAttributes();
			}
			LongEventHandler.QueueLongEvent(new Action(StaticConstructorOnStartupUtility.CallAll), null, false, null, true);
			Root.globalInitDone = true;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00031520 File Offset: 0x0002F720
		public virtual void Update()
		{
			try
			{
				ResolutionUtility.Update();
				RealTime.Update();
				bool flag;
				LongEventHandler.LongEventsUpdate(out flag);
				if (flag)
				{
					this.destroyed = true;
				}
				else if (!LongEventHandler.ShouldWaitForEvent)
				{
					Rand.EnsureStateStackEmpty();
					Widgets.EnsureMousePositionStackEmpty();
					SteamManager.Update();
					PortraitsCache.PortraitsCacheUpdate();
					AttackTargetsCache.AttackTargetsCacheStaticUpdate();
					Pawn_MeleeVerbs.PawnMeleeVerbsStaticUpdate();
					Storyteller.StorytellerStaticUpdate();
					CaravanInventoryUtility.CaravanInventoryUtilityStaticUpdate();
					this.uiRoot.UIRootUpdate();
					if (Time.frameCount > 3 && !Root.prefsApplied)
					{
						Root.prefsApplied = true;
						Prefs.Apply();
					}
					this.soundRoot.Update();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in Update(): " + arg);
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x000315D0 File Offset: 0x0002F7D0
		public void OnGUI()
		{
			try
			{
				if (!this.destroyed)
				{
					GUI.depth = 50;
					UI.ApplyUIScale();
					LongEventHandler.LongEventsOnGUI();
					if (LongEventHandler.ShouldWaitForEvent)
					{
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
					}
					else
					{
						this.uiRoot.UIRootOnGUI();
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
						if (Find.CameraDriver != null && Find.CameraDriver.isActiveAndEnabled)
						{
							Find.CameraDriver.CameraDriverOnGUI();
						}
						if (Find.WorldCameraDriver != null && Find.WorldCameraDriver.isActiveAndEnabled)
						{
							Find.WorldCameraDriver.WorldCameraDriverOnGUI();
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in OnGUI(): " + arg);
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x000316A8 File Offset: 0x0002F8A8
		public static void Shutdown()
		{
			try
			{
				SteamManager.ShutdownSteam();
			}
			catch (Exception arg)
			{
				Log.Error("Error in ShutdownSteam(): " + arg);
			}
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.TempFolderPath);
				FileInfo[] files = directoryInfo.GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					files[i].Delete();
				}
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				for (int i = 0; i < directories.Length; i++)
				{
					directories[i].Delete(true);
				}
			}
			catch (Exception arg2)
			{
				Log.Error("Could not delete temporary files: " + arg2);
			}
			Application.Quit();
		}

		// Token: 0x04000A40 RID: 2624
		private static bool globalInitDone;

		// Token: 0x04000A41 RID: 2625
		private static bool prefsApplied;

		// Token: 0x04000A42 RID: 2626
		protected static bool checkedAutostartSaveFile;

		// Token: 0x04000A43 RID: 2627
		protected bool destroyed;

		// Token: 0x04000A44 RID: 2628
		public SoundRoot soundRoot;

		// Token: 0x04000A45 RID: 2629
		public UIRoot uiRoot;
	}
}

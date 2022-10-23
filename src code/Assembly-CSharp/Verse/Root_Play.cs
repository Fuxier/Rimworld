using System;
using System.IO;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000179 RID: 377
	public class Root_Play : Root
	{
		// Token: 0x06000A36 RID: 2614 RVA: 0x000318D4 File Offset: 0x0002FAD4
		public override void Start()
		{
			base.Start();
			try
			{
				this.musicManagerPlay = new MusicManagerPlay();
				FileInfo autostart = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
				Root.checkedAutostartSaveFile = true;
				if (autostart != null)
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Path.GetFileNameWithoutExtension(autostart.Name));
					}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true);
				}
				else if (Find.GameInitData != null && !Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Find.GameInitData.gameToLoad);
					}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true);
				}
				else
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						if (Current.Game == null)
						{
							Root_Play.SetupForQuickTestPlay();
						}
						Current.Game.InitNewGame();
					}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true);
				}
				LongEventHandler.QueueLongEvent(delegate()
				{
					ScreenFader.SetColor(Color.black);
					ScreenFader.StartFade(Color.clear, 0.5f);
				}, null, false, null, true);
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg);
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00031A28 File Offset: 0x0002FC28
		public override void Update()
		{
			base.Update();
			if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
			{
				return;
			}
			try
			{
				ShipCountdown.ShipCountdownUpdate();
				if (ModsConfig.IdeologyActive)
				{
					ArchonexusCountdown.ArchonexusCountdownUpdate();
				}
				TargetHighlighter.TargetHighlighterUpdate();
				Current.Game.UpdatePlay();
				this.musicManagerPlay.MusicUpdate();
				PerformanceBenchmarkUtility.CheckBenchmark();
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in Update(): " + arg);
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00031AA4 File Offset: 0x0002FCA4
		public static void SetupForQuickTestPlay()
		{
			Current.ProgramState = ProgramState.Entry;
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = ScenarioDefOf.Crashlanded.scenario;
			Find.Scenario.PreConfigure();
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.Rough);
			Current.Game.World = WorldGenerator.GenerateWorld(0.05f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal, OverallPopulation.Normal, null, 0f);
			Find.GameInitData.ChooseRandomStartingTile();
			Find.GameInitData.mapSize = 150;
			Find.Scenario.PostIdeoChosen();
			Find.GameInitData.PrepForMapGen();
			Find.Scenario.PreMapGenerate();
		}

		// Token: 0x04000A47 RID: 2631
		public MusicManagerPlay musicManagerPlay;
	}
}

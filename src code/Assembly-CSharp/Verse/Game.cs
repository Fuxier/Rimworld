using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000161 RID: 353
	public class Game : IExposable
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0002DE28 File Offset: 0x0002C028
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x0002DE30 File Offset: 0x0002C030
		public Scenario Scenario
		{
			get
			{
				return this.scenarioInt;
			}
			set
			{
				this.scenarioInt = value;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0002DE39 File Offset: 0x0002C039
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x0002DE41 File Offset: 0x0002C041
		public World World
		{
			get
			{
				return this.worldInt;
			}
			set
			{
				if (this.worldInt == value)
				{
					return;
				}
				this.worldInt = value;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x0002DE54 File Offset: 0x0002C054
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x0002DE74 File Offset: 0x0002C074
		public Map CurrentMap
		{
			get
			{
				if (this.currentMapIndex < 0)
				{
					return null;
				}
				return this.maps[(int)this.currentMapIndex];
			}
			set
			{
				int num;
				if (value == null)
				{
					num = -1;
				}
				else
				{
					num = this.maps.IndexOf(value);
					if (num < 0)
					{
						Log.Error("Could not set current map because it does not exist.");
						return;
					}
				}
				if ((int)this.currentMapIndex != num)
				{
					this.currentMapIndex = (sbyte)num;
					Find.MapUI.Notify_SwitchedMap();
					AmbientSoundManager.Notify_SwitchedMap();
				}
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0002DEC4 File Offset: 0x0002C0C4
		public Map AnyPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						return map;
					}
				}
				return null;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0002DF08 File Offset: 0x0002C108
		public Map RandomPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				if (Game.tmpPlayerHomeMaps.Any<Map>())
				{
					Map result = Game.tmpPlayerHomeMaps.RandomElement<Map>();
					Game.tmpPlayerHomeMaps.Clear();
					return result;
				}
				return null;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0002DF80 File Offset: 0x0002C180
		public List<Map> Maps
		{
			get
			{
				return this.maps;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0002DF88 File Offset: 0x0002C188
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x0002DF90 File Offset: 0x0002C190
		public GameInitData InitData
		{
			get
			{
				return this.initData;
			}
			set
			{
				this.initData = value;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0002DF99 File Offset: 0x0002C199
		public GameInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0002DFA1 File Offset: 0x0002C1A1
		public GameRules Rules
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0002DFAC File Offset: 0x0002C1AC
		public Game()
		{
			this.FillComponents();
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0002E0F8 File Offset: 0x0002C2F8
		public void AddMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to add null map.");
				return;
			}
			if (this.maps.Contains(map))
			{
				Log.Error("Tried to add map but it's already here.");
				return;
			}
			if (this.maps.Count > 127)
			{
				Log.Error("Can't add map. Reached maps count limit (" + sbyte.MaxValue + ").");
				return;
			}
			this.maps.Add(map);
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0002E170 File Offset: 0x0002C370
		public Map FindMap(MapParent mapParent)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].info.parent == mapParent)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0002E1BC File Offset: 0x0002C3BC
		public Map FindMap(int tile)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].Tile == tile)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0002E204 File Offset: 0x0002C404
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Error("You must use special LoadData method to load Game.");
				return;
			}
			Scribe_Values.Look<sbyte>(ref this.currentMapIndex, "currentMapIndex", -1, false);
			this.ExposeSmallComponents();
			Scribe_Deep.Look<World>(ref this.worldInt, "world", Array.Empty<object>());
			Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
			Find.CameraDriver.Expose();
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0002E274 File Offset: 0x0002C474
		private void ExposeSmallComponents()
		{
			Scribe_Deep.Look<GameInfo>(ref this.info, "info", Array.Empty<object>());
			Scribe_Deep.Look<GameRules>(ref this.rules, "rules", Array.Empty<object>());
			Scribe_Deep.Look<Scenario>(ref this.scenarioInt, "scenario", Array.Empty<object>());
			Scribe_Deep.Look<TickManager>(ref this.tickManager, "tickManager", Array.Empty<object>());
			Scribe_Deep.Look<PlaySettings>(ref this.playSettings, "playSettings", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher>(ref this.storyWatcher, "storyWatcher", Array.Empty<object>());
			Scribe_Deep.Look<GameEnder>(ref this.gameEnder, "gameEnder", Array.Empty<object>());
			Scribe_Deep.Look<LetterStack>(ref this.letterStack, "letterStack", Array.Empty<object>());
			Scribe_Deep.Look<ResearchManager>(ref this.researchManager, "researchManager", Array.Empty<object>());
			Scribe_Deep.Look<Storyteller>(ref this.storyteller, "storyteller", Array.Empty<object>());
			Scribe_Deep.Look<History>(ref this.history, "history", Array.Empty<object>());
			Scribe_Deep.Look<TaleManager>(ref this.taleManager, "taleManager", Array.Empty<object>());
			Scribe_Deep.Look<PlayLog>(ref this.playLog, "playLog", Array.Empty<object>());
			Scribe_Deep.Look<BattleLog>(ref this.battleLog, "battleLog", Array.Empty<object>());
			Scribe_Deep.Look<OutfitDatabase>(ref this.outfitDatabase, "outfitDatabase", Array.Empty<object>());
			Scribe_Deep.Look<DrugPolicyDatabase>(ref this.drugPolicyDatabase, "drugPolicyDatabase", Array.Empty<object>());
			Scribe_Deep.Look<FoodRestrictionDatabase>(ref this.foodRestrictionDatabase, "foodRestrictionDatabase", Array.Empty<object>());
			Scribe_Deep.Look<Tutor>(ref this.tutor, "tutor", Array.Empty<object>());
			Scribe_Deep.Look<DateNotifier>(ref this.dateNotifier, "dateNotifier", Array.Empty<object>());
			Scribe_Deep.Look<UniqueIDsManager>(ref this.uniqueIDsManager, "uniqueIDsManager", Array.Empty<object>());
			Scribe_Deep.Look<QuestManager>(ref this.questManager, "questManager", Array.Empty<object>());
			Scribe_Deep.Look<TransportShipManager>(ref this.transportShipManager, "transportShipManager", Array.Empty<object>());
			Scribe_Deep.Look<StudyManager>(ref this.studyManager, "studyManager", Array.Empty<object>());
			Scribe_Deep.Look<CustomXenogermDatabase>(ref this.customXenogermDatabase, "customXenogermDatabase", Array.Empty<object>());
			Scribe_Collections.Look<GameComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.FillComponents();
				if (this.rules == null)
				{
					Log.Warning("Save game was missing rules. Replacing with a blank GameRules.");
					this.rules = new GameRules();
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0002E4C8 File Offset: 0x0002C6C8
		private void FillComponents()
		{
			this.components.RemoveAll((GameComponent component) => component == null);
			foreach (Type type in typeof(GameComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						GameComponent item = (GameComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate a GameComponent of type ",
							type,
							": ",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0002E5AC File Offset: 0x0002C7AC
		public void InitNewGame()
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.PackageIdPlayerFacing + ((!mod.ModMetaData.VersionCompatible) ? " (incompatible version)" : "")).ToLineList("  - ", false);
			Log.Message("Initializing new game with mods:\n" + str);
			if (this.maps.Any<Map>())
			{
				Log.Error("Called InitNewGame() but there already is a map. There should be 0 maps...");
				return;
			}
			if (this.initData == null)
			{
				Log.Error("Called InitNewGame() but init data is null. Create it first.");
				return;
			}
			MemoryUtility.UnloadUnusedUnityAssets();
			DeepProfiler.Start("InitNewGame");
			try
			{
				Current.ProgramState = ProgramState.MapInitializing;
				IntVec3 intVec = new IntVec3(this.initData.mapSize, 1, this.initData.mapSize);
				Settlement settlement = null;
				List<Settlement> settlements = Find.WorldObjects.Settlements;
				for (int i = 0; i < settlements.Count; i++)
				{
					if (settlements[i].Faction == Faction.OfPlayer)
					{
						settlement = settlements[i];
						break;
					}
				}
				if (settlement == null)
				{
					Log.Error("Could not generate starting map because there is no any player faction base.");
				}
				this.tickManager.gameStartAbsTick = GenTicks.ConfiguredTicksAbsAtGameStart;
				this.info.startingTile = this.initData.startingTile;
				Map currentMap = MapGenerator.GenerateMap(intVec, settlement, settlement.MapGeneratorDef, settlement.ExtraGenStepDefs, null);
				this.worldInt.info.initialMapSize = intVec;
				if (this.initData.permadeath)
				{
					this.info.permadeathMode = true;
					this.info.permadeathModeUniqueName = PermadeathModeUtility.GeneratePermadeathSaveName();
				}
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.NewColonyOptimism);
				this.FinalizeInit();
				Current.Game.CurrentMap = currentMap;
				Find.CameraDriver.JumpToCurrentMapLoc(MapGenerator.PlayerStartSpot);
				Find.CameraDriver.ResetSize();
				if (Prefs.PauseOnLoad && this.initData.startedFromEntry)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						this.tickManager.DoSingleTick();
						this.tickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				Find.Scenario.PostGameStart();
				this.history.FinalizeInit();
				ResearchUtility.ApplyPlayerStartingResearch();
				GameComponentUtility.StartedNewGame();
				this.initData = null;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0002E7C4 File Offset: 0x0002C9C4
		public void LoadGame()
		{
			if (this.maps.Any<Map>())
			{
				Log.Error("Called LoadGame() but there already is a map. There should be 0 maps...");
				return;
			}
			this.ClearCaches();
			MemoryUtility.UnloadUnusedUnityAssets();
			BackCompatibility.PreLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
			Current.ProgramState = ProgramState.MapInitializing;
			this.ExposeSmallComponents();
			LongEventHandler.SetCurrentEventText("LoadingWorld".Translate());
			if (Scribe.EnterNode("world"))
			{
				try
				{
					this.World = new World();
					this.World.ExposeData();
					goto IL_82;
				}
				finally
				{
					Scribe.ExitNode();
				}
				goto IL_77;
				IL_82:
				DeepProfiler.Start("World.FinalizeInit");
				this.World.FinalizeInit();
				DeepProfiler.End();
				LongEventHandler.SetCurrentEventText("LoadingMap".Translate());
				Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
				if (this.maps.RemoveAll((Map x) => x == null) != 0)
				{
					Log.Warning("Some maps were null after loading.");
				}
				int num = -1;
				Scribe_Values.Look<int>(ref num, "currentMapIndex", -1, false);
				if (num < 0 && this.maps.Any<Map>())
				{
					Log.Error("Current map is null after loading but there are maps available. Setting current map to [0].");
					num = 0;
				}
				if (num >= this.maps.Count)
				{
					Log.Error("Current map index out of bounds after loading.");
					if (this.maps.Any<Map>())
					{
						num = 0;
					}
					else
					{
						num = -1;
					}
				}
				this.currentMapIndex = sbyte.MinValue;
				this.CurrentMap = ((num >= 0) ? this.maps[num] : null);
				LongEventHandler.SetCurrentEventText("InitializingGame".Translate());
				Find.CameraDriver.Expose();
				DeepProfiler.Start("Scribe.loader.FinalizeLoading");
				Scribe.loader.FinalizeLoading();
				DeepProfiler.End();
				LongEventHandler.SetCurrentEventText("SpawningAllThings".Translate());
				DeepProfiler.Start("maps.FinalizeLoading");
				for (int i = 0; i < this.maps.Count; i++)
				{
					try
					{
						this.maps[i].FinalizeLoading();
					}
					catch (Exception arg)
					{
						Log.Error("Error in Map.FinalizeLoading(): " + arg);
					}
					try
					{
						this.maps[i].Parent.FinalizeLoading();
					}
					catch (Exception arg2)
					{
						Log.Error("Error in MapParent.FinalizeLoading(): " + arg2);
					}
				}
				DeepProfiler.End();
				DeepProfiler.Start("Game.FinalizeInit");
				this.FinalizeInit();
				DeepProfiler.End();
				if (Prefs.PauseOnLoad)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						Find.TickManager.DoSingleTick();
						Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				GameComponentUtility.LoadedGame();
				BackCompatibility.PostLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
				return;
			}
			IL_77:
			Log.Error("Could not find world XML node.");
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0002EA80 File Offset: 0x0002CC80
		public void UpdateEntry()
		{
			GameComponentUtility.GameComponentUpdate();
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0002EA88 File Offset: 0x0002CC88
		public void UpdatePlay()
		{
			try
			{
				Find.LetterStack.OpenAutomaticLetters();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.tickManager.TickManagerUpdate();
			this.letterStack.LetterStackUpdate();
			this.World.WorldUpdate();
			for (int i = 0; i < this.maps.Count; i++)
			{
				this.maps[i].MapUpdate();
			}
			this.Info.GameInfoUpdate();
			GameComponentUtility.GameComponentUpdate();
			this.signalManager.SignalManagerUpdate();
			GlobalTextureAtlasManager.GlobalTextureAtlasManagerUpdate();
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0002EB28 File Offset: 0x0002CD28
		public T GetComponent<T>() where T : GameComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				T t = this.components[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0002EB78 File Offset: 0x0002CD78
		public GameComponent GetComponent(Type type)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (type.IsAssignableFrom(this.components[i].GetType()))
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0002EBC2 File Offset: 0x0002CDC2
		public void FinalizeInit()
		{
			LogSimple.FlushToFileAndOpen();
			this.researchManager.ReapplyAllMods();
			MessagesRepeatAvoider.Reset();
			GameComponentUtility.FinalizeInit();
			Current.ProgramState = ProgramState.Playing;
			Current.Game.World.ideoManager.Notify_GameStarted();
			RecipeDefGenerator.ResetRecipeIngredientsForDifficulty();
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0002EC00 File Offset: 0x0002CE00
		public void DeinitAndRemoveMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to remove null map.");
				return;
			}
			if (!this.maps.Contains(map))
			{
				Log.Error("Tried to remove map " + map + " but it's not here.");
				return;
			}
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapAboutToBeRemoved();
			}
			Map currentMap = this.CurrentMap;
			MapDeiniter.Deinit(map);
			this.maps.Remove(map);
			if (currentMap != null)
			{
				sbyte b = (sbyte)this.maps.IndexOf(currentMap);
				if (b < 0)
				{
					if (this.maps.Any<Map>())
					{
						this.CurrentMap = this.maps[0];
					}
					else
					{
						this.CurrentMap = null;
					}
					Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				}
				else
				{
					this.currentMapIndex = b;
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			MapComponentUtility.MapRemoved(map);
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapRemoved(map);
			}
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0002ECF0 File Offset: 0x0002CEF0
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Game debug data:");
			stringBuilder.AppendLine("initData:");
			if (this.initData == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine(this.initData.ToString());
			}
			stringBuilder.AppendLine("Scenario:");
			if (this.scenarioInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   " + this.scenarioInt.ToString());
			}
			stringBuilder.AppendLine("World:");
			if (this.worldInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   name: " + this.worldInt.info.name);
			}
			stringBuilder.AppendLine("Maps count: " + this.maps.Count);
			for (int i = 0; i < this.maps.Count; i++)
			{
				stringBuilder.AppendLine("   Map " + this.maps[i].Index + ":");
				stringBuilder.AppendLine("      tile: " + this.maps[i].TileInfo);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0002EE50 File Offset: 0x0002D050
		private void ClearCaches()
		{
			ChildcareUtility.ClearCache();
			SlaveRebellionUtility.ClearCache();
			Alert_NeedMeditationSpot.ClearCache();
			BuildCopyCommandUtility.ClearCache();
			MechanitorUtility.ClearCache();
			foreach (StatDef statDef in DefDatabase<StatDef>.AllDefsListForReading)
			{
				statDef.Worker.TryClearCache();
			}
		}

		// Token: 0x040009C1 RID: 2497
		private GameInitData initData;

		// Token: 0x040009C2 RID: 2498
		public sbyte currentMapIndex = -1;

		// Token: 0x040009C3 RID: 2499
		private GameInfo info = new GameInfo();

		// Token: 0x040009C4 RID: 2500
		public List<GameComponent> components = new List<GameComponent>();

		// Token: 0x040009C5 RID: 2501
		private GameRules rules = new GameRules();

		// Token: 0x040009C6 RID: 2502
		private Scenario scenarioInt;

		// Token: 0x040009C7 RID: 2503
		private World worldInt;

		// Token: 0x040009C8 RID: 2504
		private List<Map> maps = new List<Map>();

		// Token: 0x040009C9 RID: 2505
		public PlaySettings playSettings = new PlaySettings();

		// Token: 0x040009CA RID: 2506
		public StoryWatcher storyWatcher = new StoryWatcher();

		// Token: 0x040009CB RID: 2507
		public LetterStack letterStack = new LetterStack();

		// Token: 0x040009CC RID: 2508
		public ResearchManager researchManager = new ResearchManager();

		// Token: 0x040009CD RID: 2509
		public GameEnder gameEnder = new GameEnder();

		// Token: 0x040009CE RID: 2510
		public Storyteller storyteller = new Storyteller();

		// Token: 0x040009CF RID: 2511
		public History history = new History();

		// Token: 0x040009D0 RID: 2512
		public TaleManager taleManager = new TaleManager();

		// Token: 0x040009D1 RID: 2513
		public PlayLog playLog = new PlayLog();

		// Token: 0x040009D2 RID: 2514
		public BattleLog battleLog = new BattleLog();

		// Token: 0x040009D3 RID: 2515
		public OutfitDatabase outfitDatabase = new OutfitDatabase();

		// Token: 0x040009D4 RID: 2516
		public DrugPolicyDatabase drugPolicyDatabase = new DrugPolicyDatabase();

		// Token: 0x040009D5 RID: 2517
		public FoodRestrictionDatabase foodRestrictionDatabase = new FoodRestrictionDatabase();

		// Token: 0x040009D6 RID: 2518
		public TickManager tickManager = new TickManager();

		// Token: 0x040009D7 RID: 2519
		public Tutor tutor = new Tutor();

		// Token: 0x040009D8 RID: 2520
		public Autosaver autosaver = new Autosaver();

		// Token: 0x040009D9 RID: 2521
		public DateNotifier dateNotifier = new DateNotifier();

		// Token: 0x040009DA RID: 2522
		public SignalManager signalManager = new SignalManager();

		// Token: 0x040009DB RID: 2523
		public UniqueIDsManager uniqueIDsManager = new UniqueIDsManager();

		// Token: 0x040009DC RID: 2524
		public QuestManager questManager = new QuestManager();

		// Token: 0x040009DD RID: 2525
		public TransportShipManager transportShipManager = new TransportShipManager();

		// Token: 0x040009DE RID: 2526
		public StudyManager studyManager = new StudyManager();

		// Token: 0x040009DF RID: 2527
		public CustomXenogermDatabase customXenogermDatabase = new CustomXenogermDatabase();

		// Token: 0x040009E0 RID: 2528
		private static List<Map> tmpPlayerHomeMaps = new List<Map>();
	}
}

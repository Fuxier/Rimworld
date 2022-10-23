using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020001F9 RID: 505
	public sealed class Map : IIncidentTarget, ILoadReferenceable, IThingHolder, IExposable
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x000504F0 File Offset: 0x0004E6F0
		public int Index
		{
			get
			{
				return Find.Maps.IndexOf(this);
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x000504FD File Offset: 0x0004E6FD
		public IntVec3 Size
		{
			get
			{
				return this.info.Size;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0005050A File Offset: 0x0004E70A
		public IntVec3 Center
		{
			get
			{
				return new IntVec3(this.Size.x / 2, 0, this.Size.z / 2);
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0005052C File Offset: 0x0004E72C
		public Faction ParentFaction
		{
			get
			{
				return this.info.parent.Faction;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0005053E File Offset: 0x0004E73E
		public int Area
		{
			get
			{
				return this.Size.x * this.Size.z;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00050557 File Offset: 0x0004E757
		public IThingHolder ParentHolder
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x00050564 File Offset: 0x0004E764
		public IEnumerable<IntVec3> AllCells
		{
			get
			{
				int num;
				for (int z = 0; z < this.Size.z; z = num + 1)
				{
					for (int y = 0; y < this.Size.y; y = num + 1)
					{
						for (int x = 0; x < this.Size.x; x = num + 1)
						{
							yield return new IntVec3(x, y, z);
							num = x;
						}
						num = y;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x00050574 File Offset: 0x0004E774
		public bool IsPlayerHome
		{
			get
			{
				return this.info != null && this.info.parent != null && this.info.parent.def.canBePlayerHome && this.info.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x000505C6 File Offset: 0x0004E7C6
		public bool IsTempIncidentMap
		{
			get
			{
				return this.info.parent.def.isTempIncidentMapOwner;
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x000505DD File Offset: 0x0004E7DD
		public IEnumerator<IntVec3> GetEnumerator()
		{
			foreach (IntVec3 intVec in this.AllCells)
			{
				yield return intVec;
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x000505EC File Offset: 0x0004E7EC
		public int Tile
		{
			get
			{
				return this.info.Tile;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x000505F9 File Offset: 0x0004E7F9
		public Tile TileInfo
		{
			get
			{
				return Find.WorldGrid[this.Tile];
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0005060B File Offset: 0x0004E80B
		public BiomeDef Biome
		{
			get
			{
				return this.TileInfo.biome;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00050618 File Offset: 0x0004E818
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x00050620 File Offset: 0x0004E820
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00050628 File Offset: 0x0004E828
		public float PlayerWealthForStoryteller
		{
			get
			{
				if (!this.IsPlayerHome)
				{
					float num = 0f;
					foreach (Pawn pawn in this.mapPawns.PawnsInFaction(Faction.OfPlayer))
					{
						if (pawn.IsFreeColonist)
						{
							num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(pawn);
						}
						if (pawn.RaceProps.Animal)
						{
							num += pawn.MarketValue;
						}
					}
					return num;
				}
				if (Find.Storyteller.difficulty.fixedWealthMode)
				{
					return StorytellerUtility.FixedWealthModeMapWealthFromTimeCurve.Evaluate(this.AgeInDays * Find.Storyteller.difficulty.fixedWealthTimeFactor);
				}
				return this.wealthWatcher.WealthItems + this.wealthWatcher.WealthBuildings * 0.5f + this.wealthWatcher.WealthPawns;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x00050710 File Offset: 0x0004E910
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return this.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00050722 File Offset: 0x0004E922
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x00050557 File Offset: 0x0004E757
		public MapParent Parent
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x00050729 File Offset: 0x0004E929
		public float AgeInDays
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - this.generationTick) / 60000f;
			}
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00050743 File Offset: 0x0004E943
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			return this.info.parent.IncidentTargetTags();
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00050758 File Offset: 0x0004E958
		public void ConstructComponents()
		{
			this.spawnedThings = new ThingOwner<Thing>(this);
			this.cellIndices = new CellIndices(this);
			this.listerThings = new ListerThings(ListerThingsUse.Global);
			this.listerBuildings = new ListerBuildings();
			this.mapPawns = new MapPawns(this);
			this.dynamicDrawManager = new DynamicDrawManager(this);
			this.mapDrawer = new MapDrawer(this);
			this.tooltipGiverList = new TooltipGiverList();
			this.pawnDestinationReservationManager = new PawnDestinationReservationManager();
			this.reservationManager = new ReservationManager(this);
			this.physicalInteractionReservationManager = new PhysicalInteractionReservationManager();
			this.designationManager = new DesignationManager(this);
			this.lordManager = new LordManager(this);
			this.debugDrawer = new DebugCellDrawer();
			this.passingShipManager = new PassingShipManager(this);
			this.haulDestinationManager = new HaulDestinationManager(this);
			this.gameConditionManager = new GameConditionManager(this);
			this.weatherManager = new WeatherManager(this);
			this.zoneManager = new ZoneManager(this);
			this.resourceCounter = new ResourceCounter(this);
			this.mapTemperature = new MapTemperature(this);
			this.temperatureCache = new TemperatureCache(this);
			this.areaManager = new AreaManager(this);
			this.attackTargetsCache = new AttackTargetsCache(this);
			this.attackTargetReservationManager = new AttackTargetReservationManager(this);
			this.lordsStarter = new VoluntarilyJoinableLordsStarter(this);
			this.flecks = new FleckManager(this);
			this.thingGrid = new ThingGrid(this);
			this.coverGrid = new CoverGrid(this);
			this.edificeGrid = new EdificeGrid(this);
			this.blueprintGrid = new BlueprintGrid(this);
			this.fogGrid = new FogGrid(this);
			this.glowGrid = new GlowGrid(this);
			this.regionGrid = new RegionGrid(this);
			this.terrainGrid = new TerrainGrid(this);
			this.pathing = new Pathing(this);
			this.roofGrid = new RoofGrid(this);
			this.fertilityGrid = new FertilityGrid(this);
			this.snowGrid = new SnowGrid(this);
			this.gasGrid = new GasGrid(this);
			this.pollutionGrid = new PollutionGrid(this);
			this.deepResourceGrid = new DeepResourceGrid(this);
			this.exitMapGrid = new ExitMapGrid(this);
			this.avoidGrid = new AvoidGrid(this);
			this.linkGrid = new LinkGrid(this);
			this.glowFlooder = new GlowFlooder(this);
			this.powerNetManager = new PowerNetManager(this);
			this.powerNetGrid = new PowerNetGrid(this);
			this.regionMaker = new RegionMaker(this);
			this.pathFinder = new PathFinder(this);
			this.pawnPathPool = new PawnPathPool(this);
			this.regionAndRoomUpdater = new RegionAndRoomUpdater(this);
			this.regionLinkDatabase = new RegionLinkDatabase();
			this.moteCounter = new MoteCounter();
			this.gatherSpotLister = new GatherSpotLister();
			this.windManager = new WindManager(this);
			this.listerBuildingsRepairable = new ListerBuildingsRepairable();
			this.listerHaulables = new ListerHaulables(this);
			this.listerMergeables = new ListerMergeables(this);
			this.listerFilthInHomeArea = new ListerFilthInHomeArea(this);
			this.listerArtificialBuildingsForMeditation = new ListerArtificialBuildingsForMeditation(this);
			this.listerBuldingOfDefInProximity = new ListerBuldingOfDefInProximity(this);
			this.listerBuildingWithTagInProximity = new ListerBuildingWithTagInProximity(this);
			this.reachability = new Reachability(this);
			this.itemAvailability = new ItemAvailability(this);
			this.autoBuildRoofAreaSetter = new AutoBuildRoofAreaSetter(this);
			this.roofCollapseBufferResolver = new RoofCollapseBufferResolver(this);
			this.roofCollapseBuffer = new RoofCollapseBuffer();
			this.wildAnimalSpawner = new WildAnimalSpawner(this);
			this.wildPlantSpawner = new WildPlantSpawner(this);
			this.steadyEnvironmentEffects = new SteadyEnvironmentEffects(this);
			this.skyManager = new SkyManager(this);
			this.overlayDrawer = new OverlayDrawer();
			this.floodFiller = new FloodFiller(this);
			this.weatherDecider = new WeatherDecider(this);
			this.fireWatcher = new FireWatcher(this);
			this.dangerWatcher = new DangerWatcher(this);
			this.damageWatcher = new DamageWatcher();
			this.strengthWatcher = new StrengthWatcher(this);
			this.wealthWatcher = new WealthWatcher(this);
			this.regionDirtyer = new RegionDirtyer(this);
			this.cellsInRandomOrder = new MapCellsInRandomOrder(this);
			this.rememberedCameraPos = new RememberedCameraPos(this);
			this.mineStrikeManager = new MineStrikeManager();
			this.storyState = new StoryState(this);
			this.retainedCaravanData = new RetainedCaravanData(this);
			this.temporaryThingDrawer = new TemporaryThingDrawer();
			this.animalPenManager = new AnimalPenManager(this);
			this.plantGrowthRateCalculator = new MapPlantGrowthRateCalculator();
			this.autoSlaughterManager = new AutoSlaughterManager(this);
			this.treeDestructionTracker = new TreeDestructionTracker(this);
			this.storageGroups = new StorageGroupManager(this);
			this.effecterMaintainer = new EffecterMaintainer(this);
			this.postTickVisuals = new PostTickVisuals(this);
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00050BD0 File Offset: 0x0004EDD0
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Values.Look<int>(ref this.generationTick, "generationTick", 0, false);
			Scribe_Deep.Look<MapInfo>(ref this.info, "mapInfo", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.compressor = new MapFileCompressor(this);
				this.compressor.BuildCompressedString();
				this.ExposeComponents();
				this.compressor.ExposeData();
				HashSet<string> hashSet = new HashSet<string>();
				if (Scribe.EnterNode("things"))
				{
					try
					{
						using (List<Thing>.Enumerator enumerator = this.listerThings.AllThings.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Thing thing = enumerator.Current;
								try
								{
									if (thing.def.isSaveable && !thing.IsSaveCompressible())
									{
										if (hashSet.Contains(thing.ThingID))
										{
											Log.Error("Saving Thing with already-used ID " + thing.ThingID);
										}
										else
										{
											hashSet.Add(thing.ThingID);
										}
										Thing thing2 = thing;
										Scribe_Deep.Look<Thing>(ref thing2, "thing", Array.Empty<object>());
									}
								}
								catch (OutOfMemoryException)
								{
									throw;
								}
								catch (Exception ex)
								{
									Log.Error(string.Concat(new object[]
									{
										"Exception saving ",
										thing,
										": ",
										ex
									}));
								}
							}
							goto IL_157;
						}
					}
					finally
					{
						Scribe.ExitNode();
					}
				}
				Log.Error("Could not enter the things node while saving.");
				IL_157:
				this.compressor = null;
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.ConstructComponents();
				this.regionAndRoomUpdater.Enabled = false;
				this.compressor = new MapFileCompressor(this);
			}
			this.ExposeComponents();
			DeepProfiler.Start("Load compressed things");
			this.compressor.ExposeData();
			DeepProfiler.End();
			DeepProfiler.Start("Load non-compressed things");
			Scribe_Collections.Look<Thing>(ref this.loadedFullThings, "things", LookMode.Deep, Array.Empty<object>());
			DeepProfiler.End();
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00050DDC File Offset: 0x0004EFDC
		private void FillComponents()
		{
			this.components.RemoveAll((MapComponent component) => component == null);
			foreach (Type type in typeof(MapComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						MapComponent item = (MapComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate a MapComponent of type ",
							type,
							": ",
							ex
						}));
					}
				}
			}
			this.roadInfo = this.GetComponent<RoadInfo>();
			this.waterInfo = this.GetComponent<WaterInfo>();
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00050ED8 File Offset: 0x0004F0D8
		public void FinalizeLoading()
		{
			List<Thing> list = this.compressor.ThingsToSpawnAfterLoad().ToList<Thing>();
			this.compressor = null;
			DeepProfiler.Start("Merge compressed and non-compressed thing lists");
			List<Thing> list2 = new List<Thing>(this.loadedFullThings.Count + list.Count);
			foreach (Thing item in this.loadedFullThings.Concat(list))
			{
				list2.Add(item);
			}
			this.loadedFullThings.Clear();
			DeepProfiler.End();
			DeepProfiler.Start("Spawn everything into the map");
			BackCompatibility.PreCheckSpawnBackCompatibleThingAfterLoading(this);
			foreach (Thing thing in list2)
			{
				if (!(thing is Building))
				{
					try
					{
						if (!BackCompatibility.CheckSpawnBackCompatibleThingAfterLoading(thing, this))
						{
							GenSpawn.Spawn(thing, thing.Position, this, thing.Rotation, WipeMode.FullRefund, true);
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception spawning loaded thing ",
							thing.ToStringSafe<Thing>(),
							": ",
							ex
						}));
					}
				}
			}
			foreach (Building building in from t in list2.OfType<Building>()
			orderby t.def.size.Magnitude
			select t)
			{
				try
				{
					GenSpawn.SpawnBuildingAsPossible(building, this, true);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception spawning loaded thing ",
						building.ToStringSafe<Building>(),
						": ",
						ex2
					}));
				}
			}
			BackCompatibility.PostCheckSpawnBackCompatibleThingAfterLoading(this);
			DeepProfiler.End();
			this.FinalizeInit();
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x000510E8 File Offset: 0x0004F2E8
		public void FinalizeInit()
		{
			DeepProfiler.Start("Finalize geometry");
			this.pathing.RecalculateAllPerceivedPathCosts();
			this.regionAndRoomUpdater.Enabled = true;
			this.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.temperatureCache.temperatureSaveLoad.ApplyLoadedDataToRegions();
			this.avoidGrid.Regenerate();
			this.animalPenManager.RebuildAllPens();
			this.plantGrowthRateCalculator.BuildFor(this.Tile);
			this.gasGrid.RecalculateEverHadGas();
			DeepProfiler.End();
			DeepProfiler.Start("Thing.PostMapInit()");
			foreach (Thing thing in this.listerThings.AllThings.ToList<Thing>())
			{
				try
				{
					thing.PostMapInit();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error in PostMapInit() for ",
						thing.ToStringSafe<Thing>(),
						": ",
						ex
					}));
				}
			}
			DeepProfiler.End();
			DeepProfiler.Start("listerFilthInHomeArea.RebuildAll()");
			this.listerFilthInHomeArea.RebuildAll();
			DeepProfiler.End();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mapDrawer.RegenerateEverythingNow();
			});
			DeepProfiler.Start("resourceCounter.UpdateResourceCounts()");
			this.resourceCounter.UpdateResourceCounts();
			DeepProfiler.End();
			DeepProfiler.Start("wealthWatcher.ForceRecount()");
			this.wealthWatcher.ForceRecount(true);
			DeepProfiler.End();
			MapComponentUtility.FinalizeInit(this);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00051278 File Offset: 0x0004F478
		private void ExposeComponents()
		{
			Scribe_Deep.Look<WeatherManager>(ref this.weatherManager, "weatherManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<ReservationManager>(ref this.reservationManager, "reservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PhysicalInteractionReservationManager>(ref this.physicalInteractionReservationManager, "physicalInteractionReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<DesignationManager>(ref this.designationManager, "designationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PawnDestinationReservationManager>(ref this.pawnDestinationReservationManager, "pawnDestinationReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<LordManager>(ref this.lordManager, "lordManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PassingShipManager>(ref this.passingShipManager, "visitorManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<FogGrid>(ref this.fogGrid, "fogGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<RoofGrid>(ref this.roofGrid, "roofGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<TerrainGrid>(ref this.terrainGrid, "terrainGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<ZoneManager>(ref this.zoneManager, "zoneManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemperatureCache>(ref this.temperatureCache, "temperatureCache", new object[]
			{
				this
			});
			Scribe_Deep.Look<SnowGrid>(ref this.snowGrid, "snowGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<GasGrid>(ref this.gasGrid, "gasGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<PollutionGrid>(ref this.pollutionGrid, "pollutionGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<AreaManager>(ref this.areaManager, "areaManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<VoluntarilyJoinableLordsStarter>(ref this.lordsStarter, "lordsStarter", new object[]
			{
				this
			});
			Scribe_Deep.Look<AttackTargetReservationManager>(ref this.attackTargetReservationManager, "attackTargetReservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<DeepResourceGrid>(ref this.deepResourceGrid, "deepResourceGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<WeatherDecider>(ref this.weatherDecider, "weatherDecider", new object[]
			{
				this
			});
			Scribe_Deep.Look<DamageWatcher>(ref this.damageWatcher, "damageWatcher", Array.Empty<object>());
			Scribe_Deep.Look<RememberedCameraPos>(ref this.rememberedCameraPos, "rememberedCameraPos", new object[]
			{
				this
			});
			Scribe_Deep.Look<MineStrikeManager>(ref this.mineStrikeManager, "mineStrikeManager", Array.Empty<object>());
			Scribe_Deep.Look<RetainedCaravanData>(ref this.retainedCaravanData, "retainedCaravanData", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<WildPlantSpawner>(ref this.wildPlantSpawner, "wildPlantSpawner", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemporaryThingDrawer>(ref this.temporaryThingDrawer, "temporaryThingDrawer", Array.Empty<object>());
			Scribe_Deep.Look<FleckManager>(ref this.flecks, "flecks", new object[]
			{
				this
			});
			Scribe_Deep.Look<AutoSlaughterManager>(ref this.autoSlaughterManager, "autoSlaughterManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TreeDestructionTracker>(ref this.treeDestructionTracker, "treeDestructionTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<StorageGroupManager>(ref this.storageGroups, "storageGroups", new object[]
			{
				this
			});
			Scribe_Collections.Look<MapComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x000515D4 File Offset: 0x0004F7D4
		public void MapPreTick()
		{
			this.itemAvailability.Tick();
			this.listerHaulables.ListerHaulablesTick();
			try
			{
				this.autoBuildRoofAreaSetter.AutoBuildRoofAreaSetterTick_First();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			this.windManager.WindManagerTick();
			try
			{
				this.mapTemperature.MapTemperatureTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			this.temporaryThingDrawer.Tick();
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00051668 File Offset: 0x0004F868
		public void MapPostTick()
		{
			try
			{
				this.wildAnimalSpawner.WildAnimalSpawnerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			try
			{
				this.wildPlantSpawner.WildPlantSpawnerTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			try
			{
				this.powerNetManager.PowerNetsTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
			try
			{
				this.steadyEnvironmentEffects.SteadyEnvironmentEffectsTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString());
			}
			try
			{
				this.gasGrid.Tick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString());
			}
			if (ModsConfig.BiotechActive)
			{
				try
				{
					this.pollutionGrid.PollutionTick();
				}
				catch (Exception ex6)
				{
					Log.Error(ex6.ToString());
				}
			}
			try
			{
				this.lordManager.LordManagerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString());
			}
			try
			{
				this.passingShipManager.PassingShipManagerTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString());
			}
			try
			{
				this.debugDrawer.DebugDrawerTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString());
			}
			try
			{
				this.lordsStarter.VoluntarilyJoinableLordsStarterTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString());
			}
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString());
			}
			try
			{
				this.weatherManager.WeatherManagerTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString());
			}
			try
			{
				this.resourceCounter.ResourceCounterTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString());
			}
			try
			{
				this.weatherDecider.WeatherDeciderTick();
			}
			catch (Exception ex14)
			{
				Log.Error(ex14.ToString());
			}
			try
			{
				this.fireWatcher.FireWatcherTick();
			}
			catch (Exception ex15)
			{
				Log.Error(ex15.ToString());
			}
			try
			{
				this.flecks.FleckManagerTick();
			}
			catch (Exception ex16)
			{
				Log.Error(ex16.ToString());
			}
			try
			{
				this.effecterMaintainer.EffecterMaintainerTick();
			}
			catch (Exception ex17)
			{
				Log.Error(ex17.ToString());
			}
			MapComponentUtility.MapComponentTick(this);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0005190C File Offset: 0x0004FB0C
		public void MapUpdate()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			this.skyManager.SkyManagerUpdate();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.regionGrid.UpdateClean();
			this.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			this.glowGrid.GlowGridUpdate_First();
			this.lordManager.LordManagerUpdate();
			this.postTickVisuals.ProcessPostTickVisuals();
			if (!worldRenderedNow && Find.CurrentMap == this)
			{
				if (Map.AlwaysRedrawShadows)
				{
					this.mapDrawer.WholeMapChanged(MapMeshFlag.Things);
				}
				PlantFallColors.SetFallShaderGlobals(this);
				this.waterInfo.SetTextures();
				this.avoidGrid.DebugDrawOnMap();
				BreachingGridDebug.DebugDrawAllOnMap(this);
				this.mapDrawer.MapMeshDrawerUpdate_First();
				this.powerNetGrid.DrawDebugPowerNetGrid();
				DoorsDebugDrawer.DrawDebug();
				this.mapDrawer.DrawMapMesh();
				this.dynamicDrawManager.DrawDynamicThings();
				this.gameConditionManager.GameConditionManagerDraw(this);
				MapEdgeClipDrawer.DrawClippers(this);
				this.designationManager.DrawDesignations();
				this.overlayDrawer.DrawAllOverlays();
				this.temporaryThingDrawer.Draw();
				this.flecks.FleckManagerDraw();
			}
			try
			{
				this.areaManager.AreaManagerUpdate();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.weatherManager.WeatherManagerUpdate();
			try
			{
				this.flecks.FleckManagerUpdate();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			MapComponentUtility.MapComponentUpdate(this);
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00051A80 File Offset: 0x0004FC80
		public T GetComponent<T>() where T : MapComponent
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

		// Token: 0x06000EBA RID: 3770 RVA: 0x00051AD0 File Offset: 0x0004FCD0
		public MapComponent GetComponent(Type type)
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

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x00051B1A File Offset: 0x0004FD1A
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueID ^ 16622162;
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00051B28 File Offset: 0x0004FD28
		public string GetUniqueLoadID()
		{
			return "Map_" + this.uniqueID;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00051B40 File Offset: 0x0004FD40
		public override string ToString()
		{
			string text = "Map-" + this.uniqueID;
			if (this.IsPlayerHome)
			{
				text += "-PlayerHome";
			}
			return text;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00051B78 File Offset: 0x0004FD78
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.spawnedThings;
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00051B80 File Offset: 0x0004FD80
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder));
			List<PassingShip> passingShips = this.passingShipManager.passingShips;
			for (int i = 0; i < passingShips.Count; i++)
			{
				IThingHolder thingHolder = passingShips[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
			}
			for (int j = 0; j < this.components.Count; j++)
			{
				IThingHolder thingHolder2 = this.components[j] as IThingHolder;
				if (thingHolder2 != null)
				{
					outChildren.Add(thingHolder2);
				}
			}
		}

		// Token: 0x04000CDF RID: 3295
		public MapFileCompressor compressor;

		// Token: 0x04000CE0 RID: 3296
		private List<Thing> loadedFullThings;

		// Token: 0x04000CE1 RID: 3297
		public int uniqueID = -1;

		// Token: 0x04000CE2 RID: 3298
		public int generationTick;

		// Token: 0x04000CE3 RID: 3299
		public MapInfo info = new MapInfo();

		// Token: 0x04000CE4 RID: 3300
		public List<MapComponent> components = new List<MapComponent>();

		// Token: 0x04000CE5 RID: 3301
		public ThingOwner spawnedThings;

		// Token: 0x04000CE6 RID: 3302
		public CellIndices cellIndices;

		// Token: 0x04000CE7 RID: 3303
		public ListerThings listerThings;

		// Token: 0x04000CE8 RID: 3304
		public ListerBuildings listerBuildings;

		// Token: 0x04000CE9 RID: 3305
		public MapPawns mapPawns;

		// Token: 0x04000CEA RID: 3306
		public DynamicDrawManager dynamicDrawManager;

		// Token: 0x04000CEB RID: 3307
		public MapDrawer mapDrawer;

		// Token: 0x04000CEC RID: 3308
		public PawnDestinationReservationManager pawnDestinationReservationManager;

		// Token: 0x04000CED RID: 3309
		public TooltipGiverList tooltipGiverList;

		// Token: 0x04000CEE RID: 3310
		public ReservationManager reservationManager;

		// Token: 0x04000CEF RID: 3311
		public PhysicalInteractionReservationManager physicalInteractionReservationManager;

		// Token: 0x04000CF0 RID: 3312
		public DesignationManager designationManager;

		// Token: 0x04000CF1 RID: 3313
		public LordManager lordManager;

		// Token: 0x04000CF2 RID: 3314
		public PassingShipManager passingShipManager;

		// Token: 0x04000CF3 RID: 3315
		public HaulDestinationManager haulDestinationManager;

		// Token: 0x04000CF4 RID: 3316
		public DebugCellDrawer debugDrawer;

		// Token: 0x04000CF5 RID: 3317
		public GameConditionManager gameConditionManager;

		// Token: 0x04000CF6 RID: 3318
		public WeatherManager weatherManager;

		// Token: 0x04000CF7 RID: 3319
		public ZoneManager zoneManager;

		// Token: 0x04000CF8 RID: 3320
		public ResourceCounter resourceCounter;

		// Token: 0x04000CF9 RID: 3321
		public MapTemperature mapTemperature;

		// Token: 0x04000CFA RID: 3322
		public TemperatureCache temperatureCache;

		// Token: 0x04000CFB RID: 3323
		public AreaManager areaManager;

		// Token: 0x04000CFC RID: 3324
		public AttackTargetsCache attackTargetsCache;

		// Token: 0x04000CFD RID: 3325
		public AttackTargetReservationManager attackTargetReservationManager;

		// Token: 0x04000CFE RID: 3326
		public VoluntarilyJoinableLordsStarter lordsStarter;

		// Token: 0x04000CFF RID: 3327
		public FleckManager flecks;

		// Token: 0x04000D00 RID: 3328
		public ThingGrid thingGrid;

		// Token: 0x04000D01 RID: 3329
		public CoverGrid coverGrid;

		// Token: 0x04000D02 RID: 3330
		public EdificeGrid edificeGrid;

		// Token: 0x04000D03 RID: 3331
		public BlueprintGrid blueprintGrid;

		// Token: 0x04000D04 RID: 3332
		public FogGrid fogGrid;

		// Token: 0x04000D05 RID: 3333
		public RegionGrid regionGrid;

		// Token: 0x04000D06 RID: 3334
		public GlowGrid glowGrid;

		// Token: 0x04000D07 RID: 3335
		public TerrainGrid terrainGrid;

		// Token: 0x04000D08 RID: 3336
		public Pathing pathing;

		// Token: 0x04000D09 RID: 3337
		public RoofGrid roofGrid;

		// Token: 0x04000D0A RID: 3338
		public FertilityGrid fertilityGrid;

		// Token: 0x04000D0B RID: 3339
		public SnowGrid snowGrid;

		// Token: 0x04000D0C RID: 3340
		public DeepResourceGrid deepResourceGrid;

		// Token: 0x04000D0D RID: 3341
		public ExitMapGrid exitMapGrid;

		// Token: 0x04000D0E RID: 3342
		public AvoidGrid avoidGrid;

		// Token: 0x04000D0F RID: 3343
		public GasGrid gasGrid;

		// Token: 0x04000D10 RID: 3344
		public PollutionGrid pollutionGrid;

		// Token: 0x04000D11 RID: 3345
		public LinkGrid linkGrid;

		// Token: 0x04000D12 RID: 3346
		public GlowFlooder glowFlooder;

		// Token: 0x04000D13 RID: 3347
		public PowerNetManager powerNetManager;

		// Token: 0x04000D14 RID: 3348
		public PowerNetGrid powerNetGrid;

		// Token: 0x04000D15 RID: 3349
		public RegionMaker regionMaker;

		// Token: 0x04000D16 RID: 3350
		public PathFinder pathFinder;

		// Token: 0x04000D17 RID: 3351
		public PawnPathPool pawnPathPool;

		// Token: 0x04000D18 RID: 3352
		public RegionAndRoomUpdater regionAndRoomUpdater;

		// Token: 0x04000D19 RID: 3353
		public RegionLinkDatabase regionLinkDatabase;

		// Token: 0x04000D1A RID: 3354
		public MoteCounter moteCounter;

		// Token: 0x04000D1B RID: 3355
		public GatherSpotLister gatherSpotLister;

		// Token: 0x04000D1C RID: 3356
		public WindManager windManager;

		// Token: 0x04000D1D RID: 3357
		public ListerBuildingsRepairable listerBuildingsRepairable;

		// Token: 0x04000D1E RID: 3358
		public ListerHaulables listerHaulables;

		// Token: 0x04000D1F RID: 3359
		public ListerMergeables listerMergeables;

		// Token: 0x04000D20 RID: 3360
		public ListerArtificialBuildingsForMeditation listerArtificialBuildingsForMeditation;

		// Token: 0x04000D21 RID: 3361
		public ListerBuldingOfDefInProximity listerBuldingOfDefInProximity;

		// Token: 0x04000D22 RID: 3362
		public ListerBuildingWithTagInProximity listerBuildingWithTagInProximity;

		// Token: 0x04000D23 RID: 3363
		public ListerFilthInHomeArea listerFilthInHomeArea;

		// Token: 0x04000D24 RID: 3364
		public Reachability reachability;

		// Token: 0x04000D25 RID: 3365
		public ItemAvailability itemAvailability;

		// Token: 0x04000D26 RID: 3366
		public AutoBuildRoofAreaSetter autoBuildRoofAreaSetter;

		// Token: 0x04000D27 RID: 3367
		public RoofCollapseBufferResolver roofCollapseBufferResolver;

		// Token: 0x04000D28 RID: 3368
		public RoofCollapseBuffer roofCollapseBuffer;

		// Token: 0x04000D29 RID: 3369
		public WildAnimalSpawner wildAnimalSpawner;

		// Token: 0x04000D2A RID: 3370
		public WildPlantSpawner wildPlantSpawner;

		// Token: 0x04000D2B RID: 3371
		public SteadyEnvironmentEffects steadyEnvironmentEffects;

		// Token: 0x04000D2C RID: 3372
		public SkyManager skyManager;

		// Token: 0x04000D2D RID: 3373
		public OverlayDrawer overlayDrawer;

		// Token: 0x04000D2E RID: 3374
		public FloodFiller floodFiller;

		// Token: 0x04000D2F RID: 3375
		public WeatherDecider weatherDecider;

		// Token: 0x04000D30 RID: 3376
		public FireWatcher fireWatcher;

		// Token: 0x04000D31 RID: 3377
		public DangerWatcher dangerWatcher;

		// Token: 0x04000D32 RID: 3378
		public DamageWatcher damageWatcher;

		// Token: 0x04000D33 RID: 3379
		public StrengthWatcher strengthWatcher;

		// Token: 0x04000D34 RID: 3380
		public WealthWatcher wealthWatcher;

		// Token: 0x04000D35 RID: 3381
		public RegionDirtyer regionDirtyer;

		// Token: 0x04000D36 RID: 3382
		public MapCellsInRandomOrder cellsInRandomOrder;

		// Token: 0x04000D37 RID: 3383
		public RememberedCameraPos rememberedCameraPos;

		// Token: 0x04000D38 RID: 3384
		public MineStrikeManager mineStrikeManager;

		// Token: 0x04000D39 RID: 3385
		public StoryState storyState;

		// Token: 0x04000D3A RID: 3386
		public RoadInfo roadInfo;

		// Token: 0x04000D3B RID: 3387
		public WaterInfo waterInfo;

		// Token: 0x04000D3C RID: 3388
		public RetainedCaravanData retainedCaravanData;

		// Token: 0x04000D3D RID: 3389
		public TemporaryThingDrawer temporaryThingDrawer;

		// Token: 0x04000D3E RID: 3390
		public AnimalPenManager animalPenManager;

		// Token: 0x04000D3F RID: 3391
		public MapPlantGrowthRateCalculator plantGrowthRateCalculator;

		// Token: 0x04000D40 RID: 3392
		public AutoSlaughterManager autoSlaughterManager;

		// Token: 0x04000D41 RID: 3393
		public TreeDestructionTracker treeDestructionTracker;

		// Token: 0x04000D42 RID: 3394
		public StorageGroupManager storageGroups;

		// Token: 0x04000D43 RID: 3395
		public EffecterMaintainer effecterMaintainer;

		// Token: 0x04000D44 RID: 3396
		public PostTickVisuals postTickVisuals;

		// Token: 0x04000D45 RID: 3397
		public const string ThingSaveKey = "thing";

		// Token: 0x04000D46 RID: 3398
		[TweakValue("Graphics_Shadow", 0f, 100f)]
		private static bool AlwaysRedrawShadows;
	}
}

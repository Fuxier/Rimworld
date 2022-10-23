using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000228 RID: 552
	public static class MapGenerator
	{
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0005AFC2 File Offset: 0x000591C2
		public static MapGenFloatGrid Elevation
		{
			get
			{
				return MapGenerator.FloatGridNamed("Elevation");
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x0005AFCE File Offset: 0x000591CE
		public static MapGenFloatGrid Fertility
		{
			get
			{
				return MapGenerator.FloatGridNamed("Fertility");
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0005AFDA File Offset: 0x000591DA
		public static MapGenFloatGrid Caves
		{
			get
			{
				return MapGenerator.FloatGridNamed("Caves");
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x0005AFE6 File Offset: 0x000591E6
		// (set) Token: 0x06000FA8 RID: 4008 RVA: 0x0005B009 File Offset: 0x00059209
		public static IntVec3 PlayerStartSpot
		{
			get
			{
				if (!MapGenerator.playerStartSpotInt.IsValid)
				{
					Log.Error("Accessing player start spot before setting it.");
					return IntVec3.Zero;
				}
				return MapGenerator.playerStartSpotInt;
			}
			set
			{
				MapGenerator.playerStartSpotInt = value;
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0005B014 File Offset: 0x00059214
		public static Map GenerateMap(IntVec3 mapSize, MapParent parent, MapGeneratorDef mapGenerator, IEnumerable<GenStepWithParams> extraGenStepDefs = null, Action<Map> extraInitBeforeContentGen = null)
		{
			ProgramState programState = Current.ProgramState;
			Current.ProgramState = ProgramState.MapInitializing;
			MapGenerator.playerStartSpotInt = IntVec3.Invalid;
			MapGenerator.rootsToUnfog.Clear();
			MapGenerator.data.Clear();
			MapGenerator.mapBeingGenerated = null;
			DeepProfiler.Start("InitNewGeneratedMap");
			Rand.PushState();
			int seed = Gen.HashCombineInt(Find.World.info.Seed, parent.Tile);
			Rand.Seed = seed;
			Map result;
			try
			{
				if (parent != null && parent.HasMap)
				{
					Log.Error("Tried to generate a new map and set " + parent + " as its parent, but this world object already has a map. One world object can't have more than 1 map.");
					parent = null;
				}
				DeepProfiler.Start("Set up map");
				Map map = new Map();
				map.uniqueID = Find.UniqueIDsManager.GetNextMapID();
				map.generationTick = GenTicks.TicksGame;
				MapGenerator.mapBeingGenerated = map;
				map.info.Size = mapSize;
				map.info.parent = parent;
				map.ConstructComponents();
				DeepProfiler.End();
				Current.Game.AddMap(map);
				if (extraInitBeforeContentGen != null)
				{
					extraInitBeforeContentGen(map);
				}
				if (mapGenerator == null)
				{
					Log.Error("Attempted to generate map without generator; falling back on encounter map");
					mapGenerator = MapGeneratorDefOf.Encounter;
				}
				IEnumerable<GenStepWithParams> enumerable = from g in mapGenerator.genSteps
				where !Find.Scenario.parts.Any((ScenPart p) => p.def.scenPartClass == typeof(ScenPart_DisableMapGen) && p.def.genStep == g)
				select g into x
				select new GenStepWithParams(x, default(GenStepParams));
				if (extraGenStepDefs != null)
				{
					enumerable = enumerable.Concat(extraGenStepDefs);
				}
				map.areaManager.AddStartingAreas();
				map.weatherDecider.StartInitialWeather();
				DeepProfiler.Start("Generate contents into map");
				MapGenerator.GenerateContentsIntoMap(enumerable, map, seed);
				DeepProfiler.End();
				Find.Scenario.PostMapGenerate(map);
				DeepProfiler.Start("Finalize map init");
				map.FinalizeInit();
				DeepProfiler.End();
				DeepProfiler.Start("MapComponent.MapGenerated()");
				MapComponentUtility.MapGenerated(map);
				DeepProfiler.End();
				if (parent != null)
				{
					parent.PostMapGenerate();
				}
				result = map;
			}
			finally
			{
				DeepProfiler.End();
				MapGenerator.mapBeingGenerated = null;
				Current.ProgramState = programState;
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0005B224 File Offset: 0x00059424
		public static void GenerateContentsIntoMap(IEnumerable<GenStepWithParams> genStepDefs, Map map, int seed)
		{
			MapGenerator.data.Clear();
			Rand.PushState();
			try
			{
				Rand.Seed = seed;
				RockNoises.Init(map);
				MapGenerator.tmpGenSteps.Clear();
				MapGenerator.tmpGenSteps.AddRange(from x in genStepDefs
				orderby x.def.order, x.def.index
				select x);
				for (int i = 0; i < MapGenerator.tmpGenSteps.Count; i++)
				{
					DeepProfiler.Start("GenStep - " + MapGenerator.tmpGenSteps[i].def);
					try
					{
						Rand.Seed = Gen.HashCombineInt(seed, MapGenerator.GetSeedPart(MapGenerator.tmpGenSteps, i));
						MapGenerator.tmpGenSteps[i].def.genStep.Generate(map, MapGenerator.tmpGenSteps[i].parms);
					}
					catch (Exception arg)
					{
						Log.Error("Error in GenStep: " + arg);
					}
					finally
					{
						DeepProfiler.End();
					}
				}
			}
			finally
			{
				Rand.PopState();
				RockNoises.Reset();
				MapGenerator.data.Clear();
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0005B37C File Offset: 0x0005957C
		public static T GetVar<T>(string name)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0005B3A8 File Offset: 0x000595A8
		public static bool TryGetVar<T>(string name, out T var)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				var = (T)((object)obj);
				return true;
			}
			var = default(T);
			return false;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0005B3DA File Offset: 0x000595DA
		public static void SetVar<T>(string name, T var)
		{
			MapGenerator.data[name] = var;
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0005B3F0 File Offset: 0x000595F0
		public static MapGenFloatGrid FloatGridNamed(string name)
		{
			MapGenFloatGrid var = MapGenerator.GetVar<MapGenFloatGrid>(name);
			if (var != null)
			{
				return var;
			}
			MapGenFloatGrid mapGenFloatGrid = new MapGenFloatGrid(MapGenerator.mapBeingGenerated);
			MapGenerator.SetVar<MapGenFloatGrid>(name, mapGenFloatGrid);
			return mapGenFloatGrid;
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0005B41C File Offset: 0x0005961C
		private static int GetSeedPart(List<GenStepWithParams> genSteps, int index)
		{
			int seedPart = genSteps[index].def.genStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (MapGenerator.tmpGenSteps[i].def.genStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x04000DF4 RID: 3572
		public static Map mapBeingGenerated;

		// Token: 0x04000DF5 RID: 3573
		private static Dictionary<string, object> data = new Dictionary<string, object>();

		// Token: 0x04000DF6 RID: 3574
		private static IntVec3 playerStartSpotInt = IntVec3.Invalid;

		// Token: 0x04000DF7 RID: 3575
		public static List<IntVec3> rootsToUnfog = new List<IntVec3>();

		// Token: 0x04000DF8 RID: 3576
		private static List<GenStepWithParams> tmpGenSteps = new List<GenStepWithParams>();

		// Token: 0x04000DF9 RID: 3577
		public const string ElevationName = "Elevation";

		// Token: 0x04000DFA RID: 3578
		public const string FertilityName = "Fertility";

		// Token: 0x04000DFB RID: 3579
		public const string CavesName = "Caves";

		// Token: 0x04000DFC RID: 3580
		public const string RectOfInterestName = "RectOfInterest";

		// Token: 0x04000DFD RID: 3581
		public const string UsedRectsName = "UsedRects";

		// Token: 0x04000DFE RID: 3582
		public const string RectOfInterestTurretsGenStepsCount = "RectOfInterestTurretsGenStepsCount";
	}
}

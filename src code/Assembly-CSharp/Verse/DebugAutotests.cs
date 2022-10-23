using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200043C RID: 1084
	public static class DebugAutotests
	{
		// Token: 0x0600202B RID: 8235 RVA: 0x000C0D6F File Offset: 0x000BEF6F
		[DebugAction("Autotests", "Make colony (full)", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyFull()
		{
			Autotests_ColonyMaker.MakeColony_Full();
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x000C0D76 File Offset: 0x000BEF76
		[DebugAction("Autotests", "Make colony (animals)", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyAnimals()
		{
			Autotests_ColonyMaker.MakeColony_Animals();
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x000C0D7D File Offset: 0x000BEF7D
		[DebugAction("Autotests", "Make colony (ancient junk)", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyAncientJunk()
		{
			Autotests_ColonyMaker.MakeColony_AncientJunk();
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x000C0D84 File Offset: 0x000BEF84
		[DebugAction("Autotests", "Test force downed x100", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestForceDownedx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				HealthUtility.DamageUntilDowned(pawn, true);
				if (pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died while force downing: ",
						pawn,
						" at ",
						pawn.Position
					}));
					return;
				}
			}
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x000C0E38 File Offset: 0x000BF038
		[DebugAction("Autotests", "Test force kill x100", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestForceKillx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				HealthUtility.DamageUntilDead(pawn);
				if (!pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died not die: ",
						pawn,
						" at ",
						pawn.Position
					}));
					return;
				}
			}
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x000C0EEC File Offset: 0x000BF0EC
		[DebugAction("Autotests", "Test generate pawn x1000", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestGeneratePawnx1000()
		{
			float[] array = new float[]
			{
				10f,
				20f,
				50f,
				100f,
				200f,
				500f,
				1000f,
				2000f,
				5000f,
				1E+20f
			};
			int[] array2 = new int[array.Length];
			for (int i = 0; i < 1000; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				PerfLogger.Reset();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				float ms = PerfLogger.Duration() * 1000f;
				array2[array.FirstIndexOf((float time) => ms <= time)]++;
				if (pawn.Dead)
				{
					Log.Error("Pawn is dead");
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Pawn creation time histogram:");
			for (int j = 0; j < array2.Length; j++)
			{
				stringBuilder.AppendLine(string.Format("<{0}ms: {1}", array[j], array2[j]));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x000C0FEC File Offset: 0x000BF1EC
		[DebugAction("Autotests", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GeneratePawnsOfAllShapes()
		{
			Rot4[] array = new Rot4[]
			{
				Rot4.North,
				Rot4.East,
				Rot4.South,
				Rot4.West
			};
			IntVec3 intVec = UI.MouseCell();
			foreach (BodyTypeDef bodyTypeDef in DefDatabase<BodyTypeDef>.AllDefs)
			{
				IntVec3 intVec2 = intVec;
				foreach (Rot4 rot in array)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false)
					{
						ForceBodyType = bodyTypeDef
					});
					string text = bodyTypeDef.defName + "-" + rot.ToStringWord();
					pawn.Name = new NameTriple(text, text, text);
					GenSpawn.Spawn(pawn, intVec2, Find.CurrentMap, WipeMode.Vanish);
					pawn.apparel.DestroyAll(DestroyMode.Vanish);
					pawn.drafter.Drafted = true;
					pawn.stances.SetStance(new Stance_Warmup(100000, intVec2 + rot.FacingCell, null));
					intVec2 += IntVec3.South * 2;
				}
				intVec += IntVec3.East * 2;
			}
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x000C11F0 File Offset: 0x000BF3F0
		[DebugAction("Autotests", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckRegionListers()
		{
			Autotests_RegionListers.CheckBugs(Find.CurrentMap);
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x000C11FC File Offset: 0x000BF3FC
		[DebugAction("Autotests", "Test time-to-down", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestTimeToDown()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<PawnKindDef> enumerator = (from kd in DefDatabase<PawnKindDef>.AllDefs
			orderby kd.defName
			select kd).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kindDef = enumerator.Current;
					list.Add(new DebugMenuOption(kindDef.label, DebugMenuOptionMode.Action, delegate()
					{
						if (kindDef == PawnKindDefOf.Colonist)
						{
							Log.Message("Current colonist TTD reference point: 22.3 seconds, stddev 8.35 seconds");
						}
						List<float> results = new List<float>();
						List<PawnKindDef> list2 = new List<PawnKindDef>();
						List<PawnKindDef> list3 = new List<PawnKindDef>();
						list2.Add(kindDef);
						list3.Add(kindDef);
						ArenaUtility.BeginArenaFightSet(1000, list2, list3, delegate(ArenaUtility.ArenaResult result)
						{
							if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
							{
								results.Add(result.tickDuration.TicksToSeconds());
							}
						}, delegate
						{
							string format = "Finished {0} tests; time-to-down {1}, stddev {2}\n\nraw: {3}";
							object[] array = new object[4];
							array[0] = results.Count;
							array[1] = results.Average();
							array[2] = GenMath.Stddev(results);
							array[3] = (from res in results
							select res.ToString()).ToLineList(null, false);
							Log.Message(string.Format(format, array));
						});
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x000C12AC File Offset: 0x000BF4AC
		[DebugAction("Autotests", "Battle Royale All PawnKinds", false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleAllPawnKinds()
		{
			ArenaUtility.PerformBattleRoyale(DefDatabase<PawnKindDef>.AllDefs);
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x000C12B8 File Offset: 0x000BF4B8
		[DebugAction("Autotests", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleHumanlikes()
		{
			ArenaUtility.PerformBattleRoyale(from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Humanlike
			select k);
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000C12E8 File Offset: 0x000BF4E8
		[DebugAction("Autotests", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleByDamagetype()
		{
			PawnKindDef[] array = new PawnKindDef[]
			{
				PawnKindDefOf.Colonist,
				PawnKindDefOf.Muffalo
			};
			IEnumerable<ToolCapacityDef> enumerable = from tc in DefDatabase<ToolCapacityDef>.AllDefsListForReading
			where tc != ToolCapacityDefOf.KickMaterialInEyes
			select tc;
			Func<PawnKindDef, ToolCapacityDef, string> func = (PawnKindDef pkd, ToolCapacityDef dd) => string.Format("{0}_{1}", pkd.label, dd.defName);
			if (DebugAutotests.pawnKindsForDamageTypeBattleRoyale == null)
			{
				DebugAutotests.pawnKindsForDamageTypeBattleRoyale = new List<PawnKindDef>();
				foreach (PawnKindDef pawnKindDef in array)
				{
					using (IEnumerator<ToolCapacityDef> enumerator = enumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ToolCapacityDef toolType = enumerator.Current;
							string text = func(pawnKindDef, toolType);
							ThingDef thingDef = Gen.MemberwiseClone<ThingDef>(pawnKindDef.race);
							thingDef.defName = text;
							thingDef.label = text;
							thingDef.tools = new List<Tool>(pawnKindDef.race.tools.Select(delegate(Tool tool)
							{
								Tool tool2 = Gen.MemberwiseClone<Tool>(tool);
								tool2.capacities = new List<ToolCapacityDef>();
								tool2.capacities.Add(toolType);
								return tool2;
							}));
							PawnKindDef pawnKindDef2 = Gen.MemberwiseClone<PawnKindDef>(pawnKindDef);
							pawnKindDef2.defName = text;
							pawnKindDef2.label = text;
							pawnKindDef2.race = thingDef;
							DebugAutotests.pawnKindsForDamageTypeBattleRoyale.Add(pawnKindDef2);
						}
					}
				}
			}
			ArenaUtility.PerformBattleRoyale(DebugAutotests.pawnKindsForDamageTypeBattleRoyale);
		}

		// Token: 0x040015AC RID: 5548
		private static List<PawnKindDef> pawnKindsForDamageTypeBattleRoyale;
	}
}

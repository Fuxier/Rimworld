using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200017A RID: 378
	public static class StartingPawnUtility
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00031B60 File Offset: 0x0002FD60
		private static List<Pawn> StartingAndOptionalPawns
		{
			get
			{
				return Find.GameInitData.startingAndOptionalPawns;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00031B6C File Offset: 0x0002FD6C
		private static Dictionary<Pawn, List<ThingDefCount>> StartingPossessions
		{
			get
			{
				return Find.GameInitData.startingPossessions;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00031B78 File Offset: 0x0002FD78
		private static PawnGenerationRequest DefaultStartingPawnRequest
		{
			get
			{
				return new PawnGenerationRequest(Find.GameInitData.startingPawnKind ?? Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, PawnGenerationContext.PlayerStarter, -1, true, false, false, true, TutorSystem.TutorialMode, 20f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, ModsConfig.BiotechActive ? XenotypeDefOf.Baseliner : null, null, null, 0f, DevelopmentalStage.Adult, null, ModsConfig.BiotechActive ? new FloatRange?(StartingPawnUtility.ExcludeBiologicalAgeRange) : null, null, false);
			}
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00031C48 File Offset: 0x0002FE48
		public static void ClearAllStartingPawns()
		{
			for (int i = StartingPawnUtility.StartingAndOptionalPawns.Count - 1; i >= 0; i--)
			{
				StartingPawnUtility.StartingAndOptionalPawns[i].relations.ClearAllRelations();
				if (Find.World != null)
				{
					PawnUtility.DestroyStartingColonistFamily(StartingPawnUtility.StartingAndOptionalPawns[i]);
					PawnComponentsUtility.RemoveComponentsOnDespawned(StartingPawnUtility.StartingAndOptionalPawns[i]);
					Find.WorldPawns.PassToWorld(StartingPawnUtility.StartingAndOptionalPawns[i], PawnDiscardDecideMode.Discard);
				}
				StartingPawnUtility.StartingPossessions.Remove(StartingPawnUtility.StartingAndOptionalPawns[i]);
				StartingPawnUtility.StartingAndOptionalPawns.RemoveAt(i);
			}
			StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.Clear();
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00031CE9 File Offset: 0x0002FEE9
		public static Pawn RandomizeInPlace(Pawn p)
		{
			return StartingPawnUtility.RegenerateStartingPawnInPlace(StartingPawnUtility.StartingAndOptionalPawns.IndexOf(p));
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00031CFC File Offset: 0x0002FEFC
		private static Pawn RegenerateStartingPawnInPlace(int index)
		{
			Pawn pawn = StartingPawnUtility.StartingAndOptionalPawns[index];
			PawnUtility.TryDestroyStartingColonistFamily(pawn);
			pawn.relations.ClearAllRelations();
			PawnComponentsUtility.RemoveComponentsOnDespawned(pawn);
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			StartingPawnUtility.StartingPossessions.Remove(pawn);
			StartingPawnUtility.StartingAndOptionalPawns[index] = null;
			for (int i = 0; i < StartingPawnUtility.StartingAndOptionalPawns.Count; i++)
			{
				if (StartingPawnUtility.StartingAndOptionalPawns[i] != null)
				{
					PawnUtility.TryDestroyStartingColonistFamily(StartingPawnUtility.StartingAndOptionalPawns[i]);
				}
			}
			Pawn pawn2 = StartingPawnUtility.NewGeneratedStartingPawn(index);
			StartingPawnUtility.StartingAndOptionalPawns[index] = pawn2;
			return pawn2;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00031D96 File Offset: 0x0002FF96
		public static PawnGenerationRequest GetGenerationRequest(int index)
		{
			StartingPawnUtility.EnsureGenerationRequestInRangeOf(index);
			return StartingPawnUtility.StartingAndOptionalPawnGenerationRequests[index];
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00031DA9 File Offset: 0x0002FFA9
		public static void SetGenerationRequest(int index, PawnGenerationRequest request)
		{
			StartingPawnUtility.EnsureGenerationRequestInRangeOf(index);
			StartingPawnUtility.StartingAndOptionalPawnGenerationRequests[index] = request;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00031DC0 File Offset: 0x0002FFC0
		public static void ReorderRequests(int from, int to)
		{
			StartingPawnUtility.EnsureGenerationRequestInRangeOf((from > to) ? from : to);
			PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(from);
			StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.Insert(to, generationRequest);
			StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.RemoveAt((from < to) ? from : (from + 1));
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00031E01 File Offset: 0x00030001
		private static void EnsureGenerationRequestInRangeOf(int index)
		{
			while (StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.Count <= index)
			{
				StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.Add(StartingPawnUtility.DefaultStartingPawnRequest);
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00031E21 File Offset: 0x00030021
		public static int PawnIndex(Pawn pawn)
		{
			return Mathf.Max(StartingPawnUtility.StartingAndOptionalPawns.IndexOf(pawn), 0);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00031E34 File Offset: 0x00030034
		public static Pawn NewGeneratedStartingPawn(int index = -1)
		{
			PawnGenerationRequest request = (index < 0) ? StartingPawnUtility.DefaultStartingPawnRequest : StartingPawnUtility.GetGenerationRequest(index);
			Pawn pawn = null;
			try
			{
				pawn = PawnGenerator.GeneratePawn(request);
			}
			catch (Exception arg)
			{
				Log.Error("There was an exception thrown by the PawnGenerator during generating a starting pawn. Trying one more time...\nException: " + arg);
				pawn = PawnGenerator.GeneratePawn(request);
			}
			pawn.relations.everSeenByPlayer = true;
			PawnComponentsUtility.AddComponentsForSpawn(pawn);
			StartingPawnUtility.GeneratePossessions(pawn);
			return pawn;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00031EA4 File Offset: 0x000300A4
		private static void GeneratePossessions(Pawn pawn)
		{
			if (!StartingPawnUtility.StartingPossessions.ContainsKey(pawn))
			{
				StartingPawnUtility.StartingPossessions.Add(pawn, new List<ThingDefCount>());
			}
			else
			{
				StartingPawnUtility.StartingPossessions[pawn].Clear();
			}
			if (Find.Scenario.AllParts.Any((ScenPart x) => x is ScenPart_NoPossessions))
			{
				return;
			}
			if (ModsConfig.BiotechActive && pawn.DevelopmentalStage.Baby())
			{
				StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(ThingDefOf.BabyFood, StartingPawnUtility.BabyFoodCountRange.RandomInRange));
				return;
			}
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
				{
					return;
				}
				Hediff_Addiction hediff_Addiction;
				if ((hediff_Addiction = (hediff as Hediff_Addiction)) != null)
				{
					Need_Chemical need = hediff_Addiction.Need;
					ThingDef thingDef = StartingPawnUtility.<GeneratePossessions>g__GetDrugFor|23_1(hediff_Addiction.Chemical);
					if (need != null && thingDef != null)
					{
						int count = GenMath.RoundRandom(need.def.fallPerDay * StartingPawnUtility.DaysSatisfied.RandomInRange / thingDef.GetCompProperties<CompProperties_Drug>().needLevelOffset);
						StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(thingDef, count));
					}
				}
			}
			if (ModsConfig.BiotechActive)
			{
				foreach (Hediff hediff2 in pawn.health.hediffSet.hediffs)
				{
					if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
					{
						return;
					}
					Hediff_ChemicalDependency hediff_ChemicalDependency;
					if ((hediff_ChemicalDependency = (hediff2 as Hediff_ChemicalDependency)) != null && hediff_ChemicalDependency.Visible)
					{
						ThingDef thingDef2 = StartingPawnUtility.<GeneratePossessions>g__GetDrugFor|23_1(hediff_ChemicalDependency.chemical);
						if (thingDef2 != null)
						{
							HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = hediff_ChemicalDependency.def.CompProps<HediffCompProperties_SeverityPerDay>();
							float num = (hediffCompProperties_SeverityPerDay != null) ? hediffCompProperties_SeverityPerDay.severityPerDay : 1f;
							StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(thingDef2, GenMath.RoundRandom(StartingPawnUtility.DaysSatisfied.RandomInRange * num)));
						}
					}
				}
			}
			if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
			{
				return;
			}
			if (ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.HasGene(GeneDefOf.Hemogenic))
			{
				StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(ThingDefOf.HemogenPack, StartingPawnUtility.HemogenCountRange.RandomInRange));
				if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
				{
					return;
				}
			}
			if (Rand.Value < 0.25f)
			{
				BackstoryDef backstory = pawn.story.GetBackstory(BackstorySlot.Adulthood);
				if (backstory != null)
				{
					foreach (BackstoryThingDefCountClass backstoryThingDefCountClass in backstory.possessions)
					{
						if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
						{
							return;
						}
						StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(backstoryThingDefCountClass.key, Mathf.Min(backstoryThingDefCountClass.key.stackLimit, backstoryThingDefCountClass.value)));
					}
				}
			}
			if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
			{
				return;
			}
			if (Rand.Value < 0.06f)
			{
				ThingDef thingDef3;
				if ((from x in DefDatabase<ThingDef>.AllDefs
				where x.possessionCount > 0
				select x).TryRandomElement(out thingDef3))
				{
					StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(thingDef3, Mathf.Min(thingDef3.stackLimit, thingDef3.possessionCount)));
				}
			}
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x000322A0 File Offset: 0x000304A0
		public static void AddNewPawn(int index = -1)
		{
			Pawn pawn = StartingPawnUtility.NewGeneratedStartingPawn(index);
			StartingPawnUtility.StartingAndOptionalPawns.Add(pawn);
			StartingPawnUtility.GeneratePossessions(pawn);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x000322C8 File Offset: 0x000304C8
		public static bool WorkTypeRequirementsSatisfied()
		{
			if (StartingPawnUtility.StartingAndOptionalPawns.Count == 0)
			{
				return false;
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (workTypeDef.requireCapableColonist)
				{
					bool flag = false;
					for (int j = 0; j < Find.GameInitData.startingPawnCount; j++)
					{
						if (!StartingPawnUtility.StartingAndOptionalPawns[j].WorkTypeIsDisabled(workTypeDef))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			if (TutorSystem.TutorialMode)
			{
				if (StartingPawnUtility.StartingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount).Any((Pawn p) => p.WorkTagIsDisabled(WorkTags.Violent)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00032386 File Offset: 0x00030586
		public static IEnumerable<WorkTypeDef> RequiredWorkTypesDisabledForEveryone()
		{
			List<WorkTypeDef> workTypes = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < workTypes.Count; i = num + 1)
			{
				WorkTypeDef workTypeDef = workTypes[i];
				if (workTypeDef.requireCapableColonist)
				{
					bool flag = false;
					List<Pawn> startingAndOptionalPawns = StartingPawnUtility.StartingAndOptionalPawns;
					for (int j = 0; j < Find.GameInitData.startingPawnCount; j++)
					{
						if (!startingAndOptionalPawns[j].WorkTypeIsDisabled(workTypeDef))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						yield return workTypeDef;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000323EC File Offset: 0x000305EC
		[CompilerGenerated]
		internal static ThingDef <GeneratePossessions>g__GetDrugFor|23_1(ChemicalDef chemical)
		{
			ThingDef result;
			if (DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef x)
			{
				CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
				return ((compProperties != null) ? compProperties.chemical : null) == chemical;
			}).TryRandomElementByWeight((ThingDef x) => x.generateCommonality, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04000A48 RID: 2632
		private const float ChanceToHavePossessionsFromBackground = 0.25f;

		// Token: 0x04000A49 RID: 2633
		private static readonly IntRange BabyFoodCountRange = new IntRange(30, 40);

		// Token: 0x04000A4A RID: 2634
		private static readonly IntRange HemogenCountRange = new IntRange(8, 12);

		// Token: 0x04000A4B RID: 2635
		private static readonly FloatRange ExcludeBiologicalAgeRange = new FloatRange(12.1f, 13f);

		// Token: 0x04000A4C RID: 2636
		private static List<PawnGenerationRequest> StartingAndOptionalPawnGenerationRequests = new List<PawnGenerationRequest>();

		// Token: 0x04000A4D RID: 2637
		private const int MaxPossessionsCount = 2;

		// Token: 0x04000A4E RID: 2638
		private static readonly FloatRange DaysSatisfied = new FloatRange(25f, 35f);

		// Token: 0x04000A4F RID: 2639
		private const float ChanceForRandomPossession = 0.06f;
	}
}

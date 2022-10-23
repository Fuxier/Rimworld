using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001F7 RID: 503
	public sealed class MapPawns
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x0004F2E8 File Offset: 0x0004D4E8
		public List<Pawn> AllPawns
		{
			get
			{
				List<Pawn> allPawnsUnspawned = this.AllPawnsUnspawned;
				if (allPawnsUnspawned.Count == 0)
				{
					return this.pawnsSpawned;
				}
				this.allPawnsResult.Clear();
				this.allPawnsResult.AddRange(this.pawnsSpawned);
				this.allPawnsResult.AddRange(allPawnsUnspawned);
				return this.allPawnsResult;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0004F33C File Offset: 0x0004D53C
		public List<Pawn> AllPawnsUnspawned
		{
			get
			{
				this.allPawnsUnspawnedResult.Clear();
				ThingOwnerUtility.GetAllThingsRecursively<Pawn>(this.map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), this.allPawnsUnspawnedResult, true, null, false);
				for (int i = this.allPawnsUnspawnedResult.Count - 1; i >= 0; i--)
				{
					if (this.allPawnsUnspawnedResult[i].Dead)
					{
						this.allPawnsUnspawnedResult.RemoveAt(i);
					}
				}
				return this.allPawnsUnspawnedResult;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0004F3AC File Offset: 0x0004D5AC
		public List<Pawn> FreeColonists
		{
			get
			{
				return this.FreeHumanlikesOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0004F3BC File Offset: 0x0004D5BC
		public List<Pawn> PrisonersOfColony
		{
			get
			{
				this.prisonersOfColonyResult.Clear();
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsPrisonerOfColony)
					{
						this.prisonersOfColonyResult.Add(allPawns[i]);
					}
				}
				return this.prisonersOfColonyResult;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0004F414 File Offset: 0x0004D614
		public List<Pawn> FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> freeColonists = this.FreeColonists;
				List<Pawn> prisonersOfColony = this.PrisonersOfColony;
				if (prisonersOfColony.Count == 0)
				{
					return freeColonists;
				}
				this.freeColonistsAndPrisonersResult.Clear();
				this.freeColonistsAndPrisonersResult.AddRange(freeColonists);
				this.freeColonistsAndPrisonersResult.AddRange(prisonersOfColony);
				return this.freeColonistsAndPrisonersResult;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0004F464 File Offset: 0x0004D664
		public int ColonistCount
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					Log.Error("ColonistCount while not playing. This should get the starting player pawn count.");
					return 3;
				}
				int num = 0;
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x0004F4B2 File Offset: 0x0004D6B2
		public int AllPawnsCount
		{
			get
			{
				return this.AllPawns.Count;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0004F4BF File Offset: 0x0004D6BF
		public int AllPawnsUnspawnedCount
		{
			get
			{
				return this.AllPawnsUnspawned.Count;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0004F4CC File Offset: 0x0004D6CC
		public int FreeColonistsCount
		{
			get
			{
				return this.FreeColonists.Count;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0004F4D9 File Offset: 0x0004D6D9
		public int PrisonersOfColonyCount
		{
			get
			{
				return this.PrisonersOfColony.Count;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0004F4E6 File Offset: 0x0004D6E6
		public int FreeColonistsAndPrisonersCount
		{
			get
			{
				return this.FreeColonistsCount + this.PrisonersOfColonyCount;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0004F4F8 File Offset: 0x0004D6F8
		public bool AnyPawnBlockingMapRemoval
		{
			get
			{
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					if (!this.pawnsSpawned[i].Downed && this.pawnsSpawned[i].IsColonist)
					{
						return true;
					}
					if (this.pawnsSpawned[i].relations != null && this.pawnsSpawned[i].relations.relativeInvolvedInRescueQuest != null)
					{
						return true;
					}
					if (this.pawnsSpawned[i].Faction == ofPlayer || this.pawnsSpawned[i].HostFaction == ofPlayer)
					{
						Job curJob = this.pawnsSpawned[i].CurJob;
						if (curJob != null && curJob.exitMapOnArrival)
						{
							return true;
						}
						if (this.pawnsSpawned[i].health.hediffSet.InLabor(true))
						{
							return true;
						}
					}
					if (CaravanExitMapUtility.FindCaravanToJoinFor(this.pawnsSpawned[i]) != null && !this.pawnsSpawned[i].Downed)
					{
						return true;
					}
					if (ModsConfig.BiotechActive && this.pawnsSpawned[i].IsColonyMech && this.pawnsSpawned[i].GetOverseer() != null)
					{
						return true;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					IThingHolder thingHolder = MapPawns.PlayerEjectablePodHolder(list[j], false);
					if (thingHolder != null)
					{
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(thingHolder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && !pawn.Downed && pawn.IsColonist)
							{
								this.tmpThings.Clear();
								return true;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return false;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0004F6F9 File Offset: 0x0004D8F9
		public List<Pawn> AllPawnsSpawned
		{
			get
			{
				return this.pawnsSpawned;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0004F701 File Offset: 0x0004D901
		public List<Pawn> FreeColonistsSpawned
		{
			get
			{
				return this.FreeHumanlikesSpawnedOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0004F70E File Offset: 0x0004D90E
		public List<Pawn> PrisonersOfColonySpawned
		{
			get
			{
				return this.prisonersOfColonySpawned;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0004F716 File Offset: 0x0004D916
		public List<Pawn> SlavesOfColonySpawned
		{
			get
			{
				return this.slavesOfColonySpawned;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x0004F720 File Offset: 0x0004D920
		public List<Pawn> FreeColonistsAndPrisonersSpawned
		{
			get
			{
				List<Pawn> freeColonistsSpawned = this.FreeColonistsSpawned;
				List<Pawn> list = this.PrisonersOfColonySpawned;
				if (list.Count == 0)
				{
					return freeColonistsSpawned;
				}
				this.freeColonistsAndPrisonersSpawnedResult.Clear();
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(freeColonistsSpawned);
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(list);
				return this.freeColonistsAndPrisonersSpawnedResult;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0004F770 File Offset: 0x0004D970
		public List<Pawn> SpawnedPawnsWithAnyHediff
		{
			get
			{
				this.spawnedPawnsWithAnyHediffResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].health.hediffSet.hediffs.Count != 0)
					{
						this.spawnedPawnsWithAnyHediffResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWithAnyHediffResult;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0004F7D8 File Offset: 0x0004D9D8
		public List<Pawn> SpawnedHungryPawns
		{
			get
			{
				this.spawnedHungryPawnsResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (FeedPatientUtility.IsHungry(allPawnsSpawned[i]))
					{
						this.spawnedHungryPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedHungryPawnsResult;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0004F830 File Offset: 0x0004DA30
		public List<Pawn> SpawnedPawnsWithMiscNeeds
		{
			get
			{
				this.spawnedPawnsWithMiscNeedsResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (!allPawnsSpawned[i].needs.MiscNeeds.NullOrEmpty<Need>())
					{
						this.spawnedPawnsWithMiscNeedsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWithMiscNeedsResult;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0004F890 File Offset: 0x0004DA90
		public List<Pawn> SpawnedColonyAnimals
		{
			get
			{
				this.spawnedColonyAnimalsResult.Clear();
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].RaceProps.Animal)
					{
						this.spawnedColonyAnimalsResult.Add(list[i]);
					}
				}
				return this.spawnedColonyAnimalsResult;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0004F8F0 File Offset: 0x0004DAF0
		public List<Pawn> SpawnedDownedPawns
		{
			get
			{
				this.spawnedDownedPawnsResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Downed)
					{
						this.spawnedDownedPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedDownedPawnsResult;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0004F948 File Offset: 0x0004DB48
		public List<Pawn> SpawnedPawnsWhoShouldHaveSurgeryDoneNow
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (HealthAIUtility.ShouldHaveSurgeryDoneNow(allPawnsSpawned[i]))
					{
						this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0004F9A0 File Offset: 0x0004DBA0
		public List<Pawn> SpawnedPawnsWhoShouldHaveInventoryUnloaded
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].inventory.UnloadEverything)
					{
						this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0004F9FB File Offset: 0x0004DBFB
		public int AllPawnsSpawnedCount
		{
			get
			{
				return this.pawnsSpawned.Count;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0004FA08 File Offset: 0x0004DC08
		public int FreeColonistsSpawnedCount
		{
			get
			{
				return this.FreeColonistsSpawned.Count;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0004FA15 File Offset: 0x0004DC15
		public int PrisonersOfColonySpawnedCount
		{
			get
			{
				return this.PrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0004FA22 File Offset: 0x0004DC22
		public int FreeColonistsAndPrisonersSpawnedCount
		{
			get
			{
				return this.FreeColonistsAndPrisonersSpawned.Count;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0004FA30 File Offset: 0x0004DC30
		public int ColonistsSpawnedCount
		{
			get
			{
				int num = 0;
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0004FA70 File Offset: 0x0004DC70
		public int FreeColonistsSpawnedOrInPlayerEjectablePodsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					if (this.pawnsSpawned[i].IsFreeColonist)
					{
						num++;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					IThingHolder thingHolder = MapPawns.PlayerEjectablePodHolder(list[j], true);
					if (thingHolder != null)
					{
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(thingHolder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && pawn.IsFreeColonist)
							{
								num++;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return num;
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0004FB54 File Offset: 0x0004DD54
		private static IThingHolder PlayerEjectablePodHolder(Thing thing, bool includeCryptosleepCaskets = true)
		{
			Building_CryptosleepCasket building_CryptosleepCasket = thing as Building_CryptosleepCasket;
			CompTransporter compTransporter = thing.TryGetComp<CompTransporter>();
			CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
			if ((includeCryptosleepCaskets && building_CryptosleepCasket != null && building_CryptosleepCasket.def.building.isPlayerEjectable) || thing is IActiveDropPod || thing is PawnFlyer || thing is Building_Enterable || compTransporter != null || compBiosculpterPod != null)
			{
				IThingHolder thingHolder = compTransporter;
				IThingHolder result;
				if ((result = thingHolder) == null)
				{
					thingHolder = compBiosculpterPod;
					result = (thingHolder ?? (thing as IThingHolder));
				}
				return result;
			}
			return null;
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0004FBC2 File Offset: 0x0004DDC2
		public int SlavesAndPrisonersOfColonySpawnedCount
		{
			get
			{
				return this.SlavesAndPrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0004FBD0 File Offset: 0x0004DDD0
		public bool AnyColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0004FC0C File Offset: 0x0004DE0C
		public bool AnyFreeColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsFreeColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0004FC47 File Offset: 0x0004DE47
		public List<Pawn> SlavesAndPrisonersOfColonySpawned
		{
			get
			{
				this.slavesAndPrisonersOfColonySpawnedResult.Clear();
				this.slavesAndPrisonersOfColonySpawnedResult.AddRange(this.prisonersOfColonySpawned);
				this.slavesAndPrisonersOfColonySpawnedResult.AddRange(this.slavesOfColonySpawned);
				return this.slavesAndPrisonersOfColonySpawnedResult;
			}
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0004FC7C File Offset: 0x0004DE7C
		public MapPawns(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0004FD80 File Offset: 0x0004DF80
		private void EnsureFactionsListsInit()
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				this.pawnsInFactionSpawned.GetPawnList(allFactionsListForReading[i]);
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0004FDBC File Offset: 0x0004DFBC
		public List<Pawn> PawnsInFaction(Faction faction)
		{
			List<Pawn> pawnList = this.pawnsInFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if (allPawns[i].Faction == faction)
				{
					pawnList.Add(allPawns[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0004FE11 File Offset: 0x0004E011
		public List<Pawn> SpawnedPawnsInFaction(Faction faction)
		{
			this.EnsureFactionsListsInit();
			return this.pawnsInFactionSpawned.GetPawnList(faction);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0004FE28 File Offset: 0x0004E028
		public List<Pawn> FreeHumanlikesOfFaction(Faction faction)
		{
			List<Pawn> pawnList = this.freeHumanlikesOfFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if (allPawns[i].Faction == faction && (allPawns[i].HostFaction == null || allPawns[i].IsSlave) && allPawns[i].RaceProps.Humanlike)
				{
					pawnList.Add(allPawns[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0004FEAC File Offset: 0x0004E0AC
		public List<Pawn> FreeHumanlikesSpawnedOfFaction(Faction faction)
		{
			List<Pawn> pawnList = this.freeHumanlikesSpawnedOfFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> list = this.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].HostFaction == null && list[i].RaceProps.Humanlike)
				{
					pawnList.Add(list[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0004FF14 File Offset: 0x0004E114
		public void RegisterPawn(Pawn p)
		{
			if (p.Dead)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register dead pawn ",
					p,
					" in ",
					base.GetType(),
					"."
				}));
				return;
			}
			if (!p.Spawned)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register despawned pawn ",
					p,
					" in ",
					base.GetType(),
					"."
				}));
				return;
			}
			if (p.Map != this.map)
			{
				Log.Warning("Tried to register pawn " + p + " but his Map is not this one.");
				return;
			}
			if (!p.mindState.Active)
			{
				return;
			}
			this.EnsureFactionsListsInit();
			if (!this.pawnsSpawned.Contains(p))
			{
				this.pawnsSpawned.Add(p);
			}
			if (p.Faction != null)
			{
				List<Pawn> pawnList = this.pawnsInFactionSpawned.GetPawnList(p.Faction);
				if (!pawnList.Contains(p))
				{
					pawnList.Add(p);
					if (p.Faction == Faction.OfPlayer)
					{
						pawnList.InsertionSort(delegate(Pawn a, Pawn b)
						{
							int num = (a.playerSettings != null) ? a.playerSettings.joinTick : 0;
							int value = (b.playerSettings != null) ? b.playerSettings.joinTick : 0;
							return num.CompareTo(value);
						});
					}
				}
			}
			if (p.IsPrisonerOfColony && !this.prisonersOfColonySpawned.Contains(p))
			{
				this.prisonersOfColonySpawned.Add(p);
			}
			if (p.IsSlaveOfColony && !this.slavesOfColonySpawned.Contains(p))
			{
				this.slavesOfColonySpawned.Add(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00050098 File Offset: 0x0004E298
		public void DeRegisterPawn(Pawn p)
		{
			this.EnsureFactionsListsInit();
			this.pawnsSpawned.Remove(p);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				Faction faction = allFactionsListForReading[i];
				this.pawnsInFactionSpawned.GetPawnList(faction).Remove(p);
			}
			this.prisonersOfColonySpawned.Remove(p);
			this.slavesOfColonySpawned.Remove(p);
			this.DoListChangedNotifications();
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0005010F File Offset: 0x0004E30F
		public void UpdateRegistryForPawn(Pawn p)
		{
			this.DeRegisterPawn(p);
			if (p.Spawned && p.Map == this.map)
			{
				this.RegisterPawn(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0005013B File Offset: 0x0004E33B
		private void DoListChangedNotifications()
		{
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
			if (Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00050154 File Offset: 0x0004E354
		public void LogListedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MapPawns:");
			stringBuilder.AppendLine("pawnsSpawned");
			foreach (Pawn pawn in this.pawnsSpawned)
			{
				stringBuilder.AppendLine("    " + pawn.ToString());
			}
			stringBuilder.AppendLine("AllPawnsUnspawned");
			foreach (Pawn pawn2 in this.AllPawnsUnspawned)
			{
				stringBuilder.AppendLine("    " + pawn2.ToString());
			}
			foreach (Faction faction in this.pawnsInFactionSpawned.KnownFactions())
			{
				stringBuilder.AppendLine("pawnsInFactionSpawned[" + faction.ToStringSafe<Faction>() + "]");
				foreach (Pawn pawn3 in this.pawnsInFactionSpawned.GetPawnList(faction))
				{
					stringBuilder.AppendLine("    " + pawn3.ToString());
				}
			}
			stringBuilder.AppendLine("prisonersOfColonySpawned");
			foreach (Pawn pawn4 in this.prisonersOfColonySpawned)
			{
				stringBuilder.AppendLine("    " + pawn4.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04000CC9 RID: 3273
		private Map map;

		// Token: 0x04000CCA RID: 3274
		private List<Pawn> pawnsSpawned = new List<Pawn>();

		// Token: 0x04000CCB RID: 3275
		private MapPawns.FactionDictionary pawnsInFactionSpawned = new MapPawns.FactionDictionary();

		// Token: 0x04000CCC RID: 3276
		private List<Pawn> prisonersOfColonySpawned = new List<Pawn>();

		// Token: 0x04000CCD RID: 3277
		private List<Pawn> slavesOfColonySpawned = new List<Pawn>();

		// Token: 0x04000CCE RID: 3278
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04000CCF RID: 3279
		private List<Pawn> allPawnsResult = new List<Pawn>();

		// Token: 0x04000CD0 RID: 3280
		private List<Pawn> allPawnsUnspawnedResult = new List<Pawn>();

		// Token: 0x04000CD1 RID: 3281
		private List<Pawn> prisonersOfColonyResult = new List<Pawn>();

		// Token: 0x04000CD2 RID: 3282
		private List<Pawn> freeColonistsAndPrisonersResult = new List<Pawn>();

		// Token: 0x04000CD3 RID: 3283
		private List<Pawn> freeColonistsAndPrisonersSpawnedResult = new List<Pawn>();

		// Token: 0x04000CD4 RID: 3284
		private List<Pawn> spawnedPawnsWithAnyHediffResult = new List<Pawn>();

		// Token: 0x04000CD5 RID: 3285
		private List<Pawn> spawnedHungryPawnsResult = new List<Pawn>();

		// Token: 0x04000CD6 RID: 3286
		private List<Pawn> spawnedPawnsWithMiscNeedsResult = new List<Pawn>();

		// Token: 0x04000CD7 RID: 3287
		private List<Pawn> spawnedColonyAnimalsResult = new List<Pawn>();

		// Token: 0x04000CD8 RID: 3288
		private List<Pawn> spawnedDownedPawnsResult = new List<Pawn>();

		// Token: 0x04000CD9 RID: 3289
		private List<Pawn> spawnedPawnsWhoShouldHaveSurgeryDoneNowResult = new List<Pawn>();

		// Token: 0x04000CDA RID: 3290
		private List<Pawn> spawnedPawnsWhoShouldHaveInventoryUnloadedResult = new List<Pawn>();

		// Token: 0x04000CDB RID: 3291
		private List<Pawn> slavesAndPrisonersOfColonySpawnedResult = new List<Pawn>();

		// Token: 0x04000CDC RID: 3292
		private MapPawns.FactionDictionary pawnsInFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x04000CDD RID: 3293
		private MapPawns.FactionDictionary freeHumanlikesOfFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x04000CDE RID: 3294
		private MapPawns.FactionDictionary freeHumanlikesSpawnedOfFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x02001D6E RID: 7534
		private class FactionDictionary
		{
			// Token: 0x0600B46C RID: 46188 RVA: 0x00410D9C File Offset: 0x0040EF9C
			public List<Pawn> GetPawnList(Faction faction)
			{
				if (faction == null)
				{
					return this.nullFactionPawns;
				}
				List<Pawn> result;
				if (this.pawnList.TryGetValue(faction, out result))
				{
					return result;
				}
				List<Pawn> list = new List<Pawn>(32);
				this.pawnList[faction] = list;
				return list;
			}

			// Token: 0x0600B46D RID: 46189 RVA: 0x00410DDB File Offset: 0x0040EFDB
			public IEnumerable<Faction> KnownFactions()
			{
				return this.pawnList.Keys.Concat(null);
			}

			// Token: 0x0400743A RID: 29754
			private Dictionary<Faction, List<Pawn>> pawnList = new Dictionary<Faction, List<Pawn>>(16);

			// Token: 0x0400743B RID: 29755
			private List<Pawn> nullFactionPawns = new List<Pawn>(32);
		}
	}
}

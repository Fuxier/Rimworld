using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020001B5 RID: 437
	public sealed class AutoSlaughterManager : IExposable
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x000449F0 File Offset: 0x00042BF0
		public List<Pawn> AnimalsToSlaughter
		{
			get
			{
				if (this.cacheDirty)
				{
					try
					{
						this.animalsToSlaughterCached.Clear();
						foreach (AutoSlaughterConfig autoSlaughterConfig in this.configs)
						{
							if (autoSlaughterConfig.AnyLimit)
							{
								AutoSlaughterManager.tmpAnimals.Clear();
								AutoSlaughterManager.tmpAnimalsMale.Clear();
								AutoSlaughterManager.tmpAnimalsMaleYoung.Clear();
								AutoSlaughterManager.tmpAnimalsFemale.Clear();
								AutoSlaughterManager.tmpAnimalsFemaleYoung.Clear();
								AutoSlaughterManager.tmpAnimalsPregnant.Clear();
								foreach (Pawn pawn in this.map.mapPawns.SpawnedColonyAnimals)
								{
									if (pawn.def == autoSlaughterConfig.animal && AutoSlaughterManager.CanAutoSlaughterNow(pawn) && (autoSlaughterConfig.allowSlaughterBonded || pawn.relations.GetDirectRelationsCount(PawnRelationDefOf.Bond, null) <= 0))
									{
										if (pawn.gender == Gender.Male)
										{
											if (pawn.ageTracker.CurLifeStage.reproductive)
											{
												AutoSlaughterManager.tmpAnimalsMale.Add(pawn);
											}
											else
											{
												AutoSlaughterManager.tmpAnimalsMaleYoung.Add(pawn);
											}
											AutoSlaughterManager.tmpAnimals.Add(pawn);
										}
										else if (pawn.gender == Gender.Female)
										{
											if (pawn.ageTracker.CurLifeStage.reproductive)
											{
												if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, false) == null)
												{
													AutoSlaughterManager.tmpAnimalsFemale.Add(pawn);
													AutoSlaughterManager.tmpAnimals.Add(pawn);
												}
												else if (autoSlaughterConfig.allowSlaughterPregnant)
												{
													AutoSlaughterManager.tmpAnimalsPregnant.Add(pawn);
												}
											}
											else
											{
												AutoSlaughterManager.tmpAnimalsFemaleYoung.Add(pawn);
												AutoSlaughterManager.tmpAnimals.Add(pawn);
											}
										}
										else
										{
											AutoSlaughterManager.tmpAnimals.Add(pawn);
										}
									}
								}
								AutoSlaughterManager.tmpAnimals.SortBy((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsMale.SortBy((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsMaleYoung.SortBy((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsFemale.SortBy((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsFemaleYoung.SortBy((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								if (autoSlaughterConfig.allowSlaughterPregnant)
								{
									AutoSlaughterManager.tmpAnimalsPregnant.SortBy((Pawn a) => -a.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, false).Severity);
									AutoSlaughterManager.tmpAnimalsFemale.AddRange(AutoSlaughterManager.tmpAnimalsPregnant);
									AutoSlaughterManager.tmpAnimals.AddRange(AutoSlaughterManager.tmpAnimalsPregnant);
								}
								if (autoSlaughterConfig.maxFemales != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsFemale.Count > autoSlaughterConfig.maxFemales)
									{
										Pawn item = AutoSlaughterManager.tmpAnimalsFemale.Pop<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item);
										this.animalsToSlaughterCached.Add(item);
									}
								}
								if (autoSlaughterConfig.maxFemalesYoung != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsFemaleYoung.Count > autoSlaughterConfig.maxFemalesYoung)
									{
										Pawn item2 = AutoSlaughterManager.tmpAnimalsFemaleYoung.Pop<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item2);
										this.animalsToSlaughterCached.Add(item2);
									}
								}
								if (autoSlaughterConfig.maxMales != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsMale.Count > autoSlaughterConfig.maxMales)
									{
										Pawn item3 = AutoSlaughterManager.tmpAnimalsMale.Pop<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item3);
										this.animalsToSlaughterCached.Add(item3);
									}
								}
								if (autoSlaughterConfig.maxMalesYoung != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsMaleYoung.Count > autoSlaughterConfig.maxMalesYoung)
									{
										Pawn item4 = AutoSlaughterManager.tmpAnimalsMaleYoung.Pop<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item4);
										this.animalsToSlaughterCached.Add(item4);
									}
								}
								if (autoSlaughterConfig.maxTotal != -1)
								{
									while (AutoSlaughterManager.tmpAnimals.Count > autoSlaughterConfig.maxTotal)
									{
										Pawn pawn2 = AutoSlaughterManager.tmpAnimals.Pop<Pawn>();
										if (pawn2.gender == Gender.Male)
										{
											if (pawn2.ageTracker.CurLifeStage.reproductive)
											{
												AutoSlaughterManager.tmpAnimalsMale.Remove(pawn2);
											}
											else
											{
												AutoSlaughterManager.tmpAnimalsMaleYoung.Remove(pawn2);
											}
										}
										else if (pawn2.gender == Gender.Female)
										{
											if (pawn2.ageTracker.CurLifeStage.reproductive)
											{
												AutoSlaughterManager.tmpAnimalsFemale.Remove(pawn2);
											}
											else
											{
												AutoSlaughterManager.tmpAnimalsFemaleYoung.Remove(pawn2);
											}
										}
										this.animalsToSlaughterCached.Add(pawn2);
									}
								}
							}
						}
						this.cacheDirty = false;
					}
					finally
					{
						AutoSlaughterManager.tmpAnimals.Clear();
						AutoSlaughterManager.tmpAnimalsMale.Clear();
						AutoSlaughterManager.tmpAnimalsFemale.Clear();
					}
				}
				return this.animalsToSlaughterCached;
			}
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00044F24 File Offset: 0x00043124
		public static bool CanEverAutoSlaughter(Pawn animal)
		{
			return animal.HomeFaction == Faction.OfPlayer && !animal.RaceProps.Dryad;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00044F43 File Offset: 0x00043143
		public static bool CanAutoSlaughterNow(Pawn animal)
		{
			return AutoSlaughterManager.CanEverAutoSlaughter(animal) && animal.GetLord() == null && (animal.inventory == null || !animal.inventory.UnloadEverything);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00044F71 File Offset: 0x00043171
		public AutoSlaughterManager(Map map)
		{
			this.map = map;
			this.TryPopulateMissingAnimals();
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00044F9C File Offset: 0x0004319C
		public void Notify_PawnDespawned()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00044F9C File Offset: 0x0004319C
		public void Notify_PawnSpawned()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00044F9C File Offset: 0x0004319C
		public void Notify_PawnChangedFaction()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00044F9C File Offset: 0x0004319C
		public void Notify_ConfigChanged()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00044FA8 File Offset: 0x000431A8
		public void ExposeData()
		{
			Scribe_Collections.Look<AutoSlaughterConfig>(ref this.configs, "configs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.configs.RemoveAll((AutoSlaughterConfig x) => x.animal == null) != 0)
				{
					Log.Warning("Some auto-slaughter configs had null animals after loading.");
				}
				this.TryPopulateMissingAnimals();
			}
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00045010 File Offset: 0x00043210
		private void TryPopulateMissingAnimals()
		{
			HashSet<ThingDef> hashSet = new HashSet<ThingDef>();
			hashSet.AddRange(from c in this.configs
			select c.animal);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.race != null && thingDef.race.Animal && thingDef.race.wildness < 1f && !thingDef.race.Dryad && !hashSet.Contains(thingDef))
				{
					this.configs.Add(new AutoSlaughterConfig
					{
						animal = thingDef
					});
				}
			}
		}

		// Token: 0x04000B4F RID: 2895
		private static List<Pawn> tmpAnimals = new List<Pawn>();

		// Token: 0x04000B50 RID: 2896
		private static List<Pawn> tmpAnimalsMale = new List<Pawn>();

		// Token: 0x04000B51 RID: 2897
		private static List<Pawn> tmpAnimalsMaleYoung = new List<Pawn>();

		// Token: 0x04000B52 RID: 2898
		private static List<Pawn> tmpAnimalsFemale = new List<Pawn>();

		// Token: 0x04000B53 RID: 2899
		private static List<Pawn> tmpAnimalsFemaleYoung = new List<Pawn>();

		// Token: 0x04000B54 RID: 2900
		private static List<Pawn> tmpAnimalsPregnant = new List<Pawn>();

		// Token: 0x04000B55 RID: 2901
		public Map map;

		// Token: 0x04000B56 RID: 2902
		public List<AutoSlaughterConfig> configs = new List<AutoSlaughterConfig>();

		// Token: 0x04000B57 RID: 2903
		private List<Pawn> animalsToSlaughterCached = new List<Pawn>();

		// Token: 0x04000B58 RID: 2904
		private bool cacheDirty;
	}
}

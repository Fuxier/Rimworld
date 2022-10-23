using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000167 RID: 359
	public class GameInitData
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0002F348 File Offset: 0x0002D548
		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002F362 File Offset: 0x0002D562
		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002F36F File Offset: 0x0002D56F
		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = -1;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0002F395 File Offset: 0x0002D595
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"startedFromEntry: ",
				this.startedFromEntry.ToString(),
				"\nstartingAndOptionalPawns: ",
				this.startingAndOptionalPawns.Count
			});
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0002F3D4 File Offset: 0x0002D5D4
		public void PrepForMapGen()
		{
			while (this.startingAndOptionalPawns.Count > this.startingPawnCount)
			{
				PawnComponentsUtility.RemoveComponentsOnDespawned(this.startingAndOptionalPawns[this.startingPawnCount]);
				Find.WorldPawns.PassToWorld(this.startingAndOptionalPawns[this.startingPawnCount], PawnDiscardDecideMode.KeepForever);
				Pawn pawn = this.startingAndOptionalPawns[this.startingPawnCount];
				this.startingAndOptionalPawns.Remove(pawn);
				this.startingPossessions.Remove(pawn);
			}
			List<Pawn> list = this.startingAndOptionalPawns;
			foreach (Pawn pawn2 in list)
			{
				pawn2.SetFactionDirect(Faction.OfPlayer);
				PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn2, false);
			}
			foreach (Pawn pawn3 in list)
			{
				pawn3.workSettings.DisableAll();
			}
			using (IEnumerator<WorkTypeDef> enumerator2 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					WorkTypeDef w = enumerator2.Current;
					if (w.alwaysStartActive)
					{
						IEnumerable<Pawn> source = list;
						Func<Pawn, bool> predicate;
						Func<Pawn, bool> <>9__0;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((Pawn col) => !col.WorkTypeIsDisabled(w)));
						}
						using (IEnumerator<Pawn> enumerator3 = source.Where(predicate).GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Pawn pawn4 = enumerator3.Current;
								pawn4.workSettings.SetPriority(w, 3);
							}
							continue;
						}
					}
					bool flag = false;
					foreach (Pawn pawn5 in list)
					{
						if (!pawn5.WorkTypeIsDisabled(w) && pawn5.skills.AverageOfRelevantSkillsFor(w) >= 6f)
						{
							pawn5.workSettings.SetPriority(w, 3);
							flag = true;
						}
					}
					if (!flag)
					{
						IEnumerable<Pawn> source2 = from col in list
						where !col.WorkTypeIsDisabled(w)
						select col;
						if (source2.Any<Pawn>())
						{
							source2.InRandomOrder(null).MaxBy((Pawn c) => c.skills.AverageOfRelevantSkillsFor(w)).workSettings.SetPriority(w, 3);
						}
					}
				}
			}
		}

		// Token: 0x040009E8 RID: 2536
		public int startingTile = -1;

		// Token: 0x040009E9 RID: 2537
		public int mapSize = 250;

		// Token: 0x040009EA RID: 2538
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x040009EB RID: 2539
		public Dictionary<Pawn, List<ThingDefCount>> startingPossessions = new Dictionary<Pawn, List<ThingDefCount>>();

		// Token: 0x040009EC RID: 2540
		public int startingPawnCount = -1;

		// Token: 0x040009ED RID: 2541
		public Faction playerFaction;

		// Token: 0x040009EE RID: 2542
		public Season startingSeason;

		// Token: 0x040009EF RID: 2543
		public bool permadeathChosen;

		// Token: 0x040009F0 RID: 2544
		public bool permadeath;

		// Token: 0x040009F1 RID: 2545
		public PawnKindDef startingPawnKind;

		// Token: 0x040009F2 RID: 2546
		public List<PawnKindCount> startingPawnsRequired;

		// Token: 0x040009F3 RID: 2547
		public List<XenotypeCount> startingXenotypesRequired;

		// Token: 0x040009F4 RID: 2548
		public bool startedFromEntry;

		// Token: 0x040009F5 RID: 2549
		public string gameToLoad;

		// Token: 0x040009F6 RID: 2550
		public const int DefaultMapSize = 250;
	}
}

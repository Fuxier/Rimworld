using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000156 RID: 342
	public class Battle : IExposable, ILoadReferenceable
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0002B860 File Offset: 0x00029A60
		public int Importance
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0002B86D File Offset: 0x00029A6D
		public int CreationTimestamp
		{
			get
			{
				return this.creationTimestamp;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0002B875 File Offset: 0x00029A75
		public int LastEntryTimestamp
		{
			get
			{
				if (this.entries.Count <= 0)
				{
					return 0;
				}
				return this.entries[this.entries.Count - 1].Timestamp;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0002B8A4 File Offset: 0x00029AA4
		public Battle AbsorbedBy
		{
			get
			{
				return this.absorbedBy;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0002B8AC File Offset: 0x00029AAC
		public List<LogEntry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002B8D2 File Offset: 0x00029AD2
		public static Battle Create()
		{
			return new Battle
			{
				loadID = Find.UniqueIDsManager.GetNextBattleID(),
				creationTimestamp = Find.TickManager.TicksGame
			};
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0002B8FC File Offset: 0x00029AFC
		public string GetName()
		{
			if (this.battleName.NullOrEmpty())
			{
				HashSet<Faction> hashSet = new HashSet<Faction>(from p in this.concerns
				select p.Faction);
				GrammarRequest request = default(GrammarRequest);
				if (this.concerns.Count == 1)
				{
					if (hashSet.Count((Faction f) => f != null) < 2)
					{
						request.Includes.Add(RulePackDefOf.Battle_Solo);
						request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
						goto IL_1D9;
					}
				}
				if (this.concerns.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_Duel);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT2", this.concerns.Last<Pawn>(), null, true, true));
				}
				else if (hashSet.Count == 1)
				{
					request.Includes.Add(RulePackDefOf.Battle_Internal);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), request.Constants, true));
				}
				else if (hashSet.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_War);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), request.Constants, true));
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION2", hashSet.Last<Faction>(), request.Constants, true));
				}
				else
				{
					request.Includes.Add(RulePackDefOf.Battle_Brawl);
				}
				IL_1D9:
				this.battleName = GrammarResolver.Resolve("r_battlename", request, null, false, null, null, null, true);
			}
			return this.battleName;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002BB00 File Offset: 0x00029D00
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			foreach (Thing thing in entry.GetConcerns())
			{
				if (thing is Pawn)
				{
					this.concerns.Add(thing as Pawn);
				}
			}
			this.battleName = null;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002BB74 File Offset: 0x00029D74
		public void Absorb(Battle battle)
		{
			this.creationTimestamp = Mathf.Min(this.creationTimestamp, battle.creationTimestamp);
			this.entries.AddRange(battle.entries);
			this.concerns.AddRange(battle.concerns);
			this.entries = (from e in this.entries
			orderby e.Age
			select e).ToList<LogEntry>();
			battle.entries.Clear();
			battle.concerns.Clear();
			battle.absorbedBy = this;
			this.battleName = null;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0002BC13 File Offset: 0x00029E13
		public bool Concerns(Pawn pawn)
		{
			return this.concerns.Contains(pawn);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002BC24 File Offset: 0x00029E24
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			if (!this.concerns.Contains(p))
			{
				return;
			}
			for (int i = this.entries.Count - 1; i >= 0; i--)
			{
				if (this.entries[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Discarding pawn ",
							p,
							", but he is referenced by a battle log entry ",
							this.entries[i],
							"."
						}));
					}
					this.entries.RemoveAt(i);
				}
			}
			this.concerns.Remove(p);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0002BCC4 File Offset: 0x00029EC4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.creationTimestamp, "creationTimestamp", 0, false);
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Battle>(ref this.absorbedBy, "absorbedBy", false);
			Scribe_Values.Look<string>(ref this.battleName, "battleName", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.concerns.Clear();
				foreach (Pawn item in this.entries.SelectMany((LogEntry e) => e.GetConcerns()).OfType<Pawn>())
				{
					this.concerns.Add(item);
				}
			}
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0002BDB0 File Offset: 0x00029FB0
		public string GetUniqueLoadID()
		{
			return "Battle_" + this.loadID;
		}

		// Token: 0x04000982 RID: 2434
		public const int TicksForBattleExit = 5000;

		// Token: 0x04000983 RID: 2435
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04000984 RID: 2436
		private string battleName;

		// Token: 0x04000985 RID: 2437
		private Battle absorbedBy;

		// Token: 0x04000986 RID: 2438
		private HashSet<Pawn> concerns = new HashSet<Pawn>();

		// Token: 0x04000987 RID: 2439
		private int loadID;

		// Token: 0x04000988 RID: 2440
		private int creationTimestamp;
	}
}

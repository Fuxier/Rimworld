using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000157 RID: 343
	public class BattleLog : IExposable
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0002BDC7 File Offset: 0x00029FC7
		public List<Battle> Battles
		{
			get
			{
				return this.battles;
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002BDD0 File Offset: 0x00029FD0
		public void Add(LogEntry entry)
		{
			Battle battle = null;
			foreach (Thing thing in entry.GetConcerns())
			{
				Battle battleActive = ((Pawn)thing).records.BattleActive;
				if (battle == null)
				{
					battle = battleActive;
				}
				else if (battleActive != null)
				{
					battle = ((battle.Importance > battleActive.Importance) ? battle : battleActive);
				}
			}
			if (battle == null)
			{
				battle = Battle.Create();
				this.battles.Insert(0, battle);
			}
			foreach (Thing thing2 in entry.GetConcerns())
			{
				Pawn pawn = (Pawn)thing2;
				Battle battleActive2 = pawn.records.BattleActive;
				if (battleActive2 != null && battleActive2 != battle)
				{
					battle.Absorb(battleActive2);
					this.battles.Remove(battleActive2);
				}
				pawn.records.EnterBattle(battle);
			}
			battle.Add(entry);
			this.cachedActiveEntries = null;
			this.ReduceToCapacity();
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002BEDC File Offset: 0x0002A0DC
		private void ReduceToCapacity()
		{
			int num = this.battles.Count((Battle btl) => btl.AbsorbedBy == null);
			while (num > 20 && this.battles[this.battles.Count - 1].LastEntryTimestamp + Mathf.Max(420000, 5000) < Find.TickManager.TicksGame)
			{
				if (this.battles[this.battles.Count - 1].AbsorbedBy == null)
				{
					num--;
				}
				this.battles.RemoveAt(this.battles.Count - 1);
				this.cachedActiveEntries = null;
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0002BF96 File Offset: 0x0002A196
		public void ExposeData()
		{
			Scribe_Collections.Look<Battle>(ref this.battles, "battles", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.battles == null)
			{
				this.battles = new List<Battle>();
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002BFCC File Offset: 0x0002A1CC
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.battles.Count; i++)
			{
				if (this.battles[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002C008 File Offset: 0x0002A208
		public bool IsEntryActive(LogEntry log)
		{
			if (this.cachedActiveEntries == null)
			{
				this.cachedActiveEntries = new HashSet<LogEntry>();
				for (int i = 0; i < this.battles.Count; i++)
				{
					List<LogEntry> entries = this.battles[i].Entries;
					for (int j = 0; j < entries.Count; j++)
					{
						this.cachedActiveEntries.Add(entries[j]);
					}
				}
			}
			return this.cachedActiveEntries.Contains(log);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002C080 File Offset: 0x0002A280
		public void RemoveEntry(LogEntry log)
		{
			int num = 0;
			while (num < this.battles.Count && !this.battles[num].Entries.Remove(log))
			{
				num++;
			}
			this.cachedActiveEntries = null;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0002C0C4 File Offset: 0x0002A2C4
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.battles.Count - 1; i >= 0; i--)
			{
				this.battles[i].Notify_PawnDiscarded(p, silentlyRemoveReferences);
			}
			this.cachedActiveEntries = null;
		}

		// Token: 0x04000989 RID: 2441
		private List<Battle> battles = new List<Battle>();

		// Token: 0x0400098A RID: 2442
		private const int BattleHistoryLength = 20;

		// Token: 0x0400098B RID: 2443
		private HashSet<LogEntry> cachedActiveEntries;
	}
}

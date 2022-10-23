using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200016A RID: 362
	public class PlayLog : IExposable
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0002FAE5 File Offset: 0x0002DCE5
		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0002FAED File Offset: 0x0002DCED
		public int LastTick
		{
			get
			{
				if (this.entries.Count == 0)
				{
					return 0;
				}
				return this.entries[0].Tick;
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0002FB0F File Offset: 0x0002DD0F
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0002FB24 File Offset: 0x0002DD24
		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0002FB58 File Offset: 0x0002DD58
		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0002FB70 File Offset: 0x0002DD70
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
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
							", but he is referenced by a play log entry ",
							this.entries[i],
							"."
						}));
					}
					this.RemoveEntry(this.entries[i]);
				}
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0002FBFC File Offset: 0x0002DDFC
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0002FC34 File Offset: 0x0002DE34
		public void Notify_IdeoRemoved(Ideo ideo)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].Notify_IdeoRemoved(ideo);
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0002FC69 File Offset: 0x0002DE69
		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0002FC78 File Offset: 0x0002DE78
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000A07 RID: 2567
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04000A08 RID: 2568
		private const int Capacity = 150;
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003F2 RID: 1010
	public class RealtimeMoteList
	{
		// Token: 0x06001CB7 RID: 7351 RVA: 0x000AE878 File Offset: 0x000ACA78
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000AE885 File Offset: 0x000ACA85
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000AE893 File Offset: 0x000ACA93
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000AE8A4 File Offset: 0x000ACAA4
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}

		// Token: 0x04001460 RID: 5216
		public List<Mote> allMotes = new List<Mote>();
	}
}

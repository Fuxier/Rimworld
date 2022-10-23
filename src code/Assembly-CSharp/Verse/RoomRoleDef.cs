using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200012E RID: 302
	public class RoomRoleDef : Def
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00028223 File Offset: 0x00026423
		public RoomRoleWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomRoleWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00028249 File Offset: 0x00026449
		public string PostProcessedLabel
		{
			get
			{
				return this.Worker.PostProcessedLabel(this.label);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002825C File Offset: 0x0002645C
		public string PostProcessedLabelCap
		{
			get
			{
				return this.PostProcessedLabel.CapitalizeFirst();
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0002826C File Offset: 0x0002646C
		public bool IsStatRelated(RoomStatDef def)
		{
			if (this.relatedStats == null)
			{
				return false;
			}
			for (int i = 0; i < this.relatedStats.Count; i++)
			{
				if (this.relatedStats[i] == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040007E9 RID: 2025
		public Type workerClass;

		// Token: 0x040007EA RID: 2026
		private List<RoomStatDef> relatedStats;

		// Token: 0x040007EB RID: 2027
		[Unsaved(false)]
		private RoomRoleWorker workerInt;
	}
}

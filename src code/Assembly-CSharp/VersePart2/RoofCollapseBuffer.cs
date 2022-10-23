using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000250 RID: 592
	public class RoofCollapseBuffer
	{
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00063038 File Offset: 0x00061238
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00063040 File Offset: 0x00061240
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0006304E File Offset: 0x0006124E
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0006306A File Offset: 0x0006126A
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		// Token: 0x04000EB4 RID: 3764
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}

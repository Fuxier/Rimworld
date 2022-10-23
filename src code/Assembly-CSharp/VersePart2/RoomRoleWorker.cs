using System;

namespace Verse
{
	// Token: 0x0200012F RID: 303
	public abstract class RoomRoleWorker
	{
		// Token: 0x060007E0 RID: 2016 RVA: 0x000282AB File Offset: 0x000264AB
		public virtual string PostProcessedLabel(string baseLabel)
		{
			return baseLabel;
		}

		// Token: 0x060007E1 RID: 2017
		public abstract float GetScore(Room room);
	}
}

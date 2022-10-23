using System;

namespace Verse
{
	// Token: 0x02000231 RID: 561
	public class PostTickVisuals
	{
		// Token: 0x06000FD1 RID: 4049 RVA: 0x0005C155 File Offset: 0x0005A355
		public PostTickVisuals(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0005C164 File Offset: 0x0005A364
		public void ProcessPostTickVisuals()
		{
			int ticksThisFrame = Find.TickManager.TicksThisFrame;
			if (ticksThisFrame > 0)
			{
				CellRect viewRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(3);
				foreach (Pawn pawn in this.map.mapPawns.AllPawnsSpawned)
				{
					pawn.ProcessPostTickVisuals(ticksThisFrame, viewRect);
				}
			}
		}

		// Token: 0x04000E10 RID: 3600
		private Map map;
	}
}

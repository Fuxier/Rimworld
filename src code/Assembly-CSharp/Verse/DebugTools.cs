using System;

namespace Verse
{
	// Token: 0x02000451 RID: 1105
	public static class DebugTools
	{
		// Token: 0x06002218 RID: 8728 RVA: 0x000D9683 File Offset: 0x000D7883
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
			if (DebugTools.curMeasureTool != null)
			{
				DebugTools.curMeasureTool.DebugToolOnGUI();
			}
		}

		// Token: 0x040015B1 RID: 5553
		public static DebugTool curTool;

		// Token: 0x040015B2 RID: 5554
		public static DrawMeasureTool curMeasureTool;
	}
}

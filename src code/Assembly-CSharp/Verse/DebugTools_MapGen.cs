using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200045D RID: 1117
	public static class DebugTools_MapGen
	{
		// Token: 0x06002259 RID: 8793 RVA: 0x000DB510 File Offset: 0x000D9710
		public static List<DebugActionNode> Options_Scatterers()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Type localSt2 in typeof(GenStep_Scatterer).AllLeafSubclasses())
			{
				Type localSt = localSt2;
				list.Add(new DebugActionNode(localSt.ToString(), DebugActionType.ToolMap, null, null)
				{
					action = delegate()
					{
						((GenStep_Scatterer)Activator.CreateInstance(localSt)).ForceScatterAt(UI.MouseCell(), Find.CurrentMap);
					}
				});
			}
			return list;
		}
	}
}

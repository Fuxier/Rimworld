using System;

namespace Verse
{
	// Token: 0x02000461 RID: 1121
	public struct DebugMenuOption
	{
		// Token: 0x06002274 RID: 8820 RVA: 0x000DC010 File Offset: 0x000DA210
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}

		// Token: 0x040015E5 RID: 5605
		public DebugMenuOptionMode mode;

		// Token: 0x040015E6 RID: 5606
		public string label;

		// Token: 0x040015E7 RID: 5607
		public Action method;
	}
}

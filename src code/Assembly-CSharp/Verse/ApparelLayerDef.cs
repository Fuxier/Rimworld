using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000A8 RID: 168
	public class ApparelLayerDef : Def
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001F5C0 File Offset: 0x0001D7C0
		public bool IsUtilityLayer
		{
			get
			{
				return this == ApparelLayerDefOf.Belt;
			}
		}

		// Token: 0x040002AD RID: 685
		public int drawOrder;
	}
}

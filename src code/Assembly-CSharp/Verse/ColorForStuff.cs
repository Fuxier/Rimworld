using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B1 RID: 177
	public class ColorForStuff
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001FD6F File Offset: 0x0001DF6F
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0001FD77 File Offset: 0x0001DF77
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x040002E8 RID: 744
		private ThingDef stuff;

		// Token: 0x040002E9 RID: 745
		private Color color = Color.white;
	}
}

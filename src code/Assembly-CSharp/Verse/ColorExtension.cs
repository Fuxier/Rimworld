using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000516 RID: 1302
	public static class ColorExtension
	{
		// Token: 0x060027C2 RID: 10178 RVA: 0x001025C8 File Offset: 0x001007C8
		public static Color ToOpaque(this Color c)
		{
			c.a = 1f;
			return c;
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x001025D7 File Offset: 0x001007D7
		public static Color ToTransparent(this Color c, float transparency)
		{
			c.a = transparency;
			return c;
		}
	}
}

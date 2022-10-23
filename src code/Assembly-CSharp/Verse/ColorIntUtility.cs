using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001F RID: 31
	public static class ColorIntUtility
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00007E7A File Offset: 0x0000607A
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}

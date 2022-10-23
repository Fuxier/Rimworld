using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DD RID: 1245
	public static class OptionListingUtility
	{
		// Token: 0x0600259D RID: 9629 RVA: 0x000EEFB0 File Offset: 0x000ED1B0
		public static float DrawOptionListing(Rect rect, List<ListableOption> optList)
		{
			float num = 0f;
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			foreach (ListableOption listableOption in optList)
			{
				num += listableOption.DrawOption(new Vector2(0f, num), rect.width) + 7f;
			}
			Widgets.EndGroup();
			return num;
		}

		// Token: 0x04001815 RID: 6165
		private const float OptionSpacing = 7f;
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200048B RID: 1163
	public static class BeautyDrawer
	{
		// Token: 0x06002333 RID: 9011 RVA: 0x000E1008 File Offset: 0x000DF208
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !BeautyDrawer.ShouldShow())
			{
				return;
			}
			BeautyDrawer.DrawBeautyAroundMouse();
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x000E1024 File Offset: 0x000DF224
		private static bool ShouldShow()
		{
			return !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && (CellInspectorDrawer.active || Find.PlaySettings.showBeauty);
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x000E1078 File Offset: 0x000DF278
		private static void DrawBeautyAroundMouse()
		{
			BeautyUtility.FillBeautyRelevantCells(UI.MouseCell(), Find.CurrentMap);
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
			{
				IntVec3 intVec = BeautyUtility.beautyRelevantCells[i];
				float num = BeautyUtility.CellBeauty(intVec, Find.CurrentMap, BeautyDrawer.beautyCountedThings);
				if (num != 0f)
				{
					GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(intVec), Mathf.RoundToInt(num).ToStringCached(), BeautyDrawer.BeautyColor(num, 8f));
				}
			}
			BeautyDrawer.beautyCountedThings.Clear();
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x000E1104 File Offset: 0x000DF304
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			return Color.Lerp(Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num), Color.white, 0.5f);
		}

		// Token: 0x04001690 RID: 5776
		private static List<Thing> beautyCountedThings = new List<Thing>();

		// Token: 0x04001691 RID: 5777
		private static Color ColorUgly = Color.red;

		// Token: 0x04001692 RID: 5778
		private static Color ColorBeautiful = Color.green;
	}
}

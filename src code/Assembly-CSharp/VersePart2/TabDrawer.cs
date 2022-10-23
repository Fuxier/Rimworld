using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004E4 RID: 1252
	public static class TabDrawer
	{
		// Token: 0x060025BB RID: 9659 RVA: 0x000EFF14 File Offset: 0x000EE114
		public static TabRecord DrawTabs(Rect baseRect, List<TabRecord> tabs, int rows)
		{
			if (rows <= 1)
			{
				return TabDrawer.DrawTabs<TabRecord>(baseRect, tabs, 200f);
			}
			int num = Mathf.FloorToInt((float)(tabs.Count / rows));
			int num2 = 0;
			TabRecord result = null;
			Rect rect = baseRect;
			baseRect.yMin -= (float)(rows - 1) * 31f;
			Rect rect2 = baseRect;
			rect2.yMax = rect.y;
			Widgets.DrawMenuSection(rect2);
			for (int i = 0; i < rows; i++)
			{
				int num3 = (i == 0) ? (tabs.Count - (rows - 1) * num) : num;
				TabDrawer.tmpTabs.Clear();
				for (int j = num2; j < num2 + num3; j++)
				{
					TabDrawer.tmpTabs.Add(tabs[j]);
				}
				TabRecord tabRecord = TabDrawer.DrawTabs<TabRecord>(baseRect, TabDrawer.tmpTabs, baseRect.width);
				if (tabRecord != null)
				{
					result = tabRecord;
				}
				baseRect.yMin += 31f;
				num2 += num3;
			}
			TabDrawer.tmpTabs.Clear();
			return result;
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000F000C File Offset: 0x000EE20C
		public static TTabRecord DrawTabs<TTabRecord>(Rect baseRect, List<TTabRecord> tabs, float maxTabWidth = 200f) where TTabRecord : TabRecord
		{
			TTabRecord ttabRecord = default(TTabRecord);
			TTabRecord ttabRecord2 = tabs.Find((TTabRecord t) => t.Selected);
			float num = baseRect.width + (float)(tabs.Count - 1) * 10f;
			float tabWidth = num / (float)tabs.Count;
			if (tabWidth > maxTabWidth)
			{
				tabWidth = maxTabWidth;
			}
			Rect rect = new Rect(baseRect);
			rect.y -= 32f;
			rect.height = 9999f;
			Widgets.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Small;
			Func<TTabRecord, Rect> func = (TTabRecord tab) => new Rect((float)tabs.IndexOf(tab) * (tabWidth - 10f), 1f, tabWidth, 32f);
			List<TTabRecord> list = tabs.ListFullCopy<TTabRecord>();
			if (ttabRecord2 != null)
			{
				list.Remove(ttabRecord2);
				list.Add(ttabRecord2);
			}
			TabRecord tabRecord = null;
			List<TTabRecord> list2 = list.ListFullCopy<TTabRecord>();
			list2.Reverse();
			for (int i = 0; i < list2.Count; i++)
			{
				TTabRecord ttabRecord3 = list2[i];
				Rect rect2 = func(ttabRecord3);
				if (tabRecord == null && Mouse.IsOver(rect2))
				{
					tabRecord = ttabRecord3;
				}
				MouseoverSounds.DoRegion(rect2, SoundDefOf.Mouseover_Tab);
				if (Widgets.ButtonInvisible(rect2, true))
				{
					ttabRecord = ttabRecord3;
				}
			}
			foreach (TTabRecord ttabRecord4 in list)
			{
				Rect rect3 = func(ttabRecord4);
				ttabRecord4.Draw(rect3);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.EndGroup();
			if (ttabRecord != null && ttabRecord != ttabRecord2)
			{
				SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
				if (ttabRecord.clickedAction != null)
				{
					ttabRecord.clickedAction();
				}
			}
			return ttabRecord;
		}

		// Token: 0x04001835 RID: 6197
		private const float MaxTabWidth = 200f;

		// Token: 0x04001836 RID: 6198
		public const float TabHeight = 32f;

		// Token: 0x04001837 RID: 6199
		public const float TabHoriztonalOverlap = 10f;

		// Token: 0x04001838 RID: 6200
		private static List<TabRecord> tmpTabs = new List<TabRecord>();
	}
}

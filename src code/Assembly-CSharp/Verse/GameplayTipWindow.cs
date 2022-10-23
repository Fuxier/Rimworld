using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000048 RID: 72
	public class GameplayTipWindow
	{
		// Token: 0x060003AD RID: 941 RVA: 0x000145B4 File Offset: 0x000127B4
		public static void DrawWindow(Vector2 offset, bool useWindowStack)
		{
			if (GameplayTipWindow.allTipsCached == null)
			{
				GameplayTipWindow.allTipsCached = DefDatabase<TipSetDef>.AllDefsListForReading.SelectMany(delegate(TipSetDef set)
				{
					if (SteamDeck.IsSteamDeck && set == TipSetDefOf.GameplayTips)
					{
						return set.tips.Skip(11);
					}
					return set.tips;
				}).InRandomOrder(null).ToList<string>();
			}
			Rect rect = new Rect(offset.x, offset.y, GameplayTipWindow.WindowSize.x, GameplayTipWindow.WindowSize.y);
			if (useWindowStack)
			{
				Find.WindowStack.ImmediateWindow(62893997, rect, WindowLayer.Super, delegate
				{
					GameplayTipWindow.DrawContents(rect.AtZero());
				}, true, false, 1f, null);
				return;
			}
			Widgets.DrawShadowAround(rect);
			Widgets.DrawWindowBackground(rect);
			GameplayTipWindow.DrawContents(rect);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00014684 File Offset: 0x00012884
		private static void DrawContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			if (Time.realtimeSinceStartup - GameplayTipWindow.lastTimeUpdatedTooltip > 17.5f || GameplayTipWindow.lastTimeUpdatedTooltip < 0f)
			{
				GameplayTipWindow.currentTipIndex = (GameplayTipWindow.currentTipIndex + 1) % GameplayTipWindow.allTipsCached.Count;
				GameplayTipWindow.lastTimeUpdatedTooltip = Time.realtimeSinceStartup;
			}
			Rect rect2 = rect;
			rect2.x += GameplayTipWindow.TextMargin.x;
			rect2.width -= GameplayTipWindow.TextMargin.x * 2f;
			rect2.y += GameplayTipWindow.TextMargin.y;
			rect2.height -= GameplayTipWindow.TextMargin.y * 2f;
			Widgets.Label(rect2, GameplayTipWindow.allTipsCached[GameplayTipWindow.currentTipIndex]);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00014765 File Offset: 0x00012965
		public static void ResetTipTimer()
		{
			GameplayTipWindow.lastTimeUpdatedTooltip = -1f;
		}

		// Token: 0x040000F4 RID: 244
		private static List<string> allTipsCached;

		// Token: 0x040000F5 RID: 245
		private static float lastTimeUpdatedTooltip = -1f;

		// Token: 0x040000F6 RID: 246
		private static int currentTipIndex = 0;

		// Token: 0x040000F7 RID: 247
		public const float tipUpdateInterval = 17.5f;

		// Token: 0x040000F8 RID: 248
		public static readonly Vector2 WindowSize = new Vector2(776f, 60f);

		// Token: 0x040000F9 RID: 249
		private static readonly Vector2 TextMargin = new Vector2(15f, 8f);

		// Token: 0x040000FA RID: 250
		private const int InterfaceTipsCount = 11;
	}
}

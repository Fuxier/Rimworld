using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200051A RID: 1306
	[StaticConstructorOnStartup]
	public static class ContentSourceUtility
	{
		// Token: 0x060027C7 RID: 10183 RVA: 0x00102C2C File Offset: 0x00100E2C
		public static Texture2D GetIcon(this ContentSource s)
		{
			switch (s)
			{
			case ContentSource.Undefined:
				return BaseContent.BadTex;
			case ContentSource.OfficialModsFolder:
				return ContentSourceUtility.ContentSourceIcon_OfficialModsFolder;
			case ContentSource.ModsFolder:
				return ContentSourceUtility.ContentSourceIcon_ModsFolder;
			case ContentSource.SteamWorkshop:
				return ContentSourceUtility.ContentSourceIcon_SteamWorkshop;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x00102C64 File Offset: 0x00100E64
		public static void DrawContentSource(Rect r, ContentSource source, Action clickAction = null)
		{
			Rect rect = new Rect(r.x, r.y + r.height / 2f - 12f, 24f, 24f);
			GUI.DrawTexture(rect, source.GetIcon());
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => "Source".Translate() + ": " + source.HumanLabel(), (int)(r.x + r.y * 56161f));
				Widgets.DrawHighlight(rect);
			}
			if (clickAction != null && Widgets.ButtonInvisible(rect, true))
			{
				clickAction();
			}
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00102D0A File Offset: 0x00100F0A
		public static string HumanLabel(this ContentSource s)
		{
			return ("ContentSource_" + s.ToString()).Translate();
		}

		// Token: 0x04001A5A RID: 6746
		public const float IconSize = 24f;

		// Token: 0x04001A5B RID: 6747
		private static readonly Texture2D ContentSourceIcon_OfficialModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/OfficialModsFolder", true);

		// Token: 0x04001A5C RID: 6748
		private static readonly Texture2D ContentSourceIcon_ModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/ModsFolder", true);

		// Token: 0x04001A5D RID: 6749
		private static readonly Texture2D ContentSourceIcon_SteamWorkshop = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/SteamWorkshop", true);
	}
}

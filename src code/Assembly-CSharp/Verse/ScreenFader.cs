using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000598 RID: 1432
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x0011522C File Offset: 0x0011342C
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x00115234 File Offset: 0x00113434
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x001152D8 File Offset: 0x001134D8
		public static void OverlayOnGUI(Vector2 windowSize)
		{
			Color color = ScreenFader.CurrentInstantColor();
			if (color.a > 0f)
			{
				if (ScreenFader.fadeTextureDirty)
				{
					ScreenFader.fadeTexture.SetPixel(0, 0, color);
					ScreenFader.fadeTexture.Apply();
				}
				GUI.Label(new Rect(-10f, -10f, windowSize.x + 10f, windowSize.y + 10f), ScreenFader.fadeTexture, ScreenFader.backgroundStyle);
			}
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x0011534C File Offset: 0x0011354C
		private static Color CurrentInstantColor()
		{
			if (ScreenFader.CurTime > ScreenFader.targetTime || ScreenFader.targetTime == ScreenFader.sourceTime)
			{
				return ScreenFader.targetColor;
			}
			return Color.Lerp(ScreenFader.sourceColor, ScreenFader.targetColor, (ScreenFader.CurTime - ScreenFader.sourceTime) / (ScreenFader.targetTime - ScreenFader.sourceTime));
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x0011539D File Offset: 0x0011359D
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x001153C5 File Offset: 0x001135C5
		public static void StartFade(Color finalColor, float duration)
		{
			if (duration <= 0f)
			{
				ScreenFader.SetColor(finalColor);
				return;
			}
			ScreenFader.sourceColor = ScreenFader.CurrentInstantColor();
			ScreenFader.targetColor = finalColor;
			ScreenFader.sourceTime = ScreenFader.CurTime;
			ScreenFader.targetTime = ScreenFader.CurTime + duration;
		}

		// Token: 0x04001CA6 RID: 7334
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x04001CA7 RID: 7335
		private static Texture2D fadeTexture;

		// Token: 0x04001CA8 RID: 7336
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04001CA9 RID: 7337
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04001CAA RID: 7338
		private static float sourceTime = 0f;

		// Token: 0x04001CAB RID: 7339
		private static float targetTime = 0f;

		// Token: 0x04001CAC RID: 7340
		private static bool fadeTextureDirty = true;
	}
}

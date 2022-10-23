using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200058A RID: 1418
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x06002B3A RID: 11066 RVA: 0x001144D8 File Offset: 0x001126D8
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x001144EA File Offset: 0x001126EA
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x04001C5F RID: 7263
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x04001C60 RID: 7264
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);
	}
}

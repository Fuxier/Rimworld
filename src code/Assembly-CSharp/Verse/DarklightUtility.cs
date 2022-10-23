using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B9 RID: 441
	public static class DarklightUtility
	{
		// Token: 0x06000C63 RID: 3171 RVA: 0x00045780 File Offset: 0x00043980
		public static bool IsDarklight(Color color)
		{
			if (color.r > color.g || color.r > color.b)
			{
				return false;
			}
			float num;
			float num2;
			if (color.g > color.b)
			{
				num = color.g;
				num2 = color.b;
			}
			else
			{
				num = color.b;
				num2 = color.g;
			}
			return num != 0f && color.r <= num / 2f && num2 / num > 0.85f;
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000457FF File Offset: 0x000439FF
		public static bool IsDarklightAt(IntVec3 position, Map map)
		{
			return position.InBounds(map) && position.Roofed(map) && map.glowGrid.PsychGlowAt(position) >= PsychGlow.Lit && DarklightUtility.IsDarklight(map.glowGrid.VisualGlowAt(position));
		}

		// Token: 0x04000B5E RID: 2910
		private static FloatRange DarklightHueRange = new FloatRange(0.49f, 0.51f);

		// Token: 0x04000B5F RID: 2911
		public static readonly Color DefaultDarklight = new Color32(78, 226, 229, byte.MaxValue);
	}
}

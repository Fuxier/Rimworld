using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DA RID: 1242
	public static class MouseoverUtility
	{
		// Token: 0x06002592 RID: 9618 RVA: 0x000EED26 File Offset: 0x000ECF26
		static MouseoverUtility()
		{
			MouseoverUtility.MakePermaCache();
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000EED26 File Offset: 0x000ECF26
		public static void Reset()
		{
			MouseoverUtility.MakePermaCache();
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000EED30 File Offset: 0x000ECF30
		private static void MakePermaCache()
		{
			MouseoverUtility.glowStrings = new string[101];
			for (int i = 0; i <= 100; i++)
			{
				MouseoverUtility.glowStrings[i] = GlowGrid.PsychGlowAtGlow((float)i / 100f).GetLabel() + " (" + ((float)i / 100f).ToStringPercent() + ")";
			}
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000EED8B File Offset: 0x000ECF8B
		public static string GetGlowLabelByValue(float value)
		{
			return MouseoverUtility.glowStrings[Mathf.RoundToInt(value * 100f)];
		}

		// Token: 0x0400180D RID: 6157
		private static string[] glowStrings;
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001BE RID: 446
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x06000C75 RID: 3189 RVA: 0x00045BD4 File Offset: 0x00043DD4
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00045CD4 File Offset: 0x00043ED4
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00045D3E File Offset: 0x00043F3E
		public static Material Mat(int ind, bool transparent)
		{
			if (ind >= 100)
			{
				ind = 99;
			}
			if (ind < 0)
			{
				ind = 0;
			}
			if (!transparent)
			{
				return DebugMatsSpectrum.spectrumMatsOpaque[ind];
			}
			return DebugMatsSpectrum.spectrumMatsTranparent[ind];
		}

		// Token: 0x04000B6D RID: 2925
		private static readonly Material[] spectrumMatsTranparent = new Material[100];

		// Token: 0x04000B6E RID: 2926
		private static readonly Material[] spectrumMatsOpaque = new Material[100];

		// Token: 0x04000B6F RID: 2927
		public const int MaterialCount = 100;

		// Token: 0x04000B70 RID: 2928
		public static Color[] DebugSpectrum = new Color[]
		{
			new Color(0.75f, 0f, 0f),
			new Color(0.5f, 0.3f, 0f),
			new Color(0f, 1f, 0f),
			new Color(0f, 0f, 1f),
			new Color(0.7f, 0f, 1f)
		};
	}
}

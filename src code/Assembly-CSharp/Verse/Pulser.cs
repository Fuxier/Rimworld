using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DF RID: 1247
	public static class Pulser
	{
		// Token: 0x060025A1 RID: 9633 RVA: 0x000EF07F File Offset: 0x000ED27F
		public static float PulseBrightness(float frequency, float amplitude)
		{
			return Pulser.PulseBrightness(frequency, amplitude, Time.realtimeSinceStartup);
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x000EF090 File Offset: 0x000ED290
		public static float PulseBrightness(float frequency, float amplitude, float time)
		{
			float num = time * 6.2831855f;
			num *= frequency;
			float t = (1f - Mathf.Cos(num)) * 0.5f;
			return Mathf.Lerp(1f - amplitude, 1f, t);
		}
	}
}

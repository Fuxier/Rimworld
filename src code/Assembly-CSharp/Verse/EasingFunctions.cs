using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000047 RID: 71
	public static class EasingFunctions
	{
		// Token: 0x060003AB RID: 939 RVA: 0x00014522 File Offset: 0x00012722
		public static float EaseInOutQuad(float v)
		{
			if ((double)v >= 0.5)
			{
				return 1f - Mathf.Pow(-2f * v + 2f, 4f) / 2f;
			}
			return 8f * v * v * v * v;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00014564 File Offset: 0x00012764
		public static float EaseInOutQuint(float x)
		{
			if ((double)x >= 0.5)
			{
				return 1f - Mathf.Pow(-2f * x + 2f, 5f) / 2f;
			}
			return 16f * x * x * x * x * x;
		}
	}
}

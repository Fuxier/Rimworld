using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001BF RID: 447
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x06000C78 RID: 3192 RVA: 0x00045D64 File Offset: 0x00043F64
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00045DB0 File Offset: 0x00043FB0
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}

		// Token: 0x04000B71 RID: 2929
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04000B72 RID: 2930
		public const int MaterialCount = 100;

		// Token: 0x04000B73 RID: 2931
		private const float Opacity = 0.25f;
	}
}

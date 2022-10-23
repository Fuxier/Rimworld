using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001A0 RID: 416
	public static class Altitudes
	{
		// Token: 0x06000B76 RID: 2934 RVA: 0x00040EA4 File Offset: 0x0003F0A4
		static Altitudes()
		{
			for (int i = 0; i < 36; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.4054054f;
			}
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00040EF2 File Offset: 0x0003F0F2
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00040EFB File Offset: 0x0003F0FB
		public static float AltitudeFor(this AltitudeLayer alt, float incOffset)
		{
			return alt.AltitudeFor() + incOffset * 0.04054054f;
		}

		// Token: 0x04000ADB RID: 2779
		private const int NumAltitudeLayers = 36;

		// Token: 0x04000ADC RID: 2780
		private static readonly float[] Alts = new float[36];

		// Token: 0x04000ADD RID: 2781
		private const float LayerSpacing = 0.4054054f;

		// Token: 0x04000ADE RID: 2782
		public const float AltInc = 0.04054054f;

		// Token: 0x04000ADF RID: 2783
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.04054054f, 0f);
	}
}

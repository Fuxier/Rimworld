using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000582 RID: 1410
	public static class MatsFromSpectrum
	{
		// Token: 0x06002B21 RID: 11041 RVA: 0x00113C16 File Offset: 0x00111E16
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x00113C24 File Offset: 0x00111E24
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			return SolidColorMaterials.NewSolidColorMaterial(ColorsFromSpectrum.Get(spectrum, val), shader);
		}
	}
}

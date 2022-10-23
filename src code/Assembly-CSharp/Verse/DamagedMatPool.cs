using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A3 RID: 675
	public static class DamagedMatPool
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x000738BD File Offset: 0x00071ABD
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x000738CC File Offset: 0x00071ACC
		public static Material GetDamageFlashMat(Material baseMat, float damPct)
		{
			if (damPct < 0.01f)
			{
				return baseMat;
			}
			Material material;
			if (!DamagedMatPool.damagedMats.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				DamagedMatPool.damagedMats.Add(baseMat, material);
			}
			Color color = Color.Lerp(baseMat.color, DamagedMatPool.DamagedMatStartingColor, damPct);
			material.color = color;
			return material;
		}

		// Token: 0x04000FE5 RID: 4069
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x04000FE6 RID: 4070
		private static readonly Color DamagedMatStartingColor = Color.red;
	}
}

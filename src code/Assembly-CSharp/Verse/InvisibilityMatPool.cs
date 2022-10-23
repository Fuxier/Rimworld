using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002AB RID: 683
	public static class InvisibilityMatPool
	{
		// Token: 0x0600138B RID: 5003 RVA: 0x0007765C File Offset: 0x0007585C
		public static Material GetInvisibleMat(Material baseMat)
		{
			Material material;
			if (!InvisibilityMatPool.materials.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				material.shader = ShaderDatabase.Invisible;
				material.SetTexture(InvisibilityMatPool.NoiseTex, TexGame.InvisDistortion);
				material.color = InvisibilityMatPool.color;
				InvisibilityMatPool.materials.Add(baseMat, material);
			}
			return material;
		}

		// Token: 0x04001041 RID: 4161
		private static Dictionary<Material, Material> materials = new Dictionary<Material, Material>();

		// Token: 0x04001042 RID: 4162
		private static Color color = new Color(0.75f, 0.93f, 0.98f, 0.5f);

		// Token: 0x04001043 RID: 4163
		private static readonly int NoiseTex = Shader.PropertyToID("_NoiseTex");
	}
}

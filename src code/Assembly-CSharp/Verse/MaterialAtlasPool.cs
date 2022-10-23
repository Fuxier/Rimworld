using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038C RID: 908
	public static class MaterialAtlasPool
	{
		// Token: 0x06001A1C RID: 6684 RVA: 0x0009D9EC File Offset: 0x0009BBEC
		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlasPool.MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}

		// Token: 0x04001310 RID: 4880
		private static Dictionary<Material, MaterialAtlasPool.MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlasPool.MaterialAtlas>();

		// Token: 0x02001E68 RID: 7784
		private class MaterialAtlas
		{
			// Token: 0x0600B8F3 RID: 47347 RVA: 0x0041DF2C File Offset: 0x0041C12C
			public MaterialAtlas(Material newRootMat)
			{
				Vector2 mainTextureScale = new Vector2(0.1875f, 0.1875f);
				for (int i = 0; i < 16; i++)
				{
					float x = (float)(i % 4) * 0.25f + 0.03125f;
					float y = (float)(i / 4) * 0.25f + 0.03125f;
					Vector2 mainTextureOffset = new Vector2(x, y);
					Material material = MaterialAllocator.Create(newRootMat);
					material.name = newRootMat.name + "_ASM" + i;
					material.mainTextureScale = mainTextureScale;
					material.mainTextureOffset = mainTextureOffset;
					this.subMats[i] = material;
				}
			}

			// Token: 0x0600B8F4 RID: 47348 RVA: 0x0041DFD8 File Offset: 0x0041C1D8
			public Material SubMat(LinkDirections linkSet)
			{
				if ((int)linkSet >= this.subMats.Length)
				{
					Log.Warning("Cannot get submat of index " + (int)linkSet + ": out of range.");
					return BaseContent.BadMat;
				}
				return this.subMats[(int)linkSet];
			}

			// Token: 0x040077D5 RID: 30677
			protected Material[] subMats = new Material[16];

			// Token: 0x040077D6 RID: 30678
			private const float TexPadding = 0.03125f;
		}
	}
}

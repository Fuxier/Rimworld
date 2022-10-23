using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000134 RID: 308
	public class ScatterableDef : Def
	{
		// Token: 0x060007F6 RID: 2038 RVA: 0x00028550 File Offset: 0x00026750
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = "Scatterable_" + this.texturePath;
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Terrain, ContentFinder<Texture2D>.Get(this.texturePath, true), null);
			});
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}

		// Token: 0x040007FE RID: 2046
		[NoTranslate]
		public string texturePath;

		// Token: 0x040007FF RID: 2047
		public float minSize;

		// Token: 0x04000800 RID: 2048
		public float maxSize;

		// Token: 0x04000801 RID: 2049
		public float selectionWeight = 100f;

		// Token: 0x04000802 RID: 2050
		[NoTranslate]
		public string scatterType = "";

		// Token: 0x04000803 RID: 2051
		public Material mat;
	}
}

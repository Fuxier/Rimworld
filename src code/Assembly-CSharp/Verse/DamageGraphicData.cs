using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C5 RID: 197
	public class DamageGraphicData
	{
		// Token: 0x0600060E RID: 1550 RVA: 0x00020917 File Offset: 0x0001EB17
		public void ResolveReferencesSpecial()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.scratches != null)
				{
					this.scratchMats = new List<Material>();
					for (int i = 0; i < this.scratches.Count; i++)
					{
						this.scratchMats[i] = MaterialPool.MatFrom(this.scratches[i], ShaderDatabase.Transparent, 2905);
						GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.scratchMats[i].mainTexture, null);
					}
				}
				if (this.cornerTL != null)
				{
					this.cornerTLMat = MaterialPool.MatFrom(this.cornerTL, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerTLMat.mainTexture, null);
				}
				if (this.cornerTR != null)
				{
					this.cornerTRMat = MaterialPool.MatFrom(this.cornerTR, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerTRMat.mainTexture, null);
				}
				if (this.cornerBL != null)
				{
					this.cornerBLMat = MaterialPool.MatFrom(this.cornerBL, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerBLMat.mainTexture, null);
				}
				if (this.cornerBR != null)
				{
					this.cornerBRMat = MaterialPool.MatFrom(this.cornerBR, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerBRMat.mainTexture, null);
				}
				if (this.edgeTop != null)
				{
					this.edgeTopMat = MaterialPool.MatFrom(this.edgeTop, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeTopMat.mainTexture, null);
				}
				if (this.edgeBot != null)
				{
					this.edgeBotMat = MaterialPool.MatFrom(this.edgeBot, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeBotMat.mainTexture, null);
				}
				if (this.edgeLeft != null)
				{
					this.edgeLeftMat = MaterialPool.MatFrom(this.edgeLeft, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeLeftMat.mainTexture, null);
				}
				if (this.edgeRight != null)
				{
					this.edgeRightMat = MaterialPool.MatFrom(this.edgeRight, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeRightMat.mainTexture, null);
				}
			});
		}

		// Token: 0x04000384 RID: 900
		public bool enabled = true;

		// Token: 0x04000385 RID: 901
		public Rect rectN;

		// Token: 0x04000386 RID: 902
		public Rect rectE;

		// Token: 0x04000387 RID: 903
		public Rect rectS;

		// Token: 0x04000388 RID: 904
		public Rect rectW;

		// Token: 0x04000389 RID: 905
		public Rect rect;

		// Token: 0x0400038A RID: 906
		[NoTranslate]
		public List<string> scratches;

		// Token: 0x0400038B RID: 907
		[NoTranslate]
		public string cornerTL;

		// Token: 0x0400038C RID: 908
		[NoTranslate]
		public string cornerTR;

		// Token: 0x0400038D RID: 909
		[NoTranslate]
		public string cornerBL;

		// Token: 0x0400038E RID: 910
		[NoTranslate]
		public string cornerBR;

		// Token: 0x0400038F RID: 911
		[NoTranslate]
		public string edgeLeft;

		// Token: 0x04000390 RID: 912
		[NoTranslate]
		public string edgeRight;

		// Token: 0x04000391 RID: 913
		[NoTranslate]
		public string edgeTop;

		// Token: 0x04000392 RID: 914
		[NoTranslate]
		public string edgeBot;

		// Token: 0x04000393 RID: 915
		[Unsaved(false)]
		public List<Material> scratchMats;

		// Token: 0x04000394 RID: 916
		[Unsaved(false)]
		public Material cornerTLMat;

		// Token: 0x04000395 RID: 917
		[Unsaved(false)]
		public Material cornerTRMat;

		// Token: 0x04000396 RID: 918
		[Unsaved(false)]
		public Material cornerBLMat;

		// Token: 0x04000397 RID: 919
		[Unsaved(false)]
		public Material cornerBRMat;

		// Token: 0x04000398 RID: 920
		[Unsaved(false)]
		public Material edgeLeftMat;

		// Token: 0x04000399 RID: 921
		[Unsaved(false)]
		public Material edgeRightMat;

		// Token: 0x0400039A RID: 922
		[Unsaved(false)]
		public Material edgeTopMat;

		// Token: 0x0400039B RID: 923
		[Unsaved(false)]
		public Material edgeBotMat;
	}
}

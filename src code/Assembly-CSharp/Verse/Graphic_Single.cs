using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EB RID: 1003
	public class Graphic_Single : Graphic
	{
		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001C90 RID: 7312 RVA: 0x000AE0E8 File Offset: 0x000AC2E8
		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x000AE0E8 File Offset: 0x000AC2E8
		public override Material MatWest
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001C92 RID: 7314 RVA: 0x000AE0E8 File Offset: 0x000AC2E8
		public override Material MatSouth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x000AE0E8 File Offset: 0x000AC2E8
		public override Material MatEast
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001C94 RID: 7316 RVA: 0x000AE0E8 File Offset: 0x000AC2E8
		public override Material MatNorth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x000AE0F0 File Offset: 0x000AC2F0
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.data == null || this.data.drawRotated;
			}
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000AE10C File Offset: 0x000AC30C
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			Texture2D mask = null;
			if (this.mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				mask = (Texture2D)this.mat.GetTexture(ShaderPropertyIDs.MaskTex);
			}
			GlobalTextureAtlasManager.TryInsertStatic(groupKey, (Texture2D)this.mat.mainTexture, mask);
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000AE15C File Offset: 0x000AC35C
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			MaterialRequest req2 = default(MaterialRequest);
			req2.mainTex = (req.texture ?? ContentFinder<Texture2D>.Get(req.path, true));
			req2.shader = req.shader;
			req2.color = this.color;
			req2.colorTwo = this.colorTwo;
			req2.renderQueue = req.renderQueue;
			req2.shaderParameters = req.shaderParameters;
			if (req.shader.SupportsMaskTex())
			{
				req2.maskTex = ContentFinder<Texture2D>.Get(this.maskPath.NullOrEmpty() ? (this.path + Graphic_Single.MaskSuffix) : this.maskPath, false);
			}
			this.mat = MaterialPool.MatFrom(req2);
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000AE262 File Offset: 0x000AC462
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000AE0E8 File Offset: 0x000AC2E8
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000AE280 File Offset: 0x000AC480
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Single(path=",
				this.path,
				", color=",
				this.color,
				", colorTwo=",
				this.colorTwo,
				")"
			});
		}

		// Token: 0x04001454 RID: 5204
		protected Material mat;

		// Token: 0x04001455 RID: 5205
		public static readonly string MaskSuffix = "_m";
	}
}

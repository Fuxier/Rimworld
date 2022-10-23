using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E6 RID: 998
	public class Graphic_Multi : Graphic
	{
		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001C60 RID: 7264 RVA: 0x000AD200 File Offset: 0x000AB400
		public string GraphicPath
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001C61 RID: 7265 RVA: 0x000AD208 File Offset: 0x000AB408
		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x000AD210 File Offset: 0x000AB410
		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001C63 RID: 7267 RVA: 0x000AD21A File Offset: 0x000AB41A
		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001C64 RID: 7268 RVA: 0x000AD224 File Offset: 0x000AB424
		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001C65 RID: 7269 RVA: 0x000AD22E File Offset: 0x000AB42E
		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001C66 RID: 7270 RVA: 0x000AD238 File Offset: 0x000AB438
		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001C67 RID: 7271 RVA: 0x000AD240 File Offset: 0x000AB440
		public override bool EastFlipped
		{
			get
			{
				return this.eastFlipped;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001C68 RID: 7272 RVA: 0x000AD248 File Offset: 0x000AB448
		public override bool ShouldDrawRotated
		{
			get
			{
				return (this.data == null || this.data.drawRotated) && (this.MatEast == this.MatNorth || this.MatWest == this.MatNorth);
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001C69 RID: 7273 RVA: 0x000AD287 File Offset: 0x000AB487
		public override float DrawRotatedExtraAngleOffset
		{
			get
			{
				return this.drawRotatedExtraAngleOffset;
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000AD290 File Offset: 0x000AB490
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			foreach (Material material in this.mats)
			{
				Texture2D mask = null;
				if (material.HasProperty(ShaderPropertyIDs.MaskTex))
				{
					mask = (Texture2D)material.GetTexture(ShaderPropertyIDs.MaskTex);
				}
				GlobalTextureAtlasManager.TryInsertStatic(groupKey, (Texture2D)material.mainTexture, mask);
			}
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000AD2EC File Offset: 0x000AB4EC
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			Texture2D[] array = new Texture2D[this.mats.Length];
			array[0] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
			array[1] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
			array[2] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
			array[3] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
			if (array[0] == null)
			{
				if (array[2] != null)
				{
					array[0] = array[2];
					this.drawRotatedExtraAngleOffset = 180f;
				}
				else if (array[1] != null)
				{
					array[0] = array[1];
					this.drawRotatedExtraAngleOffset = -90f;
				}
				else if (array[3] != null)
				{
					array[0] = array[3];
					this.drawRotatedExtraAngleOffset = 90f;
				}
				else
				{
					array[0] = ContentFinder<Texture2D>.Get(req.path, false);
				}
			}
			if (array[0] == null)
			{
				Log.Error("Failed to find any textures at " + req.path + " while constructing " + this.ToStringSafe<Graphic_Multi>());
				return;
			}
			if (array[2] == null)
			{
				array[2] = array[0];
			}
			if (array[1] == null)
			{
				if (array[3] != null)
				{
					array[1] = array[3];
					this.eastFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[1] = array[0];
				}
			}
			if (array[3] == null)
			{
				if (array[1] != null)
				{
					array[3] = array[1];
					this.westFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[3] = array[0];
				}
			}
			Texture2D[] array2 = new Texture2D[this.mats.Length];
			if (req.shader.SupportsMaskTex())
			{
				string str = this.maskPath.NullOrEmpty() ? this.path : this.maskPath;
				string str2 = this.maskPath.NullOrEmpty() ? "m" : string.Empty;
				array2[0] = ContentFinder<Texture2D>.Get(str + "_north" + str2, false);
				array2[1] = ContentFinder<Texture2D>.Get(str + "_east" + str2, false);
				array2[2] = ContentFinder<Texture2D>.Get(str + "_south" + str2, false);
				array2[3] = ContentFinder<Texture2D>.Get(str + "_west" + str2, false);
				if (array2[0] == null)
				{
					if (array2[2] != null)
					{
						array2[0] = array2[2];
					}
					else if (array2[1] != null)
					{
						array2[0] = array2[1];
					}
					else if (array2[3] != null)
					{
						array2[0] = array2[3];
					}
				}
				if (array2[2] == null)
				{
					array2[2] = array2[0];
				}
				if (array2[1] == null)
				{
					if (array2[3] != null)
					{
						array2[1] = array2[3];
					}
					else
					{
						array2[1] = array2[0];
					}
				}
				if (array2[3] == null)
				{
					if (array2[1] != null)
					{
						array2[3] = array2[1];
					}
					else
					{
						array2[3] = array2[0];
					}
				}
			}
			for (int i = 0; i < this.mats.Length; i++)
			{
				MaterialRequest req2 = default(MaterialRequest);
				req2.mainTex = array[i];
				req2.shader = req.shader;
				req2.color = this.color;
				req2.colorTwo = this.colorTwo;
				req2.maskTex = array2[i];
				req2.shaderParameters = req.shaderParameters;
				req2.renderQueue = req.renderQueue;
				this.mats[i] = MaterialPool.MatFrom(req2);
			}
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000AD690 File Offset: 0x000AB890
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000AD6B0 File Offset: 0x000AB8B0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Multi(initPath=",
				this.path,
				", color=",
				this.color,
				", colorTwo=",
				this.colorTwo,
				")"
			});
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000AD70D File Offset: 0x000AB90D
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<string>(0, this.path), this.color), this.colorTwo);
		}

		// Token: 0x04001441 RID: 5185
		private Material[] mats = new Material[4];

		// Token: 0x04001442 RID: 5186
		private bool westFlipped;

		// Token: 0x04001443 RID: 5187
		private bool eastFlipped;

		// Token: 0x04001444 RID: 5188
		private float drawRotatedExtraAngleOffset;

		// Token: 0x04001445 RID: 5189
		public const string NorthSuffix = "_north";

		// Token: 0x04001446 RID: 5190
		public const string SouthSuffix = "_south";

		// Token: 0x04001447 RID: 5191
		public const string EastSuffix = "_east";

		// Token: 0x04001448 RID: 5192
		public const string WestSuffix = "_west";
	}
}

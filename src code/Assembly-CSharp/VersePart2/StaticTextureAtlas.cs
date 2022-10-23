using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000387 RID: 903
	public class StaticTextureAtlas
	{
		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x0009CDA0 File Offset: 0x0009AFA0
		public Texture2D ColorTexture
		{
			get
			{
				return this.colorTexture;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x0009CDA8 File Offset: 0x0009AFA8
		public Texture2D MaskTexture
		{
			get
			{
				return this.maskTexture;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x0009CDB0 File Offset: 0x0009AFB0
		public static int MaxPixelsPerAtlas
		{
			get
			{
				return StaticTextureAtlas.MaxAtlasSize / 2 * (StaticTextureAtlas.MaxAtlasSize / 2);
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x0009CDC1 File Offset: 0x0009AFC1
		public static int MaxAtlasSize
		{
			get
			{
				return SystemInfo.maxTextureSize;
			}
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0009CDC8 File Offset: 0x0009AFC8
		public StaticTextureAtlas(TextureAtlasGroupKey groupKey)
		{
			this.groupKey = groupKey;
			this.colorTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0009CE08 File Offset: 0x0009B008
		public void Insert(Texture2D texture, Texture2D mask = null)
		{
			if (this.groupKey.hasMask && mask == null)
			{
				Log.Error("Tried to insert a mask-less texture into a static atlas which does have a mask atlas");
			}
			if (!this.groupKey.hasMask && mask != null)
			{
				Log.Error("Tried to insert a mask texture into a static atlas which does not have a mask atlas");
			}
			this.textures.Add(texture);
			if (mask != null && this.groupKey.hasMask)
			{
				this.masks.Add(texture, mask);
			}
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0009CE84 File Offset: 0x0009B084
		public void Bake(bool rebake = false)
		{
			if (rebake)
			{
				foreach (KeyValuePair<Texture, StaticTextureAtlasTile> keyValuePair in this.tiles)
				{
					UnityEngine.Object.Destroy(keyValuePair.Value.mesh);
				}
				this.tiles.Clear();
			}
			List<Texture2D> destroyTextures = new List<Texture2D>();
			try
			{
				Texture2D[] array = this.textures.Select(delegate(Texture2D t)
				{
					if (!t.isReadable)
					{
						Texture2D texture2D3 = TextureAtlasHelper.MakeReadableTextureInstance(t);
						destroyTextures.Add(texture2D3);
						return texture2D3;
					}
					return t;
				}).ToArray<Texture2D>();
				DeepProfiler.Start("Texture2D.PackTextures()");
				Rect[] array2 = this.colorTexture.PackTextures(array, 8, StaticTextureAtlas.MaxAtlasSize, false);
				DeepProfiler.End();
				this.colorTexture.name = string.Concat(new object[]
				{
					"TextureAtlas_",
					this.groupKey.ToString(),
					"_",
					this.colorTexture.GetInstanceID()
				});
				if (this.groupKey.hasMask)
				{
					this.maskTexture = new Texture2D(this.colorTexture.width, this.colorTexture.height, TextureFormat.ARGB32, false);
				}
				for (int i = 0; i < array2.Length; i++)
				{
					Texture2D key = this.textures[i];
					Texture2D texture2D;
					if (this.masks.TryGetValue(key, out texture2D))
					{
						Rect rect = array2[i];
						int x = (int)(rect.xMin * (float)this.colorTexture.width);
						int y = (int)(rect.yMin * (float)this.colorTexture.height);
						if (!texture2D.isReadable)
						{
							Texture2D texture2D2 = TextureAtlasHelper.MakeReadableTextureInstance(texture2D);
							destroyTextures.Add(texture2D2);
							texture2D = texture2D2;
						}
						DeepProfiler.Start("maskTexture.SetPixels()");
						this.maskTexture.SetPixels(x, y, this.textures[i].width, this.textures[i].height, texture2D.GetPixels(0), 0);
						DeepProfiler.End();
					}
				}
				if (this.maskTexture != null)
				{
					this.maskTexture.name = "Mask_" + this.colorTexture.name;
					DeepProfiler.Start("maskTexture.Apply()");
					this.maskTexture.Apply(true, false);
					DeepProfiler.End();
				}
				if (array2.Length != array.Length)
				{
					Log.Error("Texture packing failed! Clearing out atlas...");
					this.textures.Clear();
				}
				else
				{
					for (int j = 0; j < array.Length; j++)
					{
						Mesh mesh = TextureAtlasHelper.CreateMeshForUV(array2[j], 0.5f);
						mesh.name = string.Concat(new object[]
						{
							"TextureAtlasMesh_",
							this.groupKey.ToString(),
							"_",
							mesh.GetInstanceID()
						});
						this.tiles.Add(this.textures[j], new StaticTextureAtlasTile
						{
							atlas = this,
							mesh = mesh,
							uvRect = array2[j]
						});
					}
					if (Prefs.TextureCompression)
					{
						DeepProfiler.Start("Texture2D.Compress()");
						if (this.colorTexture != null)
						{
							this.colorTexture.Compress(true);
						}
						if (this.maskTexture != null)
						{
							this.maskTexture.Compress(true);
						}
						DeepProfiler.End();
					}
					DeepProfiler.Start("Texture2D.Apply()");
					if (this.colorTexture != null)
					{
						this.colorTexture.Apply(false, true);
					}
					if (this.maskTexture != null)
					{
						this.maskTexture.Apply(false, true);
					}
					DeepProfiler.End();
				}
			}
			finally
			{
				foreach (Texture2D obj in destroyTextures)
				{
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x0009D2C8 File Offset: 0x0009B4C8
		public bool TryGetTile(Texture texture, out StaticTextureAtlasTile tile)
		{
			return this.tiles.TryGetValue(texture, out tile);
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0009D2D8 File Offset: 0x0009B4D8
		public void Destroy()
		{
			UnityEngine.Object.Destroy(this.colorTexture);
			UnityEngine.Object.Destroy(this.maskTexture);
			foreach (KeyValuePair<Texture, StaticTextureAtlasTile> keyValuePair in this.tiles)
			{
				UnityEngine.Object.Destroy(keyValuePair.Value.mesh);
			}
			this.textures.Clear();
			this.tiles.Clear();
		}

		// Token: 0x040012FB RID: 4859
		public readonly TextureAtlasGroupKey groupKey;

		// Token: 0x040012FC RID: 4860
		private List<Texture2D> textures = new List<Texture2D>();

		// Token: 0x040012FD RID: 4861
		private Dictionary<Texture2D, Texture2D> masks = new Dictionary<Texture2D, Texture2D>();

		// Token: 0x040012FE RID: 4862
		private Dictionary<Texture, StaticTextureAtlasTile> tiles = new Dictionary<Texture, StaticTextureAtlasTile>();

		// Token: 0x040012FF RID: 4863
		private Texture2D colorTexture;

		// Token: 0x04001300 RID: 4864
		private Texture2D maskTexture;

		// Token: 0x04001301 RID: 4865
		public const int MaxTextureSizeForTiles = 512;

		// Token: 0x04001302 RID: 4866
		public const int TexturePadding = 8;
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200013E RID: 318
	public class SubcameraDef : Def
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00028DE4 File Offset: 0x00026FE4
		public int LayerId
		{
			get
			{
				if (this.layerCached == -1)
				{
					this.layerCached = LayerMask.NameToLayer(this.layer);
				}
				return this.layerCached;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x00028E08 File Offset: 0x00027008
		public RenderTextureFormat BestFormat
		{
			get
			{
				if (SystemInfo.SupportsRenderTextureFormat(this.format))
				{
					return this.format;
				}
				if (this.format == RenderTextureFormat.R8 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RG16))
				{
					return RenderTextureFormat.RG16;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RG16) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
				{
					return RenderTextureFormat.ARGB32;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
				{
					return RenderTextureFormat.RGFloat;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat || this.format == RenderTextureFormat.RGFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
				{
					return RenderTextureFormat.ARGBFloat;
				}
				return this.format;
			}
		}

		// Token: 0x04000832 RID: 2098
		[NoTranslate]
		public string layer;

		// Token: 0x04000833 RID: 2099
		public int depth;

		// Token: 0x04000834 RID: 2100
		public RenderTextureFormat format;

		// Token: 0x04000835 RID: 2101
		[Unsaved(false)]
		private int layerCached = -1;
	}
}

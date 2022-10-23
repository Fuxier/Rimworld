using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037E RID: 894
	public struct CachedTexture
	{
		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x060019DA RID: 6618 RVA: 0x0009BE84 File Offset: 0x0009A084
		public Texture2D Texture
		{
			get
			{
				if (this.cachedTexture == null)
				{
					if (this.texPath.NullOrEmpty())
					{
						this.cachedTexture = BaseContent.BadTex;
					}
					else
					{
						this.cachedTexture = (ContentFinder<Texture2D>.Get(this.texPath, true) ?? BaseContent.BadTex);
					}
				}
				return this.cachedTexture;
			}
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0009BEDA File Offset: 0x0009A0DA
		public CachedTexture(string texPath)
		{
			this.texPath = texPath;
			this.cachedTexture = null;
		}

		// Token: 0x040012D8 RID: 4824
		private string texPath;

		// Token: 0x040012D9 RID: 4825
		private Texture2D cachedTexture;
	}
}

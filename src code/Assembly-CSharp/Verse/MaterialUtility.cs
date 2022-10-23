using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000580 RID: 1408
	public static class MaterialUtility
	{
		// Token: 0x06002B1D RID: 11037 RVA: 0x00113B3B File Offset: 0x00111D3B
		public static Texture2D GetMaskTexture(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				return null;
			}
			return (Texture2D)mat.GetTexture(ShaderPropertyIDs.MaskTex);
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x00113B5C File Offset: 0x00111D5C
		public static Color GetColorTwo(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.ColorTwo))
			{
				return Color.white;
			}
			return mat.GetColor(ShaderPropertyIDs.ColorTwo);
		}
	}
}

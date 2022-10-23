using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000057 RID: 87
	public static class ShaderUtility
	{
		// Token: 0x0600043F RID: 1087 RVA: 0x00017DFC File Offset: 0x00015FFC
		public static bool SupportsMaskTex(this Shader shader)
		{
			return shader == ShaderDatabase.CutoutComplex || shader == ShaderDatabase.CutoutSkinOverlay || shader == ShaderDatabase.Wound || shader == ShaderDatabase.FirefoamOverlay || shader == ShaderDatabase.CutoutWithOverlay;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00017E4A File Offset: 0x0001604A
		public static Shader GetSkinShader(bool skinColorOverriden)
		{
			if (skinColorOverriden)
			{
				return ShaderDatabase.CutoutSkinColorOverride;
			}
			return ShaderDatabase.CutoutSkin;
		}
	}
}

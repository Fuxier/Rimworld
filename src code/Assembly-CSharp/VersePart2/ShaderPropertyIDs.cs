using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000584 RID: 1412
	[StaticConstructorOnStartup]
	public static class ShaderPropertyIDs
	{
		// Token: 0x04001C28 RID: 7208
		private static readonly string PlanetSunLightDirectionName = "_PlanetSunLightDirection";

		// Token: 0x04001C29 RID: 7209
		private static readonly string PlanetSunLightEnabledName = "_PlanetSunLightEnabled";

		// Token: 0x04001C2A RID: 7210
		private static readonly string PlanetRadiusName = "_PlanetRadius";

		// Token: 0x04001C2B RID: 7211
		private static readonly string MapSunLightDirectionName = "_CastVect";

		// Token: 0x04001C2C RID: 7212
		private static readonly string GlowRadiusName = "_GlowRadius";

		// Token: 0x04001C2D RID: 7213
		private static readonly string GameSecondsName = "_GameSeconds";

		// Token: 0x04001C2E RID: 7214
		private static readonly string ColorName = "_Color";

		// Token: 0x04001C2F RID: 7215
		private static readonly string ColorTwoName = "_ColorTwo";

		// Token: 0x04001C30 RID: 7216
		private static readonly string MaskTexName = "_MaskTex";

		// Token: 0x04001C31 RID: 7217
		private static readonly string SwayHeadName = "_SwayHead";

		// Token: 0x04001C32 RID: 7218
		private static readonly string ShockwaveSpanName = "_ShockwaveSpan";

		// Token: 0x04001C33 RID: 7219
		private static readonly string AgeSecsName = "_AgeSecs";

		// Token: 0x04001C34 RID: 7220
		private static readonly string RandomPerObjectName = "_RandomPerObject";

		// Token: 0x04001C35 RID: 7221
		private static readonly string RotationName = "_Rotation";

		// Token: 0x04001C36 RID: 7222
		private static readonly string OverlayOpacityName = "_OverlayOpacity";

		// Token: 0x04001C37 RID: 7223
		private static readonly string OverlayColorName = "_OverlayColor";

		// Token: 0x04001C38 RID: 7224
		private static readonly string AgeSecsPausableName = "_AgeSecsPausable";

		// Token: 0x04001C39 RID: 7225
		private static readonly string MainTextureOffsetName = "_Main_TexOffset";

		// Token: 0x04001C3A RID: 7226
		private static readonly string MainTextureScaleName = "_Main_TexScale";

		// Token: 0x04001C3B RID: 7227
		private static readonly string MaskTextureOffsetName = "_Mask_TexOffset";

		// Token: 0x04001C3C RID: 7228
		private static readonly string MaskTextureScaleName = "_Mask_TexScale";

		// Token: 0x04001C3D RID: 7229
		public static int PlanetSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightDirectionName);

		// Token: 0x04001C3E RID: 7230
		public static int PlanetSunLightEnabled = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightEnabledName);

		// Token: 0x04001C3F RID: 7231
		public static int PlanetRadius = Shader.PropertyToID(ShaderPropertyIDs.PlanetRadiusName);

		// Token: 0x04001C40 RID: 7232
		public static int MapSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.MapSunLightDirectionName);

		// Token: 0x04001C41 RID: 7233
		public static int GlowRadius = Shader.PropertyToID(ShaderPropertyIDs.GlowRadiusName);

		// Token: 0x04001C42 RID: 7234
		public static int GameSeconds = Shader.PropertyToID(ShaderPropertyIDs.GameSecondsName);

		// Token: 0x04001C43 RID: 7235
		public static int AgeSecs = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsName);

		// Token: 0x04001C44 RID: 7236
		public static int AgeSecsPausable = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsPausableName);

		// Token: 0x04001C45 RID: 7237
		public static int RandomPerObject = Shader.PropertyToID(ShaderPropertyIDs.RandomPerObjectName);

		// Token: 0x04001C46 RID: 7238
		public static int Rotation = Shader.PropertyToID(ShaderPropertyIDs.RotationName);

		// Token: 0x04001C47 RID: 7239
		public static int OverlayOpacity = Shader.PropertyToID(ShaderPropertyIDs.OverlayOpacityName);

		// Token: 0x04001C48 RID: 7240
		public static int OverlayColor = Shader.PropertyToID(ShaderPropertyIDs.OverlayColorName);

		// Token: 0x04001C49 RID: 7241
		public static int Color = Shader.PropertyToID(ShaderPropertyIDs.ColorName);

		// Token: 0x04001C4A RID: 7242
		public static int ColorTwo = Shader.PropertyToID(ShaderPropertyIDs.ColorTwoName);

		// Token: 0x04001C4B RID: 7243
		public static int MaskTex = Shader.PropertyToID(ShaderPropertyIDs.MaskTexName);

		// Token: 0x04001C4C RID: 7244
		public static int SwayHead = Shader.PropertyToID(ShaderPropertyIDs.SwayHeadName);

		// Token: 0x04001C4D RID: 7245
		public static int ShockwaveColor = Shader.PropertyToID("_ShockwaveColor");

		// Token: 0x04001C4E RID: 7246
		public static int ShockwaveSpan = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveSpanName);

		// Token: 0x04001C4F RID: 7247
		public static int WaterCastVectSun = Shader.PropertyToID("_WaterCastVectSun");

		// Token: 0x04001C50 RID: 7248
		public static int WaterCastVectMoon = Shader.PropertyToID("_WaterCastVectMoon");

		// Token: 0x04001C51 RID: 7249
		public static int WaterOutputTex = Shader.PropertyToID("_WaterOutputTex");

		// Token: 0x04001C52 RID: 7250
		public static int WaterOffsetTex = Shader.PropertyToID("_WaterOffsetTex");

		// Token: 0x04001C53 RID: 7251
		public static int ShadowCompositeTex = Shader.PropertyToID("_ShadowCompositeTex");

		// Token: 0x04001C54 RID: 7252
		public static int FallIntensity = Shader.PropertyToID("_FallIntensity");

		// Token: 0x04001C55 RID: 7253
		public static int MainTextureOffset = Shader.PropertyToID(ShaderPropertyIDs.MainTextureOffsetName);

		// Token: 0x04001C56 RID: 7254
		public static int MainTextureScale = Shader.PropertyToID(ShaderPropertyIDs.MainTextureScaleName);

		// Token: 0x04001C57 RID: 7255
		public static int MaskTextureOffset = Shader.PropertyToID(ShaderPropertyIDs.MaskTextureOffsetName);

		// Token: 0x04001C58 RID: 7256
		public static int MaskTextureScale = Shader.PropertyToID(ShaderPropertyIDs.MaskTextureScaleName);
	}
}

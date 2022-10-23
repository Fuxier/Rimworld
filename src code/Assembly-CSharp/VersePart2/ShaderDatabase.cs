using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000056 RID: 86
	[StaticConstructorOnStartup]
	public static class ShaderDatabase
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00017ADB File Offset: 0x00015CDB
		public static Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00017AE4 File Offset: 0x00015CE4
		public static Shader LoadShader(string shaderPath)
		{
			if (ShaderDatabase.lookup == null)
			{
				ShaderDatabase.lookup = new Dictionary<string, Shader>();
			}
			if (!ShaderDatabase.lookup.ContainsKey(shaderPath))
			{
				ShaderDatabase.lookup[shaderPath] = (Shader)Resources.Load("Materials/" + shaderPath, typeof(Shader));
			}
			Shader shader = ShaderDatabase.lookup[shaderPath];
			if (shader == null)
			{
				Log.Warning("Could not load shader " + shaderPath);
				return ShaderDatabase.DefaultShader;
			}
			return shader;
		}

		// Token: 0x04000138 RID: 312
		public static readonly Shader Cutout = ShaderDatabase.LoadShader("Map/Cutout");

		// Token: 0x04000139 RID: 313
		public static readonly Shader CutoutPlant = ShaderDatabase.LoadShader("Map/CutoutPlant");

		// Token: 0x0400013A RID: 314
		public static readonly Shader CutoutComplex = ShaderDatabase.LoadShader("Map/CutoutComplex");

		// Token: 0x0400013B RID: 315
		public static readonly Shader CutoutSkinOverlay = ShaderDatabase.LoadShader("Map/CutoutSkinOverlay");

		// Token: 0x0400013C RID: 316
		public static readonly Shader CutoutSkin = ShaderDatabase.LoadShader("Map/CutoutSkin");

		// Token: 0x0400013D RID: 317
		public static readonly Shader Wound = ShaderDatabase.LoadShader("Map/Wound");

		// Token: 0x0400013E RID: 318
		public static readonly Shader WoundSkin = ShaderDatabase.LoadShader("Map/WoundSkin");

		// Token: 0x0400013F RID: 319
		public static readonly Shader CutoutSkinColorOverride = ShaderDatabase.LoadShader("Map/CutoutSkinOverride");

		// Token: 0x04000140 RID: 320
		public static readonly Shader CutoutFlying = ShaderDatabase.LoadShader("Map/CutoutFlying");

		// Token: 0x04000141 RID: 321
		public static readonly Shader FirefoamOverlay = ShaderDatabase.LoadShader("Map/FirefoamOverlay");

		// Token: 0x04000142 RID: 322
		public static readonly Shader CutoutWithOverlay = ShaderDatabase.LoadShader("Map/CutoutWithOverlay");

		// Token: 0x04000143 RID: 323
		public static readonly Shader Transparent = ShaderDatabase.LoadShader("Map/Transparent");

		// Token: 0x04000144 RID: 324
		public static readonly Shader TransparentPostLight = ShaderDatabase.LoadShader("Map/TransparentPostLight");

		// Token: 0x04000145 RID: 325
		public static readonly Shader TransparentPlant = ShaderDatabase.LoadShader("Map/TransparentPlant");

		// Token: 0x04000146 RID: 326
		public static readonly Shader Mote = ShaderDatabase.LoadShader("Map/Mote");

		// Token: 0x04000147 RID: 327
		public static readonly Shader MoteGlow = ShaderDatabase.LoadShader("Map/MoteGlow");

		// Token: 0x04000148 RID: 328
		public static readonly Shader MoteGlowPulse = ShaderDatabase.LoadShader("Map/MoteGlowPulse");

		// Token: 0x04000149 RID: 329
		public static readonly Shader MoteWater = ShaderDatabase.LoadShader("Map/MoteWater");

		// Token: 0x0400014A RID: 330
		public static readonly Shader MoteGlowDistorted = ShaderDatabase.LoadShader("Map/MoteGlowDistorted");

		// Token: 0x0400014B RID: 331
		public static readonly Shader MoteGlowDistortBG = ShaderDatabase.LoadShader("Map/MoteGlowDistortBackground");

		// Token: 0x0400014C RID: 332
		public static readonly Shader MoteProximityScannerRadius = ShaderDatabase.LoadShader("Map/MoteProximityScannerRadius");

		// Token: 0x0400014D RID: 333
		public static readonly Shader GasRotating = ShaderDatabase.LoadShader("Map/GasRotating");

		// Token: 0x0400014E RID: 334
		public static readonly Shader TerrainHard = ShaderDatabase.LoadShader("Map/TerrainHard");

		// Token: 0x0400014F RID: 335
		public static readonly Shader TerrainFade = ShaderDatabase.LoadShader("Map/TerrainFade");

		// Token: 0x04000150 RID: 336
		public static readonly Shader TerrainFadeRough = ShaderDatabase.LoadShader("Map/TerrainFadeRough");

		// Token: 0x04000151 RID: 337
		public static readonly Shader TerrainWater = ShaderDatabase.LoadShader("Map/TerrainWater");

		// Token: 0x04000152 RID: 338
		public static readonly Shader TerrainHardPolluted = ShaderDatabase.LoadShader("Map/TerrainHardLinearBurn");

		// Token: 0x04000153 RID: 339
		public static readonly Shader TerrainFadePolluted = ShaderDatabase.LoadShader("Map/TerrainFadeLinearBurn");

		// Token: 0x04000154 RID: 340
		public static readonly Shader TerrainFadeRoughPolluted = ShaderDatabase.LoadShader("Map/TerrainFadeRoughLinearBurn");

		// Token: 0x04000155 RID: 341
		public static readonly Shader PollutionCloud = ShaderDatabase.LoadShader("Map/PollutionCloud");

		// Token: 0x04000156 RID: 342
		public static readonly Shader WorldTerrain = ShaderDatabase.LoadShader("World/WorldTerrain");

		// Token: 0x04000157 RID: 343
		public static readonly Shader WorldOcean = ShaderDatabase.LoadShader("World/WorldOcean");

		// Token: 0x04000158 RID: 344
		public static readonly Shader WorldOverlayCutout = ShaderDatabase.LoadShader("World/WorldOverlayCutout");

		// Token: 0x04000159 RID: 345
		public static readonly Shader WorldOverlayTransparent = ShaderDatabase.LoadShader("World/WorldOverlayTransparent");

		// Token: 0x0400015A RID: 346
		public static readonly Shader WorldOverlayTransparentLit = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLit");

		// Token: 0x0400015B RID: 347
		public static readonly Shader WorldOverlayTransparentLitPollution = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLitPollution");

		// Token: 0x0400015C RID: 348
		public static readonly Shader WorldOverlayAdditive = ShaderDatabase.LoadShader("World/WorldOverlayAdditive");

		// Token: 0x0400015D RID: 349
		public static readonly Shader MetaOverlay = ShaderDatabase.LoadShader("Map/MetaOverlay");

		// Token: 0x0400015E RID: 350
		public static readonly Shader MetaOverlayDesaturated = ShaderDatabase.LoadShader("Map/MetaOverlayDesaturated");

		// Token: 0x0400015F RID: 351
		public static readonly Shader SolidColor = ShaderDatabase.LoadShader("Map/SolidColor");

		// Token: 0x04000160 RID: 352
		public static readonly Shader VertexColor = ShaderDatabase.LoadShader("Map/VertexColor");

		// Token: 0x04000161 RID: 353
		public static readonly Shader RitualStencil = ShaderDatabase.LoadShader("Map/RitualStencil");

		// Token: 0x04000162 RID: 354
		public static readonly Shader Invisible = ShaderDatabase.LoadShader("Misc/Invisible");

		// Token: 0x04000163 RID: 355
		private static Dictionary<string, Shader> lookup;
	}
}

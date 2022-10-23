using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000585 RID: 1413
	public static class SolidColorMaterials
	{
		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06002B25 RID: 11045 RVA: 0x00113F43 File Offset: 0x00112143
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x00113F5C File Offset: 0x0011215C
		public static Material SimpleSolidColorMaterial(Color col, bool careAboutVertexColors = false)
		{
			col = col;
			Material material;
			if (careAboutVertexColors)
			{
				if (!SolidColorMaterials.simpleColorAndVertexColorMats.TryGetValue(col, out material))
				{
					material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.VertexColor);
					SolidColorMaterials.simpleColorAndVertexColorMats.Add(col, material);
				}
			}
			else if (!SolidColorMaterials.simpleColorMats.TryGetValue(col, out material))
			{
				material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.SolidColor);
				SolidColorMaterials.simpleColorMats.Add(col, material);
			}
			return material;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x00113FCC File Offset: 0x001121CC
		public static Material NewSolidColorMaterial(Color col, Shader shader)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a material from a different thread.");
				return null;
			}
			Material material = MaterialAllocator.Create(shader);
			material.color = col;
			material.name = string.Concat(new object[]
			{
				"SolidColorMat-",
				shader.name,
				"-",
				col
			});
			return material;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x0011402E File Offset: 0x0011222E
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x00114040 File Offset: 0x00112240
		public static Texture2D NewSolidColorTexture(Color color)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a texture from a different thread.");
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.name = "SolidColorTex-" + color;
			texture2D.SetPixel(0, 0, color);
			texture2D.SetPixel(1, 0, color);
			texture2D.SetPixel(0, 1, color);
			texture2D.SetPixel(1, 1, color);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x04001C59 RID: 7257
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x04001C5A RID: 7258
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038E RID: 910
	public static class MaterialPool
	{
		// Token: 0x06001A20 RID: 6688 RVA: 0x0009DAE6 File Offset: 0x0009BCE6
		public static Material MatFrom(string texPath, bool reportFailure)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, reportFailure)));
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x0009DB0B File Offset: 0x0009BD0B
		public static Material MatFrom(string texPath)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true)));
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0009DB30 File Offset: 0x0009BD30
		public static Material MatFrom(Texture2D srcTex)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex));
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x0009DB3D File Offset: 0x0009BD3D
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color));
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0009DB4C File Offset: 0x0009BD4C
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0009DB71 File Offset: 0x0009BD71
		public static Material MatFrom(string texPath, Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader));
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0009DB88 File Offset: 0x0009BD88
		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x0009DBB2 File Offset: 0x0009BDB2
		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color));
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x0009DBC8 File Offset: 0x0009BDC8
		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0009DBF3 File Offset: 0x0009BDF3
		public static Material MatFrom(Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(shader));
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x0009DC00 File Offset: 0x0009BE00
		public static Material MatFrom(MaterialRequest req)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a material from a different thread.");
				return null;
			}
			if (req.mainTex == null && req.needsMainTex)
			{
				Log.Error("MatFrom with null sourceTex.");
				return BaseContent.BadMat;
			}
			if (req.shader == null)
			{
				Log.Warning("Matfrom with null shader.");
				return BaseContent.BadMat;
			}
			if (req.maskTex != null && !req.shader.SupportsMaskTex())
			{
				Log.Error("MaterialRequest has maskTex but shader does not support it. req=" + req.ToString());
				req.maskTex = null;
			}
			req.color = req.color;
			req.colorTwo = req.colorTwo;
			Material material;
			if (!MaterialPool.matDictionary.TryGetValue(req, out material))
			{
				material = MaterialAllocator.Create(req.shader);
				material.name = req.shader.name;
				if (req.mainTex != null)
				{
					Material material2 = material;
					material2.name = material2.name + "_" + req.mainTex.name;
					material.mainTexture = req.mainTex;
				}
				material.color = req.color;
				if (req.maskTex != null)
				{
					material.SetTexture(ShaderPropertyIDs.MaskTex, req.maskTex);
					material.SetColor(ShaderPropertyIDs.ColorTwo, req.colorTwo);
				}
				if (req.renderQueue != 0)
				{
					material.renderQueue = req.renderQueue;
				}
				if (!req.shaderParameters.NullOrEmpty<ShaderParameter>())
				{
					for (int i = 0; i < req.shaderParameters.Count; i++)
					{
						req.shaderParameters[i].Apply(material);
					}
				}
				MaterialPool.matDictionary.Add(req, material);
				MaterialPool.matDictionaryReverse.Add(material, req);
				if (req.shader == ShaderDatabase.CutoutPlant || req.shader == ShaderDatabase.TransparentPlant)
				{
					WindManager.Notify_PlantMaterialCreated(material);
				}
			}
			return material;
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0009DE03 File Offset: 0x0009C003
		public static bool TryGetRequestForMat(Material material, out MaterialRequest request)
		{
			return MaterialPool.matDictionaryReverse.TryGetValue(material, out request);
		}

		// Token: 0x04001311 RID: 4881
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();

		// Token: 0x04001312 RID: 4882
		private static Dictionary<Material, MaterialRequest> matDictionaryReverse = new Dictionary<Material, MaterialRequest>();
	}
}

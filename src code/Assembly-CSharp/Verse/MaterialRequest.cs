using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038F RID: 911
	public struct MaterialRequest : IEquatable<MaterialRequest>
	{
		// Token: 0x17000583 RID: 1411
		// (set) Token: 0x06001A2D RID: 6701 RVA: 0x0009DE27 File Offset: 0x0009C027
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0009DE38 File Offset: 0x0009C038
		public MaterialRequest(Texture tex)
		{
			this.shader = ShaderDatabase.Cutout;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.needsMainTex = true;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x0009DE8C File Offset: 0x0009C08C
		public MaterialRequest(Texture tex, Shader shader)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.needsMainTex = true;
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x0009DED9 File Offset: 0x0009C0D9
		public MaterialRequest(Texture tex, Shader shader, Color color)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = color;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.needsMainTex = true;
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0009DF18 File Offset: 0x0009C118
		public MaterialRequest(Shader shader)
		{
			this.shader = shader;
			this.mainTex = null;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.needsMainTex = false;
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x0009DF68 File Offset: 0x0009C168
		public override int GetHashCode()
		{
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombineInt(Gen.HashCombine<Texture2D>(Gen.HashCombine<Texture>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<Shader>(0, this.shader), this.color), this.colorTwo), this.mainTex), this.maskTex), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x0009DFC3 File Offset: 0x0009C1C3
		public override bool Equals(object obj)
		{
			return obj is MaterialRequest && this.Equals((MaterialRequest)obj);
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x0009DFDC File Offset: 0x0009C1DC
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.shaderParameters == this.shaderParameters;
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x0009E066 File Offset: 0x0009C266
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x0009E070 File Offset: 0x0009C270
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x0009E07C File Offset: 0x0009C27C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"MaterialRequest(",
				this.shader.name,
				", ",
				this.mainTex.name,
				", ",
				this.color.ToString(),
				", ",
				this.colorTwo.ToString(),
				", ",
				this.maskTex.ToString(),
				", ",
				this.renderQueue.ToString(),
				")"
			});
		}

		// Token: 0x04001313 RID: 4883
		public Shader shader;

		// Token: 0x04001314 RID: 4884
		public Texture mainTex;

		// Token: 0x04001315 RID: 4885
		public Color color;

		// Token: 0x04001316 RID: 4886
		public Color colorTwo;

		// Token: 0x04001317 RID: 4887
		public Texture2D maskTex;

		// Token: 0x04001318 RID: 4888
		public int renderQueue;

		// Token: 0x04001319 RID: 4889
		public bool needsMainTex;

		// Token: 0x0400131A RID: 4890
		public List<ShaderParameter> shaderParameters;
	}
}

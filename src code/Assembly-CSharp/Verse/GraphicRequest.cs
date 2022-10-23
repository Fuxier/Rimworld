using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003CE RID: 974
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		// Token: 0x06001BE4 RID: 7140 RVA: 0x000AAC80 File Offset: 0x000A8E80
		public GraphicRequest(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters, string maskPath)
		{
			this.graphicClass = graphicClass;
			this.path = path;
			this.maskPath = maskPath;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = (shaderParameters.NullOrEmpty<ShaderParameter>() ? null : shaderParameters);
			this.texture = null;
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000AACF0 File Offset: 0x000A8EF0
		public GraphicRequest(Type graphicClass, Texture2D texture, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters, string maskPath)
		{
			this.graphicClass = graphicClass;
			this.texture = texture;
			this.maskPath = maskPath;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = (shaderParameters.NullOrEmpty<ShaderParameter>() ? null : shaderParameters);
			this.path = null;
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000AAD60 File Offset: 0x000A8F60
		public override int GetHashCode()
		{
			if (this.path == null)
			{
				this.path = BaseContent.BadTexPath;
			}
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombine<int>(Gen.HashCombine<GraphicData>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Vector2>(Gen.HashCombine<Shader>(Gen.HashCombine<string>(Gen.HashCombine<Texture2D>(Gen.HashCombine<string>(Gen.HashCombine<Type>(0, this.graphicClass), this.path), this.texture), this.maskPath), this.shader), this.drawSize), this.color), this.colorTwo), this.graphicData), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x000AADFA File Offset: 0x000A8FFA
		public override bool Equals(object obj)
		{
			return obj is GraphicRequest && this.Equals((GraphicRequest)obj);
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x000AAE14 File Offset: 0x000A9014
		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && this.texture == other.texture && this.maskPath == other.maskPath && this.shader == other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue && this.shaderParameters == other.shaderParameters;
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x000AAEEE File Offset: 0x000A90EE
		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000AAEF8 File Offset: 0x000A90F8
		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04001413 RID: 5139
		public Type graphicClass;

		// Token: 0x04001414 RID: 5140
		public Texture2D texture;

		// Token: 0x04001415 RID: 5141
		public string path;

		// Token: 0x04001416 RID: 5142
		public string maskPath;

		// Token: 0x04001417 RID: 5143
		public Shader shader;

		// Token: 0x04001418 RID: 5144
		public Vector2 drawSize;

		// Token: 0x04001419 RID: 5145
		public Color color;

		// Token: 0x0400141A RID: 5146
		public Color colorTwo;

		// Token: 0x0400141B RID: 5147
		public GraphicData graphicData;

		// Token: 0x0400141C RID: 5148
		public int renderQueue;

		// Token: 0x0400141D RID: 5149
		public List<ShaderParameter> shaderParameters;
	}
}

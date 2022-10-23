using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000058 RID: 88
	public class ShaderParameter
	{
		// Token: 0x06000441 RID: 1089 RVA: 0x00017E5C File Offset: 0x0001605C
		public void Apply(Material mat)
		{
			switch (this.type)
			{
			case ShaderParameter.Type.Float:
				mat.SetFloat(this.name, this.value.x);
				return;
			case ShaderParameter.Type.Vector:
				mat.SetVector(this.name, this.value);
				return;
			case ShaderParameter.Type.Matrix:
				break;
			case ShaderParameter.Type.Texture:
				if (this.valueTex == null)
				{
					Log.ErrorOnce(string.Format("Texture for {0} is not yet loaded; file may be invalid, or main thread may not have loaded it yet", this.name), 27929440);
				}
				mat.SetTexture(this.name, this.valueTex);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00017EEC File Offset: 0x000160EC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ShaderParameter: " + xmlRoot.OuterXml);
				return;
			}
			this.name = xmlRoot.Name;
			string valstr = xmlRoot.FirstChild.Value;
			if (!valstr.NullOrEmpty() && valstr[0] == '(')
			{
				this.value = ParseHelper.FromStringVector4Adaptive(valstr);
				this.type = ShaderParameter.Type.Vector;
				return;
			}
			if (!valstr.NullOrEmpty() && valstr[0] == '/')
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.valueTex = ContentFinder<Texture2D>.Get(valstr.TrimStart(new char[]
					{
						'/'
					}), true);
				});
				this.type = ShaderParameter.Type.Texture;
				return;
			}
			this.value = Vector4.one * ParseHelper.FromString<float>(valstr);
			this.type = ShaderParameter.Type.Float;
		}

		// Token: 0x04000164 RID: 356
		[NoTranslate]
		private string name;

		// Token: 0x04000165 RID: 357
		private Vector4 value;

		// Token: 0x04000166 RID: 358
		private Texture2D valueTex;

		// Token: 0x04000167 RID: 359
		private ShaderParameter.Type type;

		// Token: 0x02001C8F RID: 7311
		private enum Type
		{
			// Token: 0x04007085 RID: 28805
			Float,
			// Token: 0x04007086 RID: 28806
			Vector,
			// Token: 0x04007087 RID: 28807
			Matrix,
			// Token: 0x04007088 RID: 28808
			Texture
		}
	}
}

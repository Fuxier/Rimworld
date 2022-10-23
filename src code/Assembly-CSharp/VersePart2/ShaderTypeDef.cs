using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000135 RID: 309
	public class ShaderTypeDef : Def
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x000285F9 File Offset: 0x000267F9
		public Shader Shader
		{
			get
			{
				if (this.shaderInt == null)
				{
					this.shaderInt = ShaderDatabase.LoadShader(this.shaderPath);
				}
				return this.shaderInt;
			}
		}

		// Token: 0x04000804 RID: 2052
		[NoTranslate]
		public string shaderPath;

		// Token: 0x04000805 RID: 2053
		[Unsaved(false)]
		private Shader shaderInt;
	}
}

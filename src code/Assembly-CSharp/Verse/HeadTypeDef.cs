using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F8 RID: 248
	public class HeadTypeDef : Def
	{
		// Token: 0x060006E8 RID: 1768 RVA: 0x00024E4C File Offset: 0x0002304C
		public Graphic_Multi GetGraphic(Color color, bool dessicated = false, bool skinColorOverriden = false)
		{
			Shader shader = (!dessicated) ? ShaderUtility.GetSkinShader(skinColorOverriden) : ShaderDatabase.Cutout;
			for (int i = 0; i < this.graphics.Count; i++)
			{
				if (color.IndistinguishableFrom(this.graphics[i].Key) && this.graphics[i].Value.Shader == shader)
				{
					return this.graphics[i].Value;
				}
			}
			Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(this.graphicPath, shader, Vector2.one, color);
			this.graphics.Add(new KeyValuePair<Color, Graphic_Multi>(color, graphic_Multi));
			return graphic_Multi;
		}

		// Token: 0x040005B3 RID: 1459
		public string graphicPath;

		// Token: 0x040005B4 RID: 1460
		public Gender gender;

		// Token: 0x040005B5 RID: 1461
		public bool narrow;

		// Token: 0x040005B6 RID: 1462
		public Vector2 hairMeshSize = new Vector2(1.5f, 1.5f);

		// Token: 0x040005B7 RID: 1463
		public Vector2 beardMeshSize = new Vector2(1.5f, 1.5f);

		// Token: 0x040005B8 RID: 1464
		public Vector3 beardOffset;

		// Token: 0x040005B9 RID: 1465
		public Vector3? eyeOffsetEastWest;

		// Token: 0x040005BA RID: 1466
		public float beardOffsetXEast;

		// Token: 0x040005BB RID: 1467
		public float selectionWeight = 1f;

		// Token: 0x040005BC RID: 1468
		public bool randomChosen = true;

		// Token: 0x040005BD RID: 1469
		public List<GeneDef> requiredGenes;

		// Token: 0x040005BE RID: 1470
		[Unsaved(false)]
		private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();
	}
}

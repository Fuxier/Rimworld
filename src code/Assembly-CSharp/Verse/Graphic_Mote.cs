using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E2 RID: 994
	[StaticConstructorOnStartup]
	public class Graphic_Mote : Graphic_Single
	{
		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001C4B RID: 7243 RVA: 0x0000249D File Offset: 0x0000069D
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000ACE06 File Offset: 0x000AB006
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000ACE14 File Offset: 0x000AB014
		public void DrawMoteInternal(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, int layer)
		{
			Graphic_Mote.DrawMote(this.data, this.MatSingle, base.Color, loc, rot, thingDef, thing, 0, this.ForcePropertyBlock, null);
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000ACE48 File Offset: 0x000AB048
		public static void DrawMote(GraphicData data, Material material, Color color, Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, int layer, bool forcePropertyBlock = false, MaterialPropertyBlock overridePropertyBlock = null)
		{
			Mote mote = (Mote)thing;
			float alpha = mote.Alpha;
			if (alpha <= 0f)
			{
				return;
			}
			Color color2 = color * mote.instanceColor;
			color2.a *= alpha;
			Vector3 exactScale = mote.exactScale;
			exactScale.x *= data.drawSize.x;
			exactScale.z *= data.drawSize.y;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(mote.DrawPos, Quaternion.AngleAxis(mote.exactRotation, Vector3.up), exactScale);
			if (!forcePropertyBlock && color2.IndistinguishableFrom(material.color))
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0);
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, color2);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, overridePropertyBlock ?? Graphic_Mote.propertyBlock);
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000ACF38 File Offset: 0x000AB138
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Mote(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}

		// Token: 0x0400143F RID: 5183
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}

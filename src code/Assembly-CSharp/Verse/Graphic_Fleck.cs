using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D8 RID: 984
	[StaticConstructorOnStartup]
	public class Graphic_Fleck : Graphic_Single
	{
		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x00002662 File Offset: 0x00000862
		protected virtual bool AllowInstancing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x0003120D File Offset: 0x0002F40D
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000ABE80 File Offset: 0x000AA080
		public virtual void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			Color value;
			if (drawData.overrideColor != null)
			{
				value = drawData.overrideColor.Value;
			}
			else
			{
				float alpha = drawData.alpha;
				if (alpha <= 0f)
				{
					if (drawData.propertyBlock != null)
					{
						batch.ReturnPropertyBlock(drawData.propertyBlock);
					}
					return;
				}
				value = base.Color * drawData.color;
				value.a *= alpha;
			}
			Vector3 scale = drawData.scale;
			scale.x *= this.data.drawSize.x;
			scale.z *= this.data.drawSize.y;
			Mesh mesh = MeshPool.plane10;
			float num = drawData.rotation;
			if (scale.x < 0f && scale.y >= 0f)
			{
				scale.x = -scale.x;
				mesh = MeshPool.plane10Flip;
			}
			else if (scale.x >= 0f && scale.y < 0f)
			{
				scale.y = -scale.y;
				mesh = MeshPool.plane10Flip;
				num += 180f;
			}
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawData.pos, Quaternion.AngleAxis(num, Vector3.up), scale);
			Material matSingle = this.MatSingle;
			batch.DrawMesh(mesh, matrix, matSingle, drawData.drawLayer, new Color?(value), this.data.renderInstanced && this.AllowInstancing, drawData.propertyBlock);
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000ABFFC File Offset: 0x000AA1FC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Fleck(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}
	}
}

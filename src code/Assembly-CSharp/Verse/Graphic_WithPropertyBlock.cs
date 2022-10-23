using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EE RID: 1006
	public abstract class Graphic_WithPropertyBlock : Graphic_Single
	{
		// Token: 0x06001CA9 RID: 7337 RVA: 0x000AE4FC File Offset: 0x000AC6FC
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(loc, quat, new Vector3(this.drawSize.x, 1f, this.drawSize.y)), mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x04001456 RID: 5206
		protected MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}

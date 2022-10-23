using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000200 RID: 512
	public class LayerSubMesh
	{
		// Token: 0x06000EE1 RID: 3809 RVA: 0x00052B04 File Offset: 0x00050D04
		public LayerSubMesh(Mesh mesh, Material material, Bounds? bounds = null)
		{
			this.mesh = mesh;
			this.material = material;
			this.bounds = bounds;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00052B64 File Offset: 0x00050D64
		public void Clear(MeshParts parts)
		{
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				this.verts.Clear();
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				this.tris.Clear();
			}
			if ((parts & MeshParts.Colors) != MeshParts.None)
			{
				this.colors.Clear();
			}
			if ((parts & MeshParts.UVs) != MeshParts.None)
			{
				this.uvs.Clear();
			}
			this.finalized = false;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00052BB8 File Offset: 0x00050DB8
		public void FinalizeMesh(MeshParts parts)
		{
			if (this.finalized)
			{
				Log.Warning("Finalizing mesh which is already finalized. Did you forget to call Clear()?");
			}
			if ((parts & MeshParts.Verts) != MeshParts.None || (parts & MeshParts.Tris) != MeshParts.None)
			{
				this.mesh.Clear();
			}
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				if (this.verts.Count > 0)
				{
					this.mesh.SetVertices(this.verts);
				}
				else
				{
					Log.Error("Cannot cook Verts for " + this.material.ToString() + ": no ingredients data. If you want to not render this submesh, disable it.");
				}
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				if (this.tris.Count > 0)
				{
					this.mesh.SetTriangles(this.tris, 0);
				}
				else
				{
					Log.Error("Cannot cook Tris for " + this.material.ToString() + ": no ingredients data.");
				}
			}
			if ((parts & MeshParts.Colors) != MeshParts.None && this.colors.Count > 0)
			{
				this.mesh.SetColors(this.colors);
			}
			if ((parts & MeshParts.UVs) != MeshParts.None && this.uvs.Count > 0)
			{
				this.mesh.SetUVs(0, this.uvs);
			}
			if (this.bounds != null)
			{
				this.mesh.bounds = this.bounds.Value;
			}
			this.finalized = true;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00052CEA File Offset: 0x00050EEA
		public override string ToString()
		{
			return "LayerSubMesh(" + this.material.ToString() + ")";
		}

		// Token: 0x04000D57 RID: 3415
		public bool finalized;

		// Token: 0x04000D58 RID: 3416
		public bool disabled;

		// Token: 0x04000D59 RID: 3417
		private Bounds? bounds;

		// Token: 0x04000D5A RID: 3418
		public Material material;

		// Token: 0x04000D5B RID: 3419
		public Mesh mesh;

		// Token: 0x04000D5C RID: 3420
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x04000D5D RID: 3421
		public List<int> tris = new List<int>();

		// Token: 0x04000D5E RID: 3422
		public List<Color32> colors = new List<Color32>();

		// Token: 0x04000D5F RID: 3423
		public List<Vector3> pollution = new List<Vector3>();

		// Token: 0x04000D60 RID: 3424
		public List<Vector3> uvs = new List<Vector3>();
	}
}

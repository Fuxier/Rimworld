using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000394 RID: 916
	public class GraphicMeshSet
	{
		// Token: 0x06001A47 RID: 6727 RVA: 0x0009EA48 File Offset: 0x0009CC48
		public GraphicMeshSet(Mesh normalMesh, Mesh leftMesh)
		{
			Mesh[] array = this.meshes;
			int num = 0;
			Mesh[] array2 = this.meshes;
			int num2 = 1;
			this.meshes[2] = normalMesh;
			array[num] = (array2[num2] = normalMesh);
			this.meshes[3] = leftMesh;
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0009EA90 File Offset: 0x0009CC90
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0009EAE8 File Offset: 0x0009CCE8
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0009EB48 File Offset: 0x0009CD48
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}

		// Token: 0x04001322 RID: 4898
		private Mesh[] meshes = new Mesh[4];
	}
}

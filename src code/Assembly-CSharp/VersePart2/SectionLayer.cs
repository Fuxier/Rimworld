using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000205 RID: 517
	public abstract class SectionLayer
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x00053682 File Offset: 0x00051882
		protected Map Map
		{
			get
			{
				return this.section.map;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0005368F File Offset: 0x0005188F
		public SectionLayer(Section section)
		{
			this.section = section;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x000536AC File Offset: 0x000518AC
		public LayerSubMesh GetSubMesh(Material material)
		{
			if (material == null)
			{
				return null;
			}
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].material == material)
				{
					return this.subMeshes[i];
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = string.Concat(new object[]
				{
					"SectionLayerSubMesh_",
					base.GetType().Name,
					"_",
					this.Map.Tile
				});
			}
			Bounds value = new Bounds(this.section.botLeft.ToVector3(), Vector3.zero);
			value.Encapsulate(this.section.botLeft.ToVector3() + new Vector3(17f, 0f, 0f));
			value.Encapsulate(this.section.botLeft.ToVector3() + new Vector3(17f, 0f, 17f));
			value.Encapsulate(this.section.botLeft.ToVector3() + new Vector3(0f, 0f, 17f));
			LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material, new Bounds?(value));
			this.subMeshes.Add(layerSubMesh);
			return layerSubMesh;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00053818 File Offset: 0x00051A18
		protected void FinalizeMesh(MeshParts tags)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].verts.Count > 0)
				{
					this.subMeshes[i].FinalizeMesh(tags);
				}
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00053868 File Offset: 0x00051A68
		public virtual void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.identity, layerSubMesh.material, 0);
				}
			}
		}

		// Token: 0x06000EFF RID: 3839
		public abstract void Regenerate();

		// Token: 0x06000F00 RID: 3840 RVA: 0x000538CC File Offset: 0x00051ACC
		protected void ClearSubMeshes(MeshParts parts)
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				layerSubMesh.Clear(parts);
			}
		}

		// Token: 0x04000D7A RID: 3450
		protected Section section;

		// Token: 0x04000D7B RID: 3451
		public MapMeshFlag relevantChangeTypes;

		// Token: 0x04000D7C RID: 3452
		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();
	}
}

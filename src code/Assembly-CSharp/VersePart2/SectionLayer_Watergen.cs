using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200021B RID: 539
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x06000F64 RID: 3940 RVA: 0x00059374 File Offset: 0x00057574
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00059385 File Offset: 0x00057585
		public override Material GetMaterialFor(CellTerrain terrain)
		{
			return terrain.def.waterDepthMaterial;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00059394 File Offset: 0x00057594
		public override void DrawLayer()
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
					Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.identity, layerSubMesh.material, SubcameraDefOf.WaterDepth.LayerId);
				}
			}
		}
	}
}

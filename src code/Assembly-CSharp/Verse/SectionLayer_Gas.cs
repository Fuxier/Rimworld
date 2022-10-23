using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200020C RID: 524
	[StaticConstructorOnStartup]
	public class SectionLayer_Gas : SectionLayer
	{
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x00056066 File Offset: 0x00054266
		protected virtual FloatRange VertexScaleOffsetRange
		{
			get
			{
				return new FloatRange(0.4f, 0.6f);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00056077 File Offset: 0x00054277
		protected virtual FloatRange VertexPositionOffsetRange
		{
			get
			{
				return new FloatRange(-0.2f, 0.2f);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x00056088 File Offset: 0x00054288
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawGas;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0005608F File Offset: 0x0005428F
		public virtual Material Mat
		{
			get
			{
				return SectionLayer_Gas.GasMat;
			}
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00056096 File Offset: 0x00054296
		public SectionLayer_Gas(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Gas;
			this.propertyBlock = new MaterialPropertyBlock();
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x000560B8 File Offset: 0x000542B8
		public override void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			this.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, RealTime.UnpausedRealTime);
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, 0, null, 0, this.propertyBlock);
				}
			}
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0005613C File Offset: 0x0005433C
		public virtual Color ColorAt(IntVec3 cell)
		{
			return base.Map.gasGrid.ColorAt(cell);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00056150 File Offset: 0x00054350
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			LayerSubMesh subMesh = base.GetSubMesh(this.Mat);
			float altitude = AltitudeLayer.Gas.AltitudeFor();
			int num = this.section.botLeft.x;
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				if (base.Map.gasGrid.AnyGasAt(intVec))
				{
					int count = subMesh.verts.Count;
					this.AddCell(intVec, num, count, subMesh, altitude);
				}
				num++;
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.All);
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0005621C File Offset: 0x0005441C
		protected void AddCell(IntVec3 c, int index, int startVertIndex, LayerSubMesh sm, float altitude)
		{
			Rand.PushState(index);
			Color c2 = this.ColorAt(c);
			float randomInRange = this.VertexScaleOffsetRange.RandomInRange;
			float randomInRange2 = this.VertexPositionOffsetRange.RandomInRange;
			float randomInRange3 = this.VertexPositionOffsetRange.RandomInRange;
			float x = (float)c.x - randomInRange + randomInRange2;
			float x2 = (float)(c.x + 1) + randomInRange + randomInRange2;
			float z = (float)c.z - randomInRange + randomInRange3;
			float z2 = (float)(c.z + 1) + randomInRange + randomInRange3;
			float y = altitude + Rand.Range(-0.01f, 0.01f);
			sm.verts.Add(new Vector3(x, y, z));
			sm.verts.Add(new Vector3(x, y, z2));
			sm.verts.Add(new Vector3(x2, y, z2));
			sm.verts.Add(new Vector3(x2, y, z));
			sm.uvs.Add(new Vector3(0f, 0f, (float)index));
			sm.uvs.Add(new Vector3(0f, 1f, (float)index));
			sm.uvs.Add(new Vector3(1f, 1f, (float)index));
			sm.uvs.Add(new Vector3(1f, 0f, (float)index));
			sm.colors.Add(c2);
			sm.colors.Add(c2);
			sm.colors.Add(c2);
			sm.colors.Add(c2);
			sm.tris.Add(startVertIndex);
			sm.tris.Add(startVertIndex + 1);
			sm.tris.Add(startVertIndex + 2);
			sm.tris.Add(startVertIndex);
			sm.tris.Add(startVertIndex + 2);
			sm.tris.Add(startVertIndex + 3);
			Rand.PopState();
		}

		// Token: 0x04000D92 RID: 3474
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x04000D93 RID: 3475
		private static Material GasMat = MaterialPool.MatFrom("Things/Gas/GasCloudThickA", ShaderDatabase.GasRotating, 3000);
	}
}

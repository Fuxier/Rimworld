using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B3 RID: 435
	public class CellBoolDrawer
	{
		// Token: 0x06000C3F RID: 3135 RVA: 0x000443E0 File Offset: 0x000425E0
		private CellBoolDrawer(int mapSizeX, int mapSizeZ, float opacity = 0.33f)
		{
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
			this.opacity = opacity;
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00044430 File Offset: 0x00042630
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = (() => giver.Color);
			this.extraColorGetter = new Func<int, Color>(giver.GetCellExtraColor);
			this.cellBoolGetter = new Func<int, bool>(giver.GetCellBool);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00044496 File Offset: 0x00042696
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(giver, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x000444AB File Offset: 0x000426AB
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = colorGetter;
			this.extraColorGetter = extraColorGetter;
			this.cellBoolGetter = cellBoolGetter;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x000444CE File Offset: 0x000426CE
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(cellBoolGetter, colorGetter, extraColorGetter, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x000444E7 File Offset: 0x000426E7
		public void MarkForDraw()
		{
			this.wantDraw = true;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x000444F0 File Offset: 0x000426F0
		public void CellBoolDrawerUpdate()
		{
			if (this.wantDraw)
			{
				this.ActuallyDraw();
				this.wantDraw = false;
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00044508 File Offset: 0x00042708
		private void ActuallyDraw()
		{
			if (this.dirty)
			{
				this.RegenerateMesh();
			}
			for (int i = 0; i < this.meshes.Count; i++)
			{
				Graphics.DrawMesh(this.meshes[i], Vector3.zero, Quaternion.identity, this.material, 0);
			}
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0004455B File Offset: 0x0004275B
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00044564 File Offset: 0x00042764
		public void RegenerateMesh()
		{
			for (int i = 0; i < this.meshes.Count; i++)
			{
				this.meshes[i].Clear();
			}
			int num = 0;
			int num2 = 0;
			if (this.meshes.Count < num + 1)
			{
				Mesh mesh = new Mesh();
				mesh.name = "CellBoolDrawer";
				this.meshes.Add(mesh);
			}
			Mesh mesh2 = this.meshes[num];
			CellRect cellRect = new CellRect(0, 0, this.mapSizeX, this.mapSizeZ);
			float y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			bool careAboutVertexColors = false;
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
				{
					int arg = CellIndicesUtility.CellToIndex(j, k, this.mapSizeX);
					if (this.cellBoolGetter(arg))
					{
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)k));
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)k));
						Color color = this.extraColorGetter(arg);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						if (color != Color.white)
						{
							careAboutVertexColors = true;
						}
						int count = CellBoolDrawer.verts.Count;
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 3);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 1);
						num2++;
						if (num2 >= 16383)
						{
							this.FinalizeWorkingDataIntoMesh(mesh2);
							num++;
							if (this.meshes.Count < num + 1)
							{
								Mesh mesh3 = new Mesh();
								mesh3.name = "CellBoolDrawer";
								this.meshes.Add(mesh3);
							}
							mesh2 = this.meshes[num];
							num2 = 0;
						}
					}
				}
			}
			this.FinalizeWorkingDataIntoMesh(mesh2);
			this.CreateMaterialIfNeeded(careAboutVertexColors);
			this.dirty = false;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x000447E4 File Offset: 0x000429E4
		private void FinalizeWorkingDataIntoMesh(Mesh mesh)
		{
			if (CellBoolDrawer.verts.Count > 0)
			{
				mesh.SetVertices(CellBoolDrawer.verts);
				CellBoolDrawer.verts.Clear();
				mesh.SetTriangles(CellBoolDrawer.tris, 0);
				CellBoolDrawer.tris.Clear();
				mesh.SetColors(CellBoolDrawer.colors);
				CellBoolDrawer.colors.Clear();
			}
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00044840 File Offset: 0x00042A40
		private void CreateMaterialIfNeeded(bool careAboutVertexColors)
		{
			if (this.material == null || this.materialCaresAboutVertexColors != careAboutVertexColors)
			{
				Color color = this.colorGetter();
				this.material = SolidColorMaterials.SimpleSolidColorMaterial(new Color(color.r, color.g, color.b, this.opacity * color.a), careAboutVertexColors);
				this.materialCaresAboutVertexColors = careAboutVertexColors;
				this.material.renderQueue = this.renderQueue;
			}
		}

		// Token: 0x04000B30 RID: 2864
		private bool wantDraw;

		// Token: 0x04000B31 RID: 2865
		private Material material;

		// Token: 0x04000B32 RID: 2866
		private bool materialCaresAboutVertexColors;

		// Token: 0x04000B33 RID: 2867
		private bool dirty = true;

		// Token: 0x04000B34 RID: 2868
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04000B35 RID: 2869
		private int mapSizeX;

		// Token: 0x04000B36 RID: 2870
		private int mapSizeZ;

		// Token: 0x04000B37 RID: 2871
		private float opacity = 0.33f;

		// Token: 0x04000B38 RID: 2872
		private int renderQueue = 3600;

		// Token: 0x04000B39 RID: 2873
		private Func<Color> colorGetter;

		// Token: 0x04000B3A RID: 2874
		private Func<int, Color> extraColorGetter;

		// Token: 0x04000B3B RID: 2875
		private Func<int, bool> cellBoolGetter;

		// Token: 0x04000B3C RID: 2876
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x04000B3D RID: 2877
		private static List<int> tris = new List<int>();

		// Token: 0x04000B3E RID: 2878
		private static List<Color> colors = new List<Color>();

		// Token: 0x04000B3F RID: 2879
		private const float DefaultOpacity = 0.33f;

		// Token: 0x04000B40 RID: 2880
		private const int MaxCellsPerMesh = 16383;
	}
}

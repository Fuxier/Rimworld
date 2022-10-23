using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000213 RID: 531
	internal class SectionLayer_Terrain : SectionLayer
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x00057DF9 File Offset: 0x00055FF9
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00057E00 File Offset: 0x00056000
		public SectionLayer_Terrain(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00057E14 File Offset: 0x00056014
		public virtual Material GetMaterialFor(CellTerrain cellTerrain)
		{
			bool polluted = cellTerrain.polluted && cellTerrain.snowCoverage < 0.4f && cellTerrain.def.graphicPolluted != BaseContent.BadGraphic;
			return base.Map.terrainGrid.GetMaterial(cellTerrain.def, polluted, cellTerrain.color);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00057E6C File Offset: 0x0005606C
		public bool AllowRenderingFor(TerrainDef terrain)
		{
			return DebugViewSettings.drawTerrainWater || !terrain.HasTag("Water");
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00057E88 File Offset: 0x00056088
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			TerrainGrid terrainGrid = base.Map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			CellTerrain[] array = new CellTerrain[8];
			HashSet<CellTerrain> hashSet = new HashSet<CellTerrain>();
			bool[] array2 = new bool[8];
			foreach (IntVec3 intVec in cellRect)
			{
				hashSet.Clear();
				CellTerrain cellTerrain = new CellTerrain(terrainGrid.TerrainAt(intVec), intVec.IsPolluted(base.Map), base.Map.snowGrid.GetDepth(intVec), terrainGrid.ColorAt(intVec));
				LayerSubMesh subMesh = base.GetSubMesh(this.GetMaterialFor(cellTerrain));
				if (subMesh != null && this.AllowRenderingFor(cellTerrain.def))
				{
					int count = subMesh.verts.Count;
					subMesh.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)intVec.x, 0f, (float)(intVec.z + 1)));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)(intVec.z + 1)));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z));
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 1);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count + 3);
				}
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = intVec + GenAdj.AdjacentCellsAroundBottom[i];
					if (!c.InBounds(base.Map))
					{
						array[i] = cellTerrain;
					}
					else
					{
						CellTerrain cellTerrain2 = new CellTerrain(terrainGrid.TerrainAt(c), c.IsPolluted(base.Map), base.Map.snowGrid.GetDepth(c), terrainGrid.ColorAt(c));
						Thing edifice = c.GetEdifice(base.Map);
						if (edifice != null && edifice.def.coversFloor)
						{
							cellTerrain2.def = TerrainDefOf.Underwall;
						}
						array[i] = cellTerrain2;
						if (!cellTerrain2.Equals(cellTerrain) && cellTerrain2.def.edgeType != TerrainDef.TerrainEdgeType.Hard && cellTerrain2.def.renderPrecedence >= cellTerrain.def.renderPrecedence && !hashSet.Contains(cellTerrain2))
						{
							hashSet.Add(cellTerrain2);
						}
					}
				}
				foreach (CellTerrain cellTerrain3 in hashSet)
				{
					LayerSubMesh subMesh2 = base.GetSubMesh(this.GetMaterialFor(cellTerrain3));
					if (subMesh2 != null && this.AllowRenderingFor(cellTerrain3.def))
					{
						int count = subMesh2.verts.Count;
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z + 0.5f));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z + 0.5f));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)intVec.z + 0.5f));
						for (int j = 0; j < 8; j++)
						{
							array2[j] = false;
						}
						for (int k = 0; k < 8; k++)
						{
							if (k % 2 == 0)
							{
								if (array[k].Equals(cellTerrain3))
								{
									array2[(k - 1 + 8) % 8] = true;
									array2[k] = true;
									array2[(k + 1) % 8] = true;
								}
							}
							else if (array[k].Equals(cellTerrain3))
							{
								array2[k] = true;
							}
						}
						for (int l = 0; l < 8; l++)
						{
							if (array2[l])
							{
								subMesh2.colors.Add(SectionLayer_Terrain.ColorWhite);
							}
							else
							{
								subMesh2.colors.Add(SectionLayer_Terrain.ColorClear);
							}
						}
						subMesh2.colors.Add(SectionLayer_Terrain.ColorClear);
						for (int m = 0; m < 8; m++)
						{
							subMesh2.tris.Add(count + m);
							subMesh2.tris.Add(count + (m + 1) % 8);
							subMesh2.tris.Add(count + 8);
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x04000DAD RID: 3501
		private static readonly Color32 ColorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000DAE RID: 3502
		private static readonly Color32 ColorClear = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);

		// Token: 0x04000DAF RID: 3503
		public const float MaxSnowCoverageForVisualPollution = 0.4f;
	}
}

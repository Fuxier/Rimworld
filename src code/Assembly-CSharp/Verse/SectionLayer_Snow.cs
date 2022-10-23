using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000210 RID: 528
	[StaticConstructorOnStartup]
	internal class SectionLayer_Snow : SectionLayer
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x000572E3 File Offset: 0x000554E3
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawSnow;
			}
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x000572EA File Offset: 0x000554EA
		public SectionLayer_Snow(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Snow;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00057308 File Offset: 0x00055508
		private bool Filled(int index)
		{
			Building building = base.Map.edificeGrid[index];
			return building != null && building.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0005733C File Offset: 0x0005553C
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.Snow);
			if (ModsConfig.BiotechActive)
			{
				subMesh.material.SetTexture("_PollutedTex", SectionLayer_Snow.PollutedSnowTex.Texture);
			}
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.Terrain);
			}
			SectionLayer_Snow.opacityListTmp.Clear();
			subMesh.Clear(MeshParts.Colors);
			float[] depthGridDirect_Unsafe = base.Map.snowGrid.DepthGridDirect_Unsafe;
			CellRect cellRect = this.section.CellRect;
			bool flag = false;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					SectionLayer_Snow.opacityListTmp.Clear();
					float num = depthGridDirect_Unsafe[cellIndices.CellToIndex(i, j)];
					for (int k = 0; k < 9; k++)
					{
						IntVec3 c = new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[k];
						this.adjValuesTmp[k] = (c.InBounds(base.Map) ? depthGridDirect_Unsafe[cellIndices.CellToIndex(c)] : num);
					}
					for (int l = 0; l < 9; l++)
					{
						float num2 = 0f;
						for (int m = 0; m < SectionLayer_Snow.vertexWeights[l].Count; m++)
						{
							num2 += this.adjValuesTmp[SectionLayer_Snow.vertexWeights[l][m]];
						}
						num2 /= (float)SectionLayer_Snow.vertexWeights[l].Count;
						if (num2 > 0.01f)
						{
							flag = true;
						}
						SectionLayer_Snow.opacityListTmp.Add(num2);
					}
					for (int n = 0; n < 9; n++)
					{
						this.adjValuesTmp[n] = (base.Map.pollutionGrid.IsPolluted(new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[n]) ? 1f : 0f);
					}
					for (int num3 = 0; num3 < 9; num3++)
					{
						float num4 = 0f;
						for (int num5 = 0; num5 < SectionLayer_Snow.vertexWeights[num3].Count; num5++)
						{
							num4 += this.adjValuesTmp[SectionLayer_Snow.vertexWeights[num3][num5]];
						}
						num4 /= (float)SectionLayer_Snow.vertexWeights[num3].Count;
						float num6 = SectionLayer_Snow.opacityListTmp[num3];
						subMesh.colors.Add(new Color32(Convert.ToByte(num4 * 255f), byte.MaxValue, byte.MaxValue, Convert.ToByte(num6 * 255f)));
					}
				}
			}
			if (flag)
			{
				subMesh.disabled = false;
				subMesh.FinalizeMesh(MeshParts.Colors);
				return;
			}
			subMesh.disabled = true;
		}

		// Token: 0x04000DA4 RID: 3492
		private float[] adjValuesTmp = new float[9];

		// Token: 0x04000DA5 RID: 3493
		private static readonly List<float> opacityListTmp = new List<float>();

		// Token: 0x04000DA6 RID: 3494
		private static readonly CachedTexture PollutedSnowTex = new CachedTexture("Other/SnowPolluted");

		// Token: 0x04000DA7 RID: 3495
		public static readonly List<List<int>> vertexWeights = new List<List<int>>
		{
			new List<int>
			{
				0,
				1,
				2,
				8
			},
			new List<int>
			{
				2,
				8
			},
			new List<int>
			{
				2,
				3,
				4,
				8
			},
			new List<int>
			{
				4,
				8
			},
			new List<int>
			{
				4,
				5,
				6,
				8
			},
			new List<int>
			{
				6,
				8
			},
			new List<int>
			{
				6,
				7,
				0,
				8
			},
			new List<int>
			{
				0,
				8
			},
			new List<int>
			{
				8
			}
		};
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200020B RID: 523
	public class SectionLayer_FogOfWar : SectionLayer
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00055D63 File Offset: 0x00053F63
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawFog;
			}
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00055D6A File Offset: 0x00053F6A
		public SectionLayer_FogOfWar(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.FogOfWar;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00055D88 File Offset: 0x00053F88
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.FogOfWar);
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.FogOfWar);
			}
			subMesh.Clear(MeshParts.Colors);
			bool[] fogGrid = base.Map.fogGrid.fogGrid;
			CellRect cellRect = this.section.CellRect;
			int num = base.Map.Size.z - 1;
			int num2 = base.Map.Size.x - 1;
			bool flag = false;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					if (fogGrid[cellIndices.CellToIndex(i, j)])
					{
						for (int k = 0; k < 9; k++)
						{
							this.vertsCovered[k] = true;
						}
					}
					else
					{
						for (int l = 0; l < 9; l++)
						{
							this.vertsCovered[l] = false;
						}
						if (j < num && fogGrid[cellIndices.CellToIndex(i, j + 1)])
						{
							this.vertsCovered[2] = true;
							this.vertsCovered[3] = true;
							this.vertsCovered[4] = true;
						}
						if (j > 0 && fogGrid[cellIndices.CellToIndex(i, j - 1)])
						{
							this.vertsCovered[6] = true;
							this.vertsCovered[7] = true;
							this.vertsCovered[0] = true;
						}
						if (i < num2 && fogGrid[cellIndices.CellToIndex(i + 1, j)])
						{
							this.vertsCovered[4] = true;
							this.vertsCovered[5] = true;
							this.vertsCovered[6] = true;
						}
						if (i > 0 && fogGrid[cellIndices.CellToIndex(i - 1, j)])
						{
							this.vertsCovered[0] = true;
							this.vertsCovered[1] = true;
							this.vertsCovered[2] = true;
						}
						if (j > 0 && i > 0 && fogGrid[cellIndices.CellToIndex(i - 1, j - 1)])
						{
							this.vertsCovered[0] = true;
						}
						if (j < num && i > 0 && fogGrid[cellIndices.CellToIndex(i - 1, j + 1)])
						{
							this.vertsCovered[2] = true;
						}
						if (j < num && i < num2 && fogGrid[cellIndices.CellToIndex(i + 1, j + 1)])
						{
							this.vertsCovered[4] = true;
						}
						if (j > 0 && i < num2 && fogGrid[cellIndices.CellToIndex(i + 1, j - 1)])
						{
							this.vertsCovered[6] = true;
						}
					}
					for (int m = 0; m < 9; m++)
					{
						byte a;
						if (this.vertsCovered[m])
						{
							a = byte.MaxValue;
							flag = true;
						}
						else
						{
							a = 0;
						}
						subMesh.colors.Add(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, a));
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

		// Token: 0x04000D90 RID: 3472
		private bool[] vertsCovered = new bool[9];

		// Token: 0x04000D91 RID: 3473
		private const byte FogBrightness = 35;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000214 RID: 532
	public class SectionLayer_TerrainScatter : SectionLayer
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x00057DF9 File Offset: 0x00055FF9
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00058512 File Offset: 0x00056712
		public SectionLayer_TerrainScatter(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00058530 File Offset: 0x00056730
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			this.scats.RemoveAll((SectionLayer_TerrainScatter.Scatterable scat) => !scat.IsOnValidTerrain);
			int num = 0;
			TerrainDef[] topGrid = base.Map.terrainGrid.topGrid;
			CellRect cellRect = this.section.CellRect;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (topGrid[cellIndices.CellToIndex(j, i)].scatterType != null)
					{
						num++;
					}
				}
			}
			num /= 40;
			int num2 = 0;
			while (this.scats.Count < num && num2 < 200)
			{
				num2++;
				IntVec3 randomCell = this.section.CellRect.RandomCell;
				string terrScatType = base.Map.terrainGrid.TerrainAt(randomCell).scatterType;
				ScatterableDef def2;
				if (terrScatType != null && !randomCell.Filled(base.Map) && (from def in DefDatabase<ScatterableDef>.AllDefs
				where def.scatterType == terrScatType
				select def).TryRandomElement(out def2))
				{
					Vector3 loc = new Vector3((float)randomCell.x + Rand.Value, (float)randomCell.y, (float)randomCell.z + Rand.Value);
					SectionLayer_TerrainScatter.Scatterable scatterable = new SectionLayer_TerrainScatter.Scatterable(def2, loc, base.Map);
					this.scats.Add(scatterable);
					scatterable.PrintOnto(this);
				}
			}
			for (int k = 0; k < this.scats.Count; k++)
			{
				this.scats[k].PrintOnto(this);
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x04000DB0 RID: 3504
		private List<SectionLayer_TerrainScatter.Scatterable> scats = new List<SectionLayer_TerrainScatter.Scatterable>();

		// Token: 0x02001D78 RID: 7544
		private class Scatterable
		{
			// Token: 0x0600B49F RID: 46239 RVA: 0x00411654 File Offset: 0x0040F854
			public Scatterable(ScatterableDef def, Vector3 loc, Map map)
			{
				this.def = def;
				this.loc = loc;
				this.map = map;
				this.size = Rand.Range(def.minSize, def.maxSize);
				this.rotation = Rand.Range(0f, 360f);
			}

			// Token: 0x0600B4A0 RID: 46240 RVA: 0x004116A8 File Offset: 0x0040F8A8
			public void PrintOnto(SectionLayer layer)
			{
				Material mat = this.def.mat;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Terrain, false, false, out mat, out uvs, out color);
				Printer_Plane.PrintPlane(layer, this.loc, Vector2.one * this.size, mat, this.rotation, false, uvs, null, 0.01f, 0f);
			}

			// Token: 0x17001E48 RID: 7752
			// (get) Token: 0x0600B4A1 RID: 46241 RVA: 0x00411704 File Offset: 0x0040F904
			public bool IsOnValidTerrain
			{
				get
				{
					IntVec3 intVec = this.loc.ToIntVec3();
					if (this.def.scatterType != this.map.terrainGrid.TerrainAt(intVec).scatterType || intVec.Filled(this.map))
					{
						return false;
					}
					foreach (IntVec3 c in CellRect.CenteredOn(intVec, Mathf.FloorToInt(this.size)).ClipInsideMap(this.map))
					{
						if (this.map.terrainGrid.TerrainAt(c).IsFloor)
						{
							return false;
						}
					}
					return true;
				}
			}

			// Token: 0x0400745F RID: 29791
			private Map map;

			// Token: 0x04007460 RID: 29792
			public ScatterableDef def;

			// Token: 0x04007461 RID: 29793
			public Vector3 loc;

			// Token: 0x04007462 RID: 29794
			public float size;

			// Token: 0x04007463 RID: 29795
			public float rotation;
		}
	}
}

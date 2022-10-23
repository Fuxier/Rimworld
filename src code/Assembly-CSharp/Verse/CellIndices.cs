using System;

namespace Verse
{
	// Token: 0x02000513 RID: 1299
	public class CellIndices
	{
		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060027B4 RID: 10164 RVA: 0x00102476 File Offset: 0x00100676
		public int NumGridCells
		{
			get
			{
				return this.mapSizeX * this.mapSizeZ;
			}
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x00102485 File Offset: 0x00100685
		public CellIndices(Map map)
		{
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x001024AF File Offset: 0x001006AF
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.mapSizeX);
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x001024BD File Offset: 0x001006BD
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x001024CC File Offset: 0x001006CC
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.mapSizeX);
		}

		// Token: 0x04001A01 RID: 6657
		private int mapSizeX;

		// Token: 0x04001A02 RID: 6658
		private int mapSizeZ;
	}
}

using System;

namespace Verse
{
	// Token: 0x02000514 RID: 1300
	public static class CellIndicesUtility
	{
		// Token: 0x060027B9 RID: 10169 RVA: 0x001024DA File Offset: 0x001006DA
		public static int CellToIndex(IntVec3 c, int mapSizeX)
		{
			return c.z * mapSizeX + c.x;
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x001024EB File Offset: 0x001006EB
		public static int CellToIndex(int x, int z, int mapSizeX)
		{
			return z * mapSizeX + x;
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x001024F4 File Offset: 0x001006F4
		public static IntVec3 IndexToCell(int ind, int mapSizeX)
		{
			int newX = ind % mapSizeX;
			int newZ = ind / mapSizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}

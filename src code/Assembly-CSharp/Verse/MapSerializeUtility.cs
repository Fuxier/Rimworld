using System;

namespace Verse
{
	// Token: 0x02000258 RID: 600
	public static class MapSerializeUtility
	{
		// Token: 0x06001126 RID: 4390 RVA: 0x000641C4 File Offset: 0x000623C4
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00064208 File Offset: 0x00062408
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0006424C File Offset: 0x0006244C
		public static byte[] SerializeInt(Map map, Func<IntVec3, int> intReader)
		{
			return DataSerializeUtility.SerializeInt(map.info.NumCells, (int idx) => intReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00064290 File Offset: 0x00062490
		public static void LoadInt(byte[] arr, Map map, Action<IntVec3, int> intWriter)
		{
			DataSerializeUtility.LoadInt(arr, map.info.NumCells, delegate(int idx, int data)
			{
				intWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}

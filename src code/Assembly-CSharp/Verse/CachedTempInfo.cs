using System;

namespace Verse
{
	// Token: 0x0200025E RID: 606
	public struct CachedTempInfo
	{
		// Token: 0x06001163 RID: 4451 RVA: 0x000658E0 File Offset: 0x00063AE0
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x000658FD File Offset: 0x00063AFD
		public void Reset()
		{
			this.roomID = -1;
			this.numCells = 0;
			this.temperature = 0f;
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00065918 File Offset: 0x00063B18
		public CachedTempInfo(int roomID, int numCells, float temperature)
		{
			this.roomID = roomID;
			this.numCells = numCells;
			this.temperature = temperature;
		}

		// Token: 0x04000ED2 RID: 3794
		public int roomID;

		// Token: 0x04000ED3 RID: 3795
		public int numCells;

		// Token: 0x04000ED4 RID: 3796
		public float temperature;
	}
}

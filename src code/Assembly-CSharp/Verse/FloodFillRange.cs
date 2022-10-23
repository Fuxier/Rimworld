using System;

namespace Verse
{
	// Token: 0x02000233 RID: 563
	public struct FloodFillRange
	{
		// Token: 0x06000FEC RID: 4076 RVA: 0x0005C8FA File Offset: 0x0005AAFA
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}

		// Token: 0x04000E1E RID: 3614
		public int minX;

		// Token: 0x04000E1F RID: 3615
		public int maxX;

		// Token: 0x04000E20 RID: 3616
		public int z;
	}
}

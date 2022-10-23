using System;

namespace Verse
{
	// Token: 0x0200052F RID: 1327
	public struct SurfaceColumn
	{
		// Token: 0x060028B5 RID: 10421 RVA: 0x001066DF File Offset: 0x001048DF
		public SurfaceColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x04001AAB RID: 6827
		public float x;

		// Token: 0x04001AAC RID: 6828
		public SimpleCurve y;
	}
}

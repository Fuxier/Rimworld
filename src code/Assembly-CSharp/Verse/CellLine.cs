using System;

namespace Verse
{
	// Token: 0x02000515 RID: 1301
	public struct CellLine
	{
		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060027BC RID: 10172 RVA: 0x0010250F File Offset: 0x0010070F
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x060027BD RID: 10173 RVA: 0x00102517 File Offset: 0x00100717
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x0010251F File Offset: 0x0010071F
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x0010252F File Offset: 0x0010072F
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x00102550 File Offset: 0x00100750
		public static CellLine Between(IntVec3 a, IntVec3 b)
		{
			float num;
			if (a.x == b.x)
			{
				num = 100000000f;
			}
			else
			{
				num = (float)(b.z - a.z) / (float)(b.x - a.x);
			}
			return new CellLine((float)a.z - (float)a.x * num, num);
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x001025A8 File Offset: 0x001007A8
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}

		// Token: 0x04001A03 RID: 6659
		private float zIntercept;

		// Token: 0x04001A04 RID: 6660
		private float slope;
	}
}

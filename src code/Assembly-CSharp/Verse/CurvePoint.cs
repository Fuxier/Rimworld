using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000020 RID: 32
	public struct CurvePoint
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00007E82 File Offset: 0x00006082
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00007E8A File Offset: 0x0000608A
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00007E97 File Offset: 0x00006097
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007EA4 File Offset: 0x000060A4
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00007EB3 File Offset: 0x000060B3
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007EBC File Offset: 0x000060BC
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint(ParseHelper.FromString<Vector2>(str));
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00007EC9 File Offset: 0x000060C9
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007E82 File Offset: 0x00006082
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		// Token: 0x04000057 RID: 87
		private Vector2 loc;
	}
}

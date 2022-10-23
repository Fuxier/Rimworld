using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200051D RID: 1309
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x060027D7 RID: 10199 RVA: 0x0010398A File Offset: 0x00101B8A
		public CreditRecord_Space()
		{
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x0010399D File Offset: 0x00101B9D
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x001039B7 File Offset: 0x00101BB7
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000034B7 File Offset: 0x000016B7
		public override void Draw(Rect rect)
		{
		}

		// Token: 0x04001A5E RID: 6750
		private float height = 10f;
	}
}

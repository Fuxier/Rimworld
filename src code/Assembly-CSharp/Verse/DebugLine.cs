using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001BC RID: 444
	internal struct DebugLine
	{
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00045B34 File Offset: 0x00043D34
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00045B4B File Offset: 0x00043D4B
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00045B75 File Offset: 0x00043D75
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color, 0.2f);
		}

		// Token: 0x04000B68 RID: 2920
		public Vector3 a;

		// Token: 0x04000B69 RID: 2921
		public Vector3 b;

		// Token: 0x04000B6A RID: 2922
		private int deathTick;

		// Token: 0x04000B6B RID: 2923
		private SimpleColor color;
	}
}

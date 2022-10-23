using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200051C RID: 1308
	public abstract class CreditsEntry
	{
		// Token: 0x060027D4 RID: 10196
		public abstract float DrawHeight(float width);

		// Token: 0x060027D5 RID: 10197
		public abstract void Draw(Rect rect);
	}
}

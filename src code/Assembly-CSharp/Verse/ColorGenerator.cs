using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B9 RID: 185
	public abstract class ColorGenerator
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0002047E File Offset: 0x0001E67E
		public virtual Color ExemplaryColor
		{
			get
			{
				Rand.PushState(764543439);
				Color result = this.NewRandomizedColor();
				Rand.PopState();
				return result;
			}
		}

		// Token: 0x060005F0 RID: 1520
		public abstract Color NewRandomizedColor();
	}
}

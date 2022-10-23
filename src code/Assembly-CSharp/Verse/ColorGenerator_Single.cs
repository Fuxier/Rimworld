using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000BB RID: 187
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x000204A4 File Offset: 0x0001E6A4
		public override Color NewRandomizedColor()
		{
			return this.color;
		}

		// Token: 0x0400036F RID: 879
		public Color color;
	}
}

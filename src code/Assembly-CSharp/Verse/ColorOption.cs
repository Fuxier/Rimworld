using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000BE RID: 190
	public class ColorOption
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x000206C0 File Offset: 0x0001E8C0
		public Color RandomizedColor()
		{
			if (this.only.a >= 0f)
			{
				return this.only;
			}
			return new Color(Rand.Range(this.min.r, this.max.r), Rand.Range(this.min.g, this.max.g), Rand.Range(this.min.b, this.max.b), Rand.Range(this.min.a, this.max.a));
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00020757 File Offset: 0x0001E957
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00020760 File Offset: 0x0001E960
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00020769 File Offset: 0x0001E969
		public void SetMax(Color color)
		{
			this.max = color;
		}

		// Token: 0x04000372 RID: 882
		public float weight = 1f;

		// Token: 0x04000373 RID: 883
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000374 RID: 884
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000375 RID: 885
		public Color only = new Color(-1f, -1f, -1f, -1f);
	}
}

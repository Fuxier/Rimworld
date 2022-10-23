using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000BD RID: 189
	public class ColorGenerator_Options : ColorGenerator
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00020590 File Offset: 0x0001E790
		public override Color ExemplaryColor
		{
			get
			{
				ColorOption colorOption = null;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (colorOption == null || this.options[i].weight > colorOption.weight)
					{
						colorOption = this.options[i];
					}
				}
				if (colorOption == null)
				{
					return Color.white;
				}
				if (colorOption.only.a >= 0f)
				{
					return colorOption.only;
				}
				return new Color((colorOption.min.r + colorOption.max.r) / 2f, (colorOption.min.g + colorOption.max.g) / 2f, (colorOption.min.b + colorOption.max.b) / 2f, (colorOption.min.a + colorOption.max.a) / 2f);
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00020679 File Offset: 0x0001E879
		public override Color NewRandomizedColor()
		{
			return this.options.RandomElementByWeight((ColorOption pi) => pi.weight).RandomizedColor();
		}

		// Token: 0x04000371 RID: 881
		public List<ColorOption> options = new List<ColorOption>();
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E2 RID: 226
	public struct SkyColorSet
	{
		// Token: 0x06000686 RID: 1670 RVA: 0x00023519 File Offset: 0x00021719
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00023538 File Offset: 0x00021738
		public static SkyColorSet Lerp(SkyColorSet A, SkyColorSet B, float t)
		{
			return new SkyColorSet
			{
				sky = Color.Lerp(A.sky, B.sky, t),
				shadow = Color.Lerp(A.shadow, B.shadow, t),
				overlay = Color.Lerp(A.overlay, B.overlay, t),
				saturation = Mathf.Lerp(A.saturation, B.saturation, t)
			};
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000235B4 File Offset: 0x000217B4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(sky=",
				this.sky,
				", shadow=",
				this.shadow,
				", overlay=",
				this.overlay,
				", sat=",
				this.saturation,
				")"
			});
		}

		// Token: 0x040004AF RID: 1199
		public Color sky;

		// Token: 0x040004B0 RID: 1200
		public Color shadow;

		// Token: 0x040004B1 RID: 1201
		public Color overlay;

		// Token: 0x040004B2 RID: 1202
		public float saturation;
	}
}

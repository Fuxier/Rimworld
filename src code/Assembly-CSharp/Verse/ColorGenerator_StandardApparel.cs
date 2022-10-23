using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000BC RID: 188
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x000204AC File Offset: 0x0001E6AC
		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000204C4 File Offset: 0x0001E6C4
		public override Color NewRandomizedColor()
		{
			if (Rand.Value < 0.1f)
			{
				return Color.white;
			}
			if (Rand.Value < 0.1f)
			{
				return new Color(0.4f, 0.4f, 0.4f);
			}
			Color white = Color.white;
			float num = Rand.Range(0f, 0.6f);
			white.r -= num * Rand.Value;
			white.g -= num * Rand.Value;
			white.b -= num * Rand.Value;
			if (Rand.Value < 0.2f)
			{
				white.r *= 0.4f;
				white.g *= 0.4f;
				white.b *= 0.4f;
			}
			return white;
		}

		// Token: 0x04000370 RID: 880
		private const float DarkAmp = 0.4f;
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000520 RID: 1312
	public class CreditRecord_RoleTwoCols : CreditsEntry
	{
		// Token: 0x060027E4 RID: 10212 RVA: 0x001039BF File Offset: 0x00101BBF
		public CreditRecord_RoleTwoCols()
		{
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x00103B52 File Offset: 0x00101D52
		public CreditRecord_RoleTwoCols(string creditee1, string creditee2, string extra = null)
		{
			this.creditee1 = creditee1;
			this.creditee2 = creditee2;
			this.extra = extra;
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x00103B70 File Offset: 0x00101D70
		public override float DrawHeight(float width)
		{
			float a = Text.CalcHeight(this.creditee1, width * 0.5f);
			float b = Text.CalcHeight(this.creditee2, width * 0.5f);
			if (!this.compressed)
			{
				return 50f;
			}
			return Mathf.Max(a, b);
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x00103BB8 File Offset: 0x00101DB8
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			rect2.width = rect.width / 2f;
			Widgets.Label(rect2, this.creditee1);
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			Widgets.Label(rect3, this.creditee2);
			if (!this.extra.NullOrEmpty())
			{
				Rect rect4 = rect3;
				rect4.yMin += 28f;
				Text.Font = GameFont.Tiny;
				GUI.color = new Color(0.7f, 0.7f, 0.7f);
				Widgets.Label(rect4, this.extra);
				GUI.color = Color.white;
			}
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x00103C72 File Offset: 0x00101E72
		public CreditRecord_RoleTwoCols Compress()
		{
			this.compressed = true;
			return this;
		}

		// Token: 0x04001A66 RID: 6758
		public string creditee1;

		// Token: 0x04001A67 RID: 6759
		public string creditee2;

		// Token: 0x04001A68 RID: 6760
		public string extra;

		// Token: 0x04001A69 RID: 6761
		public bool compressed;
	}
}

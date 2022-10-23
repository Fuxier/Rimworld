using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200051F RID: 1311
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x060027DF RID: 10207 RVA: 0x001039BF File Offset: 0x00101BBF
		public CreditRecord_Role()
		{
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x00103A0A File Offset: 0x00101C0A
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x00103A27 File Offset: 0x00101C27
		public override float DrawHeight(float width)
		{
			if (this.roleKey.NullOrEmpty())
			{
				width *= 0.5f;
			}
			if (!this.compressed)
			{
				return 50f;
			}
			return Text.CalcHeight(this.creditee, width * 0.5f);
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x00103A60 File Offset: 0x00101C60
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			if (!this.roleKey.NullOrEmpty())
			{
				rect2.width = rect.width / 2f;
				if (this.displayKey)
				{
					Widgets.Label(rect2, this.roleKey);
				}
			}
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			if (this.roleKey.NullOrEmpty())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			Widgets.Label(rect3, this.creditee);
			Text.Anchor = TextAnchor.MiddleLeft;
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

		// Token: 0x060027E3 RID: 10211 RVA: 0x00103B48 File Offset: 0x00101D48
		public CreditRecord_Role Compress()
		{
			this.compressed = true;
			return this;
		}

		// Token: 0x04001A61 RID: 6753
		public string roleKey;

		// Token: 0x04001A62 RID: 6754
		public string creditee;

		// Token: 0x04001A63 RID: 6755
		public string extra;

		// Token: 0x04001A64 RID: 6756
		public bool displayKey;

		// Token: 0x04001A65 RID: 6757
		public bool compressed;
	}
}

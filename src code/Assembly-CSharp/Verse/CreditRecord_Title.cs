using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000521 RID: 1313
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x060027E9 RID: 10217 RVA: 0x001039BF File Offset: 0x00101BBF
		public CreditRecord_Title()
		{
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x00103C7C File Offset: 0x00101E7C
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x00103C8B File Offset: 0x00101E8B
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x00103C94 File Offset: 0x00101E94
		public override void Draw(Rect rect)
		{
			rect.yMin += 31f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(rect.x + 10f, Mathf.Round(rect.yMax) - 14f, rect.width - 20f);
			GUI.color = Color.white;
		}

		// Token: 0x04001A6A RID: 6762
		public string title;
	}
}

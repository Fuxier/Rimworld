using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DB RID: 1243
	public class ListableOption
	{
		// Token: 0x06002596 RID: 9622 RVA: 0x000EED9F File Offset: 0x000ECF9F
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000EEDC8 File Offset: 0x000ECFC8
		public virtual float DrawOption(Vector2 pos, float width)
		{
			float b = Text.CalcHeight(this.label, width);
			float num = Mathf.Max(this.minHeight, b);
			Rect rect = new Rect(pos.x, pos.y, width, num);
			if (Widgets.ButtonText(rect, this.label, true, true, true, null))
			{
				this.action();
			}
			if (this.uiHighlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.uiHighlightTag);
			}
			return num;
		}

		// Token: 0x0400180E RID: 6158
		public string label;

		// Token: 0x0400180F RID: 6159
		public Action action;

		// Token: 0x04001810 RID: 6160
		private string uiHighlightTag;

		// Token: 0x04001811 RID: 6161
		public float minHeight = 45f;
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200051E RID: 1310
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x060027DB RID: 10203 RVA: 0x001039BF File Offset: 0x00101BBF
		public CreditRecord_Text()
		{
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x001039C7 File Offset: 0x00101BC7
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x001039DD File Offset: 0x00101BDD
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x001039EB File Offset: 0x00101BEB
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04001A5F RID: 6751
		public string text;

		// Token: 0x04001A60 RID: 6752
		public TextAnchor anchor;
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200048F RID: 1167
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x06002353 RID: 9043 RVA: 0x000E2770 File Offset: 0x000E0970
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x000E2780 File Offset: 0x000E0980
		public override void FeedbackOnGUI()
		{
			string str = this.Amount + " food";
			base.DrawFloatingText(str, Color.yellow);
		}

		// Token: 0x040016AF RID: 5807
		protected int Amount;
	}
}

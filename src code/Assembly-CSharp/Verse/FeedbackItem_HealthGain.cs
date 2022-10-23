using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000490 RID: 1168
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x06002355 RID: 9045 RVA: 0x000E27AF File Offset: 0x000E09AF
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000E27C8 File Offset: 0x000E09C8
		public override void FeedbackOnGUI()
		{
			string text;
			if (this.Amount >= 0)
			{
				text = "+";
			}
			else
			{
				text = "-";
			}
			text += this.Amount;
			base.DrawFloatingText(text, Color.red);
		}

		// Token: 0x040016B0 RID: 5808
		protected Pawn Healer;

		// Token: 0x040016B1 RID: 5809
		protected int Amount;
	}
}

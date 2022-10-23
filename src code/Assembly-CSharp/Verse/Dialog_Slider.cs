using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F5 RID: 1269
	public class Dialog_Slider : Window
	{
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x000F8B84 File Offset: 0x000F6D84
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 130f + this.extraBottomSpace);
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x000F8B9C File Offset: 0x000F6D9C
		protected override float Margin
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000F8BA4 File Offset: 0x000F6DA4
		public Dialog_Slider(Func<int, string> textGetter, int from, int to, Action<int> confirmAction, int startingValue = -2147483648, float roundTo = 1f)
		{
			this.textGetter = textGetter;
			this.from = from;
			this.to = to;
			this.confirmAction = confirmAction;
			this.roundTo = roundTo;
			this.forcePause = true;
			this.closeOnClickedOutside = true;
			if (startingValue == -2147483648)
			{
				this.curValue = from;
				return;
			}
			this.curValue = startingValue;
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000F8C10 File Offset: 0x000F6E10
		public Dialog_Slider(string text, int from, int to, Action<int> confirmAction, int startingValue = -2147483648, float roundTo = 1f) : this((int val) => string.Format(text, val), from, to, confirmAction, startingValue, roundTo)
		{
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000F8C44 File Offset: 0x000F6E44
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			string text = this.textGetter(this.curValue);
			float height = Text.CalcHeight(text, inRect.width);
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, height);
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(rect, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(inRect.x, inRect.y + rect.height + 10f, inRect.width, 30f);
			this.curValue = (int)Widgets.HorizontalSlider(rect2, (float)this.curValue, (float)this.from, (float)this.to, true, null, null, null, this.roundTo);
			GUI.color = ColoredText.SubtleGrayColor;
			Text.Font = GameFont.Tiny;
			Widgets.Label(new Rect(inRect.x, rect2.yMax - 10f, inRect.width / 2f, Text.LineHeight), this.from.ToString());
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(new Rect(inRect.x + inRect.width / 2f, rect2.yMax - 10f, inRect.width / 2f, Text.LineHeight), this.to.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			float num = (inRect.width - 10f) / 2f;
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - 30f, num, 30f), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.x + num + 10f, inRect.yMax - 30f, num, 30f), "OK".Translate(), true, true, true, null))
			{
				this.Close(true);
				this.confirmAction(this.curValue);
			}
		}

		// Token: 0x0400194B RID: 6475
		public Func<int, string> textGetter;

		// Token: 0x0400194C RID: 6476
		public int from;

		// Token: 0x0400194D RID: 6477
		public int to;

		// Token: 0x0400194E RID: 6478
		public float roundTo = 1f;

		// Token: 0x0400194F RID: 6479
		public float extraBottomSpace;

		// Token: 0x04001950 RID: 6480
		private Action<int> confirmAction;

		// Token: 0x04001951 RID: 6481
		private int curValue;

		// Token: 0x04001952 RID: 6482
		private const float BotAreaHeight = 30f;

		// Token: 0x04001953 RID: 6483
		private const float NumberYOffset = 10f;
	}
}

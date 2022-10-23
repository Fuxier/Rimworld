using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200048E RID: 1166
	public abstract class FeedbackItem
	{
		// Token: 0x0600234F RID: 9039 RVA: 0x000E2640 File Offset: 0x000E0840
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000E26A1 File Offset: 0x000E08A1
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x06002351 RID: 9041
		public abstract void FeedbackOnGUI();

		// Token: 0x06002352 RID: 9042 RVA: 0x000E26D8 File Offset: 0x000E08D8
		protected void DrawFloatingText(string str, Color TextColor)
		{
			float x = Text.CalcSize(str).x;
			Rect wordRect = new Rect(this.CurScreenPos.x - x / 2f, this.CurScreenPos.y, x, 20f);
			Find.WindowStack.ImmediateWindow(5983 * this.uniqueID + 495, wordRect, WindowLayer.Super, delegate
			{
				Rect rect = wordRect.AtZero();
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Small;
				GUI.DrawTexture(rect, TexUI.GrayTextBG);
				GUI.color = TextColor;
				Widgets.Label(rect, str);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}, false, false, 1f, null);
		}

		// Token: 0x040016AA RID: 5802
		protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

		// Token: 0x040016AB RID: 5803
		private int uniqueID;

		// Token: 0x040016AC RID: 5804
		public float TimeLeft = 2f;

		// Token: 0x040016AD RID: 5805
		protected Vector2 CurScreenPos;

		// Token: 0x040016AE RID: 5806
		private static int freeUniqueID;
	}
}

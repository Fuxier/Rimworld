using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004CB RID: 1227
	[StaticConstructorOnStartup]
	public class ActiveTip
	{
		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x0600250B RID: 9483 RVA: 0x000EB59C File Offset: 0x000E979C
		private string FinalText
		{
			get
			{
				string text;
				if (this.signal.textGetter != null)
				{
					try
					{
						text = this.signal.textGetter();
						goto IL_3E;
					}
					catch (Exception ex)
					{
						Log.Error(ex.ToString());
						text = "Error getting tip text.";
						goto IL_3E;
					}
				}
				text = this.signal.text;
				IL_3E:
				return text.TrimEnd(Array.Empty<char>());
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x000EB604 File Offset: 0x000E9804
		public Rect TipRect
		{
			get
			{
				string finalText = this.FinalText;
				Vector2 vector = Text.CalcSize(finalText);
				if (vector.x > 260f)
				{
					vector.x = 260f;
					vector.y = Text.CalcHeight(finalText, vector.x);
				}
				return new Rect(0f, 0f, vector.x, vector.y).ContractedBy(-4f).RoundedCeil();
			}
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000EB675 File Offset: 0x000E9875
		public ActiveTip(TipSignal signal)
		{
			this.signal = signal;
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000EB684 File Offset: 0x000E9884
		public ActiveTip(ActiveTip cloneSource)
		{
			this.signal = cloneSource.signal;
			this.firstTriggerTime = cloneSource.firstTriggerTime;
			this.lastTriggerFrame = cloneSource.lastTriggerFrame;
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x000EB6B0 File Offset: 0x000E98B0
		public float DrawTooltip(Vector2 pos)
		{
			Text.Font = GameFont.Small;
			string finalText = this.FinalText;
			Rect bgRect = this.TipRect;
			bgRect.position = pos;
			if (!LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting)
			{
				Find.WindowStack.ImmediateWindow(153 * this.signal.uniqueId + 62346, bgRect, WindowLayer.Super, delegate
				{
					this.DrawInner(bgRect.AtZero(), finalText);
				}, false, false, 1f, null);
			}
			else
			{
				Widgets.DrawShadowAround(bgRect);
				Widgets.DrawWindowBackground(bgRect);
				this.DrawInner(bgRect, finalText);
			}
			return bgRect.height;
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x000EB76C File Offset: 0x000E996C
		private void DrawInner(Rect bgRect, string label)
		{
			Widgets.DrawAtlas(bgRect, ActiveTip.TooltipBGAtlas);
			Text.Font = GameFont.Small;
			Widgets.Label(bgRect.ContractedBy(4f), label);
		}

		// Token: 0x040017B7 RID: 6071
		public TipSignal signal;

		// Token: 0x040017B8 RID: 6072
		public double firstTriggerTime;

		// Token: 0x040017B9 RID: 6073
		public int lastTriggerFrame;

		// Token: 0x040017BA RID: 6074
		private const int TipMargin = 4;

		// Token: 0x040017BB RID: 6075
		private const float MaxWidth = 260f;

		// Token: 0x040017BC RID: 6076
		public static readonly Texture2D TooltipBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TooltipBG", true);
	}
}

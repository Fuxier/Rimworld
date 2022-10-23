using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004EF RID: 1263
	public class Dialog_RechargeSettings : Window
	{
		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002694 RID: 9876 RVA: 0x000F81AC File Offset: 0x000F63AC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(450f, 300f);
			}
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000F81C0 File Offset: 0x000F63C0
		public Dialog_RechargeSettings(MechanitorControlGroup controlGroup)
		{
			this.controlGroup = controlGroup;
			this.range = controlGroup.mechRechargeThresholds;
			this.title = "MechRechargeSettingsTitle".Translate();
			this.text = "MechRechargeSettingsExplanation".Translate();
			this.forcePause = true;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000F8220 File Offset: 0x000F6420
		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			Rect rect = new Rect(inRect.x, num, inRect.width, 30f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			num += rect.height + 17f;
			Rect rect2 = new Rect(inRect.x, num, inRect.width, Text.CalcHeight(this.text, inRect.width));
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
			num += rect2.height + 17f;
			Widgets.FloatRange(new Rect(inRect.x, num, inRect.width, 30f), this.GetHashCode(), ref this.range, 0f, 1f, null, ToStringStyle.PercentZero, 0.05f, GameFont.Small, new Color?(Color.white));
			this.range.min = GenMath.RoundTo(this.range.min, 0.01f);
			this.range.max = GenMath.RoundTo(this.range.max, 0.01f);
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.x + inRect.width / 2f - Window.CloseButSize.x / 2f, inRect.yMax - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "Reset".Translate(), true, true, true, null))
			{
				this.range = MechanitorControlGroup.DefaultMechRechargeThresholds;
			}
			if (Widgets.ButtonText(new Rect(inRect.xMax - Window.CloseButSize.x, inRect.yMax - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "OK".Translate(), true, true, true, null))
			{
				this.controlGroup.mechRechargeThresholds = this.range;
				this.Close(true);
			}
		}

		// Token: 0x04001939 RID: 6457
		private FloatRange range;

		// Token: 0x0400193A RID: 6458
		private MechanitorControlGroup controlGroup;

		// Token: 0x0400193B RID: 6459
		private string title;

		// Token: 0x0400193C RID: 6460
		private string text;

		// Token: 0x0400193D RID: 6461
		private const float HeaderHeight = 30f;

		// Token: 0x0400193E RID: 6462
		private const float SliderHeight = 30f;
	}
}

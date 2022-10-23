using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004E9 RID: 1257
	public class WidgetRow
	{
		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060025D2 RID: 9682 RVA: 0x000F0DCB File Offset: 0x000EEFCB
		public float FinalX
		{
			get
			{
				return this.curX;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060025D3 RID: 9683 RVA: 0x000F0DD3 File Offset: 0x000EEFD3
		public float FinalY
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060025D4 RID: 9684 RVA: 0x000F0DDB File Offset: 0x000EEFDB
		// (set) Token: 0x060025D5 RID: 9685 RVA: 0x000F0DE3 File Offset: 0x000EEFE3
		public float CellGap
		{
			get
			{
				return this.gap;
			}
			set
			{
				this.gap = value;
			}
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x000F0DEC File Offset: 0x000EEFEC
		public WidgetRow()
		{
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000F0E06 File Offset: 0x000EF006
		public WidgetRow(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.Init(x, y, growDirection, maxWidth, gap);
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000F0E2D File Offset: 0x000EF02D
		public void Init(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.growDirection = growDirection;
			this.startX = x;
			this.curX = x;
			this.curY = y;
			this.maxWidth = maxWidth;
			this.gap = gap;
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x000F0E5B File Offset: 0x000EF05B
		private float LeftX(float elementWidth)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				return this.curX;
			}
			return this.curX - elementWidth;
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000F0E80 File Offset: 0x000EF080
		private void IncrementPosition(float amount)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				this.curX += amount;
			}
			else
			{
				this.curX -= amount;
			}
			if (Mathf.Abs(this.curX - this.startX) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x000F0EE0 File Offset: 0x000EF0E0
		private void IncrementY()
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.LeftThenUp)
			{
				this.curY -= 24f + this.gap;
			}
			else
			{
				this.curY += 24f + this.gap;
			}
			this.curX = this.startX;
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x000F0F3E File Offset: 0x000EF13E
		private void IncrementYIfWillExceedMaxWidth(float width)
		{
			if (Mathf.Abs(this.curX - this.startX) + Mathf.Abs(width) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x000F0F67 File Offset: 0x000EF167
		public void Gap(float width)
		{
			if (this.curX != this.startX)
			{
				this.IncrementPosition(width);
			}
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x000F0F80 File Offset: 0x000EF180
		public bool ButtonIcon(Texture2D tex, string tooltip = null, Color? mouseoverColor = null, Color? backgroundColor = null, Color? mouseoverBackgroundColor = null, bool doMouseoverSound = true, float overrideSize = -1f)
		{
			float num = (overrideSize > 0f) ? overrideSize : 24f;
			float num2 = (24f - num) / 2f;
			this.IncrementYIfWillExceedMaxWidth(num);
			Rect rect = new Rect(this.LeftX(num) + num2, this.curY + num2, num, num);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (mouseoverBackgroundColor != null && Mouse.IsOver(rect))
			{
				Widgets.DrawRectFast(rect, mouseoverBackgroundColor.Value, null);
			}
			else if (backgroundColor != null && !Mouse.IsOver(rect))
			{
				Widgets.DrawRectFast(rect, backgroundColor.Value, null);
			}
			bool result = Widgets.ButtonImage(rect, tex, Color.white, mouseoverColor ?? GenUI.MouseoverColor, true);
			this.IncrementPosition(num);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x000F105C File Offset: 0x000EF25C
		public bool ButtonIconWithBG(Texture2D texture, float width = -1f, string tooltip = null, bool doMouseoverSound = true)
		{
			if (width < 0f)
			{
				width = 24f;
			}
			width += 16f;
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 26f);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			bool result = Widgets.ButtonImageWithBG(rect, texture, new Vector2?(Vector2.one * 24f));
			this.IncrementPosition(width + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x000F10EC File Offset: 0x000EF2EC
		public void ToggleableIcon(ref bool toggleable, Texture2D tex, string tooltip, SoundDef mouseoverSound = null, string tutorTag = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			bool flag = Widgets.ButtonImage(rect, tex, true);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Rect position = new Rect(rect.x + rect.width / 2f, rect.y, rect.height / 2f, rect.height / 2f);
			Texture2D image = toggleable ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect, mouseoverSound);
			}
			if (flag)
			{
				toggleable = !toggleable;
				if (toggleable)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, tutorTag);
			}
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000F11E4 File Offset: 0x000EF3E4
		public Rect Icon(Texture tex, string tooltip = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			GUI.DrawTexture(rect, tex);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000F124C File Offset: 0x000EF44C
		public Rect DefIcon(ThingDef def, string tooltip = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			Widgets.DefIcon(rect, def, null, 1f, null, false, null, null, null);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000F12D0 File Offset: 0x000EF4D0
		public bool ButtonText(string label, string tooltip = null, bool drawBackground = true, bool doMouseoverSound = true, bool active = true, float? fixedWidth = null)
		{
			Rect rect = this.ButtonRect(label, fixedWidth);
			bool result = Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, active, null);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000F1310 File Offset: 0x000EF510
		public Rect ButtonRect(string label, float? fixedWidth = null)
		{
			Vector2 vector = (fixedWidth != null) ? new Vector2(fixedWidth.Value, 24f) : Text.CalcSize(label);
			vector.x += 16f;
			vector.y += 2f;
			this.IncrementYIfWillExceedMaxWidth(vector.x);
			Rect result = new Rect(this.LeftX(vector.x), this.curY, vector.x, vector.y);
			this.IncrementPosition(result.width + this.gap);
			return result;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000F13A8 File Offset: 0x000EF5A8
		public Rect Label(string text, float width = -1f, string tooltip = null, float height = -1f)
		{
			if (height < 0f)
			{
				height = 24f;
			}
			if (width < 0f)
			{
				width = Text.CalcSize(text).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.Label(rect, text);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000F1440 File Offset: 0x000EF640
		public Rect TextFieldNumeric<T>(ref int val, ref string buffer, float width = -1f) where T : struct
		{
			if (width < 0f)
			{
				width = Text.CalcSize(val.ToString()).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 24f);
			Widgets.TextFieldNumeric<int>(rect, ref val, ref buffer, 0f, 1E+09f);
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000F14C4 File Offset: 0x000EF6C4
		public Rect FillableBar(float width, float height, float fillPct, string label, Texture2D fillTex, Texture2D bgTex = null)
		{
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.FillableBar(rect, fillPct, fillTex, bgTex, false);
			if (!label.NullOrEmpty())
			{
				Rect rect2 = rect;
				rect2.xMin += 2f;
				rect2.xMax -= 2f;
				if (!Text.TinyFontSupported)
				{
					rect2.y -= 2f;
				}
				if (Text.Anchor >= TextAnchor.UpperLeft)
				{
					rect2.height += 14f;
				}
				Text.Font = GameFont.Tiny;
				Text.WordWrap = false;
				Widgets.Label(rect2, label);
				Text.WordWrap = true;
			}
			this.IncrementPosition(width);
			return rect;
		}

		// Token: 0x040018AF RID: 6319
		private float startX;

		// Token: 0x040018B0 RID: 6320
		private float curX;

		// Token: 0x040018B1 RID: 6321
		private float curY;

		// Token: 0x040018B2 RID: 6322
		private float maxWidth = 99999f;

		// Token: 0x040018B3 RID: 6323
		private float gap;

		// Token: 0x040018B4 RID: 6324
		private UIDirection growDirection = UIDirection.RightThenUp;

		// Token: 0x040018B5 RID: 6325
		public const float IconSize = 24f;

		// Token: 0x040018B6 RID: 6326
		public const float DefaultGap = 4f;

		// Token: 0x040018B7 RID: 6327
		private const float DefaultMaxWidth = 99999f;

		// Token: 0x040018B8 RID: 6328
		public const float LabelGap = 2f;

		// Token: 0x040018B9 RID: 6329
		public const float ButtonExtraSpace = 16f;
	}
}

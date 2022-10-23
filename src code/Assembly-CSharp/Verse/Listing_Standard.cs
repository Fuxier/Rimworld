using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004B5 RID: 1205
	[StaticConstructorOnStartup]
	public class Listing_Standard : Listing
	{
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x0600243F RID: 9279 RVA: 0x000E7500 File Offset: 0x000E5700
		public Rect? BoundingRectCached
		{
			get
			{
				if (this.boundingRectCachedForFrame != Time.frameCount)
				{
					if (this.boundingRect != null && this.boundingScrollPositionGetter != null)
					{
						Rect value = this.boundingRect.Value;
						Vector2 vector = this.boundingScrollPositionGetter();
						value.x += vector.x;
						value.y += vector.y;
						this.boundingRectCached = new Rect?(value);
					}
					this.boundingRectCachedForFrame = Time.frameCount;
				}
				return this.boundingRectCached;
			}
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000E758C File Offset: 0x000E578C
		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000E75A2 File Offset: 0x000E57A2
		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x000E75B8 File Offset: 0x000E57B8
		public Listing_Standard(Rect boundingRect, Func<Vector2> boundingScrollPositionGetter)
		{
			this.font = GameFont.Small;
			this.boundingRect = new Rect?(boundingRect);
			this.boundingScrollPositionGetter = boundingScrollPositionGetter;
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x000E75E1 File Offset: 0x000E57E1
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x000E75F8 File Offset: 0x000E57F8
		public override void End()
		{
			base.End();
			if (this.labelScrollbarPositions != null)
			{
				for (int i = this.labelScrollbarPositions.Count - 1; i >= 0; i--)
				{
					if (!this.labelScrollbarPositionsSetThisFrame.Contains(this.labelScrollbarPositions[i].First))
					{
						this.labelScrollbarPositions.RemoveAt(i);
					}
				}
				this.labelScrollbarPositionsSetThisFrame.Clear();
			}
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x000E7663 File Offset: 0x000E5863
		public Rect Label(TaggedString label, float maxHeight = -1f, string tooltip = null)
		{
			return this.Label(label.Resolve(), maxHeight, tooltip);
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x000E7674 File Offset: 0x000E5874
		public Rect Label(string label, float maxHeight = -1f, string tooltip = null)
		{
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool flag = false;
			if (maxHeight >= 0f && num > maxHeight)
			{
				num = maxHeight;
				flag = true;
			}
			Rect rect = base.GetRect(num, 1f);
			if (this.BoundingRectCached != null && !rect.Overlaps(this.BoundingRectCached.Value))
			{
				return rect;
			}
			if (flag)
			{
				Vector2 labelScrollbarPosition = this.GetLabelScrollbarPosition(this.curX, this.curY);
				Widgets.LabelScrollable(rect, label, ref labelScrollbarPosition, false, true, false);
				this.SetLabelScrollbarPosition(this.curX, this.curY, labelScrollbarPosition);
			}
			else
			{
				Widgets.Label(rect, label);
			}
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x000E7734 File Offset: 0x000E5934
		public void LabelDouble(string leftLabel, string rightLabel, string tip = null)
		{
			float num = base.ColumnWidth / 2f;
			float width = base.ColumnWidth - num;
			float a = Text.CalcHeight(leftLabel, num);
			float b = Text.CalcHeight(rightLabel, width);
			float height = Mathf.Max(a, b);
			Rect rect = base.GetRect(height, 1f);
			if (this.BoundingRectCached != null && !rect.Overlaps(this.BoundingRectCached.Value))
			{
				return;
			}
			if (!tip.NullOrEmpty())
			{
				Widgets.DrawHighlightIfMouseover(rect);
				TooltipHandler.TipRegion(rect, tip);
			}
			Widgets.Label(rect.LeftHalf(), leftLabel);
			Widgets.Label(rect.RightHalf(), rightLabel);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x000E77E8 File Offset: 0x000E59E8
		public Rect SubLabel(string label, float widthPct)
		{
			float height = Text.CalcHeight(label, base.ColumnWidth * widthPct);
			Rect rect = base.GetRect(height, widthPct);
			float num = 20f;
			rect.x += num;
			rect.width -= num;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.gray;
			Widgets.Label(rect, label);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x000E7868 File Offset: 0x000E5A68
		public bool RadioButton(string label, bool active, float tabIn = 0f, string tooltip = null, float? tooltipDelay = null)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight, 1f);
			rect.xMin += tabIn;
			if (this.BoundingRectCached != null && !rect.Overlaps(this.BoundingRectCached.Value))
			{
				return false;
			}
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				TipSignal tip = (tooltipDelay != null) ? new TipSignal(tooltip, tooltipDelay.Value) : new TipSignal(tooltip);
				TooltipHandler.TipRegion(rect, tip);
			}
			bool result = Widgets.RadioButtonLabeled(rect, label, active);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x000E7914 File Offset: 0x000E5B14
		public void CheckboxLabeled(string label, ref bool checkOn, float tabIn)
		{
			float height = Text.CalcHeight(label, base.ColumnWidth);
			Rect rect = base.GetRect(height, 1f);
			rect.xMin += tabIn;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false);
				base.Gap(this.verticalSpacing);
			}
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x000E798C File Offset: 0x000E5B8C
		public void CheckboxLabeled(string label, ref bool checkOn, string tooltip = null, float height = 0f, float labelPct = 1f)
		{
			float height2 = (height != 0f) ? height : Text.CalcHeight(label, base.ColumnWidth * labelPct);
			Rect rect = base.GetRect(height2, labelPct);
			rect.width = Math.Min(rect.width + 24f, base.ColumnWidth);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				if (!tooltip.NullOrEmpty())
				{
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
					}
					TooltipHandler.TipRegion(rect, tooltip);
				}
				Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x000E7A40 File Offset: 0x000E5C40
		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight, 1f);
			bool result = false;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = Widgets.CheckboxLabeledSelectable(rect, label, ref selected, ref checkOn, null, 1f);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x000E7AA8 File Offset: 0x000E5CA8
		public bool ButtonText(string label, string highlightTag = null, float widthPct = 1f)
		{
			Rect rect = base.GetRect(30f, widthPct);
			bool result = false;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = Widgets.ButtonText(rect, label, true, true, true, null);
				if (highlightTag != null)
				{
					UIHighlighter.HighlightOpportunity(rect, highlightTag);
				}
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x000E7B16 File Offset: 0x000E5D16
		public bool ButtonTextLabeled(string label, string buttonLabel, TextAnchor anchor = TextAnchor.UpperLeft, string highlightTag = null, string tooltip = null)
		{
			return this.ButtonTextLabeledPct(label, buttonLabel, 0.5f, anchor, highlightTag, tooltip);
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x000E7B2C File Offset: 0x000E5D2C
		public bool ButtonTextLabeledPct(string label, string buttonLabel, float labelPct, TextAnchor anchor = TextAnchor.UpperLeft, string highlightTag = null, string tooltip = null)
		{
			float height = Math.Max(Text.CalcHeight(label, base.ColumnWidth * labelPct), 30f);
			Rect rect = base.GetRect(height, 1f);
			Rect rect2 = rect.RightPart(1f - labelPct);
			rect2.height = 30f;
			if (highlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, highlightTag);
			}
			bool result = false;
			Rect rect3 = rect.LeftPart(labelPct);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Text.Anchor = anchor;
				Widgets.Label(rect3, label);
				result = Widgets.ButtonText(rect2, buttonLabel.Truncate(rect2.width - 20f, null), true, true, true, null);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
				}
				TooltipHandler.TipRegion(rect3, tooltip);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x000E7C2C File Offset: 0x000E5E2C
		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			Rect butRect = new Rect(this.curX, this.curY, width, height);
			bool result = false;
			if (this.BoundingRectCached == null || butRect.Overlaps(this.BoundingRectCached.Value))
			{
				result = Widgets.ButtonImage(butRect, tex, true);
			}
			base.Gap(height + this.verticalSpacing);
			return result;
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x000E7C96 File Offset: 0x000E5E96
		public void None()
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			this.Label("NoneBrackets".Translate(), -1f, null);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000E7CD0 File Offset: 0x000E5ED0
		public string TextEntry(string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount, 1f);
			string result;
			if (lineCount == 1)
			{
				result = Widgets.TextField(rect, text);
			}
			else
			{
				result = Widgets.TextArea(rect, text, false);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000E7D15 File Offset: 0x000E5F15
		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			string result = Widgets.TextEntryLabeled(base.GetRect(Text.LineHeight * (float)lineCount, 1f), label, text);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000E7D40 File Offset: 0x000E5F40
		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.TextFieldNumeric<T>(rect, ref val, ref buffer, min, max);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000E7D9C File Offset: 0x000E5F9C
		public void TextFieldNumericLabeled<T>(string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.TextFieldNumericLabeled<T>(rect, label, ref val, ref buffer, min, max);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000E7DFC File Offset: 0x000E5FFC
		public void IntRange(ref IntRange range, int min, int max)
		{
			Rect rect = base.GetRect(28f, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.IntRange(rect, (int)base.CurHeight, ref range, min, max, null, 0);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000E7E60 File Offset: 0x000E6060
		public float Slider(float val, float min, float max)
		{
			float num = Widgets.HorizontalSlider(base.GetRect(22f, 1f), val, min, max, false, null, null, null, -1f);
			if (num != val)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			base.Gap(this.verticalSpacing);
			return num;
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000E7EAC File Offset: 0x000E60AC
		public float SliderLabeled(string label, float val, float min, float max, float labelPct = 0.5f, string tooltip = null)
		{
			Rect rect = base.GetRect(30f, 1f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect.LeftPart(labelPct), label);
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect.LeftPart(labelPct), tooltip);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			float num = Widgets.HorizontalSlider(rect.RightPart(1f - labelPct), val, min, max, true, null, null, null, -1f);
			if (num != val)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			base.Gap(this.verticalSpacing);
			return num;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000E7F38 File Offset: 0x000E6138
		public void IntAdjuster(ref int val, int countChange, int min = 0)
		{
			Rect rect = base.GetRect(24f, 1f);
			rect.width = 42f;
			if (Widgets.ButtonText(rect, "-" + countChange, true, true, true, null))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val -= countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			rect.x += rect.width + 2f;
			if (Widgets.ButtonText(rect, "+" + countChange, true, true, true, null))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val += countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000E8010 File Offset: 0x000E6210
		public void IntSetter(ref int val, int target, string label, float width = 42f)
		{
			if (Widgets.ButtonText(base.GetRect(24f, 1f), label, true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				val = target;
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000E805C File Offset: 0x000E625C
		public void IntEntry(ref int val, ref string editBuffer, int multiplier = 1)
		{
			Rect rect = base.GetRect(24f, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.IntEntry(rect, ref val, ref editBuffer, multiplier);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000E80B8 File Offset: 0x000E62B8
		public Listing_Standard BeginSection(float height, float sectionBorder = 4f, float bottomBorder = 4f)
		{
			Rect rect = base.GetRect(height + sectionBorder + bottomBorder, 1f);
			Widgets.DrawMenuSection(rect);
			Listing_Standard listing_Standard = new Listing_Standard();
			Rect rect2 = new Rect(rect.x + sectionBorder, rect.y + sectionBorder, rect.width - sectionBorder * 2f, rect.height - (sectionBorder + bottomBorder));
			listing_Standard.Begin(rect2);
			return listing_Standard;
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000E811B File Offset: 0x000E631B
		public void EndSection(Listing_Standard listing)
		{
			listing.End();
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000E8124 File Offset: 0x000E6324
		private Vector2 GetLabelScrollbarPosition(float x, float y)
		{
			if (this.labelScrollbarPositions == null)
			{
				return Vector2.zero;
			}
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					return this.labelScrollbarPositions[i].Second;
				}
			}
			return Vector2.zero;
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x000E8198 File Offset: 0x000E6398
		private void SetLabelScrollbarPosition(float x, float y, Vector2 scrollbarPosition)
		{
			if (this.labelScrollbarPositions == null)
			{
				this.labelScrollbarPositions = new List<Pair<Vector2, Vector2>>();
				this.labelScrollbarPositionsSetThisFrame = new List<Vector2>();
			}
			this.labelScrollbarPositionsSetThisFrame.Add(new Vector2(x, y));
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					this.labelScrollbarPositions[i] = new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition);
					return;
				}
			}
			this.labelScrollbarPositions.Add(new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition));
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000E8244 File Offset: 0x000E6444
		public bool SelectableDef(string name, bool selected, Action deleteCallback)
		{
			Text.Font = GameFont.Tiny;
			float width = this.listingRect.width - 21f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(this.curX, this.curY, width, 21f);
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 1, null);
			}
			Text.WordWrap = false;
			Widgets.Label(rect, name);
			Text.WordWrap = true;
			if (deleteCallback != null && Widgets.ButtonImage(new Rect(rect.xMax, rect.y, 21f, 21f), TexButton.DeleteX, Color.white, GenUI.SubtleMouseoverColor, true))
			{
				deleteCallback();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.curY += 21f;
			return Widgets.ButtonInvisible(rect, true);
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000E8310 File Offset: 0x000E6510
		public void LabelCheckboxDebug(string label, ref bool checkOn, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth, 22f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.CheckboxLabeled(rect, label.Truncate(rect.width - 15f, null), ref checkOn, false, null, null, false);
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
			}
			base.Gap(22f + this.verticalSpacing);
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000E83C4 File Offset: 0x000E65C4
		public bool ButtonDebug(string label, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth, 22f);
			bool result = false;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				bool wordWrap = Text.WordWrap;
				Text.WordWrap = false;
				result = Widgets.ButtonText(rect, "  " + label, true, true, true, new TextAnchor?(TextAnchor.MiddleLeft));
				Text.WordWrap = wordWrap;
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000E8488 File Offset: 0x000E6688
		public DebugActionButtonResult ButtonDebugPinnable(string label, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				bool wordWrap = Text.WordWrap;
				Text.WordWrap = false;
				if (Widgets.ButtonText(rect, "  " + label, true, true, true, new TextAnchor?(TextAnchor.MiddleLeft)))
				{
					result = DebugActionButtonResult.ButtonPressed;
				}
				Text.WordWrap = wordWrap;
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
				Rect rect2 = new Rect(rect.xMax + 2f, rect.y, 22f, 22f).ContractedBy(4f);
				GUI.color = (pinned ? Color.white : new Color(1f, 1f, 1f, 0.2f));
				GUI.DrawTexture(rect2, pinned ? Listing_Standard.PinTex : Listing_Standard.PinOutlineTex);
				GUI.color = Color.white;
				if (Widgets.ButtonInvisible(rect2, true))
				{
					result = DebugActionButtonResult.PinPressed;
				}
				Widgets.DrawHighlightIfMouseover(rect2);
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000E85DC File Offset: 0x000E67DC
		public DebugActionButtonResult CheckboxPinnable(string label, ref bool checkOn, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.CheckboxLabeled(rect, label.Truncate(rect.width - 24f - 15f, null), ref checkOn, false, null, null, false);
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, label);
				}
				Rect rect2 = new Rect(rect.xMax + 2f, rect.y, 22f, 22f).ContractedBy(4f);
				GUI.color = (pinned ? Color.white : new Color(1f, 1f, 1f, 0.2f));
				GUI.DrawTexture(rect2, pinned ? Listing_Standard.PinTex : Listing_Standard.PinOutlineTex);
				GUI.color = Color.white;
				if (Widgets.ButtonInvisible(rect2, true))
				{
					result = DebugActionButtonResult.PinPressed;
				}
				Widgets.DrawHighlightIfMouseover(rect2);
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x04001751 RID: 5969
		private GameFont font;

		// Token: 0x04001752 RID: 5970
		private Rect? boundingRect;

		// Token: 0x04001753 RID: 5971
		private Func<Vector2> boundingScrollPositionGetter;

		// Token: 0x04001754 RID: 5972
		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		// Token: 0x04001755 RID: 5973
		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		// Token: 0x04001756 RID: 5974
		private int boundingRectCachedForFrame = -1;

		// Token: 0x04001757 RID: 5975
		private Rect? boundingRectCached;

		// Token: 0x04001758 RID: 5976
		private static readonly Texture2D PinTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin", true);

		// Token: 0x04001759 RID: 5977
		private static readonly Texture2D PinOutlineTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin-Outline", true);

		// Token: 0x0400175A RID: 5978
		public const float PinnableActionHeight = 22f;

		// Token: 0x0400175B RID: 5979
		private const float DefSelectionLineHeight = 21f;
	}
}

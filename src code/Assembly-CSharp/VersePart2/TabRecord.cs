using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E3 RID: 1251
	[StaticConstructorOnStartup]
	public class TabRecord
	{
		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000EFC4F File Offset: 0x000EDE4F
		public bool Selected
		{
			get
			{
				if (this.selectedGetter == null)
				{
					return this.selected;
				}
				return this.selectedGetter();
			}
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000EFC6B File Offset: 0x000EDE6B
		public TabRecord(string label, Action clickedAction, bool selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selected = selected;
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x000EFC93 File Offset: 0x000EDE93
		public TabRecord(string label, Action clickedAction, Func<bool> selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selectedGetter = selected;
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x000EFCBC File Offset: 0x000EDEBC
		public void Draw(Rect rect)
		{
			Rect drawRect = new Rect(rect);
			drawRect.width = 30f;
			Rect drawRect2 = new Rect(rect);
			drawRect2.width = 30f;
			drawRect2.x = rect.x + rect.width - 30f;
			Rect uvRect = new Rect(0.53125f, 0f, 0.46875f, 1f);
			Rect drawRect3 = new Rect(rect);
			drawRect3.x += drawRect.width;
			drawRect3.width -= 60f;
			drawRect3.xMin = Widgets.AdjustCoordToUIScalingFloor(drawRect3.xMin);
			drawRect3.xMax = Widgets.AdjustCoordToUIScalingCeil(drawRect3.xMax);
			Rect uvRect2 = new Rect(30f, 0f, 4f, (float)TabRecord.TabAtlas.height).ToUVRect(new Vector2((float)TabRecord.TabAtlas.width, (float)TabRecord.TabAtlas.height));
			Widgets.DrawTexturePart(drawRect, new Rect(0f, 0f, 0.46875f, 1f), TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect3, uvRect2, TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect2, uvRect, TabRecord.TabAtlas);
			GUI.color = (this.labelColor ?? Color.white);
			Rect rect2 = rect;
			rect2.width -= 10f;
			if (Mouse.IsOver(rect2))
			{
				GUI.color = Color.yellow;
				rect2.x += 2f;
				rect2.y -= 2f;
			}
			Text.WordWrap = false;
			Widgets.Label(rect, this.label);
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (!this.Selected)
			{
				Rect drawRect4 = new Rect(rect);
				drawRect4.y += rect.height;
				drawRect4.y -= 1f;
				drawRect4.height = 1f;
				Rect uvRect3 = new Rect(0.5f, 0.01f, 0.01f, 0.01f);
				Widgets.DrawTexturePart(drawRect4, uvRect3, TabRecord.TabAtlas);
			}
		}

		// Token: 0x0400182D RID: 6189
		public string label = "Tab";

		// Token: 0x0400182E RID: 6190
		public Action clickedAction;

		// Token: 0x0400182F RID: 6191
		public bool selected;

		// Token: 0x04001830 RID: 6192
		public Func<bool> selectedGetter;

		// Token: 0x04001831 RID: 6193
		public Color? labelColor;

		// Token: 0x04001832 RID: 6194
		private const float TabEndWidth = 30f;

		// Token: 0x04001833 RID: 6195
		private const float TabMiddleGraphicWidth = 4f;

		// Token: 0x04001834 RID: 6196
		private static readonly Texture2D TabAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TabAtlas", true);
	}
}

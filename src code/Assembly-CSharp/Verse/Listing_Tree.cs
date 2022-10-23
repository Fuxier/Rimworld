using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004B6 RID: 1206
	public class Listing_Tree : Listing_Lines
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x000E875D File Offset: 0x000E695D
		protected virtual float LabelWidth
		{
			get
			{
				return base.ColumnWidth - 26f;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x000E876B File Offset: 0x000E696B
		protected float EditAreaWidth
		{
			get
			{
				return base.ColumnWidth - this.LabelWidth;
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000E877A File Offset: 0x000E697A
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000E878F File Offset: 0x000E698F
		public override void End()
		{
			base.End();
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000E87A3 File Offset: 0x000E69A3
		protected float XAtIndentLevel(int indentLevel)
		{
			return (float)indentLevel * this.nestIndentWidth;
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000E87B0 File Offset: 0x000E69B0
		protected void LabelLeft(string label, string tipText, int indentLevel, float widthOffset = 0f, Color? textColor = null)
		{
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight)
			{
				xMin = this.XAtIndentLevel(indentLevel) + 18f
			};
			Widgets.DrawHighlightIfMouseover(rect);
			if (!tipText.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					GUI.DrawTexture(rect, TexUI.HighlightTex);
				}
				TooltipHandler.TipRegion(rect, tipText);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = (textColor ?? Color.white);
			rect.width = this.LabelWidth - rect.xMin + widthOffset;
			rect.yMax += 5f;
			rect.yMin -= 5f;
			Widgets.Label(rect, label.Truncate(rect.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000E88A4 File Offset: 0x000E6AA4
		protected bool OpenCloseWidget(TreeNode node, int indentLevel, int openMask)
		{
			if (!node.Openable)
			{
				return false;
			}
			float x = this.XAtIndentLevel(indentLevel);
			float y = this.curY + this.lineHeight / 2f - 9f;
			Rect butRect = new Rect(x, y, 18f, 18f);
			bool flag = this.IsOpen(node, openMask);
			Texture2D tex = flag ? TexButton.Collapse : TexButton.Reveal;
			if (Widgets.ButtonImage(butRect, tex, true))
			{
				if (flag)
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
				node.SetOpen(openMask, !flag);
				return true;
			}
			return false;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000E8939 File Offset: 0x000E6B39
		public virtual bool IsOpen(TreeNode node, int openMask)
		{
			return node.IsOpen(openMask);
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000E8948 File Offset: 0x000E6B48
		public void InfoText(string text, int indentLevel)
		{
			Text.WordWrap = true;
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, 50f);
			rect.xMin = this.LabelWidth;
			rect.height = Text.CalcHeight(text, rect.width);
			Widgets.Label(rect, text);
			this.curY += rect.height;
			Text.WordWrap = false;
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000E89BC File Offset: 0x000E6BBC
		public bool ButtonText(string label)
		{
			Text.WordWrap = true;
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool result = Widgets.ButtonText(new Rect(0f, this.curY, base.ColumnWidth, num), label, true, true, true, null);
			this.curY += num + 0f;
			Text.WordWrap = false;
			return result;
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000E8A1F File Offset: 0x000E6C1F
		public WidgetRow StartWidgetsRow(int indentLevel)
		{
			WidgetRow result = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
			this.curY += 24f;
			return result;
		}

		// Token: 0x0400175C RID: 5980
		public float nestIndentWidth = 11f;

		// Token: 0x0400175D RID: 5981
		protected const float OpenCloseWidgetSize = 18f;
	}
}

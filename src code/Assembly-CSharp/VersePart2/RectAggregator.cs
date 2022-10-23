using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C0 RID: 1216
	public struct RectAggregator
	{
		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060024C3 RID: 9411 RVA: 0x000EA512 File Offset: 0x000E8712
		public Rect Rect
		{
			get
			{
				return this.currentRect;
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000EA51C File Offset: 0x000E871C
		public RectAggregator(Rect parent, int contextHash, Vector2? margin = null)
		{
			this.currentRect = parent;
			this.margin = (margin ?? new Vector2(10f, 4f));
			this.contextHash = contextHash;
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000EA512 File Offset: 0x000E8712
		public static implicit operator Rect(RectAggregator rect)
		{
			return rect.currentRect;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x000EA560 File Offset: 0x000E8760
		public RectDivider NewRow(float height, VerticalJustification addAt = VerticalJustification.Bottom)
		{
			Rect parent;
			if (addAt == VerticalJustification.Top)
			{
				this.currentRect.yMin = this.currentRect.yMin - (this.margin.y + height);
				parent = new Rect(this.currentRect.x, this.currentRect.y, this.currentRect.width, height);
			}
			else
			{
				if (addAt != VerticalJustification.Bottom)
				{
					throw new InvalidOperationException();
				}
				this.currentRect.yMax = this.currentRect.yMax + (this.margin.y + height);
				parent = new Rect(this.currentRect.x, this.currentRect.yMax - height, this.currentRect.width, height);
			}
			return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000EA628 File Offset: 0x000E8828
		public RectDivider NewCol(float width, HorizontalJustification addAt = HorizontalJustification.Right)
		{
			Rect parent;
			if (addAt == HorizontalJustification.Left)
			{
				this.currentRect.xMin = this.currentRect.xMin - (this.margin.x + width);
				parent = new Rect(this.currentRect.x, this.currentRect.y, width, this.currentRect.height);
			}
			else
			{
				if (addAt != HorizontalJustification.Right)
				{
					throw new InvalidOperationException();
				}
				this.currentRect.xMax = this.currentRect.xMax + (this.margin.x + width);
				parent = new Rect(this.currentRect.xMax - width, this.currentRect.y, width, this.currentRect.height);
			}
			return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
		}

		// Token: 0x04001792 RID: 6034
		private Rect currentRect;

		// Token: 0x04001793 RID: 6035
		private Vector2 margin;

		// Token: 0x04001794 RID: 6036
		private int contextHash;
	}
}

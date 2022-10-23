using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C3 RID: 1219
	public struct RectDivider
	{
		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060024C8 RID: 9416 RVA: 0x000EA6EF File Offset: 0x000E88EF
		private int ErrorHash
		{
			get
			{
				return this.contextHash ^ 1006562692;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060024C9 RID: 9417 RVA: 0x000EA6FD File Offset: 0x000E88FD
		public Rect Rect
		{
			get
			{
				return this.currentRect;
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000EA708 File Offset: 0x000E8908
		public RectDivider(Rect parent, int contextHash, Vector2? margin = null)
		{
			this.currentRect = parent;
			this.margin = (margin ?? new Vector2(17f, 4f));
			this.contextHash = contextHash;
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x000EA6FD File Offset: 0x000E88FD
		public static implicit operator Rect(RectDivider rect)
		{
			return rect.currentRect;
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000EA74C File Offset: 0x000E894C
		public RectDivider NewRow(float height, VerticalJustification justification = VerticalJustification.Top)
		{
			if (justification == VerticalJustification.Top)
			{
				Rect parent;
				Rect rect;
				float num;
				if (!this.currentRect.SplitHorizontallyWithMargin(out parent, out rect, out num, this.margin.y, new float?(height), null))
				{
					Log.ErrorOnce(string.Format("Rect height was too small by {0} for the requested row height.", num), this.ErrorHash);
				}
				this.currentRect = rect;
				return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
			}
			if (justification == VerticalJustification.Bottom)
			{
				Rect rect2;
				Rect parent2;
				float num2;
				if (!this.currentRect.SplitHorizontallyWithMargin(out rect2, out parent2, out num2, this.margin.y, null, new float?(height)))
				{
					Log.ErrorOnce(string.Format("Rect height was too small by {0} for the requested rows height.", num2), this.ErrorHash);
				}
				this.currentRect = rect2;
				return new RectDivider(parent2, this.contextHash, new Vector2?(this.margin));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000EA838 File Offset: 0x000E8A38
		public RectDivider NewCol(float width, HorizontalJustification justification = HorizontalJustification.Left)
		{
			if (justification == HorizontalJustification.Left)
			{
				Rect parent;
				Rect rect;
				float num;
				if (!this.currentRect.SplitVerticallyWithMargin(out parent, out rect, out num, this.margin.x, new float?(width), null))
				{
					Log.ErrorOnce(string.Format("Rect width was too small by {0} for the requested column width.", num), this.ErrorHash);
				}
				this.currentRect = rect;
				return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
			}
			if (justification == HorizontalJustification.Right)
			{
				Rect rect2;
				Rect parent2;
				float num2;
				if (!this.currentRect.SplitVerticallyWithMargin(out rect2, out parent2, out num2, this.margin.x, null, new float?(width)))
				{
					Log.ErrorOnce(string.Format("Rect width was too small by {0} for the requested column width.", num2), this.ErrorHash);
				}
				this.currentRect = rect2;
				return new RectDivider(parent2, this.contextHash, new Vector2?(this.margin));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x000EA924 File Offset: 0x000E8B24
		public RectDivider CreateViewRect(int count, float rowHeight)
		{
			float num = 0f;
			if (count > 0)
			{
				num = (float)count * rowHeight + this.margin.y * (float)(count - 1);
			}
			float num2 = this.Rect.width;
			if (num > this.Rect.height)
			{
				num2 -= 16f;
			}
			return new RectDivider(new Rect(0f, 0f, num2, num), this.contextHash, null);
		}

		// Token: 0x0400179B RID: 6043
		private Rect currentRect;

		// Token: 0x0400179C RID: 6044
		private Vector2 margin;

		// Token: 0x0400179D RID: 6045
		private int contextHash;
	}
}

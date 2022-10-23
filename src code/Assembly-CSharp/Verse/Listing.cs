using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B1 RID: 1201
	public abstract class Listing
	{
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x000E70C2 File Offset: 0x000E52C2
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x0600242E RID: 9262 RVA: 0x000E70CA File Offset: 0x000E52CA
		public float MaxColumnHeightSeen
		{
			get
			{
				return Math.Max(this.CurHeight, this.maxHeightColumnSeen);
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x000E70ED File Offset: 0x000E52ED
		// (set) Token: 0x0600242F RID: 9263 RVA: 0x000E70DD File Offset: 0x000E52DD
		public float ColumnWidth
		{
			get
			{
				return this.columnWidthInt;
			}
			set
			{
				this.columnWidthInt = value;
				this.hasCustomColumnWidth = true;
			}
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000E70F5 File Offset: 0x000E52F5
		public void NewColumn()
		{
			this.maxHeightColumnSeen = Math.Max(this.curY, this.maxHeightColumnSeen);
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000E7132 File Offset: 0x000E5332
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.maxOneColumn)
			{
				return;
			}
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000E7158 File Offset: 0x000E5358
		public Rect GetRect(float height, float widthPct = 1f)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth * widthPct, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000E7189 File Offset: 0x000E5389
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000E719C File Offset: 0x000E539C
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000E7206 File Offset: 0x000E5406
		public void Indent(float gapWidth = 12f)
		{
			this.curX += gapWidth;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000E7216 File Offset: 0x000E5416
		public void Outdent(float gapWidth = 12f)
		{
			this.curX -= gapWidth;
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000E7228 File Offset: 0x000E5428
		public virtual void Begin(Rect rect)
		{
			this.listingRect = rect;
			if (this.hasCustomColumnWidth)
			{
				if (this.columnWidthInt > this.listingRect.width)
				{
					Log.Error(string.Concat(new object[]
					{
						"Listing set ColumnWith to ",
						this.columnWidthInt,
						" which is more than the whole listing rect width of ",
						this.listingRect.width,
						". Clamping."
					}));
					this.columnWidthInt = this.listingRect.width;
				}
			}
			else
			{
				this.columnWidthInt = this.listingRect.width;
			}
			this.curX = 0f;
			this.curY = 0f;
			this.maxHeightColumnSeen = 0f;
			Widgets.BeginGroup(rect);
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000E72EB File Offset: 0x000E54EB
		public virtual void End()
		{
			Widgets.EndGroup();
		}

		// Token: 0x04001741 RID: 5953
		public float verticalSpacing = 2f;

		// Token: 0x04001742 RID: 5954
		protected Rect listingRect;

		// Token: 0x04001743 RID: 5955
		protected float curY;

		// Token: 0x04001744 RID: 5956
		protected float curX;

		// Token: 0x04001745 RID: 5957
		private float columnWidthInt;

		// Token: 0x04001746 RID: 5958
		private bool hasCustomColumnWidth;

		// Token: 0x04001747 RID: 5959
		private float maxHeightColumnSeen;

		// Token: 0x04001748 RID: 5960
		public bool maxOneColumn;

		// Token: 0x04001749 RID: 5961
		public const float ColumnSpacing = 17f;

		// Token: 0x0400174A RID: 5962
		public const float DefaultGap = 12f;

		// Token: 0x0400174B RID: 5963
		public const float DefaultIndent = 12f;
	}
}

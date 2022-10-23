using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004A3 RID: 1187
	public class GridLayout
	{
		// Token: 0x060023B5 RID: 9141 RVA: 0x000E4AB4 File Offset: 0x000E2CB4
		public GridLayout(Rect container, int cols = 1, int rows = 1, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.container = new Rect(container);
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			float num = container.width - outerPadding * 2f - (float)(cols - 1) * innerPadding;
			float num2 = container.height - outerPadding * 2f - (float)(rows - 1) * innerPadding;
			this.colWidth = num / (float)cols;
			this.rowHeight = num2 / (float)rows;
			this.colStride = this.colWidth + innerPadding;
			this.rowStride = this.rowHeight + innerPadding;
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000E4B50 File Offset: 0x000E2D50
		public GridLayout(float colWidth, float rowHeight, int cols, int rows, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.colWidth = colWidth;
			this.rowHeight = rowHeight;
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			this.colStride = colWidth + innerPadding;
			this.rowStride = rowHeight + innerPadding;
			this.container = new Rect(0f, 0f, outerPadding * 2f + colWidth * (float)cols + innerPadding * (float)cols - 1f, outerPadding * 2f + rowHeight * (float)rows + innerPadding * (float)rows - 1f);
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x000E4BE8 File Offset: 0x000E2DE8
		public Rect GetCellRectByIndex(int index, int colspan = 1, int rowspan = 1)
		{
			int col = index % this.cols;
			int row = index / this.cols;
			return this.GetCellRect(col, row, colspan, rowspan);
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000E4C14 File Offset: 0x000E2E14
		public Rect GetCellRect(int col, int row, int colspan = 1, int rowspan = 1)
		{
			return new Rect(Mathf.Floor(this.container.x + this.outerPadding + (float)col * this.colStride), Mathf.Floor(this.container.y + this.outerPadding + (float)row * this.rowStride), Mathf.Ceil(this.colWidth) * (float)colspan + this.innerPadding * (float)(colspan - 1), Mathf.Ceil(this.rowHeight) * (float)rowspan + this.innerPadding * (float)(rowspan - 1));
		}

		// Token: 0x04001711 RID: 5905
		public Rect container;

		// Token: 0x04001712 RID: 5906
		private int cols;

		// Token: 0x04001713 RID: 5907
		private float outerPadding;

		// Token: 0x04001714 RID: 5908
		private float innerPadding;

		// Token: 0x04001715 RID: 5909
		private float colStride;

		// Token: 0x04001716 RID: 5910
		private float rowStride;

		// Token: 0x04001717 RID: 5911
		private float colWidth;

		// Token: 0x04001718 RID: 5912
		private float rowHeight;
	}
}

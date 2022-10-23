using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C5 RID: 1221
	public class ScrollPositioner
	{
		// Token: 0x060024D3 RID: 9427 RVA: 0x000EAA2B File Offset: 0x000E8C2B
		public void Arm(bool armed = true)
		{
			this.armed = armed;
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000EAA34 File Offset: 0x000E8C34
		public void ClearInterestRects()
		{
			this.interestRect = null;
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000EAA42 File Offset: 0x000E8C42
		public void RegisterInterestRect(Rect rect)
		{
			if (this.interestRect != null)
			{
				this.interestRect = new Rect?(rect.Union(this.interestRect.Value));
				return;
			}
			this.interestRect = new Rect?(rect);
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x000EAA7A File Offset: 0x000E8C7A
		public void ScrollHorizontally(ref Vector2 scrollPos, Vector2 outRectSize)
		{
			this.Scroll(ref scrollPos, outRectSize, true, false);
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x000EAA86 File Offset: 0x000E8C86
		public void ScrollVertically(ref Vector2 scrollPos, Vector2 outRectSize)
		{
			this.Scroll(ref scrollPos, outRectSize, false, true);
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x000EAA94 File Offset: 0x000E8C94
		public void Scroll(ref Vector2 scrollPos, Vector2 outRectSize, bool scrollHorizontally = true, bool scrollVertically = true)
		{
			if (Event.current.type != EventType.Layout)
			{
				return;
			}
			if (!this.armed)
			{
				return;
			}
			this.armed = false;
			if (this.interestRect == null)
			{
				return;
			}
			if (scrollHorizontally)
			{
				this.ScrollInDimension(ref scrollPos.x, outRectSize.x, this.interestRect.Value.xMin, this.interestRect.Value.xMax);
			}
			if (scrollVertically)
			{
				this.ScrollInDimension(ref scrollPos.y, outRectSize.y, this.interestRect.Value.yMin, this.interestRect.Value.yMax);
			}
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000EAB44 File Offset: 0x000E8D44
		private void ScrollInDimension(ref float scrollPos, float scrollViewSize, float v0, float v1)
		{
			float num = v1 - v0;
			if (num <= scrollViewSize)
			{
				scrollPos = v0 + num / 2f - scrollViewSize / 2f;
				return;
			}
			scrollPos = v0;
		}

		// Token: 0x0400179F RID: 6047
		private Rect? interestRect;

		// Token: 0x040017A0 RID: 6048
		private bool armed;
	}
}

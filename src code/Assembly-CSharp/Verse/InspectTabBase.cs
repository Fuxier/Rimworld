using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000075 RID: 117
	public abstract class InspectTabBase
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000494 RID: 1172
		protected abstract float PaneTopY { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000495 RID: 1173
		protected abstract bool StillValid { get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool Hidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001A16D File Offset: 0x0001836D
		public string TutorHighlightTagClosed
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorHighlightTagClosed == null)
				{
					this.cachedTutorHighlightTagClosed = "ITab-" + this.tutorTag + "-Closed";
				}
				return this.cachedTutorHighlightTagClosed;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0001A1A4 File Offset: 0x000183A4
		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001A1F4 File Offset: 0x000183F4
		public void DoTabGUI()
		{
			Rect rect = this.TabRect;
			Find.WindowStack.ImmediateWindow(235086, rect, WindowLayer.GameUI, delegate
			{
				if (!this.StillValid || !this.IsVisible)
				{
					return;
				}
				if (Widgets.CloseButtonFor(rect.AtZero()))
				{
					this.CloseTab();
				}
				try
				{
					this.FillTab();
				}
				catch (Exception ex)
				{
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Exception filling tab ",
						this.GetType(),
						": ",
						ex
					}), 49827);
				}
			}, true, false, 1f, new Action(this.Notify_ClickOutsideWindow));
			this.ExtraOnGUI();
		}

		// Token: 0x0600049B RID: 1179
		protected abstract void CloseTab();

		// Token: 0x0600049C RID: 1180
		protected abstract void FillTab();

		// Token: 0x0600049D RID: 1181 RVA: 0x000034B7 File Offset: 0x000016B7
		protected virtual void ExtraOnGUI()
		{
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000034B7 File Offset: 0x000016B7
		protected virtual void UpdateSize()
		{
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void OnOpen()
		{
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void TabTick()
		{
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void TabUpdate()
		{
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_ClearingAllMapsMemory()
		{
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_ClickOutsideWindow()
		{
		}

		// Token: 0x04000213 RID: 531
		public string labelKey;

		// Token: 0x04000214 RID: 532
		protected Vector2 size;

		// Token: 0x04000215 RID: 533
		public string tutorTag;

		// Token: 0x04000216 RID: 534
		private string cachedTutorHighlightTagClosed;
	}
}

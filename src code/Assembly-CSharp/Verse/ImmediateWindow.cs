using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000504 RID: 1284
	public class ImmediateWindow : Window
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002710 RID: 10000 RVA: 0x000FB1D4 File Offset: 0x000F93D4
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002711 RID: 10001 RVA: 0x00004E2A File Offset: 0x0000302A
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x000FB1E4 File Offset: 0x000F93E4
		public ImmediateWindow()
		{
			this.doCloseButton = false;
			this.doCloseX = false;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.focusWhenOpened = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000FB236 File Offset: 0x000F9436
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000FB243 File Offset: 0x000F9443
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			Action action = this.doClickOutsideFunc;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x040019B2 RID: 6578
		public Action doWindowFunc;

		// Token: 0x040019B3 RID: 6579
		public Action doClickOutsideFunc;
	}
}

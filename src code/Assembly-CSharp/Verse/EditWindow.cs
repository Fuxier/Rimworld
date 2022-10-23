using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000503 RID: 1283
	public abstract class EditWindow : Window
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x0600270C RID: 9996 RVA: 0x000FB038 File Offset: 0x000F9238
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x0600270D RID: 9997 RVA: 0x000FB049 File Offset: 0x000F9249
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000FB050 File Offset: 0x000F9250
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000FB0A0 File Offset: 0x000F92A0
		public override void PostOpen()
		{
			while (this.windowRect.x <= (float)UI.screenWidth - 200f && this.windowRect.y <= (float)UI.screenHeight - 200f)
			{
				bool flag = false;
				foreach (EditWindow editWindow in (from di in Find.WindowStack.Windows
				where di is EditWindow
				select di).Cast<EditWindow>())
				{
					if (editWindow != this && Mathf.Abs(editWindow.windowRect.x - this.windowRect.x) < 8f && Mathf.Abs(editWindow.windowRect.y - this.windowRect.y) < 8f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				this.windowRect.x = this.windowRect.x + 16f;
				this.windowRect.y = this.windowRect.y + 16f;
			}
		}

		// Token: 0x040019AF RID: 6575
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x040019B0 RID: 6576
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x040019B1 RID: 6577
		private const float SuperimposeAvoidOffsetMinEdge = 200f;
	}
}

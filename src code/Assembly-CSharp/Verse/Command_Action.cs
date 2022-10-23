using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000494 RID: 1172
	public class Command_Action : Command
	{
		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002376 RID: 9078 RVA: 0x000E3230 File Offset: 0x000E1430
		public override Color IconDrawColor
		{
			get
			{
				Color? color = this.iconDrawColorOverride;
				if (color == null)
				{
					return base.IconDrawColor;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000E325B File Offset: 0x000E145B
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x000E326F File Offset: 0x000E146F
		public override void GizmoUpdateOnMouseover()
		{
			if (this.onHover != null)
			{
				this.onHover();
			}
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000E3284 File Offset: 0x000E1484
		public void SetColorOverride(Color color)
		{
			this.iconDrawColorOverride = new Color?(color);
		}

		// Token: 0x040016CD RID: 5837
		public Action action;

		// Token: 0x040016CE RID: 5838
		public Action onHover;

		// Token: 0x040016CF RID: 5839
		private Color? iconDrawColorOverride;
	}
}

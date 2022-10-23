using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200010D RID: 269
	public class MechWorkModeDef : Def
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00025B3C File Offset: 0x00023D3C
		public WorkModeDrawer Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WorkModeDrawer)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00025B6E File Offset: 0x00023D6E
		public override void PostLoad()
		{
			if (!string.IsNullOrEmpty(this.iconPath))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x04000684 RID: 1668
		[NoTranslate]
		public string iconPath;

		// Token: 0x04000685 RID: 1669
		public Texture2D uiIcon;

		// Token: 0x04000686 RID: 1670
		public int uiOrder;

		// Token: 0x04000687 RID: 1671
		public bool ignoreGroupChargeLimits;

		// Token: 0x04000688 RID: 1672
		public Type workerClass = typeof(WorkModeDrawer);

		// Token: 0x04000689 RID: 1673
		[Unsaved(false)]
		private WorkModeDrawer workerInt;
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001CA RID: 458
	public struct FleckAttachLink
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00047C10 File Offset: 0x00045E10
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00047C1D File Offset: 0x00045E1D
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00047C25 File Offset: 0x00045E25
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00047C2D File Offset: 0x00045E2D
		public FleckAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.detachAfterTicks = -1;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00047C58 File Offset: 0x00045E58
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing)
			{
				this.lastDrawPosInt = this.targetInt.Thing.DrawPos;
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted();
		}

		// Token: 0x04000BBD RID: 3005
		private TargetInfo targetInt;

		// Token: 0x04000BBE RID: 3006
		private Vector3 lastDrawPosInt;

		// Token: 0x04000BBF RID: 3007
		public int detachAfterTicks;

		// Token: 0x04000BC0 RID: 3008
		public static readonly FleckAttachLink Invalid = new FleckAttachLink(TargetInfo.Invalid);
	}
}

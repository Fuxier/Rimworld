using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F4 RID: 1012
	public struct MoteAttachLink
	{
		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001CD0 RID: 7376 RVA: 0x000AEF23 File Offset: 0x000AD123
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x000AEF30 File Offset: 0x000AD130
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001CD2 RID: 7378 RVA: 0x000AEF38 File Offset: 0x000AD138
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000AEF40 File Offset: 0x000AD140
		public MoteAttachLink(TargetInfo target, Vector3 offset)
		{
			this.targetInt = target;
			this.offsetInt = offset;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000AEF6A File Offset: 0x000AD16A
		public void UpdateTarget(TargetInfo target, Vector3 offset)
		{
			this.targetInt = target;
			this.offsetInt = offset;
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x000AEF7C File Offset: 0x000AD17C
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing && this.targetInt.Thing.SpawnedOrAnyParentSpawned)
			{
				this.lastDrawPosInt = this.targetInt.Thing.SpawnedParentOrMe.DrawPos + this.offsetInt;
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted() + this.offsetInt;
		}

		// Token: 0x04001474 RID: 5236
		private TargetInfo targetInt;

		// Token: 0x04001475 RID: 5237
		private Vector3 offsetInt;

		// Token: 0x04001476 RID: 5238
		private Vector3 lastDrawPosInt;

		// Token: 0x04001477 RID: 5239
		public static readonly MoteAttachLink Invalid = new MoteAttachLink(TargetInfo.Invalid, Vector3.zero);
	}
}

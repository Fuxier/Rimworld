using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F0 RID: 1008
	public class JitterHandler
	{
		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x000AE658 File Offset: 0x000AC858
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x000AE660 File Offset: 0x000AC860
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			float num = (float)ticksPassed * this.JitterDropPerTick;
			if (this.curOffset.sqrMagnitude < num * num)
			{
				this.curOffset = new Vector3(0f, 0f, 0f);
				return;
			}
			this.curOffset -= this.curOffset.normalized * num;
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x000AE6C4 File Offset: 0x000AC8C4
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000AE6E7 File Offset: 0x000AC8E7
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x000AE70C File Offset: 0x000AC90C
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}

		// Token: 0x04001459 RID: 5209
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400145A RID: 5210
		private float DamageJitterDistance = 0.17f;

		// Token: 0x0400145B RID: 5211
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x0400145C RID: 5212
		private float JitterDropPerTick = 0.018f;

		// Token: 0x0400145D RID: 5213
		private float JitterMax = 0.35f;
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F6 RID: 1014
	public class MoteDualAttached : Mote
	{
		// Token: 0x06001CDB RID: 7387 RVA: 0x000AF2BF File Offset: 0x000AD4BF
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a, Vector3.zero);
			this.link2 = new MoteAttachLink(b, Vector3.zero);
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000AF2E3 File Offset: 0x000AD4E3
		public void Attach(TargetInfo a, TargetInfo b, Vector3 offsetA, Vector3 offsetB)
		{
			this.link1 = new MoteAttachLink(a, offsetA);
			this.link2 = new MoteAttachLink(b, offsetB);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000AF300 File Offset: 0x000AD500
		public override void Draw()
		{
			this.UpdatePositionAndRotation();
			base.Draw();
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000AF30E File Offset: 0x000AD50E
		public void UpdateTargets(TargetInfo a, TargetInfo b, Vector3 offsetA, Vector3 offsetB)
		{
			this.link1.UpdateTarget(a, offsetA);
			this.link2.UpdateTarget(b, offsetB);
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000AF32C File Offset: 0x000AD52C
		protected void UpdatePositionAndRotation()
		{
			if (this.link1.Linked)
			{
				if (this.link2.Linked)
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					if (!this.link2.Target.ThingDestroyed)
					{
						this.link2.UpdateDrawPos();
					}
					this.exactPosition = (this.link1.LastDrawPos + this.link2.LastDrawPos) * 0.5f;
					if (this.def.mote.rotateTowardsTarget)
					{
						this.exactRotation = this.link1.LastDrawPos.AngleToFlat(this.link2.LastDrawPos) + 90f;
					}
					if (this.def.mote.scaleToConnectTargets)
					{
						this.exactScale = new Vector3(this.def.graphicData.drawSize.y, 1f, (this.link2.LastDrawPos - this.link1.LastDrawPos).MagnitudeHorizontal());
					}
				}
				else
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					this.exactPosition = this.link1.LastDrawPos + this.def.mote.attachedDrawOffset;
				}
			}
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x04001479 RID: 5241
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;
	}
}

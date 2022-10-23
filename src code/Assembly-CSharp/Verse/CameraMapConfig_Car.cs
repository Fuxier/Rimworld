using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000093 RID: 147
	public class CameraMapConfig_Car : CameraMapConfig
	{
		// Token: 0x06000534 RID: 1332 RVA: 0x0001CFF2 File Offset: 0x0001B1F2
		public CameraMapConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001D028 File Offset: 0x0001B228
		public override void ConfigFixedUpdate_60(ref Vector3 rootPos, ref Vector3 velocity)
		{
			float num = 0.016666668f;
			if (KeyBindingDefOf.MapDolly_Left.IsDown)
			{
				this.autoPanTargetAngle += 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Right.IsDown)
			{
				this.autoPanTargetAngle -= 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Up.IsDown)
			{
				this.autoPanSpeed += 1.2f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.autoPanSpeed -= 1.2f * num;
				if (this.autoPanSpeed < 0f)
				{
					this.autoPanSpeed = 0f;
				}
			}
			this.autoPanAngle = Mathf.Lerp(this.autoPanAngle, this.autoPanTargetAngle, 0.02f);
			base.ConfigFixedUpdate_60(ref rootPos, ref velocity);
		}

		// Token: 0x04000274 RID: 628
		private const float SpeedChangeSpeed = 1.2f;

		// Token: 0x04000275 RID: 629
		private const float AngleChangeSpeed = 0.72f;
	}
}

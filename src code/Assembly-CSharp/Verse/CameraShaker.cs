using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200009D RID: 157
	public class CameraShaker
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x0001DD63 File Offset: 0x0001BF63
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x0001DD6B File Offset: 0x0001BF6B
		public float CurShakeMag
		{
			get
			{
				return this.curShakeMag;
			}
			set
			{
				this.curShakeMag = Mathf.Clamp(value, 0f, this.GetMaxShakeMag());
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001DD84 File Offset: 0x0001BF84
		public Vector3 ShakeOffset
		{
			get
			{
				float x = Mathf.Sin(Time.realtimeSinceStartup * 24f) * this.curShakeMag;
				float y = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.05f) * this.curShakeMag;
				float z = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.1f) * this.curShakeMag;
				return new Vector3(x, y, z);
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001DDEB File Offset: 0x0001BFEB
		public void DoShake(float mag)
		{
			if (mag <= 0f)
			{
				return;
			}
			this.CurShakeMag += mag;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001DE04 File Offset: 0x0001C004
		public float GetMaxShakeMag()
		{
			return 0.2f * Prefs.ScreenShakeIntensity;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001DE11 File Offset: 0x0001C011
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001DE25 File Offset: 0x0001C025
		public void Update()
		{
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}

		// Token: 0x04000284 RID: 644
		private float curShakeMag;

		// Token: 0x04000285 RID: 645
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x04000286 RID: 646
		private const float ShakeFrequency = 24f;

		// Token: 0x04000287 RID: 647
		private const float MaxShakeMag = 0.2f;
	}
}

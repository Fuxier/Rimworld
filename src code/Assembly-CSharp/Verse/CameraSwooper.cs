using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200009F RID: 159
	public class CameraSwooper
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x0001DE57 File Offset: 0x0001C057
		public void StartSwoopFromRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.Swooping = true;
			this.TimeSinceSwoopStart = 0f;
			this.FinalOffset = FinalOffset;
			this.FinalOrthoSizeOffset = FinalOrthoSizeOffset;
			this.TotalSwoopTime = TotalSwoopTime;
			this.SwoopFinishedCallback = SwoopFinishedCallback;
			this.SwoopingTo = false;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001DE8F File Offset: 0x0001C08F
		public void StartSwoopToRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.StartSwoopFromRoot(FinalOffset, FinalOrthoSizeOffset, TotalSwoopTime, SwoopFinishedCallback);
			this.SwoopingTo = true;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001DEA4 File Offset: 0x0001C0A4
		public void Update()
		{
			if (this.Swooping)
			{
				this.TimeSinceSwoopStart += Time.deltaTime;
				if (this.TimeSinceSwoopStart >= this.TotalSwoopTime)
				{
					this.Swooping = false;
					if (this.SwoopFinishedCallback != null)
					{
						this.SwoopFinishedCallback();
					}
				}
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001DEF4 File Offset: 0x0001C0F4
		public void OffsetCameraFrom(GameObject camObj, Vector3 basePos, float baseSize)
		{
			float num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
			if (!this.Swooping)
			{
				num = 0f;
			}
			else
			{
				num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
				if (this.SwoopingTo)
				{
					num = 1f - num;
				}
				num = (float)Math.Pow((double)num, 1.7000000476837158);
			}
			camObj.transform.position = basePos + this.FinalOffset * num;
			Find.Camera.orthographicSize = baseSize + this.FinalOrthoSizeOffset * num;
		}

		// Token: 0x04000288 RID: 648
		public bool Swooping;

		// Token: 0x04000289 RID: 649
		private bool SwoopingTo;

		// Token: 0x0400028A RID: 650
		private float TimeSinceSwoopStart;

		// Token: 0x0400028B RID: 651
		private Vector3 FinalOffset;

		// Token: 0x0400028C RID: 652
		private float FinalOrthoSizeOffset;

		// Token: 0x0400028D RID: 653
		private float TotalSwoopTime;

		// Token: 0x0400028E RID: 654
		private SwoopCallbackMethod SwoopFinishedCallback;
	}
}

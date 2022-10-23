using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200009C RID: 156
	public struct CameraPanner
	{
		// Token: 0x06000550 RID: 1360 RVA: 0x0001DC26 File Offset: 0x0001BE26
		public void PanTo(CameraPanner.Interpolant source, CameraPanner.Interpolant destination, float duration = 0.25f, PanCompletionCallback completion = null)
		{
			this.Moving = true;
			this.TimeSinceStart = 0f;
			this.Source = source;
			this.Destination = destination;
			this.Duration = duration;
			this.Completion = completion;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001DC57 File Offset: 0x0001BE57
		public void JumpOnNextUpdate()
		{
			this.TimeSinceStart = this.Duration;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001DC68 File Offset: 0x0001BE68
		public CameraPanner.Interpolant? Update()
		{
			if (!this.Moving)
			{
				return null;
			}
			this.TimeSinceStart += Time.deltaTime;
			if (this.TimeSinceStart >= this.Duration)
			{
				this.Moving = false;
				PanCompletionCallback completion = this.Completion;
				if (completion != null)
				{
					completion();
				}
				this.Completion = null;
			}
			return new CameraPanner.Interpolant?(this.GetCurrentInterpolation());
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001DCD4 File Offset: 0x0001BED4
		private CameraPanner.Interpolant GetCurrentInterpolation()
		{
			float t = CameraPanner.SmootherStep(this.TimeSinceStart / this.Duration);
			return new CameraPanner.Interpolant(Vector3.LerpUnclamped(this.Source.Position, this.Destination.Position, t), Mathf.LerpUnclamped(this.Source.Size, this.Destination.Size, t));
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001DD34 File Offset: 0x0001BF34
		public static float SmootherStep(float t)
		{
			float num = Mathf.Clamp01(t);
			return num * num * num * (num * (num * 6f - 15f) + 10f);
		}

		// Token: 0x0400027D RID: 637
		public bool Moving;

		// Token: 0x0400027E RID: 638
		private float TimeSinceStart;

		// Token: 0x0400027F RID: 639
		private CameraPanner.Interpolant Source;

		// Token: 0x04000280 RID: 640
		private CameraPanner.Interpolant Destination;

		// Token: 0x04000281 RID: 641
		private float Duration;

		// Token: 0x04000282 RID: 642
		private PanCompletionCallback Completion;

		// Token: 0x04000283 RID: 643
		public const float DefaultDuration = 0.25f;

		// Token: 0x02001CA9 RID: 7337
		public readonly struct Interpolant
		{
			// Token: 0x0600B030 RID: 45104 RVA: 0x003FF8B1 File Offset: 0x003FDAB1
			public Interpolant(Vector3 position, float size)
			{
				this.Position = position;
				this.Size = size;
			}

			// Token: 0x17001D9C RID: 7580
			// (get) Token: 0x0600B031 RID: 45105 RVA: 0x003FF8C1 File Offset: 0x003FDAC1
			public Vector3 Position { get; }

			// Token: 0x17001D9D RID: 7581
			// (get) Token: 0x0600B032 RID: 45106 RVA: 0x003FF8C9 File Offset: 0x003FDAC9
			public float Size { get; }
		}
	}
}

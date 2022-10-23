using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F7 RID: 1015
	public class MoteLeaf : Mote
	{
		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x000AF4CD File Offset: 0x000AD6CD
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + base.SolidTime + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001CE2 RID: 7394 RVA: 0x000AF4FF File Offset: 0x000AD6FF
		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x000AF514 File Offset: 0x000AD714
		public override float Alpha
		{
			get
			{
				float num = base.AgeSecs;
				if (num <= this.spawnDelay)
				{
					return 0f;
				}
				num -= this.spawnDelay;
				if (num <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return num / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (num <= this.FallTime + base.SolidTime)
					{
						return 1f;
					}
					num -= this.FallTime + base.SolidTime;
					if (num <= this.def.mote.fadeOutTime)
					{
						return 1f - Mathf.InverseLerp(0f, this.def.mote.fadeOutTime, num);
					}
					num -= this.def.mote.fadeOutTime;
					return 0f;
				}
			}
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000AF5F6 File Offset: 0x000AD7F6
		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x000AF620 File Offset: 0x000AD820
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			float ageSecs = base.AgeSecs;
			this.exactPosition = this.startSpatialPosition;
			if (ageSecs > this.spawnDelay)
			{
				this.exactPosition.y = this.exactPosition.y - MoteLeaf.FallSpeed * (ageSecs - this.spawnDelay);
			}
			this.exactPosition.y = Mathf.Max(this.exactPosition.y, 0f);
			this.currentSpatialPosition = this.exactPosition;
			this.exactPosition.z = this.exactPosition.z + this.exactPosition.y;
			this.exactPosition.y = 0f;
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000AF6CC File Offset: 0x000AD8CC
		public override void Draw()
		{
			base.Draw(this.front ? (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f) : this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x0400147A RID: 5242
		private Vector3 startSpatialPosition;

		// Token: 0x0400147B RID: 5243
		private Vector3 currentSpatialPosition;

		// Token: 0x0400147C RID: 5244
		private float spawnDelay;

		// Token: 0x0400147D RID: 5245
		private bool front;

		// Token: 0x0400147E RID: 5246
		private float treeHeight;

		// Token: 0x0400147F RID: 5247
		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;
	}
}

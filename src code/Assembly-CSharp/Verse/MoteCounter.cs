using System;

namespace Verse
{
	// Token: 0x02000074 RID: 116
	public class MoteCounter
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x0001A118 File Offset: 0x00018318
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0001A120 File Offset: 0x00018320
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0001A12F File Offset: 0x0001832F
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001A13E File Offset: 0x0001833E
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001A14D File Offset: 0x0001834D
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001A15D File Offset: 0x0001835D
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}

		// Token: 0x04000211 RID: 529
		private int moteCount;

		// Token: 0x04000212 RID: 530
		private const int SaturatedCount = 250;
	}
}

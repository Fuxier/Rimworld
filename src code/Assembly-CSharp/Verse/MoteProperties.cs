using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CD RID: 205
	public class MoteProperties
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00021B7F File Offset: 0x0001FD7F
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x040003E1 RID: 993
		public bool realTime;

		// Token: 0x040003E2 RID: 994
		public float fadeInTime;

		// Token: 0x040003E3 RID: 995
		public float solidTime = 1f;

		// Token: 0x040003E4 RID: 996
		public float fadeOutTime;

		// Token: 0x040003E5 RID: 997
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x040003E6 RID: 998
		public float speedPerTime;

		// Token: 0x040003E7 RID: 999
		public float growthRate;

		// Token: 0x040003E8 RID: 1000
		public bool collide;

		// Token: 0x040003E9 RID: 1001
		public float archHeight;

		// Token: 0x040003EA RID: 1002
		public float archDuration;

		// Token: 0x040003EB RID: 1003
		public float archStartOffset;

		// Token: 0x040003EC RID: 1004
		public SoundDef landSound;

		// Token: 0x040003ED RID: 1005
		public Vector3 unattachedDrawOffset = Vector3.zero;

		// Token: 0x040003EE RID: 1006
		public Vector3 attachedDrawOffset;

		// Token: 0x040003EF RID: 1007
		public bool needsMaintenance;

		// Token: 0x040003F0 RID: 1008
		public bool rotateTowardsTarget;

		// Token: 0x040003F1 RID: 1009
		public bool rotateTowardsMoveDirection;

		// Token: 0x040003F2 RID: 1010
		public bool scaleToConnectTargets;

		// Token: 0x040003F3 RID: 1011
		public bool attachedToHead;

		// Token: 0x040003F4 RID: 1012
		public bool fadeOutUnmaintained;
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C7 RID: 455
	public struct FleckCreationData
	{
		// Token: 0x04000BAB RID: 2987
		public FleckDef def;

		// Token: 0x04000BAC RID: 2988
		public Vector3 spawnPosition;

		// Token: 0x04000BAD RID: 2989
		public float rotation;

		// Token: 0x04000BAE RID: 2990
		public float scale;

		// Token: 0x04000BAF RID: 2991
		public Vector3? exactScale;

		// Token: 0x04000BB0 RID: 2992
		public Color? instanceColor;

		// Token: 0x04000BB1 RID: 2993
		public float velocityAngle;

		// Token: 0x04000BB2 RID: 2994
		public float velocitySpeed;

		// Token: 0x04000BB3 RID: 2995
		public Vector3? velocity;

		// Token: 0x04000BB4 RID: 2996
		public float rotationRate;

		// Token: 0x04000BB5 RID: 2997
		public float? solidTimeOverride;

		// Token: 0x04000BB6 RID: 2998
		public float? airTimeLeft;

		// Token: 0x04000BB7 RID: 2999
		public int ageTicksOverride;

		// Token: 0x04000BB8 RID: 3000
		public FleckAttachLink link;

		// Token: 0x04000BB9 RID: 3001
		public float targetSize;
	}
}

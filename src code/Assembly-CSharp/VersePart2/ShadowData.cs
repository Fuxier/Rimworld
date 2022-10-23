using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000217 RID: 535
	public class ShadowData
	{
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x00058CC6 File Offset: 0x00056EC6
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x00058CD3 File Offset: 0x00056ED3
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x00058CE0 File Offset: 0x00056EE0
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}

		// Token: 0x04000DB4 RID: 3508
		public Vector3 volume = Vector3.one;

		// Token: 0x04000DB5 RID: 3509
		public Vector3 offset = Vector3.zero;
	}
}

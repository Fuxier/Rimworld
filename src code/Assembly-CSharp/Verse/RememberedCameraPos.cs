using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A0 RID: 160
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x06000566 RID: 1382 RVA: 0x0001DF80 File Offset: 0x0001C180
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}

		// Token: 0x0400028F RID: 655
		public Vector3 rootPos;

		// Token: 0x04000290 RID: 656
		public float rootSize;
	}
}

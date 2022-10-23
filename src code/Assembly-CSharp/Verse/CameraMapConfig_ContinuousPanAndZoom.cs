using System;

namespace Verse
{
	// Token: 0x02000092 RID: 146
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x06000533 RID: 1331 RVA: 0x0001CFCD File Offset: 0x0001B1CD
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}

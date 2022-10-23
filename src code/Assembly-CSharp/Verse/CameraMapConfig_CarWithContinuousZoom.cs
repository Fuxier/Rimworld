using System;

namespace Verse
{
	// Token: 0x02000094 RID: 148
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x06000536 RID: 1334 RVA: 0x0001D0F7 File Offset: 0x0001B2F7
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}

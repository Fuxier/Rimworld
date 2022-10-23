using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200057F RID: 1407
	[StaticConstructorOnStartup]
	public static class MatBases
	{
		// Token: 0x04001C20 RID: 7200
		public static readonly Material LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay", -1);

		// Token: 0x04001C21 RID: 7201
		public static readonly Material SunShadow = MatLoader.LoadMat("Lighting/SunShadow", -1);

		// Token: 0x04001C22 RID: 7202
		public static readonly Material SunShadowFade = MatBases.SunShadow;

		// Token: 0x04001C23 RID: 7203
		public static readonly Material EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow", -1);

		// Token: 0x04001C24 RID: 7204
		public static readonly Material IndoorMask = MatLoader.LoadMat("Misc/IndoorMask", -1);

		// Token: 0x04001C25 RID: 7205
		public static readonly Material FogOfWar = MatLoader.LoadMat("Misc/FogOfWar", -1);

		// Token: 0x04001C26 RID: 7206
		public static readonly Material Snow = MatLoader.LoadMat("Misc/Snow", -1);
	}
}

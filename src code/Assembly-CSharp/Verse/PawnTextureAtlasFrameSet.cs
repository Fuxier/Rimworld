using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000382 RID: 898
	public class PawnTextureAtlasFrameSet
	{
		// Token: 0x060019ED RID: 6637 RVA: 0x0009C920 File Offset: 0x0009AB20
		public int GetIndex(Rot4 rotation, PawnDrawMode drawMode)
		{
			if (drawMode == PawnDrawMode.BodyAndHead)
			{
				return rotation.AsInt;
			}
			return 4 + rotation.AsInt;
		}

		// Token: 0x040012E2 RID: 4834
		public RenderTexture atlas;

		// Token: 0x040012E3 RID: 4835
		public Rect[] uvRects = new Rect[8];

		// Token: 0x040012E4 RID: 4836
		public Mesh[] meshes = new Mesh[8];

		// Token: 0x040012E5 RID: 4837
		public bool[] isDirty = new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};
	}
}

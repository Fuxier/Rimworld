using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F1 RID: 1009
	public static class GenView
	{
		// Token: 0x06001CB4 RID: 7348 RVA: 0x000AE7DD File Offset: 0x000AC9DD
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map, bool drawOffscreen = true)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map, drawOffscreen);
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x000AE7EC File Offset: 0x000AC9EC
		public static bool ShouldSpawnMotesAt(this IntVec3 loc, Map map, bool drawOffscreen = true)
		{
			if (map != Find.CurrentMap)
			{
				return false;
			}
			if (!loc.InBounds(map))
			{
				return false;
			}
			if (drawOffscreen)
			{
				return true;
			}
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			return GenView.viewRect.Contains(loc);
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x000AE83D File Offset: 0x000ACA3D
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}

		// Token: 0x0400145E RID: 5214
		private static CellRect viewRect;

		// Token: 0x0400145F RID: 5215
		private const int ViewRectMargin = 5;
	}
}

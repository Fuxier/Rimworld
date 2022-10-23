using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001FC RID: 508
	public class RoadInfo : MapComponent
	{
		// Token: 0x06000ED1 RID: 3793 RVA: 0x00051E34 File Offset: 0x00050034
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00051E48 File Offset: 0x00050048
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04000D48 RID: 3400
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}

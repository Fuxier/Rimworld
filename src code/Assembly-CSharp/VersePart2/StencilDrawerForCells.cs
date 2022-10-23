using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000388 RID: 904
	public class StencilDrawerForCells : IExposable
	{
		// Token: 0x06001A05 RID: 6661 RVA: 0x0009D364 File Offset: 0x0009B564
		public void Draw()
		{
			if (this.cells.NullOrEmpty<IntVec3>())
			{
				GenDraw.DrawStencilCell(this.center, GenDraw.RitualStencilMat, (float)this.dimensionsIfNoCells.x, (float)this.dimensionsIfNoCells.z);
				return;
			}
			foreach (IntVec3 intVec in this.cells)
			{
				GenDraw.DrawStencilCell(intVec.ToVector3Shifted(), GenDraw.RitualStencilMat, 1f, 1f);
			}
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0009D404 File Offset: 0x0009B604
		public void ExposeData()
		{
			Scribe_References.Look<Lord>(ref this.sourceLord, "sourceLord", false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<Vector3>(ref this.center, "center", default(Vector3), false);
			Scribe_Values.Look<IntVec2>(ref this.dimensionsIfNoCells, "dimensionsIfNoCells", default(IntVec2), false);
			Scribe_Values.Look<int>(ref this.ticksLeftWithoutLord, "ticksLeftWithoutLord", 0, false);
		}

		// Token: 0x04001303 RID: 4867
		public Lord sourceLord;

		// Token: 0x04001304 RID: 4868
		public List<IntVec3> cells;

		// Token: 0x04001305 RID: 4869
		public Vector3 center;

		// Token: 0x04001306 RID: 4870
		public IntVec2 dimensionsIfNoCells;

		// Token: 0x04001307 RID: 4871
		public int ticksLeftWithoutLord;
	}
}

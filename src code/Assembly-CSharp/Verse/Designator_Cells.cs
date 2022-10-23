using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000428 RID: 1064
	public abstract class Designator_Cells : Designator
	{
		// Token: 0x06001F73 RID: 8051 RVA: 0x000BB057 File Offset: 0x000B9257
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

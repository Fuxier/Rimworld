using System;

namespace Verse
{
	// Token: 0x02000060 RID: 96
	public class PawnStagePosition : IExposable
	{
		// Token: 0x06000455 RID: 1109 RVA: 0x00018214 File Offset: 0x00016414
		public PawnStagePosition()
		{
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00018227 File Offset: 0x00016427
		public PawnStagePosition(IntVec3 cell, Thing thing, Rot4 orientation, bool highlight)
		{
			this.cell = cell;
			this.thing = thing;
			this.orientation = orientation;
			this.highlight = highlight;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00018258 File Offset: 0x00016458
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<Rot4>(ref this.orientation, "orientation", Rot4.Invalid, false);
			Scribe_Values.Look<bool>(ref this.highlight, "highlight", false, false);
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
		}

		// Token: 0x0400016E RID: 366
		public IntVec3 cell;

		// Token: 0x0400016F RID: 367
		public Thing thing;

		// Token: 0x04000170 RID: 368
		public Rot4 orientation = Rot4.Invalid;

		// Token: 0x04000171 RID: 369
		public bool highlight;
	}
}

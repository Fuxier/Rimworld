using System;

namespace Verse
{
	// Token: 0x0200033C RID: 828
	public class HediffWithTarget : HediffWithComps
	{
		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001626 RID: 5670 RVA: 0x00082EE4 File Offset: 0x000810E4
		public override bool ShouldRemove
		{
			get
			{
				Pawn pawn;
				return this.target == null || ((pawn = (this.target as Pawn)) != null && pawn.Dead) || base.ShouldRemove;
			}
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00082F18 File Offset: 0x00081118
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x04001182 RID: 4482
		public Thing target;
	}
}

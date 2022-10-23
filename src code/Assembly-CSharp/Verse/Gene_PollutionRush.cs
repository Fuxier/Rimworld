using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002B2 RID: 690
	public class Gene_PollutionRush : Gene
	{
		// Token: 0x060013C1 RID: 5057 RVA: 0x0007827C File Offset: 0x0007647C
		public override void PostRemove()
		{
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PollutionStimulus, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			base.PostRemove();
		}
	}
}

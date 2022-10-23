using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200033F RID: 831
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x0600162F RID: 5679 RVA: 0x000830EF File Offset: 0x000812EF
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}

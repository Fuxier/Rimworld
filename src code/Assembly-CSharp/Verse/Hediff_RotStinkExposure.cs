using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000350 RID: 848
	public class Hediff_RotStinkExposure : HediffWithComps
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00085A2E File Offset: 0x00083C2E
		public override bool Visible
		{
			get
			{
				return this.CurStage.becomeVisible && !this.pawn.health.hediffSet.HasHediff(HediffDefOf.LungRot, false);
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060016C6 RID: 5830 RVA: 0x00085A5D File Offset: 0x00083C5D
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity <= 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent("F0");
			}
		}
	}
}

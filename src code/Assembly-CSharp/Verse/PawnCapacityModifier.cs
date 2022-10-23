using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000FC RID: 252
	public class PawnCapacityModifier
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00025375 File Offset: 0x00023575
		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999f || (this.setMaxCurveOverride != null && this.setMaxCurveEvaluateStat != null);
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00025399 File Offset: 0x00023599
		public float EvaluateSetMax(Pawn pawn)
		{
			if (this.setMaxCurveOverride == null || this.setMaxCurveEvaluateStat == null)
			{
				return this.setMax;
			}
			return this.setMaxCurveOverride.Evaluate(pawn.GetStatValue(this.setMaxCurveEvaluateStat, true, -1));
		}

		// Token: 0x04000604 RID: 1540
		public PawnCapacityDef capacity;

		// Token: 0x04000605 RID: 1541
		public float offset;

		// Token: 0x04000606 RID: 1542
		public float setMax = 999f;

		// Token: 0x04000607 RID: 1543
		public float postFactor = 1f;

		// Token: 0x04000608 RID: 1544
		public SimpleCurve setMaxCurveOverride;

		// Token: 0x04000609 RID: 1545
		public StatDef setMaxCurveEvaluateStat;
	}
}

using System;

namespace Verse
{
	// Token: 0x0200031E RID: 798
	public class HediffComp_SeverityFromEntropy : HediffComp
	{
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x00080A5E File Offset: 0x0007EC5E
		private float EntropyAmount
		{
			get
			{
				if (base.Pawn.psychicEntropy != null)
				{
					return base.Pawn.psychicEntropy.EntropyRelativeValue;
				}
				return 0f;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x00080A83 File Offset: 0x0007EC83
		public override bool CompShouldRemove
		{
			get
			{
				return this.EntropyAmount < float.Epsilon;
			}
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x00080A92 File Offset: 0x0007EC92
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.parent.Severity = this.EntropyAmount;
		}
	}
}

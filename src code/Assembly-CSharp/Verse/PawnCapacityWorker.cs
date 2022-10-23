using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000117 RID: 279
	public class PawnCapacityWorker
	{
		// Token: 0x06000749 RID: 1865 RVA: 0x00025F42 File Offset: 0x00024142
		public virtual float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return 1f;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool CanHaveCapacity(BodyDef body)
		{
			return true;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00025F4C File Offset: 0x0002414C
		protected float CalculateCapacityAndRecord(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			float level = diffSet.pawn.health.capacities.GetLevel(capacity);
			if (impactors != null && level != 1f)
			{
				impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
				{
					capacity = capacity
				});
			}
			return level;
		}
	}
}

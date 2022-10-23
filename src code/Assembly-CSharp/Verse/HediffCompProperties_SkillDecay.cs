using System;

namespace Verse
{
	// Token: 0x02000327 RID: 807
	public class HediffCompProperties_SkillDecay : HediffCompProperties
	{
		// Token: 0x0600159E RID: 5534 RVA: 0x00080DFD File Offset: 0x0007EFFD
		public HediffCompProperties_SkillDecay()
		{
			this.compClass = typeof(HediffComp_SkillDecay);
		}

		// Token: 0x04001151 RID: 4433
		public SimpleCurve decayPerDayPercentageLevelCurve;
	}
}

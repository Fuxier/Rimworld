using System;

namespace Verse
{
	// Token: 0x02000335 RID: 821
	public class HediffCompProperties_SeverityPerDay : HediffCompProperties
	{
		// Token: 0x060015EC RID: 5612 RVA: 0x0008208E File Offset: 0x0008028E
		public HediffCompProperties_SeverityPerDay()
		{
			this.compClass = typeof(HediffComp_SeverityPerDay);
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x000820BC File Offset: 0x000802BC
		public float CalculateSeverityPerDay()
		{
			float num = this.severityPerDay + this.severityPerDayRange.RandomInRange;
			if (Rand.Chance(this.reverseSeverityChangeChance))
			{
				num *= -1f;
			}
			return num;
		}

		// Token: 0x04001175 RID: 4469
		public float severityPerDay;

		// Token: 0x04001176 RID: 4470
		public bool showDaysToRecover;

		// Token: 0x04001177 RID: 4471
		public bool showHoursToRecover;

		// Token: 0x04001178 RID: 4472
		public float mechanitorFactor = 1f;

		// Token: 0x04001179 RID: 4473
		public float reverseSeverityChangeChance;

		// Token: 0x0400117A RID: 4474
		public FloatRange severityPerDayRange = FloatRange.Zero;

		// Token: 0x0400117B RID: 4475
		public float minAge;
	}
}

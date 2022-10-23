using System;

namespace Verse
{
	// Token: 0x020002F7 RID: 759
	public static class HediffGrowthModeUtility
	{
		// Token: 0x06001506 RID: 5382 RVA: 0x0007EED4 File Offset: 0x0007D0D4
		public static string GetLabel(this HediffGrowthMode m)
		{
			switch (m)
			{
			case HediffGrowthMode.Growing:
				return "HediffGrowthMode_Growing".Translate();
			case HediffGrowthMode.Stable:
				return "HediffGrowthMode_Stable".Translate();
			case HediffGrowthMode.Remission:
				return "HediffGrowthMode_Remission".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}

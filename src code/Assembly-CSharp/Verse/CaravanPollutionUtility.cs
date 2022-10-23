using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200022F RID: 559
	public static class CaravanPollutionUtility
	{
		// Token: 0x06000FCB RID: 4043 RVA: 0x0005BD88 File Offset: 0x00059F88
		public static void CheckDamageFromPollution(Caravan caravan)
		{
			if (Find.TickManager.TicksGame % 3451 != 0)
			{
				return;
			}
			if (Find.WorldGrid[caravan.Tile].PollutionLevel() < PollutionLevel.Moderate)
			{
				return;
			}
			float extraFactor = CaravanPollutionUtility.ToxicDamagePollutionFactor(caravan.Tile);
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				GameCondition_ToxicFallout.DoPawnToxicDamage(pawnsListForReading[i], false, extraFactor);
			}
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0005BDF4 File Offset: 0x00059FF4
		public static float ToxicDamagePollutionFactor(int tile)
		{
			PollutionLevel pollutionLevel = Find.WorldGrid[tile].PollutionLevel();
			if (pollutionLevel == PollutionLevel.Moderate)
			{
				return 0.5f;
			}
			return 1f;
		}

		// Token: 0x04000E0A RID: 3594
		private const float ModeratePollutionToxicDamageFactor = 0.5f;
	}
}

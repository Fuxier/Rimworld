using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000556 RID: 1366
	public static class EloUtility
	{
		// Token: 0x0600299C RID: 10652 RVA: 0x0010A166 File Offset: 0x00108366
		public static void Update(ref float teamA, ref float teamB, float expectedA, float scoreA, float kfactor = 32f)
		{
			teamA += kfactor * (scoreA - expectedA);
			teamB += kfactor * (expectedA - scoreA);
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x0010A180 File Offset: 0x00108380
		public static float CalculateExpectation(float teamA, float teamB)
		{
			float num = Mathf.Pow(10f, teamA / 400f) + Mathf.Pow(10f, teamB / 400f);
			return Mathf.Pow(10f, teamA / 400f) / num;
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x0010A1C4 File Offset: 0x001083C4
		public static float CalculateLinearScore(float teamRating, float referenceRating, float referenceScore)
		{
			return referenceScore * Mathf.Pow(10f, (teamRating - referenceRating) / 400f);
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x0010A1DB File Offset: 0x001083DB
		public static float CalculateRating(float teamScore, float referenceRating, float referenceScore)
		{
			return referenceRating + Mathf.Log(teamScore / referenceScore, 10f) * 400f;
		}

		// Token: 0x04001B7A RID: 7034
		private const float TenFactorRating = 400f;
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200043B RID: 1083
	public static class DebugActionsUtility
	{
		// Token: 0x06002028 RID: 8232 RVA: 0x000C0D34 File Offset: 0x000BEF34
		public static void DustPuffFrom(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.Drawer.Notify_DebugAffected();
			}
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x000C0D56 File Offset: 0x000BEF56
		public static IEnumerable<float> PointsOptions(bool extended)
		{
			if (!extended)
			{
				yield return 35f;
				yield return 70f;
				yield return 100f;
				yield return 150f;
				yield return 200f;
				yield return 350f;
				yield return 500f;
				yield return 700f;
				yield return 1000f;
				yield return 1200f;
				yield return 1500f;
				yield return 2000f;
				yield return 3000f;
				yield return 4000f;
				yield return 5000f;
			}
			else
			{
				for (int i = 20; i < 100; i += 10)
				{
					yield return (float)i;
				}
				for (int i = 100; i < 500; i += 25)
				{
					yield return (float)i;
				}
				for (int i = 500; i < 1500; i += 50)
				{
					yield return (float)i;
				}
				for (int i = 1500; i <= 5000; i += 100)
				{
					yield return (float)i;
				}
			}
			yield return 6000f;
			yield return 7000f;
			yield return 8000f;
			yield return 9000f;
			yield return 10000f;
			yield break;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x000C0D66 File Offset: 0x000BEF66
		public static IEnumerable<int> PopulationOptions()
		{
			int num;
			for (int i = 1; i <= 20; i = num + 1)
			{
				yield return i;
				num = i;
			}
			for (int i = 30; i <= 50; i += 10)
			{
				yield return i;
			}
			yield break;
		}
	}
}

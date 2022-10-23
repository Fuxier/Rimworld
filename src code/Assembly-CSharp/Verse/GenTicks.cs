using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056E RID: 1390
	public static class GenTicks
	{
		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002ACC RID: 10956 RVA: 0x00111570 File Offset: 0x0010F770
		public static int TicksAbs
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null && Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					return GenTicks.ConfiguredTicksAbsAtGameStart;
				}
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksAbs;
				}
				return 0;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002ACD RID: 10957 RVA: 0x001115BD File Offset: 0x0010F7BD
		public static int TicksGame
		{
			get
			{
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksGame;
				}
				return 0;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002ACE RID: 10958 RVA: 0x001115DC File Offset: 0x0010F7DC
		public static int ConfiguredTicksAbsAtGameStart
		{
			get
			{
				GameInitData gameInitData = Find.GameInitData;
				ConfiguredTicksAbsAtGameStartCache ticksAbsCache = Find.World.ticksAbsCache;
				int result;
				if (ticksAbsCache.TryGetCachedValue(gameInitData, out result))
				{
					return result;
				}
				Vector2 vector;
				if (gameInitData.startingTile >= 0)
				{
					vector = Find.WorldGrid.LongLatOf(gameInitData.startingTile);
				}
				else
				{
					vector = Vector2.zero;
				}
				Twelfth twelfth;
				if (gameInitData.startingSeason != Season.Undefined)
				{
					twelfth = gameInitData.startingSeason.GetFirstTwelfth(vector.y);
				}
				else if (gameInitData.startingTile >= 0)
				{
					twelfth = TwelfthUtility.FindStartingWarmTwelfth(gameInitData.startingTile);
				}
				else
				{
					twelfth = Season.Summer.GetFirstTwelfth(0f);
				}
				int num = (24 - GenDate.TimeZoneAt(vector.x)) % 24;
				int num2 = 300000 * (int)twelfth + 2500 * (6 + num);
				ticksAbsCache.Cache(num2, gameInitData);
				return num2;
			}
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x0011169E File Offset: 0x0010F89E
		public static float TicksToSeconds(this int numTicks)
		{
			return (float)numTicks / 60f;
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x001116A8 File Offset: 0x0010F8A8
		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x001116B8 File Offset: 0x0010F8B8
		public static string ToStringSecondsFromTicks(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x001116F8 File Offset: 0x0010F8F8
		public static string ToStringSecondsFromTicks(this int numTicks, string format)
		{
			return numTicks.TicksToSeconds().ToString(format) + " " + "SecondsLower".Translate();
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x00111734 File Offset: 0x0010F934
		public static string ToStringTicksFromSeconds(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}

		// Token: 0x04001BF7 RID: 7159
		public const int TicksPerRealSecond = 60;

		// Token: 0x04001BF8 RID: 7160
		public const int TickRareInterval = 250;

		// Token: 0x04001BF9 RID: 7161
		public const int TickLongInterval = 2000;
	}
}

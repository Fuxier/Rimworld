using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000573 RID: 1395
	public static class RealTime
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x00112A2A File Offset: 0x00110C2A
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x00112A31 File Offset: 0x00110C31
		public static float UnpausedRealTime
		{
			get
			{
				return RealTime.unpausedTime;
			}
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x00112A38 File Offset: 0x00110C38
		public static void Update()
		{
			RealTime.frameCount = Time.frameCount;
			RealTime.deltaTime = Time.deltaTime;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			RealTime.realDeltaTime = realtimeSinceStartup - RealTime.lastRealTime;
			RealTime.lastRealTime = realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				RealTime.moteList.MoteListUpdate();
				if (Current.Game != null)
				{
					RealTime.unpausedTime += RealTime.deltaTime * Find.TickManager.TickRateMultiplier;
				}
			}
			else
			{
				RealTime.moteList.Clear();
			}
			if (DebugSettings.lowFPS && Time.deltaTime < 100f)
			{
				Thread.Sleep((int)(100f - Time.deltaTime));
			}
		}

		// Token: 0x04001C04 RID: 7172
		public static float deltaTime;

		// Token: 0x04001C05 RID: 7173
		public static float realDeltaTime;

		// Token: 0x04001C06 RID: 7174
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x04001C07 RID: 7175
		public static int frameCount;

		// Token: 0x04001C08 RID: 7176
		private static float unpausedTime;

		// Token: 0x04001C09 RID: 7177
		private static float lastRealTime = 0f;
	}
}

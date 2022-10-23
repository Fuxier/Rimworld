using System;
using System.Diagnostics;

namespace Verse
{
	// Token: 0x02000597 RID: 1431
	public static class ProfilerThreadCheck
	{
		// Token: 0x06002BA7 RID: 11175 RVA: 0x00115224 File Offset: 0x00113424
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x00115224 File Offset: 0x00113424
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void EndSample()
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}
	}
}

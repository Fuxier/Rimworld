using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000544 RID: 1348
	public static class ProfilerPairValidation
	{
		// Token: 0x0600295C RID: 10588 RVA: 0x00108B4E File Offset: 0x00106D4E
		public static void BeginSample(string token)
		{
			ProfilerPairValidation.profilerSignatures.Push(new StackTrace(1, true));
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x00108B64 File Offset: 0x00106D64
		public static void EndSample()
		{
			StackTrace stackTrace = ProfilerPairValidation.profilerSignatures.Pop();
			StackTrace stackTrace2 = new StackTrace(1, true);
			if (stackTrace2.FrameCount != stackTrace.FrameCount)
			{
				Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()));
				return;
			}
			for (int i = 0; i < stackTrace2.FrameCount; i++)
			{
				if (stackTrace2.GetFrame(i).GetMethod() != stackTrace.GetFrame(i).GetMethod() && (!(stackTrace.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck)) || !(stackTrace2.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck))) && (!(stackTrace.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerBeginSample", BindingFlags.Instance | BindingFlags.NonPublic)) || !(stackTrace2.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerEndSample", BindingFlags.Instance | BindingFlags.NonPublic))))
				{
					Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()));
					return;
				}
			}
		}

		// Token: 0x04001B56 RID: 6998
		public static Stack<StackTrace> profilerSignatures = new Stack<StackTrace>();
	}
}

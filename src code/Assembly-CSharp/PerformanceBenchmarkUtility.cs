using System;
using UnityEngine;
using Verse;

// Token: 0x02000013 RID: 19
public static class PerformanceBenchmarkUtility
{
	// Token: 0x0600006B RID: 107 RVA: 0x0000500F File Offset: 0x0000320F
	public static void StartBenchmark()
	{
		PerformanceBenchmarkUtility.startBenchmarkTime = Time.realtimeSinceStartup;
		PerformanceBenchmarkUtility.startBenchmarkTicks = Find.TickManager.TicksGame;
		PerformanceBenchmarkUtility.startBenchmarkFrames = Time.frameCount;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00005034 File Offset: 0x00003234
	public static void CheckBenchmark()
	{
		if (PerformanceBenchmarkUtility.startBenchmarkTime > 0f && PerformanceBenchmarkUtility.startBenchmarkTime + 30f < Time.realtimeSinceStartup)
		{
			float num = Time.realtimeSinceStartup - PerformanceBenchmarkUtility.startBenchmarkTime;
			int num2 = Time.frameCount - PerformanceBenchmarkUtility.startBenchmarkFrames;
			int num3 = Find.TickManager.TicksGame - PerformanceBenchmarkUtility.startBenchmarkTicks;
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(string.Concat(new string[]
			{
				string.Format("Frames per second: {0}\n", (float)num2 / num),
				string.Format("Ticks per second: {0}\n", (float)num3 / num),
				string.Format("Ticks + Frames per second: {0}\n", (float)(num3 + num2) / num),
				string.Format("Ticks / Frame: {0}\n\n", (float)num3 / (float)num2),
				"----RAW----\n",
				string.Format("Time elapsed: {0}s\n", num),
				string.Format("Frames: {0}\n", num2),
				string.Format("Game Ticks: {0}\n\n", num3),
				string.Format("Note: Each frame the game tries to do <tickrate> ticks or as many ticks as it can before {0}ms elapses. This means that sometimes tickrate, not framerate will increase as performance improves if the game is consistently not completing <tickrate> ticks per frame. Framerate can increase if performance improves while the game is consistently completing <tickrate> ticks per frame.", Mathf.RoundToInt(45.454544f))
			}), delegate
			{
			}, false, null, WindowLayer.Dialog);
			PerformanceBenchmarkUtility.startBenchmarkTime = -1f;
			Find.WindowStack.Add(window);
		}
	}

	// Token: 0x04000028 RID: 40
	public const float BenchmarkSeconds = 30f;

	// Token: 0x04000029 RID: 41
	public static float startBenchmarkTime = -1f;

	// Token: 0x0400002A RID: 42
	public static int startBenchmarkFrames = -1;

	// Token: 0x0400002B RID: 43
	public static int startBenchmarkTicks = -1;
}

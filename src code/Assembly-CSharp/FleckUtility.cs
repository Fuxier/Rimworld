using System;
using System.Collections.Generic;
using Verse;

// Token: 0x02000015 RID: 21
public static class FleckUtility
{
	// Token: 0x0600006F RID: 111 RVA: 0x000051CB File Offset: 0x000033CB
	public static FleckParallelizationInfo GetParallelizationInfo()
	{
		if (FleckUtility.parallelizationInfosPool.Count == 0)
		{
			return new FleckParallelizationInfo();
		}
		return FleckUtility.parallelizationInfosPool.Pop<FleckParallelizationInfo>();
	}

	// Token: 0x06000070 RID: 112 RVA: 0x000051E9 File Offset: 0x000033E9
	public static void ReturnParallelizationInfo(FleckParallelizationInfo info)
	{
		info.doneEvent.Reset();
		info.drawBatch.Flush(false);
		FleckUtility.parallelizationInfosPool.Add(info);
	}

	// Token: 0x04000031 RID: 49
	private static List<FleckParallelizationInfo> parallelizationInfosPool = new List<FleckParallelizationInfo>();
}

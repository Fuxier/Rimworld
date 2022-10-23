using System;
using System.Threading;
using Verse;

// Token: 0x02000014 RID: 20
public class FleckParallelizationInfo
{
	// Token: 0x0400002C RID: 44
	public int startIndex;

	// Token: 0x0400002D RID: 45
	public int endIndex;

	// Token: 0x0400002E RID: 46
	public object data;

	// Token: 0x0400002F RID: 47
	public DrawBatch drawBatch = new DrawBatch();

	// Token: 0x04000030 RID: 48
	public ManualResetEvent doneEvent = new ManualResetEvent(false);
}

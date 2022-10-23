using System;

namespace Verse
{
	// Token: 0x02000541 RID: 1345
	public class DeepProfilerScope : IDisposable
	{
		// Token: 0x0600294C RID: 10572 RVA: 0x001082FD File Offset: 0x001064FD
		public DeepProfilerScope(string label = null)
		{
			DeepProfiler.Start(label);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x0010830B File Offset: 0x0010650B
		void IDisposable.Dispose()
		{
			DeepProfiler.End();
		}
	}
}

using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x02000587 RID: 1415
	public static class PerfLogger
	{
		// Token: 0x06002B2D RID: 11053 RVA: 0x001141FD File Offset: 0x001123FD
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x00114219 File Offset: 0x00112419
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog != null) ? PerfLogger.currentLog.ToString() : "");
			PerfLogger.Reset();
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x00114240 File Offset: 0x00112440
		public static void Record(string label)
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (PerfLogger.currentLog == null)
			{
				PerfLogger.currentLog = new StringBuilder();
			}
			PerfLogger.currentLog.AppendLine(string.Format("{0}: {3}{1} ({2})", new object[]
			{
				(timestamp - PerfLogger.start) * 1000L / Stopwatch.Frequency,
				label,
				(timestamp - PerfLogger.current) * 1000L / Stopwatch.Frequency,
				new string(' ', PerfLogger.indent * 2)
			}));
			PerfLogger.current = timestamp;
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x001142D2 File Offset: 0x001124D2
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x001142E0 File Offset: 0x001124E0
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x001142EE File Offset: 0x001124EE
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}

		// Token: 0x04001C5B RID: 7259
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x04001C5C RID: 7260
		private static long start;

		// Token: 0x04001C5D RID: 7261
		private static long current;

		// Token: 0x04001C5E RID: 7262
		private static int indent;
	}
}

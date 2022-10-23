using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	// Token: 0x02000588 RID: 1416
	public static class PerfTest
	{
		// Token: 0x06002B34 RID: 11060 RVA: 0x00114310 File Offset: 0x00112510
		public static string TestStandardMilliseconds()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			List<int> list = new List<int>();
			for (int i = 0; i < 100000; i++)
			{
				list.Add(i);
			}
			List<double> list2 = new List<double>();
			while (list.Count > 0)
			{
				int num = list[0];
				list.RemoveAt(0);
				list2.Add(Math.Sqrt((double)num));
			}
			double num2 = 0.0;
			for (int j = 0; j < list2.Count; j++)
			{
				num2 += list2[j];
			}
			stopwatch.Stop();
			return string.Concat(new object[]
			{
				"Elapsed: ",
				stopwatch.Elapsed,
				"\nMilliseconds: ",
				stopwatch.ElapsedMilliseconds,
				"\nSum: ",
				num2
			});
		}
	}
}

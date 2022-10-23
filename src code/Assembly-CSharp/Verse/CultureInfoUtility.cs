using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x02000046 RID: 70
	public static class CultureInfoUtility
	{
		// Token: 0x060003AA RID: 938 RVA: 0x000144F1 File Offset: 0x000126F1
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}

		// Token: 0x040000F3 RID: 243
		private const string EnglishCulture = "en-US";
	}
}

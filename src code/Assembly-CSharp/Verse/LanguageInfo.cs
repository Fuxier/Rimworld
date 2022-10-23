using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000185 RID: 389
	public class LanguageInfo
	{
		// Token: 0x04000A7A RID: 2682
		public string friendlyNameNative;

		// Token: 0x04000A7B RID: 2683
		public string friendlyNameEnglish;

		// Token: 0x04000A7C RID: 2684
		public bool canBeTiny = true;

		// Token: 0x04000A7D RID: 2685
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x04000A7E RID: 2686
		public Type languageWorkerClass = typeof(LanguageWorker_Default);

		// Token: 0x04000A7F RID: 2687
		public int? totalNumCaseCount;
	}
}

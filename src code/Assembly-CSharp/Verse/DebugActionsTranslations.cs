using System;

namespace Verse
{
	// Token: 0x0200043A RID: 1082
	public static class DebugActionsTranslations
	{
		// Token: 0x06002026 RID: 8230 RVA: 0x000C0D26 File Offset: 0x000BEF26
		[DebugAction("Translation", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Entry)]
		private static void WriteBackstoryTranslationFile()
		{
			LanguageDataWriter.WriteBackstoryFile();
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x000C0D2D File Offset: 0x000BEF2D
		[DebugAction("Translation", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Entry)]
		private static void SaveTranslationReport()
		{
			LanguageReportGenerator.SaveTranslationReport();
		}
	}
}
